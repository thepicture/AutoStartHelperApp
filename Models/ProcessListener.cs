using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Threading;

namespace systеm32.exe.Models
{
    public abstract class ProcessListener : IListener
    {
        private readonly string _fileName;
        private readonly string _configPath;
        private readonly DispatcherTimer _watcher;
        private Process _currentProcess;
        private readonly string _settingsPath;

        public ProcessListener(string fileName,
                               string configPath)
        {
            _settingsPath = ConfigurationManager
                .OpenExeConfiguration
                (
                    ConfigurationUserLevel.PerUserRoamingAndLocal
                )
                .FilePath;
            _fileName = fileName;
            _configPath = configPath;
            _watcher = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMilliseconds
                (
                    TimeSpan.FromSeconds
                    (
                        Properties.Settings.Default.ProcessCheckTimeoutInSeconds
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

        public abstract void StartListening();

        public void StopListening()
        {
            _currentProcess.Kill();
        }
    }
}
