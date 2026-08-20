using System;
using DG.Tweening;
using UnityEngine;

public class Spell2006CannonBarrelController : MonoBehaviour
{
	public Transform TeammateBombPosition;

	public Transform BackUpTeammateBombTransform;

	public Transform ShootPosition;

	public SpriteRenderer CannonTattooSprite;

	public SpriteRenderer HandSprite;

	public SpriteRenderer CannonFrontSprite;

	public SpriteRenderer CannonBackSprite;

	public SpriteRenderer TattooSprite;

	private static readonly int Progress1 = Shader.PropertyToID("_Progress");

	private Teammate6 targetTeammate;

	private Teammate6Sync targetTeammateSync;

	public Animator BarrelAnimator;

	public Sprite[] HandColorSprites;

	public Material[] TattooMaterial;

	public Sprite[] CannonFrontSprites;

	public Sprite[] CannonBackSprites;

	public Sprite[] TattooSprites;

	private static readonly int Idle = Animator.StringToHash("Idle");

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	[HideInInspector]
	public bool ShowCannon;

	private void OnEnable()
	{
		TattooToggle(toggle: false);
		HandToggle(toggle: false);
		UpdateTattooProgress(0f);
		CannonTattooSprite.material.SetFloat(UseGhostEffect, 0f);
		HandSprite.material.SetInt(UseGhostEffect, 0);
		CannonFrontSprite.material.SetInt(UseGhostEffect, 0);
		CannonBackSprite.material.SetInt(UseGhostEffect, 0);
		CannonTattooSprite.material.SetInt(UseFuseShineEffect, 0);
		CannonTattooSprite.material.SetFloat(0, FuseShineProcess);
		HandSprite.material.SetInt(UseFuseShineEffect, 0);
		HandSprite.material.SetFloat(0, FuseShineProcess);
		CannonFrontSprite.material.SetInt(UseFuseShineEffect, 0);
		CannonFrontSprite.material.SetFloat(0, FuseShineProcess);
		CannonBackSprite.material.SetInt(UseFuseShineEffect, 0);
		CannonBackSprite.material.SetFloat(0, FuseShineProcess);
		CannonTattooSprite.material.SetFloat(Transparency, DataMgr.settingData.SummonTransparent);
		ShowCannon = true;
		BarrelAnimator.SetTrigger(Idle);
	}

	public void CannonLoadBackUpAmmo()
	{
		BarrelAnimator.SetTrigger("Load");
	}

	public void OnEnterDelayDeathEvent()
	{
		CannonTattooSprite.material.SetFloat(UseGhostEffect, 1f);
		HandSprite.material.SetInt(UseGhostEffect, 1);
		CannonFrontSprite.material.SetInt(UseGhostEffect, 1);
		CannonBackSprite.material.SetInt(UseGhostEffect, 1);
	}

	public void OnEnterFuseStateEvent()
	{
		ShowCannon = false;
		HandSprite.material.SetInt(UseFuseShineEffect, 1);
		HandSprite.material.SetFloat(FuseShineProcess, 0f);
		HandSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		CannonFrontSprite.material.SetInt(UseFuseShineEffect, 1);
		CannonFrontSprite.material.SetFloat(FuseShineProcess, 0f);
		CannonFrontSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		CannonBackSprite.material.SetInt(UseFuseShineEffect, 1);
		CannonBackSprite.material.SetFloat(FuseShineProcess, 0f);
		CannonBackSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		CannonTattooSprite.material.SetFloat(Transparency, 0f);
	}

	public void BarrelInitialize(SpellColorType color, Teammate6Sync targetTeammate)
	{
		targetTeammateSync = targetTeammate;
		switch (color)
		{
		case SpellColorType.Frozen:
			HandSprite.sprite = HandColorSprites[0];
			CannonTattooSprite.material = TattooMaterial[0];
			CannonFrontSprite.sprite = CannonFrontSprites[0];
			CannonBackSprite.sprite = CannonBackSprites[0];
			TattooSprite.sprite = TattooSprites[0];
			break;
		case SpellColorType.Mucus:
			HandSprite.sprite = HandColorSprites[1];
			CannonTattooSprite.material = TattooMaterial[1];
			CannonFrontSprite.sprite = CannonFrontSprites[1];
			CannonBackSprite.sprite = CannonBackSprites[1];
			TattooSprite.sprite = TattooSprites[1];
			break;
		case SpellColorType.Player:
			HandSprite.sprite = HandColorSprites[2];
			CannonTattooSprite.material = TattooMaterial[2];
			CannonFrontSprite.sprite = CannonFrontSprites[2];
			CannonBackSprite.sprite = CannonBackSprites[2];
			TattooSprite.sprite = TattooSprites[2];
			break;
		case SpellColorType.Venom:
			HandSprite.sprite = HandColorSprites[3];
			CannonTattooSprite.material = TattooMaterial[3];
			CannonFrontSprite.sprite = CannonFrontSprites[3];
			CannonBackSprite.sprite = CannonBackSprites[3];
			TattooSprite.sprite = TattooSprites[3];
			break;
		case SpellColorType.Fire:
			HandSprite.sprite = HandColorSprites[4];
			CannonTattooSprite.material = TattooMaterial[4];
			CannonFrontSprite.sprite = CannonFrontSprites[4];
			CannonBackSprite.sprite = CannonBackSprites[4];
			TattooSprite.sprite = TattooSprites[4];
			break;
		case SpellColorType.Thunder:
			HandSprite.sprite = HandColorSprites[5];
			CannonTattooSprite.material = TattooMaterial[5];
			CannonFrontSprite.sprite = CannonFrontSprites[5];
			CannonBackSprite.sprite = CannonBackSprites[5];
			TattooSprite.sprite = TattooSprites[5];
			break;
		case SpellColorType.Void:
			HandSprite.sprite = HandColorSprites[6];
			CannonTattooSprite.material = TattooMaterial[6];
			CannonFrontSprite.sprite = CannonFrontSprites[6];
			CannonBackSprite.sprite = CannonBackSprites[6];
			TattooSprite.sprite = TattooSprites[6];
			break;
		default:
			throw new ArgumentOutOfRangeException("color", color, null);
		}
		UpdateTattooProgress(0f);
	}

	public void BarrelInitialize(SpellColorType color, Teammate6 targetTeammate)
	{
		this.targetTeammate = targetTeammate;
		switch (color)
		{
		case SpellColorType.Frozen:
			HandSprite.sprite = HandColorSprites[0];
			CannonTattooSprite.material = TattooMaterial[0];
			CannonFrontSprite.sprite = CannonFrontSprites[0];
			CannonBackSprite.sprite = CannonBackSprites[0];
			TattooSprite.sprite = TattooSprites[0];
			break;
		case SpellColorType.Mucus:
			HandSprite.sprite = HandColorSprites[1];
			CannonTattooSprite.material = TattooMaterial[1];
			CannonFrontSprite.sprite = CannonFrontSprites[1];
			CannonBackSprite.sprite = CannonBackSprites[1];
			TattooSprite.sprite = TattooSprites[1];
			break;
		case SpellColorType.Player:
			HandSprite.sprite = HandColorSprites[2];
			CannonTattooSprite.material = TattooMaterial[2];
			CannonFrontSprite.sprite = CannonFrontSprites[2];
			CannonBackSprite.sprite = CannonBackSprites[2];
			TattooSprite.sprite = TattooSprites[2];
			break;
		case SpellColorType.Venom:
			HandSprite.sprite = HandColorSprites[3];
			CannonTattooSprite.material = TattooMaterial[3];
			CannonFrontSprite.sprite = CannonFrontSprites[3];
			CannonBackSprite.sprite = CannonBackSprites[3];
			TattooSprite.sprite = TattooSprites[3];
			break;
		case SpellColorType.Fire:
			HandSprite.sprite = HandColorSprites[4];
			CannonTattooSprite.material = TattooMaterial[4];
			CannonFrontSprite.sprite = CannonFrontSprites[4];
			CannonBackSprite.sprite = CannonBackSprites[4];
			TattooSprite.sprite = TattooSprites[4];
			break;
		case SpellColorType.Thunder:
			HandSprite.sprite = HandColorSprites[5];
			CannonTattooSprite.material = TattooMaterial[5];
			CannonFrontSprite.sprite = CannonFrontSprites[5];
			CannonBackSprite.sprite = CannonBackSprites[5];
			TattooSprite.sprite = TattooSprites[5];
			break;
		case SpellColorType.Void:
			HandSprite.sprite = HandColorSprites[6];
			CannonTattooSprite.material = TattooMaterial[6];
			CannonFrontSprite.sprite = CannonFrontSprites[6];
			CannonBackSprite.sprite = CannonBackSprites[6];
			TattooSprite.sprite = TattooSprites[6];
			break;
		default:
			throw new ArgumentOutOfRangeException("color", color, null);
		}
		UpdateTattooProgress(0f);
	}

	private void Update()
	{
		float value = 1f;
		CannonTattooSprite.material.SetFloat(Transparency, ShowCannon ? DataMgr.settingData.SummonTransparent : 0f);
		HandSprite.material.SetFloat(Transparency, value);
		CannonFrontSprite.material.SetFloat(Transparency, value);
		CannonBackSprite.material.SetFloat(Transparency, value);
	}

	public void HandToggle(bool toggle)
	{
		HandSprite.enabled = toggle;
	}

	public void TattooToggle(bool toggle)
	{
		CannonTattooSprite.enabled = toggle;
	}

	public void UpdateTattooProgress(float progress)
	{
		CannonTattooSprite.material.SetFloat(Progress1, progress);
	}

	private void OnDisable()
	{
		UpdateTattooProgress(0f);
	}
}
