using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Options;
public class WinRARArchiveOptions {

}

public struct WinRARCommand {
    public WinRARCommandName CommandName { get; set; }
    public WinRARCommand(string command) {
        if(command.StartsWith("rr")) {

        }

        if (!CommandMap.ContainsKey(command) || !WildcardCommands.All(e => !command.StartsWith(e)))
            throw new ArgumentException($"Command {command} invalid.");

    }

    public static readonly Dictionary<string, WinRARCommandName> CommandMap = new() {
        { "a", WinRARCommandName.AddFilesToArchive },
        { "c", WinRARCommandName.AddCommentToArchive },
        { "ch", WinRARCommandName.ChangeArchiveParameters },
        { "cv", WinRARCommandName.ConvertArchives },
        { "cw", WinRARCommandName.WriteArchiveCommentToFile },
        { "d", WinRARCommandName.DeleteFilesFromArchive },
        { "e", WinRARCommandName.ExtractFilesFromArchiveIgnoringPaths },
        { "f", WinRARCommandName.FreshenFilesWithinArchive },
        { "i", WinRARCommandName.FindStringInArchives },
        { "k", WinRARCommandName.LockArchive },
        { "m", WinRARCommandName.MoveFilesAndFoldersToArchive },
        { "r", WinRARCommandName.RepairDamagedArchive },
        { "rc", WinRARCommandName.ReconstructMissingVolumes },
        { "rn", WinRARCommandName.RenameArchivedFiles },
        { "rr", WinRARCommandName.AddDataRecoveryRecord },
        { "rv", WinRARCommandName.CreateRecoveryVolumes },
        { "s", WinRARCommandName.ConvertArchiveToSelfExtractingType },
        { "s-", WinRARCommandName.RemoveSFXModule },
        { "t", WinRARCommandName.TestArchiveFiles },
        { "u", WinRARCommandName.UpdateFilesWithinArchive },
        { "x", WinRARCommandName.ExtractFilesFromArchiveWithFullPaths },
    };

    public static readonly string[] WildcardCommands = ["rr", "rv", "s"];
}

public enum WinRARCommandName {
    AddFilesToArchive,
    AddCommentToArchive,
    ChangeArchiveParameters,
    ConvertArchives,
    WriteArchiveCommentToFile,
    DeleteFilesFromArchive,
    ExtractFilesFromArchiveIgnoringPaths,
    FreshenFilesWithinArchive,
    FindStringInArchives,
    LockArchive,
    MoveFilesAndFoldersToArchive,
    RepairDamagedArchive,
    ReconstructMissingVolumes,
    RenameArchivedFiles,
    AddDataRecoveryRecord,
    CreateRecoveryVolumes,
    ConvertArchiveToSelfExtractingType,
    RemoveSFXModule,
    TestArchiveFiles,
    UpdateFilesWithinArchive,
    ExtractFilesFromArchiveWithFullPaths
}

