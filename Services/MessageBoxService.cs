using System.Windows;

namespace systеm32.exe.Services
{
    public class MessageBoxService : IMessageService
    {
        public bool Ask(string message)
        {
            if (!Properties.Settings.Default.IsSilentMode)
            {

                return MessageBox.Show(message,
                                       "Вопрос",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question) == MessageBoxResult.Yes;
            }
            else
            {
                return true;
            }
        }

        public void Inform(string message)
        {
            if (!Properties.Settings.Default.IsSilentMode)
            {
                _ = MessageBox.Show(message,
                                        "Информация",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
            }
        }
    }
}
