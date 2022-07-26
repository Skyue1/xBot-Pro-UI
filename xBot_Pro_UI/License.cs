using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace xBot_Pro_UI;

public class License : Form
{
	public struct MARGINS
	{
		public int leftWidth;

		public int rightWidth;

		public int topHeight;

		public int bottomHeight;
	}

	private bool m_aeroEnabled;

	private const int CS_DROPSHADOW = 131072;

	private const int WM_NCPAINT = 133;

	private const int WM_ACTIVATEAPP = 28;

	private const int WM_NCHITTEST = 132;

	private const int HTCLIENT = 1;

	private const int HTCAPTION = 2;

	private string serverURL = "";

	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	private IContainer components;

	private GroupBox gb_license;

	private TextBox txt_cid;

	private Button btn_join;

	private Label lbl_message;

	private StatusStrip statusStrip1;

	private Label label1;

	private Timer tmr_prevent_max;

	private Button btn_close_hover;

	private Button btn_close_leave;

	private Button btn_close;

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

	public License(string message, string clientId, string server)
	{
		InitializeComponent();
		lbl_message.Text = message;
		txt_cid.Text = clientId;
		serverURL = server;
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
		Environment.Exit(0);
	}

	private void btn_join_Click(object sender, EventArgs e)
	{
		Process.Start(serverURL);
	}

	private void tmr_prevent_max_Tick(object sender, EventArgs e)
	{
		if (base.WindowState == FormWindowState.Maximized)
		{
			base.WindowState = FormWindowState.Normal;
		}
	}

	private void btn_close_MouseEnter(object sender, EventArgs e)
	{
		btn_close.BackgroundImage = btn_close_hover.BackgroundImage;
	}

	private void btn_close_MouseLeave(object sender, EventArgs e)
	{
		btn_close.BackgroundImage = btn_close_leave.BackgroundImage;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xBot_Pro_UI.License));
		this.gb_license = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txt_cid = new System.Windows.Forms.TextBox();
		this.btn_join = new System.Windows.Forms.Button();
		this.lbl_message = new System.Windows.Forms.Label();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.tmr_prevent_max = new System.Windows.Forms.Timer(this.components);
		this.btn_close_hover = new System.Windows.Forms.Button();
		this.btn_close_leave = new System.Windows.Forms.Button();
		this.btn_close = new System.Windows.Forms.Button();
		this.gb_license.SuspendLayout();
		base.SuspendLayout();
		this.gb_license.Controls.Add(this.label1);
		this.gb_license.Controls.Add(this.txt_cid);
		this.gb_license.Controls.Add(this.btn_join);
		this.gb_license.Controls.Add(this.lbl_message);
		this.gb_license.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_license.Location = new System.Drawing.Point(12, 25);
		this.gb_license.Name = "gb_license";
		this.gb_license.Size = new System.Drawing.Size(349, 172);
		this.gb_license.TabIndex = 9;
		this.gb_license.TabStop = false;
		this.gb_license.Text = "License";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(10, 119);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(72, 13);
		this.label1.TabIndex = 4;
		this.label1.Text = "Your ClientID:";
		this.txt_cid.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_cid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_cid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25f);
		this.txt_cid.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_cid.Location = new System.Drawing.Point(13, 135);
		this.txt_cid.Name = "txt_cid";
		this.txt_cid.Size = new System.Drawing.Size(198, 21);
		this.txt_cid.TabIndex = 3;
		this.btn_join.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_join.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.btn_join.Location = new System.Drawing.Point(217, 135);
		this.btn_join.Name = "btn_join";
		this.btn_join.Size = new System.Drawing.Size(121, 21);
		this.btn_join.TabIndex = 0;
		this.btn_join.Text = "Join Discord Server";
		this.btn_join.UseVisualStyleBackColor = true;
		this.btn_join.Click += new System.EventHandler(btn_join_Click);
		this.lbl_message.Location = new System.Drawing.Point(10, 30);
		this.lbl_message.Name = "lbl_message";
		this.lbl_message.Size = new System.Drawing.Size(328, 80);
		this.lbl_message.TabIndex = 2;
		this.lbl_message.Text = "n/a";
		this.statusStrip1.BackgroundImage = (System.Drawing.Image)resources.GetObject("statusStrip1.BackgroundImage");
		this.statusStrip1.Location = new System.Drawing.Point(0, 185);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(371, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 11;
		this.statusStrip1.Text = "statusStrip1";
		this.tmr_prevent_max.Enabled = true;
		this.tmr_prevent_max.Interval = 50;
		this.tmr_prevent_max.Tick += new System.EventHandler(tmr_prevent_max_Tick);
		this.btn_close_hover.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_hover.BackgroundImage");
		this.btn_close_hover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_hover.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_hover.ForeColor = System.Drawing.Color.Red;
		this.btn_close_hover.Location = new System.Drawing.Point(169, 2);
		this.btn_close_hover.Name = "btn_close_hover";
		this.btn_close_hover.Size = new System.Drawing.Size(17, 17);
		this.btn_close_hover.TabIndex = 12;
		this.btn_close_hover.UseVisualStyleBackColor = true;
		this.btn_close_hover.Visible = false;
		this.btn_close_leave.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_leave.BackgroundImage");
		this.btn_close_leave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_leave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_leave.ForeColor = System.Drawing.Color.Red;
		this.btn_close_leave.Location = new System.Drawing.Point(146, 2);
		this.btn_close_leave.Name = "btn_close_leave";
		this.btn_close_leave.Size = new System.Drawing.Size(17, 17);
		this.btn_close_leave.TabIndex = 13;
		this.btn_close_leave.UseVisualStyleBackColor = true;
		this.btn_close_leave.Visible = false;
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close.ForeColor = System.Drawing.Color.Red;
		this.btn_close.Location = new System.Drawing.Point(352, 2);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(17, 17);
		this.btn_close.TabIndex = 14;
		this.btn_close.UseVisualStyleBackColor = true;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.btn_close.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close.MouseLeave += new System.EventHandler(btn_close_MouseLeave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		base.ClientSize = new System.Drawing.Size(371, 207);
		base.Controls.Add(this.btn_close_hover);
		base.Controls.Add(this.btn_close_leave);
		base.Controls.Add(this.btn_close);
		base.Controls.Add(this.gb_license);
		base.Controls.Add(this.statusStrip1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "License";
		this.Text = "License";
		this.gb_license.ResumeLayout(false);
		this.gb_license.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
