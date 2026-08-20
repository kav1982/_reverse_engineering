using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster8_HealFog : MonoBehaviour
{
	public float healInterval;

	public float healTime;

	private float lifeTimer;

	private float healTimer;

	public int healAmount;

	public float healRadius;

	public UnitProperty owner;

	private void OnEnable()
	{
		healTimer = healInterval;
		lifeTimer = 0f;
	}

	private void Update()
	{
		healTimer += Time.deltaTime;
		lifeTimer += Time.deltaTime;
		if (lifeTimer > healTime || !(healTimer > healInterval))
		{
			return;
		}
		healTimer = 0f;
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; i++)
		{
			Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[i];
			if (UnitDotsSyncSystem.EntityIsValid(entity) && ((Vector3)UnitDotsSyncSystem.GetComponentData<LocalTransform>(entity).Position - base.transform.position).sqrMagnitude < healRadius * healRadius && entity != owner.myEntity)
			{
				UnitDotsSyncSystem.UnitRecoveryHP(entity, healAmount, World.DefaultGameObjectInjectionWorld.EntityManager);
			}
		}
	}
}
