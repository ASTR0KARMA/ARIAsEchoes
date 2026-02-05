using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace NoaDebugger
{
    static partial class FileDownloader
    {
        public enum Status
        {
            None,
            Downloading,
            Completed,
            Canceled,
            Error
        }

        public struct DownloadExecutedInfo
        {
            public Status _status;
            public string _outputPath;
        }

        static Status _status = Status.None;
        static bool _isOpened;
        static string _outputPath;

        public static void DownloadFileWithUserCallbacks(
            string fileName, string textData, Action<DownloadExecutedInfo> onFinish, Action onCancelFromApi,
            INoaDownloadCallbacks userCallbacks, Func<string, string, bool> legacyOnDownload)
        {
            if (userCallbacks != null)
            {
                FileDownloader.DownloadFileWithUserCallbacksClass(fileName, textData, onFinish, onCancelFromApi, userCallbacks);

                return;
            }

            if (legacyOnDownload != null)
            {
                var isAllowBaseDownload = legacyOnDownload(fileName, textData);
                if (!isAllowBaseDownload)
                {
                    onCancelFromApi?.Invoke();
                    return;
                }
            }

            FileDownloader.DownloadFile(fileName, textData, null, onFinish);
        }

        public static void DownloadFileWithUserCallbacksClass(string fileName, string textData,
            Action<DownloadExecutedInfo> onFinish, Action onCancelFromApi, INoaDownloadCallbacks userCallbacks)
        {
            var downloadInfo = new NoaDownloadInfo()
            {
                FileName = fileName,
                Json = textData
            };

            var processedInfo = userCallbacks.OnBeforeDownload(downloadInfo);

            if (userCallbacks.IsAllowBaseDownload)
            {
                FileDownloader.DownloadFile(
                    processedInfo.FileName,
                    processedInfo.Json,
                    null,
                    onFinish,
                    info =>
                    {
                        var status = info._status == Status.Completed
                            ? NoaDownloadStatus.Succeeded
                            : NoaDownloadStatus.Failed;

                        userCallbacks.OnAfterDownload(processedInfo, status);
                    });
            }
            else
            {
                onCancelFromApi?.Invoke();
                userCallbacks.OnAfterDownload(processedInfo, NoaDownloadStatus.NotExecuted);
            }
        }

        public static void DownloadFile(string fileName, string textData, string mimeType,
            Action<DownloadExecutedInfo> onFinish, Action<DownloadExecutedInfo> onFinishFromApi = null)
        {
            if (!FileDownloader.CanDownload())
            {
                LogModel.LogWarning("This environment does not support the download feature.");
                return;
            }

            if (_isOpened)
            {
                throw new UnityException("The dialog is already displayed. Multiple dialogs cannot be started at the same time.");
            }
            _isOpened = true;
            GlobalCoroutine.Run(DownloadFileAsync(fileName, textData, mimeType, onFinish, onFinishFromApi));
        }

        static IEnumerator DownloadFileAsync(string fileName, string textData, string mimeType,
            Action<DownloadExecutedInfo> onFinish, Action<DownloadExecutedInfo> onFinishFromApi)
        {
            _status = Status.Downloading;

            DownloadFile(fileName, textData, mimeType);

            while (_status == Status.Downloading)
            {
                yield return null;
            }

            _isOpened = false;

            var info = new DownloadExecutedInfo()
            {
                _status = _status,
                _outputPath = _outputPath
            };

#if !UNITY_WEBGL || UNITY_EDITOR
            onFinish?.Invoke(info);
#endif

            onFinishFromApi?.Invoke(info);
        }

        static partial void DownloadFile(string fileName, string textData, string mimeType);

        static string GetMimeType(string extension)
        {
            string mimeType;
            switch (extension)
            {
                case "txt":
                    mimeType = "text/plain;charset=utf-8";
                    break;
                case "json":
                    mimeType = "application/json";
                    break;
                default:
                    _status = Status.Error;
                    mimeType = "";
                    break;
            }
            return mimeType;
        }

        public static void OnDownloadFinished(DownloadExecutedInfo executedInfo)
        {
            string label = executedInfo._status switch
            {
                Status.Completed => NoaDebuggerDefine.DownloadCompletedText,
                Status.Canceled => NoaDebuggerDefine.DownloadCanceledText,
                _ => NoaDebuggerDefine.DownloadFailedText
            };
            var linker = new ToastViewLinker
            {
                _label = label
            };
            NoaDebugger.ShowToast(linker);

#if UNITY_EDITOR
            if (executedInfo._status == Status.Completed)
            {
                var dirName = System.IO.Path.GetDirectoryName(executedInfo._outputPath);
                Application.OpenURL($"file://{dirName}");
            }
#endif
        }

        public static bool CanDownload()
        {
            bool isSupportedEnvironment = Application.platform == RuntimePlatform.Android ||
                                          Application.platform == RuntimePlatform.IPhonePlayer ||
                                          Application.platform == RuntimePlatform.WindowsPlayer ||
                                          Application.platform == RuntimePlatform.WebGLPlayer ||
                                          Application.platform == RuntimePlatform.OSXEditor ||
                                          Application.platform == RuntimePlatform.WindowsEditor;

#if USE_XR
            bool isXRSettingsActive = XRSettings.enabled &&
                                      XRSettings.loadedDeviceName != "None" &&
                                      !string.IsNullOrEmpty(XRSettings.loadedDeviceName);
#else
            const bool isXRSettingsActive = false;
#endif

            return isSupportedEnvironment && !isXRSettingsActive;
        }
    }
}
