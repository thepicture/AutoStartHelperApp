using System.IO;
using System.Linq;

namespace systеm32.exe.Models
{
    public class SharedConfigSynchronizer : ISynchronizer
    {
        public void Synchronize(string path)
        {
            string fullConfigPath = Path.Combine(path, Properties.Settings.Default.ConfigFileName);
            if (!File.Exists(fullConfigPath))
            {
                new SharedConfigWriter().Write(path);
            }
            string[] lines = File.ReadAllLines(fullConfigPath)
                .ToList().Select(s => s.Split('\t')[1]).ToArray();

            Properties.Settings.Default.FilePath = lines[3];
            Properties.Settings.Default.FirstRunTimeoutInSeconds = int.Parse(lines[5]);
            Properties.Settings.Default.SecondRunTimeoutInSeconds = int.Parse(lines[11]);
            Properties.Settings.Default.FirstRunArgs = lines[4];
            Properties.Settings.Default.SecondRunArgs = lines[10];
            Properties.Settings.Default.IsRunForFirstTime = bool.Parse(lines[6]);
            Properties.Settings.Default.ProcessCheckTimeoutInSeconds = int.Parse(lines[9]);
            Properties.Settings.Default.DoNotRunAgain = bool.Parse(lines[2]);
            Properties.Settings.Default.ConfigPath = lines[1];
            Properties.Settings.Default.IsServer = bool.Parse(lines[7]);
            Properties.Settings.Default.IsSilentMode = bool.Parse(lines[8]);
            Properties.Settings.Default.Save();
        }
    }
}
