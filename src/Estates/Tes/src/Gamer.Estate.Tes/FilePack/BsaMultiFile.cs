﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gamer.Asset.Tes.FilePack
{
    /// <summary>
    /// BsaMultiFile
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class BsaMultiFile : IDisposable
    {
        public readonly List<BsaFile> Packs = new List<BsaFile>();
        //readonly ILogger _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="BsaMultiFile" /> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <exception cref="System.ArgumentNullException">filePaths</exception>
        /// <exception cref="ArgumentNullException">filePaths</exception>
        public BsaMultiFile(string[] filePaths)
        {
            //_log = services.GetRequiredService<ILogger<BsaMultiFile>>();
            var files = (filePaths ?? throw new ArgumentNullException(nameof(filePaths))).Where(x => Path.GetExtension(x) == ".bsa" || Path.GetExtension(x) == ".ba2");
            Packs.AddRange(files.Select(x => new BsaFile(x)));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Close();

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            foreach (var pack in Packs)
                pack.Close();
        }

        /// <summary>
        /// Determines whether the BSA archive contains a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        ///   <c>true</c> if the specified file path contains file; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool ContainsFile(string filePath) => Packs.Any(x => x.ContainsFile(filePath));

        /// <summary>
        /// Loads an archived file's data.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">Could not find file \"{filePath}</exception>
        public virtual byte[] LoadFileData(string filePath) =>
            (Packs.FirstOrDefault(x => x.ContainsFile(filePath)) ?? throw new FileNotFoundException($"Could not find file \"{filePath}\" in a BSA file."))
            .LoadFileData(filePath);
    }
}