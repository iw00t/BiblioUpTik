using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Windows.Forms;

namespace BiblioUpTik
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      PortableSettingsProvider settingsProvider = new PortableSettingsProvider();
      BiblioUpTik.Properties.Settings.Default.Providers.Add((ProviderBase) settingsProvider);
      foreach (SettingsProperty property in BiblioUpTik.Properties.Settings.Default.Properties)
        property.Provider = (SettingsProvider) settingsProvider;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new BiblioUpTik.BiblioUpTik());
    }
  }
}
