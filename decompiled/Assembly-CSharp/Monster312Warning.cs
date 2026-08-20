using Unity.Entities;
using UnityEngine;

public class Monster312Warning : MonoBehaviour
{
	public float delay;

	public float durationTimer;

	public Entity monsterEntity;

	private void OnEnable()
	{
		durationTimer = 0f;
	}

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer >= delay || !UnitDotsSyncSystem.EntityIsValid(monsterEntity))
		{
			durationTimer = 0f;
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
