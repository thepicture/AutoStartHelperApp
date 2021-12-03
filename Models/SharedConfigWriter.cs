using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace systеm32.exe.Models
{
    public class SharedConfigWriter : IWriter
    {
        public void Write(string destination)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SettingsProperty property in Properties.Settings.Default.Properties.Cast<SettingsProperty>().OrderBy(p => p.Name))
            {
                _ = stringBuilder.Append(property.Name + "\t" + Properties.Settings.Default[property.Name] + Environment.NewLine);
            }
            File.WriteAllText(Path.Combine(destination,
                                           Properties.Settings.Default.ConfigFileName), stringBuilder.ToString());
        }
    }
}
