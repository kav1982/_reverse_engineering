using System.Linq;
using UnityEngine;

public class Spell1002Effect : SpellEffectBase
{
	private static readonly int BeHit = Shader.PropertyToID("_BeHit");

	private float beHitTimer;

	public GameObject fireBorder;

	protected override void Update()
	{
		base.Update();
		if (beHitTimer > 0f)
		{
			beHitTimer -= Time.deltaTime;
			if (beHitTimer <= 0f)
			{
				SetBeHit(0);
			}
		}
	}

	public void PlayBeHitEffect()
	{
		SetBeHit(1);
		beHitTimer = 0.1f;
	}

	private void SetBeHit(int val)
	{
		Transform item = CurrentEffects.FirstOrDefault(((Transform trans, SpellEffectSettings effect) e) => e.effect.Name == "Spell").trans;
		if ((bool)item)
		{
			MeshRenderer meshRenderer = item.GetComponent<MeshRenderer>();
			if (!meshRenderer)
			{
				meshRenderer = item.GetComponentInChildren<MeshRenderer>();
			}
			if (!meshRenderer)
			{
				Debug.LogWarning("滚石的 MeshRenderer 哪里去了？");
			}
			else
			{
				meshRenderer.material.SetInt(BeHit, val);
			}
		}
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		if (effect.Name == "Spell")
		{
			fireBorder.SetActive(base.Spell.ColorType == SpellColorType.Fire);
			SetBeHit(0);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		fireBorder.SetActive(value: false);
		beHitTimer = 0f;
		SetBeHit(0);
	}
}
