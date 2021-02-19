using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace OneDriveBully
{
    public interface IReparseData
    {
        uint GetReparseTag { get; }

        ushort GetPrintNameOffset { get; }

        byte[] GetPathBuffer { get; }

        ushort GetPrintNameLength { get; }

        uint ExpectedReparseTag { get; }
    }

    // For More Details about this Class, please visit http://troyparsons.com/blog/2012/03/symbolic-links-in-c-sharp/
    //Version 1.3 - Changed [DllImport("kernel32.dll", SetLastError = true)] to [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]

    [StructLayout(LayoutKind.Sequential)]
    public struct SymbolicLinkReparseData : IReparseData
    {
        // Not certain about this!
        private const int maxUnicodePathLength = 260 * 2;

        public uint ReparseTag;
        public ushort ReparseDataLength;
        public ushort Reserved;
        public ushort SubstituteNameOffset;
        public ushort SubstituteNameLength;
        public ushort PrintNameOffset;
        public ushort PrintNameLength;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = maxUnicodePathLength)]
        public byte[] PathBuffer;

        public uint GetReparseTag => ReparseTag;
        public ushort GetPrintNameOffset => PrintNameOffset;
        public byte[] GetPathBuffer => PathBuffer;
        public ushort GetPrintNameLength => PrintNameLength;
        public uint ExpectedReparseTag => 0xA000000C;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MountPointReparseData : IReparseData
    {
        // Not certain about this!
        private const int maxUnicodePathLength = 260 * 2;

        public uint ReparseTag;
        public ushort ReparseDataLength;
        public ushort Reserved;
        public ushort SubstituteNameOffset;
        public ushort SubstituteNameLength;
        public ushort PrintNameOffset;
        public ushort PrintNameLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = maxUnicodePathLength)]
        public byte[] PathBuffer;

        public uint GetReparseTag => ReparseTag;
        public ushort GetPrintNameOffset => PrintNameOffset;
        public byte[] GetPathBuffer => PathBuffer;
        public ushort GetPrintNameLength => PrintNameLength;
        public uint ExpectedReparseTag => 0xA0000003;
    }

    public class SymbolicLink
    {
        private const uint genericReadAccess = 0x80000000;

        private const uint fileFlagsForOpenReparsePointAndBackupSemantics = 0x02200000;

        private const int ioctlCommandGetReparsePoint = 0x000900A8;

        private const uint openExisting = 0x3;

        private const uint pathNotAReparsePointError = 0x80071126;

        private const uint shareModeAll = 0x7; // Read, Write, Delete

        private const int targetIsAFile = 0;

        private const int targetIsADirectory = 1;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)] //Version 1.3 -+
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)] //Version 1.3 -+
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)] //Version 1.3 -+
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        public static void CreateDirectoryLink(string linkPath, string targetPath)
        {
            if (!CreateSymbolicLink(linkPath, targetPath, targetIsADirectory) || Marshal.GetLastWin32Error() != 0)
            {
                try
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
                catch (COMException exception)
                {
                    throw new IOException(exception.Message, exception);
                }
            }
        }

        public static void CreateFileLink(string linkPath, string targetPath)
        {
            if (!CreateSymbolicLink(linkPath, targetPath, targetIsAFile))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        public static bool Exists(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                return false;
            }
            string target = GetTarget(path);
            return target != null;
        }

        private static SafeFileHandle getFileHandle(string path)
        {
            return CreateFile(path, genericReadAccess, shareModeAll, IntPtr.Zero, openExisting,
                fileFlagsForOpenReparsePointAndBackupSemantics, IntPtr.Zero);
        }
        public string GetSymLinkTarget(string path)
        {
            string result = GetTarget<SymbolicLinkReparseData>(path);

            // If we couldn't get a symlink, try getting a mount point
            if (string.IsNullOrEmpty(result))
            {
                result = GetTarget<MountPointReparseData>(path);
            }

            return result;
        }

        // Old version of this method with no generic parameter for compatibility
        // It assumes that symlink is desired, which is the old functionality.
        public static string GetTarget(string path)
        {
            return GetTarget<SymbolicLinkReparseData>(path);
        }

        public static string GetTarget<TReparseData>(string path) where TReparseData : IReparseData
        {
            TReparseData reparseDataBuffer;

            using (SafeFileHandle fileHandle = getFileHandle(path))
            {
                if (fileHandle.IsInvalid)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                int outBufferSize = Marshal.SizeOf(typeof(SymbolicLinkReparseData));
                IntPtr outBuffer = IntPtr.Zero;
                try
                {
                    outBuffer = Marshal.AllocHGlobal(outBufferSize);
                    int bytesReturned;
                    bool success = DeviceIoControl(
                        fileHandle.DangerousGetHandle(), ioctlCommandGetReparsePoint, IntPtr.Zero, 0,
                        outBuffer, outBufferSize, out bytesReturned, IntPtr.Zero);

                    fileHandle.Close();

                    if (!success)
                    {
                        if (((uint)Marshal.GetHRForLastWin32Error()) == pathNotAReparsePointError)
                        {
                            return null;
                        }
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    reparseDataBuffer = (TReparseData)Marshal.PtrToStructure(
                        outBuffer, typeof(TReparseData));
                }
                finally
                {
                    Marshal.FreeHGlobal(outBuffer);
                }
            }
            if (reparseDataBuffer.GetReparseTag != reparseDataBuffer.ExpectedReparseTag)
            {
                return null;
            }

            string target = Encoding.Unicode.GetString(reparseDataBuffer.GetPathBuffer,
                reparseDataBuffer.GetPrintNameOffset, reparseDataBuffer.GetPrintNameLength);

            return target;
        }
    }
}
