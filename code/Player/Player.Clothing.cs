namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	public ModelEntity Head { get; set; }

	public void SetClothes( )
	{
		string HeadModel = string.Empty;
		string BodyModel = string.Empty;

		switch ( (eGender)Data.Character.Gender )
		{
			case eGender.MALE:
			{
				BodyModel = "models/humans/male.vmdl";
				HeadModel = "models/humans/heads/adam/adam.vmdl";
				break;
			}
			case eGender.FEMALE:
			{
				BodyModel = "models/humans/female.vmdl";
				HeadModel = "models/humans/heads/adam/adam.vmdl";
				break;
			}
		}

		SetModel( BodyModel );

		Head?.Delete();
		Head = new AnimatedEntity( HeadModel, this )
		{
			Transform = GetBoneTransform( GetBoneIndex( "head" ) ),
			EnableHideInFirstPerson = true
		};
	}

	public void TickClothes()
	{
		if ( Head is not null )
			Head.Transform = GetBoneTransform( GetBoneIndex( "head" ) );
	}

	public void Clean()
	{
		Head?.Delete();
	}
}
