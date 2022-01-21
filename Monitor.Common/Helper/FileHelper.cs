using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Monitor.Common
{
    public class FileHelper
    {
        public uint             ValidSize { get; set; }
        public uint             ValidCrc  { get; set; }
        public uint             WholeSize { get; set; }
        public uint             WholeCrc  { get; set; }
        public List<List<byte>> Packet    { get; set; } = new List<List<byte>>();

        public FileHelper(string path, int offset = 0)
        {
            var bytes = File.ReadAllBytes(path);

            ValidSize = (uint)bytes.Length - (uint)offset;

            WholeSize = (uint)bytes.Length;

            WholeCrc = CrcHelper.Crc32(bytes);

            ValidCrc = CrcHelper.Crc32(bytes.Skip(offset).ToArray());

            var list = new List<byte>(File.ReadAllBytes(path));

            while (list.Count > 255)
            {
                Packet.Add(list.Take(256).ToList());
                list.RemoveRange(0, 256);
            }

            if (list.Count > 0)
            {
                Packet.Add(list);
            }
        }
    }
}
