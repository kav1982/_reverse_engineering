using System.Collections.Generic;
using UnityEngine;

public class Elite11_DamageZone : MonoBehaviour
{
	public Elite11 master;

	public int damage;

	public float knockback;

	public float damageRadius;

	public float damageInterval;

	public float damageCD;

	private float timer;

	public List<UnitProperty> attackedPpts = new List<UnitProperty>();

	private List<float> attackedPptsCD = new List<float>();

	private void Update()
	{
		for (int num = attackedPptsCD.Count - 1; num >= 0; num--)
		{
			attackedPptsCD[num] -= Time.deltaTime;
			if (attackedPptsCD[num] < 0f)
			{
				attackedPptsCD.RemoveAt(num);
				attackedPpts.RemoveAt(num);
			}
		}
		timer += Time.deltaTime;
		if (timer > damageInterval)
		{
			DealDamage();
		}
	}

	private void DealDamage()
	{
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, damageRadius, "Destructible", "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
			if (!attackedPpts.Contains(component))
			{
				string text = "EF_MonsterPunch_Large";
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
				TakeDamageInfo info = new TakeDamageInfo();
				if (component.unitCfg.unitType == UnitType.Player)
				{
					component.TakeDamage(damage, null, info);
					attackedPpts.Add(component);
					attackedPptsCD.Add(damageCD);
				}
				else
				{
					component.TakeDamage(damage, null, info);
					attackedPpts.Add(component);
					attackedPptsCD.Add(damageCD);
				}
				component.TakeKnockback((component.transform.position - master.transform.position).normalized * knockback);
			}
		}
	}
}
