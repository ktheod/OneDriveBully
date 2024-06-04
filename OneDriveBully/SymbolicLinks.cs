using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OneDriveBully
{
    internal class SymbolicLinks
    {

        //Release 1.4 - New Class to handle symbolic links
        
        // Create symbolic link ******************************************************************************************************
        
        // Import CreateSymbolicLink function from kernel32.dll
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, uint dwFlags);

        // Constants for symbolic link types
        const uint SYMBOLIC_LINK_FLAG_FILE = 0x0;
        const uint SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;

        public static void CreateSymlink(string symlink, string target, bool isDirectory)
        {
            uint flags = isDirectory ? SYMBOLIC_LINK_FLAG_DIRECTORY : SYMBOLIC_LINK_FLAG_FILE;
            bool result = CreateSymbolicLink(symlink, target, flags);

            if (!result)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // Delete symbolic link ******************************************************************************************************

        // Import DeleteFile function from kernel32.dll
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteFile(string lpFileName);

        // Import RemoveDirectory function from kernel32.dll
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveDirectory(string lpPathName);

        public static void DeleteSymlink(string symlink, bool isDirectory)
        {
            bool result;
            if (isDirectory)
            {
                result = RemoveDirectory(symlink);
            }
            else
            {
                result = DeleteFile(symlink);
            }

            if (!result)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // Check if directroy is a symbolic link ******************************************************************************************************

        // Import GetFileAttributes function from kernel32.dll
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern uint GetFileAttributes(string lpFileName);

        // Constants for file attributes
        private const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x0400;
        private const uint INVALID_FILE_ATTRIBUTES = 0xFFFFFFFF;

        // Constant for the reparse point tag that identifies a symbolic link
        private const uint IO_REPARSE_TAG_SYMLINK = 0xA000000C;

        // Import CreateFile function from kernel32.dll
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        // Import DeviceIoControl function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            [Out] byte[] lpOutBuffer,
            int nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        // Constants for CreateFile function
        private const uint GENERIC_READ = 0x80000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        private const uint FSCTL_GET_REPARSE_POINT = 0x000900A8;
        private const int MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16 * 1024;
        private const int REPARSE_DATA_BUFFER_HEADER_SIZE = 8;
        private const int REPARSE_DATA_BUFFER_TAG_OFFSET = 8;

        // Reparse point structure
        [StructLayout(LayoutKind.Sequential)]
        private struct REPARSE_DATA_BUFFER
        {
            public uint ReparseTag;
            public ushort ReparseDataLength;
            public ushort Reserved;
            public ushort SubstituteNameOffset;
            public ushort SubstituteNameLength;
            public ushort PrintNameOffset;
            public ushort PrintNameLength;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXIMUM_REPARSE_DATA_BUFFER_SIZE)]
            public byte[] PathBuffer;
        }

        public static bool IsDirectorySymbolicLink(string path)
        {
            uint attributes = GetFileAttributes(path);

            if (attributes == INVALID_FILE_ATTRIBUTES)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            // Check if the directory is a reparse point
            if ((attributes & FILE_ATTRIBUTE_REPARSE_POINT) == FILE_ATTRIBUTE_REPARSE_POINT)
            {
                return IsReparsePointSymbolicLink(path);
            }

            return false;
        }

        private static bool IsReparsePointSymbolicLink(string path)
        {
            using (SafeFileHandle handle = CreateFile(
                path,
                GENERIC_READ,
                FILE_SHARE_READ,
                IntPtr.Zero,
                OPEN_EXISTING,
                FILE_FLAG_OPEN_REPARSE_POINT | FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero))
            {
                if (handle.IsInvalid)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                byte[] reparseDataBuffer = new byte[REPARSE_DATA_BUFFER_HEADER_SIZE + MAXIMUM_REPARSE_DATA_BUFFER_SIZE];
                if (!DeviceIoControl(handle.DangerousGetHandle(), FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, reparseDataBuffer, reparseDataBuffer.Length, out uint bytesReturned, IntPtr.Zero))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                var reparseData = ByteArrayToStructure<REPARSE_DATA_BUFFER>(reparseDataBuffer);

                return reparseData.ReparseTag == IO_REPARSE_TAG_SYMLINK;
            }
        }

        // Get the target of a symbolic link ******************************************************************************************************
        public static string GetSymbolicLinkTarget(string symlinkPath)
        {
            using (SafeFileHandle handle = CreateFile(
                symlinkPath,
                GENERIC_READ,
                FILE_SHARE_READ,
                IntPtr.Zero,
                OPEN_EXISTING,
                FILE_FLAG_OPEN_REPARSE_POINT | FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero))
            {
                if (handle.IsInvalid)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                byte[] reparseDataBuffer = new byte[REPARSE_DATA_BUFFER_HEADER_SIZE + MAXIMUM_REPARSE_DATA_BUFFER_SIZE];
                if (!DeviceIoControl(handle.DangerousGetHandle(), FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, reparseDataBuffer, reparseDataBuffer.Length, out uint bytesReturned, IntPtr.Zero))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                var reparseData = ByteArrayToStructure<REPARSE_DATA_BUFFER>(reparseDataBuffer);

                if (reparseData.ReparseTag != IO_REPARSE_TAG_SYMLINK)
                {
                    throw new InvalidOperationException("The specified path is not a symbolic link.");
                }

                string target = Encoding.Unicode.GetString(reparseData.PathBuffer, reparseData.SubstituteNameOffset, reparseData.SubstituteNameLength);
                target = target.Substring(target.IndexOf("\\??\\") + 4);
                return target;
            }
        }

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
