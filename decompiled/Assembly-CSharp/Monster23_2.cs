using UnityEngine;

public class Monster23_2 : UnitBase
{
	public SpriteRenderer sr;

	public MeshRenderer mr;

	private Sprite nowSprite;

	public VariableInt attackSpellCount;

	public VariableInt deadSpellCount;

	public VariableFloat shootSpellSpeed;

	public VariableFloat deadSpellSpeed;

	[Header("Spell Butterfly")]
	public float spellHeight;

	public float spellOffset;

	public float spellSpeed;

	public float spellDuration;

	public float spellMinSpeed;

	public float spellDamage;

	private SpellSpawnParams ssp;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10031);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void Update()
	{
		if (nowSprite != sr.sprite)
		{
			nowSprite = sr.sprite;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, nowSprite.texture);
		}
		base.Update();
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "Shoot")
		{
			attackSpellCount.RandomResult();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; i < attackSpellCount.result; i++)
			{
				Vector3 dir = Tool2D.GetDir();
				Vector3 vector = (sSPModifier.SpawnPosition = base.transform.position + dir * spellOffset + new Vector3(0f, 0f, 0f - spellHeight));
				sSPModifier.Speed = shootSpellSpeed.RandomResult();
				sSPModifier.Float2 = spellMinSpeed / sSPModifier.Speed;
				sSPModifier.Direction = dir;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		deadSpellCount.RandomResult();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; i < deadSpellCount.result; i++)
		{
			Vector3 dir = Tool2D.GetDir(360f / (float)deadSpellCount.result * (float)i);
			Vector3 vector = (sSPModifier.SpawnPosition = base.transform.position + dir * spellOffset + new Vector3(0f, 0f, 0f - spellHeight));
			sSPModifier.Speed = deadSpellSpeed.RandomResult();
			sSPModifier.Float2 = spellMinSpeed / sSPModifier.Speed;
			sSPModifier.Direction = dir;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}
}
