using System.Reflection;
using System.Runtime.CompilerServices;

namespace HBLibrary.Common;
public static class AutoMapper {
    public static TDestination MapProperties<TSource, TDestination>(this TSource source) where TDestination : new() where TSource : new() {
        if (source == null) {
            throw new ArgumentNullException(nameof(source));
        }

        TDestination destination = new TDestination();
        PropertyInfo[] sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProperty in sourceProperties) {
            var destinationProperty = destinationProperties.FirstOrDefault(destProp => destProp.Name == sourceProperty.Name && destProp.PropertyType == sourceProperty.PropertyType);
            if (destinationProperty != null) {
                var value = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, value);
            }
        }

        return destination;
    }

    public static TDestination Map<TSource, TDestination>(this TSource source) where TDestination : new() where TSource : new() {
        TDestination destination = new TDestination();

        PropertyInfo[] sourceProperties = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);


        foreach (var sourceProperty in sourceProperties) {
            var destinationProperty = destinationProperties.FirstOrDefault(prop => prop.Name == sourceProperty.Name && prop.PropertyType == sourceProperty.PropertyType);
            if (destinationProperty != null && destinationProperty.CanWrite) {
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        FieldInfo[] sourceFields = typeof(TSource).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo[] destinationFields = typeof(TDestination).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var sourceField in sourceFields) {
            var destinationField = Array.Find(destinationFields, field => field.Name == sourceField.Name && field.FieldType == sourceField.FieldType);
            if (destinationField != null) {
                destinationField.SetValue(destination, sourceField.GetValue(source));
            }
        }

        return destination;
    }

    public static TDestination MapUnsafe<TSource, TDestination>(this TSource source) where TDestination : new() where TSource : new() {
        if (source == null) {
            throw new ArgumentNullException(nameof(source));
        }

        TDestination destination = new TDestination();

        int size = Unsafe.SizeOf<TSource>();
        if (size != Unsafe.SizeOf<TDestination>()) {
            throw new InvalidOperationException("Source and destination types must have the same size.");
        }

        unsafe {
            // Obtain pointers to the source and destination objects
            void* sourcePtr = Unsafe.AsPointer(ref source);
            void* destinationPtr = Unsafe.AsPointer(ref destination);

            // Perform memory copy directly from source to destination
            Buffer.MemoryCopy(sourcePtr, destinationPtr, size, size);
        }

        return destination;
    }
}