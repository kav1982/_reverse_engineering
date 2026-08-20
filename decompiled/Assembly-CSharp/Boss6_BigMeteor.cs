using System.Collections.Generic;
using UnityEngine;

public class Boss6_BigMeteor : MonoBehaviour
{
	[Header("表现")]
	public bool dropped;

	public bool shown;

	public ParticleSystem trailParticle;

	public ParticleSystem explodeParticle;

	public float fallHeight;

	public float fallHorizontalDistance;

	public float prapareTime;

	public float fallTime;

	public Transform tsf_warningCircle;

	public Transform tsf_WarningScale;

	public Shadow shadow;

	private Vector3 startPosition;

	private Vector3 targetPosition;

	private float existTime;

	[Header("数值")]
	public int damage;

	public float knockback;

	public float range;

	public ShockParam shockParam;

	public void Initialize(Vector3 targetPoint, Vector3 launchDirection)
	{
		existTime = 0f;
		dropped = false;
		shown = false;
		existTime = 0f;
		targetPosition = targetPoint;
		startPosition = targetPoint + new Vector3(0f, 0f, 0f - fallHeight) - launchDirection * fallHorizontalDistance;
		base.transform.position = startPosition;
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		tsf_WarningScale.transform.position = Tool2D.GetLayerPoint(targetPosition, LayerCorrectType.WarningArea);
		tsf_warningCircle.transform.localScale = Vector3.one * existTime / fallTime;
		tsf_WarningScale.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		existTime += Time.deltaTime;
		if (existTime > fallTime && !dropped)
		{
			dropped = true;
			tsf_WarningScale.gameObject.SetActive(value: false);
			trailParticle.Stop();
			explodeParticle.Play();
			shadow.Hide();
			Explode();
		}
		if (existTime > prapareTime && !shown)
		{
			shown = true;
			trailParticle.Play();
		}
		if (!dropped)
		{
			tsf_warningCircle.transform.localScale = Vector3.one * existTime / fallTime;
			if (shown)
			{
				base.transform.position = Vector3.Lerp(startPosition, targetPosition, (existTime - prapareTime) / (fallTime - prapareTime));
			}
		}
		if (existTime > fallTime + 2f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		tsf_WarningScale.transform.position = Tool2D.GetLayerPoint(targetPosition, LayerCorrectType.WarningArea);
	}

	private void Explode()
	{
		SEMgr.Inst.monster34Explosion.PlaySE();
		CamController.Inst.SetShock(shockParam);
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, range, "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].tag == "Spell" || collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				if (!collidersByTag[i].gameObject.activeInHierarchy)
				{
					continue;
				}
				SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
				if (componentInParent.spellCfg.abilityType != SpellAbilityType.FireBall)
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(damage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
				}
			}
			else
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZPoint(collidersByTag[i].transform.position - base.transform.position).normalized * knockback;
				UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
				if (component.unitCfg.unitType == UnitType.Player)
				{
					component.TakeDamage(damage, null, takeDamageInfo);
				}
				else
				{
					component.TakeDamage(damage * 3, null, takeDamageInfo);
				}
			}
		}
	}
}
