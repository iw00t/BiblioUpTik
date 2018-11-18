using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BiblioUpTik
{
  public class Settings : Form
  {
    private IContainer components = (IContainer) null;
    private TabControl settingsCtrl;
    private TabPage loginTab;
    private TextBox passwordBox;
    private TextBox usernameBox;
    private Label label2;
    private Label label1;
    private TabPage generalPage;
    private Button saveBtn;
    private Label label3;
    private NumericUpDown delayBox;
    private TextBox templateBox;
    private Label label4;
    private Button saveBtn1;
    private CheckBox addBox;
    private CheckBox coverBox;
    private CheckBox publisherBox;
    private CheckBox pagesBox;
    private CheckBox secureBox;
    private Label label10;
    private CheckBox descriptionBox;
    private Label label12;
    private Label label11;
    private NumericUpDown heightBox;
    private NumericUpDown widthBox;
    private CheckBox showCoverBox;
    private TextBox announceBox;
    private Label label14;

    public Settings()
    {
      this.InitializeComponent();
      this.usernameBox.Text = BiblioUpTik.Properties.Settings.Default.Username;
      this.passwordBox.Text = Protect.ToInsecureString(Protect.DecryptString(BiblioUpTik.Properties.Settings.Default.Password));
      this.delayBox.Value = BiblioUpTik.Properties.Settings.Default.Delay >= 100 ? (BiblioUpTik.Properties.Settings.Default.Delay <= 5000 ? (Decimal) BiblioUpTik.Properties.Settings.Default.Delay : new Decimal(5000)) : new Decimal(100);
      this.templateBox.Text = BiblioUpTik.Properties.Settings.Default.Template;
      this.addBox.Checked = BiblioUpTik.Properties.Settings.Default.OnRun;
      this.coverBox.Checked = BiblioUpTik.Properties.Settings.Default.GetCover;
      this.pagesBox.Checked = BiblioUpTik.Properties.Settings.Default.GetPages;
      this.publisherBox.Checked = BiblioUpTik.Properties.Settings.Default.GetPublisher;
      this.descriptionBox.Checked = BiblioUpTik.Properties.Settings.Default.GetDescription;
      this.secureBox.Checked = BiblioUpTik.Properties.Settings.Default.Secure;
      this.widthBox.Value = (Decimal) BiblioUpTik.Properties.Settings.Default.Width;
      this.heightBox.Value = (Decimal) BiblioUpTik.Properties.Settings.Default.Height;
      this.showCoverBox.Checked = BiblioUpTik.Properties.Settings.Default.ShowCover;
      this.announceBox.Text = Protect.ToInsecureString(Protect.DecryptString(BiblioUpTik.Properties.Settings.Default.AnnounceURL));
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.Username = this.usernameBox.Text;
      BiblioUpTik.Properties.Settings.Default.Password = Protect.EncryptString(Protect.ToSecureString(this.passwordBox.Text));
      BiblioUpTik.Properties.Settings.Default.Delay = Decimal.ToInt32(this.delayBox.Value);
      BiblioUpTik.Properties.Settings.Default.Secure = this.secureBox.Checked;
      BiblioUpTik.Properties.Settings.Default.Save();
      int num = (int) MessageBox.Show("Login Information Saved!");
    }

    private void saveBtn1_Click(object sender, EventArgs e)
    {
      BiblioUpTik.Properties.Settings.Default.Template = this.templateBox.Text;
      BiblioUpTik.Properties.Settings.Default.OnRun = this.addBox.Checked;
      BiblioUpTik.Properties.Settings.Default.GetCover = this.coverBox.Checked;
      BiblioUpTik.Properties.Settings.Default.GetPages = this.pagesBox.Checked;
      BiblioUpTik.Properties.Settings.Default.GetPublisher = this.publisherBox.Checked;
      BiblioUpTik.Properties.Settings.Default.GetDescription = this.descriptionBox.Checked;
      BiblioUpTik.Properties.Settings.Default.Width = (int) this.widthBox.Value;
      BiblioUpTik.Properties.Settings.Default.Height = (int) this.heightBox.Value;
      BiblioUpTik.Properties.Settings.Default.ShowCover = this.showCoverBox.Checked;
      BiblioUpTik.Properties.Settings.Default.AnnounceURL = Protect.EncryptString(Protect.ToSecureString(this.announceBox.Text));
      BiblioUpTik.Properties.Settings.Default.Save();
      int num = (int) MessageBox.Show("Settings Saved!");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.settingsCtrl = new TabControl();
      this.loginTab = new TabPage();
      this.secureBox = new CheckBox();
      this.label10 = new Label();
      this.delayBox = new NumericUpDown();
      this.label3 = new Label();
      this.saveBtn = new Button();
      this.passwordBox = new TextBox();
      this.usernameBox = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.generalPage = new TabPage();
      this.announceBox = new TextBox();
      this.label14 = new Label();
      this.showCoverBox = new CheckBox();
      this.heightBox = new NumericUpDown();
      this.widthBox = new NumericUpDown();
      this.label12 = new Label();
      this.label11 = new Label();
      this.descriptionBox = new CheckBox();
      this.pagesBox = new CheckBox();
      this.publisherBox = new CheckBox();
      this.addBox = new CheckBox();
      this.coverBox = new CheckBox();
      this.saveBtn1 = new Button();
      this.templateBox = new TextBox();
      this.label4 = new Label();
      this.settingsCtrl.SuspendLayout();
      this.loginTab.SuspendLayout();
      this.delayBox.BeginInit();
      this.generalPage.SuspendLayout();
      this.heightBox.BeginInit();
      this.widthBox.BeginInit();
      this.SuspendLayout();
      this.settingsCtrl.Controls.Add((Control) this.loginTab);
      this.settingsCtrl.Controls.Add((Control) this.generalPage);
      this.settingsCtrl.Dock = DockStyle.Fill;
      this.settingsCtrl.Location = new Point(0, 0);
      this.settingsCtrl.Name = "settingsCtrl";
      this.settingsCtrl.SelectedIndex = 0;
      this.settingsCtrl.Size = new Size(319, 244);
      this.settingsCtrl.TabIndex = 0;
      this.loginTab.BackColor = SystemColors.ControlDark;
      this.loginTab.Controls.Add((Control) this.secureBox);
      this.loginTab.Controls.Add((Control) this.label10);
      this.loginTab.Controls.Add((Control) this.delayBox);
      this.loginTab.Controls.Add((Control) this.label3);
      this.loginTab.Controls.Add((Control) this.saveBtn);
      this.loginTab.Controls.Add((Control) this.passwordBox);
      this.loginTab.Controls.Add((Control) this.usernameBox);
      this.loginTab.Controls.Add((Control) this.label2);
      this.loginTab.Controls.Add((Control) this.label1);
      this.loginTab.Location = new Point(4, 22);
      this.loginTab.Name = "loginTab";
      this.loginTab.Padding = new Padding(3);
      this.loginTab.Size = new Size(311, 218);
      this.loginTab.TabIndex = 0;
      this.loginTab.Text = "Login Info";
      this.secureBox.AutoSize = true;
      this.secureBox.Enabled = false;
      this.secureBox.Location = new Point(157, 162);
      this.secureBox.Name = "secureBox";
      this.secureBox.Size = new Size(15, 14);
      this.secureBox.TabIndex = 8;
      this.secureBox.UseVisualStyleBackColor = true;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(50, 162);
      this.label10.Name = "label10";
      this.label10.Size = new Size(101, 13);
      this.label10.TabIndex = 7;
      this.label10.Text = "Secure Connection:";
      this.delayBox.Increment = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.delayBox.Location = new Point(111, 128);
      this.delayBox.Maximum = new Decimal(new int[4]
      {
        5000,
        0,
        0,
        0
      });
      this.delayBox.Minimum = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.delayBox.Name = "delayBox";
      this.delayBox.Size = new Size(113, 20);
      this.delayBox.TabIndex = 3;
      this.delayBox.Value = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.label3.AutoSize = true;
      this.label3.ForeColor = SystemColors.ControlText;
      this.label3.Location = new Point(47, 131);
      this.label3.Name = "label3";
      this.label3.Size = new Size(56, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Delay (ms)";
      this.saveBtn.Location = new Point(111, 187);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 23);
      this.saveBtn.TabIndex = 4;
      this.saveBtn.Text = "Sa&ve";
      this.saveBtn.UseVisualStyleBackColor = true;
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.passwordBox.Location = new Point(111, 74);
      this.passwordBox.Name = "passwordBox";
      this.passwordBox.Size = new Size(113, 20);
      this.passwordBox.TabIndex = 2;
      this.passwordBox.UseSystemPasswordChar = true;
      this.usernameBox.Location = new Point(111, 20);
      this.usernameBox.Name = "usernameBox";
      this.usernameBox.Size = new Size(113, 20);
      this.usernameBox.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.ForeColor = SystemColors.WindowText;
      this.label2.Location = new Point(50, 77);
      this.label2.Name = "label2";
      this.label2.Size = new Size(53, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Password";
      this.label1.AutoSize = true;
      this.label1.ForeColor = SystemColors.WindowText;
      this.label1.Location = new Point(50, 23);
      this.label1.Name = "label1";
      this.label1.Size = new Size(55, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Username";
      this.generalPage.BackColor = SystemColors.ControlDark;
      this.generalPage.Controls.Add((Control) this.announceBox);
      this.generalPage.Controls.Add((Control) this.label14);
      this.generalPage.Controls.Add((Control) this.showCoverBox);
      this.generalPage.Controls.Add((Control) this.heightBox);
      this.generalPage.Controls.Add((Control) this.widthBox);
      this.generalPage.Controls.Add((Control) this.label12);
      this.generalPage.Controls.Add((Control) this.label11);
      this.generalPage.Controls.Add((Control) this.descriptionBox);
      this.generalPage.Controls.Add((Control) this.pagesBox);
      this.generalPage.Controls.Add((Control) this.publisherBox);
      this.generalPage.Controls.Add((Control) this.coverBox);
      this.generalPage.Controls.Add((Control) this.saveBtn1);
      this.generalPage.Controls.Add((Control) this.templateBox);
      this.generalPage.Controls.Add((Control) this.label4);
      this.generalPage.Controls.Add((Control) this.addBox);
      this.generalPage.Location = new Point(4, 22);
      this.generalPage.Name = "generalPage";
      this.generalPage.Padding = new Padding(3);
      this.generalPage.Size = new Size(311, 218);
      this.generalPage.TabIndex = 1;
      this.generalPage.Text = "General";
      this.announceBox.Location = new Point(63, 152);
      this.announceBox.Name = "announceBox";
      this.announceBox.Size = new Size(240, 20);
      this.announceBox.TabIndex = 10;
      this.announceBox.WordWrap = false;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(3, 155);
      this.label14.Name = "label14";
      this.label14.Size = new Size(59, 13);
      this.label14.TabIndex = 20;
      this.label14.Text = "Announce:";
      this.showCoverBox.AutoSize = true;
      this.showCoverBox.CheckAlign = ContentAlignment.MiddleRight;
      this.showCoverBox.Location = new Point(193, 71);
      this.showCoverBox.Name = "showCoverBox";
      this.showCoverBox.Size = new Size(89, 17);
      this.showCoverBox.TabIndex = 7;
      this.showCoverBox.Text = "&Show Covers";
      this.showCoverBox.UseVisualStyleBackColor = true;
      this.heightBox.Location = new Point(211, 116);
      this.heightBox.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.heightBox.Name = "heightBox";
      this.heightBox.Size = new Size(50, 20);
      this.heightBox.TabIndex = 9;
      this.widthBox.Location = new Point(79, 116);
      this.widthBox.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.widthBox.Name = "widthBox";
      this.widthBox.Size = new Size(50, 20);
      this.widthBox.TabIndex = 8;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(135, 118);
      this.label12.Name = "label12";
      this.label12.Size = new Size(70, 13);
      this.label12.TabIndex = 15;
      this.label12.Text = "Image Width:";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(3, 118);
      this.label11.Name = "label11";
      this.label11.Size = new Size(70, 13);
      this.label11.TabIndex = 13;
      this.label11.Text = "Image Width:";
      this.descriptionBox.AutoSize = true;
      this.descriptionBox.CheckAlign = ContentAlignment.MiddleRight;
      this.descriptionBox.Location = new Point(88, 71);
      this.descriptionBox.Name = "descriptionBox";
      this.descriptionBox.Size = new Size(99, 17);
      this.descriptionBox.TabIndex = 6;
      this.descriptionBox.Text = "Get &Description";
      this.descriptionBox.UseVisualStyleBackColor = true;
      this.pagesBox.AutoSize = true;
      this.pagesBox.CheckAlign = ContentAlignment.MiddleRight;
      this.pagesBox.Location = new Point(6, 71);
      this.pagesBox.Name = "pagesBox";
      this.pagesBox.Size = new Size(76, 17);
      this.pagesBox.TabIndex = 5;
      this.pagesBox.Text = "Get Pa&ges";
      this.pagesBox.UseVisualStyleBackColor = true;
      this.publisherBox.AutoSize = true;
      this.publisherBox.CheckAlign = ContentAlignment.MiddleRight;
      this.publisherBox.Location = new Point(168, 48);
      this.publisherBox.Name = "publisherBox";
      this.publisherBox.Size = new Size(89, 17);
      this.publisherBox.TabIndex = 4;
      this.publisherBox.Text = "Get &Publisher";
      this.publisherBox.UseVisualStyleBackColor = true;
      this.addBox.AutoSize = true;
      this.addBox.CheckAlign = ContentAlignment.MiddleRight;
      this.addBox.ForeColor = SystemColors.ControlText;
      this.addBox.Location = new Point(6, 48);
      this.addBox.Name = "addBox";
      this.addBox.Size = new Size(76, 17);
      this.addBox.TabIndex = 2;
      this.addBox.Text = "On &Adding";
      this.addBox.UseVisualStyleBackColor = true;
      this.coverBox.AutoSize = true;
      this.coverBox.CheckAlign = ContentAlignment.MiddleRight;
      this.coverBox.Location = new Point(88, 48);
      this.coverBox.Name = "coverBox";
      this.coverBox.Size = new Size(74, 17);
      this.coverBox.TabIndex = 3;
      this.coverBox.Text = "Get &Cover";
      this.coverBox.UseVisualStyleBackColor = true;
      this.saveBtn1.Location = new Point(99, 187);
      this.saveBtn1.Name = "saveBtn1";
      this.saveBtn1.Size = new Size(75, 23);
      this.saveBtn1.TabIndex = 11;
      this.saveBtn1.Text = "Sa&ve";
      this.saveBtn1.UseVisualStyleBackColor = true;
      this.saveBtn1.Click += new EventHandler(this.saveBtn1_Click);
      this.templateBox.Location = new Point(63, 18);
      this.templateBox.Name = "templateBox";
      this.templateBox.Size = new Size(240, 20);
      this.templateBox.TabIndex = 1;
      this.templateBox.Text = "%author% - %title% (%subtitle%)";
      this.templateBox.WordWrap = false;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 21);
      this.label4.Name = "label4";
      this.label4.Size = new Size(54, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Template:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlDark;
      this.ClientSize = new Size(319, 244);
      this.Controls.Add((Control) this.settingsCtrl);
      this.Name = nameof (Settings);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (Settings);
      this.TopMost = true;
      this.settingsCtrl.ResumeLayout(false);
      this.loginTab.ResumeLayout(false);
      this.loginTab.PerformLayout();
      this.delayBox.EndInit();
      this.generalPage.ResumeLayout(false);
      this.generalPage.PerformLayout();
      this.heightBox.EndInit();
      this.widthBox.EndInit();
      this.ResumeLayout(false);
    }
  }
}
