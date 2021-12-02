namespace systеm32.exe.Services
{
    public interface IMessageService
    {
        bool Ask(string message);
        void Inform(string message);
    }
}
