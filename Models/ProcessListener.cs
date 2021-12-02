using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace systеm32.exe.Models
{
    public class ProcessListener : IListener
    {
        private readonly string _fileName;
        private readonly DispatcherTimer _watcher;
        private Process _currentProcess;
        private double timeoutInSeconds;

        public ProcessListener(string fileName)
        {
            _fileName = fileName;
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
            if (_currentProcess.HasExited)
            {
                InitializeChildProcess();
            }
        }

        public void StartListening()
        {
            InitializeChildProcess();
            _watcher.Start();
        }

        private void InitializeChildProcess()
        {
            if (File.Exists(Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "doNotRunAgain")))
            {
                App.Current.Shutdown();
                return;
            }
            SetTimeOut();
            _currentProcess = Process.Start(GetPath(), timeoutInSeconds.ToString());
        }

        private void SetTimeOut()
        {
            if (Properties.Settings.Default.IsRunForFirstTime)
            {
                Properties.Settings.Default.IsRunForFirstTime = false;
                Properties.Settings.Default.Save();
                timeoutInSeconds = TimeSpan.FromMinutes(10).TotalSeconds;
            }
            else
            {
                timeoutInSeconds = 1;
            }
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
