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
        private string firstRunArgs;
        private string secondRunArgs;
        private string configPath;
        private int firstRunTimeoutInSeconds;
        private int secondRunTimeoutInSeconds;
        private int processCheckTimeoutInSeconds;
        private ICommand runFileWatcherCommand;
        private ICommand selectFileCommand;
        private ICommand selectConfigCommand;
        private readonly IDialogService dialogService;
        private readonly IMessageService messageService;
        private readonly IDialogService folderService;
        private readonly IWriter writer;
        private IListener listener;
        private bool isRunForFirstTime;
        private bool doNotRunAgain;
        private bool isNotBackgroundProcess;
        private bool isServer;
        private bool isSilentMode;

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
                    runFileWatcherCommand = new RelayCommand(RunFileWatcher);
                }
                return runFileWatcherCommand;
            }
        }

        private void RunFileWatcher(object commandParameter = null)
        {
            if (!messageService.Ask("Если значения не были сохранены, " +
                "то при запуске они вернутся на старые значения. " +
                "Приложение станет клиентом. " +
                "Нажмите да, чтобы запустить программу " +
                "с данными условиями"))
            {
                return;
            }
            IsServer = false;
            Properties.Settings.Default.IsServer = IsServer;
            Properties.Settings.Default.Save();

            try
            {
                new AutoStartSettler().Set();
                listener = new ProcessListener();
                listener.StartListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public SettingsViewModel(IDialogService dialogService,
                                 IMessageService messageService,
                                 IDialogService folderService)
        {
            this.dialogService = dialogService;
            this.messageService = messageService;
            this.folderService = folderService;
            writer = new SharedConfigWriter();
            Title = "Настройка автозапуска";

            FilePath = Properties.Settings.Default.FilePath;
            FirstRunTimeoutInSeconds = Properties.Settings.Default.FirstRunTimeoutInSeconds;
            SecondRunTimeoutInSeconds = Properties.Settings.Default.SecondRunTimeoutInSeconds;
            FirstRunArgs = Properties.Settings.Default.FirstRunArgs;
            SecondRunArgs = Properties.Settings.Default.SecondRunArgs;
            IsRunForFirstTime = Properties.Settings.Default.IsRunForFirstTime;
            ProcessCheckTimeoutInSeconds = Properties.Settings.Default.ProcessCheckTimeoutInSeconds;
            DoNotRunAgain = Properties.Settings.Default.DoNotRunAgain;
            ConfigPath = Properties.Settings.Default.ConfigPath;
            IsServer = Properties.Settings.Default.IsServer;
            IsSilentMode = Properties.Settings.Default.IsSilentMode;

            IsNotBackgroundProcess = IsServer;

            if (!Properties.Settings.Default.IsRunForFirstTime
                && !Properties.Settings.Default.DoNotRunAgain
                && !IsServer)
            {
                RunFileWatcher();
            }
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

            string path = (string)dialogService.ShowDialog();
            if (string.IsNullOrWhiteSpace(path))
            {
                messageService.Inform("Выбор файла был отменён");
            }
            else
            {
                FilePath = path;
            }
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

        public bool IsNotBackgroundProcess
        {
            get => isNotBackgroundProcess; set
            {
                isNotBackgroundProcess = value;
                OnPropertyChanged();
            }
        }

        public string ConfigPath
        {
            get => configPath; set
            {
                configPath = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectConfigCommand
        {
            get
            {
                if (selectConfigCommand == null)
                {
                    selectConfigCommand = new RelayCommand(SelectConfig);
                }
                return selectConfigCommand;
            }
        }

        public bool IsServer
        {
            get => isServer; set
            {
                isServer = value;
                OnPropertyChanged();
            }
        }

        public bool IsSilentMode
        {
            get => isSilentMode; set
            {
                isSilentMode = value;
                OnPropertyChanged();
            }
        }

        private void SelectConfig(object obj)
        {
            string path = (string)folderService.ShowDialog();
            if (string.IsNullOrWhiteSpace(path))
            {
                messageService.Inform("Выбор папки для " +
                    "общего файла " +
                    "конфигурации " +
                    "был отменён");
            }
            else
            {
                ConfigPath = path;
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
            Properties.Settings.Default.ConfigPath = ConfigPath;
            Properties.Settings.Default.IsServer = IsServer;
            Properties.Settings.Default.IsSilentMode = IsSilentMode;
            Properties.Settings.Default.Save();
            if (IsServer)
            {
                writer.Write(ConfigPath);
            }
        }
    }
}
