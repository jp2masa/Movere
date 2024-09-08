using System;
using System.IO;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace Movere.Win32.Services
{
    internal sealed class Win32Stream(Stream stream) : IStream.Interface
    {
        private readonly Stream _stream = stream;

        public unsafe HRESULT Seek(long dlibMove, SeekOrigin dwOrigin, ulong* plibNewPosition = default)
        {
            var _origin = dwOrigin switch
            {
                SeekOrigin.Begin =>
                    SeekOrigin.Begin,
                SeekOrigin.End =>
                    SeekOrigin.End,
                SeekOrigin.Current =>
                    SeekOrigin.Current,
                _ =>
                    default(SeekOrigin?)
            };

            if (_origin is not { } origin)
            {
                return HRESULT.STG_E_INVALIDFUNCTION;
            }

            try
            {
                var newPosition = _stream.Seek(dlibMove, origin);

                if (plibNewPosition != null)
                {
                    *plibNewPosition = (ulong)newPosition;
                }

                return HRESULT.S_OK;
            }
            catch
            {
                return HRESULT.STG_E_INVALIDFUNCTION;
            }
        }

        public HRESULT SetSize(ulong libNewSize)
        {
            try
            {
                _stream.SetLength((long)libNewSize);
                return HRESULT.S_OK;
            }
            catch
            {
                return HRESULT.STG_E_INVALIDFUNCTION;
            }
        }

        public unsafe HRESULT CopyTo(IStream* pstm, ulong cb, ulong* pcbRead = default, ulong* pcbWritten = default) => throw new NotImplementedException();

        public HRESULT Commit(uint grfCommitFlags) =>
            HRESULT.S_OK;

        public HRESULT Revert() =>
            HRESULT.S_OK;

        public HRESULT LockRegion(ulong libOffset, ulong cb, uint dwLockType) => throw new NotImplementedException();

        public HRESULT UnlockRegion(ulong libOffset, ulong cb, uint dwLockType) => throw new NotImplementedException();

        public unsafe HRESULT Stat(STATSTG* pstatstg, uint grfStatFlag) => throw new NotImplementedException();

        public unsafe HRESULT Clone(IStream** ppstm) => throw new NotImplementedException();

        public unsafe HRESULT Read(void* pv, uint cb, uint* pcbRead)
        {
            try
            {
                return (*pcbRead = (uint)_stream.Read(new Span<byte>(pv, (int)cb))) < cb
                    ? HRESULT.S_FALSE
                    : HRESULT.S_OK;
            }
            catch
            {
                return HRESULT.STG_E_ACCESSDENIED;
            }
        }

        public unsafe HRESULT Write(void* pv, uint cb, uint* pcbWritten)
        {
            try
            {
                _stream.Write(new Span<byte>(pv, (int)cb));

                if (pcbWritten != null)
                {
                    *pcbWritten = cb;
                }

                return HRESULT.S_OK;
            }
            catch
            {
                return HRESULT.STG_E_ACCESSDENIED;
            }
        }

        unsafe HRESULT ISequentialStream.Interface.Read(void* pv, uint cb, uint* pcbRead) =>
            Read(pv, cb, pcbRead);

        unsafe HRESULT ISequentialStream.Interface.Write(void* pv, uint cb, uint* pcbWritten) =>
            Write(pv, cb, pcbWritten);
    }
}
