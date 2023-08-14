namespace Mbk.RoleplayAPI.Inventory;

public class ObjectItem : ResourceItem<ObjectResource, ObjectItem>
{
	public override bool CanStackWith( IInventoryItem other )
	{
		return true;
	}

	protected override void BuildTags( HashSet<string> tags )
	{
		tags.Add( "object" );

		base.BuildTags( tags );
	}
}
