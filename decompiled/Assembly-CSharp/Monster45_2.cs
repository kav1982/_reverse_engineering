using UnityEngine;

public class Monster45_2 : UnitBase
{
	private Monster45 thismonster;

	private bool left;

	private void Start()
	{
		myPpt.unitCfg.isDeadSE3D = false;
	}

	public void SetMother(Monster45 Monster45, bool left)
	{
		thismonster = Monster45;
		base.gameObject.SetActive(value: true);
		this.left = left;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		Vector3 vector = info.beHitShakeDir;
		info.ignoreFloatText = true;
		info.beHitShakeDir = Vector3.zero;
		info.ignoreBeHitColor = true;
		UnitDotsSyncSystem.AddTakeDamageRequest(thismonster.myPpt.myEntity, info);
		info.beHitShakeDir = vector;
		info.ignoreBeHitColor = false;
		info.ignoreFloatText = false;
		info.knockbackForce = Vector3.zero;
		info.dontCreateDeadEF = true;
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		thismonster.EyeBeenAttack(left);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		thismonster.GetComponent<Monster45>().EyeDead(left);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawSphere(Tool2D.IgnoreZPoint(base.transform), 0.1f);
		Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
		Gizmos.DrawSphere(Tool2D.IgnoreZPoint(thismonster.transform), 0.1f);
	}
}
