using System;
using System.Windows.Input;
using systеm32.exe.Commands;
using systеm32.exe.Models;
using systеm32.exe.Services;

namespace systеm32.exe.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string filePath;
        private int firstRunTimeoutInSeconds;
        private int secondRunTimeoutInSeconds;
        private string firstRunArgs;
        private string secondRunArgs;
        private ICommand runFileWatcherCommand;
        private RelayCommand selectFileCommand;
        private readonly IDialogService dialogService;
        private IListener listener;

        public string FilePath
        {
            get
            {
                filePath = Properties.Settings.Default.FilePath;
                return filePath;
            }

            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }
        public int FirstRunTimeoutInSeconds
        {
            get
            {
                firstRunTimeoutInSeconds = Properties.Settings.Default.FirstRunTimeoutInSeconds;
                return firstRunTimeoutInSeconds;
            }

            set
            {
                firstRunTimeoutInSeconds = value;
                OnPropertyChanged();
            }
        }
        public int SecondRunTimeoutInSeconds
        {
            get
            {
                secondRunTimeoutInSeconds = Properties.Settings.Default.SecondRunTimeoutInSeconds;
                return secondRunTimeoutInSeconds;
            }

            set
            {
                secondRunTimeoutInSeconds = value;
                OnPropertyChanged();
            }
        }
        public string FirstRunArgs
        {
            get
            {
                firstRunArgs = Properties.Settings.Default.FirstRunArgs;
                return firstRunArgs;
            }

            set
            {
                firstRunArgs = value;
                OnPropertyChanged();
            }
        }
        public string SecondRunArgs
        {
            get
            {
                secondRunArgs = Properties.Settings.Default.SecondRunArgs;
                return secondRunArgs;
            }

            set
            {
                secondRunArgs = value;
                OnPropertyChanged();
            }
        }

        public ICommand RunFileWatcherCommand
        {
            get
            {
                if (runFileWatcherCommand == null)
                {
                    runFileWatcherCommand = new RelayCommand(RunFileWatcher, CanRunFileWatcherExecute);
                }
                return runFileWatcherCommand;
            }
        }

        private bool CanRunFileWatcherExecute(object arg)
        {
            return FilePath != null;
        }

        private void RunFileWatcher(object commandParameter)
        {
            try
            {
                listener = new ProcessListener(FilePath,
                                                Properties.Settings.Default.IsRunForFirstTime
                                                ? TimeSpan.FromSeconds(Properties.Settings.Default.FirstRunTimeoutInSeconds)
                                                : TimeSpan.FromSeconds(Properties.Settings.Default.SecondRunTimeoutInSeconds));
                new AutoStartSettler().Set();
                listener.StartListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public SettingsViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            Title = "Настройка автозапуска";
        }

        public ICommand SelectFileCommand
        {
            get
            {
                if (selectFileCommand == null)
                {
                    selectFileCommand = new RelayCommand(SelectFile);
                }

                return selectFileCommand;
            }
        }

        private void SelectFile(object commandParameter)
        {
            FilePath = (string)dialogService.ShowDialog();
        }

        private RelayCommand saveValuesCommand;

        public ICommand SaveValuesCommand
        {
            get
            {
                if (saveValuesCommand == null)
                {
                    saveValuesCommand = new RelayCommand(SaveValues);
                }

                return saveValuesCommand;
            }
        }

        private void SaveValues(object commandParameter)
        {
            Properties.Settings.Default.FilePath = FilePath;
            Properties.Settings.Default.FirstRunTimeoutInSeconds = FirstRunTimeoutInSeconds;
            Properties.Settings.Default.SecondRunTimeoutInSeconds = SecondRunTimeoutInSeconds;
            Properties.Settings.Default.FirstRunArgs = FirstRunArgs;
            Properties.Settings.Default.SecondRunArgs = SecondRunArgs;
            Properties.Settings.Default.Save();
        }
    }
}
