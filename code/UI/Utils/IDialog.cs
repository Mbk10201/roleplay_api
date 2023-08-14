namespace Mbk.RoleplayAPI.UI.Utils;

public interface IDialog
{
	bool AllowMovement { get; }
	bool IsOpen { get; }
	void Open();
	void Close();
}
