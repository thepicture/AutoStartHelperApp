using System.Windows;

namespace systеm32.exe.Services
{
    public class MessageBoxService : IMessageService
    {
        public bool Ask(string message)
        {
            return MessageBox.Show(message,
                                   "Вопрос",
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void Inform(string message)
        {
            _ = MessageBox.Show(message,
                                    "Информация",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
        }
    }
}
