namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	public const string OnAimTarget = "onaimtarget";
	public class OnAimTargetAttribute : EventAttribute
	{
		public OnAimTargetAttribute() : base( OnAimTarget ) { }
	}

	public const string OnFootstep = "onfootstep";
	public class OnFootstepAttribute : EventAttribute
	{
		public OnFootstepAttribute() : base( OnFootstep ) { }
	}
}
