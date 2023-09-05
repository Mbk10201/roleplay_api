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

	public const string OnAFK = "onafk";
	public class OnAFKAttribute : EventAttribute
	{
		public OnAFKAttribute() : base( OnAFK ) { }
	}

	public const string OnUnAFK = "onunafk";
	public class OnUnAFKAttribute : EventAttribute
	{
		public OnUnAFKAttribute() : base( OnUnAFK ) { }
	}
}
