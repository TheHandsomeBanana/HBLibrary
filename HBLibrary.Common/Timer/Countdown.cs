using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HBLibrary.Common.Timer;
public sealed class Countdown : IDisposable {
    private readonly System.Timers.Timer timer;
    private readonly TimeSpan initialTime;
    private readonly Stopwatch fullTimer;
    private TimeSpan remainingTime;

    public event ElapsedEventHandler? CountdownTick;
    public event DoneHandler? CountdownCompleted;

    public Countdown(TimeSpan countdown, TimeSpan? interval = null) {
        initialTime = countdown;
        timer = new System.Timers.Timer(interval?.TotalMilliseconds ?? 1000);
        timer.Elapsed += OnElapsed;
        fullTimer = new Stopwatch();
    }

    private void OnElapsed(object? sender, ElapsedEventArgs e) {
        if (remainingTime.TotalMilliseconds <= 0) {
            timer.Stop();
            CountdownCompleted?.Invoke(this, fullTimer.Elapsed);
        }
        else {
            remainingTime = remainingTime.Subtract(TimeSpan.FromMilliseconds(timer.Interval));
            CountdownTick?.Invoke(this, e);
        }
    }

    public void Start() {
        if (!fullTimer.IsRunning) {
            fullTimer.Start();
        }

        timer.Start();
    }

    public void Stop() {
        fullTimer.Stop();

        timer.Stop();
    }

    public void Reset() {
        remainingTime = initialTime;
        timer.Stop();
    }

    public void Reset(TimeSpan newTime) {
        remainingTime = newTime;
        timer.Stop();
    }

    public void Restart() {
        remainingTime = initialTime;
    }

    public void Dispose() {
        timer.Dispose();
    }
}
