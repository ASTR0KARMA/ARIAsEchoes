#if UNITY_WEBGL && !UNITY_EDITOR

using System.IO;
using System.Runtime.InteropServices;

namespace NoaDebugger
{
    static partial class FileDownloader
    {
        static partial void DownloadFile(string fileName, string textData, string mimeType)
        {
            string extension = Path.GetExtension(fileName).Replace(".", "");

            if (string.IsNullOrEmpty(mimeType))
            {
                mimeType =  FileDownloader.GetMimeType(extension);

                if (string.IsNullOrEmpty(mimeType))
                {
                    return;
                }
            }
            NoaDebuggerDownloadFile(fileName, textData, mimeType);
            _status = Status.Completed;
        }

        [DllImport("__Internal")]
        static extern void NoaDebuggerDownloadFile(string fileName, string textData, string mimeType);
    }
}

#endif
