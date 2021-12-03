using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace systеm32.exe.Models
{
    public class ProcessListener : IListener
    {
        private readonly string filePath;
        private readonly string configPath;
        private readonly DispatcherTimer watcher;
        private Process _currentProcess;
        private readonly ISynchronizer synchronizer;
        private bool isInitializing = false;

        public ProcessListener(string filePath,
                               string configPath)
        {
            this.filePath = filePath;
            this.configPath = configPath;
            synchronizer = new SharedConfigSynchronizer();
            watcher = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMilliseconds
                (
                    TimeSpan.FromSeconds
                    (
                        Properties.Settings.Default.ProcessCheckTimeoutInSeconds
                    ).TotalMilliseconds
                )
            };
            watcher.Tick += KeepChildProcess;
        }

        private async void KeepChildProcess(object sender, EventArgs e)
        {
            if (isInitializing)
            {
                return;
            }
            synchronizer.Synchronize(configPath);
            if (Properties.Settings.Default.DoNotRunAgain)
            {
                return;
            }
            if (_currentProcess.HasExited)
            {
                await InitializeChildProcess();
            }
        }

        public async void StartListening()
        {
            await InitializeChildProcess();
            watcher.Start();
        }

        private async Task InitializeChildProcess()
        {
            isInitializing = true;
            if (Properties.Settings.Default.IsRunForFirstTime)
            {
                Properties.Settings.Default.IsRunForFirstTime = false;
                Properties.Settings.Default.Save();
                new SharedConfigWriter().Write(configPath);
                await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.FirstRunTimeoutInSeconds));
                _currentProcess = Process.Start(filePath, Properties.Settings.Default.FirstRunArgs);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.SecondRunTimeoutInSeconds));
                _currentProcess = Process.Start(filePath, Properties.Settings.Default.SecondRunArgs);
            }
            isInitializing = false;
        }

        public void StopListening()
        {
            _currentProcess.Kill();
        }
    }
}
