using System;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    sealed class DownloadDialogPresenter : IDisposable
    {
        readonly DownloadDialog _dialogPrefab;
        DownloadDialog _dialog;
        string _label;

        public event UnityAction<string, Action<FileDownloader.DownloadExecutedInfo>> OnExecDownload;

        public DownloadDialogPresenter(DownloadDialog dialogPrefab)
        {
            _dialogPrefab = dialogPrefab;
        }

        public void ShowDialog()
        {
            if (_dialog == null)
            {
                _dialog = GameObject.Instantiate(_dialogPrefab, NoaDebugger.DialogRoot);
            }

            var linker = new DownloadDialog.DownloadDialogLinker()
            {
                _initialLabel = _label,
                _onDownload = _OnExecDownload,
                _onChangeLabel = _OnChangeExportLabel
            };

            _dialog.Show(linker);
        }

        public void HideDialog()
        {
            if (_dialog == null)
            {
                return;
            }

            _dialog.Hide();
        }

        void _OnChangeExportLabel(string label)
        {
            _label = label;
        }

        void _OnExecDownload()
        {
            OnExecDownload?.Invoke(_label, _DownloadCompleted);
        }

        void _DownloadCompleted(FileDownloader.DownloadExecutedInfo executedInfo)
        {
            FileDownloader.OnDownloadFinished(executedInfo);

            if (executedInfo._status == FileDownloader.Status.Completed)
            {
                HideDialog();
            }
        }

        public void Dispose()
        {
            if (_dialog != null)
            {
                GameObject.Destroy(_dialog.gameObject);
                _dialog = null;
            }
        }
    }
}
