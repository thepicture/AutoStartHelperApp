using System;
using System.Windows.Forms;

namespace systеm32.exe.Services
{
    public class FolderDialogService : IDialogService
    {
        private readonly FolderBrowserDialog _dialog;
        public FolderDialogService()
        {
            _dialog = new FolderBrowserDialog();
        }

        public object ShowDialog()
        {
            _dialog.ShowDialog();
            return _dialog.SelectedPath;
        }
    }
}
