using DG.Tweening;
using UnityEngine;

public class Spell2007WormHole : MonoBehaviour
{
	public SpriteRenderer HoleSr;

	public SpriteRenderer HoleFireSr;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public Material mat_ECThunder;

	public Sprite NormalSr;

	public Sprite VoidSr;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public void EnterDelayDeathState()
	{
		HoleSr.material.SetInt(UseGhostEffect, 1);
	}

	public void EnterFuseState()
	{
		HoleSr.material.SetInt(UseFuseShineEffect, 1);
		HoleSr.material.DOFloat(1f, FuseShineProcess, 1.3f);
	}

	public void SetColor(SpellColorType type)
	{
		HoleSr.material.SetInt(UseGhostEffect, 0);
		HoleSr.material.SetInt(UseFuseShineEffect, 0);
		HoleSr.material.SetFloat(FuseShineProcess, 0f);
		HoleFireSr.gameObject.SetActive(value: false);
		HoleSr.sprite = ((type == SpellColorType.Void) ? VoidSr : NormalSr);
		switch (type)
		{
		case SpellColorType.Frozen:
			if (HoleSr.material != mat_ECFrozen)
			{
				HoleSr.material = mat_ECFrozen;
			}
			break;
		case SpellColorType.Mucus:
			if (HoleSr.material != mat_ECMucus)
			{
				HoleSr.material = mat_ECMucus;
			}
			break;
		case SpellColorType.Fire:
			if (HoleSr.material != mat_ECPlayer)
			{
				HoleSr.material = mat_ECPlayer;
			}
			HoleFireSr.gameObject.SetActive(value: true);
			break;
		case SpellColorType.Thunder:
			if (HoleSr.material != mat_ECThunder)
			{
				HoleSr.material = mat_ECThunder;
			}
			break;
		case SpellColorType.Player:
			if (HoleSr.material != mat_ECPlayer)
			{
				HoleSr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Venom:
			if (HoleSr.material != mat_ECVenom)
			{
				HoleSr.material = mat_ECVenom;
			}
			break;
		case SpellColorType.Void:
			if (HoleSr.material != mat_ECVoid)
			{
				HoleSr.material = mat_ECVoid;
			}
			break;
		case SpellColorType.Monster:
			break;
		}
	}
}
