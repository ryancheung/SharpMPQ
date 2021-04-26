// MIT License - Copyright (C) ryancheung
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpMPQ
{
    public unsafe static class NativeMethods
    {
        private static readonly List<string> MPQLibrarySearchPaths = new();
        public const string MPQLibraryName = "MPQ";
        public static readonly string MPQLibrarySuffix;

        static NativeMethods()
        {
            if (OperatingSystem.IsWindows())
                MPQLibrarySuffix = ".dll";
            else if (OperatingSystem.IsMacOS())
                MPQLibrarySuffix = ".dylib";
            else if (OperatingSystem.IsLinux())
                MPQLibrarySuffix = ".so";
            else
                MPQLibrarySuffix = ".so";

            var appDir = AppContext.BaseDirectory;

            MPQLibrarySearchPaths.Add(appDir);

            if (OperatingSystem.IsWindows())
            {
                string arch = Environment.Is64BitProcess ? "win-x64" : "win-x86";
                MPQLibrarySearchPaths.Add(Path.Combine(appDir, "runtimes", arch, "native"));
                MPQLibrarySearchPaths.Add(Path.Combine(appDir, Environment.Is64BitProcess ? "x64" : "x86"));
            }
            else
            {
                string arch = OperatingSystem.IsMacOS() ? "osx" : "linux-" + (Environment.Is64BitProcess ? "x64" : "x86");
                MPQLibrarySearchPaths.Add(Path.Combine(appDir, "runtimes", arch, "native"));
                MPQLibrarySearchPaths.Add("/usr/lib");
                MPQLibrarySearchPaths.Add("/usr/local/lib");
            }

            NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, ImportResolver);
        }

        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != MPQLibraryName) return default;

            // Correct libraryName dependent on platforms
            if (OperatingSystem.IsWindows())
                libraryName = "mpq";
            else if (OperatingSystem.IsMacOS())
                libraryName = "libmpq";
            else if (OperatingSystem.IsLinux())
                libraryName = "libmpq";
            else
                libraryName = "libmpq";

            IntPtr handle;
            var success = NativeLibrary.TryLoad(libraryName, typeof(NativeMethods).Assembly,
                DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories | DllImportSearchPath.UseDllDirectoryForDependencies,
                out handle);

            foreach (var path in MPQLibrarySearchPaths)
            {
                success = NativeLibrary.TryLoad(Path.Combine(path, $"{libraryName}{MPQLibrarySuffix}"), out handle);
                if (success)
                    break;
            }

            return handle;
        }

        [DllImport(MPQLibraryName, CallingConvention = CallingConvention.StdCall, EntryPoint = "libmpq__version")]
        private static extern IntPtr _libmpq__version();
        public static string? libmpq__version()
        {
            return Marshal.PtrToStringAnsi(_libmpq__version());
        }

        /* string error message for a libmpq return code. */
        [DllImport(MPQLibraryName, CallingConvention = CallingConvention.StdCall, EntryPoint = "libmpq__strerror")]
        private static extern IntPtr _libmpq__strerror(int return_code);
        public static string? libmpq__strerror(int return_code)
        {
            return Marshal.PtrToStringAnsi(_libmpq__strerror(return_code));
        }
    }
}
