using System.Media;
using System.Threading;

namespace xBot_Pro_UI;

internal class MacroClick
{
	private int coordinate;

	private bool mousedown;

	private SoundPlayer player;

	public int getCoord => coordinate;

	public MacroClick(int coord, bool down, string soundfile)
	{
		player = new SoundPlayer();
		player.SoundLocation = soundfile;
		player.Load();
		while (!player.IsLoadCompleted)
		{
			Thread.Sleep(10);
		}
		coordinate = coord;
		mousedown = down;
	}

	public async void execute()
	{
		player.Play();
	}
}
