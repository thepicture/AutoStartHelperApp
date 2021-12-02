using System;
using System.IO;
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
        private ICommand selectFileCommand;
        private readonly IDialogService dialogService;
        private readonly IMessageService messageService;
        private IListener listener;
        private bool isRunForFirstTime;
        private int processCheckTimeoutInSeconds;
        private bool doNotRunAgain;
        private bool asBackgroundProcess = false;

        public string FilePath
        {
            get => filePath;

            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }
        public int FirstRunTimeoutInSeconds
        {
            get => firstRunTimeoutInSeconds;

            set
            {
                firstRunTimeoutInSeconds = value;
                OnPropertyChanged();
            }
        }
        public int SecondRunTimeoutInSeconds
        {
            get => secondRunTimeoutInSeconds;

            set
            {
                secondRunTimeoutInSeconds = value;
                OnPropertyChanged();
            }
        }
        public string FirstRunArgs
        {
            get => firstRunArgs;

            set
            {
                firstRunArgs = value;
                OnPropertyChanged();
            }
        }
        public string SecondRunArgs
        {
            get => secondRunArgs;

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
            if (!File.Exists(FilePath))
            {
                messageService.Inform("Файл для запуска не найден по указанному пути");
                return;
            }
            if (!messageService.Ask("Если значения не были сохранены, " +
                "то при запуске они вернутся на старые значения. " +
                "Нажмите да, чтобы запустить программу " +
                "с данным условием"))
            {
                return;
            }
            try
            {
                listener = new ProcessListener(FilePath);
                new AutoStartSettler().Set();
                listener.StartListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public SettingsViewModel(IDialogService dialogService, IMessageService messageService)
        {
            this.dialogService = dialogService;
            this.messageService = messageService;
            Title = "Настройка автозапуска";

            filePath = Properties.Settings.Default.FilePath;
            firstRunTimeoutInSeconds = Properties.Settings.Default.FirstRunTimeoutInSeconds;
            secondRunTimeoutInSeconds = Properties.Settings.Default.SecondRunTimeoutInSeconds;
            firstRunArgs = Properties.Settings.Default.FirstRunArgs;
            secondRunArgs = Properties.Settings.Default.SecondRunArgs;
            IsRunForFirstTime = Properties.Settings.Default.IsRunForFirstTime;
            ProcessCheckTimeoutInSeconds = Properties.Settings.Default.ProcessCheckTimeoutInSeconds;
            DoNotRunAgain = Properties.Settings.Default.DoNotRunAgain;
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

        public bool IsRunForFirstTime
        {
            get => isRunForFirstTime; set
            {
                isRunForFirstTime = value;
                OnPropertyChanged();
            }
        }

        public int ProcessCheckTimeoutInSeconds
        {
            get => processCheckTimeoutInSeconds; set
            {
                processCheckTimeoutInSeconds = value;
                OnPropertyChanged();
            }
        }

        public bool DoNotRunAgain
        {
            get => doNotRunAgain; set
            {
                doNotRunAgain = value;
                OnPropertyChanged();
            }
        }

        public bool AsBackgroundProcess
        {
            get => asBackgroundProcess; set
            {
                asBackgroundProcess = value;
                OnPropertyChanged();
            }
        }

        private void SaveValues(object commandParameter)
        {
            if (!messageService.Ask("Точно сохранить новые значения?"))
            {
                return;
            }
            Properties.Settings.Default.FilePath = FilePath;
            Properties.Settings.Default.FirstRunTimeoutInSeconds = FirstRunTimeoutInSeconds;
            Properties.Settings.Default.SecondRunTimeoutInSeconds = SecondRunTimeoutInSeconds;
            Properties.Settings.Default.FirstRunArgs = FirstRunArgs;
            Properties.Settings.Default.SecondRunArgs = SecondRunArgs;
            Properties.Settings.Default.IsRunForFirstTime = IsRunForFirstTime;
            Properties.Settings.Default.ProcessCheckTimeoutInSeconds = ProcessCheckTimeoutInSeconds;
            Properties.Settings.Default.DoNotRunAgain = DoNotRunAgain;
            Properties.Settings.Default.Save();
        }
    }
}
