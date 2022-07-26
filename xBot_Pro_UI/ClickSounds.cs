using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using GD_MENU.ForMenu.mlibkee;

namespace xBot_Pro_UI;

public class ClickSounds : Form
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

	private MacroClick[] clicklist;

	private Thread clickThread;

	private string macroname;

	private int distance = 10000;

	private int keyP1 = 1;

	private int keyP2 = 2;

	private IContainer components;

	private StatusStrip statusStrip1;

	private Button btn_close_hover;

	private Button btn_close_leave;

	private Button btn_close;

	private ToolStripProgressBar pgb_state;

	private ToolStripStatusLabel lbl_analyse;

	private ListView lv_clicks;

	private ColumnHeader ch_coord;

	private ColumnHeader ch_type;

	private ColumnHeader ch_clicktype;

	private Button btn_execute;

	private TrackBar tb_distance;

	private ColumnHeader ch_player;

	private ComboBox cb_keys;

	private Label label1;

	private Label label2;

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

	private void btn_close_Click(object sender, EventArgs e)
	{
		if (clickThread != null)
		{
			clickThread.Abort();
		}
		Close();
	}

	private void btn_close_MouseEnter(object sender, EventArgs e)
	{
		btn_close.BackgroundImage = btn_close_hover.BackgroundImage;
	}

	private void btn_close_MouseLeave(object sender, EventArgs e)
	{
		btn_close.BackgroundImage = btn_close_leave.BackgroundImage;
	}

	public ClickSounds()
	{
		InitializeComponent();
	}

	public void Start(string macro_name)
	{
		macroname = macro_name;
		interpretMacro(loadmacro(macro_name));
	}

	private string[] loadmacro(string macroname)
	{
		string[] array = File.ReadAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", macroname));
		int num = -1;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.Contains("pro") && !text.Contains("pro_plus"))
			{
				num = 0;
			}
			if (text.Contains("frames"))
			{
				num = -1;
			}
			if (text.Contains("pro_plus"))
			{
				num = 2;
			}
		}
		if (num == -1)
		{
			MessageBox.Show("The selected Macro is not compatible");
			return null;
		}
		List<string> list = array.ToList();
		list.RemoveAt(list.FindIndex((string i) => i.Contains("fps")));
		list.RemoveAt(list.FindIndex((string i) => i.Contains("pro")));
		if (list.FindIndex((string i) => i.ToString() == "\n") != -1)
		{
			list.RemoveAt(list.FindIndex((string i) => i.ToString() == "\n"));
		}
		array = list.ToArray();
		if (num == 0)
		{
			int num2 = array.Length;
			string[] array3 = new string[num2 * 2];
			for (int k = 0; k < num2; k++)
			{
				array3[k * 2] = "1 " + array[k].Split(' ')[0];
				array3[k * 2 + 1] = "0 " + array[k].Split(' ')[1];
			}
			array = array3;
		}
		return array;
	}

	private string getSoundfile(bool hard, bool down, int key)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "wav\\";
		int num = new Random().Next(1, 10);
		switch (key)
		{
		case 1:
			text += "mouse\\";
			if (down)
			{
				if (hard)
				{
					return text + "mouse_hard_down_0" + (num % 2 + 1) + ".wav";
				}
				return text + "mouse_soft_down_0" + (num % 2 + 1) + ".wav";
			}
			if (hard)
			{
				return text + "mouse_hard_release_0" + (num % 2 + 1) + ".wav";
			}
			return text + "mouse_soft_release_0" + (num % 2 + 1) + ".wav";
		case 3:
			text += "uparrow\\";
			if (down)
			{
				if (hard)
				{
					return text + "up_hard_press_0" + (num % 3 + 1) + ".wav";
				}
				return text + "up_soft_press_0" + (num % 4 + 1) + ".wav";
			}
			if (hard)
			{
				return text + "up_hard_release_0" + (num % 3 + 1) + ".wav";
			}
			return text + "up_soft_release_0" + (num % 4 + 1) + ".wav";
		default:
			text += "space\\";
			if (down)
			{
				return text + "space_hard_press_0" + (num % 4 + 1) + ".wav";
			}
			return text + "space_hard_press_0" + (num % 4 + 1) + ".wav";
		}
	}

	private void interpretMacro(string[] macro)
	{
		if (macro == null)
		{
			return;
		}
		pgb_state.Visible = true;
		lbl_analyse.Visible = true;
		clicklist = new MacroClick[macro.Length];
		pgb_state.Maximum = clicklist.Length;
		pgb_state.Value = 0;
		Application.DoEvents();
		for (int i = 0; i < clicklist.Length; i++)
		{
			int num = int.Parse(macro[i].Split(' ')[1]);
			bool flag = false;
			bool flag2;
			bool flag3;
			if (int.Parse(macro[i].Split(' ')[0]) == 1)
			{
				flag = true;
				if (int.Parse(macro[i + 1].Split(' ')[1]) - int.Parse(macro[i].Split(' ')[1]) >= distance)
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: true, down: true, keyP1));
					flag2 = true;
					flag3 = true;
				}
				else
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: false, down: true, keyP1));
					flag2 = true;
					flag3 = false;
				}
			}
			else if (int.Parse(macro[i].Split(' ')[0]) == 0)
			{
				flag = true;
				if (int.Parse(macro[i].Split(' ')[1]) - int.Parse(macro[i - 1].Split(' ')[1]) >= distance)
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: true, down: false, keyP1));
					flag2 = false;
					flag3 = true;
				}
				else
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: false, down: false, keyP1));
					flag2 = false;
					flag3 = false;
				}
			}
			else if (int.Parse(macro[i].Split(' ')[0]) == 3)
			{
				if (int.Parse(macro[i + 1].Split(' ')[1]) - int.Parse(macro[i].Split(' ')[1]) >= distance)
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: true, down: true, keyP2));
					flag2 = true;
					flag3 = true;
				}
				else
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: false, down: true, keyP2));
					flag2 = true;
					flag3 = false;
				}
			}
			else if (int.Parse(macro[i].Split(' ')[1]) - int.Parse(macro[i - 1].Split(' ')[1]) >= distance)
			{
				clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: true, down: false, keyP2));
				flag2 = false;
				flag3 = true;
			}
			else
			{
				clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: false, down: false, keyP2));
				flag2 = false;
				flag3 = false;
			}
			pgb_state.Value = i;
			ListViewItem listViewItem = new ListViewItem();
			listViewItem.UseItemStyleForSubItems = false;
			lv_clicks.View = View.Details;
			listViewItem = new ListViewItem(new string[4]
			{
				num.ToString(),
				flag ? "1" : "2",
				flag2 ? "Press" : "Release",
				flag3 ? "Hard Press" : "Soft Press"
			});
			lv_clicks.Items.Add(listViewItem);
			if (i % 10 == 0)
			{
				Application.DoEvents();
			}
		}
		pgb_state.Visible = false;
		lbl_analyse.Visible = false;
	}

	private void execute()
	{
		string process_Name = "GeometryDash";
		VAMemory vAMemory = new VAMemory("GeometryDash");
		bool flag = false;
		if (vAMemory.CheckProcess())
		{
			int num = keeProc.ReadAddress(process_Name, "GeometryDash.exe+003222D0 164 224 67C");
			for (int i = 0; i < clicklist.Length; i++)
			{
				MacroClick macroClick = clicklist[i];
				int num2 = 0;
				while (num2 < macroClick.getCoord)
				{
					Thread.Sleep(1);
					num2 = vAMemory.ReadInt32((IntPtr)num);
					num = keeProc.ReadAddress(process_Name, "GeometryDash.exe+003222D0 164 224 67C");
					if (num2 < 1115243428)
					{
						i = 0;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					macroClick.execute();
				}
				flag = false;
			}
		}
		btn_execute.ForeColor = Color.Gainsboro;
	}

	private void btn_execute_Click(object sender, EventArgs e)
	{
		if (btn_execute.Text != "⬛   Stop")
		{
			clickThread = new Thread((ThreadStart)delegate
			{
				execute();
			});
			clickThread.Start();
			btn_execute.ForeColor = Color.FromArgb(128, 255, 128);
			btn_execute.Text = "⬛   Stop";
		}
		else
		{
			clickThread.Abort();
			btn_execute.ForeColor = Color.Gainsboro;
			btn_execute.Text = "▶    Start";
		}
	}

	private void cb_keys_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (cb_keys.SelectedIndex)
		{
		case 1:
			keyP1 = 1;
			keyP2 = 3;
			break;
		case 2:
			keyP1 = 3;
			keyP2 = 2;
			break;
		case 3:
			keyP1 = 3;
			keyP2 = 1;
			break;
		case 4:
			keyP1 = 2;
			keyP2 = 1;
			break;
		case 5:
			keyP1 = 2;
			keyP2 = 3;
			break;
		default:
			keyP1 = 1;
			keyP2 = 2;
			break;
		}
		lv_clicks.Items.Clear();
		Start(macroname);
	}

	private void tb_distance_Scroll(object sender, MouseEventArgs e)
	{
		distance = tb_distance.Value * 100;
		lv_clicks.Items.Clear();
		Start(macroname);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xBot_Pro_UI.ClickSounds));
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.pgb_state = new System.Windows.Forms.ToolStripProgressBar();
		this.lbl_analyse = new System.Windows.Forms.ToolStripStatusLabel();
		this.btn_close_hover = new System.Windows.Forms.Button();
		this.btn_close_leave = new System.Windows.Forms.Button();
		this.btn_close = new System.Windows.Forms.Button();
		this.lv_clicks = new System.Windows.Forms.ListView();
		this.ch_coord = new System.Windows.Forms.ColumnHeader();
		this.ch_player = new System.Windows.Forms.ColumnHeader();
		this.ch_type = new System.Windows.Forms.ColumnHeader();
		this.ch_clicktype = new System.Windows.Forms.ColumnHeader();
		this.btn_execute = new System.Windows.Forms.Button();
		this.tb_distance = new System.Windows.Forms.TrackBar();
		this.cb_keys = new System.Windows.Forms.ComboBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.statusStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.tb_distance).BeginInit();
		base.SuspendLayout();
		this.statusStrip1.BackgroundImage = (System.Drawing.Image)resources.GetObject("statusStrip1.BackgroundImage");
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.pgb_state, this.lbl_analyse });
		this.statusStrip1.Location = new System.Drawing.Point(0, 247);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(344, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 12;
		this.statusStrip1.Text = "statusStrip1";
		this.pgb_state.Name = "pgb_state";
		this.pgb_state.Size = new System.Drawing.Size(100, 16);
		this.lbl_analyse.Name = "lbl_analyse";
		this.lbl_analyse.Size = new System.Drawing.Size(118, 17);
		this.lbl_analyse.Text = "Interpreting Macro ...";
		this.btn_close_hover.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_hover.BackgroundImage");
		this.btn_close_hover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_hover.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_hover.ForeColor = System.Drawing.Color.Red;
		this.btn_close_hover.Location = new System.Drawing.Point(299, 3);
		this.btn_close_hover.Name = "btn_close_hover";
		this.btn_close_hover.Size = new System.Drawing.Size(17, 17);
		this.btn_close_hover.TabIndex = 13;
		this.btn_close_hover.UseVisualStyleBackColor = true;
		this.btn_close_hover.Visible = false;
		this.btn_close_leave.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_leave.BackgroundImage");
		this.btn_close_leave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_leave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_leave.ForeColor = System.Drawing.Color.Red;
		this.btn_close_leave.Location = new System.Drawing.Point(276, 3);
		this.btn_close_leave.Name = "btn_close_leave";
		this.btn_close_leave.Size = new System.Drawing.Size(17, 17);
		this.btn_close_leave.TabIndex = 14;
		this.btn_close_leave.UseVisualStyleBackColor = true;
		this.btn_close_leave.Visible = false;
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close.ForeColor = System.Drawing.Color.Red;
		this.btn_close.Location = new System.Drawing.Point(324, 3);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(17, 17);
		this.btn_close.TabIndex = 15;
		this.btn_close.UseVisualStyleBackColor = true;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.btn_close.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close.MouseLeave += new System.EventHandler(btn_close_MouseLeave);
		this.lv_clicks.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.lv_clicks.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lv_clicks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.ch_coord, this.ch_player, this.ch_type, this.ch_clicktype });
		this.lv_clicks.ForeColor = System.Drawing.Color.Gainsboro;
		this.lv_clicks.FullRowSelect = true;
		this.lv_clicks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lv_clicks.HideSelection = false;
		this.lv_clicks.Location = new System.Drawing.Point(1, 33);
		this.lv_clicks.Name = "lv_clicks";
		this.lv_clicks.Size = new System.Drawing.Size(343, 132);
		this.lv_clicks.TabIndex = 16;
		this.lv_clicks.UseCompatibleStateImageBehavior = false;
		this.lv_clicks.View = System.Windows.Forms.View.Details;
		this.ch_coord.Text = "Coordinate";
		this.ch_coord.Width = 85;
		this.ch_player.Text = "Player";
		this.ch_type.Text = "MouseState";
		this.ch_type.Width = 102;
		this.ch_clicktype.Text = "Click";
		this.ch_clicktype.Width = 77;
		this.btn_execute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_execute.Location = new System.Drawing.Point(268, 195);
		this.btn_execute.Name = "btn_execute";
		this.btn_execute.Size = new System.Drawing.Size(75, 23);
		this.btn_execute.TabIndex = 17;
		this.btn_execute.Text = "▶    Start";
		this.btn_execute.UseVisualStyleBackColor = true;
		this.btn_execute.Click += new System.EventHandler(btn_execute_Click);
		this.tb_distance.LargeChange = 50;
		this.tb_distance.Location = new System.Drawing.Point(0, 167);
		this.tb_distance.Maximum = 1000;
		this.tb_distance.Name = "tb_distance";
		this.tb_distance.Orientation = System.Windows.Forms.Orientation.Vertical;
		this.tb_distance.Size = new System.Drawing.Size(45, 77);
		this.tb_distance.TabIndex = 18;
		this.tb_distance.TickStyle = System.Windows.Forms.TickStyle.None;
		this.tb_distance.Value = 250;
		this.tb_distance.MouseUp += new System.Windows.Forms.MouseEventHandler(tb_distance_Scroll);
		this.cb_keys.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.cb_keys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cb_keys.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cb_keys.ForeColor = System.Drawing.Color.Gainsboro;
		this.cb_keys.FormattingEnabled = true;
		this.cb_keys.Items.AddRange(new object[6] { "P1: Mouse | P2: ArrowUp", "P1: Mouse | P2: Space", "P1: Space | P2: ArrowUp", "P1: Space | P2: Mouse", "P1: ArrowUp | P2: Mouse", "P1: ArrowUp | P2: Space" });
		this.cb_keys.Location = new System.Drawing.Point(195, 172);
		this.cb_keys.Name = "cb_keys";
		this.cb_keys.Size = new System.Drawing.Size(148, 21);
		this.cb_keys.TabIndex = 20;
		this.cb_keys.Text = "P1: Mouse | P2: ArrowUp";
		this.cb_keys.SelectedIndexChanged += new System.EventHandler(cb_keys_SelectedIndexChanged);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(24, 223);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(90, 13);
		this.label1.TabIndex = 21;
		this.label1.Text = "more hardpresses";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(24, 173);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 13);
		this.label2.TabIndex = 21;
		this.label2.Text = "more softpresses";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		base.ClientSize = new System.Drawing.Size(344, 269);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.cb_keys);
		base.Controls.Add(this.btn_execute);
		base.Controls.Add(this.lv_clicks);
		base.Controls.Add(this.btn_close_hover);
		base.Controls.Add(this.btn_close_leave);
		base.Controls.Add(this.btn_close);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.tb_distance);
		this.ForeColor = System.Drawing.Color.Gainsboro;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "ClickSounds";
		this.Text = "ClickSounds";
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.tb_distance).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
