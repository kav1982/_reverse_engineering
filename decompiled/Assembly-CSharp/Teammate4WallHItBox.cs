using UnityEngine;

public class Teammate4WallHItBox : Teammate
{
	public BoxCollider hitBox;

	public Teammate4FuseController mainBody;

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		mainBody.TakeDamageFromHitBox(info);
		info.immuneDamage = true;
	}
}
