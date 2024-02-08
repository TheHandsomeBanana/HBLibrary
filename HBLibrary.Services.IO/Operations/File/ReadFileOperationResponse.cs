﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HBLibrary.Services.IO.Operations.File;
public class ReadFileOperationResponse : FileOperationResponse
{
    public byte[] Result { get; internal set; } = [];
    public string? ResultString { get; internal set; }

    public override string GetStringResult()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToString());

        if (ResultString is not null)
            sb.Append("\nResult: " + ResultString);

        return sb.ToString();
    }

    public TJson? ParseResultJson<TJson>(JsonSerializerOptions? options)
    {
        if (ResultString is null)
            throw new IOException("Result is not parsable.");

        return JsonSerializer.Deserialize<TJson?>(ResultString, options);
    }

    public TXml? ParseResultXml<TXml>()
    {
        if (ResultString is null)
            throw new IOException("Result is not parsable.");

        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        return (TXml?)serializer.Deserialize(new StringReader(ResultString));
    }
}
