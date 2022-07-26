using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace xBot_Pro_UI;

public class Leaderboard : Form
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

	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	private IContainer components;

	private Button btn_close_hover;

	private Button btn_close_leave;

	private Button btn_close;

	private ListView lv_leaderboard;

	private ColumnHeader ch_name;

	private ColumnHeader ch_amount;

	private StatusStrip statusStrip1;

	private Label label1;

	private Label label2;

	private Timer tmr_prevent_max;

	private Label label4;

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

	public Leaderboard()
	{
		InitializeComponent();
		loadboard();
	}

	private void loadboard()
	{
		lv_leaderboard.Items.Clear();
		byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/leaderboard.php");
		string @string = Encoding.ASCII.GetString(bytes);
		string[] separator = new string[1] { "<br>" };
		string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		int num = 0;
		string[] array2 = array;
		foreach (string text in array2)
		{
			num++;
			string text2 = text.Split(':')[0];
			string text3 = text.Split(':')[1];
			ListViewItem listViewItem = new ListViewItem();
			listViewItem.UseItemStyleForSubItems = false;
			lv_leaderboard.View = View.Details;
			listViewItem = new ListViewItem(new string[2] { text2, text3 });
			switch (num)
			{
			case 1:
				listViewItem.ForeColor = Color.FromArgb(255, 230, 128);
				break;
			case 2:
				listViewItem.ForeColor = Color.Gainsboro;
				break;
			case 3:
				listViewItem.ForeColor = Color.FromArgb(255, 180, 128);
				break;
			default:
				listViewItem.ForeColor = Color.FromArgb(128, 128, 128);
				break;
			}
			lv_leaderboard.Items.Add(listViewItem);
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void btn_close_MouseEnter(object sender, EventArgs e)
	{
		btn_close.BackgroundImage = btn_close_hover.BackgroundImage;
	}

	private void btn_close_MouseLeave(object sender, EventArgs e)
	{
		btn_close.BackgroundImage = btn_close_leave.BackgroundImage;
	}

	private void tmr_prevent_max_Tick(object sender, EventArgs e)
	{
		if (base.WindowState == FormWindowState.Maximized)
		{
			base.WindowState = FormWindowState.Normal;
		}
	}

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	private void label4_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ReleaseCapture();
			SendMessage(base.Handle, 161, 2, 0);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xBot_Pro_UI.Leaderboard));
		System.Windows.Forms.ListViewItem listViewItem = new System.Windows.Forms.ListViewItem(new string[2] { "test", "12" }, -1);
		System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[2] { "test2", "xd" }, -1);
		this.btn_close_hover = new System.Windows.Forms.Button();
		this.btn_close_leave = new System.Windows.Forms.Button();
		this.btn_close = new System.Windows.Forms.Button();
		this.lv_leaderboard = new System.Windows.Forms.ListView();
		this.ch_name = new System.Windows.Forms.ColumnHeader();
		this.ch_amount = new System.Windows.Forms.ColumnHeader();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.tmr_prevent_max = new System.Windows.Forms.Timer(this.components);
		this.label4 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.btn_close_hover.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_hover.BackgroundImage");
		this.btn_close_hover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_hover.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_hover.ForeColor = System.Drawing.Color.Red;
		this.btn_close_hover.Location = new System.Drawing.Point(204, 3);
		this.btn_close_hover.Name = "btn_close_hover";
		this.btn_close_hover.Size = new System.Drawing.Size(17, 17);
		this.btn_close_hover.TabIndex = 15;
		this.btn_close_hover.UseVisualStyleBackColor = true;
		this.btn_close_hover.Visible = false;
		this.btn_close_leave.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_leave.BackgroundImage");
		this.btn_close_leave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_leave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_leave.ForeColor = System.Drawing.Color.Red;
		this.btn_close_leave.Location = new System.Drawing.Point(181, 3);
		this.btn_close_leave.Name = "btn_close_leave";
		this.btn_close_leave.Size = new System.Drawing.Size(17, 17);
		this.btn_close_leave.TabIndex = 16;
		this.btn_close_leave.UseVisualStyleBackColor = true;
		this.btn_close_leave.Visible = false;
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close.ForeColor = System.Drawing.Color.Red;
		this.btn_close.Location = new System.Drawing.Point(282, 3);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(17, 17);
		this.btn_close.TabIndex = 17;
		this.btn_close.UseVisualStyleBackColor = true;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.btn_close.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close.MouseLeave += new System.EventHandler(btn_close_MouseLeave);
		this.lv_leaderboard.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.lv_leaderboard.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lv_leaderboard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.ch_name, this.ch_amount });
		this.lv_leaderboard.ForeColor = System.Drawing.Color.Gainsboro;
		this.lv_leaderboard.FullRowSelect = true;
		this.lv_leaderboard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lv_leaderboard.HideSelection = false;
		this.lv_leaderboard.Items.AddRange(new System.Windows.Forms.ListViewItem[2] { listViewItem, listViewItem2 });
		this.lv_leaderboard.Location = new System.Drawing.Point(-1, 26);
		this.lv_leaderboard.Name = "lv_leaderboard";
		this.lv_leaderboard.Size = new System.Drawing.Size(325, 194);
		this.lv_leaderboard.TabIndex = 18;
		this.lv_leaderboard.UseCompatibleStateImageBehavior = false;
		this.lv_leaderboard.View = System.Windows.Forms.View.Details;
		this.ch_name.Text = "Name";
		this.ch_name.Width = 182;
		this.ch_amount.Text = "amount";
		this.ch_amount.Width = 66;
		this.statusStrip1.BackgroundImage = (System.Drawing.Image)resources.GetObject("statusStrip1.BackgroundImage");
		this.statusStrip1.Location = new System.Drawing.Point(0, 220);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(302, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 19;
		this.statusStrip1.Text = "statusStrip1";
		this.label1.Location = new System.Drawing.Point(0, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(186, 23);
		this.label1.TabIndex = 20;
		this.label1.Text = "Macro Creator";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.Location = new System.Drawing.Point(187, 26);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(115, 23);
		this.label2.TabIndex = 20;
		this.label2.Text = "Verified Macros";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tmr_prevent_max.Enabled = true;
		this.tmr_prevent_max.Interval = 50;
		this.tmr_prevent_max.Tick += new System.EventHandler(tmr_prevent_max_Tick);
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(-2, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(47, 31);
		this.label4.TabIndex = 20;
		this.label4.Text = "\ud83c\udfc6";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(label4_MouseDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		base.ClientSize = new System.Drawing.Size(302, 242);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.lv_leaderboard);
		base.Controls.Add(this.btn_close_hover);
		base.Controls.Add(this.btn_close_leave);
		base.Controls.Add(this.btn_close);
		this.ForeColor = System.Drawing.Color.Gainsboro;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "Leaderboard";
		this.Text = "Leaderboard";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
