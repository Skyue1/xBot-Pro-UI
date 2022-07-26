using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace xBot_Pro_UI;

public class ManageMacro : Form
{
	public struct MARGINS
	{
		public int leftWidth;

		public int rightWidth;

		public int topHeight;

		public int bottomHeight;
	}

	public string macroName;

	private bool m_aeroEnabled;

	private const int CS_DROPSHADOW = 131072;

	private const int WM_NCPAINT = 133;

	private const int WM_ACTIVATEAPP = 28;

	private const int WM_NCHITTEST = 132;

	private const int HTCLIENT = 1;

	private const int HTCAPTION = 2;

	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	private IContainer components;

	private GroupBox gb_upload;

	private TextBox txt_macroName;

	private Label label1;

	private CheckBox cb_anonymously;

	private Button btn_start_up;

	private TextBox txt_levelid;

	private Label label2;

	private GroupBox gb_local;

	private TextBox txt_rename;

	private Button btn_apply;

	private Button btn_delete;

	private Label label3;

	private Button btn_close;

	private StatusStrip statusStrip1;

	private Timer tmr_prevent_max;

	protected override CreateParams CreateParams
	{
		get
		{
			m_aeroEnabled = CheckAeroEnabled();
			CreateParams createParams = base.CreateParams;
			if (!m_aeroEnabled)
			{
				createParams.ClassStyle |= 131072;
			}
			return createParams;
		}
	}

	[DllImport("Gdi32.dll")]
	private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	[DllImport("dwmapi.dll")]
	public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

	[DllImport("dwmapi.dll")]
	public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

	private bool CheckAeroEnabled()
	{
		if (Environment.OSVersion.Version.Major >= 6)
		{
			int pfEnabled = 0;
			DwmIsCompositionEnabled(ref pfEnabled);
			if (pfEnabled != 1)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 133 && m_aeroEnabled)
		{
			int attrValue = 2;
			DwmSetWindowAttribute(base.Handle, 2, ref attrValue, 4);
			MARGINS mARGINS = default(MARGINS);
			mARGINS.bottomHeight = 1;
			mARGINS.leftWidth = 0;
			mARGINS.rightWidth = 0;
			mARGINS.topHeight = 0;
			MARGINS pMarInset = mARGINS;
			DwmExtendFrameIntoClientArea(base.Handle, ref pMarInset);
		}
		base.WndProc(ref m);
		if (m.Msg == 132 && (int)m.Result == 1)
		{
			m.Result = (IntPtr)2;
		}
	}

	public ManageMacro(string macro)
	{
		InitializeComponent();
		macroName = macro;
		txt_macroName.Text = macroName;
	}

	private void btn_apply_Click(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			File.Move(text + macroName, text + txt_rename.Text);
		}
		catch
		{
			MessageBox.Show("Error");
			return;
		}
		macroName = txt_rename.Text;
		txt_macroName.Text = macroName;
		MessageBox.Show("Macro renamed successfully!");
	}

	private void btn_delete_Click(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			File.Delete(text + macroName);
		}
		catch
		{
			MessageBox.Show("Error");
			return;
		}
		MessageBox.Show("Macro deleted successfully!");
		Close();
	}

	private void btn_start_up_Click(object sender, EventArgs e)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string value = "";
		text = macroName;
		text2 = txt_levelid.Text;
		text3 = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", macroName));
		if (!text3.Contains("pro") && !text3.Contains("frames") && !text3.Contains("pro_plus"))
		{
			MessageBox.Show("Macro not supported!", "Error");
			return;
		}
		if (text3.Contains("pro") && !text3.Contains("pro_plus"))
		{
			value = "Pro";
		}
		else if (text3.Contains("frames"))
		{
			value = "Frames";
		}
		else if (text3.Contains("pro_plus"))
		{
			value = "Pro+";
		}
		string text4 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text3));
		string text5 = Convert.ToBase64String(Encoding.UTF8.GetBytes(getMac()));
		WebClient webClient = new WebClient();
		byte[] bytes = webClient.DownloadData("http://xbot.4uhr20.eu/login.php?clientid=" + text5);
		string @string = Encoding.ASCII.GetString(bytes);
		if (!new Form1().checkLicnese(@string.Split(':')[0]))
		{
			MessageBox.Show("Invalid License!");
			return;
		}
		if (cb_anonymously.Checked)
		{
			text5 = "";
		}
		text4 = text4.Replace('=', '_');
		text5 = text5.Replace('=', '_');
		NameValueCollection nameValueCollection = new NameValueCollection();
		nameValueCollection["name"] = text;
		nameValueCollection["macro"] = text4;
		nameValueCollection["creator"] = text5;
		nameValueCollection["lid"] = text2;
		nameValueCollection["type"] = value;
		bytes = webClient.UploadValues("http://xbot.4uhr20.eu/macroUp.php", nameValueCollection);
		string string2 = Encoding.ASCII.GetString(bytes);
		if (string2.Contains("-2:"))
		{
			MessageBox.Show("Macro is already availible under the name " + string2.Split(':')[1] + "!");
		}
		else if (string2 == "0")
		{
			MessageBox.Show("Uploaded successfully!");
		}
	}

	private string getMac()
	{
		return ProcessString((from nic in NetworkInterface.GetAllNetworkInterfaces()
			where nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
			select nic.GetPhysicalAddress().ToString()).FirstOrDefault());
	}

	private static string ProcessString(string input)
	{
		StringBuilder stringBuilder = new StringBuilder(input.Length * 3 / 2);
		for (int i = 0; i < input.Length; i++)
		{
			if ((i > 0) & (i % 2 == 0))
			{
				stringBuilder.Append(":");
			}
			stringBuilder.Append(input[i]);
		}
		return stringBuilder.ToString();
	}

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	private void ManageMacro_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ReleaseCapture();
			SendMessage(base.Handle, 161, 2, 0);
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void tmr_prevent_max_Tick(object sender, EventArgs e)
	{
		if (base.WindowState == FormWindowState.Maximized)
		{
			base.WindowState = FormWindowState.Normal;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xBot_Pro_UI.ManageMacro));
		this.gb_upload = new System.Windows.Forms.GroupBox();
		this.txt_macroName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cb_anonymously = new System.Windows.Forms.CheckBox();
		this.btn_start_up = new System.Windows.Forms.Button();
		this.txt_levelid = new System.Windows.Forms.TextBox();
		this.gb_local = new System.Windows.Forms.GroupBox();
		this.txt_rename = new System.Windows.Forms.TextBox();
		this.btn_apply = new System.Windows.Forms.Button();
		this.btn_delete = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.btn_close = new System.Windows.Forms.Button();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.tmr_prevent_max = new System.Windows.Forms.Timer(this.components);
		this.gb_upload.SuspendLayout();
		this.gb_local.SuspendLayout();
		base.SuspendLayout();
		this.gb_upload.Controls.Add(this.txt_macroName);
		this.gb_upload.Controls.Add(this.label2);
		this.gb_upload.Controls.Add(this.label1);
		this.gb_upload.Controls.Add(this.cb_anonymously);
		this.gb_upload.Controls.Add(this.btn_start_up);
		this.gb_upload.Controls.Add(this.txt_levelid);
		this.gb_upload.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_upload.Location = new System.Drawing.Point(140, 35);
		this.gb_upload.Name = "gb_upload";
		this.gb_upload.Size = new System.Drawing.Size(217, 163);
		this.gb_upload.TabIndex = 1;
		this.gb_upload.TabStop = false;
		this.gb_upload.Text = "Upload";
		this.txt_macroName.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_macroName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_macroName.Enabled = false;
		this.txt_macroName.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_macroName.Location = new System.Drawing.Point(97, 30);
		this.txt_macroName.Name = "txt_macroName";
		this.txt_macroName.Size = new System.Drawing.Size(100, 20);
		this.txt_macroName.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(19, 33);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 13);
		this.label2.TabIndex = 2;
		this.label2.Text = "Macro name:";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(19, 59);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(50, 13);
		this.label1.TabIndex = 2;
		this.label1.Text = "Level ID:";
		this.cb_anonymously.AutoSize = true;
		this.cb_anonymously.Location = new System.Drawing.Point(97, 82);
		this.cb_anonymously.Name = "cb_anonymously";
		this.cb_anonymously.Size = new System.Drawing.Size(115, 17);
		this.cb_anonymously.TabIndex = 1;
		this.cb_anonymously.Text = "anonymous upload";
		this.cb_anonymously.UseVisualStyleBackColor = true;
		this.btn_start_up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_start_up.Location = new System.Drawing.Point(97, 119);
		this.btn_start_up.Name = "btn_start_up";
		this.btn_start_up.Size = new System.Drawing.Size(75, 23);
		this.btn_start_up.TabIndex = 0;
		this.btn_start_up.Text = "Upload";
		this.btn_start_up.UseVisualStyleBackColor = true;
		this.btn_start_up.Click += new System.EventHandler(btn_start_up_Click);
		this.txt_levelid.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_levelid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_levelid.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_levelid.Location = new System.Drawing.Point(97, 56);
		this.txt_levelid.Name = "txt_levelid";
		this.txt_levelid.Size = new System.Drawing.Size(100, 20);
		this.txt_levelid.TabIndex = 0;
		this.gb_local.Controls.Add(this.txt_rename);
		this.gb_local.Controls.Add(this.btn_apply);
		this.gb_local.Controls.Add(this.btn_delete);
		this.gb_local.Controls.Add(this.label3);
		this.gb_local.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_local.Location = new System.Drawing.Point(12, 35);
		this.gb_local.Name = "gb_local";
		this.gb_local.Size = new System.Drawing.Size(122, 163);
		this.gb_local.TabIndex = 2;
		this.gb_local.TabStop = false;
		this.gb_local.Text = "Local";
		this.txt_rename.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_rename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_rename.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_rename.Location = new System.Drawing.Point(20, 52);
		this.txt_rename.Name = "txt_rename";
		this.txt_rename.Size = new System.Drawing.Size(80, 20);
		this.txt_rename.TabIndex = 3;
		this.btn_apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_apply.Location = new System.Drawing.Point(20, 76);
		this.btn_apply.Name = "btn_apply";
		this.btn_apply.Size = new System.Drawing.Size(80, 23);
		this.btn_apply.TabIndex = 0;
		this.btn_apply.Text = "Apply";
		this.btn_apply.UseVisualStyleBackColor = true;
		this.btn_apply.Click += new System.EventHandler(btn_apply_Click);
		this.btn_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_delete.Location = new System.Drawing.Point(20, 119);
		this.btn_delete.Name = "btn_delete";
		this.btn_delete.Size = new System.Drawing.Size(80, 23);
		this.btn_delete.TabIndex = 0;
		this.btn_delete.Text = "delete";
		this.btn_delete.UseVisualStyleBackColor = true;
		this.btn_delete.Click += new System.EventHandler(btn_delete_Click);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(17, 30);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(83, 13);
		this.label3.TabIndex = 2;
		this.label3.Text = "Rename Macro:";
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close.ForeColor = System.Drawing.Color.Red;
		this.btn_close.Location = new System.Drawing.Point(350, 3);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(17, 17);
		this.btn_close.TabIndex = 6;
		this.btn_close.UseVisualStyleBackColor = true;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.statusStrip1.BackgroundImage = (System.Drawing.Image)resources.GetObject("statusStrip1.BackgroundImage");
		this.statusStrip1.Location = new System.Drawing.Point(0, 188);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(370, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 7;
		this.statusStrip1.Text = "statusStrip1";
		this.tmr_prevent_max.Enabled = true;
		this.tmr_prevent_max.Interval = 50;
		this.tmr_prevent_max.Tick += new System.EventHandler(tmr_prevent_max_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		base.ClientSize = new System.Drawing.Size(370, 210);
		base.Controls.Add(this.btn_close);
		base.Controls.Add(this.gb_local);
		base.Controls.Add(this.gb_upload);
		base.Controls.Add(this.statusStrip1);
		this.ForeColor = System.Drawing.Color.Gainsboro;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "ManageMacro";
		this.Text = "Manage Macro";
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(ManageMacro_MouseDown);
		this.gb_upload.ResumeLayout(false);
		this.gb_upload.PerformLayout();
		this.gb_local.ResumeLayout(false);
		this.gb_local.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
