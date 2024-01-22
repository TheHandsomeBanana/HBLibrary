using HBLibrary.NetFramework.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO.Compression.WinRAR {
    // https://documentation.help/WinRAR/HELPCommandLineSyntax.htm
    public class WinRARCompressionSettings {
        public string ArchiveName { get; set; }
        public WinRARCompressionFormat Format { get; set; } = WinRARCompressionFormat.RAR;
        public WinRARCompressionMethod Method { get; set; } = WinRARCompressionMethod.Best;
        public WinRARDictionarySize DictionarySize { get; set; } = WinRARDictionarySize.Md32m;
        public WinRARVolumeSize? VolumeSize { get; set; }
        public string Password { get; set; }
        public bool ProtectAgainstChanges { get; set; } = false;

        public WinRARCompressionSettings(string archiveName) {
            this.ArchiveName = archiveName;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();


            return sb.ToString();
        }
    }

    public readonly struct WinRARVolumeSize {
        public WinRARSizeType SizeType { get; }
        public int Size { get; }

        public WinRARVolumeSize(WinRARSizeType sizeType, int size) {
            SizeType = sizeType;
            Size = size;
        }

        public static WinRARVolumeSize CD700() => new WinRARVolumeSize(WinRARSizeType.MB, 700);
        public static WinRARVolumeSize FAT32() => new WinRARVolumeSize(WinRARSizeType.MB, 4095);
        public static WinRARVolumeSize DVD_R() => new WinRARVolumeSize(WinRARSizeType.MB, 4481);

        public override string ToString() {
            switch (SizeType) {
                case WinRARSizeType.kB:
                    return "-v" + Size + "k";
                case WinRARSizeType.MB:
                    return "-v" + Size + "m";
                case WinRARSizeType.GB:
                    return "-v" + Size + "g";
            }

            throw new NotSupportedException(SizeType.ToString());
        }
    }


    public enum WinRARSizeType {
        kB,
        MB,
        GB
    }

    public enum WinRARCompressionFormat {
        RAR,
        RAR4,
        ZIP
    }

    public enum WinRARCompressionMethod {
        Save,
        Fastest,
        Fast,
        Normal,
        Good,
        Best
    }

    public enum WinRARDictionarySize {
        Md32k,
        Md64k,
        Md128k,
        Md256k,
        Md512k,
        Md1024k,
        Md2048k,
        Md4096k,
        Md1m,
        Md2m,
        Md4m,
        Md8m,
        Md16m,
        Md32m,
        Md64m,
        Md128m,
        Md256m,
        Md512m,
        Md1024m
    }
}
