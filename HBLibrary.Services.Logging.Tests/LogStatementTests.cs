using HBLibrary.Services.Logging.Statements;
using System.Globalization;
using System.Text.Json;
using System.Xml.Serialization;

namespace HBLibrary.Services.Logging.Tests;
[TestClass]
public class LogStatementTests {
    [TestMethod]
    public void LogStatement_SerializeToJson() {
        LogStatement logStatement = new LogStatement {
            CreatedOn = DateTime.Now,
            Level = LogLevel.Debug,
            Message = "Testmessage",
            Name = "Testlogger"
        };

        try {
            string json = logStatement.ToJson();
        }
        catch (Exception ex) {
            Assert.Fail(ex.ToString());
        }
    }

    [TestMethod]
    public void LogStatement_DeserializeFromJson() {
        string logStatement = "{\"Message\":\"Testmessage\",\"Name\":\"Testlogger\",\"Level\":0,\"CreatedOn\":\"2024-01-22 12:40:26\",\"CreatedOnFormatted\":\"2024-01-22 01:40:26\"}";
        LogStatement statement = JsonSerializer.Deserialize<LogStatement>(logStatement);
        Assert.AreEqual("Testmessage", statement.Message);
    }

    [TestMethod]
    public void LogStatement_SerializeToXml() {
        LogStatement logStatement = new LogStatement {
            CreatedOn = DateTime.Now,
            Level = LogLevel.Debug,
            Message = "Testmessage",
            Name = "Testlogger"
        };

        try {
            string xml = logStatement.ToXml();
        }
        catch (Exception ex) {
            Assert.Fail(ex.ToString());
        }
    }

    [TestMethod]
    public void LogStatement_DeserializeFromXml() {
        string logStatement = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<LogStatement xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Message>Testmessage</Message>\r\n  <Name>Testlogger</Name>\r\n  <Level>Debug</Level>\r\n  <CreatedOn>2024-01-22 01:43:21</CreatedOn>\r\n</LogStatement>";

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogStatement));

        LogStatement statement;
        using (StringReader sr = new StringReader(logStatement)) {
            statement = (LogStatement)xmlSerializer.Deserialize(sr)!;
        }

        Assert.AreEqual("2024-01-22 01:43:21", statement.CreatedOnFormatted);
        Assert.AreEqual(DateTime.ParseExact("2024-01-22 01:43:21", "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture), statement.CreatedOn);
    }
}
