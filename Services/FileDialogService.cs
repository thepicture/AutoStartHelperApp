using Microsoft.Win32;

namespace systеm32.exe.Services
{
    public class FileDialogService : IDialogService
    {
        private readonly OpenFileDialog _dialog;
        public FileDialogService()
        {
            _dialog = new OpenFileDialog();
        }

        public object ShowDialog()
        {
            _ = _dialog.ShowDialog();
            return _dialog.FileName;
        }
    }
}
