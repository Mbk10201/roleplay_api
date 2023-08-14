namespace Mbk.RoleplayAPI.UI.Shared.AlertSystem;

public interface IAlertItem
{
	string Title { get; set; }
	string Message { get; set; }
	eAlertType Type { get; set; }
}
