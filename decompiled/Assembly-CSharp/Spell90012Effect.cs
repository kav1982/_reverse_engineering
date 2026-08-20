using UnityEngine;

public class Spell90012Effect : SpellEffectBase
{
	private Transform spriteTrans;

	public VariableFloat rotateSpeed;

	private Spell90012BoBoBomb bombScript;

	protected override void Awake()
	{
		base.Awake();
		bombScript = GetComponent<Spell90012BoBoBomb>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		spriteTrans = null;
	}

	protected override void Update()
	{
		base.Update();
		RotateSprite();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Spell":
			spriteTrans = trans;
			trans.localScale = Vector3.one * Mathf.Pow(bombScript.damageRatio * bombScript.finalDamageRatio, 0.3333f);
			break;
		case "Trail":
			trans.localScale = Vector3.one * Mathf.Pow(bombScript.damageRatio * bombScript.finalDamageRatio, 0.3333f);
			break;
		case "Explosion":
			trans.localScale = Vector3.one * bombScript.spellCfg.radius * 2f;
			break;
		}
	}

	private void RotateSprite()
	{
		if ((bool)spriteTrans)
		{
			spriteTrans.right = Tool2D.GetDir(spriteTrans.right, rotateSpeed.RandomResult() * Time.deltaTime);
		}
	}
}
