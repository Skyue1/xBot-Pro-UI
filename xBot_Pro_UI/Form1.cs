using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using GD_MENU.ForMenu.mlibkee;
using gh;
using Microsoft.Win32;

namespace xBot_Pro_UI;

public class Form1 : Form
{
	public struct MARGINS
	{
		public int leftWidth;

		public int rightWidth;

		public int topHeight;

		public int bottomHeight;
	}

	private const string FRAMES_DLL = "frames.dll";

	private const string SPEED_DLL = "C9H13N.dll";

	private const string PRO_RECORD_DLL = "xbot_3_0.dll";

	private const string FRAMESKIP_DLL = "libcocos4d.dll";

	private const int TOGGLE_RECORD = 309128;

	private const int TOGGLE_RESET_RECORD = 79536;

	private const int TOGGLE_PRACTICE = 79596;

	private const int TOGGLE_PLAY = 79544;

	private const int TOGGLE_RESET_PLAY = 79540;

	private const int LENGTH = 309120;

	private const int POSITION = 79584;

	private const int MACROLOCATION = 79696;

	private const int TOGGLE_FRAMES = 210752;

	private const int TOGGLE_SMART = 309116;

	private const int DEATHS = 79676;

	private const int CHECKPOINTS = 210772;

	private const int SPEEDHACK = 77860;

	private const int SMOOTHFIX = 77856;

	private const int FRAMERATE = 77864;

	private const int SLOWSPEED = 77872;

	private const int BOOSTSPEED = 77876;

	private const int TOGGLE_FPS_UPDATE = 66381;

	private const int TOGGLE_SMOOTHFIX = 309140;

	private const int CREDITS = 79608;

	private const string versionNum = "3.02";

	private string license = "Basic";

	private string copyrightNote = "xBot 3.02 - AndxArtZ (c) 2017-2021";

	private static string kProc = "GeometryDash";

	private Process[] GDproc;

	private VAMemory vaM;

	private string serverURL;

	private bool pro;

	private bool v2_21;

	private IntPtr gameBase;

	private IntPtr libcocos2dBase;

	private IntPtr xbotBase;

	private IntPtr libcocos4dBase;

	private IntPtr speedhackBase;

	private IntPtr framesBase;

	private EditClick currentClick;

	private int currentFps;

	private double currentCoordinate;

	private Thread getCurCoordThread;

	private Thread clickThread;

	private MacroClick[] clicklist;

	private string macroname;

	private int distance = 50;

	private int keyP1 = 1;

	private int keyP2 = 2;

	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	private bool m_aeroEnabled;

	private const int CS_DROPSHADOW = 131072;

	private const int WM_NCPAINT = 133;

	private const int WM_ACTIVATEAPP = 28;

	private const int WM_NCHITTEST = 132;

	private const int HTCLIENT = 1;

	private const int HTCAPTION = 2;

	private byte[] randombytes;

	private IContainer components;

	private TabControl tc_main;

	private TabPage tp_rec;

	private TabPage tp_play;

	private TabControl tc_play;

	private TabPage tp_play_normal;

	private Button btn_play;

	private Label label4;

	private TabPage tp_play_sequence;

	private TabPage tp_macros;

	private ListBox lb_macros;

	private TabPage tp_online;

	private ListView lv_online;

	private ColumnHeader ch_id;

	private ColumnHeader ch_name;

	private ColumnHeader ch_size;

	private ColumnHeader ch_Type;

	private ColumnHeader ch_creator;

	private ColumnHeader ch_levelid;

	private ColumnHeader ch_verified;

	private TabPage tp_settings;

	private TabPage tp_help;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel lbl_toolbar;

	private Button btn_load;

	private Button btn_search;

	private TextBox txt_search;

	private Button btn_search_local;

	private TextBox txt_search_local;

	private Label lbl_macro_size_play;

	private Label label8;

	private NumericUpDown num_fps;

	private Label label10;

	private Label lbl_path_rec;

	private Label label11;

	private Label label9;

	private Button btn_record;

	private TextBox txt_record;

	private Button btn_apply;

	private NumericUpDown num_fps_settings;

	private Label label14;

	private Label label13;

	private Label label12;

	private Label lbl_versions;

	private TextBox txt_versions;

	private Label lbl_help_answer;

	private Label lbl_h_macro;

	private Label lbl_h_dll;

	private Label lbl_h_online;

	private Label lbl_h_crash;

	private CheckBox cb_smart_rec;

	private Label label19;

	private CheckBox cb_framerate;

	private Label label26;

	private Label label20;

	private Button btn_close;

	private Button btn_play_seq;

	private ComboBox cb_play_seq;

	private Label label27;

	private ListBox lb_play_seq;

	private Label label28;

	private System.Windows.Forms.Timer tmr_prevent_max;

	private GroupBox gb_convert;

	private ComboBox cb_convert_macros;

	private Label label6;

	private Label label1;

	private Label label5;

	private Label label2;

	private TextBox textBox1;

	private Label label3;

	private Button btn_convert;

	private ComboBox comboBox2;

	private Label lbl_smartmsg;

	private PictureBox pb_logo;

	private TabPage tp_space;

	private PictureBox pb_hidetab;

	private Label lbl_size_macros;

	private Label label30;

	private Label lbl_path_macros;

	private Label label32;

	private Label lbl_smart_framesnote;

	private Label lbl_h_slowgame;

	private Label lbl_h_dual;

	private Button btn_minimize_leave;

	private Button btn_minimize_hover;

	private Button btn_minimize;

	private Button btn_leaderboard;

	private CheckBox cb_practice_rec;

	private Label lbl_practice_rec;

	private Label lbl_practice_msg;

	private GroupBox gb_local;

	private TextBox txt_rename;

	private Button btn_apply_rename;

	private Button btn_delete;

	private Label label17;

	private GroupBox gb_upload;

	private TextBox txt_macroName;

	private Label label18;

	private Label label21;

	private CheckBox cb_anonymously;

	private Button btn_start_up;

	private TextBox txt_levelid;

	private GroupBox gb_clicks_play;

	private ProgressBar pgb_state;

	private Label label24;

	private Label label22;

	private Label lbl_analyse;

	private Label label23;

	private ComboBox cb_keys;

	private Button btn_execute;

	private ListView lv_clicks;

	private ColumnHeader ch_coord;

	private ColumnHeader ch_player;

	private ColumnHeader columnHeader1;

	private ColumnHeader ch_clicktype;

	private TrackBar tb_distance;

	private Button btn_laod_sounds;

	private GroupBox gb_rec_options;

	private Label lbl_leaderboard_verified;

	private Label lbl_online_id;

	private Label lbl_online_verified;

	private Label lbl_online_lid;

	private Label lbl_online_creator;

	private Label lbl_online_type;

	private Label lbl_online_size;

	private Label lbl_online_name;

	private Label lbl_leaderboard_creator;

	private ListView lv_leaderboard;

	private ColumnHeader ch_lb_creator;

	private ColumnHeader ch_amount;

	private ComboBox cb_play_macro;

	private GroupBox gb_license;

	private Label label7;

	private TextBox txt_cid;

	private Button btn_join;

	private Label lbl_message;

	private Button btn_copy;

	private Label lbl_clicks_click;

	private Label lbl_clicks_type;

	private Label lbl_clicks_player;

	private Label lbl_clicks_coords;

	private Label label25;

	private Button btn_reload_license;

	private Button btn_switch_cid;

	private Button btn_upgrade;

	private Button btn_edit;

	private Panel pnl_edit;

	private Button btn_close_edit;

	private Button btn_edit_save;

	private Button btn_edit_sel_next;

	private GroupBox gb_edit;

	private Button btn_edit_save_click;

	private Label lbl_edit_cur_coord;

	private Label label16;

	private Label label29;

	private Label label15;

	private PictureBox pictureBox2;

	private TrackBar tb_edit_release;

	private PictureBox pictureBox1;

	private TrackBar tb_edit_press;

	private ListView lv_edit_coords;

	private ColumnHeader ch_edit_type;

	private ColumnHeader ch_edit_coord;

	private Label lbl_edit_coord_release;

	private Label lbl_edit_coord_press;

	private Label label35;

	private Label label31;

	private Label label33;

	private Button btn_edit_delete;

	private Button btn_edit_merge;

	private OpenFileDialog fd_edit_merge;

	private Label lbl_speedhack;

	private NumericUpDown num_speedhack;

	private TrackBar tb_speed;

	private Label label37;

	private Label label36;

	private NumericUpDown num_boost;

	private CheckBox cb_lagSpikes;

	private Label label39;

	private CheckBox cb_sh_eject;

	private Label label40;

	private Label lbl_boostspeed;

	private CheckBox cb_auto_reset;

	private Label label34;

	private CheckBox cb_death_effect;

	private CheckBox cb_prac_music;

	private Label lbl_death_effect;

	private Label lbl_prac_music;

	private Label lbl_resp;

	private NumericUpDown num_resp_time;

	private Button btn_close_license;

	private Button btn_close_leave;

	private Button btn_close_hover;

	private Button btn_toggle_smoothfix;

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

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

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
			mARGINS.leftWidth = 1;
			mARGINS.rightWidth = 1;
			mARGINS.topHeight = 1;
			MARGINS pMarInset = mARGINS;
			DwmExtendFrameIntoClientArea(base.Handle, ref pMarInset);
		}
		base.WndProc(ref m);
		if (m.Msg == 132 && (int)m.Result == 1)
		{
			m.Result = (IntPtr)2;
		}
	}

	public Form1()
	{
		InitializeComponent();
		tc_main.SelectedIndex = 1;
		tc_main.Visible = false;
		tc_main.Enabled = false;
		base.Size = new Size(547, 345);
		tc_main.Size = new Size(556, 305);
		tc_play.Size = new Size(563, 284);
		attachGD();
		StartingSequence();
		StartingSequence();
		loadConfig();
		activateProForEveryone();
		writeStatus(copyrightNote);
		new Thread((ThreadStart)delegate
		{
			focusthing();
		}).Start();
	}

	private void activateProForEveryone()
	{
		gb_license.Visible = false;
		tc_main.Visible = true;
		tc_main.Enabled = true;
		enableProPlusFeatures();
	}

	private void disableProPlusFeatures()
	{
		cb_practice_rec.Visible = false;
		gb_clicks_play.Visible = false;
		lbl_practice_rec.Visible = false;
		btn_edit.Visible = false;
		cb_prac_music.Visible = false;
		lbl_death_effect.Visible = false;
		cb_death_effect.Visible = false;
		lbl_prac_music.Visible = false;
		lbl_resp.Visible = false;
		num_resp_time.Visible = false;
		btn_upgrade.Visible = true;
		copyrightNote = "xBot 3.02 - " + license + " - AndxArtZ (c) 2017-2021";
	}

	private void enableProPlusFeatures()
	{
		cb_practice_rec.Visible = true;
		gb_clicks_play.Visible = true;
		lbl_practice_rec.Visible = true;
		btn_edit.Visible = true;
		cb_prac_music.Visible = true;
		lbl_death_effect.Visible = true;
		cb_death_effect.Visible = true;
		lbl_prac_music.Visible = true;
		lbl_resp.Visible = true;
		num_resp_time.Visible = true;
		if (!v2_21)
		{
			btn_upgrade.Visible = false;
		}
		else
		{
			btn_upgrade.Visible = true;
			btn_upgrade.Text = "⭮ Update License";
			btn_upgrade.Font = new Font(new FontFamily("Consolas"), 8.5f);
		}
		copyrightNote = "xBot 3.02 - " + license + " - AndxArtZ (c) 2017-2021";
	}

	private string getClientID()
	{
		string text = "";
		try
		{
			ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
			managementObject.Get();
			text = managementObject["VolumeSerialNumber"].ToString();
		}
		catch
		{
			try
			{
				ManagementObject managementObject2 = new ManagementObject("win32_logicaldisk.deviceid=\"D:\"");
				managementObject2.Get();
				text = managementObject2["VolumeSerialNumber"].ToString();
			}
			catch
			{
				File.WriteAllText("disk.mising", "need C or D");
				Environment.Exit(0);
			}
		}
		string text2 = CpuID.ProcessorId();
		string text3 = (string)RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, RegistryExtensions.RegistryHiveType.X64).OpenSubKey("SOFTWARE\\Microsoft\\Cryptography").GetValue("MachineGuid");
		Convert.ToBase64String(Encoding.UTF8.GetBytes(text3.Split('-')[0] + "-" + text2.ToLower() + "-" + text.ToLower()));
		return text3.Split('-')[0] + "-" + text2.ToLower() + "-" + text.ToLower();
	}

	private void updateSQLEntry()
	{
		if (!vaM.CheckProcess())
		{
			int num;
			do
			{
				num = (int)MessageBox.Show("The game isn't running!", "Warning!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
				if (num == 3)
				{
					Environment.Exit(0);
				}
			}
			while ((num != 4 || !vaM.CheckProcess()) && num != 5);
		}
		string clientID = getClientID();
		string userName = Environment.UserName;
		string text = "";
		if (kProc.Length > 0)
		{
			try
			{
				int num2 = keeProc.ReadAddress(kProc, "GeometryDash.exe+003222D0 198");
				int value = 16;
				for (int i = 0; i < 16; i++)
				{
					if (vaM.ReadByte((IntPtr)num2 + i) == 0)
					{
						value = i;
					}
				}
				text = vaM.ReadStringASCII((IntPtr)num2, Convert.ToUInt16(value));
				text = text.Split(default(char))[0];
			}
			catch
			{
			}
		}
		bool flag = true;
		try
		{
			WebClient webClient = new WebClient();
			byte[] bytes = webClient.DownloadData("http://xbot.4uhr20.eu/login.php?clientid=" + clientID);
			string @string = Encoding.ASCII.GetString(bytes);
			switch (@string)
			{
			case "-1":
			{
				string text3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(getMac()));
				byte[] bytes3 = webClient.DownloadData("http://xbot.4uhr20.eu/login.php?clientid=" + text3.TrimEnd('='));
				string string3 = Encoding.ASCII.GetString(bytes3);
				if (string3 != "-1" && string3 != "-2" && string3 != "-3")
				{
					bool flag3 = checkLicnese2_21(string3.Split(':')[0]);
					if (string3.Split(':')[1] == "0")
					{
						flag = false;
					}
					if (string3.Split(':')[2] == "0")
					{
						bytes3 = webClient.DownloadData("http://xbot.4uhr20.eu/update.php?cID=" + text3 + "&gName=" + text);
						string3 = Encoding.ASCII.GetString(bytes3);
					}
					if (flag3)
					{
						pro = true;
						v2_21 = true;
						license = "Pro";
						copyrightNote = "xBot 3.02 - " + license + " - AndxArtZ (c) 2017-2021";
						bytes = webClient.DownloadData("http://xbot.4uhr20.eu/register.php?cID=" + clientID + "&uName=" + userName + "&gName=" + text);
						@string = Encoding.ASCII.GetString(bytes);
						flag = false;
					}
					else
					{
						bytes = webClient.DownloadData("http://xbot.4uhr20.eu/register.php?cID=" + clientID + "&uName=" + userName + "&gName=" + text);
						@string = Encoding.ASCII.GetString(bytes);
						flag = false;
					}
				}
				else
				{
					bytes = webClient.DownloadData("http://xbot.4uhr20.eu/register.php?cID=" + clientID + "&uName=" + userName + "&gName=" + text);
					@string = Encoding.ASCII.GetString(bytes);
					flag = false;
				}
				break;
			}
			case "-2":
			{
				string text2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(getMac()));
				byte[] bytes2 = webClient.DownloadData("http://xbot.4uhr20.eu/login.php?clientid=" + text2.TrimEnd('='));
				string string2 = Encoding.ASCII.GetString(bytes2);
				if (string2 != "-1" && string2 != "-2" && string2 != "-3")
				{
					bool flag2 = checkLicnese2_21(string2.Split(':')[0]);
					if (string2.Split(':')[1] == "0")
					{
						flag = false;
					}
					if (string2.Split(':')[2] == "0")
					{
						bytes2 = webClient.DownloadData("http://xbot.4uhr20.eu/update.php?cID=" + text2 + "&gName=" + text);
						string2 = Encoding.ASCII.GetString(bytes2);
					}
					if (flag2)
					{
						pro = true;
						v2_21 = true;
						license = "Pro";
						copyrightNote = "xBot 3.02 - " + license + " - AndxArtZ (c) 2017-2021";
					}
				}
				flag = false;
				break;
			}
			case "-3":
				flag = false;
				break;
			default:
				pro = checkLicnese(@string.Split(':')[0]);
				if (@string.Split(':')[1] == "0")
				{
					flag = false;
				}
				else if (@string.Split(':')[1] == "1")
				{
					flag = true;
				}
				if (@string.Split(':')[2] == "0")
				{
					bytes = webClient.DownloadData("http://xbot.4uhr20.eu/update.php?cID=" + clientID + "&gName=" + text);
					@string = Encoding.ASCII.GetString(bytes);
				}
				if (pro)
				{
					enableProPlusFeatures();
					v2_21 = false;
					license = "Pro";
					copyrightNote = "xBot 3.02 - " + license + " - AndxArtZ (c) 2017-2021";
				}
				break;
			}
			bool flag4 = (pro ? true : false);
			if (!pro)
			{
				if (!v2_21)
				{
					btn_switch_cid.Visible = false;
				}
				else
				{
					btn_switch_cid.Visible = true;
				}
				bytes = webClient.DownloadData("http://xbot.4uhr20.eu/message.php");
				@string = Encoding.ASCII.GetString(bytes);
				string[] separator = new string[1] { "<br>" };
				string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				string text4 = array[0];
				string text5 = array[1];
				if (text4.Length > 1)
				{
					lbl_message.Text = text4;
					txt_cid.Text = clientID;
					serverURL = text5;
				}
			}
			else
			{
				tc_main.Visible = true;
				tc_main.Enabled = true;
				enableProPlusFeatures();
			}
			if (!flag4 && flag)
			{
				MessageBox.Show("You are banned from the xBot service!");
				Environment.Exit(0);
			}
		}
		catch
		{
			lbl_message.Text = "A network error has occurred!";
			txt_cid.Text = clientID;
		}
	}

	private string encryptDecrypt(string input)
	{
		char[] array = input.ToCharArray();
		char[] array2 = new char[28]
		{
			'4', 'n', 'D', 'x', 'A', 'r', 'T', 'z', 'X', 'b',
			'0', 'x', 'T', 'k', 'E', 'y', '%', '$', '3', 'n',
			'3', 'r', 'Y', 'p', 't', '1', 'O', 'n'
		};
		int num = 0;
		for (int i = 0; i < input.Length; i++)
		{
			num++;
			if (num > array2.Length - 1)
			{
				num = 0;
			}
			array[i] = (char)(input.ToCharArray()[i] ^ (array2[num] - 16));
			Console.WriteLine((char)(array2[num] - 16) + ": " + (array2[num] - 16));
		}
		return new string(array);
	}

	private string encryptDecrypt2_21(string input)
	{
		char[] array = input.ToCharArray();
		char[] array2 = new char[18]
		{
			'4', 'n', 'D', 'x', 'A', 'r', 'T', 'z', 'X', 'b',
			'0', 'x', 'T', 'k', 'E', 'y', '%', '$'
		};
		int num = 0;
		for (int i = 0; i < input.Length; i++)
		{
			num++;
			if (num > 17)
			{
				num = 0;
			}
			array[i] = (char)(input.ToCharArray()[i] ^ array2[num]);
		}
		return new string(array);
	}

	public bool checkLicnese2_21(string key)
	{
		string @string;
		try
		{
			@string = Encoding.UTF8.GetString(Convert.FromBase64String(key));
		}
		catch
		{
			return false;
		}
		if (encryptDecrypt2_21(@string) == getMac())
		{
			return true;
		}
		return false;
	}

	public bool checkLicnese(string key)
	{
		_ = (string)RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, RegistryExtensions.RegistryHiveType.X64).OpenSubKey("SOFTWARE\\Microsoft\\Cryptography").GetValue("MachineGuid");
		string @string;
		try
		{
			@string = Encoding.UTF8.GetString(Convert.FromBase64String(key));
			Console.WriteLine(@string);
		}
		catch
		{
			return false;
		}
		if (encryptDecrypt(@string) == getClientID())
		{
			return true;
		}
		return false;
	}

	private void attachGD()
	{
		GDproc = null;
		GDproc = Process.GetProcessesByName(kProc);
		vaM = new VAMemory("GeometryDash");
	}

	private void loadConfig()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		try
		{
			string[] array = File.ReadAllLines(baseDirectory + "\\config.xbot");
			int num = int.Parse(array[0]);
			bool @checked = bool.Parse(array[3]);
			num_fps.Value = num;
			num_fps_settings.Value = num;
			cb_framerate.Checked = @checked;
			if (cb_framerate.Checked)
			{
				cb_smart_rec.Checked = false;
				cb_smart_rec.Enabled = false;
				cb_practice_rec.Checked = false;
				cb_practice_rec.Enabled = false;
				lbl_smart_framesnote.Visible = true;
			}
			else
			{
				cb_smart_rec.Enabled = true;
				cb_practice_rec.Enabled = true;
				lbl_smart_framesnote.Visible = false;
			}
		}
		catch
		{
			try
			{
				File.Delete(baseDirectory + "\\config.xbot");
			}
			catch
			{
			}
			File.WriteAllText(baseDirectory + "\\config.xbot", "60\ntrue\nfalse\nfalse");
			loadConfig();
		}
	}

	private void writeConfig()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		try
		{
			string[] array = File.ReadAllLines(baseDirectory + "\\config.xbot");
			array[0] = num_fps_settings.Value.ToString();
			array[3] = cb_framerate.Checked.ToString().ToLower();
			File.Delete(baseDirectory + "\\config.xbot");
			File.WriteAllLines(baseDirectory + "\\config.xbot", array);
		}
		catch
		{
			File.Delete(baseDirectory + "\\config.xbot");
			File.WriteAllText(baseDirectory + "\\config.xbot", "60\ntrue\nfalse\nfalse\nfalse");
			loadConfig();
		}
	}

	private void StartingSequence()
	{
		attachGD();
		if (GDproc.Length == 0)
		{
			return;
		}
		int[] array = new int[5];
		using (Process process = GDproc[0])
		{
			ProcessModuleCollection modules = process.Modules;
			ProcessModule processModule;
			for (int i = 0; i < modules.Count; i++)
			{
				processModule = modules[i];
				if (processModule.ModuleName == "frames.dll")
				{
					array[0] = i;
				}
				if (processModule.ModuleName == "xbot_3_0.dll")
				{
					array[1] = i;
				}
				if (processModule.ModuleName == "libcocos4d.dll")
				{
					array[2] = i;
				}
				if (processModule.ModuleName == "C9H13N.dll")
				{
					array[4] = i;
				}
				if (processModule.ModuleName == "libcocos2d.dll")
				{
					array[3] = i;
				}
			}
			processModule = modules[array[3]];
			libcocos2dBase = processModule.BaseAddress;
			processModule = modules[array[4]];
			speedhackBase = processModule.BaseAddress;
			processModule = modules[array[1]];
			xbotBase = processModule.BaseAddress;
			processModule = modules[array[2]];
			libcocos4dBase = processModule.BaseAddress;
			processModule = modules[array[0]];
			framesBase = processModule.BaseAddress;
			gameBase = process.MainModule.BaseAddress;
		}
		attachGD();
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		_ = array[0];
		if (array[1] == 0)
		{
			GDproc[0].Suspend();
			Thread.Sleep(100);
			if (ghapi.InjectDLL(baseDirectory + "xbot_3_0.dll", "GeometryDash.exe"))
			{
				Console.WriteLine("Success: xbot_3_0.dll");
			}
			Thread.Sleep(100);
			Process.GetProcessesByName(kProc)[0].Resume();
		}
		_ = array[2];
		_ = array[4];
		try
		{
			vaM.WriteByteArray(gameBase + 2095010, new byte[2] { 144, 144 });
			vaM.WriteByteArray(gameBase + 2151347, new byte[5] { 144, 144, 144, 144, 144 });
			vaM.WriteByteArray(gameBase + 2108074, new byte[1] { 235 });
		}
		catch
		{
			Console.WriteLine("couldn't apply acb");
		}
	}

	private void writeStatus(string msg)
	{
		lbl_toolbar.Text = msg;
		Application.DoEvents();
	}

	private void btn_play_Click(object sender, EventArgs e)
	{
		pb_logo.Focus();
		if ((cb_play_macro.Text == "" && ((Button)sender).Name == "btn_play") || (lb_play_seq.Items.Count == 0 && ((Button)sender).Name == "btn_play_seq"))
		{
			return;
		}
		writeStatus("Initializing game... [1/2]");
		Application.DoEvents();
		StartingSequence();
		writeStatus("Initializing game... [2/2]");
		StartingSequence();
		if (!vaM.CheckProcess())
		{
			return;
		}
		vaM.WriteByte(xbotBase + 309128, 0);
		vaM.WriteByte(xbotBase + 309120, 0);
		writeStatus("Write macro to the game...");
		if (((Button)sender).Name == "btn_play")
		{
			if (!writeMacro(cb_play_macro.Text))
			{
				writeStatus(copyrightNote);
				return;
			}
		}
		else if (((Button)sender).Name == "btn_play_seq")
		{
			writeMacro((string)lb_play_seq.Items[0]);
			lb_play_seq.SelectedIndex = 0;
		}
		int num = 0;
		writeStatus("Start playing...");
		SendKey();
		vaM.WriteByte(xbotBase + 79544, 1);
		vaM.WriteByte(xbotBase + 79540, 1);
		vaM.WriteInt32(xbotBase + 79676, 0);
		if (((Button)sender).Name == "btn_play")
		{
			writeStatus("Playing! press [F10] to quit!");
			btn_play.ForeColor = Color.FromArgb(128, 255, 128);
			lbl_toolbar.ForeColor = Color.FromArgb(128, 255, 128);
			btn_record.Enabled = false;
		}
		else
		{
			writeStatus("Playing! press [F10] to quit, [F7] for the next Macro and [F6] for the previous one!");
			btn_play_seq.ForeColor = Color.FromArgb(128, 255, 128);
			lbl_toolbar.ForeColor = Color.FromArgb(128, 255, 128);
			btn_record.Enabled = false;
		}
		bool flag = true;
		while (flag)
		{
			Thread.Sleep(40);
			Application.DoEvents();
			if ((int)(Keyboard.GetKeyStates(Key.F10) & KeyStates.Down) > 0)
			{
				flag = false;
				vaM.WriteByte(xbotBase + 79544, 0);
				vaM.WriteByte(xbotBase + 79540, 0);
				vaM.WriteByte(xbotBase + 210752, 0);
				btn_play.ForeColor = Color.Gainsboro;
				btn_play_seq.ForeColor = Color.Gainsboro;
				lbl_toolbar.ForeColor = Color.Gainsboro;
				btn_record.Enabled = true;
				writeStatus(copyrightNote);
			}
			if (!(((Button)sender).Name == "btn_play_seq"))
			{
				continue;
			}
			int num2 = keeProc.ReadAddress(kProc, "GeometryDash.exe+003222D0 164 224 63F");
			if (vaM.ReadByte((IntPtr)num2) == 1)
			{
				num = mod(num + 1, lb_play_seq.Items.Count);
				writeMacro((string)lb_play_seq.Items[num]);
				lb_play_seq.SelectedIndex = num;
				while (vaM.ReadByte((IntPtr)num2) == 1)
				{
					Thread.Sleep(50);
					Application.DoEvents();
				}
			}
			if ((int)(Keyboard.GetKeyStates(Key.F6) & KeyStates.Down) > 0)
			{
				num = mod(num - 1, lb_play_seq.Items.Count);
				writeMacro((string)lb_play_seq.Items[num]);
				lb_play_seq.SelectedIndex = num;
				Thread.Sleep(250);
			}
			if ((int)(Keyboard.GetKeyStates(Key.F7) & KeyStates.Down) > 0)
			{
				num = mod(num + 1, lb_play_seq.Items.Count);
				writeMacro((string)lb_play_seq.Items[num]);
				lb_play_seq.SelectedIndex = num;
				Thread.Sleep(250);
			}
		}
		tc_main.Enabled = true;
	}

	private int mod(int x, int m)
	{
		int num = x % m;
		if (num >= 0)
		{
			return num;
		}
		return num + m;
	}

	private bool writeMacro(string macroName)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		string[] array = new string[0];
		try
		{
			array = File.ReadAllLines(text + macroName);
		}
		catch
		{
			MessageBox.Show("Error! Did you type the macro name correctly?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		int num = 0;
		int num2 = -1;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text2.Contains("pro") && !text2.Contains("pro_plus"))
			{
				num2 = 0;
			}
			if (text2.Contains("frames"))
			{
				num2 = 1;
			}
			if (text2.Contains("pro_plus"))
			{
				num2 = 2;
			}
		}
		if (num2 == -1)
		{
			MessageBox.Show("The selected Macro is not compatible");
			return false;
		}
		array2 = array;
		foreach (string text3 in array2)
		{
			if (!(text3 != "\n") || !(text3 != "\r\n") || !(text3 != "\r") || !(text3 != ""))
			{
				continue;
			}
			if (text3.Contains("fps"))
			{
				setfps(int.Parse(text3.Split(' ')[1]));
				continue;
			}
			switch (num2)
			{
			case 0:
				if (!text3.Contains("pro"))
				{
					vaM.WriteInt32(xbotBase + 79696 + num, int.Parse(text3.Split(' ')[0]));
					vaM.WriteByte(xbotBase + 79696 + num + 4, 1);
					vaM.WriteInt32(xbotBase + 79696 + num + 8, int.Parse(text3.Split(' ')[1]));
					vaM.WriteByte(xbotBase + 79696 + num + 12, 0);
					num += 16;
				}
				if (text3.Contains("pro"))
				{
					vaM.WriteByte(xbotBase + 210752, 0);
				}
				break;
			case 1:
				if (!text3.Contains("frames"))
				{
					vaM.WriteInt32(xbotBase + 79696 + num, int.Parse(text3.Split(' ')[1]));
					vaM.WriteInt32(xbotBase + 79696 + num + 4, int.Parse(text3.Split(' ')[0]));
					num += 8;
				}
				if (text3.Contains("frames"))
				{
					vaM.WriteByte(xbotBase + 210752, 1);
				}
				break;
			case 2:
				if (!text3.Contains("pro_plus"))
				{
					vaM.WriteInt32(xbotBase + 79696 + num, int.Parse(text3.Split(' ')[1]));
					vaM.WriteInt32(xbotBase + 79696 + num + 4, int.Parse(text3.Split(' ')[0]));
					num += 8;
				}
				if (text3.Contains("pro_plus"))
				{
					vaM.WriteByte(xbotBase + 210752, 0);
				}
				break;
			}
		}
		if (num2 == 0)
		{
			vaM.WriteInt32(xbotBase + 309120, (array.Length - 2) * 16);
		}
		else
		{
			vaM.WriteInt32(xbotBase + 309120, (array.Length - 2) * 8);
		}
		return true;
	}

	[DllImport("User32.dll")]
	private static extern int SetForegroundWindow(IntPtr point);

	private void SendKey()
	{
		Process process = Process.GetProcessesByName(kProc).FirstOrDefault();
		if (process != null)
		{
			SetForegroundWindow(process.MainWindowHandle);
			SendKeys.Send("{UP}");
		}
	}

	private void setfps(int value)
	{
		vaM.WriteFloat(xbotBase + 77856, 1f / (float)value);
		vaM.WriteDouble(xbotBase + 77864, 1f / (float)value);
	}

	private void btn_convert_Click(object sender, EventArgs e)
	{
	}

	private void tc_main_Selected(object sender, TabControlEventArgs e)
	{
		pb_hidetab.Visible = true;
		base.Size = new Size(547, 345);
		tc_main.Size = new Size(556, 305);
		tc_play.Size = new Size(563, 284);
		switch (tc_main.SelectedIndex)
		{
		case 1:
			pb_hidetab.Visible = false;
			if (cb_framerate.Checked)
			{
				cb_smart_rec.Checked = false;
				cb_smart_rec.Enabled = false;
				cb_practice_rec.Checked = false;
				cb_practice_rec.Enabled = false;
				lbl_smart_framesnote.Visible = true;
			}
			else if (cb_smart_rec.Checked)
			{
				cb_practice_rec.Checked = false;
				cb_practice_rec.Enabled = false;
			}
			else if (cb_practice_rec.Checked)
			{
				cb_smart_rec.Checked = false;
				cb_smart_rec.Enabled = false;
			}
			else
			{
				cb_smart_rec.Enabled = true;
				cb_practice_rec.Enabled = true;
				lbl_smart_framesnote.Visible = false;
			}
			writeStatus(copyrightNote);
			break;
		case 2:
		{
			cb_play_macro.Items.Clear();
			ComboBox.ObjectCollection items = cb_play_macro.Items;
			object[] macroNames = getMacroNames("");
			items.AddRange(macroNames);
			cb_play_macro.Sorted = true;
			cb_keys.SelectedIndex = 0;
			break;
		}
		case 7:
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
			int skipDirectory = text.Length;
			if (!text.EndsWith(Path.DirectorySeparatorChar.ToString() ?? ""))
			{
				skipDirectory++;
			}
			string[] array = (from f in Directory.EnumerateFiles(text, "*", SearchOption.AllDirectories)
				select f.Substring(skipDirectory)).ToArray();
			cb_convert_macros.Items.Clear();
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				if (text2 != "reg.xbot" && text2 != "config.xbot")
				{
					cb_convert_macros.Items.Add(text2);
				}
			}
			cb_convert_macros.Sorted = true;
			writeStatus(copyrightNote);
			break;
		}
		case 3:
			getLocalMacros("");
			writeStatus("Manage your Macros!");
			break;
		case 4:
			tc_main.SelectedIndex = 1;
			writeStatus("Double click on a macro to download it! A golden macro appears to be approved by a moderator!");
			break;
		case 5:
			writeStatus("Personalize your xBot settings!");
			break;
		case 6:
			writeStatus("You need help?");
			break;
		}
	}

	private string[] getMacroNames(string pattern)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		int skipDirectory = text.Length;
		if (!text.EndsWith(Path.DirectorySeparatorChar.ToString() ?? ""))
		{
			skipDirectory++;
		}
		string[] array = (from f in Directory.EnumerateFiles(text, "*", SearchOption.AllDirectories)
			select f.Substring(skipDirectory)).ToArray();
		int num = array.Length;
		int num2 = 0;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text2 == "reg.xbot" || text2 == "config.xbot" || !text2.ToUpper().Contains(pattern.ToUpper()))
			{
				num--;
			}
		}
		string[] array3 = new string[num];
		array2 = array;
		foreach (string text3 in array2)
		{
			if (text3 != "reg.xbot" && text3 != "config.xbot" && text3.ToUpper().Contains(pattern.ToUpper()))
			{
				array3[num2] = text3;
				num2++;
			}
		}
		writeStatus(copyrightNote);
		return array3;
	}

	private void loadVersions()
	{
		txt_versions.Text = "";
		try
		{
			byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/version.php");
			string @string = Encoding.ASCII.GetString(bytes);
			string[] separator = new string[1] { "<br>" };
			string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] separator2 = new string[2] { "version: ", " - comment: " };
				string[] array2 = text.Split(separator2, 7, StringSplitOptions.RemoveEmptyEntries);
				TextBox textBox = txt_versions;
				textBox.Text = textBox.Text + array2[0] + " : " + array2[1] + Environment.NewLine;
			}
			txt_versions.Text = txt_versions.Text.TrimEnd('\r', '\n');
		}
		catch
		{
			txt_versions.Text = "Network Error!";
		}
	}

	private void checkVersion()
	{
		try
		{
			byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/checkforupdate.php");
			string @string = Encoding.ASCII.GetString(bytes);
			if (float.Parse(@string.Split('|')[0]) > float.Parse("3.02"))
			{
				MessageBox.Show("There is an update availible!");
				Process.Start(@string.Split('|')[1]);
			}
		}
		catch
		{
			MessageBox.Show("Error while checking for update!");
		}
	}

	private void btn_load_Click(object sender, EventArgs e)
	{
		lv_online.Items.Clear();
		lbl_online_verified.Size = new Size(77, 23);
		lv_online.Visible = true;
		lbl_online_id.Visible = true;
		lbl_online_name.Visible = true;
		lbl_online_size.Visible = true;
		lbl_online_creator.Visible = true;
		lbl_online_type.Visible = true;
		lbl_online_lid.Visible = true;
		lbl_online_verified.Visible = true;
		try
		{
			byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/macroBrowse.php");
			string @string = Encoding.ASCII.GetString(bytes);
			string[] separator = new string[1] { "<br>" };
			string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] separator2 = new string[7] { "id: ", " name: ", " length: ", " type: ", " creator: ", " levelid: ", " verified: " };
				string[] array2 = text.Split(separator2, 7, StringSplitOptions.RemoveEmptyEntries);
				string text2 = "";
				string text3 = "";
				string text4 = "False";
				if (!text.Contains("creator:  levelid:"))
				{
					text2 = array2[4];
					if (!text.Contains("levelid:  verified:"))
					{
						text3 = array2[5];
					}
				}
				else if (!text.Contains("levelid: verified:"))
				{
					text3 = array2[4];
				}
				if (array2[array2.Length - 1] == "1")
				{
					text4 = "True";
				}
				ListViewItem listViewItem = new ListViewItem();
				listViewItem.UseItemStyleForSubItems = false;
				lv_online.View = View.Details;
				string[] obj = new string[7]
				{
					array2[0],
					array2[1],
					int.Parse(array2[2]) / 1024 + "KB",
					array2[3],
					text2,
					text3,
					text4
				};
				listViewItem = new ListViewItem(obj);
				if (obj[6] == "True")
				{
					listViewItem.ForeColor = Color.FromArgb(255, 230, 128);
				}
				lv_online.Items.Add(listViewItem);
			}
		}
		catch
		{
			MessageBox.Show("A network error has occured!");
		}
	}

	private void btn_search_Click(object sender, EventArgs e)
	{
		lv_online.Items.Clear();
		lbl_online_verified.Size = new Size(77, 23);
		lv_online.Visible = true;
		lbl_online_id.Visible = true;
		lbl_online_name.Visible = true;
		lbl_online_size.Visible = true;
		lbl_online_creator.Visible = true;
		lbl_online_type.Visible = true;
		lbl_online_lid.Visible = true;
		lbl_online_verified.Visible = true;
		try
		{
			byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/macroBrowse.php?search=" + txt_search.Text);
			string @string = Encoding.ASCII.GetString(bytes);
			string[] separator = new string[1] { "<br>" };
			string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (array[0] == "-1")
			{
				return;
			}
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] separator2 = new string[7] { "id: ", " name: ", " length: ", " type: ", " creator: ", " levelid: ", " verified: " };
				string[] array3 = text.Split(separator2, 7, StringSplitOptions.RemoveEmptyEntries);
				string text2 = "";
				string text3 = "";
				string text4 = "False";
				if (!text.Contains("creator:  levelid:"))
				{
					text2 = array3[4];
					if (!text.Contains("levelid:  verified:"))
					{
						text3 = array3[5];
					}
				}
				else if (!text.Contains("levelid: verified:"))
				{
					text3 = array3[4];
				}
				if (array3[array3.Length - 1] == "1")
				{
					text4 = "True";
				}
				ListViewItem listViewItem = new ListViewItem();
				listViewItem.UseItemStyleForSubItems = false;
				lv_online.View = View.Details;
				string[] obj = new string[7]
				{
					array3[0],
					array3[1],
					int.Parse(array3[2]) / 1024 + "KB",
					array3[3],
					text2,
					text3,
					text4
				};
				listViewItem = new ListViewItem(obj);
				if (obj[6] == "True")
				{
					listViewItem.ForeColor = Color.FromArgb(255, 230, 128);
				}
				lv_online.Items.Add(listViewItem);
			}
		}
		catch
		{
			MessageBox.Show("A network error has occured!");
		}
	}

	private void lv_online_DoubleClick(object sender, EventArgs e)
	{
		string s = lv_online.SelectedItems[0].SubItems[0].Text;
		download(int.Parse(s));
	}

	private void download(int id)
	{
		try
		{
			byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/macroDown.php?macroid=" + id);
			string @string = Encoding.ASCII.GetString(bytes);
			string[] separator = new string[1] { "<br>" };
			string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string string2 = Encoding.UTF8.GetString(Convert.FromBase64String(array[1]));
			File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", array[0]), string2);
			MessageBox.Show("The Macro '" + array[0] + "' was\ndownloaded successfully");
		}
		catch
		{
			MessageBox.Show("A network error has occured!");
		}
	}

	private void lb_macros_DoubleClick(object sender, EventArgs e)
	{
		new ManageMacro(lb_macros.SelectedItem.ToString()).Show();
	}

	private void btn_search_local_Click(object sender, EventArgs e)
	{
		getLocalMacros(txt_search_local.Text);
	}

	private void getLocalMacros(string Pattern)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		int skipDirectory = text.Length;
		if (!text.EndsWith(Path.DirectorySeparatorChar.ToString() ?? ""))
		{
			skipDirectory++;
		}
		string[] array = (from f in Directory.EnumerateFiles(text, "*", SearchOption.AllDirectories)
			select f.Substring(skipDirectory)).ToArray();
		lb_macros.Items.Clear();
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (!(text2 != "reg.xbot") || !(text2 != "config.xbot"))
			{
				continue;
			}
			if (Pattern != "")
			{
				if (text2.ToUpper().Contains(Pattern.ToUpper()))
				{
					lb_macros.Items.Add(text2);
				}
			}
			else
			{
				lb_macros.Items.Add(text2);
			}
		}
		lb_macros.Sorted = true;
	}

	private void cb_play_macro_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			float num = (float)File.ReadAllText(text + cb_play_macro.Text).Length / 1024f;
			lbl_macro_size_play.Text = num.ToString("N2") + " KB";
		}
		catch
		{
			lbl_macro_size_play.Text = "n/a";
		}
	}

	private void txt_record_TextChanged(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		if (txt_record.Text != "")
		{
			lbl_path_rec.Text = text + txt_record.Text;
		}
		else
		{
			lbl_path_rec.Text = "n/a";
		}
	}

	private void btn_record_Click(object sender, EventArgs e)
	{
		if (btn_record.ForeColor != Color.Gainsboro)
		{
			return;
		}
		if (txt_record.Text == "")
		{
			MessageBox.Show("You need to type in a name!", "Missing name!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		writeStatus("Initializing game... [1/2]");
		Application.DoEvents();
		StartingSequence();
		writeStatus("Initializing game... [2/2]");
		StartingSequence();
		if (!vaM.CheckProcess())
		{
			return;
		}
		writeStatus("Write FPS...");
		setfps((int)num_fps.Value);
		vaM.WriteByte(xbotBase + 79544, 0);
		vaM.WriteByte(xbotBase + 79540, 0);
		vaM.WriteByte(xbotBase + 309128, 1);
		vaM.WriteByte(xbotBase + 309120, 0);
		vaM.WriteByte(xbotBase + 210772, 0);
		btn_record.ForeColor = Color.FromArgb(255, 128, 128);
		lbl_toolbar.ForeColor = Color.FromArgb(255, 128, 128);
		btn_play.Enabled = false;
		btn_play_seq.Enabled = false;
		cb_smart_rec.Enabled = false;
		cb_practice_rec.Enabled = false;
		if (cb_framerate.Checked)
		{
			vaM.WriteByte(xbotBase + 210752, 1);
			vaM.WriteByte(xbotBase + 79536, 1);
			vaM.WriteByte(xbotBase + 309116, 0);
			vaM.WriteByte(xbotBase + 79596, 0);
		}
		if (cb_smart_rec.Checked)
		{
			vaM.WriteByte(xbotBase + 309116, 1);
			vaM.WriteByte(xbotBase + 79536, 1);
			vaM.WriteByte(xbotBase + 79596, 0);
			vaM.WriteFloat(xbotBase + 77876, (float)num_boost.Value);
			vaM.WriteFloat(xbotBase + 77872, (float)num_speedhack.Value);
			writeStatus("Recording... Press [F10] to save and [F5] to reset the recording!");
		}
		else if (cb_practice_rec.Checked)
		{
			vaM.WriteByte(xbotBase + 309116, 0);
			vaM.WriteByte(xbotBase + 79596, 1);
			writeStatus("Recording... Press [F10] to save and [F5] to reset the recording!");
		}
		else
		{
			if (cb_auto_reset.Checked)
			{
				vaM.WriteByte(xbotBase + 79536, 1);
			}
			vaM.WriteByte(xbotBase + 309116, 0);
			vaM.WriteByte(xbotBase + 79596, 0);
			writeStatus("Recording... Press [F10] to save and [F5] to reset the recording after death!");
		}
		SendKey();
		bool flag = true;
		while (flag)
		{
			Thread.Sleep(40);
			pb_logo.Focus();
			Application.DoEvents();
			if (cb_smart_rec.Checked)
			{
				if ((int)(Keyboard.GetKeyStates(Key.F10) & KeyStates.Down) > 0)
				{
					flag = false;
					vaM.WriteByte(xbotBase + 309116, 0);
					vaM.WriteByte(xbotBase + 309128, 0);
					vaM.WriteByte(xbotBase + 79536, 0);
					if (cb_framerate.Checked)
					{
						vaM.WriteByte(xbotBase + 210752, 0);
					}
					writeStatus("Save...");
					int num = (int)num_fps.Value;
					int num2 = vaM.ReadInt32(xbotBase + 309120);
					string text = "fps: " + num + Environment.NewLine;
					text = ((!cb_framerate.Checked) ? (text + "pro_plus" + Environment.NewLine) : (text + "frames" + Environment.NewLine));
					writeStatus("Read macro...");
					for (int i = 0; i < num2; i += 8)
					{
						int num3 = vaM.ReadInt32(xbotBase + 79696 + i);
						int num4 = vaM.ReadByte(xbotBase + 79696 + (i + 4));
						if (num3 != 0)
						{
							text = text + num4 + " " + num3 + Environment.NewLine;
						}
						else if (num4 == 1 && vaM.ReadInt32(xbotBase + 79696 + i + 8) == 0)
						{
							i += 8;
						}
					}
					vaM.WriteInt32(xbotBase + 309120, 0);
					writeStatus("Write macro to file...");
					File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", txt_record.Text), text);
					writeStatus(copyrightNote);
				}
				if ((int)(Keyboard.GetKeyStates(Key.F5) & KeyStates.Down) > 0)
				{
					writeStatus("Reset...");
					vaM.ReadInt32(xbotBase + 309120);
					vaM.WriteInt32(xbotBase + 309120, 0);
					writeStatus("Recording... Press [F10] to save and [F5] to reset the recording!");
				}
				continue;
			}
			if (cb_practice_rec.Checked)
			{
				if ((int)(Keyboard.GetKeyStates(Key.F10) & KeyStates.Down) > 0)
				{
					flag = false;
					vaM.WriteByte(xbotBase + 309128, 0);
					vaM.WriteByte(xbotBase + 79596, 0);
					writeStatus("Save...");
					int num5 = (int)num_fps.Value;
					int num6 = vaM.ReadInt32(xbotBase + 309120);
					string text2 = "fps: " + num5 + Environment.NewLine;
					text2 = ((!cb_framerate.Checked) ? (text2 + "pro_plus" + Environment.NewLine) : (text2 + "frames" + Environment.NewLine));
					writeStatus("Read macro...");
					for (int j = 0; j < num6; j += 8)
					{
						int num7 = vaM.ReadInt32(xbotBase + 79696 + j);
						int num8 = vaM.ReadByte(xbotBase + 79696 + (j + 4));
						if (num7 != 0)
						{
							text2 = text2 + num8 + " " + num7 + Environment.NewLine;
						}
						else if (num8 == 1 && vaM.ReadInt32(xbotBase + 79696 + j + 8) == 0)
						{
							j += 8;
						}
					}
					vaM.WriteInt32(xbotBase + 309120, 0);
					writeStatus("Write macro to file...");
					File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", txt_record.Text), text2);
					writeStatus(copyrightNote);
				}
				if ((int)(Keyboard.GetKeyStates(Key.F5) & KeyStates.Down) > 0)
				{
					writeStatus("Reset...");
					vaM.WriteInt32(xbotBase + 309120, 0);
					vaM.WriteByte(xbotBase + 210772, 0);
					writeStatus("Recording... Press [F10] to save and [F5] to reset the recording after death!");
				}
				continue;
			}
			if ((int)(Keyboard.GetKeyStates(Key.F10) & KeyStates.Down) > 0)
			{
				flag = false;
				vaM.WriteByte(xbotBase + 309128, 0);
				vaM.WriteByte(xbotBase + 79536, 0);
				if (cb_framerate.Checked)
				{
					vaM.WriteByte(xbotBase + 210752, 0);
				}
				writeStatus("Save...");
				int num9 = (int)num_fps.Value;
				int num10 = vaM.ReadInt32(xbotBase + 309120);
				string text3 = "fps: " + num9 + Environment.NewLine;
				text3 = ((!cb_framerate.Checked) ? (text3 + "pro_plus" + Environment.NewLine) : (text3 + "frames" + Environment.NewLine));
				writeStatus("Read macro...");
				for (int k = 0; k < num10; k += 8)
				{
					int num11 = vaM.ReadInt32(xbotBase + 79696 + k);
					int num12 = vaM.ReadByte(xbotBase + 79696 + (k + 4));
					if (num11 != 0)
					{
						text3 = text3 + num12 + " " + num11 + Environment.NewLine;
					}
					else if (num12 == 1 && vaM.ReadInt32(xbotBase + 79696 + k + 8) == 0)
					{
						k += 8;
					}
				}
				vaM.WriteInt32(xbotBase + 309120, 0);
				writeStatus("Write macro to file...");
				File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", txt_record.Text), text3);
				writeStatus(copyrightNote);
			}
			if ((int)(Keyboard.GetKeyStates(Key.F5) & KeyStates.Down) > 0)
			{
				writeStatus("Reset...");
				vaM.WriteInt32(xbotBase + 309120, 0);
				writeStatus("Recording... Press [F10] to save and [F5] to reset the recording after death!");
			}
		}
		btn_record.ForeColor = Color.Gainsboro;
		lbl_toolbar.ForeColor = Color.Gainsboro;
		btn_play.Enabled = true;
		btn_play_seq.Enabled = true;
		cb_smart_rec.Enabled = true;
		cb_practice_rec.Enabled = true;
		btn_record.Enabled = true;
		tc_main.Enabled = true;
	}

	private void btn_apply_Click(object sender, EventArgs e)
	{
		writeConfig();
	}

	private string getMac()
	{
		string text = (from nic in NetworkInterface.GetAllNetworkInterfaces()
			where nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
			select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
		if (text == null)
		{
			return "null";
		}
		return ProcessString(text);
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

	private void btn_close_Click(object sender, EventArgs e)
	{
		Environment.Exit(Environment.ExitCode);
	}

	private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ReleaseCapture();
			SendMessage(base.Handle, 161, 2, 0);
		}
	}

	private void tabControl3_Selected(object sender, TabControlEventArgs e)
	{
		if (tc_play.SelectedIndex == 1)
		{
			cb_play_seq.Items.Clear();
			ComboBox.ObjectCollection items = cb_play_seq.Items;
			object[] macroNames = getMacroNames("seq_");
			items.AddRange(macroNames);
			cb_play_seq.Sorted = true;
			cb_play_seq.SelectionStart = cb_play_macro.Text.Length;
			cb_play_seq.SelectionLength = 0;
			cb_play_seq.DropDownStyle = ComboBoxStyle.DropDown;
			cb_play_seq.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			cb_play_seq.AutoCompleteSource = AutoCompleteSource.ListItems;
		}
	}

	private void cb_play_seq_SelectedIndexChanged(object sender, EventArgs e)
	{
		string pattern = "_" + cb_play_seq.Text.Split('_')[cb_play_seq.Text.Split('_').Length - 2] + "_";
		string[] macroNames = getMacroNames(pattern);
		lb_play_seq.Items.Clear();
		ListBox.ObjectCollection items = lb_play_seq.Items;
		object[] items2 = macroNames;
		items.AddRange(items2);
	}

	private void tmr_prevent_max_Tick(object sender, EventArgs e)
	{
		if (base.WindowState == FormWindowState.Maximized)
		{
			base.WindowState = FormWindowState.Normal;
		}
	}

	private void cb_smart_rec_CheckedChanged(object sender, EventArgs e)
	{
		if (cb_smart_rec.Checked)
		{
			lbl_smartmsg.Visible = true;
			cb_practice_rec.Enabled = false;
			num_boost.Enabled = true;
			cb_auto_reset.Enabled = false;
			cb_auto_reset.Checked = false;
		}
		else
		{
			num_boost.Enabled = false;
			lbl_smartmsg.Visible = false;
			cb_practice_rec.Enabled = true;
			cb_auto_reset.Enabled = true;
		}
	}

	private void btn_close_MouseEnter(object sender, EventArgs e)
	{
		((Button)sender).BackgroundImage = btn_close_hover.BackgroundImage;
	}

	private void btn_close_MouseLeave(object sender, EventArgs e)
	{
		((Button)sender).BackgroundImage = btn_close_leave.BackgroundImage;
	}

	private void lb_macros_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			string obj = File.ReadAllText(text + lb_macros.Items[lb_macros.SelectedIndex]);
			lbl_path_macros.Text = text + lb_macros.Items[lb_macros.SelectedIndex];
			float num = (float)obj.Length / 1024f;
			lbl_size_macros.Text = num.ToString("N2") + " KB";
			txt_macroName.Text = (string)lb_macros.Items[lb_macros.SelectedIndex];
		}
		catch
		{
			lbl_path_macros.Text = "n/a";
			lbl_size_macros.Text = "n/a";
			txt_macroName.Text = "";
		}
	}

	private void lbl_h_crash_Click(object sender, EventArgs e)
	{
		lbl_help_answer.Text = "If you run the steam version of Geometry Dash, check if you’ve installed an update for the game lately.. (if yes this could be the reason.)  If you run a cracked version of the Game, make sure that you are using \"Geometry Dash 2.113\".If you use Pro record, make sure you follow the information provided if you select the pro-record setting!";
	}

	private void lbl_h_online_Click(object sender, EventArgs e)
	{
		lbl_help_answer.Text = "If you can't browse, upload or download macros from the online section of the bot, the problem could be on both sides. That means a) my server isn't working properly or b) your internet connection is the reason for the problem. In case a) I am probably already working on it, and in case b) you could try a VPN.";
	}

	private void lbl_h_dll_Click(object sender, EventArgs e)
	{
		lbl_help_answer.Text = "If the bot can't start properly, or you can't use some functions of it due to missing DLL files, I recommend to reinstall the bot and try again.";
	}

	private void lbl_h_macro_Click(object sender, EventArgs e)
	{
		lbl_help_answer.Text = "If you miss a macro you created, the reason could be that the macro wasn't saved correctly. For how to save a macro correctly you can look in the Controls menu.";
	}

	private void lbl_h_slowgame_Click(object sender, EventArgs e)
	{
		lbl_help_answer.Text = "If your game is running slow when you use the bot, there might be an interference with another hack like Absolute's Mega Hack v6, a framerate hack, or you forgot to disable VSYNC in the games settings.";
	}

	private void lbl_h_dual_Click(object sender, EventArgs e)
	{
		lbl_help_answer.Text = "If your dual parts are not accurate, there are several things, that can cause this problem. Make sure that VSYNC is disabled and your PC can handle the framerate you set.";
	}

	private void btn_minimize_MouseEnter(object sender, EventArgs e)
	{
		btn_minimize.BackgroundImage = btn_minimize_hover.BackgroundImage;
	}

	private void btn_minimize_MouseLeave(object sender, EventArgs e)
	{
		btn_minimize.BackgroundImage = btn_minimize_leave.BackgroundImage;
	}

	private void btn_minimize_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void btn_leaderboard_Click(object sender, EventArgs e)
	{
		lv_leaderboard.Items.Clear();
		lv_online.Visible = false;
		lbl_online_id.Visible = false;
		lbl_online_name.Visible = false;
		lbl_online_size.Visible = false;
		lbl_online_creator.Visible = false;
		lbl_online_type.Visible = false;
		lbl_online_lid.Visible = false;
		lbl_online_verified.Visible = false;
		try
		{
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
		catch
		{
			MessageBox.Show("A network error has occured!");
		}
	}

	private void btn_macor_sounds_Click(object sender, EventArgs e)
	{
		ClickSounds clickSounds = new ClickSounds();
		clickSounds.Show();
		clickSounds.Start(cb_play_macro.Text);
	}

	private void cb_practice_rec_CheckedChanged(object sender, EventArgs e)
	{
		if (cb_practice_rec.Checked)
		{
			lbl_practice_msg.Visible = true;
			cb_smart_rec.Enabled = false;
			cb_auto_reset.Enabled = false;
			cb_auto_reset.Checked = false;
		}
		else
		{
			lbl_practice_msg.Visible = false;
			cb_smart_rec.Enabled = true;
			cb_auto_reset.Enabled = true;
		}
	}

	private void btn_apply_rename_Click(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			File.Move(text + lb_macros.Items[lb_macros.SelectedIndex], text + txt_rename.Text);
		}
		catch
		{
			MessageBox.Show("Error");
			return;
		}
		lb_macros.Items[lb_macros.SelectedIndex] = txt_rename.Text;
		txt_macroName.Text = txt_rename.Text;
		MessageBox.Show("Macro renamed successfully!");
	}

	private void btn_delete_Click(object sender, EventArgs e)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			File.Delete(text + lb_macros.Items[lb_macros.SelectedIndex]);
		}
		catch
		{
			MessageBox.Show("Error");
			return;
		}
		lb_macros.Items.RemoveAt(lb_macros.SelectedIndex);
		MessageBox.Show("Macro deleted successfully!");
	}

	private void btn_start_up_Click(object sender, EventArgs e)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string value = "";
		text = txt_macroName.Text;
		text2 = txt_levelid.Text;
		string text4 = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		try
		{
			text3 = File.ReadAllText(text4 + text);
		}
		catch
		{
			MessageBox.Show("Error!");
		}
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
		string text5 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text3));
		string text6 = getClientID();
		try
		{
			WebClient webClient = new WebClient();
			byte[] bytes = webClient.DownloadData("http://xbot.4uhr20.eu/login.php?clientid=" + text6);
			string @string = Encoding.ASCII.GetString(bytes);
			if (@string == "-1" || @string == "-3" || @string.Contains(":1:"))
			{
				MessageBox.Show("Invalid License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (cb_anonymously.Checked)
			{
				text6 = "";
			}
			text5 = text5.Replace('=', '_');
			text6 = text6.Replace('=', '_');
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["name"] = text;
			nameValueCollection["macro"] = text5;
			nameValueCollection["creator"] = text6;
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
		catch
		{
			MessageBox.Show("A network error has occured!");
		}
	}

	private void btn_laod_sounds_Click(object sender, EventArgs e)
	{
		lv_clicks.Items.Clear();
		Start(cb_play_macro.Text);
	}

	public void Start(string macro_name)
	{
		try
		{
			macroname = macro_name;
			lv_clicks.BeginUpdate();
			interpretMacro(loadmacro(macro_name));
			lv_clicks.EndUpdate();
			lbl_clicks_click.Size = new Size(69, 23);
		}
		catch (Exception ex)
		{
			lv_clicks.EndUpdate();
			Console.WriteLine(ex.Message);
		}
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
		currentFps = int.Parse(list[list.FindIndex((string i) => i.Contains("fps"))].Split(' ')[1]);
		list.RemoveAt(list.FindIndex((string i) => i.Contains("fps")));
		list.RemoveAt(list.FindIndex((string i) => i.Contains("pro")));
		while (list.FindIndex((string i) => i.ToString() == "") != -1)
		{
			list.RemoveAt(list.FindIndex((string i) => i.ToString() == ""));
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

	private string getSoundfile(bool hard, bool down, int key, int index)
	{
		string text = AppDomain.CurrentDomain.BaseDirectory + "wav\\";
		int num = BitConverter.ToInt32(randombytes, index);
		if (num < 0)
		{
			num *= -1;
		}
		switch (key)
		{
		case 1:
			text += "mouse\\";
			if (down)
			{
				if (hard)
				{
					return text + "mouse_hard_down_0" + (num % 4 + 1) + ".wav";
				}
				return text + "mouse_soft_down_0" + (num % 4 + 1) + ".wav";
			}
			if (hard)
			{
				return text + "mouse_hard_release_0" + (num % 4 + 1) + ".wav";
			}
			return text + "mouse_soft_release_0" + (num % 4 + 1) + ".wav";
		case 2:
			text += "uparrow\\";
			if (down)
			{
				if (hard)
				{
					return text + "up_hard_press_0" + (num % 4 + 1) + ".wav";
				}
				return text + "up_soft_press_0" + (num % 4 + 1) + ".wav";
			}
			if (hard)
			{
				return text + "up_hard_release_0" + (num % 4 + 1) + ".wav";
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
		RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		randombytes = new byte[4 * clicklist.Length];
		randomNumberGenerator.GetBytes(randombytes);
		for (int i = 0; i < clicklist.Length; i++)
		{
			float num = Int32BitsToSingle(int.Parse(macro[i].Split(' ')[1]));
			bool flag = false;
			bool flag2;
			bool flag3;
			if (int.Parse(macro[i].Split(' ')[0]) == 1)
			{
				flag = true;
				if (Int32BitsToSingle(int.Parse(macro[i + 1].Split(' ')[1])) - Int32BitsToSingle(int.Parse(macro[i].Split(' ')[1])) >= (float)distance)
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: true, down: true, keyP1, i * 4));
					flag2 = true;
					flag3 = true;
				}
				else
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: false, down: true, keyP1, i * 4));
					flag2 = true;
					flag3 = false;
				}
			}
			else if (int.Parse(macro[i].Split(' ')[0]) == 0)
			{
				flag = true;
				if (Int32BitsToSingle(int.Parse(macro[i].Split(' ')[1])) - Int32BitsToSingle(int.Parse(macro[i - 1].Split(' ')[1])) >= (float)distance)
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: true, down: false, keyP1, i * 4));
					flag2 = false;
					flag3 = true;
				}
				else
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: false, down: false, keyP1, i * 4));
					flag2 = false;
					flag3 = false;
				}
			}
			else if (int.Parse(macro[i].Split(' ')[0]) == 3)
			{
				if (Int32BitsToSingle(int.Parse(macro[i + 1].Split(' ')[1])) - Int32BitsToSingle(int.Parse(macro[i].Split(' ')[1])) >= (float)distance)
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: true, down: true, keyP2, i * 4));
					flag2 = true;
					flag3 = true;
				}
				else
				{
					clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: true, getSoundfile(hard: false, down: true, keyP2, i * 4));
					flag2 = true;
					flag3 = false;
				}
			}
			else if (Int32BitsToSingle(int.Parse(macro[i].Split(' ')[1])) - Int32BitsToSingle(int.Parse(macro[i - 1].Split(' ')[1])) >= (float)distance)
			{
				clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: true, down: false, keyP2, i * 4));
				flag2 = false;
				flag3 = true;
			}
			else
			{
				clicklist[i] = new MacroClick(int.Parse(macro[i].Split(' ')[1]), down: false, getSoundfile(hard: false, down: false, keyP2, i * 4));
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
		bool flag = false;
		if (vaM.CheckProcess())
		{
			int num = keeProc.ReadAddress(kProc, "GeometryDash.exe+003222D0 164 224 67C");
			for (int i = 0; i < clicklist.Length; i++)
			{
				MacroClick macroClick = clicklist[i];
				int num2 = 0;
				while (num2 < macroClick.getCoord)
				{
					Thread.Sleep(1);
					num2 = vaM.ReadInt32((IntPtr)num);
					num = keeProc.ReadAddress(kProc, "GeometryDash.exe+003222D0 164 224 67C");
					if (num2 < 1115243428 && macroClick.getCoord > 1115243428)
					{
						i = -1;
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
		btn_execute.Invoke((Action)delegate
		{
			btn_execute.ForeColor = Color.Gainsboro;
			btn_execute.Text = "▶    Start";
		});
	}

	private void btn_execute_Click(object sender, EventArgs e)
	{
		if (clicklist == null)
		{
			return;
		}
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

	private void tb_distance_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		distance = tb_distance.Value;
		lv_clicks.Items.Clear();
		Start(macroname);
	}

	private void btn_join_Click(object sender, EventArgs e)
	{
		Process.Start(serverURL);
	}

	private void btn_copy_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(txt_cid.Text);
		MessageBox.Show("Copied to Clipboard!", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	private void btn_reload_license_Click(object sender, EventArgs e)
	{
		updateSQLEntry();
		if (pro)
		{
			checkVersion();
			StartingSequence();
			loadConfig();
		}
		writeStatus(copyrightNote);
	}

	private void focusthing()
	{
		while (true)
		{
			removeFocusFromButton(this);
			Thread.Sleep(250);
		}
	}

	private void removeFocusFromButton(Control ctl)
	{
		foreach (Control c in ctl.Controls)
		{
			if (c is TabControl || c is GroupBox || c is TabPage || c is Panel)
			{
				removeFocusFromButton(c);
			}
			if (!(c is Button))
			{
				continue;
			}
			try
			{
				c.Invoke((Action)delegate
				{
					if (c.Focused && Control.MouseButtons != MouseButtons.Left)
					{
						pb_logo.Invoke((Action)delegate
						{
							pb_logo.Focus();
						});
					}
				});
			}
			catch
			{
			}
		}
	}

	private void btn_switch_cid_Click(object sender, EventArgs e)
	{
		if (btn_switch_cid.Text.Contains("old"))
		{
			string mac = getMac();
			if (mac != "null")
			{
				txt_cid.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(mac));
			}
			else
			{
				txt_cid.Text = "null";
			}
			btn_switch_cid.Text = "Show new ClientID";
		}
		else
		{
			txt_cid.Text = getClientID();
			btn_switch_cid.Text = "Show old ClientID";
		}
	}

	private void btn_upgrade_Click(object sender, EventArgs e)
	{
		if (!v2_21)
		{
			btn_switch_cid.Visible = false;
		}
		else
		{
			btn_switch_cid.Visible = true;
		}
		byte[] bytes = new WebClient().DownloadData("http://xbot.4uhr20.eu/message.php");
		string clientID = getClientID();
		string @string = Encoding.ASCII.GetString(bytes);
		string[] separator = new string[1] { "<br>" };
		string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		string text = array[0];
		string text2 = array[1];
		if (text.Length > 1)
		{
			lbl_message.Text = text;
			txt_cid.Text = clientID;
			serverURL = text2;
		}
		tc_main.Visible = false;
		gb_license.Visible = true;
	}

	private void btn_close_edit_Click(object sender, EventArgs e)
	{
		pnl_edit.Visible = false;
		if (getCurCoordThread != null)
		{
			getCurCoordThread.Abort();
		}
	}

	private void btn_edit_Click(object sender, EventArgs e)
	{
		if (!(cb_play_macro.Text != ""))
		{
			return;
		}
		pnl_edit.Visible = true;
		lv_edit_coords.Items.Clear();
		string[] array = loadmacro(cb_play_macro.Text);
		if (array == null)
		{
			return;
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			double num = Int32BitsToSingle(int.Parse(text.Split(' ')[1]));
			string text2 = "";
			ListViewItem listViewItem = new ListViewItem();
			listViewItem.UseItemStyleForSubItems = false;
			lv_clicks.View = View.Details;
			switch (int.Parse(text.Split(' ')[0]))
			{
			case 0:
				text2 = "P1 Release";
				break;
			case 1:
				text2 = "P1 Press";
				break;
			case 2:
				text2 = "P2 Release";
				break;
			case 3:
				text2 = "P2 Press";
				break;
			}
			listViewItem = new ListViewItem(new string[2]
			{
				text2,
				num.ToString("G10")
			});
			switch (int.Parse(text.Split(' ')[0]))
			{
			case 0:
				listViewItem.ForeColor = Color.FromArgb(210, 210, 255);
				break;
			case 1:
				listViewItem.ForeColor = Color.FromArgb(230, 230, 255);
				break;
			case 2:
				listViewItem.ForeColor = Color.FromArgb(210, 255, 210);
				break;
			case 3:
				listViewItem.ForeColor = Color.FromArgb(230, 255, 230);
				break;
			}
			lv_edit_coords.Items.Add(listViewItem);
		}
		getCurCoordThread = new Thread((ThreadStart)delegate
		{
			getPosition();
		});
		getCurCoordThread.Start();
	}

	private void getPosition()
	{
		while (true)
		{
			if (vaM.CheckProcess())
			{
				int num = keeProc.ReadAddress(kProc, "GeometryDash.exe+003222D0 164 224 67C");
				Thread.Sleep(100);
				currentCoordinate = vaM.ReadFloat((IntPtr)num);
				lbl_edit_cur_coord.Invoke((Action)delegate
				{
					lbl_edit_cur_coord.Text = currentCoordinate.ToString("G10");
				});
			}
		}
	}

	public unsafe static int SingleToInt32Bits(float value)
	{
		return *(int*)(&value);
	}

	public unsafe static float Int32BitsToSingle(int value)
	{
		return *(float*)(&value);
	}

	private void lv_edit_coords_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			currentClick = new EditClick();
			double num = 0.0;
			double num2 = 0.0;
			tb_edit_press.Value = 0;
			tb_edit_release.Value = 0;
			ListViewItem listViewItem = lv_edit_coords.SelectedItems[0];
			int i = lv_edit_coords.Items.IndexOf(listViewItem);
			switch (listViewItem.SubItems[0].Text)
			{
			case "P1 Press":
				num = double.Parse(listViewItem.SubItems[1].Text);
				currentClick.down = num;
				currentClick.idown = i;
				for (; lv_edit_coords.Items[i].SubItems[0].Text != "P1 Release"; i++)
				{
				}
				num2 = double.Parse(lv_edit_coords.Items[i].SubItems[1].Text);
				currentClick.up = num2;
				currentClick.iup = i;
				lbl_edit_coord_press.Text = num.ToString("G10");
				lbl_edit_coord_release.Text = num2.ToString("G10");
				break;
			case "P1 Release":
				num2 = double.Parse(listViewItem.SubItems[1].Text);
				currentClick.up = num2;
				currentClick.iup = i;
				while (lv_edit_coords.Items[i].SubItems[0].Text != "P1 Press")
				{
					i--;
				}
				num = double.Parse(lv_edit_coords.Items[i].SubItems[1].Text);
				currentClick.down = num;
				currentClick.idown = i;
				lbl_edit_coord_press.Text = num.ToString("G10");
				lbl_edit_coord_release.Text = num2.ToString("G10");
				break;
			case "P2 Press":
				num = double.Parse(listViewItem.SubItems[1].Text);
				currentClick.down = num;
				currentClick.idown = i;
				for (; lv_edit_coords.Items[i].SubItems[0].Text != "P2 Release"; i++)
				{
				}
				num2 = double.Parse(lv_edit_coords.Items[i].SubItems[1].Text);
				currentClick.up = num2;
				currentClick.iup = i;
				lbl_edit_coord_press.Text = num.ToString("G10");
				lbl_edit_coord_release.Text = num2.ToString("G10");
				break;
			case "P2 Release":
				num2 = double.Parse(listViewItem.SubItems[1].Text);
				currentClick.up = num2;
				currentClick.iup = i;
				while (lv_edit_coords.Items[i].SubItems[0].Text != "P2 Press")
				{
					i--;
				}
				num = double.Parse(lv_edit_coords.Items[i].SubItems[1].Text);
				currentClick.down = num;
				currentClick.idown = i;
				lbl_edit_coord_press.Text = num.ToString("G10");
				lbl_edit_coord_release.Text = num2.ToString("G10");
				break;
			}
		}
		catch
		{
		}
	}

	private void tb_edit_press_Scroll(object sender, EventArgs e)
	{
		try
		{
			if (currentClick.down + (double)((float)tb_edit_press.Value / 10f) < currentClick.up + (double)((float)tb_edit_release.Value / 10f))
			{
				lbl_edit_coord_press.Text = (currentClick.down + (double)((float)tb_edit_press.Value / 10f)).ToString("G10");
			}
		}
		catch
		{
		}
	}

	private void tb_edit_release_Scroll(object sender, EventArgs e)
	{
		try
		{
			if (currentClick.up + (double)((float)tb_edit_release.Value / 10f) > currentClick.down + (double)((float)tb_edit_press.Value / 10f))
			{
				lbl_edit_coord_release.Text = (currentClick.up + (double)((float)tb_edit_release.Value / 10f)).ToString("G10");
			}
		}
		catch
		{
		}
	}

	private void btn_edit_save_click_Click(object sender, EventArgs e)
	{
		if (!(lbl_edit_coord_press.Text == "n/a") && !(lbl_edit_coord_release.Text == "n/a"))
		{
			lv_edit_coords.Items[currentClick.idown].SubItems[1].Text = lbl_edit_coord_press.Text;
			lv_edit_coords.Items[currentClick.iup].SubItems[1].Text = lbl_edit_coord_release.Text;
			tb_edit_press.Value = 0;
			tb_edit_release.Value = 0;
		}
	}

	private void btn_edit_delete_Click(object sender, EventArgs e)
	{
		if (!(lbl_edit_coord_press.Text == "n/a") && !(lbl_edit_coord_release.Text == "n/a") && lv_edit_coords.SelectedItems.Count == 1)
		{
			int idown = currentClick.idown;
			int iup = currentClick.iup;
			lv_edit_coords.Items[idown].Remove();
			lv_edit_coords.Items[iup - 1].Remove();
		}
	}

	private void btn_edit_save_Click(object sender, EventArgs e)
	{
		string[] array = new string[lv_edit_coords.Items.Count + 2];
		array[0] = "fps: " + currentFps;
		array[1] = "pro_plus";
		for (int i = 0; i < lv_edit_coords.Items.Count; i++)
		{
			string text = lv_edit_coords.Items[i].SubItems[0].Text;
			string text2 = SingleToInt32Bits(float.Parse(lv_edit_coords.Items[i].SubItems[1].Text)).ToString();
			if (text == "P1 Press")
			{
				text = "1";
			}
			if (text == "P1 Release")
			{
				text = "0";
			}
			if (text == "P2 Press")
			{
				text = "3";
			}
			if (text == "P2 Release")
			{
				text = "2";
			}
			array[i + 2] = text + " " + text2;
		}
		File.WriteAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "macros\\", cb_play_macro.Text), array);
		pnl_edit.Visible = false;
		getCurCoordThread.Abort();
	}

	private void btn_edit_sel_next_Click(object sender, EventArgs e)
	{
		lv_edit_coords.SelectedIndices.Clear();
		for (int i = 0; i < lv_edit_coords.Items.Count; i++)
		{
			if ((double)float.Parse(lv_edit_coords.Items[i].SubItems[1].Text) > currentCoordinate)
			{
				lv_edit_coords.Items[i].Selected = true;
				lv_edit_coords.EnsureVisible(i);
				break;
			}
		}
	}

	private void btn_edit_merge_Click(object sender, EventArgs e)
	{
		string initialDirectory = AppDomain.CurrentDomain.BaseDirectory + "macros\\";
		fd_edit_merge.InitialDirectory = initialDirectory;
		fd_edit_merge.ShowDialog();
		if (!fd_edit_merge.CheckFileExists || fd_edit_merge.FileName == "")
		{
			return;
		}
		string fileName = fd_edit_merge.FileName;
		string[] array = File.ReadAllLines(fileName);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.Contains("fps: "))
			{
				if (int.Parse(text.Split(' ')[1]) == currentFps)
				{
					break;
				}
				MessageBox.Show("Error, the macros are not recorded with the same framerate!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
		}
		array = loadmacro(fileName.Split('\\')[fileName.Split('\\').Length - 1]);
		int num = int.Parse(array[0].Split(' ')[1]);
		int num2 = int.Parse(array[array.Length - 1].Split(' ')[1]);
		int num3 = 0;
		int num4 = 0;
		for (int j = 0; j < lv_edit_coords.Items.Count; j++)
		{
			int num5 = SingleToInt32Bits((float)double.Parse(lv_edit_coords.Items[j].SubItems[1].Text));
			if (num5 <= num)
			{
				num3 = j;
			}
			if (num5 <= num2)
			{
				num4 = j;
			}
		}
		if (num3 >= lv_edit_coords.Items.Count - 1)
		{
			for (int k = 0; k < array.Length; k++)
			{
				string text2 = "";
				switch (int.Parse(array[k].Split(' ')[0]))
				{
				case 0:
					text2 = "P1 Release";
					break;
				case 1:
					text2 = "P1 Press";
					break;
				case 2:
					text2 = "P2 Release";
					break;
				case 3:
					text2 = "P2 Press";
					break;
				}
				ListViewItem listViewItem = new ListViewItem(new string[2]
				{
					text2,
					Int32BitsToSingle(int.Parse(array[k].Split(' ')[1])).ToString("G10")
				});
				switch (int.Parse(array[k].Split(' ')[0]))
				{
				case 0:
					listViewItem.ForeColor = Color.FromArgb(210, 210, 255);
					break;
				case 1:
					listViewItem.ForeColor = Color.FromArgb(230, 230, 255);
					break;
				case 2:
					listViewItem.ForeColor = Color.FromArgb(210, 255, 210);
					break;
				case 3:
					listViewItem.ForeColor = Color.FromArgb(230, 255, 230);
					break;
				}
				lv_edit_coords.Items.Add(listViewItem);
			}
		}
		else if (num4 < 1)
		{
			ListViewItem[] array3 = new ListViewItem[lv_edit_coords.Items.Count];
			lv_edit_coords.Items.CopyTo(array3, 0);
			lv_edit_coords.Items.Clear();
			for (int l = 0; l < array.Length; l++)
			{
				string text3 = "";
				switch (int.Parse(array[l].Split(' ')[0]))
				{
				case 0:
					text3 = "P1 Release";
					break;
				case 1:
					text3 = "P1 Press";
					break;
				case 2:
					text3 = "P2 Release";
					break;
				case 3:
					text3 = "P2 Press";
					break;
				}
				ListViewItem listViewItem2 = new ListViewItem(new string[2]
				{
					text3,
					Int32BitsToSingle(int.Parse(array[l].Split(' ')[1])).ToString("G10")
				});
				switch (int.Parse(array[l].Split(' ')[0]))
				{
				case 0:
					listViewItem2.ForeColor = Color.FromArgb(210, 210, 255);
					break;
				case 1:
					listViewItem2.ForeColor = Color.FromArgb(230, 230, 255);
					break;
				case 2:
					listViewItem2.ForeColor = Color.FromArgb(210, 255, 210);
					break;
				case 3:
					listViewItem2.ForeColor = Color.FromArgb(230, 255, 230);
					break;
				}
				lv_edit_coords.Items.Add(listViewItem2);
			}
			for (int m = 0; m < array3.Length; m++)
			{
				lv_edit_coords.Items.Add(array3[m]);
			}
		}
		else
		{
			if (num3 <= 1)
			{
				return;
			}
			if (lv_edit_coords.Items[num3 - 1].SubItems[0].Text == "P1 Press")
			{
				num3++;
			}
			if (num4 == lv_edit_coords.Items.Count)
			{
				num4++;
			}
			else
			{
				_ = lv_edit_coords.Items[num4].SubItems[0].Text == "P1 Release";
			}
			ListViewItem[] array4 = new ListViewItem[lv_edit_coords.Items.Count];
			lv_edit_coords.Items.CopyTo(array4, 0);
			int num6 = lv_edit_coords.Items.Count - (num4 - num3) + array.Length - 1;
			ListViewItem[] array5 = new ListViewItem[num6];
			for (int n = 0; n < num6; n++)
			{
				if (n < num3)
				{
					array5[n] = array4[n];
				}
				else if (n <= num3 + array.Length - 1)
				{
					string text4 = string.Empty;
					switch (int.Parse(array[n - num3].Split(' ')[0]))
					{
					case 0:
						text4 = "P1 Release";
						break;
					case 1:
						text4 = "P1 Press";
						break;
					case 2:
						text4 = "P2 Release";
						break;
					case 3:
						text4 = "P2 Press";
						break;
					}
					ListViewItem listViewItem3 = new ListViewItem(new string[2]
					{
						text4,
						Int32BitsToSingle(int.Parse(array[n - num3].Split(' ')[1])).ToString("G10")
					});
					switch (int.Parse(array[n - num3].Split(' ')[0]))
					{
					case 0:
						listViewItem3.ForeColor = Color.FromArgb(210, 210, 255);
						break;
					case 1:
						listViewItem3.ForeColor = Color.FromArgb(230, 230, 255);
						break;
					case 2:
						listViewItem3.ForeColor = Color.FromArgb(210, 255, 210);
						break;
					case 3:
						listViewItem3.ForeColor = Color.FromArgb(230, 255, 230);
						break;
					}
					array5[n] = listViewItem3;
				}
				else if (n > num3 + array.Length - 1)
				{
					int num7 = num4 + (n - (array.Length + num3));
					array5[n] = array4[num7];
				}
			}
			lv_edit_coords.Items.Clear();
			ListViewItem[] array6 = array5;
			foreach (ListViewItem value in array6)
			{
				lv_edit_coords.Items.Add(value);
			}
		}
	}

	private void setGameSpeed(float speed)
	{
		vaM.WriteFloat(xbotBase + 77860, speed);
	}

	private void num_speedhack_ValueChanged(object sender, EventArgs e)
	{
		setGameSpeed((float)num_speedhack.Value);
	}

	private void tb_speed_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		decimal value = default(decimal);
		switch (tb_speed.Value)
		{
		case 4:
			value = 5m;
			break;
		case 3:
			value = 2.5m;
			break;
		case 2:
			value = 2m;
			break;
		case 1:
			value = 1.5m;
			break;
		case 0:
			value = 1m;
			break;
		case -1:
			value = 0.5m;
			break;
		case -2:
			value = 0.25m;
			break;
		case -3:
			value = 0.05m;
			break;
		case -4:
			value = 0.01m;
			break;
		}
		num_speedhack.Value = value;
	}

	private void cb_lagSpikes_CheckedChanged(object sender, EventArgs e)
	{
		if (!vaM.CheckProcess())
		{
			cb_lagSpikes.Checked = false;
		}
		else if (cb_lagSpikes.Checked)
		{
			vaM.WriteByte(xbotBase + 66381, 0);
		}
		else
		{
			vaM.WriteByte(xbotBase + 66381, 1);
		}
	}

	private void cb_sh_eject_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void cb_prac_music_CheckedChanged(object sender, EventArgs e)
	{
		if (cb_prac_music.Checked)
		{
			if (!vaM.CheckProcess())
			{
				cb_prac_music.Checked = false;
				return;
			}
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2148645, new byte[6] { 144, 144, 144, 144, 144, 144 });
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2150723, new byte[2] { 144, 144 });
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2139491, new byte[2] { 144, 144 });
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2139541, new byte[2] { 144, 144 });
		}
		else if (!vaM.CheckProcess())
		{
			cb_prac_music.Checked = false;
		}
		else
		{
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2148645, new byte[6] { 15, 133, 247, 0, 0, 0 });
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2150723, new byte[2] { 117, 65 });
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2139491, new byte[2] { 117, 62 });
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2139541, new byte[2] { 117, 12 });
		}
	}

	private void cb_death_effect_CheckedChanged(object sender, EventArgs e)
	{
		if (cb_death_effect.Checked)
		{
			if (!vaM.CheckProcess())
			{
				cb_death_effect.Checked = false;
				return;
			}
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2030500, new byte[5] { 144, 144, 144, 144, 144 });
		}
		else if (!vaM.CheckProcess())
		{
			cb_death_effect.Checked = false;
		}
		else
		{
			vaM.WriteByteArray((IntPtr)vaM.getBaseAddress + 2030500, new byte[5] { 232, 55, 0, 0, 0 });
		}
	}

	private void num_resp_time_ValueChanged(object sender, EventArgs e)
	{
		if (vaM.CheckProcess())
		{
			vaM.WriteInt32((IntPtr)vaM.getBaseAddress + 2139767, (int)(xbotBase + 79608));
			vaM.WriteFloat((IntPtr)vaM.ReadInt32((IntPtr)vaM.getBaseAddress + 2139767), (float)num_resp_time.Value);
		}
	}

	private void btn_close_license_Click(object sender, EventArgs e)
	{
		gb_license.Visible = false;
		tc_main.Visible = true;
		tc_main.Enabled = true;
		writeStatus(copyrightNote);
	}

	private void btn_toggle_smoothfix_Click(object sender, EventArgs e)
	{
		int num = (int)MessageBox.Show("Do you want to use Smoothfix?", "Smoothfix", MessageBoxButtons.YesNo);
		Console.WriteLine(num);
		vaM.WriteInt32(xbotBase + 309140, num - 6);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(xBot_Pro_UI.Form1));
		System.Windows.Forms.ListViewItem listViewItem = new System.Windows.Forms.ListViewItem(new string[2] { "test", "12" }, -1);
		System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[2] { "test2", "xd" }, -1);
		this.tc_main = new System.Windows.Forms.TabControl();
		this.tp_space = new System.Windows.Forms.TabPage();
		this.tp_rec = new System.Windows.Forms.TabPage();
		this.lbl_speedhack = new System.Windows.Forms.Label();
		this.cb_sh_eject = new System.Windows.Forms.CheckBox();
		this.gb_rec_options = new System.Windows.Forms.GroupBox();
		this.cb_death_effect = new System.Windows.Forms.CheckBox();
		this.cb_prac_music = new System.Windows.Forms.CheckBox();
		this.cb_auto_reset = new System.Windows.Forms.CheckBox();
		this.lbl_boostspeed = new System.Windows.Forms.Label();
		this.lbl_resp = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.lbl_death_effect = new System.Windows.Forms.Label();
		this.lbl_prac_music = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.num_resp_time = new System.Windows.Forms.NumericUpDown();
		this.lbl_practice_rec = new System.Windows.Forms.Label();
		this.num_fps = new System.Windows.Forms.NumericUpDown();
		this.cb_practice_rec = new System.Windows.Forms.CheckBox();
		this.num_boost = new System.Windows.Forms.NumericUpDown();
		this.cb_smart_rec = new System.Windows.Forms.CheckBox();
		this.cb_lagSpikes = new System.Windows.Forms.CheckBox();
		this.lbl_practice_msg = new System.Windows.Forms.Label();
		this.lbl_smart_framesnote = new System.Windows.Forms.Label();
		this.num_speedhack = new System.Windows.Forms.NumericUpDown();
		this.lbl_smartmsg = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txt_record = new System.Windows.Forms.TextBox();
		this.btn_record = new System.Windows.Forms.Button();
		this.label39 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.tb_speed = new System.Windows.Forms.TrackBar();
		this.lbl_path_rec = new System.Windows.Forms.Label();
		this.tp_play = new System.Windows.Forms.TabPage();
		this.tc_play = new System.Windows.Forms.TabControl();
		this.tp_play_normal = new System.Windows.Forms.TabPage();
		this.pnl_edit = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.btn_edit_save = new System.Windows.Forms.Button();
		this.btn_edit_merge = new System.Windows.Forms.Button();
		this.btn_edit_sel_next = new System.Windows.Forms.Button();
		this.lbl_edit_cur_coord = new System.Windows.Forms.Label();
		this.gb_edit = new System.Windows.Forms.GroupBox();
		this.btn_edit_delete = new System.Windows.Forms.Button();
		this.btn_edit_save_click = new System.Windows.Forms.Button();
		this.lbl_edit_coord_release = new System.Windows.Forms.Label();
		this.lbl_edit_coord_press = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.tb_edit_release = new System.Windows.Forms.TrackBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.tb_edit_press = new System.Windows.Forms.TrackBar();
		this.lv_edit_coords = new System.Windows.Forms.ListView();
		this.ch_edit_type = new System.Windows.Forms.ColumnHeader();
		this.ch_edit_coord = new System.Windows.Forms.ColumnHeader();
		this.label16 = new System.Windows.Forms.Label();
		this.btn_close_edit = new System.Windows.Forms.Button();
		this.cb_play_macro = new System.Windows.Forms.ComboBox();
		this.gb_clicks_play = new System.Windows.Forms.GroupBox();
		this.lbl_clicks_click = new System.Windows.Forms.Label();
		this.lbl_clicks_type = new System.Windows.Forms.Label();
		this.lbl_clicks_player = new System.Windows.Forms.Label();
		this.lbl_clicks_coords = new System.Windows.Forms.Label();
		this.pgb_state = new System.Windows.Forms.ProgressBar();
		this.label24 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.lbl_analyse = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.cb_keys = new System.Windows.Forms.ComboBox();
		this.btn_execute = new System.Windows.Forms.Button();
		this.lv_clicks = new System.Windows.Forms.ListView();
		this.ch_coord = new System.Windows.Forms.ColumnHeader();
		this.ch_player = new System.Windows.Forms.ColumnHeader();
		this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.ch_clicktype = new System.Windows.Forms.ColumnHeader();
		this.tb_distance = new System.Windows.Forms.TrackBar();
		this.btn_laod_sounds = new System.Windows.Forms.Button();
		this.lbl_macro_size_play = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.btn_edit = new System.Windows.Forms.Button();
		this.btn_play = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.tp_play_sequence = new System.Windows.Forms.TabPage();
		this.label28 = new System.Windows.Forms.Label();
		this.btn_play_seq = new System.Windows.Forms.Button();
		this.cb_play_seq = new System.Windows.Forms.ComboBox();
		this.label27 = new System.Windows.Forms.Label();
		this.lb_play_seq = new System.Windows.Forms.ListBox();
		this.tp_macros = new System.Windows.Forms.TabPage();
		this.gb_local = new System.Windows.Forms.GroupBox();
		this.txt_rename = new System.Windows.Forms.TextBox();
		this.btn_apply_rename = new System.Windows.Forms.Button();
		this.btn_delete = new System.Windows.Forms.Button();
		this.label17 = new System.Windows.Forms.Label();
		this.gb_upload = new System.Windows.Forms.GroupBox();
		this.txt_macroName = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.cb_anonymously = new System.Windows.Forms.CheckBox();
		this.btn_start_up = new System.Windows.Forms.Button();
		this.txt_levelid = new System.Windows.Forms.TextBox();
		this.lbl_size_macros = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.lbl_path_macros = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.btn_search_local = new System.Windows.Forms.Button();
		this.txt_search_local = new System.Windows.Forms.TextBox();
		this.lb_macros = new System.Windows.Forms.ListBox();
		this.tp_online = new System.Windows.Forms.TabPage();
		this.lbl_online_id = new System.Windows.Forms.Label();
		this.lbl_online_verified = new System.Windows.Forms.Label();
		this.lbl_online_lid = new System.Windows.Forms.Label();
		this.lbl_online_creator = new System.Windows.Forms.Label();
		this.lbl_online_type = new System.Windows.Forms.Label();
		this.lbl_online_size = new System.Windows.Forms.Label();
		this.lbl_online_name = new System.Windows.Forms.Label();
		this.lv_online = new System.Windows.Forms.ListView();
		this.ch_id = new System.Windows.Forms.ColumnHeader();
		this.ch_name = new System.Windows.Forms.ColumnHeader();
		this.ch_size = new System.Windows.Forms.ColumnHeader();
		this.ch_Type = new System.Windows.Forms.ColumnHeader();
		this.ch_creator = new System.Windows.Forms.ColumnHeader();
		this.ch_levelid = new System.Windows.Forms.ColumnHeader();
		this.ch_verified = new System.Windows.Forms.ColumnHeader();
		this.label25 = new System.Windows.Forms.Label();
		this.lbl_leaderboard_verified = new System.Windows.Forms.Label();
		this.lbl_leaderboard_creator = new System.Windows.Forms.Label();
		this.lv_leaderboard = new System.Windows.Forms.ListView();
		this.ch_lb_creator = new System.Windows.Forms.ColumnHeader();
		this.ch_amount = new System.Windows.Forms.ColumnHeader();
		this.btn_search = new System.Windows.Forms.Button();
		this.txt_search = new System.Windows.Forms.TextBox();
		this.btn_leaderboard = new System.Windows.Forms.Button();
		this.btn_load = new System.Windows.Forms.Button();
		this.tp_settings = new System.Windows.Forms.TabPage();
		this.btn_upgrade = new System.Windows.Forms.Button();
		this.cb_framerate = new System.Windows.Forms.CheckBox();
		this.txt_versions = new System.Windows.Forms.TextBox();
		this.btn_apply = new System.Windows.Forms.Button();
		this.num_fps_settings = new System.Windows.Forms.NumericUpDown();
		this.label14 = new System.Windows.Forms.Label();
		this.lbl_versions = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.tp_help = new System.Windows.Forms.TabPage();
		this.lbl_help_answer = new System.Windows.Forms.Label();
		this.lbl_h_dual = new System.Windows.Forms.Label();
		this.lbl_h_slowgame = new System.Windows.Forms.Label();
		this.lbl_h_macro = new System.Windows.Forms.Label();
		this.lbl_h_dll = new System.Windows.Forms.Label();
		this.lbl_h_online = new System.Windows.Forms.Label();
		this.lbl_h_crash = new System.Windows.Forms.Label();
		this.gb_license = new System.Windows.Forms.GroupBox();
		this.btn_switch_cid = new System.Windows.Forms.Button();
		this.btn_reload_license = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.txt_cid = new System.Windows.Forms.TextBox();
		this.btn_copy = new System.Windows.Forms.Button();
		this.btn_join = new System.Windows.Forms.Button();
		this.lbl_message = new System.Windows.Forms.Label();
		this.btn_close_license = new System.Windows.Forms.Button();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.lbl_toolbar = new System.Windows.Forms.ToolStripStatusLabel();
		this.btn_close = new System.Windows.Forms.Button();
		this.tmr_prevent_max = new System.Windows.Forms.Timer(this.components);
		this.gb_convert = new System.Windows.Forms.GroupBox();
		this.cb_convert_macros = new System.Windows.Forms.ComboBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.btn_convert = new System.Windows.Forms.Button();
		this.comboBox2 = new System.Windows.Forms.ComboBox();
		this.pb_logo = new System.Windows.Forms.PictureBox();
		this.pb_hidetab = new System.Windows.Forms.PictureBox();
		this.btn_minimize_leave = new System.Windows.Forms.Button();
		this.btn_minimize_hover = new System.Windows.Forms.Button();
		this.btn_minimize = new System.Windows.Forms.Button();
		this.fd_edit_merge = new System.Windows.Forms.OpenFileDialog();
		this.btn_close_leave = new System.Windows.Forms.Button();
		this.btn_close_hover = new System.Windows.Forms.Button();
		this.btn_toggle_smoothfix = new System.Windows.Forms.Button();
		this.tc_main.SuspendLayout();
		this.tp_rec.SuspendLayout();
		this.gb_rec_options.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.num_resp_time).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.num_fps).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.num_boost).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.num_speedhack).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tb_speed).BeginInit();
		this.tp_play.SuspendLayout();
		this.tc_play.SuspendLayout();
		this.tp_play_normal.SuspendLayout();
		this.pnl_edit.SuspendLayout();
		this.gb_edit.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tb_edit_release).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tb_edit_press).BeginInit();
		this.gb_clicks_play.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.tb_distance).BeginInit();
		this.tp_play_sequence.SuspendLayout();
		this.tp_macros.SuspendLayout();
		this.gb_local.SuspendLayout();
		this.gb_upload.SuspendLayout();
		this.tp_online.SuspendLayout();
		this.tp_settings.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.num_fps_settings).BeginInit();
		this.tp_help.SuspendLayout();
		this.gb_license.SuspendLayout();
		this.statusStrip1.SuspendLayout();
		this.gb_convert.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pb_logo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pb_hidetab).BeginInit();
		base.SuspendLayout();
		this.tc_main.Controls.Add(this.tp_space);
		this.tc_main.Controls.Add(this.tp_rec);
		this.tc_main.Controls.Add(this.tp_play);
		this.tc_main.Controls.Add(this.tp_macros);
		this.tc_main.Controls.Add(this.tp_online);
		this.tc_main.Controls.Add(this.tp_settings);
		this.tc_main.Controls.Add(this.tp_help);
		this.tc_main.ItemSize = new System.Drawing.Size(47, 18);
		this.tc_main.Location = new System.Drawing.Point(-5, 21);
		this.tc_main.Multiline = true;
		this.tc_main.Name = "tc_main";
		this.tc_main.SelectedIndex = 0;
		this.tc_main.Size = new System.Drawing.Size(556, 396);
		this.tc_main.TabIndex = 2;
		this.tc_main.Selected += new System.Windows.Forms.TabControlEventHandler(tc_main_Selected);
		this.tp_space.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_space.Location = new System.Drawing.Point(4, 22);
		this.tp_space.Name = "tp_space";
		this.tp_space.Padding = new System.Windows.Forms.Padding(3);
		this.tp_space.Size = new System.Drawing.Size(548, 370);
		this.tp_space.TabIndex = 7;
		this.tp_space.Text = ".____________________________________________";
		this.tp_rec.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_rec.Controls.Add(this.lbl_speedhack);
		this.tp_rec.Controls.Add(this.cb_sh_eject);
		this.tp_rec.Controls.Add(this.gb_rec_options);
		this.tp_rec.Controls.Add(this.cb_lagSpikes);
		this.tp_rec.Controls.Add(this.lbl_practice_msg);
		this.tp_rec.Controls.Add(this.lbl_smart_framesnote);
		this.tp_rec.Controls.Add(this.num_speedhack);
		this.tp_rec.Controls.Add(this.lbl_smartmsg);
		this.tp_rec.Controls.Add(this.label40);
		this.tp_rec.Controls.Add(this.txt_record);
		this.tp_rec.Controls.Add(this.btn_record);
		this.tp_rec.Controls.Add(this.label39);
		this.tp_rec.Controls.Add(this.label11);
		this.tp_rec.Controls.Add(this.label9);
		this.tp_rec.Controls.Add(this.tb_speed);
		this.tp_rec.Controls.Add(this.lbl_path_rec);
		this.tp_rec.Location = new System.Drawing.Point(4, 22);
		this.tp_rec.Name = "tp_rec";
		this.tp_rec.Padding = new System.Windows.Forms.Padding(3);
		this.tp_rec.Size = new System.Drawing.Size(548, 370);
		this.tp_rec.TabIndex = 0;
		this.tp_rec.Text = "Record";
		this.lbl_speedhack.AutoSize = true;
		this.lbl_speedhack.Location = new System.Drawing.Point(268, 22);
		this.lbl_speedhack.Name = "lbl_speedhack";
		this.lbl_speedhack.Size = new System.Drawing.Size(65, 13);
		this.lbl_speedhack.TabIndex = 7;
		this.lbl_speedhack.Text = "Speedhack:";
		this.cb_sh_eject.AutoSize = true;
		this.cb_sh_eject.Checked = true;
		this.cb_sh_eject.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cb_sh_eject.Location = new System.Drawing.Point(207, 343);
		this.cb_sh_eject.Name = "cb_sh_eject";
		this.cb_sh_eject.Size = new System.Drawing.Size(15, 14);
		this.cb_sh_eject.TabIndex = 9;
		this.cb_sh_eject.UseVisualStyleBackColor = true;
		this.cb_sh_eject.Visible = false;
		this.cb_sh_eject.CheckedChanged += new System.EventHandler(cb_sh_eject_CheckedChanged);
		this.gb_rec_options.Controls.Add(this.cb_death_effect);
		this.gb_rec_options.Controls.Add(this.cb_prac_music);
		this.gb_rec_options.Controls.Add(this.cb_auto_reset);
		this.gb_rec_options.Controls.Add(this.lbl_boostspeed);
		this.gb_rec_options.Controls.Add(this.lbl_resp);
		this.gb_rec_options.Controls.Add(this.label10);
		this.gb_rec_options.Controls.Add(this.label19);
		this.gb_rec_options.Controls.Add(this.lbl_death_effect);
		this.gb_rec_options.Controls.Add(this.lbl_prac_music);
		this.gb_rec_options.Controls.Add(this.label34);
		this.gb_rec_options.Controls.Add(this.num_resp_time);
		this.gb_rec_options.Controls.Add(this.lbl_practice_rec);
		this.gb_rec_options.Controls.Add(this.num_fps);
		this.gb_rec_options.Controls.Add(this.cb_practice_rec);
		this.gb_rec_options.Controls.Add(this.num_boost);
		this.gb_rec_options.Controls.Add(this.cb_smart_rec);
		this.gb_rec_options.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_rec_options.Location = new System.Drawing.Point(18, 42);
		this.gb_rec_options.Name = "gb_rec_options";
		this.gb_rec_options.Size = new System.Drawing.Size(518, 135);
		this.gb_rec_options.TabIndex = 6;
		this.gb_rec_options.TabStop = false;
		this.gb_rec_options.Text = "Options";
		this.cb_death_effect.AutoSize = true;
		this.cb_death_effect.Location = new System.Drawing.Point(487, 79);
		this.cb_death_effect.Name = "cb_death_effect";
		this.cb_death_effect.Size = new System.Drawing.Size(15, 14);
		this.cb_death_effect.TabIndex = 9;
		this.cb_death_effect.UseVisualStyleBackColor = true;
		this.cb_death_effect.CheckedChanged += new System.EventHandler(cb_death_effect_CheckedChanged);
		this.cb_prac_music.AutoSize = true;
		this.cb_prac_music.Location = new System.Drawing.Point(487, 51);
		this.cb_prac_music.Name = "cb_prac_music";
		this.cb_prac_music.Size = new System.Drawing.Size(15, 14);
		this.cb_prac_music.TabIndex = 9;
		this.cb_prac_music.UseVisualStyleBackColor = true;
		this.cb_prac_music.CheckedChanged += new System.EventHandler(cb_prac_music_CheckedChanged);
		this.cb_auto_reset.AutoSize = true;
		this.cb_auto_reset.Location = new System.Drawing.Point(487, 22);
		this.cb_auto_reset.Name = "cb_auto_reset";
		this.cb_auto_reset.Size = new System.Drawing.Size(15, 14);
		this.cb_auto_reset.TabIndex = 9;
		this.cb_auto_reset.UseVisualStyleBackColor = true;
		this.lbl_boostspeed.AutoSize = true;
		this.lbl_boostspeed.Location = new System.Drawing.Point(35, 73);
		this.lbl_boostspeed.Name = "lbl_boostspeed";
		this.lbl_boostspeed.Size = new System.Drawing.Size(77, 13);
		this.lbl_boostspeed.TabIndex = 5;
		this.lbl_boostspeed.Text = "-> boostspeed:";
		this.lbl_resp.AutoSize = true;
		this.lbl_resp.Location = new System.Drawing.Point(295, 106);
		this.lbl_resp.Name = "lbl_resp";
		this.lbl_resp.Size = new System.Drawing.Size(81, 13);
		this.lbl_resp.TabIndex = 2;
		this.lbl_resp.Text = "Respawn Time:";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(15, 27);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(57, 13);
		this.label10.TabIndex = 2;
		this.label10.Text = "Framerate:";
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(15, 51);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(97, 13);
		this.label19.TabIndex = 2;
		this.label19.Text = "Use Smart-Record:";
		this.lbl_death_effect.AutoSize = true;
		this.lbl_death_effect.Location = new System.Drawing.Point(295, 79);
		this.lbl_death_effect.Name = "lbl_death_effect";
		this.lbl_death_effect.Size = new System.Drawing.Size(113, 13);
		this.lbl_death_effect.TabIndex = 2;
		this.lbl_death_effect.Text = "Remove Death Effect:";
		this.lbl_prac_music.AutoSize = true;
		this.lbl_prac_music.Location = new System.Drawing.Point(295, 51);
		this.lbl_prac_music.Name = "lbl_prac_music";
		this.lbl_prac_music.Size = new System.Drawing.Size(109, 13);
		this.lbl_prac_music.TabIndex = 2;
		this.lbl_prac_music.Text = "Practice Music Hack:";
		this.label34.AutoSize = true;
		this.label34.Location = new System.Drawing.Point(295, 22);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(119, 13);
		this.label34.TabIndex = 2;
		this.label34.Text = "Auto-Reset after Death:";
		this.num_resp_time.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.num_resp_time.DecimalPlaces = 1;
		this.num_resp_time.ForeColor = System.Drawing.Color.Gainsboro;
		this.num_resp_time.Increment = new decimal(new int[4] { 2, 0, 0, 65536 });
		this.num_resp_time.Location = new System.Drawing.Point(449, 99);
		this.num_resp_time.Maximum = new decimal(new int[4] { 2, 0, 0, 0 });
		this.num_resp_time.Name = "num_resp_time";
		this.num_resp_time.Size = new System.Drawing.Size(51, 20);
		this.num_resp_time.TabIndex = 3;
		this.num_resp_time.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.num_resp_time.ValueChanged += new System.EventHandler(num_resp_time_ValueChanged);
		this.lbl_practice_rec.AutoSize = true;
		this.lbl_practice_rec.Location = new System.Drawing.Point(15, 102);
		this.lbl_practice_rec.Name = "lbl_practice_rec";
		this.lbl_practice_rec.Size = new System.Drawing.Size(109, 13);
		this.lbl_practice_rec.TabIndex = 2;
		this.lbl_practice_rec.Text = "Use Practice-Record:";
		this.num_fps.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.num_fps.ForeColor = System.Drawing.Color.Gainsboro;
		this.num_fps.Location = new System.Drawing.Point(169, 24);
		this.num_fps.Maximum = new decimal(new int[4] { 1024, 0, 0, 0 });
		this.num_fps.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.num_fps.Name = "num_fps";
		this.num_fps.Size = new System.Drawing.Size(51, 20);
		this.num_fps.TabIndex = 3;
		this.num_fps.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.cb_practice_rec.AutoSize = true;
		this.cb_practice_rec.Location = new System.Drawing.Point(207, 102);
		this.cb_practice_rec.Name = "cb_practice_rec";
		this.cb_practice_rec.Size = new System.Drawing.Size(15, 14);
		this.cb_practice_rec.TabIndex = 4;
		this.cb_practice_rec.UseVisualStyleBackColor = true;
		this.cb_practice_rec.CheckedChanged += new System.EventHandler(cb_practice_rec_CheckedChanged);
		this.num_boost.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.num_boost.DecimalPlaces = 2;
		this.num_boost.Enabled = false;
		this.num_boost.ForeColor = System.Drawing.Color.Gainsboro;
		this.num_boost.Increment = new decimal(new int[4] { 5, 0, 0, 131072 });
		this.num_boost.Location = new System.Drawing.Point(169, 72);
		this.num_boost.Maximum = new decimal(new int[4] { 5, 0, 0, 0 });
		this.num_boost.Minimum = new decimal(new int[4] { 1, 0, 0, 131072 });
		this.num_boost.Name = "num_boost";
		this.num_boost.Size = new System.Drawing.Size(51, 20);
		this.num_boost.TabIndex = 3;
		this.num_boost.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.num_boost.ValueChanged += new System.EventHandler(num_speedhack_ValueChanged);
		this.cb_smart_rec.AutoSize = true;
		this.cb_smart_rec.Location = new System.Drawing.Point(207, 51);
		this.cb_smart_rec.Name = "cb_smart_rec";
		this.cb_smart_rec.Size = new System.Drawing.Size(15, 14);
		this.cb_smart_rec.TabIndex = 4;
		this.cb_smart_rec.UseVisualStyleBackColor = true;
		this.cb_smart_rec.CheckedChanged += new System.EventHandler(cb_smart_rec_CheckedChanged);
		this.cb_lagSpikes.AutoSize = true;
		this.cb_lagSpikes.Location = new System.Drawing.Point(207, 325);
		this.cb_lagSpikes.Name = "cb_lagSpikes";
		this.cb_lagSpikes.Size = new System.Drawing.Size(15, 14);
		this.cb_lagSpikes.TabIndex = 9;
		this.cb_lagSpikes.UseVisualStyleBackColor = true;
		this.cb_lagSpikes.Visible = false;
		this.cb_lagSpikes.CheckedChanged += new System.EventHandler(cb_lagSpikes_CheckedChanged);
		this.lbl_practice_msg.AutoSize = true;
		this.lbl_practice_msg.Location = new System.Drawing.Point(23, 236);
		this.lbl_practice_msg.Name = "lbl_practice_msg";
		this.lbl_practice_msg.Size = new System.Drawing.Size(372, 26);
		this.lbl_practice_msg.TabIndex = 5;
		this.lbl_practice_msg.Text = "Practice Record doesn't support Dual Recording!\r\nPlace your checkpoints on safe spots to reduce risk of messing up the macro!";
		this.lbl_practice_msg.Visible = false;
		this.lbl_smart_framesnote.AutoSize = true;
		this.lbl_smart_framesnote.Location = new System.Drawing.Point(23, 236);
		this.lbl_smart_framesnote.Name = "lbl_smart_framesnote";
		this.lbl_smart_framesnote.Size = new System.Drawing.Size(329, 26);
		this.lbl_smart_framesnote.TabIndex = 5;
		this.lbl_smart_framesnote.Text = "Smart Record and Practice Record isn't availible for frame recording!\r\nYou can disable frame recording in the settings!";
		this.lbl_smart_framesnote.Visible = false;
		this.num_speedhack.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.num_speedhack.DecimalPlaces = 2;
		this.num_speedhack.ForeColor = System.Drawing.Color.Gainsboro;
		this.num_speedhack.Increment = new decimal(new int[4] { 5, 0, 0, 131072 });
		this.num_speedhack.Location = new System.Drawing.Point(352, 16);
		this.num_speedhack.Maximum = new decimal(new int[4] { 5, 0, 0, 0 });
		this.num_speedhack.Minimum = new decimal(new int[4] { 1, 0, 0, 131072 });
		this.num_speedhack.Name = "num_speedhack";
		this.num_speedhack.Size = new System.Drawing.Size(84, 20);
		this.num_speedhack.TabIndex = 3;
		this.num_speedhack.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.num_speedhack.ValueChanged += new System.EventHandler(num_speedhack_ValueChanged);
		this.lbl_smartmsg.AutoSize = true;
		this.lbl_smartmsg.Location = new System.Drawing.Point(23, 236);
		this.lbl_smartmsg.Name = "lbl_smartmsg";
		this.lbl_smartmsg.Size = new System.Drawing.Size(342, 26);
		this.lbl_smartmsg.TabIndex = 5;
		this.lbl_smartmsg.Text = "Smart Record doesnt support the keyboard! (Including Dual Recording)\r\nYou can use your keyboard for e.g. pause and resume the game!";
		this.lbl_smartmsg.Visible = false;
		this.label40.AutoSize = true;
		this.label40.Location = new System.Drawing.Point(15, 343);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(92, 13);
		this.label40.TabIndex = 2;
		this.label40.Text = "Eject Speedhack:";
		this.label40.Visible = false;
		this.txt_record.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_record.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_record.Location = new System.Drawing.Point(59, 16);
		this.txt_record.Name = "txt_record";
		this.txt_record.Size = new System.Drawing.Size(196, 20);
		this.txt_record.TabIndex = 0;
		this.txt_record.TextChanged += new System.EventHandler(txt_record_TextChanged);
		this.btn_record.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_record.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_record.Location = new System.Drawing.Point(417, 239);
		this.btn_record.Name = "btn_record";
		this.btn_record.Size = new System.Drawing.Size(119, 23);
		this.btn_record.TabIndex = 1;
		this.btn_record.Text = " ⚫    record";
		this.btn_record.UseVisualStyleBackColor = true;
		this.btn_record.Click += new System.EventHandler(btn_record_Click);
		this.label39.AutoSize = true;
		this.label39.Location = new System.Drawing.Point(15, 325);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(104, 13);
		this.label39.TabIndex = 2;
		this.label39.Text = "Reduce Lag Spikes:";
		this.label39.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.label11.Location = new System.Drawing.Point(23, 188);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(37, 13);
		this.label11.TabIndex = 2;
		this.label11.Text = "Path:";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(15, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(38, 13);
		this.label9.TabIndex = 2;
		this.label9.Text = "Name:";
		this.tb_speed.LargeChange = 1;
		this.tb_speed.Location = new System.Drawing.Point(438, 14);
		this.tb_speed.Maximum = 4;
		this.tb_speed.Minimum = -4;
		this.tb_speed.Name = "tb_speed";
		this.tb_speed.Size = new System.Drawing.Size(104, 45);
		this.tb_speed.TabIndex = 8;
		this.tb_speed.MouseUp += new System.Windows.Forms.MouseEventHandler(tb_speed_MouseUp);
		this.lbl_path_rec.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.lbl_path_rec.Location = new System.Drawing.Point(66, 187);
		this.lbl_path_rec.Name = "lbl_path_rec";
		this.lbl_path_rec.Size = new System.Drawing.Size(470, 46);
		this.lbl_path_rec.TabIndex = 2;
		this.lbl_path_rec.Text = "n/a";
		this.tp_play.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_play.Controls.Add(this.tc_play);
		this.tp_play.Location = new System.Drawing.Point(4, 22);
		this.tp_play.Name = "tp_play";
		this.tp_play.Padding = new System.Windows.Forms.Padding(3);
		this.tp_play.Size = new System.Drawing.Size(548, 370);
		this.tp_play.TabIndex = 1;
		this.tp_play.Text = "Play";
		this.tc_play.Controls.Add(this.tp_play_normal);
		this.tc_play.Controls.Add(this.tp_play_sequence);
		this.tc_play.Location = new System.Drawing.Point(-3, 6);
		this.tc_play.Name = "tc_play";
		this.tc_play.SelectedIndex = 0;
		this.tc_play.Size = new System.Drawing.Size(563, 335);
		this.tc_play.TabIndex = 1;
		this.tc_play.Selected += new System.Windows.Forms.TabControlEventHandler(tabControl3_Selected);
		this.tp_play_normal.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_play_normal.Controls.Add(this.pnl_edit);
		this.tp_play_normal.Controls.Add(this.cb_play_macro);
		this.tp_play_normal.Controls.Add(this.gb_clicks_play);
		this.tp_play_normal.Controls.Add(this.lbl_macro_size_play);
		this.tp_play_normal.Controls.Add(this.label8);
		this.tp_play_normal.Controls.Add(this.btn_edit);
		this.tp_play_normal.Controls.Add(this.btn_play);
		this.tp_play_normal.Controls.Add(this.label4);
		this.tp_play_normal.Location = new System.Drawing.Point(4, 22);
		this.tp_play_normal.Name = "tp_play_normal";
		this.tp_play_normal.Padding = new System.Windows.Forms.Padding(3);
		this.tp_play_normal.Size = new System.Drawing.Size(555, 309);
		this.tp_play_normal.TabIndex = 0;
		this.tp_play_normal.Text = "Normal";
		this.pnl_edit.Controls.Add(this.label37);
		this.pnl_edit.Controls.Add(this.label36);
		this.pnl_edit.Controls.Add(this.btn_edit_save);
		this.pnl_edit.Controls.Add(this.btn_edit_merge);
		this.pnl_edit.Controls.Add(this.btn_edit_sel_next);
		this.pnl_edit.Controls.Add(this.lbl_edit_cur_coord);
		this.pnl_edit.Controls.Add(this.gb_edit);
		this.pnl_edit.Controls.Add(this.lv_edit_coords);
		this.pnl_edit.Controls.Add(this.label16);
		this.pnl_edit.Controls.Add(this.btn_close_edit);
		this.pnl_edit.Location = new System.Drawing.Point(2, 3);
		this.pnl_edit.Name = "pnl_edit";
		this.pnl_edit.Size = new System.Drawing.Size(542, 247);
		this.pnl_edit.TabIndex = 31;
		this.pnl_edit.Visible = false;
		this.label37.Location = new System.Drawing.Point(124, 3);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(127, 23);
		this.label37.TabIndex = 39;
		this.label37.Text = "Coordinate";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label36.Location = new System.Drawing.Point(5, 3);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(118, 23);
		this.label36.TabIndex = 39;
		this.label36.Text = "Type";
		this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btn_edit_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_edit_save.Location = new System.Drawing.Point(445, 218);
		this.btn_edit_save.Name = "btn_edit_save";
		this.btn_edit_save.Size = new System.Drawing.Size(75, 23);
		this.btn_edit_save.TabIndex = 8;
		this.btn_edit_save.Text = "Save Macro";
		this.btn_edit_save.UseVisualStyleBackColor = true;
		this.btn_edit_save.Click += new System.EventHandler(btn_edit_save_Click);
		this.btn_edit_merge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_edit_merge.Location = new System.Drawing.Point(274, 218);
		this.btn_edit_merge.Name = "btn_edit_merge";
		this.btn_edit_merge.Size = new System.Drawing.Size(75, 23);
		this.btn_edit_merge.TabIndex = 8;
		this.btn_edit_merge.Text = "Merge File";
		this.btn_edit_merge.UseVisualStyleBackColor = true;
		this.btn_edit_merge.Click += new System.EventHandler(btn_edit_merge_Click);
		this.btn_edit_sel_next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_edit_sel_next.Location = new System.Drawing.Point(193, 218);
		this.btn_edit_sel_next.Name = "btn_edit_sel_next";
		this.btn_edit_sel_next.Size = new System.Drawing.Size(75, 23);
		this.btn_edit_sel_next.TabIndex = 8;
		this.btn_edit_sel_next.Text = "Select Next";
		this.btn_edit_sel_next.UseVisualStyleBackColor = true;
		this.btn_edit_sel_next.Click += new System.EventHandler(btn_edit_sel_next_Click);
		this.lbl_edit_cur_coord.Location = new System.Drawing.Point(101, 223);
		this.lbl_edit_cur_coord.Name = "lbl_edit_cur_coord";
		this.lbl_edit_cur_coord.Size = new System.Drawing.Size(90, 13);
		this.lbl_edit_cur_coord.TabIndex = 8;
		this.lbl_edit_cur_coord.Text = "n/a";
		this.lbl_edit_cur_coord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.gb_edit.Controls.Add(this.btn_edit_delete);
		this.gb_edit.Controls.Add(this.btn_edit_save_click);
		this.gb_edit.Controls.Add(this.lbl_edit_coord_release);
		this.gb_edit.Controls.Add(this.lbl_edit_coord_press);
		this.gb_edit.Controls.Add(this.label29);
		this.gb_edit.Controls.Add(this.label15);
		this.gb_edit.Controls.Add(this.label35);
		this.gb_edit.Controls.Add(this.label31);
		this.gb_edit.Controls.Add(this.label33);
		this.gb_edit.Controls.Add(this.pictureBox2);
		this.gb_edit.Controls.Add(this.tb_edit_release);
		this.gb_edit.Controls.Add(this.pictureBox1);
		this.gb_edit.Controls.Add(this.tb_edit_press);
		this.gb_edit.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_edit.Location = new System.Drawing.Point(274, 22);
		this.gb_edit.Name = "gb_edit";
		this.gb_edit.Size = new System.Drawing.Size(259, 190);
		this.gb_edit.TabIndex = 7;
		this.gb_edit.TabStop = false;
		this.gb_edit.Text = "Edit";
		this.btn_edit_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_edit_delete.ForeColor = System.Drawing.Color.FromArgb(255, 128, 128);
		this.btn_edit_delete.Location = new System.Drawing.Point(171, 131);
		this.btn_edit_delete.Name = "btn_edit_delete";
		this.btn_edit_delete.Size = new System.Drawing.Size(75, 23);
		this.btn_edit_delete.TabIndex = 10;
		this.btn_edit_delete.Text = "Delete Click";
		this.btn_edit_delete.UseVisualStyleBackColor = true;
		this.btn_edit_delete.Click += new System.EventHandler(btn_edit_delete_Click);
		this.btn_edit_save_click.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_edit_save_click.Location = new System.Drawing.Point(171, 157);
		this.btn_edit_save_click.Name = "btn_edit_save_click";
		this.btn_edit_save_click.Size = new System.Drawing.Size(75, 23);
		this.btn_edit_save_click.TabIndex = 9;
		this.btn_edit_save_click.Text = "Save Click";
		this.btn_edit_save_click.UseVisualStyleBackColor = true;
		this.btn_edit_save_click.Click += new System.EventHandler(btn_edit_save_click_Click);
		this.lbl_edit_coord_release.Location = new System.Drawing.Point(159, 42);
		this.lbl_edit_coord_release.Name = "lbl_edit_coord_release";
		this.lbl_edit_coord_release.Size = new System.Drawing.Size(90, 13);
		this.lbl_edit_coord_release.TabIndex = 8;
		this.lbl_edit_coord_release.Text = "n/a";
		this.lbl_edit_coord_release.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbl_edit_coord_press.Location = new System.Drawing.Point(159, 26);
		this.lbl_edit_coord_press.Name = "lbl_edit_coord_press";
		this.lbl_edit_coord_press.Size = new System.Drawing.Size(90, 13);
		this.lbl_edit_coord_press.TabIndex = 8;
		this.lbl_edit_coord_press.Text = "n/a";
		this.lbl_edit_coord_press.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label29.AutoSize = true;
		this.label29.Location = new System.Drawing.Point(83, 81);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(49, 13);
		this.label29.TabIndex = 2;
		this.label29.Text = "Release:";
		this.label15.AutoSize = true;
		this.label15.Location = new System.Drawing.Point(10, 81);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(36, 13);
		this.label15.TabIndex = 2;
		this.label15.Text = "Press:";
		this.label35.AutoSize = true;
		this.label35.Location = new System.Drawing.Point(104, 42);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(49, 13);
		this.label35.TabIndex = 8;
		this.label35.Text = "Release:";
		this.label31.AutoSize = true;
		this.label31.Location = new System.Drawing.Point(117, 26);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(36, 13);
		this.label31.TabIndex = 8;
		this.label31.Text = "Press:";
		this.label33.AutoSize = true;
		this.label33.Location = new System.Drawing.Point(10, 26);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(70, 13);
		this.label33.TabIndex = 8;
		this.label33.Text = "Current Click:";
		this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(23, 70, 255);
		this.pictureBox2.BackgroundImage = (System.Drawing.Image)resources.GetObject("pictureBox2.BackgroundImage");
		this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox2.Location = new System.Drawing.Point(86, 130);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(44, 44);
		this.pictureBox2.TabIndex = 0;
		this.pictureBox2.TabStop = false;
		this.tb_edit_release.Location = new System.Drawing.Point(74, 99);
		this.tb_edit_release.Maximum = 150;
		this.tb_edit_release.Minimum = -150;
		this.tb_edit_release.Name = "tb_edit_release";
		this.tb_edit_release.Size = new System.Drawing.Size(68, 45);
		this.tb_edit_release.TabIndex = 1;
		this.tb_edit_release.TickStyle = System.Windows.Forms.TickStyle.None;
		this.tb_edit_release.Scroll += new System.EventHandler(tb_edit_release_Scroll);
		this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(23, 70, 255);
		this.pictureBox1.BackgroundImage = (System.Drawing.Image)resources.GetObject("pictureBox1.BackgroundImage");
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.Location = new System.Drawing.Point(13, 130);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(44, 44);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.tb_edit_press.Location = new System.Drawing.Point(1, 99);
		this.tb_edit_press.Maximum = 150;
		this.tb_edit_press.Minimum = -150;
		this.tb_edit_press.Name = "tb_edit_press";
		this.tb_edit_press.Size = new System.Drawing.Size(68, 45);
		this.tb_edit_press.TabIndex = 1;
		this.tb_edit_press.TickStyle = System.Windows.Forms.TickStyle.None;
		this.tb_edit_press.Scroll += new System.EventHandler(tb_edit_press_Scroll);
		this.lv_edit_coords.BackColor = System.Drawing.Color.FromArgb(42, 42, 42);
		this.lv_edit_coords.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lv_edit_coords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.ch_edit_type, this.ch_edit_coord });
		this.lv_edit_coords.ForeColor = System.Drawing.Color.Gainsboro;
		this.lv_edit_coords.FullRowSelect = true;
		this.lv_edit_coords.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lv_edit_coords.HideSelection = false;
		this.lv_edit_coords.Location = new System.Drawing.Point(4, 3);
		this.lv_edit_coords.Name = "lv_edit_coords";
		this.lv_edit_coords.Size = new System.Drawing.Size(264, 209);
		this.lv_edit_coords.TabIndex = 6;
		this.lv_edit_coords.UseCompatibleStateImageBehavior = false;
		this.lv_edit_coords.View = System.Windows.Forms.View.Details;
		this.lv_edit_coords.SelectedIndexChanged += new System.EventHandler(lv_edit_coords_SelectedIndexChanged);
		this.ch_edit_type.Text = "Type";
		this.ch_edit_type.Width = 121;
		this.ch_edit_coord.Text = "Coordinate";
		this.ch_edit_coord.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.ch_edit_coord.Width = 124;
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(1, 223);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(98, 13);
		this.label16.TabIndex = 8;
		this.label16.Text = "Current Coordinate:";
		this.btn_close_edit.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_edit.BackgroundImage");
		this.btn_close_edit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_edit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_edit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_edit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_edit.ForeColor = System.Drawing.Color.Red;
		this.btn_close_edit.Location = new System.Drawing.Point(525, 0);
		this.btn_close_edit.Name = "btn_close_edit";
		this.btn_close_edit.Size = new System.Drawing.Size(17, 17);
		this.btn_close_edit.TabIndex = 5;
		this.btn_close_edit.UseVisualStyleBackColor = true;
		this.btn_close_edit.Click += new System.EventHandler(btn_close_edit_Click);
		this.btn_close_edit.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close_edit.MouseLeave += new System.EventHandler(btn_close_MouseLeave);
		this.cb_play_macro.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.cb_play_macro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cb_play_macro.ForeColor = System.Drawing.Color.Gainsboro;
		this.cb_play_macro.FormattingEnabled = true;
		this.cb_play_macro.Location = new System.Drawing.Point(60, 13);
		this.cb_play_macro.Name = "cb_play_macro";
		this.cb_play_macro.Size = new System.Drawing.Size(180, 21);
		this.cb_play_macro.TabIndex = 30;
		this.cb_play_macro.SelectedIndexChanged += new System.EventHandler(cb_play_macro_SelectedIndexChanged);
		this.gb_clicks_play.Controls.Add(this.lbl_clicks_click);
		this.gb_clicks_play.Controls.Add(this.lbl_clicks_type);
		this.gb_clicks_play.Controls.Add(this.lbl_clicks_player);
		this.gb_clicks_play.Controls.Add(this.lbl_clicks_coords);
		this.gb_clicks_play.Controls.Add(this.pgb_state);
		this.gb_clicks_play.Controls.Add(this.label24);
		this.gb_clicks_play.Controls.Add(this.label22);
		this.gb_clicks_play.Controls.Add(this.lbl_analyse);
		this.gb_clicks_play.Controls.Add(this.label23);
		this.gb_clicks_play.Controls.Add(this.cb_keys);
		this.gb_clicks_play.Controls.Add(this.btn_execute);
		this.gb_clicks_play.Controls.Add(this.lv_clicks);
		this.gb_clicks_play.Controls.Add(this.tb_distance);
		this.gb_clicks_play.Controls.Add(this.btn_laod_sounds);
		this.gb_clicks_play.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_clicks_play.Location = new System.Drawing.Point(12, 51);
		this.gb_clicks_play.Name = "gb_clicks_play";
		this.gb_clicks_play.Size = new System.Drawing.Size(516, 188);
		this.gb_clicks_play.TabIndex = 29;
		this.gb_clicks_play.TabStop = false;
		this.gb_clicks_play.Text = "Click Sounds";
		this.lbl_clicks_click.Location = new System.Drawing.Point(189, 21);
		this.lbl_clicks_click.Name = "lbl_clicks_click";
		this.lbl_clicks_click.Size = new System.Drawing.Size(85, 23);
		this.lbl_clicks_click.TabIndex = 38;
		this.lbl_clicks_click.Text = "Click";
		this.lbl_clicks_click.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_clicks_type.Location = new System.Drawing.Point(121, 21);
		this.lbl_clicks_type.Name = "lbl_clicks_type";
		this.lbl_clicks_type.Size = new System.Drawing.Size(67, 23);
		this.lbl_clicks_type.TabIndex = 38;
		this.lbl_clicks_type.Text = "Type";
		this.lbl_clicks_type.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_clicks_player.Location = new System.Drawing.Point(79, 21);
		this.lbl_clicks_player.Name = "lbl_clicks_player";
		this.lbl_clicks_player.Size = new System.Drawing.Size(41, 23);
		this.lbl_clicks_player.TabIndex = 38;
		this.lbl_clicks_player.Text = "Player";
		this.lbl_clicks_player.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_clicks_coords.Location = new System.Drawing.Point(10, 21);
		this.lbl_clicks_coords.Name = "lbl_clicks_coords";
		this.lbl_clicks_coords.Size = new System.Drawing.Size(68, 23);
		this.lbl_clicks_coords.TabIndex = 38;
		this.lbl_clicks_coords.Text = "Coordinates";
		this.lbl_clicks_coords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pgb_state.Location = new System.Drawing.Point(284, 160);
		this.pgb_state.Name = "pgb_state";
		this.pgb_state.Size = new System.Drawing.Size(100, 18);
		this.pgb_state.TabIndex = 37;
		this.pgb_state.Visible = false;
		this.label24.AutoSize = true;
		this.label24.Location = new System.Drawing.Point(281, 55);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(46, 13);
		this.label24.TabIndex = 36;
		this.label24.Text = "Sounds:";
		this.label22.AutoSize = true;
		this.label22.Location = new System.Drawing.Point(306, 86);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(86, 13);
		this.label22.TabIndex = 34;
		this.label22.Text = "more softpresses";
		this.lbl_analyse.AutoSize = true;
		this.lbl_analyse.Location = new System.Drawing.Point(389, 162);
		this.lbl_analyse.Name = "lbl_analyse";
		this.lbl_analyse.Size = new System.Drawing.Size(86, 13);
		this.lbl_analyse.TabIndex = 35;
		this.lbl_analyse.Text = "interpret macro...";
		this.lbl_analyse.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Location = new System.Drawing.Point(306, 133);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(90, 13);
		this.label23.TabIndex = 35;
		this.label23.Text = "more hardpresses";
		this.cb_keys.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.cb_keys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cb_keys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cb_keys.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cb_keys.ForeColor = System.Drawing.Color.Gainsboro;
		this.cb_keys.FormattingEnabled = true;
		this.cb_keys.Items.AddRange(new object[6] { "P1: Mouse | P2: ArrowUp", "P1: Mouse | P2: Space", "P1: Space | P2: ArrowUp", "P1: Space | P2: Mouse", "P1: ArrowUp | P2: Mouse", "P1: ArrowUp | P2: Space" });
		this.cb_keys.Location = new System.Drawing.Point(333, 53);
		this.cb_keys.Name = "cb_keys";
		this.cb_keys.Size = new System.Drawing.Size(164, 21);
		this.cb_keys.TabIndex = 33;
		this.cb_keys.SelectedIndexChanged += new System.EventHandler(cb_keys_SelectedIndexChanged);
		this.btn_execute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_execute.Location = new System.Drawing.Point(422, 123);
		this.btn_execute.Name = "btn_execute";
		this.btn_execute.Size = new System.Drawing.Size(75, 23);
		this.btn_execute.TabIndex = 31;
		this.btn_execute.Text = "▶    Start";
		this.btn_execute.UseVisualStyleBackColor = true;
		this.btn_execute.Click += new System.EventHandler(btn_execute_Click);
		this.lv_clicks.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
		this.lv_clicks.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lv_clicks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.ch_coord, this.ch_player, this.columnHeader1, this.ch_clicktype });
		this.lv_clicks.ForeColor = System.Drawing.Color.Gainsboro;
		this.lv_clicks.FullRowSelect = true;
		this.lv_clicks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lv_clicks.HideSelection = false;
		this.lv_clicks.Location = new System.Drawing.Point(9, 21);
		this.lv_clicks.Name = "lv_clicks";
		this.lv_clicks.Size = new System.Drawing.Size(266, 157);
		this.lv_clicks.TabIndex = 30;
		this.lv_clicks.UseCompatibleStateImageBehavior = false;
		this.lv_clicks.View = System.Windows.Forms.View.Details;
		this.ch_coord.Text = "Coordinate";
		this.ch_coord.Width = 68;
		this.ch_player.Text = "Player";
		this.ch_player.Width = 43;
		this.columnHeader1.Text = "MouseState";
		this.columnHeader1.Width = 69;
		this.ch_clicktype.Text = "Click";
		this.ch_clicktype.Width = 66;
		this.tb_distance.LargeChange = 30;
		this.tb_distance.Location = new System.Drawing.Point(280, 80);
		this.tb_distance.Maximum = 100;
		this.tb_distance.Minimum = 1;
		this.tb_distance.Name = "tb_distance";
		this.tb_distance.Orientation = System.Windows.Forms.Orientation.Vertical;
		this.tb_distance.Size = new System.Drawing.Size(45, 74);
		this.tb_distance.TabIndex = 32;
		this.tb_distance.TickStyle = System.Windows.Forms.TickStyle.None;
		this.tb_distance.Value = 50;
		this.tb_distance.MouseUp += new System.Windows.Forms.MouseEventHandler(tb_distance_MouseUp);
		this.btn_laod_sounds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_laod_sounds.Location = new System.Drawing.Point(284, 21);
		this.btn_laod_sounds.Name = "btn_laod_sounds";
		this.btn_laod_sounds.Size = new System.Drawing.Size(213, 23);
		this.btn_laod_sounds.TabIndex = 29;
		this.btn_laod_sounds.Text = "\ud83d\uddb1\ufe0f  Load Clicks";
		this.btn_laod_sounds.UseVisualStyleBackColor = true;
		this.btn_laod_sounds.Click += new System.EventHandler(btn_laod_sounds_Click);
		this.lbl_macro_size_play.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_macro_size_play.Location = new System.Drawing.Point(469, 17);
		this.lbl_macro_size_play.Name = "lbl_macro_size_play";
		this.lbl_macro_size_play.Size = new System.Drawing.Size(66, 13);
		this.lbl_macro_size_play.TabIndex = 6;
		this.lbl_macro_size_play.Text = "n/a";
		this.lbl_macro_size_play.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(431, 17);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(37, 13);
		this.label8.TabIndex = 6;
		this.label8.Text = "size:";
		this.btn_edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_edit.ForeColor = System.Drawing.Color.FromArgb(148, 184, 255);
		this.btn_edit.Location = new System.Drawing.Point(330, 11);
		this.btn_edit.Name = "btn_edit";
		this.btn_edit.Size = new System.Drawing.Size(78, 23);
		this.btn_edit.TabIndex = 5;
		this.btn_edit.Text = "\ud83d\udee0    Edit";
		this.btn_edit.UseVisualStyleBackColor = true;
		this.btn_edit.Click += new System.EventHandler(btn_edit_Click);
		this.btn_play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_play.Location = new System.Drawing.Point(246, 11);
		this.btn_play.Name = "btn_play";
		this.btn_play.Size = new System.Drawing.Size(78, 23);
		this.btn_play.TabIndex = 5;
		this.btn_play.Text = "▶    Play";
		this.btn_play.UseVisualStyleBackColor = true;
		this.btn_play.Click += new System.EventHandler(btn_play_Click);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(9, 17);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 13);
		this.label4.TabIndex = 3;
		this.label4.Text = "Macro: ";
		this.tp_play_sequence.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_play_sequence.Controls.Add(this.label28);
		this.tp_play_sequence.Controls.Add(this.btn_play_seq);
		this.tp_play_sequence.Controls.Add(this.cb_play_seq);
		this.tp_play_sequence.Controls.Add(this.label27);
		this.tp_play_sequence.Controls.Add(this.lb_play_seq);
		this.tp_play_sequence.Location = new System.Drawing.Point(4, 22);
		this.tp_play_sequence.Name = "tp_play_sequence";
		this.tp_play_sequence.Padding = new System.Windows.Forms.Padding(3);
		this.tp_play_sequence.Size = new System.Drawing.Size(555, 309);
		this.tp_play_sequence.TabIndex = 1;
		this.tp_play_sequence.Text = "Sequential Play";
		this.label28.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(12, 151);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(522, 95);
		this.label28.TabIndex = 9;
		this.label28.Text = "A sequence of macros must follow a speciffic name pattern:\r\n\r\nseq_[name]_[number]\r\n\r\nExample:\r\nseq_deadlocked_001\r\nseq_deadlocked_002...";
		this.btn_play_seq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_play_seq.Location = new System.Drawing.Point(150, 38);
		this.btn_play_seq.Name = "btn_play_seq";
		this.btn_play_seq.Size = new System.Drawing.Size(111, 23);
		this.btn_play_seq.TabIndex = 8;
		this.btn_play_seq.Text = "▶    Play";
		this.btn_play_seq.UseVisualStyleBackColor = true;
		this.btn_play_seq.Click += new System.EventHandler(btn_play_Click);
		this.cb_play_seq.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.cb_play_seq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cb_play_seq.ForeColor = System.Drawing.Color.Gainsboro;
		this.cb_play_seq.FormattingEnabled = true;
		this.cb_play_seq.Location = new System.Drawing.Point(74, 11);
		this.cb_play_seq.Name = "cb_play_seq";
		this.cb_play_seq.Size = new System.Drawing.Size(187, 21);
		this.cb_play_seq.TabIndex = 7;
		this.cb_play_seq.SelectedIndexChanged += new System.EventHandler(cb_play_seq_SelectedIndexChanged);
		this.label27.AutoSize = true;
		this.label27.Location = new System.Drawing.Point(6, 14);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(62, 13);
		this.label27.TabIndex = 6;
		this.label27.Text = "Sequence: ";
		this.lb_play_seq.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.lb_play_seq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lb_play_seq.ForeColor = System.Drawing.Color.Gainsboro;
		this.lb_play_seq.FormattingEnabled = true;
		this.lb_play_seq.Location = new System.Drawing.Point(267, 11);
		this.lb_play_seq.Name = "lb_play_seq";
		this.lb_play_seq.Size = new System.Drawing.Size(267, 119);
		this.lb_play_seq.TabIndex = 1;
		this.tp_macros.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_macros.Controls.Add(this.gb_local);
		this.tp_macros.Controls.Add(this.gb_upload);
		this.tp_macros.Controls.Add(this.lbl_size_macros);
		this.tp_macros.Controls.Add(this.label30);
		this.tp_macros.Controls.Add(this.lbl_path_macros);
		this.tp_macros.Controls.Add(this.label32);
		this.tp_macros.Controls.Add(this.btn_search_local);
		this.tp_macros.Controls.Add(this.txt_search_local);
		this.tp_macros.Controls.Add(this.lb_macros);
		this.tp_macros.Location = new System.Drawing.Point(4, 22);
		this.tp_macros.Name = "tp_macros";
		this.tp_macros.Size = new System.Drawing.Size(548, 370);
		this.tp_macros.TabIndex = 3;
		this.tp_macros.Text = "Macros";
		this.gb_local.Controls.Add(this.txt_rename);
		this.gb_local.Controls.Add(this.btn_apply_rename);
		this.gb_local.Controls.Add(this.btn_delete);
		this.gb_local.Controls.Add(this.label17);
		this.gb_local.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_local.Location = new System.Drawing.Point(206, 126);
		this.gb_local.Name = "gb_local";
		this.gb_local.Size = new System.Drawing.Size(122, 150);
		this.gb_local.TabIndex = 12;
		this.gb_local.TabStop = false;
		this.gb_local.Text = "Local";
		this.txt_rename.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_rename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_rename.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_rename.Location = new System.Drawing.Point(20, 45);
		this.txt_rename.Name = "txt_rename";
		this.txt_rename.Size = new System.Drawing.Size(80, 20);
		this.txt_rename.TabIndex = 3;
		this.btn_apply_rename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_apply_rename.Location = new System.Drawing.Point(20, 69);
		this.btn_apply_rename.Name = "btn_apply_rename";
		this.btn_apply_rename.Size = new System.Drawing.Size(80, 23);
		this.btn_apply_rename.TabIndex = 0;
		this.btn_apply_rename.Text = "Apply";
		this.btn_apply_rename.UseVisualStyleBackColor = true;
		this.btn_apply_rename.Click += new System.EventHandler(btn_apply_rename_Click);
		this.btn_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_delete.ForeColor = System.Drawing.Color.FromArgb(255, 128, 128);
		this.btn_delete.Location = new System.Drawing.Point(20, 108);
		this.btn_delete.Name = "btn_delete";
		this.btn_delete.Size = new System.Drawing.Size(80, 23);
		this.btn_delete.TabIndex = 0;
		this.btn_delete.Text = "delete";
		this.btn_delete.UseVisualStyleBackColor = true;
		this.btn_delete.Click += new System.EventHandler(btn_delete_Click);
		this.label17.AutoSize = true;
		this.label17.Location = new System.Drawing.Point(17, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(83, 13);
		this.label17.TabIndex = 2;
		this.label17.Text = "Rename Macro:";
		this.gb_upload.Controls.Add(this.txt_macroName);
		this.gb_upload.Controls.Add(this.label18);
		this.gb_upload.Controls.Add(this.label21);
		this.gb_upload.Controls.Add(this.cb_anonymously);
		this.gb_upload.Controls.Add(this.btn_start_up);
		this.gb_upload.Controls.Add(this.txt_levelid);
		this.gb_upload.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_upload.Location = new System.Drawing.Point(334, 126);
		this.gb_upload.Name = "gb_upload";
		this.gb_upload.Size = new System.Drawing.Size(207, 150);
		this.gb_upload.TabIndex = 11;
		this.gb_upload.TabStop = false;
		this.gb_upload.Text = "Upload";
		this.txt_macroName.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_macroName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_macroName.Enabled = false;
		this.txt_macroName.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_macroName.Location = new System.Drawing.Point(84, 20);
		this.txt_macroName.Name = "txt_macroName";
		this.txt_macroName.Size = new System.Drawing.Size(100, 20);
		this.txt_macroName.TabIndex = 3;
		this.label18.AutoSize = true;
		this.label18.Location = new System.Drawing.Point(13, 23);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(69, 13);
		this.label18.TabIndex = 2;
		this.label18.Text = "Macro name:";
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(13, 49);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(50, 13);
		this.label21.TabIndex = 2;
		this.label21.Text = "Level ID:";
		this.cb_anonymously.AutoSize = true;
		this.cb_anonymously.Location = new System.Drawing.Point(84, 71);
		this.cb_anonymously.Name = "cb_anonymously";
		this.cb_anonymously.Size = new System.Drawing.Size(115, 17);
		this.cb_anonymously.TabIndex = 1;
		this.cb_anonymously.Text = "anonymous upload";
		this.cb_anonymously.UseVisualStyleBackColor = true;
		this.btn_start_up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_start_up.Location = new System.Drawing.Point(84, 108);
		this.btn_start_up.Name = "btn_start_up";
		this.btn_start_up.Size = new System.Drawing.Size(100, 23);
		this.btn_start_up.TabIndex = 0;
		this.btn_start_up.Text = "\ud83e\udc09 Upload";
		this.btn_start_up.UseVisualStyleBackColor = true;
		this.btn_start_up.Click += new System.EventHandler(btn_start_up_Click);
		this.txt_levelid.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_levelid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_levelid.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_levelid.Location = new System.Drawing.Point(84, 46);
		this.txt_levelid.Name = "txt_levelid";
		this.txt_levelid.Size = new System.Drawing.Size(100, 20);
		this.txt_levelid.TabIndex = 0;
		this.lbl_size_macros.AutoSize = true;
		this.lbl_size_macros.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_size_macros.Location = new System.Drawing.Point(249, 8);
		this.lbl_size_macros.Name = "lbl_size_macros";
		this.lbl_size_macros.Size = new System.Drawing.Size(25, 13);
		this.lbl_size_macros.TabIndex = 7;
		this.lbl_size_macros.Text = "n/a";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(206, 8);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(37, 13);
		this.label30.TabIndex = 8;
		this.label30.Text = "size:";
		this.lbl_path_macros.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_path_macros.Location = new System.Drawing.Point(249, 32);
		this.lbl_path_macros.Name = "lbl_path_macros";
		this.lbl_path_macros.Size = new System.Drawing.Size(294, 91);
		this.lbl_path_macros.TabIndex = 9;
		this.lbl_path_macros.Text = "n/a";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(206, 32);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(37, 13);
		this.label32.TabIndex = 10;
		this.label32.Text = "path:";
		this.btn_search_local.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_search_local.Location = new System.Drawing.Point(144, 253);
		this.btn_search_local.Name = "btn_search_local";
		this.btn_search_local.Size = new System.Drawing.Size(56, 23);
		this.btn_search_local.TabIndex = 6;
		this.btn_search_local.Text = "search...";
		this.btn_search_local.UseVisualStyleBackColor = true;
		this.btn_search_local.Click += new System.EventHandler(btn_search_local_Click);
		this.txt_search_local.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_search_local.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_search_local.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_search_local.Location = new System.Drawing.Point(3, 254);
		this.txt_search_local.Name = "txt_search_local";
		this.txt_search_local.Size = new System.Drawing.Size(135, 20);
		this.txt_search_local.TabIndex = 5;
		this.lb_macros.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.lb_macros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lb_macros.ForeColor = System.Drawing.Color.Gainsboro;
		this.lb_macros.FormattingEnabled = true;
		this.lb_macros.Location = new System.Drawing.Point(3, 3);
		this.lb_macros.Name = "lb_macros";
		this.lb_macros.Size = new System.Drawing.Size(197, 249);
		this.lb_macros.TabIndex = 0;
		this.lb_macros.SelectedIndexChanged += new System.EventHandler(lb_macros_SelectedIndexChanged);
		this.tp_online.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_online.Controls.Add(this.lbl_online_id);
		this.tp_online.Controls.Add(this.lbl_online_verified);
		this.tp_online.Controls.Add(this.lbl_online_lid);
		this.tp_online.Controls.Add(this.lbl_online_creator);
		this.tp_online.Controls.Add(this.lbl_online_type);
		this.tp_online.Controls.Add(this.lbl_online_size);
		this.tp_online.Controls.Add(this.lbl_online_name);
		this.tp_online.Controls.Add(this.lv_online);
		this.tp_online.Controls.Add(this.label25);
		this.tp_online.Controls.Add(this.lbl_leaderboard_verified);
		this.tp_online.Controls.Add(this.lbl_leaderboard_creator);
		this.tp_online.Controls.Add(this.lv_leaderboard);
		this.tp_online.Controls.Add(this.btn_search);
		this.tp_online.Controls.Add(this.txt_search);
		this.tp_online.Controls.Add(this.btn_leaderboard);
		this.tp_online.Controls.Add(this.btn_load);
		this.tp_online.Location = new System.Drawing.Point(4, 22);
		this.tp_online.Name = "tp_online";
		this.tp_online.Size = new System.Drawing.Size(548, 370);
		this.tp_online.TabIndex = 4;
		this.tp_online.Text = "Online";
		this.lbl_online_id.Location = new System.Drawing.Point(2, 0);
		this.lbl_online_id.Name = "lbl_online_id";
		this.lbl_online_id.Size = new System.Drawing.Size(37, 23);
		this.lbl_online_id.TabIndex = 22;
		this.lbl_online_id.Text = "id";
		this.lbl_online_id.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_online_verified.Location = new System.Drawing.Point(454, 0);
		this.lbl_online_verified.Name = "lbl_online_verified";
		this.lbl_online_verified.Size = new System.Drawing.Size(93, 23);
		this.lbl_online_verified.TabIndex = 22;
		this.lbl_online_verified.Text = "Verified";
		this.lbl_online_verified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_online_lid.Location = new System.Drawing.Point(394, 0);
		this.lbl_online_lid.Name = "lbl_online_lid";
		this.lbl_online_lid.Size = new System.Drawing.Size(59, 23);
		this.lbl_online_lid.TabIndex = 22;
		this.lbl_online_lid.Text = "LevelID";
		this.lbl_online_lid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_online_creator.Location = new System.Drawing.Point(333, 0);
		this.lbl_online_creator.Name = "lbl_online_creator";
		this.lbl_online_creator.Size = new System.Drawing.Size(60, 23);
		this.lbl_online_creator.TabIndex = 22;
		this.lbl_online_creator.Text = "Creator";
		this.lbl_online_creator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_online_type.Location = new System.Drawing.Point(284, 0);
		this.lbl_online_type.Name = "lbl_online_type";
		this.lbl_online_type.Size = new System.Drawing.Size(48, 23);
		this.lbl_online_type.TabIndex = 22;
		this.lbl_online_type.Text = "Type";
		this.lbl_online_type.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_online_size.Location = new System.Drawing.Point(223, 0);
		this.lbl_online_size.Name = "lbl_online_size";
		this.lbl_online_size.Size = new System.Drawing.Size(60, 23);
		this.lbl_online_size.TabIndex = 22;
		this.lbl_online_size.Text = "Size";
		this.lbl_online_size.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_online_name.Location = new System.Drawing.Point(40, 0);
		this.lbl_online_name.Name = "lbl_online_name";
		this.lbl_online_name.Size = new System.Drawing.Size(182, 23);
		this.lbl_online_name.TabIndex = 22;
		this.lbl_online_name.Text = "Name";
		this.lbl_online_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lv_online.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
		this.lv_online.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lv_online.Columns.AddRange(new System.Windows.Forms.ColumnHeader[7] { this.ch_id, this.ch_name, this.ch_size, this.ch_Type, this.ch_creator, this.ch_levelid, this.ch_verified });
		this.lv_online.ForeColor = System.Drawing.Color.Gainsboro;
		this.lv_online.FullRowSelect = true;
		this.lv_online.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lv_online.HideSelection = false;
		this.lv_online.Location = new System.Drawing.Point(1, 0);
		this.lv_online.Name = "lv_online";
		this.lv_online.Size = new System.Drawing.Size(547, 248);
		this.lv_online.TabIndex = 1;
		this.lv_online.UseCompatibleStateImageBehavior = false;
		this.lv_online.View = System.Windows.Forms.View.Details;
		this.lv_online.DoubleClick += new System.EventHandler(lv_online_DoubleClick);
		this.ch_id.Text = "ID";
		this.ch_id.Width = 40;
		this.ch_name.Text = "Name";
		this.ch_name.Width = 181;
		this.ch_size.Text = "Size";
		this.ch_Type.Text = "Type";
		this.ch_Type.Width = 50;
		this.ch_creator.Text = "Creator";
		this.ch_levelid.Text = "LevelID";
		this.ch_verified.Text = "Verified";
		this.ch_verified.Width = 50;
		this.label25.Location = new System.Drawing.Point(289, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(258, 23);
		this.label25.TabIndex = 24;
		this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_leaderboard_verified.Location = new System.Drawing.Point(190, 0);
		this.lbl_leaderboard_verified.Name = "lbl_leaderboard_verified";
		this.lbl_leaderboard_verified.Size = new System.Drawing.Size(114, 23);
		this.lbl_leaderboard_verified.TabIndex = 23;
		this.lbl_leaderboard_verified.Text = "Verified Macros";
		this.lbl_leaderboard_verified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbl_leaderboard_creator.Location = new System.Drawing.Point(2, 0);
		this.lbl_leaderboard_creator.Name = "lbl_leaderboard_creator";
		this.lbl_leaderboard_creator.Size = new System.Drawing.Size(187, 23);
		this.lbl_leaderboard_creator.TabIndex = 22;
		this.lbl_leaderboard_creator.Text = "Macro Creator";
		this.lbl_leaderboard_creator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lv_leaderboard.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.lv_leaderboard.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lv_leaderboard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.ch_lb_creator, this.ch_amount });
		this.lv_leaderboard.ForeColor = System.Drawing.Color.Gainsboro;
		this.lv_leaderboard.FullRowSelect = true;
		this.lv_leaderboard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lv_leaderboard.HideSelection = false;
		this.lv_leaderboard.Items.AddRange(new System.Windows.Forms.ListViewItem[2] { listViewItem, listViewItem2 });
		this.lv_leaderboard.Location = new System.Drawing.Point(1, 0);
		this.lv_leaderboard.Name = "lv_leaderboard";
		this.lv_leaderboard.Size = new System.Drawing.Size(551, 194);
		this.lv_leaderboard.TabIndex = 21;
		this.lv_leaderboard.UseCompatibleStateImageBehavior = false;
		this.lv_leaderboard.View = System.Windows.Forms.View.Details;
		this.ch_lb_creator.Text = "Name";
		this.ch_lb_creator.Width = 184;
		this.ch_amount.Text = "amount";
		this.ch_amount.Width = 66;
		this.btn_search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_search.Location = new System.Drawing.Point(106, 253);
		this.btn_search.Name = "btn_search";
		this.btn_search.Size = new System.Drawing.Size(56, 23);
		this.btn_search.TabIndex = 4;
		this.btn_search.Text = "search...";
		this.btn_search.UseVisualStyleBackColor = true;
		this.btn_search.Click += new System.EventHandler(btn_search_Click);
		this.txt_search.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_search.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_search.Location = new System.Drawing.Point(3, 254);
		this.txt_search.Name = "txt_search";
		this.txt_search.Size = new System.Drawing.Size(100, 20);
		this.txt_search.TabIndex = 3;
		this.btn_leaderboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_leaderboard.Location = new System.Drawing.Point(384, 253);
		this.btn_leaderboard.Name = "btn_leaderboard";
		this.btn_leaderboard.Size = new System.Drawing.Size(96, 23);
		this.btn_leaderboard.TabIndex = 2;
		this.btn_leaderboard.Text = "\ud83c\udfc6 Leaderboard";
		this.btn_leaderboard.UseVisualStyleBackColor = true;
		this.btn_leaderboard.Click += new System.EventHandler(btn_leaderboard_Click);
		this.btn_load.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_load.Location = new System.Drawing.Point(486, 253);
		this.btn_load.Name = "btn_load";
		this.btn_load.Size = new System.Drawing.Size(60, 23);
		this.btn_load.TabIndex = 2;
		this.btn_load.Text = "\ud83e\udc0b load...";
		this.btn_load.UseVisualStyleBackColor = true;
		this.btn_load.Click += new System.EventHandler(btn_load_Click);
		this.tp_settings.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_settings.Controls.Add(this.btn_toggle_smoothfix);
		this.tp_settings.Controls.Add(this.btn_upgrade);
		this.tp_settings.Controls.Add(this.cb_framerate);
		this.tp_settings.Controls.Add(this.txt_versions);
		this.tp_settings.Controls.Add(this.btn_apply);
		this.tp_settings.Controls.Add(this.num_fps_settings);
		this.tp_settings.Controls.Add(this.label14);
		this.tp_settings.Controls.Add(this.lbl_versions);
		this.tp_settings.Controls.Add(this.label26);
		this.tp_settings.Controls.Add(this.label13);
		this.tp_settings.Controls.Add(this.label20);
		this.tp_settings.Controls.Add(this.label12);
		this.tp_settings.Location = new System.Drawing.Point(4, 22);
		this.tp_settings.Name = "tp_settings";
		this.tp_settings.Size = new System.Drawing.Size(548, 370);
		this.tp_settings.TabIndex = 5;
		this.tp_settings.Text = "Settings";
		this.btn_upgrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_upgrade.Location = new System.Drawing.Point(308, 97);
		this.btn_upgrade.Name = "btn_upgrade";
		this.btn_upgrade.Size = new System.Drawing.Size(142, 23);
		this.btn_upgrade.TabIndex = 5;
		this.btn_upgrade.Text = "\ud83e\udc09  Upgrade";
		this.btn_upgrade.UseVisualStyleBackColor = true;
		this.btn_upgrade.Visible = false;
		this.btn_upgrade.Click += new System.EventHandler(btn_upgrade_Click);
		this.cb_framerate.AutoSize = true;
		this.cb_framerate.Location = new System.Drawing.Point(190, 63);
		this.cb_framerate.Name = "cb_framerate";
		this.cb_framerate.Size = new System.Drawing.Size(15, 14);
		this.cb_framerate.TabIndex = 4;
		this.cb_framerate.UseVisualStyleBackColor = true;
		this.txt_versions.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_versions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_versions.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.txt_versions.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_versions.Location = new System.Drawing.Point(65, 126);
		this.txt_versions.Multiline = true;
		this.txt_versions.Name = "txt_versions";
		this.txt_versions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.txt_versions.Size = new System.Drawing.Size(466, 142);
		this.txt_versions.TabIndex = 3;
		this.btn_apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_apply.Location = new System.Drawing.Point(456, 97);
		this.btn_apply.Name = "btn_apply";
		this.btn_apply.Size = new System.Drawing.Size(75, 23);
		this.btn_apply.TabIndex = 2;
		this.btn_apply.Text = "✔   apply";
		this.btn_apply.UseVisualStyleBackColor = true;
		this.btn_apply.Click += new System.EventHandler(btn_apply_Click);
		this.num_fps_settings.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.num_fps_settings.ForeColor = System.Drawing.Color.Gainsboro;
		this.num_fps_settings.Location = new System.Drawing.Point(133, 18);
		this.num_fps_settings.Maximum = new decimal(new int[4] { 1024, 0, 0, 0 });
		this.num_fps_settings.Name = "num_fps_settings";
		this.num_fps_settings.Size = new System.Drawing.Size(72, 20);
		this.num_fps_settings.TabIndex = 1;
		this.label14.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.label14.Location = new System.Drawing.Point(311, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(220, 29);
		this.label14.TabIndex = 0;
		this.label14.Text = "To high Framerates can cause the game to run slow";
		this.lbl_versions.AutoSize = true;
		this.lbl_versions.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.lbl_versions.Location = new System.Drawing.Point(8, 129);
		this.lbl_versions.Name = "lbl_versions";
		this.lbl_versions.Size = new System.Drawing.Size(61, 13);
		this.lbl_versions.TabIndex = 0;
		this.lbl_versions.Text = "Versions:";
		this.label26.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.label26.Location = new System.Drawing.Point(232, 63);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(277, 31);
		this.label26.TabIndex = 0;
		this.label26.Text = "Uses Frames instead of Coordinates (beta, macros might not work / only mouse for now)";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.label13.Location = new System.Drawing.Point(232, 20);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(73, 13);
		this.label13.TabIndex = 0;
		this.label13.Text = "Important: ";
		this.label20.AutoSize = true;
		this.label20.Location = new System.Drawing.Point(8, 63);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(114, 13);
		this.label20.TabIndex = 0;
		this.label20.Text = "Use Frames for Macro:";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(8, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(109, 13);
		this.label12.TabIndex = 0;
		this.label12.Text = "Recording Framerate:";
		this.tp_help.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.tp_help.Controls.Add(this.lbl_help_answer);
		this.tp_help.Controls.Add(this.lbl_h_dual);
		this.tp_help.Controls.Add(this.lbl_h_slowgame);
		this.tp_help.Controls.Add(this.lbl_h_macro);
		this.tp_help.Controls.Add(this.lbl_h_dll);
		this.tp_help.Controls.Add(this.lbl_h_online);
		this.tp_help.Controls.Add(this.lbl_h_crash);
		this.tp_help.Location = new System.Drawing.Point(4, 22);
		this.tp_help.Name = "tp_help";
		this.tp_help.Size = new System.Drawing.Size(548, 370);
		this.tp_help.TabIndex = 6;
		this.tp_help.Text = "Help";
		this.lbl_help_answer.Font = new System.Drawing.Font("Consolas", 8.25f);
		this.lbl_help_answer.Location = new System.Drawing.Point(13, 174);
		this.lbl_help_answer.Name = "lbl_help_answer";
		this.lbl_help_answer.Size = new System.Drawing.Size(523, 95);
		this.lbl_help_answer.TabIndex = 0;
		this.lbl_help_answer.Text = "click on a topic at the top!";
		this.lbl_h_dual.AutoSize = true;
		this.lbl_h_dual.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lbl_h_dual.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_h_dual.ForeColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.lbl_h_dual.Location = new System.Drawing.Point(13, 123);
		this.lbl_h_dual.Name = "lbl_h_dual";
		this.lbl_h_dual.Size = new System.Drawing.Size(181, 13);
		this.lbl_h_dual.TabIndex = 0;
		this.lbl_h_dual.Text = "My dual parts are not working";
		this.lbl_h_dual.Click += new System.EventHandler(lbl_h_dual_Click);
		this.lbl_h_slowgame.AutoSize = true;
		this.lbl_h_slowgame.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lbl_h_slowgame.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_h_slowgame.ForeColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.lbl_h_slowgame.Location = new System.Drawing.Point(13, 102);
		this.lbl_h_slowgame.Name = "lbl_h_slowgame";
		this.lbl_h_slowgame.Size = new System.Drawing.Size(145, 13);
		this.lbl_h_slowgame.TabIndex = 0;
		this.lbl_h_slowgame.Text = "My game is running slow";
		this.lbl_h_slowgame.Click += new System.EventHandler(lbl_h_slowgame_Click);
		this.lbl_h_macro.AutoSize = true;
		this.lbl_h_macro.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lbl_h_macro.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_h_macro.ForeColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.lbl_h_macro.Location = new System.Drawing.Point(13, 81);
		this.lbl_h_macro.Name = "lbl_h_macro";
		this.lbl_h_macro.Size = new System.Drawing.Size(121, 13);
		this.lbl_h_macro.TabIndex = 0;
		this.lbl_h_macro.Text = "My macro is missing";
		this.lbl_h_macro.Click += new System.EventHandler(lbl_h_macro_Click);
		this.lbl_h_dll.AutoSize = true;
		this.lbl_h_dll.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lbl_h_dll.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_h_dll.ForeColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.lbl_h_dll.Location = new System.Drawing.Point(13, 60);
		this.lbl_h_dll.Name = "lbl_h_dll";
		this.lbl_h_dll.Size = new System.Drawing.Size(73, 13);
		this.lbl_h_dll.TabIndex = 0;
		this.lbl_h_dll.Text = "Missing DLL";
		this.lbl_h_dll.Click += new System.EventHandler(lbl_h_dll_Click);
		this.lbl_h_online.AutoSize = true;
		this.lbl_h_online.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lbl_h_online.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_h_online.ForeColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.lbl_h_online.Location = new System.Drawing.Point(13, 39);
		this.lbl_h_online.Name = "lbl_h_online";
		this.lbl_h_online.Size = new System.Drawing.Size(253, 13);
		this.lbl_h_online.TabIndex = 0;
		this.lbl_h_online.Text = "The online section of the bot doesnt work\r\n";
		this.lbl_h_online.Click += new System.EventHandler(lbl_h_online_Click);
		this.lbl_h_crash.AutoSize = true;
		this.lbl_h_crash.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lbl_h_crash.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl_h_crash.ForeColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.lbl_h_crash.Location = new System.Drawing.Point(13, 19);
		this.lbl_h_crash.Name = "lbl_h_crash";
		this.lbl_h_crash.Size = new System.Drawing.Size(151, 13);
		this.lbl_h_crash.TabIndex = 0;
		this.lbl_h_crash.Text = "The bot crashes the game";
		this.lbl_h_crash.Click += new System.EventHandler(lbl_h_crash_Click);
		this.gb_license.Controls.Add(this.btn_switch_cid);
		this.gb_license.Controls.Add(this.btn_reload_license);
		this.gb_license.Controls.Add(this.label7);
		this.gb_license.Controls.Add(this.txt_cid);
		this.gb_license.Controls.Add(this.btn_copy);
		this.gb_license.Controls.Add(this.btn_join);
		this.gb_license.Controls.Add(this.lbl_message);
		this.gb_license.Controls.Add(this.btn_close_license);
		this.gb_license.ForeColor = System.Drawing.Color.Gainsboro;
		this.gb_license.Location = new System.Drawing.Point(12, 42);
		this.gb_license.Name = "gb_license";
		this.gb_license.Size = new System.Drawing.Size(523, 250);
		this.gb_license.TabIndex = 10;
		this.gb_license.TabStop = false;
		this.gb_license.Text = "License";
		this.btn_switch_cid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_switch_cid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.btn_switch_cid.Location = new System.Drawing.Point(123, 211);
		this.btn_switch_cid.Name = "btn_switch_cid";
		this.btn_switch_cid.Size = new System.Drawing.Size(107, 23);
		this.btn_switch_cid.TabIndex = 5;
		this.btn_switch_cid.Text = "Show old ClientID";
		this.btn_switch_cid.UseVisualStyleBackColor = true;
		this.btn_switch_cid.Click += new System.EventHandler(btn_switch_cid_Click);
		this.btn_reload_license.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_reload_license.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.btn_reload_license.Location = new System.Drawing.Point(236, 211);
		this.btn_reload_license.Name = "btn_reload_license";
		this.btn_reload_license.Size = new System.Drawing.Size(150, 23);
		this.btn_reload_license.TabIndex = 5;
		this.btn_reload_license.Text = "Reload license information";
		this.btn_reload_license.UseVisualStyleBackColor = true;
		this.btn_reload_license.Click += new System.EventHandler(btn_reload_license_Click);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(10, 169);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 13);
		this.label7.TabIndex = 4;
		this.label7.Text = "Your ClientID:";
		this.txt_cid.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.txt_cid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt_cid.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txt_cid.ForeColor = System.Drawing.Color.Gainsboro;
		this.txt_cid.Location = new System.Drawing.Point(13, 186);
		this.txt_cid.Name = "txt_cid";
		this.txt_cid.Size = new System.Drawing.Size(373, 22);
		this.txt_cid.TabIndex = 3;
		this.btn_copy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_copy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.btn_copy.Location = new System.Drawing.Point(392, 184);
		this.btn_copy.Name = "btn_copy";
		this.btn_copy.Size = new System.Drawing.Size(121, 24);
		this.btn_copy.TabIndex = 0;
		this.btn_copy.Text = "Copy";
		this.btn_copy.UseVisualStyleBackColor = true;
		this.btn_copy.Click += new System.EventHandler(btn_copy_Click);
		this.btn_join.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_join.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.btn_join.Location = new System.Drawing.Point(392, 211);
		this.btn_join.Name = "btn_join";
		this.btn_join.Size = new System.Drawing.Size(121, 23);
		this.btn_join.TabIndex = 0;
		this.btn_join.Text = "Join Discord Server";
		this.btn_join.UseVisualStyleBackColor = true;
		this.btn_join.Click += new System.EventHandler(btn_join_Click);
		this.lbl_message.Location = new System.Drawing.Point(10, 30);
		this.lbl_message.Name = "lbl_message";
		this.lbl_message.Size = new System.Drawing.Size(503, 84);
		this.lbl_message.TabIndex = 2;
		this.lbl_message.Text = "n/a";
		this.btn_close_license.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_license.BackgroundImage");
		this.btn_close_license.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_license.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_license.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_license.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_license.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_license.ForeColor = System.Drawing.Color.Red;
		this.btn_close_license.Location = new System.Drawing.Point(502, 10);
		this.btn_close_license.Name = "btn_close_license";
		this.btn_close_license.Size = new System.Drawing.Size(17, 17);
		this.btn_close_license.TabIndex = 5;
		this.btn_close_license.UseVisualStyleBackColor = true;
		this.btn_close_license.Click += new System.EventHandler(btn_close_license_Click);
		this.btn_close_license.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close_license.MouseLeave += new System.EventHandler(btn_close_MouseLeave);
		this.statusStrip1.BackgroundImage = (System.Drawing.Image)resources.GetObject("statusStrip1.BackgroundImage");
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.lbl_toolbar });
		this.statusStrip1.Location = new System.Drawing.Point(0, 426);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(547, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 3;
		this.statusStrip1.Text = "statusStrip1";
		this.statusStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(Form1_MouseDown);
		this.lbl_toolbar.Name = "lbl_toolbar";
		this.lbl_toolbar.Size = new System.Drawing.Size(25, 17);
		this.lbl_toolbar.Text = "n/a";
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close.ForeColor = System.Drawing.Color.Red;
		this.btn_close.Location = new System.Drawing.Point(527, 3);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(17, 17);
		this.btn_close.TabIndex = 5;
		this.btn_close.UseVisualStyleBackColor = true;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.btn_close.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close.MouseLeave += new System.EventHandler(btn_close_MouseLeave);
		this.tmr_prevent_max.Enabled = true;
		this.tmr_prevent_max.Interval = 10;
		this.tmr_prevent_max.Tick += new System.EventHandler(tmr_prevent_max_Tick);
		this.gb_convert.Controls.Add(this.cb_convert_macros);
		this.gb_convert.Controls.Add(this.label6);
		this.gb_convert.Controls.Add(this.label1);
		this.gb_convert.Controls.Add(this.label5);
		this.gb_convert.Controls.Add(this.label2);
		this.gb_convert.Controls.Add(this.textBox1);
		this.gb_convert.Controls.Add(this.label3);
		this.gb_convert.Controls.Add(this.btn_convert);
		this.gb_convert.Controls.Add(this.comboBox2);
		this.gb_convert.Location = new System.Drawing.Point(583, 43);
		this.gb_convert.Name = "gb_convert";
		this.gb_convert.Size = new System.Drawing.Size(523, 152);
		this.gb_convert.TabIndex = 7;
		this.gb_convert.TabStop = false;
		this.gb_convert.Text = "Convert";
		this.cb_convert_macros.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.cb_convert_macros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cb_convert_macros.ForeColor = System.Drawing.Color.Gainsboro;
		this.cb_convert_macros.FormattingEnabled = true;
		this.cb_convert_macros.Location = new System.Drawing.Point(66, 19);
		this.cb_convert_macros.Name = "cb_convert_macros";
		this.cb_convert_macros.Size = new System.Drawing.Size(206, 21);
		this.cb_convert_macros.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(278, 84);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(151, 13);
		this.label6.TabIndex = 5;
		this.label6.Text = "<- Enter the output name";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(17, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(43, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "Macro: ";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(278, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(127, 13);
		this.label5.TabIndex = 5;
		this.label5.Text = "<- Select your macro";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(17, 53);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(37, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "Mode:";
		this.textBox1.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.ForeColor = System.Drawing.Color.Gainsboro;
		this.textBox1.Location = new System.Drawing.Point(66, 79);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(206, 21);
		this.textBox1.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(17, 83);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(42, 13);
		this.label3.TabIndex = 1;
		this.label3.Text = "Output:";
		this.btn_convert.Enabled = false;
		this.btn_convert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_convert.Location = new System.Drawing.Point(19, 110);
		this.btn_convert.Name = "btn_convert";
		this.btn_convert.Size = new System.Drawing.Size(254, 23);
		this.btn_convert.TabIndex = 3;
		this.btn_convert.Text = "start (not implemented)";
		this.btn_convert.UseVisualStyleBackColor = true;
		this.comboBox2.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.comboBox2.ForeColor = System.Drawing.Color.Gainsboro;
		this.comboBox2.FormattingEnabled = true;
		this.comboBox2.Items.AddRange(new object[2] { "GDBot -> xBot", "xBot -> GDBot" });
		this.comboBox2.Location = new System.Drawing.Point(154, 49);
		this.comboBox2.Name = "comboBox2";
		this.comboBox2.Size = new System.Drawing.Size(118, 21);
		this.comboBox2.TabIndex = 2;
		this.comboBox2.Text = "GDBot -> xBot";
		this.pb_logo.BackColor = System.Drawing.Color.Transparent;
		this.pb_logo.BackgroundImage = (System.Drawing.Image)resources.GetObject("pb_logo.BackgroundImage");
		this.pb_logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pb_logo.Location = new System.Drawing.Point(0, 0);
		this.pb_logo.Name = "pb_logo";
		this.pb_logo.Size = new System.Drawing.Size(276, 42);
		this.pb_logo.TabIndex = 8;
		this.pb_logo.TabStop = false;
		this.pb_logo.MouseDown += new System.Windows.Forms.MouseEventHandler(Form1_MouseDown);
		this.pb_hidetab.BackColor = System.Drawing.Color.Transparent;
		this.pb_hidetab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pb_hidetab.Location = new System.Drawing.Point(248, 0);
		this.pb_hidetab.Name = "pb_hidetab";
		this.pb_hidetab.Size = new System.Drawing.Size(27, 42);
		this.pb_hidetab.TabIndex = 9;
		this.pb_hidetab.TabStop = false;
		this.pb_hidetab.Visible = false;
		this.pb_hidetab.MouseDown += new System.Windows.Forms.MouseEventHandler(Form1_MouseDown);
		this.btn_minimize_leave.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_minimize_leave.BackgroundImage");
		this.btn_minimize_leave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_minimize_leave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize_leave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize_leave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize_leave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_minimize_leave.ForeColor = System.Drawing.Color.Red;
		this.btn_minimize_leave.Location = new System.Drawing.Point(418, 3);
		this.btn_minimize_leave.Name = "btn_minimize_leave";
		this.btn_minimize_leave.Size = new System.Drawing.Size(17, 17);
		this.btn_minimize_leave.TabIndex = 5;
		this.btn_minimize_leave.UseVisualStyleBackColor = true;
		this.btn_minimize_leave.Visible = false;
		this.btn_minimize_leave.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_minimize_hover.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_minimize_hover.BackgroundImage");
		this.btn_minimize_hover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_minimize_hover.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize_hover.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize_hover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize_hover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_minimize_hover.ForeColor = System.Drawing.Color.Red;
		this.btn_minimize_hover.Location = new System.Drawing.Point(441, 3);
		this.btn_minimize_hover.Name = "btn_minimize_hover";
		this.btn_minimize_hover.Size = new System.Drawing.Size(17, 17);
		this.btn_minimize_hover.TabIndex = 5;
		this.btn_minimize_hover.UseVisualStyleBackColor = true;
		this.btn_minimize_hover.Visible = false;
		this.btn_minimize.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_minimize.BackgroundImage");
		this.btn_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_minimize.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_minimize.ForeColor = System.Drawing.Color.Red;
		this.btn_minimize.Location = new System.Drawing.Point(508, 3);
		this.btn_minimize.Name = "btn_minimize";
		this.btn_minimize.Size = new System.Drawing.Size(17, 17);
		this.btn_minimize.TabIndex = 5;
		this.btn_minimize.UseVisualStyleBackColor = true;
		this.btn_minimize.Click += new System.EventHandler(btn_minimize_Click);
		this.btn_minimize.MouseEnter += new System.EventHandler(btn_minimize_MouseEnter);
		this.btn_minimize.MouseLeave += new System.EventHandler(btn_minimize_MouseLeave);
		this.btn_close_leave.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_leave.BackgroundImage");
		this.btn_close_leave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_leave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_leave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_leave.ForeColor = System.Drawing.Color.Red;
		this.btn_close_leave.Location = new System.Drawing.Point(464, 3);
		this.btn_close_leave.Name = "btn_close_leave";
		this.btn_close_leave.Size = new System.Drawing.Size(17, 17);
		this.btn_close_leave.TabIndex = 5;
		this.btn_close_leave.UseVisualStyleBackColor = true;
		this.btn_close_leave.Visible = false;
		this.btn_close_leave.MouseEnter += new System.EventHandler(btn_close_MouseEnter);
		this.btn_close_hover.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close_hover.BackgroundImage");
		this.btn_close_hover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_close_hover.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		this.btn_close_hover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_close_hover.ForeColor = System.Drawing.Color.Red;
		this.btn_close_hover.Location = new System.Drawing.Point(487, 3);
		this.btn_close_hover.Name = "btn_close_hover";
		this.btn_close_hover.Size = new System.Drawing.Size(17, 17);
		this.btn_close_hover.TabIndex = 5;
		this.btn_close_hover.UseVisualStyleBackColor = true;
		this.btn_close_hover.Visible = false;
		this.btn_toggle_smoothfix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn_toggle_smoothfix.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_toggle_smoothfix.Location = new System.Drawing.Point(11, 97);
		this.btn_toggle_smoothfix.Name = "btn_toggle_smoothfix";
		this.btn_toggle_smoothfix.Size = new System.Drawing.Size(106, 23);
		this.btn_toggle_smoothfix.TabIndex = 6;
		this.btn_toggle_smoothfix.Text = "Toggle Smoothfix";
		this.btn_toggle_smoothfix.UseVisualStyleBackColor = true;
		this.btn_toggle_smoothfix.Click += new System.EventHandler(btn_toggle_smoothfix_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
		base.ClientSize = new System.Drawing.Size(547, 448);
		base.Controls.Add(this.pb_hidetab);
		base.Controls.Add(this.pb_logo);
		base.Controls.Add(this.gb_convert);
		base.Controls.Add(this.btn_close_hover);
		base.Controls.Add(this.btn_minimize_hover);
		base.Controls.Add(this.btn_close_leave);
		base.Controls.Add(this.btn_minimize_leave);
		base.Controls.Add(this.btn_minimize);
		base.Controls.Add(this.btn_close);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.tc_main);
		base.Controls.Add(this.gb_license);
		this.ForeColor = System.Drawing.Color.Gainsboro;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Form1";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "xBot v3 - Pro - by AndxArtZ";
		base.TransparencyKey = System.Drawing.Color.Lime;
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(Form1_MouseDown);
		this.tc_main.ResumeLayout(false);
		this.tp_rec.ResumeLayout(false);
		this.tp_rec.PerformLayout();
		this.gb_rec_options.ResumeLayout(false);
		this.gb_rec_options.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.num_resp_time).EndInit();
		((System.ComponentModel.ISupportInitialize)this.num_fps).EndInit();
		((System.ComponentModel.ISupportInitialize)this.num_boost).EndInit();
		((System.ComponentModel.ISupportInitialize)this.num_speedhack).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tb_speed).EndInit();
		this.tp_play.ResumeLayout(false);
		this.tc_play.ResumeLayout(false);
		this.tp_play_normal.ResumeLayout(false);
		this.tp_play_normal.PerformLayout();
		this.pnl_edit.ResumeLayout(false);
		this.pnl_edit.PerformLayout();
		this.gb_edit.ResumeLayout(false);
		this.gb_edit.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tb_edit_release).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tb_edit_press).EndInit();
		this.gb_clicks_play.ResumeLayout(false);
		this.gb_clicks_play.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.tb_distance).EndInit();
		this.tp_play_sequence.ResumeLayout(false);
		this.tp_play_sequence.PerformLayout();
		this.tp_macros.ResumeLayout(false);
		this.tp_macros.PerformLayout();
		this.gb_local.ResumeLayout(false);
		this.gb_local.PerformLayout();
		this.gb_upload.ResumeLayout(false);
		this.gb_upload.PerformLayout();
		this.tp_online.ResumeLayout(false);
		this.tp_online.PerformLayout();
		this.tp_settings.ResumeLayout(false);
		this.tp_settings.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.num_fps_settings).EndInit();
		this.tp_help.ResumeLayout(false);
		this.tp_help.PerformLayout();
		this.gb_license.ResumeLayout(false);
		this.gb_license.PerformLayout();
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		this.gb_convert.ResumeLayout(false);
		this.gb_convert.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pb_logo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pb_hidetab).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
