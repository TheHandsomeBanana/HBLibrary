using System.Collections;
using System.Diagnostics;

namespace HBLibrary.DataStructures;

// Buckets and Lock stripping: using an array of buckets, where each bucket contains a linked list
// of nodes for the key-value pairs that hash to that bucket. To allow for concurrent access,
// it employs a technique called lock stripping, which involves locking at a finer granularity.
// Instead of locking the entire dictionary for each operation, it only locks the specific bucket that the operation affects.
// This means that operations on different buckets can proceed in parallel.

// Use concurrency level (the number of threads that are expected to access the dictionary concurrently)

// Use dynamic resizing by temporarily locking the entire collection to rehash the items into a new, larger array of buckets.

// Use memory barriers to ensure proper ordering of reads and writes across different threads. This is critical to prevent
// the compiler or processor from reordering instructions in a way that could lead to inconsistent views of the data

// Implement custom Enumerator for this collection

/// <summary>
/// A concurrent hash-set based on the implementation of <br></br>
/// <a href="https://github.com/microsoft/referencesource/blob/master/mscorlib/system/collections/Concurrent/ConcurrentDictionary.cs"></a>
/// </summary>
/// <typeparam name="T"></typeparam>
[DebuggerDisplay("Count = {Count}")]
public class ConcurrentHashSet<T> : IEnumerable, IEnumerable<T>, ICollection<T>, IReadOnlyCollection<T> {
    private const int DEFAULT_CAPACITY = 31;
    private const int MAX_LOCK_NUMBER = 1024;
    private readonly IEqualityComparer<T> comparer;
    public IEqualityComparer<T> Comparer => comparer;

    private readonly bool growLockArray; // Whether to dynamically increase the size of the striped lock
    private volatile Tables tables; // Internal tables of the hashset
    private int budget; // The maximum number of elements per lock before a resize operation is triggered

    private static int DefaultConcurrencyLevel => Environment.ProcessorCount;

    public int Count {
        get {
            int count = 0;
            int acquiredLocks = 0;
            try {
                AcquireAllLocks(ref acquiredLocks);
                int[] countPerLocks = tables.CountPerLock;
                for (int i = 0; i < countPerLocks.Length; i++)
                    count += countPerLocks[i];
            }
            finally {
                ReleaseLocks(0, acquiredLocks);
            }

            return count;
        }
    }

    public bool IsEmpty {
        get {
            if (!AreAllBucketsEmpty())
                return false;

            int acquiredLocks = 0;

            try {
                AcquireAllLocks(ref acquiredLocks);
                return AreAllBucketsEmpty();
            }
            finally {
                ReleaseLocks(0, acquiredLocks);
            }
        }
    }

    public ConcurrentHashSet() : this(DefaultConcurrencyLevel, DEFAULT_CAPACITY, true, null) { }
    public ConcurrentHashSet(int concurrencyLevel, int capacity) : this(concurrencyLevel, capacity, false, null) { }
    public ConcurrentHashSet(IEnumerable<T> collection) : this(collection, null) { }
    public ConcurrentHashSet(IEqualityComparer<T>? comparer) : this(DefaultConcurrencyLevel, DEFAULT_CAPACITY, true, comparer) { }
    public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer) : this(comparer) {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        InitializeFromCollection(collection);
    }

    public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T>? comparer) : this(concurrencyLevel, capacity, false, comparer) { }

    private ConcurrentHashSet(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<T>? comparer) {
        if (concurrencyLevel < 1)
            throw new ArgumentOutOfRangeException(nameof(concurrencyLevel));

        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        // The capacity should be at least as large as the concurrency level. Otherwise, we would have locks that don't guard
        // any buckets.
        if (capacity < concurrencyLevel)
            capacity = concurrencyLevel;

        object[] locks = new object[concurrencyLevel];
        for (int i = 0; i < locks.Length; i++) {
            locks[i] = new object();
        }

        int[] countPerLock = new int[locks.Length];
        Node[] buckets = new Node[capacity];
        tables = new Tables(buckets, locks, countPerLock);

        this.growLockArray = growLockArray;
        budget = buckets.Length / locks.Length;
        this.comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public bool Add(T item) => AddInternal(item, comparer.GetHashCode(item!), true);
    public void Clear() {
        int locksAcquired = 0;
        try {
            AcquireAllLocks(ref locksAcquired);

            if (AreAllBucketsEmpty())
                return;

            Tables tables = this.tables;
            Tables newTables = new Tables(new Node[DEFAULT_CAPACITY], tables.Locks, new int[tables.CountPerLock.Length]);
            this.tables = newTables;
            budget = Math.Max(1, newTables.Buckets.Length / newTables.Locks.Length);
        }
        finally {
            ReleaseLocks(0, locksAcquired);
        }
    }

    public bool Contains(T item) => TryGetValue(item, out _);

    public bool TryGetValue(T equalValue, out T? actualValue) {
        int hashcode = comparer.GetHashCode(equalValue!);

        // We must capture the _buckets field in a local variable. It is set to a new table on each table resize.
        Tables tables = this.tables;

        int bucketNo = GetBucket(hashcode, tables.Buckets.Length);

        // We can get away w/out a lock here.
        // The Volatile.Read ensures that the load of the fields of 'n' doesn't move before the load from buckets[i].
        Node? current = Volatile.Read(ref tables.Buckets[bucketNo]);

        while (current != null) {
            if (hashcode == current.Hashcode && comparer.Equals(current.Item, equalValue)) {
                actualValue = current.Item;
                return true;
            }

            current = current.Next;
        }

        actualValue = default;
        return false;
    }

    public bool TryRemove(T item) {
        var hashcode = comparer.GetHashCode(item!);
        while (true) {
            var tables = this.tables;

            GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

            lock (tables.Locks[lockNo]) {
                // If the table just got resized, we may not be holding the right lock, and must retry.
                // This should be a rare occurrence.
                if (tables != this.tables)
                    continue;

                Node? prev = null;
                for (Node? current = tables.Buckets[bucketNo]; current != null; current = current.Next) {
                    Debug.Assert(prev == null && current == tables.Buckets[bucketNo] || prev!.Next == current);

                    if (hashcode == current.Hashcode && comparer.Equals(current.Item, item)) {
                        if (prev == null)
                            Volatile.Write(ref tables!.Buckets[bucketNo]!, current.Next);
                        else
                            prev.Next = current.Next;

                        tables.CountPerLock[lockNo]--;
                        return true;
                    }
                    prev = current;
                }
            }

            return false;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => new HashSetEnumerator(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new HashSetEnumerator(this);
    public HashSetEnumerator GetEnumerator() => new HashSetEnumerator(this);

    void ICollection<T>.Add(T item) => Add(item);
    bool ICollection<T>.IsReadOnly => false;

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        int locksAcquired = 0;
        try {
            AcquireAllLocks(ref locksAcquired);

            int count = 0;

            int[] countPerLock = tables.CountPerLock;
            for (var i = 0; i < countPerLock.Length && count >= 0; i++) {
                count += countPerLock[i];
            }

            if (array.Length - count < arrayIndex || count < 0) // "count" itself or "count + arrayIndex" can overflow
                throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the set is greater than the available space from index to the end of the destination array.");


            CopyToItems(array, arrayIndex);
        }
        finally {
            ReleaseLocks(0, locksAcquired);
        }
    }

    bool ICollection<T>.Remove(T item) => TryRemove(item);

    private void InitializeFromCollection(IEnumerable<T> collection) {
        foreach (T item in collection)
            AddInternal(item, comparer.GetHashCode(item!), false);

        if (budget == 0) {
            Tables tables = this.tables;
            budget = tables.Buckets.Length / tables.Locks.Length;
        }
    }

    private bool AddInternal(T item, int hashcode, bool acquireLock) {
        while (true) {
            Tables tables = this.tables;
            GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

            bool resizeDesired = false;
            bool lockTaken = false;
            try {
                if (acquireLock)
                    Monitor.Enter(tables.Locks[lockNo], ref lockTaken);

                if (tables != this.tables)
                    continue;

                Node? prev = null;
                for (Node? current = tables.Buckets[bucketNo]; current != null; current = current.Next) {
                    Debug.Assert(prev == null && current == tables.Buckets[bucketNo] || prev!.Next == current);
                    if (hashcode == current.Hashcode && comparer.Equals(current.Item, item))
                        return false;

                    prev = current;
                }

                Volatile.Write(ref tables.Buckets[bucketNo], new Node(item, hashcode, tables.Buckets[bucketNo]));

                checked {
                    tables.CountPerLock[lockNo]++;
                }

                if (tables.CountPerLock[lockNo] > budget)
                    resizeDesired = true;
            }
            finally {
                if (lockTaken)
                    Monitor.Exit(tables.Locks[lockNo]);
            }

            if (resizeDesired)
                GrowTable(tables);

            return true;
        }
    }

    private static int GetBucket(int hashcode, int bucketCount) {
        int bucketNo = (hashcode & 0x7fffffff) % bucketCount;
        Debug.Assert(bucketNo >= 0 && bucketNo < bucketCount);
        return bucketNo;
    }

    private static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount) {
        bucketNo = (hashcode & 0x7fffffff) % bucketCount;
        lockNo = bucketNo % lockCount;

        Debug.Assert(bucketNo >= 0 && bucketNo < bucketCount);
        Debug.Assert(lockNo >= 0 && lockNo < lockCount);
    }

    private bool AreAllBucketsEmpty() {
        var countPerLock = tables.CountPerLock;
        for (var i = 0; i < countPerLock.Length; i++) {
            if (countPerLock[i] != 0)
                return false;
        }

        return true;
    }

    private void GrowTable(Tables tables) {
        const int maxArrayLength = 0x7FEFFFFF;
        int locksAcquired = 0;
        try {
            AcquireLocks(0, 1, ref locksAcquired);

            if (this.tables != tables)
                // We assume that since the table reference is different, it was already resized (or the budget
                // was adjusted). If we ever decide to do table shrinking, or replace the table for other reasons,
                // we will have to revisit this logic.
                return;

            // Compute the (approx.) total size. Use an Int64 accumulation variable to avoid an overflow.
            long approxCount = 0;
            for (int i = 0; i < tables.CountPerLock.Length; i++)
                approxCount += tables.CountPerLock[i];

            // If the bucket array is too empty, double the budget instead of resizing the table
            if (approxCount < tables.Buckets.Length / 4) {
                budget = 2 * budget;
                if (budget < 0)
                    budget = int.MaxValue;

                return;
            }

            // Compute the new table size. We find the smallest integer larger than twice the previous table size, and not divisible by
            // 2,3,5 or 7. We can consider a different table-sizing policy in the future.
            int newLength = 0;
            bool maximizeTableSize = false;
            try {
                checked {
                    // Double the size of the buckets table and add one, so that we have an odd integer.
                    newLength = tables.Buckets.Length * 2 + 1;

                    // Now, we only need to check odd integers, and find the first that is not divisible
                    // by 3, 5 or 7.
                    while (newLength % 3 == 0 || newLength % 5 == 0 || newLength % 7 == 0)
                        newLength += 2;

                    Debug.Assert(newLength % 2 != 0);

                    if (newLength > maxArrayLength)
                        maximizeTableSize = true;
                }
            }
            catch (OverflowException) {
                maximizeTableSize = true;
            }

            if (maximizeTableSize) {
                newLength = maxArrayLength;
                budget = int.MaxValue;
            }

            AcquireLocks(1, tables.Locks.Length, ref locksAcquired);
            object[] newLocks = tables.Locks;

            // Add more locks
            if (growLockArray && tables.Locks.Length < MAX_LOCK_NUMBER) {
                newLocks = new object[tables.Locks.Length * 2];
                Array.Copy(tables.Locks, newLocks, tables.Locks.Length);
                for (int i = tables.Locks.Length; i < newLocks.Length; i++)
                    newLocks[i] = new object();
            }

            Node[] newBuckets = new Node[newLength];
            int[] newCountPerLock = new int[newLocks.Length];

            for (int i = 0; i < tables.Buckets.Length; i++) {
                Node? current = tables.Buckets[i];
                while (current != null) {
                    Node? next = current.Next;
                    GetBucketAndLockNo(current.Hashcode, out int newBucketNo, out int newLockNo, newBuckets.Length, newLocks.Length);
                    newBuckets[newBucketNo] = new Node(current.Item, current.Hashcode, newBuckets[newBucketNo]);

                    checked {
                        newCountPerLock[newLockNo]++;
                    }

                    current = next;
                }
            }

            budget = Math.Max(1, newBuckets.Length / newLocks.Length);
            tables = new Tables(newBuckets, newLocks, newCountPerLock);
        }
        finally {
            ReleaseLocks(0, locksAcquired);
        }
    }

    private void AcquireAllLocks(ref int locksAcquired) {
        // First, acquire lock 0
        AcquireLocks(0, 1, ref locksAcquired);
        // Now that we have lock 0, the m_locks array will not change (i.e., grow),
        // and so we can safely read m_locks.Length.
        AcquireLocks(1, tables.Locks.Length, ref locksAcquired);
        Debug.Assert(locksAcquired == tables.Locks.Length);
    }

    private void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired) {
        Debug.Assert(fromInclusive <= toExclusive);

        object[] locks = tables.Locks;
        for (int i = fromInclusive; i < toExclusive; i++) {
            bool lockTaken = false;
            try {
                Monitor.Enter(locks[i], ref lockTaken);
            }
            finally {
                if (lockTaken) {
                    locksAcquired++;
                }
            }
        }
    }

    private void ReleaseLocks(int fromInclusive, int toExclusive) {
        Debug.Assert(fromInclusive <= toExclusive);

        for (int i = fromInclusive; i < toExclusive; i++)
            Monitor.Exit(tables.Locks[i]);
    }

    private void CopyToItems(T[] array, int index) {
        Node[] buckets = tables.Buckets;
        for (int i = 0; i < buckets.Length; i++) {
            for (Node? current = buckets[i]; current != null; current = current.Next) {
                array[index] = current.Item;
                index++; // this should never flow, CopyToItems is only called when there's no overflow risk
            }
        }
    }

    /// <summary>
    /// Found this in <a href="https://github.com/i3arnon/ConcurrentHashSet/blob/main/src/ConcurrentHashSet/ConcurrentHashSet.cs"></a>
    /// </summary>
    public struct HashSetEnumerator : IEnumerator<T> {
        private readonly ConcurrentHashSet<T> hashSet;

        private Node?[]? buckets;
        private Node? node;
        private int i;
        private int state;

        private const int StateUninitialized = 0;
        private const int StateOuterloop = 1;
        private const int StateInnerLoop = 2;
        private const int StateDone = 3;

        /// <summary>
        /// Constructs an enumerator for <see cref="ConcurrentHashSet{T}" />.
        /// </summary>
        public HashSetEnumerator(ConcurrentHashSet<T> hashSet) {
            this.hashSet = hashSet;
            buckets = null;
            node = null;
            Current = default!;
            i = -1;
            state = StateUninitialized;
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        public T Current { get; private set; }

        readonly object? IEnumerator.Current => Current;

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset() {
            buckets = null;
            node = null;
            Current = default!;
            i = -1;
            state = StateUninitialized;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public readonly void Dispose() { }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext() {
            switch (state) {
                case StateUninitialized:
                    this.buckets = hashSet.tables.Buckets;
                    this.i = -1;
                    goto case StateOuterloop;

                case StateOuterloop:
                    Node?[]? buckets = this.buckets;
                    Debug.Assert(buckets != null);

                    int i = ++this.i;
                    if ((uint)i < (uint)buckets!.Length) {
                        // The Volatile.Read ensures that we have a copy of the reference to buckets[i]:
                        // this protects us from reading fields ('key', 'value' and 'next') of different instances.
                        this.node = Volatile.Read(ref buckets[i]);
                        state = StateInnerLoop;
                        goto case StateInnerLoop;
                    }
                    goto default;

                case StateInnerLoop:
                    Node? node = this.node;
                    if (node != null) {
                        Current = node.Item;
                        this.node = node.Next;
                        return true;
                    }
                    goto case StateOuterloop;

                default:
                    state = StateDone;
                    return false;
            }
        }
    }

    private class Tables {
        internal readonly Node[] Buckets;
        internal readonly object[] Locks;
        internal readonly int[] CountPerLock;

        internal Tables(Node[] buckets, object[] locks, int[] countPerLock) {
            Buckets = buckets;
            Locks = locks;
            CountPerLock = countPerLock;
        }
    }

    private class Node {
        internal readonly T Item;
        internal readonly int Hashcode;
        internal volatile Node? Next;

        internal Node(T item, int hashcode, Node? next) {
            Item = item;
            Hashcode = hashcode;
            Next = next;
        }
    }

    // volatile is used to indicate that a field might be modified by multiple threads
    // that are executing at the same time. Fields that are declared volatile are not
    // subject to compiler optimizations that assume access by a single thread.
    // This is important for multi-threaded applications where such assumptions can lead to incorrect behavior.
}