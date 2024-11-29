using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core;
public static class ProgressBars {

    public static ConsoleProgressBar CreateConsoleProgressBar(int itemLength, char progressChar, char remainingChar, int progressBarSize, bool showPercentage) {
        return new ConsoleProgressBar(itemLength, progressChar, remainingChar, progressBarSize, showPercentage);
    }

    public static ConsoleProgressBar CreateDefaultConsoleProgressBar(int itemLength) {
        return new ConsoleProgressBar(itemLength, '=', ' ', 64, true);
    }
}

public readonly struct ConsoleProgressBar {
    public int ItemLength { get; }
    public char ProgressChar { get; }
    public char RemainingChar { get; }
    public int ProgressBarSize { get; }
    public bool ShowPercentage { get; }
    
    public ConsoleProgressBar(int itemLength, char progressChar, char remainingChar, int progressBarSize, bool showPercentage) {
        ItemLength = itemLength;
        ProgressChar = progressChar;
        RemainingChar = remainingChar;
        ProgressBarSize = progressBarSize;
        ShowPercentage = showPercentage;
    }

    public string Generate(int completed) {
        if(completed > ItemLength) {
            throw new ArgumentOutOfRangeException(nameof(completed), $"Exceeded maximum length {ItemLength}");
        }

        if (ItemLength == 0) {
            return $"[{new string(RemainingChar, ProgressBarSize)}] 0%";
        }

        double progress = (double)completed / ItemLength;
        int completedChars = (int)(ProgressBarSize * progress);
        int remainingChars = ProgressBarSize - completedChars;

        string generatedBar = $"[{new string(ProgressChar, completedChars)}{new string(RemainingChar, remainingChars)}]";

        return ShowPercentage
            ? $"{generatedBar} {Math.Round(progress * 100)}%"
            : generatedBar;
    }
}



