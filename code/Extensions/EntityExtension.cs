namespace RP.Utility;

internal static class EntityExtension
{
	public static float CloseDistance => 64f;

	public static bool IsClose( this Entity self, Entity target )
	{
		return GetVectorDistance( self, target ) <= CloseDistance;
	}

	public static bool IsClose( this Entity self, Entity target, float distance )
	{
		return GetVectorDistance( self, target ) <= distance;
	}

	public static float GetVectorDistance( this Entity self, Entity target )
	{
		return self.Position.Distance( target.Position );
	}
}
