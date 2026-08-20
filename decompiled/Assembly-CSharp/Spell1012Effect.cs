using DG.Tweening;
using UnityEngine;

public class Spell1012Effect : SpellEffectBase
{
	public Spell1012TrickMine SpellScript;

	public AnimationCurve twinkle;

	private Tweener twinkleSequence;

	private SpriteRenderer bodySr;

	private static readonly int Twinkle = Shader.PropertyToID("_Twinkle");

	private static readonly int IsVoidSpell = Shader.PropertyToID("_IsVoidSpell");

	private void OnDisable()
	{
		twinkleSequence.Kill(complete: true);
		twinkleSequence = null;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name == "Body")
		{
			bodySr = trans.GetComponent<SpriteRenderer>();
			bodySr.material.SetFloat(Twinkle, 0f);
			if (SpellScript.ColorType == SpellColorType.Void)
			{
				bodySr.material.SetFloat(IsVoidSpell, 1f);
			}
			else
			{
				bodySr.material.SetFloat(IsVoidSpell, 0f);
			}
		}
	}

	public void StartTwinkle(float time)
	{
		if (twinkleSequence == null && !(bodySr == null))
		{
			bodySr.material.SetFloat(Twinkle, 1f);
			twinkleSequence = bodySr.material.DOFloat(0f, Twinkle, time).SetEase(twinkle);
		}
	}
}
