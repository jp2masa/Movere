using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Imaging;
using Windows.Win32.System.Com;
using Windows.Win32.UI.WindowsAndMessaging;

using Movere.Services;

namespace Movere.Win32.Services
{
    [SupportedOSPlatform("windows5.1.2600")]
    internal sealed class FileIconProvider : IFileIconProvider
    {
        public unsafe IFileIcon? GetFileIcon(string filePath)
        {
            var piIconIndex = default(ushort);
            var piIconId = default(ushort);

            HICON hIcon;

            var filePathSpanLength = Math.Max((int)PInvoke.MAX_PATH, filePath.Length + 1);

            static void CopyFilePath(string filePath, Span<char> filePathSpan)
            {
                filePath.CopyTo(filePathSpan);
                filePathSpan[filePath.Length] = '\0';
            }

            if (filePath.Length < 256)
            {
                Span<char> filePathSpan = stackalloc char[filePathSpanLength];
                CopyFilePath(filePath, filePathSpan);

                fixed (char* pszIconPath = filePathSpan)
                {
                    hIcon = PInvoke.ExtractAssociatedIconEx(HINSTANCE.Null, pszIconPath, &piIconIndex, &piIconId);
                }
            }
            else
            {
                var filePathBuf = ArrayPool<char>.Shared.Rent(filePathSpanLength);

                try
                {
                    var filePathSpan = filePathBuf.AsSpan(0, filePathSpanLength);
                    CopyFilePath(filePath, filePathSpan);

                    hIcon = (HICON)PInvoke.ExtractAssociatedIconEx(ref filePathSpan, ref piIconIndex, ref piIconId).DangerousGetHandle();
                }
                finally
                {
                    ArrayPool<char>.Shared.Return(filePathBuf);
                }
            }

            return hIcon.IsNull
                ? null
                : new FileIcon(hIcon);
        }
    }

    [SupportedOSPlatform("windows5.1.2600")]
    internal sealed class FileIcon(HICON hIcon) : IFileIcon
    {
        public unsafe void Save(Stream stream)
        {
            AssertSuccess(
                PInvoke.CoCreateInstance(
                    PInvoke.CLSID_WICImagingFactory,
                    null,
                    CLSCTX.CLSCTX_INPROC_SERVER,
                    out IWICImagingFactory* factory
                )
            );

            IWICBitmap* ppIWicBitmap;

            AssertSuccess(factory->CreateBitmapFromHICON(hIcon, &ppIWicBitmap));

            IWICBitmapEncoder* encoder;

            AssertSuccess(factory->CreateEncoder(PInvoke.GUID_ContainerFormatPng, Unsafe.AsRef<Guid>(null), &encoder));

            var win32Stream = new Win32Stream(stream);

            var pUnknown = (IUnknown*)Marshal.GetIUnknownForObject(win32Stream);

            AssertSuccess(pUnknown->QueryInterface(typeof(IStream).GUID, out var _pIStream));
            var pIStream = (IStream*)_pIStream;

            AssertSuccess(encoder->Initialize(pIStream, WICBitmapEncoderCacheOption.WICBitmapEncoderNoCache));

            IWICBitmapFrameEncode* frameEncoder;
            AssertSuccess(encoder->CreateNewFrame(&frameEncoder, null));

            AssertSuccess(frameEncoder->Initialize(null));

            AssertSuccess(ppIWicBitmap->GetSize(out var width, out var height));
            AssertSuccess(frameEncoder->SetSize(width, height));

            AssertSuccess(ppIWicBitmap->GetResolution(out var dpiX, out var dpiY));

            if (dpiX == 0 || dpiY == 0)
            {
                dpiX = dpiY = 96.0;
            }

            AssertSuccess(frameEncoder->SetResolution(dpiX, dpiY));

            AssertSuccess(ppIWicBitmap->GetPixelFormat(out var pixelFormat));
            AssertSuccess(frameEncoder->SetPixelFormat(ref pixelFormat));
            
            var rect = new WICRect() { X = 0, Y = 0, Width = (int)width, Height = (int)height };

            AssertSuccess(ppIWicBitmap->QueryInterface(typeof(IWICBitmapSource).GUID, out var ppWicBitmapSource));
            AssertSuccess(frameEncoder->WriteSource((IWICBitmapSource*)ppWicBitmapSource, in rect));

            AssertSuccess(frameEncoder->Commit());

            AssertSuccess(encoder->Commit());

            static void AssertSuccess(HRESULT hResult)
            {
                if (!hResult.Succeeded)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() =>
            PInvoke.DestroyIcon(hIcon);
    }
}
