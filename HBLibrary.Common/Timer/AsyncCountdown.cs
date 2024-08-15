using System.Diagnostics;

namespace HBLibrary.Common.Timer;

public delegate void TickHandler(object sender, TimeSpan remainingTime);
public delegate void DoneHandler(object sender, TimeSpan fullTime);

public sealed class AsyncCountdown : IDisposable {
    private readonly TimeSpan initialTime;
    private readonly TimeSpan interval;
    private readonly Stopwatch fullTimer;

    private TimeSpan remainingTime;
    private CancellationTokenSource? cts;

    public event TickHandler? CountdownTick;
    public event DoneHandler? CountdownCompleted;

    public AsyncCountdown(TimeSpan countdown, TimeSpan? interval = null) {
        initialTime = countdown;
        remainingTime = countdown;
        this.interval = interval ?? TimeSpan.FromSeconds(1);
        this.fullTimer = new Stopwatch();
    }

    public Task StartAsync() {
        if (!fullTimer.IsRunning) {
            fullTimer.Start();
        }

        if (cts is not null) {
            cts.Cancel();
            cts.Dispose();
        }

        cts = new CancellationTokenSource();
        return RunCountdownAsync(cts.Token);
    }

    public void Stop() {
        cts?.Cancel();
    }

    public void Reset() {
        remainingTime = initialTime;
        cts?.Cancel();
    }

    public void Reset(TimeSpan newTime) {
        remainingTime = newTime;
        cts?.Cancel();
    }

    public void Restart() {
        remainingTime = initialTime;
    }

#if NET5_0_OR_GREATER
    public Task StopAsync() {
        return cts?.CancelAsync() ?? Task.CompletedTask;
    }

    public Task ResetAsync() {
        remainingTime = initialTime;
        return cts?.CancelAsync() ?? Task.CompletedTask;
    }

    public Task ResetAsync(TimeSpan newTime) {
        remainingTime = newTime;
        return cts?.CancelAsync() ?? Task.CompletedTask;
    }
#endif

    private async Task RunCountdownAsync(CancellationToken token) {
        Stopwatch sw = Stopwatch.StartNew();
        while (token.IsCancellationRequested || remainingTime.TotalMilliseconds > 0) {
            await Task.Delay(interval, token);
            TimeSpan elapsed = sw.Elapsed;
            sw.Restart();

            remainingTime = remainingTime.Subtract(elapsed);
            CountdownTick?.Invoke(this, remainingTime);

            if (remainingTime.TotalMilliseconds <= 0) {
                fullTimer.Stop();
                CountdownCompleted?.Invoke(this, fullTimer.Elapsed);
            }
        }
    }

    public void Dispose() {
        cts?.Dispose();
    }
}
