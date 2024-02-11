using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HBLibrary.Services.IO.Operations.File;
public class WriteFileOperationRequest : FileOperationRequest {
    public override bool CanAsync => true;
    public byte[] Content { get; set; } = [];
    public string? StringContent { get; set; }
    public FileShare Share { get; set; } = FileShare.None;
    public bool Append { get; set; }
    public FileAccess Access => FileAccess.Write;


    public WriteFileOperationRequest WithJsonObject<TJson>(TJson obj, JsonSerializerOptions? options = null) {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        StringContent = JsonSerializer.Serialize(obj, options);
        return this;
    }

    public WriteFileOperationRequest WithXmlObject<TXml>(TXml obj) {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using TextWriter sw = new StringWriter();
        serializer.Serialize(sw, obj);
        StringContent = sw.ToString();
        return this;
    }
}
