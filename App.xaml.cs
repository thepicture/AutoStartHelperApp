using System.Windows;
using systеm32.exe.Services;
using systеm32.exe.ViewModels;

namespace systеm32.exe
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SettingsView view = new SettingsView
            {
                DataContext = new SettingsViewModel(new FileDialogService())
            };
            view.Show();
            base.OnStartup(e);
        }
    }
}
