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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO.FileSystem.BaseClasses
{
    /// <summary>
    /// Directory base class
    /// </summary>
    /// <typeparam name="FileType">File type</typeparam>
    /// <typeparam name="InternalFileType">Internal file type</typeparam>
    public abstract class FileBase<InternalFileType, FileType> : IFile
        where FileType : FileBase<InternalFileType, FileType>, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected FileBase()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InternalFile">Internal file</param>
        protected FileBase(InternalFileType InternalFile)
        {
            this.InternalFile = InternalFile;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal directory
        /// </summary>
        protected InternalFileType InternalFile { get; set; }

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public abstract DateTime Accessed { get; }

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public abstract DateTime Created { get; }

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public abstract DateTime Modified { get; }

        /// <summary>
        /// Directory the file is within
        /// </summary>
        public abstract IDirectory Directory { get; }

        /// <summary>
        /// Does the file exist?
        /// </summary>
        public abstract bool Exists { get; }

        /// <summary>
        /// File extension
        /// </summary>
        public abstract string Extension { get; }

        /// <summary>
        /// Full path
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Size of the file
        /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public abstract string Name { get; }

        #endregion

        #region Functions

        /// <summary>
        /// Deletes the file
        /// </summary>
        public abstract Task Delete();

        /// <summary>
        /// Reads the file in as a string
        /// </summary>
        /// <returns>The file contents as a string</returns>
        public abstract string Read();

        /// <summary>
        /// Reads a file as binary
        /// </summary>
        /// <returns>The file contents as a byte array</returns>
        public abstract byte[] ReadBinary();

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="NewName">New name for the file</param>
        public abstract Task Rename(string NewName);

        /// <summary>
        /// Moves the file to a new directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public abstract Task MoveTo(IDirectory Directory);

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <param name="Encoding">Encoding to use for the content</param>
        public abstract Task Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null);

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        public abstract Task Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create);

        /// <summary>
        /// Returns the name of the file
        /// </summary>
        /// <returns>The name of the file</returns>
        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            FileBase<InternalFileType, FileType> File = obj as FileBase<InternalFileType, FileType>;
            return File != null && File == this;
        }

        /// <summary>
        /// Gets the hash code for the file
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        /// <summary>
        /// Compares this to another file
        /// </summary>
        /// <param name="other">File to compare to</param>
        /// <returns>-1 if this is smaller, 0 if they are the same, 1 if it is larger</returns>
        public int CompareTo(IFile other)
        {
            if (other == null)
                return 1;
            return string.Compare(FullName, other.FullName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares this object to another object
        /// </summary>
        /// <param name="obj">Object to compare it to</param>
        /// <returns>-1 if this is smaller, 0 if they are the same, 1 if it is larger</returns>
        public int CompareTo(object obj)
        {
            FileBase<InternalFileType, FileType> Temp = obj as FileBase<InternalFileType, FileType>;
            if (Temp == null)
                return 1;
            return CompareTo(Temp);
        }

        /// <summary>
        /// Determines if the files are equal
        /// </summary>
        /// <param name="other">Other file</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public bool Equals(IFile other)
        {
            if (other == null)
                return false;
            return other.FullName == FullName;
        }

        /// <summary>
        /// Clones the file object
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            FileBase<InternalFileType, FileType> Temp = new FileType();
            Temp.InternalFile = InternalFile;
            return Temp;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if two directories are equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if ((object)File1 == null && (object)File2 == null)
                return true;
            if ((object)File1 == null || (object)File2 == null)
                return false;
            return File1.FullName == File2.FullName;
        }

        /// <summary>
        /// Determines if two directories are not equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            return !(File1 == File2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) < 0;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <=(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) <= 0;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) > 0;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >=(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Reads the file and converts it to a string
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>The file as a string</returns>
        public static implicit operator string(FileBase<InternalFileType, FileType> File)
        {
            return File.Read();
        }

        /// <summary>
        /// Reads the file and converts it to a byte array
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>The file as a byte array</returns>
        public static implicit operator byte[](FileBase<InternalFileType, FileType> File)
        {
            return File.ReadBinary();
        }

        #endregion
    }
}