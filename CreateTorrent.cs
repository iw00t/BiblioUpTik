using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BiblioUpTik
{
  internal class CreateTorrent
  {
    private readonly ProcessStartInfo StartInfo = new ProcessStartInfo();
    private Process InterProc;

    public CreateTorrent()
    {
      this.StartInfo.UseShellExecute = false;
      this.StartInfo.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\mktorrent";
      this.StartInfo.RedirectStandardInput = true;
      this.StartInfo.RedirectStandardOutput = true;
      this.StartInfo.RedirectStandardError = true;
      this.StartInfo.CreateNoWindow = true;
    }

    public bool Create(FileSystemInfo f)
    {
      this.StartInfo.WorkingDirectory = Path.GetDirectoryName(f.FullName);
      this.StartInfo.Arguments = "-p -a " + Protect.ToInsecureString(Protect.DecryptString(BiblioUpTik.Properties.Settings.Default.AnnounceURL)) + " " + (34.ToString() + f.FullName + (object) '"');
      this.InterProc = new Process()
      {
        StartInfo = this.StartInfo
      };
      this.InterProc.Start();
      Thread.Sleep(BiblioUpTik.Properties.Settings.Default.Delay);
      return File.Exists(f.FullName + ".torrent");
    }
  }
}
