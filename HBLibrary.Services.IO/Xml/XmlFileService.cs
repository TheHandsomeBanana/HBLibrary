﻿using System.Xml.Serialization;

namespace HBLibrary.Services.IO.Xml;
public class XmlFileService : IXmlFileService {
    public TXml? ReadXml<TXml>(FileSnapshot file, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        return (TXml?)serializer.Deserialize(file.OpenStream(FileMode.Open, FileAccess.Read, share));
    }

    public object? ReadXml(Type type, FileSnapshot file, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        return serializer.Deserialize(file.OpenStream(FileMode.Open, FileAccess.Read, share));
    }

    public void WriteXml<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using TextWriter sw = new StreamWriter(file.OpenStream(append ? FileMode.Append : FileMode.Open, FileAccess.Write, share));
        serializer.Serialize(sw, xmlObject);
    }

    public void WriteXml(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using TextWriter sw = new StreamWriter(file.OpenStream(append ? FileMode.Append : FileMode.Open, FileAccess.Write, share));
        serializer.Serialize(sw, xmlObject);
    }
}
