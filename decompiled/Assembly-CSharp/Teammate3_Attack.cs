using SpriteEffectSystem;
using UnityEngine;

public class Teammate3_Attack : LayerCorrect
{
	[Space(50f)]
	public SpriteRenderer sr;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public SpriteRenderer srFire;

	public SpriteEffectAnima[] FrontEffects;

	public SpriteEffectAnima BackEffect;

	public SpriteRenderer NormalAttackSr;

	public SpriteRenderer VoidAttackSr;

	public void Initialize(SpellBase teammate3)
	{
		NormalAttackSr.enabled = true;
		VoidAttackSr.enabled = false;
		srFire.enabled = false;
		switch (teammate3.ColorType)
		{
		case SpellColorType.Frozen:
			if (sr.material != mat_ECFrozen)
			{
				sr.material = mat_ECFrozen;
			}
			break;
		case SpellColorType.Mucus:
			if (sr.material != mat_ECMucus)
			{
				sr.material = mat_ECMucus;
			}
			break;
		case SpellColorType.Fire:
			srFire.enabled = true;
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Venom:
			if (sr.material != mat_ECVenom)
			{
				sr.material = mat_ECVenom;
			}
			break;
		case SpellColorType.Void:
			NormalAttackSr.enabled = false;
			VoidAttackSr.enabled = true;
			if (sr.material != mat_ECVoid)
			{
				sr.material = mat_ECVoid;
			}
			break;
		default:
			Debug.LogError(teammate3.ColorType);
			break;
		}
		Color value = new Color(1f, 1f, 1f, DataMgr.settingData.SummonTransparent);
		SpellSpriteEffectController.Inst.PlayEffectIgnoreSpellBase(BackEffect, new EffectPlayParam
		{
			Position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect),
			Color = value,
			Scale = base.transform.localScale,
			FilpX = (Random.Range(0, 2) == 0)
		});
		Vector3 layerPoint = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Coordinate);
		layerPoint.z -= 0.1f;
		SpellSpriteEffectController.Inst.PlayEffectIgnoreSpellBase(FrontEffects[Random.Range(0, FrontEffects.Length)], new EffectPlayParam
		{
			Position = layerPoint,
			Color = value,
			Scale = base.transform.localScale,
			FilpX = (Random.Range(0, 2) == 0)
		});
	}
}
