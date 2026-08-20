using UnityEngine;

public class Boss12_Missile : MonoBehaviour
{
	[Header("震屏")]
	public ShockParam shockParam;

	public GameObject parent;

	public float damage;

	public float checkRadius;

	public LayerMask layerMask;

	public float knockBack;

	public void Boom()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		CamController.Inst.SetShock(shockParam);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_MissileBoom", base.transform.position, 1f);
		Collider[] array = Physics.OverlapSphere(base.transform.position, checkRadius, layerMask);
		foreach (Collider collider in array)
		{
			UnitProperty component = collider.GetComponent<UnitProperty>();
			TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
			takeDamageInfo.teammateTakeDamageRatio = 3f;
			takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collider.transform.position, base.transform.position) * knockBack;
			switch (collider.tag)
			{
			case "Player":
			case "Teammate":
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", collider.transform.position, 1f);
				component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
				break;
			case "Brittleness":
				component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
				break;
			case "Destructible":
				component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(parent);
	}
}
