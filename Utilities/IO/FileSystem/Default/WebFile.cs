﻿/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;
using System.Net;
using System.Threading.Tasks;
#endregion

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Basic web file class
    /// </summary>
    public class WebFile : FileBase<Uri,WebFile>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public WebFile()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        public WebFile(string Path)
            : base(new Uri(Path))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="File">File to use</param>
        public WebFile(Uri File)
            :base(File)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Time accessed (Just returns now)
        /// </summary>
        public override DateTime Accessed
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Time created (Just returns now)
        /// </summary>
        public override DateTime Created
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Time modified (just returns now)
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Directory base path
        /// </summary>
        public override IDirectory Directory
        {
            get { return new WebDirectory((string)InternalFile.AbsolutePath.Take(InternalFile.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1)); }
        }

        /// <summary>
        /// Does it exist? Always true.
        /// </summary>
        public override bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Extension (always empty)
        /// </summary>
        public override string Extension
        {
            get { return ""; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalFile.AbsolutePath; }
        }

        /// <summary>
        /// Size of the file (always 0)
        /// </summary>
        public override long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Absolute path of the file (same as FullName)
        /// </summary>
        public override string Name
        {
            get { return InternalFile.AbsolutePath; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Delete (does nothing)
        /// </summary>
        public override async Task Delete()
        {
            await Task.Run(() => { });
        }

        /// <summary>
        /// Reads the web page
        /// </summary>
        /// <returns>The content as a string</returns>
        public override string Read()
        {
            using (WebClient Client = new WebClient())
            {
                using (StreamReader Reader = new StreamReader(Client.OpenRead(InternalFile)))
                {
                    return Reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Reads the web page
        /// </summary>
        /// <returns>The content as a byte array</returns>
        public override byte[] ReadBinary()
        {
            using (WebClient Client = new WebClient())
            {
                using (Stream Reader = Client.OpenRead(InternalFile))
                {
                    using (MemoryStream FinalStream = new MemoryStream())
                    {
                        while (true)
                        {
                            byte[] Buffer = new byte[1024];
                            int Count = Reader.Read(Buffer, 0, Buffer.Length);
                            if (Count == 0)
                                break;
                            FinalStream.Write(Buffer, 0, Count);
                        }
                        return FinalStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Renames the file (not used)
        /// </summary>
        /// <param name="NewName">Not used</param>
        public override async Task Rename(string NewName)
        {
            await Task.Run(() => { });
        }

        /// <summary>
        /// Moves the file (not used)
        /// </summary>
        /// <param name="Directory">Not used</param>
        public override async Task MoveTo(IDirectory Directory)
        {
            await Task.Run(() => { });
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Content">Not used</param>
        /// <param name="Mode">Not used</param>
        /// <param name="Encoding">Not used</param>
        public override async Task Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null)
        {
            await Task.Run(() => { });
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Content">Not used</param>
        /// <param name="Mode">Not used</param>
        public override async Task Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            await Task.Run(() => { });
        }

        #endregion
    }
}