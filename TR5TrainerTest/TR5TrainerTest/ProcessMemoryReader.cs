using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static TR5TrainerTest.ProcessMemoryReaderApi;
using static TR5TrainerTest.Logger;

namespace TR5TrainerTest
{
    /// <summary>
    /// ProcessMemoryReader is a class that enables direct reading a process memory
    /// </summary>
    unsafe class ProcessMemoryReaderApi
    {
         [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            ProcessAccessFlags processAccess,
            bool bInheritHandle,
            int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, byte* lpBaseAddress, byte* buffer, uint size, out UIntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, byte* lpBaseAddress, byte* lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
    }

    public unsafe class ProcessMemoryReader
    {

        public ProcessMemoryReader()
        {
        }

        /// <summary>	
		/// Process from which to read		
		/// </summary>
		public Process ReadProcess
        {
            get
            {
                return m_ReadProcess;
            }
            set
            {
                m_ReadProcess = value;
            }
        }

        private Process m_ReadProcess = null;

        private IntPtr m_hProcess = IntPtr.Zero;

        public void OpenProcess()
        {
            Log("Attaching to process '{0}' [{1}]", Path.GetFileName(m_ReadProcess.MainModule.FileName), m_ReadProcess.Id);

            m_hProcess = ProcessMemoryReaderApi.OpenProcess(
                ProcessMemoryReaderApi.ProcessAccessFlags.VirtualMemoryRead | 
                ProcessMemoryReaderApi.ProcessAccessFlags.VirtualMemoryWrite | 
                ProcessMemoryReaderApi.ProcessAccessFlags.VirtualMemoryOperation, true, m_ReadProcess.Id);

            Log("Attached with handle 0x{0:X16}", (int) m_hProcess);
        }

        public void CloseHandle()
        {
            if (!ProcessMemoryReaderApi.CloseHandle(m_hProcess))
                throw new Exception("CloseHandle failed");
        }

        public byte ReadByte(byte* addr)
        {
            byte b;
            ReadProcessMemory(m_hProcess, addr, &b, 1, out _);
            return b;
        }

        public short ReadInt16(byte* addr)
        {
            short b;
            ReadProcessMemory(m_hProcess, addr, (byte*)&b, 2, out _);
            return b;
        }

        public ushort ReadUInt16(byte* addr)
        {
            ushort b;
            ReadProcessMemory(m_hProcess, addr, (byte*)&b, 2, out _);
            return b;
        }

        public int ReadInt32(byte* addr)
        {
            int b;
            ReadProcessMemory(m_hProcess, addr, (byte*)&b, 4, out _);
            return b;
        }

        public uint ReadUInt32(byte* addr)
        {
            uint b;
            ReadProcessMemory(m_hProcess, addr, (byte*)&b, 4, out _);
            return b;
        }

        public byte[] ReadBytes(byte* addr, uint len)
        {          
            Log("Allocating {0} bytes for byte[]", len);
            var arr = new byte[len];

            Log("Reading {0} bytes from 0x{1:X8}", len, (int)addr);
            fixed (byte* ptr = arr)
                ReadProcessMemory(m_hProcess, addr, ptr, len, out _);

            return arr;
        }

        public T ReadStruct<T>(byte* addr)
            where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));

            Log("Allocating {0} bytes for {1}", size, typeof(T).Name);
            var buf = Marshal.AllocHGlobal(size);

            Log("Reading from 0x{0:X8}", (int)addr);
            ReadProcessMemory(m_hProcess, addr, (byte*) buf, (uint)size, out _);

            var ret = (T) Marshal.PtrToStructure(buf, typeof(T));

            Marshal.FreeHGlobal(buf);

            return ret;
        }

        public T[] ReadStructArray<T>(byte* addr, int len)
            where T : struct
        {
            var ssize = Marshal.SizeOf(typeof(T));
            var size = ssize * len;

            Log("Allocating {0} bytes for {1}[{2}]", size, typeof(T).Name, len);
            var buf = Marshal.AllocHGlobal(size);

            if (len < 0) 
                return null;

            var arr = new T[len];

            Log("Reading from 0x{0:X8}", (int)addr);
            ReadProcessMemory(m_hProcess, addr, (byte*)buf, (uint)size, out _);

            for (var i = 0; i < len; i++)
                arr[i] = (T) Marshal.PtrToStructure(buf + i * ssize, typeof(T));

            return arr;
        }

        public void WriteByte(byte* addr, byte b)
        {
            WriteProcessMemory(m_hProcess, addr, &b, 1, out _);
        }

        public void WriteInt16(byte* addr, short b)
        {
            WriteProcessMemory(m_hProcess, addr, (byte*)&b, 2, out _);
        }

        public void WriteUInt16(byte* addr, ushort b)
        {
            WriteProcessMemory(m_hProcess, addr, (byte*)&b, 2, out _);
        }

        public void WriteInt32(byte* addr, int b)
        {
            WriteProcessMemory(m_hProcess, addr, (byte*)&b, 4, out _);
        }

        public void WriteUInt32(byte* addr, uint b)
        {
            WriteProcessMemory(m_hProcess, addr, (byte*)&b, 4, out _);
        }

        public void WriteStruct<T>(byte* addr, T s)
            where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));

            Log("Allocating {0} bytes for {1}", size, typeof(T).Name);
            var buf = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(s, buf, false);

            Log("Writing to 0x{0:X8}", (int)addr);
            WriteProcessMemory(m_hProcess, addr, (byte*)buf, (uint)size, out _);

            Marshal.FreeHGlobal(buf);
        }

        public void WriteStructArray<T>(byte* addr, T[] arr)
            where T : struct
        {
            var ssize = Marshal.SizeOf(typeof(T));
            var size = ssize * arr.Length;

            Log("Allocating {0} bytes for {1}[{2}]", size, typeof(T).Name, arr.Length);
            var buf = Marshal.AllocHGlobal(size);

            for(var i = 0; i < arr.Length; i++)
                Marshal.StructureToPtr(arr[i], buf + i * ssize, false);

            Log("Writing to 0x{0:X8}", (int)addr);
            WriteProcessMemory(m_hProcess, addr, (byte*)buf, (uint)size, out _);

            Marshal.FreeHGlobal(buf);
        }

        public void WriteBytes(byte* addr, byte[] bytes)
        {
            Log("Writing {0} bytes to 0x{1:X8}", bytes.Length, (int)addr);
            fixed (byte* ptr = bytes)
                WriteProcessMemory(m_hProcess, addr, ptr, (uint) bytes.Length, out _);
        }
    }
}
