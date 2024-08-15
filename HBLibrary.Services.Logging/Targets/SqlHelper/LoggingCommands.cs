﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.Targets.SqlHelper;
public static class SqliteLoggingCommands {
    public static string GetCreateTableCommand(string tableName) {
        return $@"
        CREATE TABLE IF NOT EXISTS {tableName} (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT NOT NULL,
            Category TEXT NOT NULL,
            Level TEXT NOT NULL,
            Message TEXT NOT NULL
        )";
    }

    public static string GetInsertLogCommand(string tableName) {
        return $@"
        INSERT INTO {tableName} (Date, Category, Level, Message)
        VALUES (@Date, @Category, @Level, @Message)";
    }
}

public static class SqlServerLoggingCommands  {
    public static string GetCreateTableCommand(string tableName) {
        return $@"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' and xtype='U')
        CREATE TABLE {tableName} (
            Id INT IDENTITY(1,1) PRIMARY KEY,
            Date DATETIME NOT NULL,
            Category NVARCHAR(255) NOT NULL,
            Level NVARCHAR(50) NOT NULL,
            Message NVARCHAR(MAX) NOT NULL
        )";
    }

    public static string GetInsertLogCommand(string tableName) {
        return $@"
        INSERT INTO {tableName} (Date, Category, Level, Message)
        VALUES (@Date, @Category, @Level, @Message)";
    }
}

public static class PostgresLoggingCommands {
    public static string GetCreateTableCommand(string tableName) {
        return $@"
        CREATE TABLE IF NOT EXISTS {tableName} (
            Id SERIAL PRIMARY KEY,
            Date TIMESTAMP NOT NULL,
            Category TEXT NOT NULL,
            Level TEXT NOT NULL,
            Message TEXT NOT NULL
        )";
    }

    public static string GetInsertLogCommand(string tableName) {
        return $@"
        INSERT INTO {tableName} (Date, Category, Level, Message)
        VALUES (@Date, @Category, @Level, @Message)";
    }
}

public static class MariaDBLoggingCommands {
    public static string GetCreateTableCommand(string tableName) {
        return $@"
        CREATE TABLE IF NOT EXISTS {tableName} (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Date DATETIME NOT NULL,
            Category VARCHAR(255) NOT NULL,
            Level VARCHAR(50) NOT NULL,
            Message TEXT NOT NULL
        )";
    }

    public static string GetInsertLogCommand(string tableName) {
        return $@"
        INSERT INTO {tableName} (Date, Category, Level, Message)
        VALUES (@Date, @Category, @Level, @Message)";
    }
}

public static class OracleLoggingCommands {
    public static string GetCreateTableCommand(string tableName) {
        return $@"
        BEGIN
            EXECUTE IMMEDIATE 'CREATE TABLE {tableName} (
                Id NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
                Date TIMESTAMP NOT NULL,
                Category VARCHAR2(255) NOT NULL,
                Level VARCHAR2(50) NOT NULL,
                Message CLOB NOT NULL
            )';
        EXCEPTION
            WHEN OTHERS THEN
                IF SQLCODE != -955 THEN
                    RAISE;
                END IF;
        END;";
    }

    public static string GetInsertLogCommand(string tableName) {
        return $@"
        INSERT INTO {tableName} (Date, Category, Level, Message)
        VALUES (:Date, :Category, :Level, :Message)";
    }
}