using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace BiblioUpTik
{
  internal class BiblioSite
  {
    private static readonly Uri BaseUrl = new Uri("http://bibliotik.me");
    private static readonly Uri LoginUrl = new Uri(BiblioSite.BaseUrl.ToString() + "login");
    private static readonly Uri LogoutUrl = new Uri(BiblioSite.BaseUrl.ToString() + "logout");
    private static readonly Uri SearchUri = new Uri(BiblioSite.BaseUrl.ToString() + "torrents/?search=");
    private static readonly Uri UploadUrl = new Uri(BiblioSite.BaseUrl.ToString() + "upload/ebooks");
    private readonly WebBrowser _w = new WebBrowser();
    private readonly int _delay = BiblioUpTik.Properties.Settings.Default.Delay;
    private Form _form;

    public bool Upload(ListViewItem book, FileSystemInfo torrent)
    {
      this._w.Navigate(BiblioSite.UploadUrl);
      this.LoadWait();
      bool flag;
      if (!this.LoadSet(this._w.Document, "AuthorsField", book.SubItems[2].Text))
        flag = false;
      else if (!this.LoadSet(this._w.Document, "TitleField", book.SubItems[3].Text))
        flag = false;
      else if (!this.LoadSet(this._w.Document, "IsbnField", book.SubItems[6].Text))
        flag = false;
      else if (!this.LoadSet(this._w.Document, "PublishersField", book.SubItems[4].Text))
        flag = false;
      else if (!this.LoadSet(this._w.Document, "PagesField", book.SubItems[8].Text))
        flag = false;
      else if (!this.LoadSet(this._w.Document, "YearField", book.SubItems[5].Text))
      {
        flag = false;
      }
      else
      {
        string extension = new FileInfo(book.Tag.ToString().Split('|')[0]).Extension;
        if (extension != null)
        {
          if (!(extension == ".mobi"))
          {
            if (!(extension == ".epub"))
            {
              if (!(extension == ".pdf"))
              {
                if (extension == ".azw3")
                {
                  if (!this.LoadSet(this._w.Document, "FormatField", "21"))
                    return false;
                }
                else
                  goto label_36;
              }
              else if (!this.LoadSet(this._w.Document, "FormatField", "2"))
                return false;
            }
            else if (!this.LoadSet(this._w.Document, "FormatField", "15"))
              return false;
          }
          else if (!this.LoadSet(this._w.Document, "FormatField", "16"))
            return false;
          if (book.Tag.ToString().Split('|')[1].Trim().Equals("retail"))
            this._w.Document.GetElementById("RetailField").InvokeMember("CLICK");
          if (!this.LoadSet(this._w.Document, "TagsField", book.SubItems[10].Text) || !this.LoadSet(this._w.Document, "DescriptionField", book.SubItems[7].Text))
            return false;
          HtmlDocument document = this._w.Document;
          HtmlElement htmlElement = document.GetElementsByTagName("head")[0];
          HtmlElement element = document.CreateElement("script");
          element.SetAttribute("text", "function fillDescription() { $('#DescriptionField').val('" + book.SubItems[7].Text.Replace("'", "\\'") + "'); $('#DescriptionField').change(); }");
          htmlElement.AppendChild(element);
          this._w.Document.InvokeScript("fillDescription");
          if (!this.LoadSet(this._w.Document, "ImageField", book.SubItems[9].Text))
            return false;
          if (this._form != null)
            this._form.Close();
          Form form1 = new Form();
          form1.Text = "BiblioUpTik :: Upload";
          form1.StartPosition = FormStartPosition.CenterScreen;
          form1.Size = Screen.PrimaryScreen.WorkingArea.Size;
          form1.FormBorderStyle = FormBorderStyle.FixedSingle;
          form1.MaximizeBox = false;
          form1.MinimizeBox = false;
          form1.IsMdiContainer = true;
          Form form2 = form1;
          Form form3 = new Form();
          form3.Text = "Upload Form";
          form3.StartPosition = FormStartPosition.Manual;
          form3.Location = new Point(0, 0);
          form3.Width = Convert.ToInt32((double) form2.Width * 0.5) - 4;
          form3.Height = Convert.ToInt32(form2.Height) - 40;
          form3.FormBorderStyle = FormBorderStyle.None;
          form3.MdiParent = form2;
          this._form = form3;
          this._w.ScriptErrorsSuppressed = true;
          Uri url1 = new Uri(new Uri("http://www.worldcat.org/").ToString() + "search?qt=worldcat_org_bks&q=" + book.SubItems[6].Text);
          WebBrowser webBrowser1 = new WebBrowser();
          webBrowser1.ScriptErrorsSuppressed = true;
          webBrowser1.Navigate(url1);
          Uri url2 = new Uri(new Uri("https://www.overdrive.com/").ToString() + "search?q=" + book.SubItems[6].Text);
          WebBrowser webBrowser2 = new WebBrowser();
          webBrowser2.ScriptErrorsSuppressed = true;
          webBrowser2.Navigate(url2);
          Uri url3 = new Uri(new Uri("http://www.goodreads.com/").ToString() + "search?utf8=✓&query=" + book.SubItems[6].Text);
          WebBrowser webBrowser3 = new WebBrowser();
          webBrowser3.ScriptErrorsSuppressed = true;
          webBrowser3.Navigate(url3);
          Form form4 = new Form();
          form4.StartPosition = FormStartPosition.Manual;
          form4.Width = Convert.ToInt32((double) form2.Width * 0.5) - 24;
          form4.Height = 1;
          form4.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - 4, 0);
          form4.FormBorderStyle = FormBorderStyle.None;
          form4.ShowInTaskbar = false;
          form4.MdiParent = form2;
          Form form5 = form4;
          Form form6 = new Form();
          form6.StartPosition = FormStartPosition.Manual;
          form6.Width = Convert.ToInt32((double) form2.Width * 0.5) - 24;
          form6.Height = Convert.ToInt32((double) form2.Height) - 80;
          form6.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - 4, 40);
          form6.FormBorderStyle = FormBorderStyle.None;
          form6.ShowInTaskbar = false;
          form6.MdiParent = form2;
          Form frmMessage = form6;
          Control.ControlCollection controls = frmMessage.Controls;
          Label label = new Label();
          int x = 0;
          Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
          int y = workingArea.Height / 44;
          label.Location = new Point(x, y);
          label.Width = Convert.ToInt32((double) form2.Width * 0.5) - 24;
          label.Height = Convert.ToInt32((double) form2.Height) - 80;
          label.TextAlign = ContentAlignment.MiddleCenter;
          label.Font = new Font("Arial", 24f, FontStyle.Bold);
          label.Text = "Please review the information in the upload form automatically filled by BiblioUpTik by comparing and checking against different sources.\n\nYou may browse WorldCat, OverDrive and Goodreads in this window.";
          controls.Add((Control) label);
          Form form7 = new Form();
          form7.StartPosition = FormStartPosition.Manual;
          form7.Width = Convert.ToInt32((double) form2.Width * 0.5) - 24;
          form7.Height = Convert.ToInt32((double) form2.Height) - 80;
          workingArea = Screen.PrimaryScreen.WorkingArea;
          form7.Location = new Point(workingArea.Width / 2 - 4, 40);
          form7.FormBorderStyle = FormBorderStyle.None;
          form7.ShowInTaskbar = false;
          form7.MdiParent = form2;
          Form frmWorldCat = form7;
          webBrowser1.Height = frmWorldCat.Height;
          webBrowser1.Width = frmWorldCat.Width;
          Form form8 = new Form();
          form8.StartPosition = FormStartPosition.Manual;
          form8.Width = Convert.ToInt32((double) form2.Width * 0.5) - 24;
          form8.Height = Convert.ToInt32((double) form2.Height) - 80;
          workingArea = Screen.PrimaryScreen.WorkingArea;
          form8.Location = new Point(workingArea.Width / 2 - 4, 40);
          form8.FormBorderStyle = FormBorderStyle.None;
          form8.ShowInTaskbar = false;
          form8.MdiParent = form2;
          Form frmOverDrive = form8;
          webBrowser2.Height = frmOverDrive.Height;
          webBrowser2.Width = frmOverDrive.Width;
          Form form9 = new Form();
          form9.StartPosition = FormStartPosition.Manual;
          form9.Width = Convert.ToInt32((double) form2.Width * 0.5) - 24;
          form9.Height = Convert.ToInt32((double) form2.Height) - 80;
          workingArea = Screen.PrimaryScreen.WorkingArea;
          form9.Location = new Point(workingArea.Width / 2 - 4, 40);
          form9.FormBorderStyle = FormBorderStyle.None;
          form9.ShowInTaskbar = false;
          form9.MdiParent = form2;
          Form frmGoodreads = form9;
          webBrowser3.Height = frmGoodreads.Height;
          webBrowser3.Width = frmGoodreads.Width;
          this._w.Dock = DockStyle.Fill;
          Button button1 = new Button();
          button1.Text = "WorldCat";
          button1.Height = 40;
          Button button2 = button1;
          workingArea = Screen.PrimaryScreen.WorkingArea;
          int num1 = workingArea.Width / 2 / 3 - 9;
          button2.Width = num1;
          button1.Left = 0;
          button1.Click += (EventHandler) ((s, e) =>
          {
            frmWorldCat.Show();
            frmMessage.Hide();
            frmOverDrive.Hide();
            frmGoodreads.Hide();
          });
          form5.Controls.Add((Control) button1);
          Button button3 = new Button();
          button3.Text = "OverDrive";
          button3.Height = 40;
          Button button4 = button3;
          workingArea = Screen.PrimaryScreen.WorkingArea;
          int num2 = workingArea.Width / 2 / 3 - 9;
          button4.Width = num2;
          button3.Left = button1.Width + 3;
          button3.Click += (EventHandler) ((s, e) =>
          {
            frmOverDrive.Show();
            frmMessage.Hide();
            frmWorldCat.Hide();
            frmGoodreads.Hide();
          });
          form5.Controls.Add((Control) button3);
          Button button5 = new Button();
          button5.Text = "Goodreads";
          button5.Height = 40;
          Button button6 = button5;
          workingArea = Screen.PrimaryScreen.WorkingArea;
          int num3 = workingArea.Width / 2 / 3 - 9;
          button6.Width = num3;
          button5.Left = (button1.Width + 3) * 2;
          button5.Click += (EventHandler) ((s, e) =>
          {
            frmGoodreads.Show();
            frmMessage.Hide();
            frmWorldCat.Hide();
            frmOverDrive.Hide();
          });
          form5.Controls.Add((Control) button5);
          frmWorldCat.Controls.Add((Control) webBrowser1);
          frmOverDrive.Controls.Add((Control) webBrowser2);
          frmGoodreads.Controls.Add((Control) webBrowser3);
          this._form.Controls.Add((Control) this._w);
          if (!form2.Visible)
          {
            form2.Show();
            this._form.Show();
            form5.Show();
            frmMessage.Show();
          }
          this._form.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);
          return true;
        }
label_36:
        flag = false;
      }
      return flag;
    }

    public bool LoadSet(HtmlDocument document, string elementID, string value)
    {
      HtmlElement htmlElement = (HtmlElement) null;
      while (htmlElement == (HtmlElement) null)
      {
        if (this._w.Document != (HtmlDocument) null)
        {
          htmlElement = this._w.Document.GetElementById(elementID);
          Thread.Sleep(10);
          Application.DoEvents();
        }
        else
        {
          int num = (int) MessageBox.Show("Unable to set attribute!");
          return false;
        }
      }
      htmlElement.SetAttribute(nameof (value), value);
      Thread.Sleep(50);
      return true;
    }

    public bool Login(string username, string password)
    {
      try
      {
        this._w.Navigate(BiblioSite.LoginUrl);
        this.LoadWait();
        HtmlElement htmlElement1 = (HtmlElement) null;
        while (htmlElement1 == (HtmlElement) null)
        {
          if (!(this._w.Document != (HtmlDocument) null))
          {
            int num = (int) MessageBox.Show("Login Failed, please try again!");
            return false;
          }
          htmlElement1 = this._w.Document.GetElementById(nameof (username));
          Thread.Sleep(10);
          Application.DoEvents();
        }
        htmlElement1.SetAttribute("value", username);
        HtmlElement htmlElement2 = (HtmlElement) null;
        while (htmlElement2 == (HtmlElement) null)
        {
          htmlElement2 = this._w.Document.GetElementById(nameof (password));
          Thread.Sleep(10);
          Application.DoEvents();
        }
        htmlElement2.SetAttribute("value", password);
        HtmlElementCollection elementsByTagName = this._w.Document.GetElementsByTagName("input");
        int num1 = 0;
        bool flag = false;
        foreach (HtmlElement htmlElement3 in elementsByTagName)
        {
          string attribute = htmlElement3.GetAttribute("id");
          if (flag)
          {
            htmlElement3.InvokeMember("click");
            ++num1;
          }
          if (attribute.ToUpper().Contains("KEEP"))
            flag = true;
          if (num1 > 5)
            break;
        }
      }
      catch (WebException ex)
      {
        int num = (int) MessageBox.Show("Login Failed: " + ex.Message);
        return false;
      }
      this.LoadWait();
      bool flag1;
      if (this._w.DocumentText.Contains("Wrong username/password."))
      {
        int num = (int) MessageBox.Show("Login Failed: Wrong username/password.");
        flag1 = false;
      }
      else
      {
        this._w.Navigate("https://bibliotik.me/forums/5/5408");
        this.LoadWait();
        bool flag2 = false;
        foreach (HtmlElement htmlElement in this._w.Document.GetElementsByTagName("h2"))
        {
          string attribute = htmlElement.GetAttribute("innerHTML");
          if (attribute.Contains("Latest version:"))
          {
            flag2 = true;
            if (string.Compare(Application.ProductVersion, Regex.Replace(attribute, "[A-Za-z ]", "").Replace(":", string.Empty)) < 0)
            {
              if (MessageBox.Show("A new version of BiblioUpTik is available!\n\nCurrent version: " + Application.ProductVersion + "\n" + attribute + "\n\nWould you like to visit the forum thread to download the new version?\nThis will close BiblioUpTik.", "BiblioUpTik :: New version available", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
              {
                Process.Start("https://bibliotik.me/forums/5/5408");
                Process.GetCurrentProcess().Kill();
              }
            }
          }
        }
        if (!flag2)
        {
          if (MessageBox.Show("The account with username " + username + " is not eligble to use BiblioUpTik.\nYou must be Power User or above.\n\nBiblioUpTik will now close.", "BiblioUpTik :: Account not eligble") == DialogResult.OK)
            Application.Exit();
          flag1 = false;
        }
        else
        {
          int num = (int) MessageBox.Show("You are now logged in as " + username + ".", "BiblioUpTik :: Login successful");
          flag1 = this._w.DocumentText.Contains(username);
        }
      }
      return flag1;
    }

    public bool Logout()
    {
      this._w.Navigate(BiblioSite.LogoutUrl);
      this.LoadWait();
      this._w.Navigate(BiblioSite.LoginUrl);
      this.LoadWait();
      return this._w.DocumentTitle == "Bibliotik / Login";
    }

    public Color DupeCheck(ListViewItem item)
    {
      try
      {
        BiblioUpTik.Web web = new BiblioUpTik.Web();
        this._w.Navigate(BiblioSite.SearchUrl(item));
        this.LoadWait();
        string stringBetween = web.GetStringBetween(this._w.DocumentText, "<tbody>", "</tbody>", 0);
        int num = BiblioSite.CountOccurences("<span class=\"title\">", stringBetween);
        int Start = 1;
        for (int index = 0; index < num; ++index)
        {
          if (web.GetStringBetween(stringBetween, "<td>", "</td>", Start).Contains("[Retail]"))
            return Color.Red;
          if (index + 1 == num)
            return item.Tag.ToString().Split('|')[1].Trim().Equals("retail") ? Color.Green : Color.Teal;
          Start += 8;
        }
        return Color.Green;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        return Color.Black;
      }
    }

    private static Uri SearchUrl(ListViewItem item)
    {
      string str1 = HttpUtility.UrlEncode(item.SubItems[3].Text.Split('(')[0].Trim());
      string str2 = HttpUtility.UrlEncode(item.SubItems[3].Text.Split(':')[0].Trim());
      string str3 = str2.Replace("The+", "").Replace("the+", "");
      string str4 = "\"" + str1 + "\"" + (object) '|' + "\"" + str2 + "\"" + (object) '|' + "\"" + str3 + "\"";
      string str5 = item.SubItems[2].Text.Trim().Replace(" & ", "|").Replace(" and ", "|").Replace(" ", "+").Replace(',', '|');
      string str6;
      switch (new FileInfo(item.Tag.ToString().Split('|')[0]).Extension)
      {
        case ".mobi":
          str6 = "&for[]=16";
          break;
        case ".epub":
          str6 = "&for[]=15";
          break;
        case ".pdf":
          str6 = "&for[]=2";
          break;
        case ".azw3":
          str6 = "&for[]=21";
          break;
        default:
          str6 = "";
          break;
      }
      return new Uri(BiblioSite.SearchUri.ToString() + "@title+" + str4.Replace("-", " ").Replace("  ", " ") + "+@authors+" + str5.Replace("-", " ").Replace("  ", " ") + str6);
    }

    public void ShowDupeCheck(ListViewItem book)
    {
      if (this._form != null)
        this._form.Close();
      Form form = new Form();
      form.Text = "Dupe Check Results";
      form.StartPosition = FormStartPosition.CenterScreen;
      form.Size = Screen.PrimaryScreen.WorkingArea.Size;
      form.WindowState = FormWindowState.Maximized;
      this._form = form;
      if (BiblioUpTik.Properties.Settings.Default.FormX > 0 && BiblioUpTik.Properties.Settings.Default.FormY > 0)
        this._form.SetDesktopLocation(BiblioUpTik.Properties.Settings.Default.FormX, BiblioUpTik.Properties.Settings.Default.FormY);
      // ISSUE: variable of a compiler-generated type
      BiblioUpTik.Properties.Settings settings1 = BiblioUpTik.Properties.Settings.Default;
      Point desktopLocation = this._form.DesktopLocation;
      int x = desktopLocation.X;
      settings1.FormX = x;
      // ISSUE: variable of a compiler-generated type
      BiblioUpTik.Properties.Settings settings2 = BiblioUpTik.Properties.Settings.Default;
      desktopLocation = this._form.DesktopLocation;
      int y = desktopLocation.Y;
      settings2.FormY = y;
      BiblioUpTik.Properties.Settings.Default.Save();
      this._w.Dock = DockStyle.Fill;
      this._form.Controls.Add((Control) this._w);
      if (!this._form.Visible)
        this._form.Show();
      this._w.Navigate(BiblioSite.SearchUrl(book));
      this._form.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);
    }

    private void Form_FormClosing(object sender, FormClosingEventArgs e)
    {
      this._form.MdiParent.Hide();
      this._form.Hide();
      this._form.Controls.Clear();
      e.Cancel = true;
    }

    private static int CountOccurences(string needle, string haystack)
    {
      return (haystack.Length - haystack.Replace(needle, "").Length) / needle.Length;
    }

    private void LoadWait()
    {
      for (int index = 0; index < this._delay; ++index)
      {
        Thread.Sleep(10);
        Application.DoEvents();
      }
    }

    [CompilerGenerated]
    internal static void \u003CUpload\u003Eg__button_Click\u007C0_0(object sender, EventArgs e, [In] ref BiblioSite.\u003C\u003Ec__DisplayClass0_0 obj2)
    {
      if ((sender as Button).Text == "OverDrive")
      {
        // ISSUE: reference to a compiler-generated field
        obj2.form2.Show();
        // ISSUE: reference to a compiler-generated field
        obj2.form3.Hide();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        obj2.form2.Show();
        // ISSUE: reference to a compiler-generated field
        obj2.form3.Hide();
      }
    }
  }
}
