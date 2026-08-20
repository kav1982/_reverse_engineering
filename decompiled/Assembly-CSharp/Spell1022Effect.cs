using UnityEngine;

public class Spell1022Effect : SpellEffectBase
{
	public Transform rotateTsf;

	private Spell1022Boomerang boomerang;

	private Transform bladeTrailTransform;

	private Transform bladeShadowTransform;

	private Transform bladeSpellTransform;

	public float bodySpriteRotateSpeedRatio;

	protected override void Awake()
	{
		base.Awake();
		boomerang = (Spell1022Boomerang)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		bladeTrailTransform = null;
		bladeShadowTransform = null;
		bladeSpellTransform = null;
	}

	protected override void Update()
	{
		base.Update();
		UpdateSpriteRotate();
		UpdateTrailPosition();
		UpdateShadowRotateion();
	}

	private void UpdateSpriteRotate()
	{
		rotateTsf.eulerAngles = new Vector3(0f, 0f, rotateTsf.eulerAngles.z + boomerang.spellCfg.speed * bodySpriteRotateSpeedRatio * Time.deltaTime);
	}

	private void UpdateShadowRotateion()
	{
		if (!(bladeShadowTransform == null) && !(bladeSpellTransform == null))
		{
			bladeShadowTransform.transform.right = bladeSpellTransform.right;
		}
	}

	private void UpdateTrailPosition()
	{
		if (!(bladeTrailTransform == null))
		{
			Vector3 vector = rotateTsf.position - bladeTrailTransform.transform.position;
			if (vector.sqrMagnitude > 0.15f * base.transform.localScale.x * base.transform.localScale.x)
			{
				bladeTrailTransform.transform.position += vector * 0.4f;
			}
		}
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name == "Trail")
		{
			bladeTrailTransform = trans;
		}
		if (effect.Name == "Shadow")
		{
			bladeShadowTransform = trans;
		}
		if (effect.Name == "Spell")
		{
			bladeSpellTransform = trans;
		}
	}
}
