using System.IO;

namespace systеm32.exe.Models
{
    public class PropertiesParser : IParser
    {
        public void Parse()
        {
            string[] lines = File.ReadAllLines(Properties.Settings.Default.ConfigPath);
            Properties.Settings.Default.FilePath = lines[0];
            Properties.Settings.Default.FirstRunTimeoutInSeconds = int.Parse(lines[1]);
            Properties.Settings.Default.SecondRunTimeoutInSeconds = int.Parse(lines[2]);
            Properties.Settings.Default.FirstRunArgs = lines[3];
            Properties.Settings.Default.SecondRunArgs = lines[4];
            Properties.Settings.Default.IsRunForFirstTime = bool.Parse(lines[5]);
            Properties.Settings.Default.ProcessCheckTimeoutInSeconds = int.Parse(lines[6]);
            Properties.Settings.Default.DoNotRunAgain = bool.Parse(lines[7]);
            Properties.Settings.Default.ConfigPath = lines[8];
            Properties.Settings.Default.IsServer = bool.Parse(lines[9]);
            Properties.Settings.Default.IsSilentMode = bool.Parse(lines[10]);
            Properties.Settings.Default.Save();
        }
    }
}
