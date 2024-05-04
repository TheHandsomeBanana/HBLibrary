using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace HBLibrary.Services.IO.Obsolete.Operations.File;
public class WriteFileRequest : FileOperationRequest
{
    public override bool CanAsync => true;
    public byte[] Content { get; set; } = [];
    public string? StringContent { get; set; }
    public FileShare Share { get; set; } = FileShare.None;
    public virtual bool Append { get; set; }
    public FileAccess Access => FileAccess.Write;
    public Encoding Encoding { get; set; } = Encoding.UTF8;


    public WriteFileRequest WithJsonObject<TJson>(TJson obj, JsonSerializerOptions? options = null)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        StringContent = JsonSerializer.Serialize(obj, options);
        return this;
    }

    public WriteFileRequest WithXmlObject<TXml>(TXml obj)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using TextWriter sw = new StringWriter();
        serializer.Serialize(sw, obj);
        StringContent = sw.ToString();
        return this;
    }
}
