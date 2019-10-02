﻿using Game.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Game.Estate.UltimaIX.FilePack
{
    public interface IFlxFile : IDisposable
    {
        void Close();
        HashSet<string> GetContainsSet();
        bool ContainsFile(string filePath);
        Task<byte[]> LoadFileDataAsync(string filePath);
    }

    //http://wiki.ultimacodex.com/wiki/Ultima_IX_Internal_Formats#FLX_Format
    public partial class FlxFile : IFlxFile
    {
        public class FileMetadata
        {
            public long Position;
            public int Size;
        }

        public override string ToString() => $"{Path.GetFileName(FilePath)}";
        GenericReader _r;
        internal FileMetadata[] _files;
        public string FilePath;

        public bool IsAtEof => _r.Position >= _r.BaseStream.Length;

        public FlxFile(string filePath)
        {
            if (filePath == null)
                return;
            FilePath = filePath;
            _r = new BinaryFileReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read));
            ReadMetadata();
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }
        ~FlxFile() => Close();

        public void Close()
        {
            _r?.Close();
            _r = null;
        }

        /// <summary>
        /// Gets the contains set.
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetContainsSet() => new HashSet<string>() { };

        /// <summary>
        /// Determines whether the archive contains a file.
        /// </summary>
        public bool ContainsFile(string filePath) => int.TryParse(filePath, out var file) && file < _files.Length && _files[file] != null;

        /// <summary>
        /// Loads an archived file's data.
        /// </summary>
        public Task<byte[]> LoadFileDataAsync(string filePath)
        {
            if (int.TryParse(filePath, out var file) && file < _files.Length)
                return LoadFileDataAsync(_files[file]);
            Debug.Log($"LoadFileDataAsync: {filePath} @ {_files}");
            throw new FileNotFoundException(filePath);
        }

        /// <summary>
        /// Loads an archived file's data.
        /// </summary>
        internal Task<byte[]> LoadFileDataAsync(FileMetadata file)
        {
            if (file == null || file.Size == 0)
                return Task.FromResult<byte[]>(null);
            var buf = new byte[file.Size];
            lock (_r)
            {
                _r.Position = file.Position;
                _r.Read(buf, 0, buf.Length);
            }
            return Task.FromResult(buf);
        }

        void ReadMetadata()
        {
            _r.BaseStream.Seek(0x50, SeekOrigin.Begin);
            _files = new FileMetadata[_r.ReadInt32()];
            _r.BaseStream.Seek(0x80, SeekOrigin.Begin);
            var chunk = new byte[8];
            for (var i = 0; i < _files.Length; i++)
            {
                _r.Read(chunk, 0, 8);
                var metadata = new FileMetadata
                {
                    Position = BitConverter.ToInt32(chunk, 0),
                    Size = BitConverter.ToInt32(chunk, 4),
                };
                _files[i] = metadata.Size != 0 ? metadata : null;
            }
        }
    }
}