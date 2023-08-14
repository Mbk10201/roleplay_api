namespace Mbk.RoleplayAPI.Models;

/// <summary>
/// This is what you should derive when creating a Panel
/// </summary>
public partial class RoleplayPanel : Panel
{
	private bool _display;
	public bool Display
	{
		get
		{
			return _display;
		}
		set
		{
			_display = value;
			if ( value )
				Style.Display = DisplayMode.Flex;
			else
				Style.Display = DisplayMode.None;
		}
	}

	private bool _hideOthers;

	public bool HideOthers
	{
		get
		{
			return _hideOthers;
		}
		set
		{
			_hideOthers = value;

			foreach ( RoleplayPanel panel in Game.RootPanel.Children.Where(x => x != this).OfType<RoleplayPanel>() )
			{
				panel.Display = !value;
			}
		}
	}
}
