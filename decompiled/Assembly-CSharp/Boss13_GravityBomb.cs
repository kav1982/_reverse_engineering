using UnityEngine;

public class Boss13_GravityBomb : MonoBehaviour
{
	public float activeDuration;

	public float activeTimer;

	public float force;

	public float summonReduceFactor;

	public float gravityDistance;

	private void OnEnable()
	{
		activeTimer = 0f;
	}

	private void Update()
	{
		activeTimer += Time.deltaTime;
		if (activeTimer >= activeDuration)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerPoint) < gravityDistance * gravityDistance)
		{
			UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
			componentData.TakeKnockback(Tool2D.IgnoreZV2ToV1Normal(base.transform.position, PlayerMgr.Inst.PlayerPoint) * force * Time.deltaTime);
			UnitDotsSyncSystem.SetComponentData(componentData, PlayerMgr.Inst.PlayerEtt);
		}
		foreach (UnitProperty summonsPpt in PlayerMgr.Inst.summonsPpts)
		{
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, summonsPpt.transform.position) < gravityDistance * gravityDistance)
			{
				UnitProperty_Dots componentData2 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(summonsPpt.myEntity);
				componentData2.TakeKnockback(Tool2D.IgnoreZV2ToV1Normal(base.transform.position, summonsPpt.transform.position) * force / summonReduceFactor * Time.deltaTime);
				UnitDotsSyncSystem.SetComponentData(componentData2, summonsPpt.myEntity);
			}
		}
	}
}
