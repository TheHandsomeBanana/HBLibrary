﻿using HBLibrary.NetFramework.Common.Json;
using HBLibrary.NetFramework.Services.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HBLibrary.NetFramework.Services.Logging.Statements {
    [Serializable]
    public struct LogStatement {
        public string Message { get; set; }
        public string Name { get; set; }
        public LogLevel Level { get; set; }
        [XmlIgnore]
        [JsonDateTimeFormat("yyyy-MM-dd hh:mm:ss")]
        public DateTime CreatedOn { get; set; }

        [XmlElement("CreatedOn")]
        public string CreatedOnFormatted {
            get { return CreatedOn.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture); }
            set { CreatedOn = DateTime.ParseExact(value, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture); }
        }

        [JsonConstructor]
        public LogStatement(string message, string name, LogLevel level, DateTime createdOn) {
            Message = message;
            Name = name;
            Level = level;
            CreatedOn = createdOn;
        }

        public override string ToString() => $"[{Level}]: {Message}";
        public string ToFullString()
            => $"Name: {Name}\nCreated On: {CreatedOn:yyyy-MM-dd hh:MM:ss}\nLog Level: {Level}\nMessage: {Message}";
        public string ToMinimalString() => $"[{CreatedOn:hh:MM:ss}] [{Level}]: {Message}";
        public string ToJson() => JsonSerializer.Serialize(this);
        public string ToXml() {
            using (TextWriter stringwriter = new StringWriter()) {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }

        public string Format(LogDisplayFormat format) {
            switch (format) {
                case LogDisplayFormat.MessageOnly:
                    return ToString();
                case LogDisplayFormat.Minimal:
                    return ToMinimalString();
                case LogDisplayFormat.Full:
                    return ToFullString();
                default: 
                    return ToString();
            }
        }
    }
}