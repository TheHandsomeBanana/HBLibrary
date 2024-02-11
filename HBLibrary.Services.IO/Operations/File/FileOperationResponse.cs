﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public abstract class FileOperationResponse : IOOperationResponse {
    public FileSnapshot? File { get; internal set; }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToString());

        if (File.HasValue)
            sb.Append($"\nFile: {File.Value.FullPath}");

        return sb.ToString();
    }
}