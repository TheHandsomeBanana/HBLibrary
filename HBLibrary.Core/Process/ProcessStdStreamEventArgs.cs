﻿namespace HBLibrary.Core.Process;
public class ProcessStdStreamEventArgs : EventArgs {
    public string Data { get; set; }

    public ProcessStdStreamEventArgs(string data) {
        Data = data;
    }
}