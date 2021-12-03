using System.IO;

namespace systеm32.exe.Models
{
    public class ServerWriter : IWriter
    {
        public void Write(string destination)
        {
            File.WriteAllText(destination, Properties.Settings.Default.ConfigPath);
        }
    }
}
