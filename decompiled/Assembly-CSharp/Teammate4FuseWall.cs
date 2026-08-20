using System;
using DG.Tweening;
using UnityEngine;

public class Teammate4FuseWall : MonoBehaviour
{
	public SpriteRenderer wallSprite;

	public SpriteRenderer wallLight;

	public static readonly int RotateAngle = Shader.PropertyToID("_RotateAngle");

	public Material[] wallColorMaterial;

	public Material[] wallLightMaterial;

	public Material[] wallParticleMaterial;

	public LineRenderer wallShadow;

	public ParticleSystem wallParticle;

	public Sprite[] wallHpSprites;

	public Sprite[] wallLightHpSprites;

	public Sprite[] fireWallHpSprites;

	public SpriteRenderer fireOutline;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public Transform bodyRoot;

	private float wallAngle;

	public float initialAngle { get; set; }

	public void OnEnterDelayDeathEvent()
	{
		wallSprite.material.SetInt(UseGhostEffect, 1);
		wallLight.material.SetInt(UseGhostEffect, 1);
	}

	public void HideTeammate()
	{
		ShadowToggle(toggle: false);
		bodyRoot.gameObject.SetActive(value: false);
	}

	public void ShowTeammate()
	{
		ShadowToggle();
		bodyRoot.gameObject.SetActive(value: true);
	}

	public void ShadowToggle(bool toggle = true)
	{
		wallShadow.gameObject.SetActive(toggle);
	}

	public void OnEnterFuseStateEvent()
	{
		wallSprite.material.SetInt(UseFuseShineEffect, 1);
		wallSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		wallLight.material.DOFloat(1f, FuseShineProcess, 1.3f);
		wallParticle.Stop();
		wallShadow.gameObject.SetActive(value: false);
	}

	public void WallInitialize(SpellColorType type)
	{
		wallAngle = 0f;
		wallShadow.gameObject.SetActive(value: true);
		switch (type)
		{
		case SpellColorType.Fire:
			wallSprite.material = wallColorMaterial[0];
			wallLight.material = wallLightMaterial[0];
			wallParticle.GetComponent<Renderer>().material = wallParticleMaterial[0];
			break;
		case SpellColorType.Frozen:
			wallSprite.material = wallColorMaterial[1];
			wallLight.material = wallLightMaterial[1];
			wallParticle.GetComponent<Renderer>().material = wallParticleMaterial[1];
			break;
		case SpellColorType.Mucus:
			wallSprite.material = wallColorMaterial[2];
			wallLight.material = wallLightMaterial[2];
			wallParticle.GetComponent<Renderer>().material = wallParticleMaterial[2];
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			wallSprite.material = wallColorMaterial[3];
			wallLight.material = wallLightMaterial[3];
			wallParticle.GetComponent<Renderer>().material = wallParticleMaterial[3];
			break;
		case SpellColorType.Venom:
			wallSprite.material = wallColorMaterial[4];
			wallLight.material = wallLightMaterial[4];
			wallParticle.GetComponent<Renderer>().material = wallParticleMaterial[4];
			break;
		case SpellColorType.Void:
			wallSprite.material = wallColorMaterial[5];
			wallLight.material = wallLightMaterial[5];
			wallParticle.GetComponent<Renderer>().material = wallParticleMaterial[5];
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
		wallSprite.material.SetInt(UseGhostEffect, 0);
		wallLight.material.SetInt(UseGhostEffect, 0);
		wallSprite.material.SetInt(UseFuseShineEffect, 0);
		wallSprite.material.SetFloat(FuseShineProcess, 0f);
		wallLight.material.SetFloat(FuseShineProcess, 0f);
	}

	private void LateUpdate()
	{
		wallShadow.SetPosition(0, base.transform.position + Tool2D.GetDir(wallAngle + 90f) + new Vector3(0f, 0f, 900f));
		wallShadow.SetPosition(1, base.transform.position - Tool2D.GetDir(wallAngle + 90f) + new Vector3(0f, 0f, 900f));
	}

	public void UpdataWallAngle(float wallAngle)
	{
		this.wallAngle = wallAngle;
		wallSprite.material.SetFloat(RotateAngle, wallAngle);
		wallLight.material.SetFloat(RotateAngle, wallAngle);
		fireOutline.material.SetFloat(RotateAngle, wallAngle);
		ParticleSystem.ShapeModule shape = wallParticle.shape;
		shape.rotation = new Vector3(0f, 0f, wallAngle);
	}

	public void UpdatePillarDamagePercent(Teammate4FuseController.FusePillarHpState state)
	{
		switch (state)
		{
		case Teammate4FuseController.FusePillarHpState.highHp:
			wallSprite.sprite = wallHpSprites[0];
			wallLight.sprite = wallLightHpSprites[0];
			fireOutline.sprite = fireWallHpSprites[0];
			break;
		case Teammate4FuseController.FusePillarHpState.HalfHp:
			wallSprite.sprite = wallHpSprites[1];
			wallLight.sprite = fireWallHpSprites[1];
			fireOutline.sprite = fireWallHpSprites[1];
			break;
		case Teammate4FuseController.FusePillarHpState.LowHp:
			wallSprite.sprite = wallHpSprites[2];
			wallLight.sprite = fireWallHpSprites[2];
			fireOutline.sprite = fireWallHpSprites[2];
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}

	public void RestartWallParticle()
	{
		wallParticle.Stop();
		wallParticle.Play();
	}
}
