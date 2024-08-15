﻿using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace HBLibrary.VisualStudio.Commands;
public abstract class AsyncCommandBase : CommandBase {
    private readonly Action<Exception> onException;
    private bool isRunning = false;

    protected AsyncCommandBase(AsyncPackage package, IMenuCommandService commandService, Action<Exception> onException) : base(package, commandService) {
        this.onException = onException;
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    protected override async void Execute(object sender, EventArgs e) {
        try {
            if (isRunning)
                throw new CommandException($"Command {this.GetType().Name} is already running.");

            isRunning = true;
            await ExecuteAsync(sender, e);
        }
        catch (Exception ex) {
            onException?.Invoke(ex);
        }

        isRunning = false;
    }
#pragma warning restore VSTHRD100 // Avoid async void methods

    protected abstract Task ExecuteAsync(object sender, EventArgs e);


}
