using System;
using DG.Tweening;
using UnityEngine;

public class Teammate2FuseHead : MonoBehaviour
{
	public SpriteRenderer headSprite;

	public SpriteRenderer fireOutline;

	public SpriteRenderer fuseHeadSprite;

	public SpriteRenderer fuseFireOutline;

	public Sprite normalHead;

	public Sprite fuseHead;

	public Sprite safeNormalHead;

	public Sprite safeFuseHead;

	public Sprite normalHeadVoid;

	public Sprite fuseHeadVoid;

	public Sprite safeNormalHeadVoid;

	public Sprite safeFuseHeadVoid;

	public Material[] headOutlineColor;

	private bool isFuseHead;

	public Transform headCenter;

	private SpellColorType colorType;

	public float lifeLineHeightShift;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public void OnEnterDelayDeathEvent()
	{
		headSprite.material.SetInt(UseGhostEffect, 1);
		fireOutline.material.SetInt(UseGhostEffect, 1);
		fuseHeadSprite.material.SetInt(UseGhostEffect, 1);
		fuseFireOutline.material.SetInt(UseGhostEffect, 1);
	}

	public void OnEnterFuseStateEvent()
	{
		headSprite.material.SetInt(UseFuseShineEffect, 1);
		headSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		fuseHeadSprite.material.SetInt(UseFuseShineEffect, 1);
		fuseHeadSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		fireOutline.gameObject.SetActive(value: false);
		fuseFireOutline.gameObject.SetActive(value: false);
	}

	public void Initialize(SpellColorType type, bool isTheFuseHead)
	{
		headSprite.material.SetInt(UseGhostEffect, 0);
		fireOutline.material.SetInt(UseGhostEffect, 0);
		headSprite.material.SetInt(UseFuseShineEffect, 0);
		headSprite.material.SetFloat(FuseShineProcess, 0f);
		colorType = type;
		isFuseHead = isTheFuseHead;
		if (isFuseHead)
		{
			fuseHeadSprite.gameObject.SetActive(value: true);
			headSprite.gameObject.SetActive(value: false);
		}
		else
		{
			fuseHeadSprite.gameObject.SetActive(value: false);
			headSprite.gameObject.SetActive(value: true);
		}
		fireOutline.gameObject.SetActive(value: false);
		fuseFireOutline.gameObject.SetActive(value: false);
		switch (type)
		{
		case SpellColorType.Frozen:
			headSprite.material = headOutlineColor[0];
			fuseHeadSprite.material = headOutlineColor[0];
			break;
		case SpellColorType.Mucus:
			headSprite.material = headOutlineColor[1];
			fuseHeadSprite.material = headOutlineColor[1];
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			headSprite.material = headOutlineColor[2];
			fuseHeadSprite.material = headOutlineColor[2];
			break;
		case SpellColorType.Venom:
			headSprite.material = headOutlineColor[3];
			fuseHeadSprite.material = headOutlineColor[3];
			break;
		case SpellColorType.Fire:
			headSprite.material = headOutlineColor[1];
			fuseHeadSprite.material = headOutlineColor[1];
			if (isFuseHead)
			{
				fuseFireOutline.gameObject.SetActive(value: true);
			}
			else
			{
				fireOutline.gameObject.SetActive(value: true);
			}
			break;
		case SpellColorType.Void:
			headSprite.material = headOutlineColor[4];
			fuseHeadSprite.material = headOutlineColor[4];
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public void SetSafeMode(bool state)
	{
		if (state)
		{
			if (colorType != SpellColorType.Void)
			{
				headSprite.sprite = safeNormalHead;
				fuseHeadSprite.sprite = safeFuseHead;
			}
			else
			{
				headSprite.sprite = safeNormalHeadVoid;
				fuseHeadSprite.sprite = safeFuseHeadVoid;
			}
		}
		else if (colorType != SpellColorType.Void)
		{
			headSprite.sprite = normalHead;
			fuseHeadSprite.sprite = fuseHead;
		}
		else
		{
			headSprite.sprite = normalHeadVoid;
			fuseHeadSprite.sprite = fuseHeadVoid;
		}
		fireOutline.sprite = headSprite.sprite;
		fuseFireOutline.sprite = fuseHeadSprite.sprite;
	}
}
