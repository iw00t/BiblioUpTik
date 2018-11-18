using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace BiblioUpTik
{
  public class BiblioUpTik : Form
  {
    private readonly ArrayList _addedBooks = new ArrayList();
    private readonly ArrayList _renamedBooks = new ArrayList();
    private readonly ImageList _imageList = new ImageList();
    private readonly ListViewColumnSorter _lvwColumnSorter = new ListViewColumnSorter();
    private CreateTorrent _create = new CreateTorrent();
    private IContainer components = (IContainer) null;
    private readonly BiblioSite _bibliotik;
        internal static readonly object Properties;
        private int _folderLength;
    private double _fileOn;
    private string _folderPath;
    private string[] _files;
    private int _count;
    private ListViewItem _renameLVI;
    private bool _loginSuccess;
    private bool _dupeCancel;
    private bool _running;
    private ToolStrip toolStrip;
    private ToolStripButton loginBtn;
    private ToolStripButton LogoutBtn;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton addFolderBtn;
    private ToolStripButton addFileBtn;
    private ToolStripButton rmFileBtn;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripButton renameBtn;
    private ToolStripButton upAllBtn;
    private ToolStripButton upBtn;
    private ToolStripButton stopBtn;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripButton settingsBtn;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLbl;
    private ProgressBar progressBar;
    private BackgroundWorker folderWorker;
    private BackgroundWorker renameWorker;
    private ContextMenuStrip uploadListMenu;
    private ToolStripMenuItem dupeCheckToolStripMenuItem;
    private ToolStripMenuItem showSearchPageToolStripMenuItem;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private TextBox authorsBox;
    private TextBox titleBox;
    private TextBox publisherBox;
    private TextBox tagsBox;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private TextBox isbnBox;
    private TextBox imageBox;
    private NumericUpDown pagesBox;
    private NumericUpDown yearBox;
    private CheckBox retailBox;
    private RichTextBox descriptionBox;
    private Label label10;
    private Button saveBtn;
    private ListView uploadList;
    private SplitContainer splitContainer1;
    private ToolStripMenuItem getWebDataToolStripMenuItem;
    private BackgroundWorker webWorker;
    private ToolStripButton mobiBtn;
    private ToolStripButton epubBtn;
    private ToolStripButton pdfBtn;
    private ToolStripButton rdirBtn;
    private ToolStripMenuItem displayCoverToolStripMenuItem;
    private ToolStripMenuItem openFileToolStripMenuItem;
    private ToolStripMenuItem hideCoverToolStripMenuItem;
    private ToolStripMenuItem removeToolStripMenuItem;
    private ToolStripMenuItem redsToolStripMenuItem;
    private ToolStripMenuItem tealsToolStripMenuItem;
    private ToolStripMenuItem greensToolStripMenuItem;
    private ToolStripMenuItem openTorrentToolStripMenuItem;
    private Label label11;
    private TextBox fileBox;
    private ToolStripStatusLabel versionLbl;
    private ToolStripButton azw3Btn;

    public BiblioUpTik()
    {
      this.InitializeComponent();
      this.uploadList.ListViewItemSorter = (IComparer) this._lvwColumnSorter;
      this.uploadList.ColumnClick += new ColumnClickEventHandler(this.uploadList_ColumnClick);
      this.FormClosing += new FormClosingEventHandler(this.BiblioUpTik_FormClosing);
      this.webWorker.DoWork += new DoWorkEventHandler(this.webWorker_DoWork);
      this.webWorker.ProgressChanged += new ProgressChangedEventHandler(this.webWorker_ProgressChanged);
      this.uploadList.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.uploadList_ItemSelectionChanged);
      this._bibliotik = new BiblioSite();
      this.uploadList.Columns.Add("File Name", -2, HorizontalAlignment.Left);
      this.uploadList.Columns.Add("Dupe Check", -2, HorizontalAlignment.Left);
      this.uploadList.Columns.Add("Author(s)", -2, HorizontalAlignment.Left);
      this.uploadList.Columns.Add("Title", -2, HorizontalAlignment.Left);
      this.uploadList.Columns.Add("Publisher", -2, HorizontalAlignment.Left);
      this.uploadList.Columns.Add("Published", -2, HorizontalAlignment.Left);
      this.uploadList.Columns.Add("ISBN", -2, HorizontalAlignment.Left);
      this.rdirBtn.Checked = BiblioUpTik.Properties.Settings.Default.RecursiveDir;
      this.mobiBtn.Checked = BiblioUpTik.Properties.Settings.Default.GetMobi;
      this.epubBtn.Checked = BiblioUpTik.Properties.Settings.Default.GetEpub;
      this.pdfBtn.Checked = BiblioUpTik.Properties.Settings.Default.GetPDF;
      this.azw3Btn.Checked = BiblioUpTik.Properties.Settings.Default.GetAZW3;
      this.versionLbl.Text += Application.ProductVersion;
      if (!this.CheckUpdate() || MessageBox.Show("There is an available update would you like to download it?", "Update", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      Process.Start("http://zurkei.is2.byuh.edu/bibliouptik/BiblioUpTik.zip");
      Process.GetCurrentProcess().Kill();
    }

    public bool CheckUpdate()
    {
      return false;
    }

    private void settingsBtn_Click(object sender, EventArgs e)
    {
      new Settings().Show();
    }

    private void loginBtn_Click(object sender, EventArgs e)
    {
      string username = BiblioUpTik.Properties.Settings.Default.Username;
      string insecureString = Protect.ToInsecureString(Protect.DecryptString(BiblioUpTik.Properties.Settings.Default.Password));
      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(insecureString))
      {
        int num = (int) MessageBox.Show("Username and or Passwrod are not filled in!");
      }
      else
      {
        this.statusLbl.Text = "Logging In!";
        this._loginSuccess = this._bibliotik.Login(username, insecureString);
        this.statusLbl.Text = this._loginSuccess ? "Login Succeeded!" : "Login Failed!";
        this.ChangeBtns(this._loginSuccess);
      }
    }

    private void ChangeBtns(bool inOrOut)
    {
      if (inOrOut)
      {
        this.loginBtn.Enabled = false;
        this.LogoutBtn.Enabled = true;
        this.upBtn.Enabled = true;
      }
      else
      {
        this.loginBtn.Enabled = true;
        this.LogoutBtn.Enabled = false;
        this.upBtn.Enabled = false;
      }
    }

    private void LogoutBtn_Click(object sender, EventArgs e)
    {
      bool flag = this._bibliotik.Logout();
      this.statusLbl.Text = flag ? "Logout Succeeded!" : "Logout Failed!";
      this._loginSuccess = !flag;
      this.ChangeBtns(!flag);
    }

    private void addFileBtn_Click(object sender, EventArgs e)
    {
      if (this.folderWorker.IsBusy || this._running || this.renameWorker.IsBusy || this.webWorker.IsBusy)
      {
        int num = (int) MessageBox.Show("A proccess is already running, please wait.");
      }
      else
      {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Open eBook";
        openFileDialog1.Filter = "eBook files (*.mobi;*.azw3;*.epub;*.pdf)|*.mobi;*.azw3;*.epub;*.pdf";
        openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.Multiselect = true;
        OpenFileDialog openFileDialog2 = openFileDialog1;
        string lastFolder = BiblioUpTik.Properties.Settings.Default.LastFolder;
        if (!string.IsNullOrWhiteSpace(lastFolder) && Directory.Exists(lastFolder))
          openFileDialog2.InitialDirectory = lastFolder;
        if (openFileDialog2.ShowDialog() != DialogResult.OK)
        {
          this.statusLbl.Text = "File Not Added";
        }
        else
        {
          this._files = openFileDialog2.FileNames;
          BiblioUpTik.Properties.Settings.Default.LastFolder = Path.GetDirectoryName(openFileDialog2.FileName);
          BiblioUpTik.Properties.Settings.Default.Save();
          this.folderWorker.RunWorkerAsync((object) false);
        }
      }
    }

    private void rmFileBtn_Click(object sender, EventArgs e)
    {
      if (this.folderWorker.IsBusy || this._running || this.renameWorker.IsBusy || this.webWorker.IsBusy)
      {
        int num = (int) MessageBox.Show("A proccess is already running, please wait.");
      }
      else
      {
        foreach (ListViewItem selectedItem in this.uploadList.SelectedItems)
        {
          this._imageList.Images.RemoveByKey(selectedItem.ToString());
          this.uploadList.Items.Remove(selectedItem);
        }
        this.uploadList.SmallImageList = this._imageList;
        this.ChangeWidth(new object());
        this.RefreshCovers();
      }
    }

    private void addFolderBtn_Click(object sender, EventArgs e)
    {
      if (this.folderWorker.IsBusy || this._running || this.renameWorker.IsBusy || this.webWorker.IsBusy)
      {
        int num = (int) MessageBox.Show("A proccess is already running, please wait.");
      }
      else
      {
        this.folderWorker.WorkerReportsProgress = true;
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
        {
          ShowNewFolderButton = false,
          SelectedPath = Application.StartupPath
        };
        string lastFolder = BiblioUpTik.Properties.Settings.Default.LastFolder;
        if (!string.IsNullOrWhiteSpace(lastFolder) && Directory.Exists(lastFolder))
          folderBrowserDialog.SelectedPath = lastFolder;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        {
          this.statusLbl.Text = "Folder Not Added!";
        }
        else
        {
          this._folderPath = folderBrowserDialog.SelectedPath;
          BiblioUpTik.Properties.Settings.Default.LastFolder = this._folderPath;
          BiblioUpTik.Properties.Settings.Default.Save();
          this.folderWorker.RunWorkerAsync((object) true);
        }
      }
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      if (this.uploadList.SelectedItems.Count <= 0)
        return;
      ListViewItem selectedItem1 = this.uploadList.SelectedItems[0];
      selectedItem1.SubItems[3].Text = this.titleBox.Text;
      selectedItem1.SubItems[2].Text = this.authorsBox.Text;
      selectedItem1.SubItems[4].Text = this.publisherBox.Text;
      if (Book.IsISBNValid(Book.ISBN(this.isbnBox.Text)))
        this.isbnBox.Text = Book.ISBN(this.isbnBox.Text);
      else if (!string.IsNullOrWhiteSpace(this.isbnBox.Text))
      {
        this.isbnBox.Text = "";
        int num = (int) MessageBox.Show("Please input a proper ISBN!");
      }
      selectedItem1.SubItems[6].Text = Book.ISBN(this.isbnBox.Text);
      selectedItem1.SubItems[5].Text = this.yearBox.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      selectedItem1.SubItems[7].Text = this.descriptionBox.Text;
      if (selectedItem1.SubItems.Count <= 8 && !string.IsNullOrWhiteSpace(this.pagesBox.Value.ToString()))
        selectedItem1.SubItems.Add(this.pagesBox.Value.ToString());
      else
        selectedItem1.SubItems[8].Text = this.pagesBox.Value.ToString();
      if (selectedItem1.SubItems.Count <= 9 && !string.IsNullOrWhiteSpace(this.imageBox.Text))
      {
        selectedItem1.SubItems.Add(this.imageBox.Text);
      }
      else
      {
        selectedItem1.SubItems[9].Text = this.imageBox.Text;
        if (this.uploadList.SmallImageList != null && this.uploadList.SmallImageList.Images.Count > 0)
        {
          this._imageList.Images.RemoveByKey(selectedItem1.ToString());
          this.AddCover(selectedItem1);
        }
      }
      if (selectedItem1.SubItems.Count <= 10 && !string.IsNullOrWhiteSpace(this.tagsBox.Text))
        selectedItem1.SubItems.Add(this.tagsBox.Text);
      else
        selectedItem1.SubItems[10].Text = this.tagsBox.Text;
      if (this.retailBox.Checked)
        selectedItem1.Tag = (object) selectedItem1.Tag.ToString().Replace("non-retail", "retail");
      else if (!selectedItem1.Tag.ToString().Contains("non-retail"))
        selectedItem1.Tag = (object) selectedItem1.Tag.ToString().Replace("retail", "non-retail");
      if (this.uploadList.SelectedItems.Count > 1)
      {
        foreach (ListViewItem selectedItem2 in this.uploadList.SelectedItems)
        {
          if (selectedItem1.SubItems[2].Text != this.authorsBox.Text)
            selectedItem2.SubItems[2].Text = this.authorsBox.Text;
          if (selectedItem1.SubItems[4].Text != this.publisherBox.Text)
            selectedItem2.SubItems[4].Text = this.publisherBox.Text;
          if (selectedItem2.SubItems.Count <= 10 && !string.IsNullOrWhiteSpace(this.tagsBox.Text))
            selectedItem2.SubItems.Add(this.tagsBox.Text);
          else
            selectedItem2.SubItems[10].Text = this.tagsBox.Text;
          if (this.retailBox.Checked)
            selectedItem2.Tag = (object) selectedItem2.Tag.ToString().Replace("non-retail", "retail");
          else if (!selectedItem2.Tag.ToString().Contains("non-retail"))
            selectedItem2.Tag = (object) selectedItem2.Tag.ToString().Replace("retail", "non-retail");
        }
      }
      this.ChangeWidth(new object());
      this.RefreshCovers();
    }

    private void stopBtn_Click(object sender, EventArgs e)
    {
      if (this.folderWorker.IsBusy)
        this.folderWorker.CancelAsync();
      if (this.renameWorker.IsBusy)
        this.renameWorker.CancelAsync();
      if (this.webWorker.IsBusy)
        this.webWorker.CancelAsync();
      this._dupeCancel = true;
    }

    private void BiblioUpTik_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.folderWorker.IsBusy || this.renameWorker.IsBusy || this.webWorker.IsBusy || this._running)
      {
        if (MessageBox.Show("One or more proccesses are currently running. Are you sure you would like to close?", "Process Still Running!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
          Process.GetCurrentProcess().Kill();
        else
          e.Cancel = true;
      }
      else
      {
        e.Cancel = false;
        Application.Exit();
      }
    }

    private void mobiBtn_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.GetMobi = this.mobiBtn.Checked;
      BiblioUpTik.Properties.Settings.Default.Save();
    }

    private void epubBtn_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.GetEpub = this.epubBtn.Checked;
      BiblioUpTik.Properties.Settings.Default.Save();
    }

    private void pdfBtn_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.GetPDF = this.pdfBtn.Checked;
      BiblioUpTik.Properties.Settings.Default.Save();
    }

    private void azw3Btn_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.GetAZW3 = this.azw3Btn.Checked;
      BiblioUpTik.Properties.Settings.Default.Save();
    }

    private void rdirBtn_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.RecursiveDir = this.rdirBtn.Checked;
      BiblioUpTik.Properties.Settings.Default.Save();
    }

    private void upBtn_Click(object sender, EventArgs e)
    {
      if (this.uploadList.SelectedItems.Count <= 0)
        return;
      FileInfo fileInfo = new FileInfo(this.uploadList.SelectedItems[0].Tag.ToString().Split('|')[0]);
      this.statusLbl.Text = "Creating Torrent: " + fileInfo.Name;
      if (!this._create.Create((FileSystemInfo) fileInfo))
      {
        int num = (int) MessageBox.Show("Torrent Creation Failed For File: " + fileInfo.Name);
        this.statusLbl.Text = "Torrent Creation Failed For File: " + fileInfo.Name;
      }
      else
      {
        this.statusLbl.Text = "Uploading Torrent: " + fileInfo.Name;
        if (!this._bibliotik.Upload(this.uploadList.SelectedItems[0], (FileSystemInfo) new FileInfo(fileInfo.FullName + ".torrent")))
        {
          int num = (int) MessageBox.Show("Form Population Failed For File: " + fileInfo.Name);
          this.statusLbl.Text = "Form Population Failed For File: " + fileInfo.Name;
        }
        else
          this.statusLbl.Text = fileInfo.Name + " Uploaded!";
      }
    }

    private void upAllBtn_Click(object sender, EventArgs e)
    {
    }

    private void RecursiveCount(IEnumerable<DirectoryInfo> recursiveDirs)
    {
      foreach (DirectoryInfo recursiveDir in recursiveDirs)
      {
        foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>) recursiveDir.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (file => BiblioUpTik.BiblioUpTik.AllowedFiles(file.Extension))))
          ++this._folderLength;
        this.RecursiveCount((IEnumerable<DirectoryInfo>) recursiveDir.GetDirectories());
      }
    }

    private void RecursiveSearch(IEnumerable<DirectoryInfo> recursiveDirs, CancelEventArgs e)
    {
      foreach (DirectoryInfo recursiveDir in recursiveDirs)
      {
        this.statusLbl.Text = "Searching: " + recursiveDir.FullName;
        foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>) recursiveDir.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (file => BiblioUpTik.BiblioUpTik.AllowedFiles(file.Extension))))
        {
          FileInfo file = fileInfo;
          if (this.folderWorker.CancellationPending)
          {
            e.Cancel = true;
            this.statusLbl.Text = "Canceled Adding: " + recursiveDir.FullName;
            this.folderWorker.ReportProgress(100);
            break;
          }
          this.statusLbl.Text = "Adding: " + file.Name;
          ListViewItem listViewItem = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.GetBook((FileSystemInfo) file)), 15000);
          if (listViewItem != null)
          {
            this.AddItem((object) listViewItem);
            this._addedBooks.Add((object) listViewItem);
          }
          ++this._fileOn;
          this.folderWorker.ReportProgress((int) Math.Round(this._fileOn / (double) this._folderLength * 100.0));
        }
        this.RecursiveSearch((IEnumerable<DirectoryInfo>) recursiveDir.GetDirectories(), e);
      }
    }

    private static bool AllowedFiles(string extension)
    {
      switch (extension)
      {
        case ".mobi":
          if (BiblioUpTik.Properties.Settings.Default.GetMobi)
            return true;
          break;
        case ".epub":
          if (BiblioUpTik.Properties.Settings.Default.GetEpub)
            return true;
          break;
        case ".pdf":
          if (BiblioUpTik.Properties.Settings.Default.GetPDF)
            return true;
          break;
        case ".azw3":
          if (BiblioUpTik.Properties.Settings.Default.GetAZW3)
            return true;
          break;
      }
      return false;
    }

    private void folderWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      this.folderWorker.ReportProgress(0);
      this.folderWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.folderWorker_RunWorkerCompleted);
      this._fileOn = 0.0;
      this._folderLength = 0;
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      if ((bool) e.Argument)
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(this._folderPath);
        if (directoryInfo.Exists)
        {
          foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>) directoryInfo.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (file => BiblioUpTik.BiblioUpTik.AllowedFiles(file.Extension))))
            ++this._folderLength;
          if (BiblioUpTik.Properties.Settings.Default.RecursiveDir)
            this.RecursiveCount((IEnumerable<DirectoryInfo>) directoryInfo.GetDirectories());
          this.statusLbl.Text = "Searching: " + directoryInfo.FullName;
          foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>) directoryInfo.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (file => BiblioUpTik.BiblioUpTik.AllowedFiles(file.Extension))))
          {
            FileInfo file = fileInfo;
            if (this.folderWorker.CancellationPending)
            {
              e.Cancel = true;
              this.statusLbl.Text = "Canceled Adding: " + directoryInfo.FullName;
              this.folderWorker.ReportProgress(100);
              break;
            }
            this.statusLbl.Text = "Adding: " + file.Name;
            ListViewItem listViewItem = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.GetBook((FileSystemInfo) file)), 15000);
            if (listViewItem != null)
            {
              this.AddItem((object) listViewItem);
              this._addedBooks.Add((object) listViewItem);
            }
            ++this._fileOn;
            this.folderWorker.ReportProgress((int) Math.Round(this._fileOn / (double) this._folderLength * 100.0));
          }
          if (BiblioUpTik.Properties.Settings.Default.RecursiveDir)
            this.RecursiveSearch((IEnumerable<DirectoryInfo>) directoryInfo.GetDirectories(), (CancelEventArgs) e);
        }
        if (!e.Cancel)
          this.statusLbl.Text = "Finished Searching: " + directoryInfo.FullName;
      }
      else
      {
        FileInfo[] fileInfoArray = new FileInfo[((IEnumerable<string>) this._files).Count<string>()];
        int index = 0;
        foreach (string file in this._files)
        {
          fileInfoArray[index] = new FileInfo(file);
          ++index;
        }
        this._folderLength = ((IEnumerable<string>) this._files).Count<string>();
        foreach (FileInfo fileInfo in fileInfoArray)
        {
          FileInfo file = fileInfo;
          if (this.folderWorker.CancellationPending)
          {
            e.Cancel = true;
            this.statusLbl.Text = "Canceled Adding: " + file.FullName;
            this.folderWorker.ReportProgress(100);
            break;
          }
          this.statusLbl.Text = "Adding: " + file.Name;
          ListViewItem listViewItem = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.GetBook((FileSystemInfo) file)), 15000);
          if (listViewItem != null)
          {
            this.AddItem((object) listViewItem);
            this._addedBooks.Add((object) listViewItem);
          }
          ++this._fileOn;
          this.folderWorker.ReportProgress((int) Math.Round(this._fileOn / (double) this._folderLength * 100.0));
        }
        if (!e.Cancel)
          this.statusLbl.Text = "Files Added!";
      }
      int num = (int) MessageBox.Show("Time Elapesed: " + (object) stopwatch.Elapsed);
    }

    private void folderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.ChangeWidth(new object());
      this._dupeCancel = false;
      this._running = true;
      this._count = 0;
      foreach (ListViewItem book in this._addedBooks.Cast<ListViewItem>().TakeWhile<ListViewItem>((Func<ListViewItem, bool>) (book => !this._dupeCancel)))
      {
        this.FilterDescription(book);
        if (BiblioUpTik.Properties.Settings.Default.ShowCover)
          this.AddCover(book);
        this.DupeCheck(book, (double) this._addedBooks.Count, (double) this._count);
        ++this._count;
      }
      if (BiblioUpTik.Properties.Settings.Default.ShowCover)
      {
        this._imageList.ImageSize = new Size(BiblioUpTik.Properties.Settings.Default.Width, BiblioUpTik.Properties.Settings.Default.Height);
        this.uploadList.SmallImageList = this._imageList;
        this.RefreshCovers();
      }
      this._addedBooks.Clear();
      this._running = false;
    }

    private void folderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressBar.Value = e.ProgressPercentage;
    }

    private void AddCover(ListViewItem book)
    {
      this._imageList.Images.Add(book.ToString(), !string.IsNullOrWhiteSpace(book.SubItems[9].Text) ? BiblioUpTik.BiblioUpTik.LoadPicture(book.SubItems[9].Text) : (Image) new Bitmap(BiblioUpTik.Properties.Settings.Default.Width, BiblioUpTik.Properties.Settings.Default.Height));
      book.ImageKey = book.ToString();
    }

    private void renameBtn_Click(object sender, EventArgs e)
    {
      if (this.renameWorker.IsBusy)
      {
        int num = (int) MessageBox.Show("Renaming already in progress, please wait.");
      }
      else
      {
        if (this.uploadList.SelectedItems.Count <= 0)
          return;
        this._renamedBooks.Clear();
        this._folderLength = this.uploadList.SelectedItems.Count;
        this._count = 0;
        FileInfo[] fileInfoArray = new FileInfo[this._folderLength];
        foreach (ListViewItem selectedItem in this.uploadList.SelectedItems)
        {
          fileInfoArray[this._count] = new FileInfo(selectedItem.Tag.ToString().Split('|')[0]);
          this._renamedBooks.Add((object) selectedItem);
          ++this._count;
        }
        this.renameWorker.RunWorkerAsync((object) fileInfoArray);
      }
    }

    private void renameWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      this._fileOn = 0.0;
      this._count = 0;
      FileInfo[] fileInfos = new FileInfo[this._folderLength];
      foreach (FileInfo fileInfo in (FileInfo[]) e.Argument)
      {
        if (this.renameWorker.CancellationPending)
        {
          e.Cancel = true;
          this.statusLbl.Text = "Canceled Renaming";
          this.renameWorker.ReportProgress(100);
          break;
        }
        this.statusLbl.Text = "Renaming: " + fileInfo.Name;
        string str1 = new Regex("\\%(?<value>.*?)\\%", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant).Replace(BiblioUpTik.Properties.Settings.Default.Template, new MatchEvaluator(this.EvaluateMatchCallback)).Replace("()", "").Trim();
        string str2 = Path.Combine(fileInfo.DirectoryName, str1 + fileInfo.Extension);
        System.IO.File.Move(fileInfo.FullName, str2);
        fileInfos[this._count] = new FileInfo(str2);
        ++this._count;
        ++this._fileOn;
        if (this.renameWorker.WorkerReportsProgress)
          this.renameWorker.ReportProgress((int) Math.Round(this._fileOn / (double) this._folderLength * 100.0));
      }
      this.statusLbl.Text = "Files Renamed!";
      this.RenameList(new object(), fileInfos);
    }

    private void renameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressBar.Value = e.ProgressPercentage;
    }

    private string EvaluateMatchCallback(Match match)
    {
      return this.GetReplacementTextBasedOnTemplateValue(match.Groups["value"].Value);
    }

    private string EvaluateDescriptionMatchCallback(Match match)
    {
      return this.ReplaceDescriptionTemplate(match.Groups["value"].Value);
    }

    private string GetReplacementTextBasedOnTemplateValue(string s)
    {
      this.GetLVI(new object());
      ListViewItem renameLvi = this._renameLVI;
      switch (s)
      {
        case "author":
          return renameLvi.SubItems[2].Text;
        case "title":
          string text;
          if (!renameLvi.SubItems[3].Text.Contains<char>(':'))
            text = renameLvi.SubItems[3].Text;
          else
            text = renameLvi.SubItems[3].Text.Split(':')[0];
          return text;
        case "subtitle":
          string str;
          if (!renameLvi.SubItems[3].Text.Contains<char>(':'))
            str = (string) null;
          else
            str = renameLvi.SubItems[3].Text.Split(':')[1].Remove(0, 1);
          return str;
        case "publisher":
          return renameLvi.SubItems[4].Text;
        case "year":
          return renameLvi.SubItems[5].Text.Split('-')[0];
        case "isbn":
          return renameLvi.SubItems[6].Text;
        default:
          return renameLvi.SubItems[0].Text;
      }
    }

    private string ReplaceDescriptionTemplate(string s)
    {
      switch (s)
      {
        case "strong":
        case "b":
        case "big":
        case "h1":
        case "h2":
        case "h3":
        case "h4":
          return "[b]";
        case "/strong":
        case "/b":
        case "/big":
        case "/h1":
        case "/h2":
        case "/h3":
        case "/h4":
          return "[/b]";
        case "em":
        case "i":
        case "cite":
          return "[i]";
        case "/em":
        case "/i":
        case "/cite":
          return "[/i]";
        case "u":
          return "[u]";
        case "/u":
          return "[/u]";
        case "sup":
          return "[sup]";
        case "/sup":
          return "[/sup]";
        case "li":
          return "• ";
        case "p":
        case "span":
        case "div":
        case "body":
          return "";
        case "ul":
        case "ol":
        case "/li":
          return "\n";
        default:
          return "";
      }
    }

    private void uploadList_ItemSelectionChanged(object sender, EventArgs e)
    {
      if (this.uploadList.SelectedItems.Count <= 0)
        return;
      ListViewItem selectedItem = this.uploadList.SelectedItems[0];
      this.titleBox.Text = selectedItem.SubItems[3].Text;
      this.authorsBox.Text = selectedItem.SubItems[2].Text;
      this.publisherBox.Text = selectedItem.SubItems[4].Text;
      this.isbnBox.Text = selectedItem.SubItems[6].Text;
      this.yearBox.Value = (Decimal) Convert.ToInt32(selectedItem.SubItems[5].Text);
      this.descriptionBox.Text = selectedItem.SubItems[7].Text;
      this.pagesBox.Value = (Decimal) (selectedItem.SubItems.Count <= 8 ? 0 : Convert.ToInt32(selectedItem.SubItems[8].Text));
      this.imageBox.Text = selectedItem.SubItems.Count <= 9 ? "" : selectedItem.SubItems[9].Text;
      this.tagsBox.Text = selectedItem.SubItems.Count <= 10 ? "" : selectedItem.SubItems[10].Text;
      this.retailBox.Checked = (selectedItem.Tag.ToString().Split('|')[1].Trim().Equals("retail") ? 1 : 0) != 0;
      this.fileBox.Text = new FileInfo(selectedItem.Tag.ToString().Split('|')[0].Trim()).Name;
    }

    private void uploadList_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column == this._lvwColumnSorter.SortColumn)
      {
        this._lvwColumnSorter.Order = this._lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
      }
      else
      {
        this._lvwColumnSorter.SortColumn = e.Column;
        this._lvwColumnSorter.Order = SortOrder.Ascending;
      }
      this.uploadList.Sort();
    }

    private void getWebDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ListViewItem[] listViewItemArray = new ListViewItem[this.uploadList.SelectedItems.Count];
      int index = 0;
      foreach (ListViewItem selectedItem in this.uploadList.SelectedItems)
      {
        listViewItemArray[index] = selectedItem;
        ++index;
      }
      this.webWorker.RunWorkerAsync((object) listViewItemArray);
    }

    private void webWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      this.webWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.webWorker_RunWorkerCompleted);
      this.webWorker.ReportProgress(0);
      ListViewItem[] listViewItemArray = (ListViewItem[]) e.Argument;
      double num = 0.0;
      foreach (ListViewItem listViewItem1 in listViewItemArray)
      {
        ListViewItem book = listViewItem1;
        if (this.webWorker.CancellationPending)
        {
          e.Cancel = true;
          this.statusLbl.Text = "Canceled Getting Web Data";
          this.webWorker.ReportProgress(100);
          break;
        }
        FileInfo fileInfo = new FileInfo(book.Tag.ToString().Split('|')[0]);
        this.statusLbl.Text = "Getting Web Data: " + fileInfo.Name;
        switch (fileInfo.Extension)
        {
          case ".mobi":
            this.RemoveItem((object) book);
            ListViewItem listViewItem2 = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.ScrapeMobiData(book)), 15000);
            if (listViewItem2 != null)
            {
              this.AddItem((object) listViewItem2);
              this._addedBooks.Add((object) listViewItem2);
              break;
            }
            break;
          case ".azw3":
            this.RemoveItem((object) book);
            ListViewItem listViewItem3 = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.ScrapeMobiData(book)), 15000);
            if (listViewItem3 != null)
            {
              this.AddItem((object) listViewItem3);
              this._addedBooks.Add((object) listViewItem3);
              break;
            }
            break;
          case ".epub":
            this.RemoveItem((object) book);
            ListViewItem listViewItem4 = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.ScrapeData(book)), 15000);
            if (listViewItem4 != null)
            {
              this.AddItem((object) listViewItem4);
              this._addedBooks.Add((object) listViewItem4);
              break;
            }
            break;
          case ".pdf":
            this.RemoveItem((object) book);
            ListViewItem listViewItem5 = BiblioUpTik.BiblioUpTik.Limex<ListViewItem>((Func<ListViewItem>) (() => Book.ScrapeData(book)), 15000);
            if (listViewItem5 != null)
            {
              this.AddItem((object) listViewItem5);
              this._addedBooks.Add((object) listViewItem5);
              break;
            }
            break;
        }
        ++num;
        this.webWorker.ReportProgress((int) Math.Round(num / (double) listViewItemArray.Length * 100.0));
      }
      this.statusLbl.Text = "Data Gathered";
    }

    private void webWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this._dupeCancel = false;
      this._running = true;
      foreach (ListViewItem book in this._addedBooks.Cast<ListViewItem>().TakeWhile<ListViewItem>((Func<ListViewItem, bool>) (book => !this._dupeCancel)))
      {
        this.FilterDescription(book);
        if (BiblioUpTik.Properties.Settings.Default.ShowCover)
          this.AddCover(book);
      }
      if (BiblioUpTik.Properties.Settings.Default.ShowCover)
      {
        this._imageList.ImageSize = new Size(BiblioUpTik.Properties.Settings.Default.Width, BiblioUpTik.Properties.Settings.Default.Height);
        this.uploadList.SmallImageList = this._imageList;
        this.RefreshCovers();
      }
      this._addedBooks.Clear();
      this.ChangeWidth(new object());
      this.uploadList.Sort();
      this._running = false;
    }

    private void webWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressBar.Value = e.ProgressPercentage;
    }

    private void dupeCheckToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.folderWorker.IsBusy || this._running || this.renameWorker.IsBusy || this.webWorker.IsBusy)
      {
        int num1 = (int) MessageBox.Show("A proccess is already running, please wait.");
      }
      else
      {
        this._dupeCancel = false;
        this._running = true;
        if (!this._loginSuccess)
        {
          int num2 = (int) MessageBox.Show("Must be logged in");
        }
        else
        {
          this._count = 0;
          foreach (ListViewItem book in this.uploadList.SelectedItems.Cast<ListViewItem>().TakeWhile<ListViewItem>((Func<ListViewItem, bool>) (book => !this._dupeCancel)))
          {
            this.DupeCheck(book, (double) this.uploadList.SelectedItems.Count, (double) this._count);
            ++this._count;
          }
          this._dupeCancel = false;
          this._running = false;
        }
      }
    }

    private void showSearchPageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this._loginSuccess)
      {
        int num = (int) MessageBox.Show("Must be logged in");
      }
      else
        this._bibliotik.ShowDupeCheck(this.uploadList.SelectedItems[0]);
    }

    private void DupeCheck(ListViewItem book, double total, double count)
    {
      if (!this._loginSuccess)
        return;
      this.progressBar.Value = (int) Math.Round(count / total * 100.0);
      this.statusLbl.Text = "Dupe Checking: " + new FileInfo(book.Tag.ToString().Split('|')[0]).Name;
      book.ForeColor = this._bibliotik.DupeCheck(book);
      book.SubItems[1].Text = book.ForeColor == Color.Red ? "Dupe" : (book.ForeColor == Color.Teal ? "Possible Dupe" : "Safe");
      this.progressBar.Value = (int) Math.Round((count + 1.0) / total * 100.0);
      this.statusLbl.Text = "Dupe Check Complete";
    }

    private void AddItem(object o)
    {
      if (this.uploadList.InvokeRequired)
        this.Invoke((Delegate) new BiblioUpTik.BiblioUpTik.AddItemCallback(this.AddItem), o);
      else
        this.uploadList.Items.Add((ListViewItem) o);
    }

    private void ChangeWidth(object o)
    {
      if (this.uploadList.InvokeRequired)
      {
        this.Invoke((Delegate) new BiblioUpTik.BiblioUpTik.ChangeWidthCallback(this.ChangeWidth), o);
      }
      else
      {
        foreach (ColumnHeader columnHeader in this.uploadList.Columns.Cast<ColumnHeader>().Where<ColumnHeader>((Func<ColumnHeader, bool>) (column => column.Text != "Published")))
          columnHeader.Width = -1;
      }
    }

    private void RenameList(object o, FileInfo[] fileInfos)
    {
      if (this.uploadList.InvokeRequired)
      {
        this.Invoke((Delegate) new BiblioUpTik.BiblioUpTik.RenameListCallback(this.RenameList), o, (object) fileInfos);
      }
      else
      {
        this._count = 0;
        foreach (ListViewItem renamedBook in this._renamedBooks)
        {
          renamedBook.Tag = (object) (fileInfos[this._count].FullName + "|" + renamedBook.Tag.ToString().Split('|')[1]);
          renamedBook.Text = fileInfos[this._count].Name;
          ++this._count;
        }
      }
    }

    private void GetLVI(object o)
    {
      if (this.uploadList.InvokeRequired)
        this.Invoke((Delegate) new BiblioUpTik.BiblioUpTik.GetLVICallback(this.GetLVI), o);
      else
        this._renameLVI = (ListViewItem) this._renamedBooks[this._count];
    }

    private void RemoveItem(object o)
    {
      if (this.uploadList.InvokeRequired)
      {
        this.Invoke((Delegate) new BiblioUpTik.BiblioUpTik.RemoveItemCallback(this.RemoveItem), o);
      }
      else
      {
        this.uploadList.Items.Remove((ListViewItem) o);
        this._imageList.Images.RemoveByKey(o.ToString());
        this.RefreshCovers();
      }
    }

    public static T Limex<T>(Func<T> F, int Timeout, out bool Completed)
    {
      T result = default (T);
      Thread thread = new Thread((ThreadStart) (() => result = F()));
      thread.Start();
      Completed = thread.Join(Timeout);
      if (!Completed)
        thread.Abort();
      return result;
    }

    public static T Limex<T>(Func<T> F, int Timeout)
    {
      bool Completed;
      return BiblioUpTik.BiblioUpTik.Limex<T>(F, Timeout, out Completed);
    }

    private void displayCoverToolStripMenuItem_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.ShowCover = true;
      this._dupeCancel = false;
      this._running = true;
      this._imageList.ImageSize = new Size(BiblioUpTik.Properties.Settings.Default.Width, BiblioUpTik.Properties.Settings.Default.Height);
      foreach (ListViewItem book in this.uploadList.Items.Cast<ListViewItem>().TakeWhile<ListViewItem>((Func<ListViewItem, bool>) (book => !this._dupeCancel)).Where<ListViewItem>((Func<ListViewItem, bool>) (book => BiblioUpTik.Properties.Settings.Default.ShowCover)))
        this.AddCover(book);
      this.uploadList.SmallImageList = this._imageList;
      this.RefreshCovers();
      this._running = false;
    }

    private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Process.Start(this.uploadList.SelectedItems[0].Tag.ToString().Split('|')[0]);
    }

    private void hideCoverToolStripMenuItem_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.ShowCover = false;
      this._imageList.ImageSize = new Size(1, 1);
      this._imageList.Images.Clear();
      this.uploadList.SmallImageList = this._imageList;
      this.ChangeWidth(new object());
    }

    private void RefreshCovers()
    {
      if (!BiblioUpTik.Properties.Settings.Default.ShowCover)
        return;
      foreach (ListViewItem listViewItem in this.uploadList.Items)
        listViewItem.ImageKey = listViewItem.ToString();
      this.ChangeWidth(new object());
    }

    public static Image LoadPicture(string url)
    {
      Image image = (Image) null;
      Stream stream = (Stream) null;
      HttpWebResponse httpWebResponse = (HttpWebResponse) null;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
        httpWebRequest.AllowWriteStreamBuffering = true;
        httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        if ((stream = httpWebResponse.GetResponseStream()) != null)
          image = Image.FromStream(stream);
      }
      catch
      {
      }
      finally
      {
        stream?.Close();
        httpWebResponse?.Close();
      }
      return image;
    }

    private void redsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.uploadList.Items.Cast<ListViewItem>().Where<ListViewItem>((Func<ListViewItem, bool>) (book => book.ForeColor == Color.Red)))
      {
        this._imageList.Images.RemoveByKey(listViewItem.ToString());
        this.uploadList.Items.Remove(listViewItem);
        this.uploadList.SmallImageList = this._imageList;
        this.ChangeWidth(new object());
        this.RefreshCovers();
      }
    }

    private void tealsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.uploadList.Items.Cast<ListViewItem>().Where<ListViewItem>((Func<ListViewItem, bool>) (book => book.ForeColor == Color.Teal)))
      {
        this._imageList.Images.RemoveByKey(listViewItem.ToString());
        this.uploadList.Items.Remove(listViewItem);
        this.uploadList.SmallImageList = this._imageList;
        this.ChangeWidth(new object());
        this.RefreshCovers();
      }
    }

    private void greensToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.uploadList.Items.Cast<ListViewItem>().Where<ListViewItem>((Func<ListViewItem, bool>) (book => book.ForeColor == Color.Green)))
      {
        this._imageList.Images.RemoveByKey(listViewItem.ToString());
        this.uploadList.Items.Remove(listViewItem);
        this.uploadList.SmallImageList = this._imageList;
        this.ChangeWidth(new object());
        this.RefreshCovers();
      }
    }

    private void openTorrentToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string str = this.uploadList.SelectedItems[0].Tag.ToString().Split('|')[0] + ".torrent";
      if (System.IO.File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Torrent File Does Not Exist!");
      }
    }

    private void FilterDescription(ListViewItem book)
    {
      Regex regex = new Regex("\\<(?<value>.*?)\\>", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
      book.SubItems[7].Text = regex.Replace(book.SubItems[7].Text, new MatchEvaluator(this.EvaluateDescriptionMatchCallback));
      book.SubItems[7].Text = book.SubItems[7].Text.Replace("???", "'").Replace("??", "").Replace("??????", "");
      string str1;
      try
      {
        Uri uri = new Uri("https://www.overdrive.com/");
        Uri url1 = new Uri(uri.ToString() + "search?q=" + book.SubItems[6].Text);
        Web web = new Web();
        string strSource1 = web.BookRequest(url1);
        string str2 = new Uri(new Uri(uri.ToString() + "/book/show/").ToString() + web.GetStringBetween(strSource1, "<h3 class=\"title-result-row__title\"", "</h3>", 0)).ToString();
        int startIndex = str2.IndexOf("href=\"") + 6;
        int length = str2.IndexOf("\" ") - startIndex;
        Uri url2 = new Uri("https://www.overdrive.com" + str2.Substring(startIndex, length));
        string strSource2 = new Web().BookRequest(url2);
        string input = new Uri(new Uri(url2.ToString() + "/book/show/").ToString() + web.GetStringBetween(strSource2, "Subjects</h3>", "</div>", 0)).ToString();
        string str3 = "";
        foreach (object match in new Regex(Regex.Escape("\">") + "(.*?)" + Regex.Escape("</a>")).Matches(input))
          str3 = str3 + ((Match) match).Groups[1].Value.ToLower() + ", ";
        str1 = WebUtility.HtmlDecode(str3.Substring(0, str3.Length - 2));
      }
      catch (Exception ex)
      {
        str1 = "";
      }
      string str4;
      try
      {
        Uri uri = new Uri("http://www.goodreads.com/");
        Uri url = new Uri(uri.ToString() + "search?utf8=✓&query=" + book.SubItems[6].Text);
        Web web = new Web();
        string strSource = web.BookRequest(url);
        string input = new Uri(new Uri(uri.ToString() + "/book/show/").ToString() + web.GetStringBetween(strSource, "<a class=\"actionLinkLite bookPageGenreLink\" href=\"", "<div class=\"clear\"></div></div>", 0)).ToString();
        string str2 = "";
        foreach (Match match in Regex.Matches(input, "/genres/"))
        {
          string str3 = input.Substring(match.Index, 100);
          int startIndex = str3.IndexOf("\">") + 2;
          int length = str3.IndexOf("</a>") - startIndex;
          str2 = str2 + str3.Substring(startIndex, length).ToLower() + ", ";
        }
        str4 = WebUtility.HtmlDecode(str2.Substring(0, str2.Length - 2));
      }
      catch (Exception ex)
      {
        str4 = "";
      }
      if (str1 != "" && str4 == "")
        book.SubItems[10].Text = str1;
      else if (str1 == "" && str4 != "")
        book.SubItems[10].Text = str4;
      else if (str1 != "" && str4 != "")
      {
        switch (MessageBox.Show("Click Yes for:\n• " + str1.Replace(", ", "\n• ") + "\n\nClick No for:\n• " + str4.Replace(", ", "\n• "), "BiblioUpTik :: Select tags", MessageBoxButtons.YesNo))
        {
          case DialogResult.Yes:
            book.SubItems[10].Text = str1;
            break;
          case DialogResult.No:
            book.SubItems[10].Text = str4;
            break;
        }
      }
      else
        book.SubItems[10].Text = "";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BiblioUpTik.BiblioUpTik));
      this.toolStrip = new ToolStrip();
      this.loginBtn = new ToolStripButton();
      this.LogoutBtn = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.addFolderBtn = new ToolStripButton();
      this.addFileBtn = new ToolStripButton();
      this.rmFileBtn = new ToolStripButton();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.renameBtn = new ToolStripButton();
      this.upAllBtn = new ToolStripButton();
      this.upBtn = new ToolStripButton();
      this.stopBtn = new ToolStripButton();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.settingsBtn = new ToolStripButton();
      this.mobiBtn = new ToolStripButton();
      this.epubBtn = new ToolStripButton();
      this.pdfBtn = new ToolStripButton();
      this.rdirBtn = new ToolStripButton();
      this.uploadListMenu = new ContextMenuStrip(this.components);
      this.dupeCheckToolStripMenuItem = new ToolStripMenuItem();
      this.showSearchPageToolStripMenuItem = new ToolStripMenuItem();
      this.getWebDataToolStripMenuItem = new ToolStripMenuItem();
      this.displayCoverToolStripMenuItem = new ToolStripMenuItem();
      this.hideCoverToolStripMenuItem = new ToolStripMenuItem();
      this.openFileToolStripMenuItem = new ToolStripMenuItem();
      this.openTorrentToolStripMenuItem = new ToolStripMenuItem();
      this.removeToolStripMenuItem = new ToolStripMenuItem();
      this.redsToolStripMenuItem = new ToolStripMenuItem();
      this.tealsToolStripMenuItem = new ToolStripMenuItem();
      this.greensToolStripMenuItem = new ToolStripMenuItem();
      this.statusStrip = new StatusStrip();
      this.statusLbl = new ToolStripStatusLabel();
      this.versionLbl = new ToolStripStatusLabel();
      this.progressBar = new ProgressBar();
      this.folderWorker = new BackgroundWorker();
      this.renameWorker = new BackgroundWorker();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.authorsBox = new TextBox();
      this.titleBox = new TextBox();
      this.publisherBox = new TextBox();
      this.tagsBox = new TextBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.isbnBox = new TextBox();
      this.imageBox = new TextBox();
      this.pagesBox = new NumericUpDown();
      this.yearBox = new NumericUpDown();
      this.retailBox = new CheckBox();
      this.descriptionBox = new RichTextBox();
      this.label10 = new Label();
      this.saveBtn = new Button();
      this.uploadList = new ListView();
      this.splitContainer1 = new SplitContainer();
      this.label11 = new Label();
      this.fileBox = new TextBox();
      this.webWorker = new BackgroundWorker();
      this.azw3Btn = new ToolStripButton();
      this.toolStrip.SuspendLayout();
      this.uploadListMenu.SuspendLayout();
      this.statusStrip.SuspendLayout();
      this.pagesBox.BeginInit();
      this.yearBox.BeginInit();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      this.toolStrip.AutoSize = false;
      this.toolStrip.BackColor = SystemColors.ControlDark;
      this.toolStrip.ImageScalingSize = new Size(32, 32);
      this.toolStrip.Items.AddRange(new ToolStripItem[18]
      {
        (ToolStripItem) this.loginBtn,
        (ToolStripItem) this.LogoutBtn,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.addFolderBtn,
        (ToolStripItem) this.addFileBtn,
        (ToolStripItem) this.rmFileBtn,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.renameBtn,
        (ToolStripItem) this.upAllBtn,
        (ToolStripItem) this.upBtn,
        (ToolStripItem) this.stopBtn,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.settingsBtn,
        (ToolStripItem) this.mobiBtn,
        (ToolStripItem) this.epubBtn,
        (ToolStripItem) this.pdfBtn,
        (ToolStripItem) this.azw3Btn,
        (ToolStripItem) this.rdirBtn
      });
      this.toolStrip.Location = new Point(0, 0);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.RenderMode = ToolStripRenderMode.System;
      this.toolStrip.Size = new Size(862, 56);
      this.toolStrip.TabIndex = 0;
      this.toolStrip.Text = "Tool Strip";
      this.loginBtn.AccessibleRole = AccessibleRole.None;
      this.loginBtn.Image = (Image) componentResourceManager.GetObject("loginBtn.Image");
      this.loginBtn.ImageTransparentColor = Color.Magenta;
      this.loginBtn.Name = "loginBtn";
      this.loginBtn.Size = new Size(47, 53);
      this.loginBtn.Text = "Log &In!";
      this.loginBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.loginBtn.Click += new EventHandler(this.loginBtn_Click);
      this.LogoutBtn.Enabled = false;
      this.LogoutBtn.Image = (Image) componentResourceManager.GetObject("LogoutBtn.Image");
      this.LogoutBtn.ImageTransparentColor = Color.Magenta;
      this.LogoutBtn.Name = "LogoutBtn";
      this.LogoutBtn.Size = new Size(57, 53);
      this.LogoutBtn.Text = "Log &Out!";
      this.LogoutBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.LogoutBtn.Click += new EventHandler(this.LogoutBtn_Click);
      this.toolStripSeparator1.ForeColor = SystemColors.ControlText;
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 56);
      this.addFolderBtn.Image = (Image) componentResourceManager.GetObject("addFolderBtn.Image");
      this.addFolderBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.addFolderBtn.ImageTransparentColor = Color.Magenta;
      this.addFolderBtn.Name = "addFolderBtn";
      this.addFolderBtn.Size = new Size(69, 53);
      this.addFolderBtn.Text = "&Add Folder";
      this.addFolderBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.addFolderBtn.ToolTipText = "Add all files in a folder";
      this.addFolderBtn.Click += new EventHandler(this.addFolderBtn_Click);
      this.addFileBtn.Image = (Image) componentResourceManager.GetObject("addFileBtn.Image");
      this.addFileBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.addFileBtn.ImageTransparentColor = Color.Magenta;
      this.addFileBtn.Name = "addFileBtn";
      this.addFileBtn.Size = new Size(67, 53);
      this.addFileBtn.Text = "Add &File(s)";
      this.addFileBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.addFileBtn.Click += new EventHandler(this.addFileBtn_Click);
      this.rmFileBtn.Image = (Image) componentResourceManager.GetObject("rmFileBtn.Image");
      this.rmFileBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.rmFileBtn.ImageTransparentColor = Color.Magenta;
      this.rmFileBtn.Name = "rmFileBtn";
      this.rmFileBtn.Size = new Size(75, 53);
      this.rmFileBtn.Text = "&Remove File";
      this.rmFileBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.rmFileBtn.ToolTipText = "Remove File from upload list";
      this.rmFileBtn.Click += new EventHandler(this.rmFileBtn_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(6, 56);
      this.renameBtn.Image = (Image) componentResourceManager.GetObject("renameBtn.Image");
      this.renameBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.renameBtn.ImageTransparentColor = Color.Magenta;
      this.renameBtn.Name = "renameBtn";
      this.renameBtn.Size = new Size(54, 53);
      this.renameBtn.Text = "Re&name";
      this.renameBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.renameBtn.ToolTipText = "Rename the actual file based off template in settings";
      this.renameBtn.Click += new EventHandler(this.renameBtn_Click);
      this.upAllBtn.Enabled = false;
      this.upAllBtn.Image = (Image) componentResourceManager.GetObject("upAllBtn.Image");
      this.upAllBtn.ImageAlign = ContentAlignment.TopCenter;
      this.upAllBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.upAllBtn.ImageTransparentColor = Color.Magenta;
      this.upAllBtn.Name = "upAllBtn";
      this.upAllBtn.Size = new Size(66, 53);
      this.upAllBtn.Text = "Upload All";
      this.upAllBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.upAllBtn.ToolTipText = "Upload all files, use at your own discretion";
      this.upAllBtn.Click += new EventHandler(this.upAllBtn_Click);
      this.upBtn.Enabled = false;
      this.upBtn.Image = (Image) componentResourceManager.GetObject("upBtn.Image");
      this.upBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.upBtn.ImageTransparentColor = Color.Magenta;
      this.upBtn.Name = "upBtn";
      this.upBtn.Size = new Size(70, 53);
      this.upBtn.Text = "&Upload File";
      this.upBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.upBtn.Click += new EventHandler(this.upBtn_Click);
      this.stopBtn.Image = (Image) componentResourceManager.GetObject("stopBtn.Image");
      this.stopBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.stopBtn.ImageTransparentColor = Color.Magenta;
      this.stopBtn.Name = "stopBtn";
      this.stopBtn.Size = new Size(38, 53);
      this.stopBtn.Text = "S&top!";
      this.stopBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.stopBtn.ToolTipText = "Stops current action";
      this.stopBtn.Click += new EventHandler(this.stopBtn_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(6, 56);
      this.settingsBtn.Image = (Image) componentResourceManager.GetObject("settingsBtn.Image");
      this.settingsBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.settingsBtn.ImageTransparentColor = Color.Magenta;
      this.settingsBtn.Name = "settingsBtn";
      this.settingsBtn.Size = new Size(53, 53);
      this.settingsBtn.Text = "&Settings";
      this.settingsBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.settingsBtn.Click += new EventHandler(this.settingsBtn_Click);
      this.mobiBtn.Checked = true;
      this.mobiBtn.CheckOnClick = true;
      this.mobiBtn.CheckState = CheckState.Checked;
      this.mobiBtn.Image = (Image) componentResourceManager.GetObject("mobiBtn.Image");
      this.mobiBtn.ImageTransparentColor = Color.Magenta;
      this.mobiBtn.Name = "mobiBtn";
      this.mobiBtn.Size = new Size(39, 53);
      this.mobiBtn.Text = "&Mobi";
      this.mobiBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.mobiBtn.Click += new EventHandler(this.mobiBtn_Click);
      this.epubBtn.Checked = true;
      this.epubBtn.CheckOnClick = true;
      this.epubBtn.CheckState = CheckState.Checked;
      this.epubBtn.ForeColor = Color.Transparent;
      this.epubBtn.Image = (Image) componentResourceManager.GetObject("epubBtn.Image");
      this.epubBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.epubBtn.ImageTransparentColor = Color.Magenta;
      this.epubBtn.Name = "epubBtn";
      this.epubBtn.Size = new Size(39, 53);
      this.epubBtn.Text = "&epub";
      this.epubBtn.TextImageRelation = TextImageRelation.Overlay;
      this.epubBtn.Click += new EventHandler(this.epubBtn_Click);
      this.pdfBtn.Checked = true;
      this.pdfBtn.CheckOnClick = true;
      this.pdfBtn.CheckState = CheckState.Checked;
      this.pdfBtn.ForeColor = Color.Transparent;
      this.pdfBtn.Image = (Image) componentResourceManager.GetObject("pdfBtn.Image");
      this.pdfBtn.ImageScaling = ToolStripItemImageScaling.None;
      this.pdfBtn.ImageTransparentColor = Color.Magenta;
      this.pdfBtn.Name = "pdfBtn";
      this.pdfBtn.Size = new Size(52, 53);
      this.pdfBtn.Text = "&pdf";
      this.pdfBtn.TextImageRelation = TextImageRelation.Overlay;
      this.pdfBtn.Click += new EventHandler(this.pdfBtn_Click);
      this.rdirBtn.CheckOnClick = true;
      this.rdirBtn.Image = (Image) componentResourceManager.GetObject("rdirBtn.Image");
      this.rdirBtn.ImageTransparentColor = Color.Magenta;
      this.rdirBtn.Name = "rdirBtn";
      this.rdirBtn.Size = new Size(61, 51);
      this.rdirBtn.Text = "Re&cursive";
      this.rdirBtn.TextImageRelation = TextImageRelation.ImageAboveText;
      this.rdirBtn.Click += new EventHandler(this.rdirBtn_Click);
      this.uploadListMenu.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.dupeCheckToolStripMenuItem,
        (ToolStripItem) this.showSearchPageToolStripMenuItem,
        (ToolStripItem) this.getWebDataToolStripMenuItem,
        (ToolStripItem) this.displayCoverToolStripMenuItem,
        (ToolStripItem) this.hideCoverToolStripMenuItem,
        (ToolStripItem) this.openFileToolStripMenuItem,
        (ToolStripItem) this.openTorrentToolStripMenuItem,
        (ToolStripItem) this.removeToolStripMenuItem
      });
      this.uploadListMenu.Name = "uploadListMenu";
      this.uploadListMenu.Size = new Size(234, 180);
      this.dupeCheckToolStripMenuItem.Name = "dupeCheckToolStripMenuItem";
      this.dupeCheckToolStripMenuItem.ShortcutKeys = Keys.D | Keys.Control | Keys.Alt;
      this.dupeCheckToolStripMenuItem.Size = new Size(233, 22);
      this.dupeCheckToolStripMenuItem.Text = "Dupe Check";
      this.dupeCheckToolStripMenuItem.Click += new EventHandler(this.dupeCheckToolStripMenuItem_Click);
      this.showSearchPageToolStripMenuItem.Name = "showSearchPageToolStripMenuItem";
      this.showSearchPageToolStripMenuItem.ShortcutKeys = Keys.S | Keys.Control | Keys.Alt;
      this.showSearchPageToolStripMenuItem.Size = new Size(233, 22);
      this.showSearchPageToolStripMenuItem.Text = "Show Search Page";
      this.showSearchPageToolStripMenuItem.Click += new EventHandler(this.showSearchPageToolStripMenuItem_Click);
      this.getWebDataToolStripMenuItem.Name = "getWebDataToolStripMenuItem";
      this.getWebDataToolStripMenuItem.ShortcutKeys = Keys.W | Keys.Control | Keys.Alt;
      this.getWebDataToolStripMenuItem.Size = new Size(233, 22);
      this.getWebDataToolStripMenuItem.Text = "Get Web Data";
      this.getWebDataToolStripMenuItem.Click += new EventHandler(this.getWebDataToolStripMenuItem_Click);
      this.displayCoverToolStripMenuItem.Name = "displayCoverToolStripMenuItem";
      this.displayCoverToolStripMenuItem.ShortcutKeys = Keys.C | Keys.Control | Keys.Alt;
      this.displayCoverToolStripMenuItem.Size = new Size(233, 22);
      this.displayCoverToolStripMenuItem.Text = "Display Cover";
      this.displayCoverToolStripMenuItem.Click += new EventHandler(this.displayCoverToolStripMenuItem_Click);
      this.hideCoverToolStripMenuItem.Name = "hideCoverToolStripMenuItem";
      this.hideCoverToolStripMenuItem.ShortcutKeys = Keys.H | Keys.Control | Keys.Alt;
      this.hideCoverToolStripMenuItem.Size = new Size(233, 22);
      this.hideCoverToolStripMenuItem.Text = "Hide Cover";
      this.hideCoverToolStripMenuItem.Click += new EventHandler(this.hideCoverToolStripMenuItem_Click);
      this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
      this.openFileToolStripMenuItem.ShortcutKeys = Keys.B | Keys.Control | Keys.Alt;
      this.openFileToolStripMenuItem.Size = new Size(233, 22);
      this.openFileToolStripMenuItem.Text = "Open Book";
      this.openFileToolStripMenuItem.Click += new EventHandler(this.openFileToolStripMenuItem_Click);
      this.openTorrentToolStripMenuItem.Name = "openTorrentToolStripMenuItem";
      this.openTorrentToolStripMenuItem.ShortcutKeys = Keys.O | Keys.Control | Keys.Alt;
      this.openTorrentToolStripMenuItem.Size = new Size(233, 22);
      this.openTorrentToolStripMenuItem.Text = "Open Torrent";
      this.openTorrentToolStripMenuItem.Click += new EventHandler(this.openTorrentToolStripMenuItem_Click);
      this.removeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.redsToolStripMenuItem,
        (ToolStripItem) this.tealsToolStripMenuItem,
        (ToolStripItem) this.greensToolStripMenuItem
      });
      this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
      this.removeToolStripMenuItem.Size = new Size(233, 22);
      this.removeToolStripMenuItem.Text = "Remove";
      this.redsToolStripMenuItem.Name = "redsToolStripMenuItem";
      this.redsToolStripMenuItem.ShortcutKeys = Keys.R | Keys.Control | Keys.Alt;
      this.redsToolStripMenuItem.Size = new Size(175, 22);
      this.redsToolStripMenuItem.Text = "Reds";
      this.redsToolStripMenuItem.Click += new EventHandler(this.redsToolStripMenuItem_Click);
      this.tealsToolStripMenuItem.Name = "tealsToolStripMenuItem";
      this.tealsToolStripMenuItem.ShortcutKeys = Keys.T | Keys.Control | Keys.Alt;
      this.tealsToolStripMenuItem.Size = new Size(175, 22);
      this.tealsToolStripMenuItem.Text = "Teals";
      this.tealsToolStripMenuItem.Click += new EventHandler(this.tealsToolStripMenuItem_Click);
      this.greensToolStripMenuItem.Name = "greensToolStripMenuItem";
      this.greensToolStripMenuItem.ShortcutKeys = Keys.G | Keys.Control | Keys.Alt;
      this.greensToolStripMenuItem.Size = new Size(175, 22);
      this.greensToolStripMenuItem.Text = "Greens";
      this.greensToolStripMenuItem.Click += new EventHandler(this.greensToolStripMenuItem_Click);
      this.statusStrip.BackColor = SystemColors.ControlLight;
      this.statusStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.statusLbl,
        (ToolStripItem) this.versionLbl
      });
      this.statusStrip.Location = new Point(0, 645);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(862, 22);
      this.statusStrip.TabIndex = 16;
      this.statusStrip.Text = "Status Strip";
      this.statusLbl.Name = "statusLbl";
      this.statusLbl.Size = new Size(795, 17);
      this.statusLbl.Spring = true;
      this.statusLbl.Text = "Not Logged In";
      this.statusLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.versionLbl.Name = "versionLbl";
      this.versionLbl.Size = new Size(52, 17);
      this.versionLbl.Text = "Version: ";
      this.progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar.Location = new Point(0, 613);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(862, 32);
      this.progressBar.Style = ProgressBarStyle.Continuous;
      this.progressBar.TabIndex = 15;
      this.folderWorker.WorkerReportsProgress = true;
      this.folderWorker.WorkerSupportsCancellation = true;
      this.folderWorker.DoWork += new DoWorkEventHandler(this.folderWorker_DoWork);
      this.folderWorker.ProgressChanged += new ProgressChangedEventHandler(this.folderWorker_ProgressChanged);
      this.renameWorker.WorkerReportsProgress = true;
      this.renameWorker.WorkerSupportsCancellation = true;
      this.renameWorker.DoWork += new DoWorkEventHandler(this.renameWorker_DoWork);
      this.renameWorker.ProgressChanged += new ProgressChangedEventHandler(this.renameWorker_ProgressChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(46, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Authors:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(39, 47);
      this.label2.Name = "label2";
      this.label2.Size = new Size(30, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Title:";
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(463, 113);
      this.label3.Name = "label3";
      this.label3.Size = new Size(53, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Publisher:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(35, 113);
      this.label4.Name = "label4";
      this.label4.Size = new Size(34, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Tags:";
      this.authorsBox.AccessibleName = "Authors Box";
      this.authorsBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.authorsBox.Location = new Point(73, 11);
      this.authorsBox.Name = "authorsBox";
      this.authorsBox.Size = new Size(381, 20);
      this.authorsBox.TabIndex = 3;
      this.titleBox.AccessibleName = "Title Box";
      this.titleBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.titleBox.Location = new Point(75, 44);
      this.titleBox.Name = "titleBox";
      this.titleBox.Size = new Size(379, 20);
      this.titleBox.TabIndex = 4;
      this.publisherBox.AccessibleName = "Publisher Box";
      this.publisherBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.publisherBox.Location = new Point(522, 110);
      this.publisherBox.Name = "publisherBox";
      this.publisherBox.Size = new Size(248, 20);
      this.publisherBox.TabIndex = 12;
      this.tagsBox.AccessibleName = "Tags Box";
      this.tagsBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tagsBox.Location = new Point(75, 110);
      this.tagsBox.Name = "tagsBox";
      this.tagsBox.Size = new Size(379, 20);
      this.tagsBox.TabIndex = 6;
      this.label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(463, 14);
      this.label5.Name = "label5";
      this.label5.Size = new Size(35, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "ISBN:";
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(460, 47);
      this.label6.Name = "label6";
      this.label6.Size = new Size(40, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Pages:";
      this.label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(584, 47);
      this.label7.Name = "label7";
      this.label7.Size = new Size(32, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Year:";
      this.label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(465, 80);
      this.label8.Name = "label8";
      this.label8.Size = new Size(39, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "Image:";
      this.isbnBox.AccessibleName = "ISBN Box";
      this.isbnBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.isbnBox.Location = new Point(504, 11);
      this.isbnBox.Name = "isbnBox";
      this.isbnBox.Size = new Size(266, 20);
      this.isbnBox.TabIndex = 7;
      this.imageBox.AccessibleName = "Image Box";
      this.imageBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.imageBox.Location = new Point(506, 77);
      this.imageBox.Name = "imageBox";
      this.imageBox.Size = new Size(264, 20);
      this.imageBox.TabIndex = 11;
      this.pagesBox.AccessibleDescription = "Pages in book";
      this.pagesBox.AccessibleName = "Pages Box";
      this.pagesBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pagesBox.Location = new Point(506, 45);
      this.pagesBox.Maximum = new Decimal(new int[4]
      {
        100000,
        0,
        0,
        0
      });
      this.pagesBox.Name = "pagesBox";
      this.pagesBox.Size = new Size(72, 20);
      this.pagesBox.TabIndex = 8;
      this.yearBox.AccessibleDescription = "Year published";
      this.yearBox.AccessibleName = "Year Box";
      this.yearBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.yearBox.Location = new Point(622, 45);
      this.yearBox.Maximum = new Decimal(new int[4]
      {
        2030,
        0,
        0,
        0
      });
      this.yearBox.Name = "yearBox";
      this.yearBox.Size = new Size(72, 20);
      this.yearBox.TabIndex = 9;
      this.retailBox.AccessibleName = "Retail Box";
      this.retailBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.retailBox.AutoSize = true;
      this.retailBox.CheckAlign = ContentAlignment.MiddleRight;
      this.retailBox.Location = new Point(700, 47);
      this.retailBox.Name = "retailBox";
      this.retailBox.Size = new Size(56, 17);
      this.retailBox.TabIndex = 10;
      this.retailBox.Text = "Retai&l:";
      this.retailBox.UseVisualStyleBackColor = true;
      this.descriptionBox.AccessibleName = "Description Box";
      this.descriptionBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.descriptionBox.BorderStyle = BorderStyle.None;
      this.descriptionBox.Location = new Point(75, 145);
      this.descriptionBox.Name = "descriptionBox";
      this.descriptionBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
      this.descriptionBox.Size = new Size(751, 82);
      this.descriptionBox.TabIndex = 13;
      this.descriptionBox.Text = "";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 145);
      this.label10.Name = "label10";
      this.label10.Size = new Size(63, 13);
      this.label10.TabIndex = 21;
      this.label10.Text = "Description:";
      this.saveBtn.AccessibleName = "Save";
      this.saveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.saveBtn.Location = new Point(779, 31);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(74, 83);
      this.saveBtn.TabIndex = 14;
      this.saveBtn.Text = "Sa&ve";
      this.saveBtn.UseVisualStyleBackColor = true;
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.uploadList.AllowColumnReorder = true;
      this.uploadList.BackColor = SystemColors.ScrollBar;
      this.uploadList.ContextMenuStrip = this.uploadListMenu;
      this.uploadList.Dock = DockStyle.Fill;
      this.uploadList.FullRowSelect = true;
      this.uploadList.GridLines = true;
      this.uploadList.HideSelection = false;
      this.uploadList.Location = new Point(0, 0);
      this.uploadList.Name = "uploadList";
      this.uploadList.Size = new Size(862, 312);
      this.uploadList.Sorting = SortOrder.Ascending;
      this.uploadList.TabIndex = 2;
      this.uploadList.UseCompatibleStateImageBehavior = false;
      this.uploadList.View = View.Details;
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.FixedPanel = FixedPanel.Panel2;
      this.splitContainer1.Location = new Point(0, 56);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.uploadList);
      this.splitContainer1.Panel2.AutoScroll = true;
      this.splitContainer1.Panel2.Controls.Add((Control) this.label11);
      this.splitContainer1.Panel2.Controls.Add((Control) this.fileBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.tagsBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.publisherBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label5);
      this.splitContainer1.Panel2.Controls.Add((Control) this.saveBtn);
      this.splitContainer1.Panel2.Controls.Add((Control) this.titleBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label10);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label6);
      this.splitContainer1.Panel2.Controls.Add((Control) this.descriptionBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.authorsBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.retailBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label7);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label4);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label8);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label3);
      this.splitContainer1.Panel2.Controls.Add((Control) this.yearBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.isbnBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.pagesBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.imageBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label1);
      this.splitContainer1.Size = new Size(862, 557);
      this.splitContainer1.SplitterDistance = 312;
      this.splitContainer1.TabIndex = 1;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(34, 80);
      this.label11.Name = "label11";
      this.label11.Size = new Size(38, 13);
      this.label11.TabIndex = 23;
      this.label11.Text = "Name:";
      this.fileBox.AccessibleName = "File Name Box";
      this.fileBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.fileBox.Location = new Point(75, 77);
      this.fileBox.Name = "fileBox";
      this.fileBox.ReadOnly = true;
      this.fileBox.Size = new Size(379, 20);
      this.fileBox.TabIndex = 5;
      this.webWorker.WorkerReportsProgress = true;
      this.webWorker.WorkerSupportsCancellation = true;
      this.azw3Btn.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.azw3Btn.Image = (Image) componentResourceManager.GetObject("azw3Btn.Image");
      this.azw3Btn.ImageTransparentColor = Color.Magenta;
      this.azw3Btn.Name = "azw3Btn";
      this.azw3Btn.Size = new Size(43, 53);
      this.azw3Btn.Text = "A&ZW3";
      this.azw3Btn.Click += new EventHandler(this.azw3Btn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlDark;
      this.ClientSize = new Size(862, 667);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.toolStrip);
      this.Controls.Add((Control) this.statusStrip);
      this.Name = nameof (BiblioUpTik);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (BiblioUpTik);
      this.toolStrip.ResumeLayout(false);
      this.toolStrip.PerformLayout();
      this.uploadListMenu.ResumeLayout(false);
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.pagesBox.EndInit();
      this.yearBox.EndInit();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void AddItemCallback(object o);

    private delegate void ChangeWidthCallback(object o);

    private delegate void RenameListCallback(object o, FileInfo[] files);

    private delegate void GetLVICallback(object o);

    private delegate void RemoveItemCallback(object o);

        internal class Properties
        {
        }
    }
}
