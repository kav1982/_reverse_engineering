using UnityEngine;

public class Spell1029Effect : SpellEffectBase
{
	private Spell1029DimensionTraveller ballScript;

	private Transform trailTrans;

	protected override void Awake()
	{
		base.Awake();
		ballScript = (Spell1029DimensionTraveller)base.Spell;
	}

	private void OnDisable()
	{
		trailTrans = null;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Spell":
			trans.localScale = Vector3.one * ballScript.spellCfg.radius;
			break;
		case "Trail":
		{
			trailTrans = trans;
			trans.localScale = Vector3.one * ballScript.spellCfg.radius / 1.2f;
			trans.GetComponent<TrailRenderer>().widthMultiplier = ballScript.spellCfg.radius * 2f / 1.2f;
			Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ParticleSystem component = componentsInChildren[i].GetComponent<ParticleSystem>();
				if ((bool)component)
				{
					component.Stop();
					component.Play();
				}
			}
			break;
		}
		case "Travel":
			trans.localScale = Vector3.one * ballScript.spellCfg.radius / 1.2f;
			break;
		case "Hit":
		case "Disappear":
			trans.right = ballScript.Direction;
			trans.localScale = Vector3.one * ballScript.spellCfg.radius;
			break;
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		UpdateTrailPos();
	}

	public void SpawnNewTrail()
	{
		RecycleTrail();
		Vector3? position = ballScript.tsf_Layer.transform.position;
		ManualCreateEffect("Trail", null, position);
	}

	public void RecycleTrail()
	{
		if ((bool)trailTrans)
		{
			ObjPoolMgr.Inst.RecycleGO(trailTrans.gameObject, 0.7f);
			trailTrans = null;
		}
	}

	private void UpdateTrailPos()
	{
		if ((bool)trailTrans)
		{
			trailTrans.position = ballScript.tsf_Layer.position;
		}
	}
}
