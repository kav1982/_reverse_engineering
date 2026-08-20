using System;
using DG.Tweening;
using UnityEngine;

public class Teammate4FusePillar : MonoBehaviour
{
	public GameObject[] body;

	public Material[] pillarColor;

	public SpriteRenderer sr;

	public SpriteRenderer fireSr;

	public Sprite[] pillarHpSprites;

	public Sprite[] firePillarHpSprites;

	public ParticleSystem[] pillarBottomLight;

	public ParticleSystem[] pillarParticle;

	public Shadow selfShadow;

	public Transform bodyRoot;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public void HideTeammate()
	{
		selfShadow.ShadowGO.SetActive(value: false);
		bodyRoot.gameObject.SetActive(value: false);
	}

	public void ShowTeammate()
	{
		selfShadow.ShadowGO.SetActive(value: true);
		bodyRoot.gameObject.SetActive(value: true);
	}

	public void OnEnterDelayDeathEvent()
	{
		sr.material.SetInt(UseGhostEffect, 1);
		fireSr.material.SetInt(UseGhostEffect, 1);
	}

	public void OnEnterFuseStateEvent()
	{
		sr.material.SetInt(UseFuseShineEffect, 1);
		sr.material.DOFloat(1f, FuseShineProcess, 1.3f);
		fireSr.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
		ParticleSystem[] array = pillarBottomLight;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
		array = pillarParticle;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
		selfShadow.ShadowGO.SetActive(value: false);
	}

	public void PillarInitialize(SpellColorType type)
	{
		selfShadow.ShadowGO.SetActive(value: true);
		GameObject[] array = body;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		switch (type)
		{
		case SpellColorType.Fire:
			body[0].SetActive(value: true);
			sr.material = pillarColor[0];
			break;
		case SpellColorType.Frozen:
			body[1].SetActive(value: true);
			sr.material = pillarColor[1];
			break;
		case SpellColorType.Mucus:
			body[2].SetActive(value: true);
			sr.material = pillarColor[2];
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			body[3].SetActive(value: true);
			sr.material = pillarColor[3];
			break;
		case SpellColorType.Venom:
			body[4].SetActive(value: true);
			sr.material = pillarColor[4];
			break;
		case SpellColorType.Void:
			body[5].SetActive(value: true);
			sr.material = pillarColor[5];
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
		sr.flipX = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
		sr.material.SetInt(UseGhostEffect, 0);
		sr.material.SetInt(UseFuseShineEffect, 0);
		sr.material.SetFloat(UseFuseShineEffect, 0f);
		fireSr.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
	}

	public void UpdatePillarDamagePercent(Teammate4FuseController.FusePillarHpState state)
	{
		switch (state)
		{
		case Teammate4FuseController.FusePillarHpState.highHp:
			sr.sprite = pillarHpSprites[0];
			fireSr.sprite = firePillarHpSprites[0];
			break;
		case Teammate4FuseController.FusePillarHpState.HalfHp:
			sr.sprite = pillarHpSprites[1];
			fireSr.sprite = firePillarHpSprites[1];
			break;
		case Teammate4FuseController.FusePillarHpState.LowHp:
			sr.sprite = pillarHpSprites[2];
			fireSr.sprite = firePillarHpSprites[2];
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}
}
