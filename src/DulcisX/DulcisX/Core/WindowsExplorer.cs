using DulcisX.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace DulcisX.Core
{
    /// <summary>
    /// Add credits https://gist.github.com/vbfox/551626
    /// </summary>
    public static class WindowsExplorer
    {
        private static Guid IID_IShellFolder = typeof(IShellFolder).GUID;

        public static void OpenAndSelect(IPhysicalNode node, bool edit = false)
            => OpenAndSelect(node.GetFullName(), edit);

        public static void OpenAndSelect(string fullName, bool edit = false)
        {
            if (fullName == null) throw new ArgumentNullException(nameof(fullName));

            var pidl = PathToAbsolutePIDL(fullName);
            try
            {
                SHOpenFolderAndSelectItems(pidl, null, edit);
            }
            finally
            {
                NativeMethods.ILFree(pidl);
            }
        }

        public static void OpenAndSelect(string parentDirectory, ICollection<IPhysicalNode> nodes)
            => OpenAndSelect(parentDirectory, nodes.Select(x=> x.GetFullName()).ToList());

        public static void OpenAndSelect(IPhysicalNode parent, ICollection<IPhysicalNode> nodes)
            => OpenAndSelect(parent.GetDirectoryName(), nodes.Select(x => x.GetFullName()).ToList());

        public static void OpenAndSelect(IPhysicalNode parent, ICollection<string> fileNames)
            => OpenAndSelect(parent.GetDirectoryName(), fileNames);

        public static void OpenAndSelect(string parentDirectory, ICollection<string> fileNames)
        {
            if (fileNames == null) throw new ArgumentNullException(nameof(fileNames));
            if (fileNames.Count == 0) return;

            var parentPidl = PathToAbsolutePIDL(parentDirectory);
            try
            {
                var parent = PIDLToShellFolder(parentPidl);
                var filesPidl = fileNames
                    .Select(filename => GetShellFolderChildrenRelativePIDL(parent, filename))
                    .ToArray();

                try
                {
                    SHOpenFolderAndSelectItems(parentPidl, filesPidl, false);
                }
                finally
                {
                    foreach (var pidl in filesPidl)
                    {
                        NativeMethods.ILFree(pidl);
                    }
                }
            }
            finally
            {
                NativeMethods.ILFree(parentPidl);
            }
        }

        [Flags]
        private enum SHCONT : ushort
        {
            CHECKING_FOR_CHILDREN = 0x0010,
            FOLDERS = 0x0020,
            NONFOLDERS = 0x0040,
            INCLUDEHIDDEN = 0x0080,
            INIT_ON_FIRST_NEXT = 0x0100,
            NETPRINTERSRCH = 0x0200,
            SHAREABLE = 0x0400,
            STORAGE = 0x0800,
            NAVIGATION_ENUM = 0x1000,
            FASTITEMS = 0x2000,
            FLATLIST = 0x4000,
            ENABLE_ASYNC = 0x8000
        }

        [ComImport,
        Guid("000214E6-0000-0000-C000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComConversionLoss]
        private interface IShellFolder
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void ParseDisplayName(IntPtr hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [Out] out uint pchEaten, [Out] out IntPtr ppidl, [In, Out] ref uint pdwAttributes);

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int EnumObjects([In] IntPtr hwnd, [In] SHCONT grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int BindToObject([In] IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void BindToStorage([In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out IntPtr ppv);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref uint rgfInOut);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out IntPtr ppv);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetDisplayNameOf([In] ref IntPtr pidl, [In] uint uFlags, out IntPtr pName);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] IntPtr ppidlOut);
        }

        [ComImport,
        Guid("000214F2-0000-0000-C000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IEnumIDList
        {
            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Next(uint celt, IntPtr rgelt, out uint pceltFetched);

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Skip([In] uint celt);

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Reset();

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
        }

        private static class NativeMethods
        {
            [DllImport("shell32.dll", EntryPoint = "SHGetDesktopFolder", CharSet = CharSet.Unicode,
                SetLastError = true)]
            static extern int SHGetDesktopFolder_([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppshf);

            public static IShellFolder SHGetDesktopFolder()
            {
                Marshal.ThrowExceptionForHR(SHGetDesktopFolder_(out var result));

                return result;
            }

            [DllImport("shell32.dll", EntryPoint = "SHOpenFolderAndSelectItems")]
            static extern int SHOpenFolderAndSelectItems_(
                [In] IntPtr pidlFolder, uint cidl, [In, Optional, MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl,
                int dwFlags);

            public static void SHOpenFolderAndSelectItems(IntPtr pidlFolder, IntPtr[] apidl, int dwFlags)
            {
                var cidl = (apidl != null) ? (uint)apidl.Length : 0U;
                var result = SHOpenFolderAndSelectItems_(pidlFolder, cidl, apidl, dwFlags);
                Marshal.ThrowExceptionForHR(result);
            }

            [DllImport("shell32.dll")]
            public static extern void ILFree([In] IntPtr pidl);
        }

        private static IntPtr GetShellFolderChildrenRelativePIDL(IShellFolder parentFolder, string displayName)
        {
            uint pdwAttributes = 0;

            parentFolder.ParseDisplayName(IntPtr.Zero, null, displayName, out _, out var ppidl, ref pdwAttributes);

            return ppidl;
        }

        private static IntPtr PathToAbsolutePIDL(string path)
        {
            var desktopFolder = NativeMethods.SHGetDesktopFolder();
            return GetShellFolderChildrenRelativePIDL(desktopFolder, path);
        }

        private static IShellFolder PIDLToShellFolder(IShellFolder parent, IntPtr pidl)
        {
            var result = parent.BindToObject(pidl, null, ref IID_IShellFolder, out var folder);

            Marshal.ThrowExceptionForHR(result);

            return folder;
        }

        private static IShellFolder PIDLToShellFolder(IntPtr pidl)
            => PIDLToShellFolder(NativeMethods.SHGetDesktopFolder(), pidl);

        private static void SHOpenFolderAndSelectItems(IntPtr pidlFolder, IntPtr[] apidl, bool edit)
        {
            NativeMethods.SHOpenFolderAndSelectItems(pidlFolder, apidl, edit ? 1 : 0);
        }
    }
}
