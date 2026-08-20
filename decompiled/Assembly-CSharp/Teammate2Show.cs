using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Teammate2Show : MonoBehaviour
{
	public Teammate2_Leg[] legs;

	public Teammate2_Leg[] essenceLegs;

	public SpellColorType colorType;

	public float rootScale;

	public float spellScale;

	public Vector3 mainHeadRootPos;

	public List<Vector3> fuseHeadRootPos = new List<Vector3>();

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	public void Init(DynamicBuffer<LegsData> legsData, DynamicBuffer<EssenceLegsData> essenceLegsData, float scale)
	{
		base.transform.localScale = Vector3.one * scale;
		legs = new Teammate2_Leg[legsData.Length];
		essenceLegs = new Teammate2_Leg[essenceLegsData.Length];
		for (int i = 0; i < legsData.Length; i++)
		{
			GameObject obj = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/2002/2002_Leg"), base.transform);
			obj.SetActive(value: true);
			Teammate2_Leg component = obj.GetComponent<Teammate2_Leg>();
			component.legIndex = i;
			component.SyncDotsData(legsData[i]);
			component.Initialize(this);
			legs[i] = component;
		}
		for (int j = 0; j < essenceLegsData.Length; j++)
		{
			GameObject obj2 = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/2002/2002_Leg"), base.transform);
			obj2.SetActive(value: true);
			Teammate2_Leg component2 = obj2.GetComponent<Teammate2_Leg>();
			component2.legIndex = j;
			component2.SyncEssenceDotsData(essenceLegsData[j]);
			component2.Initialize(this, isEssenceLeg: true);
			essenceLegs[j] = component2;
		}
	}

	public void SyncLegsData(DynamicBuffer<LegsData> legsData)
	{
		for (int i = 0; i < legs.Length; i++)
		{
			Teammate2_Leg obj = legs[i];
			LegsData legsData2 = legsData[i];
			obj.SyncDotsData(legsData2);
		}
	}

	public void SyncEssenceLegsData(DynamicBuffer<EssenceLegsData> legsData)
	{
		for (int i = 0; i < essenceLegs.Length; i++)
		{
			Teammate2_Leg obj = essenceLegs[i];
			EssenceLegsData legsData2 = legsData[i];
			obj.SyncEssenceDotsData(legsData2);
		}
	}

	public void OnSpellDestroy()
	{
		Teammate2_Leg[] array = legs;
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
		array = essenceLegs;
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
		Object.Destroy(base.gameObject);
	}

	public void StartGhost()
	{
		Teammate2_Leg[] array = legs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].lr_Leg.material.SetFloat(UseGhostEffect, 1f);
		}
		array = essenceLegs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].lr_Leg.material.SetFloat(UseGhostEffect, 1f);
		}
	}

	public void StartFuse()
	{
		Teammate2_Leg[] array = legs;
		foreach (Teammate2_Leg obj in array)
		{
			obj.lr_Leg.material.SetInt(UseFuseShineEffect, 1);
			obj.lr_Leg.material.DOFloat(1f, FuseShineProcess, 1.3f);
			obj.lr_Shadow.gameObject.SetActive(value: false);
		}
		array = essenceLegs;
		foreach (Teammate2_Leg obj2 in array)
		{
			obj2.lr_Leg.material.SetInt(UseFuseShineEffect, 1);
			obj2.lr_Leg.material.DOFloat(1f, FuseShineProcess, 1.3f);
			obj2.lr_Shadow.gameObject.SetActive(value: false);
			obj2.EssencelegSetFuseState();
		}
	}

	public void HideOrShowLeg(bool IsLeIinvisible)
	{
		Teammate2_Leg[] array = legs;
		foreach (Teammate2_Leg obj in array)
		{
			obj.lr_Leg.material.SetInt(Transparency, IsLeIinvisible ? 1 : 0);
			obj.lr_Shadow.gameObject.SetActive(IsLeIinvisible);
		}
		array = essenceLegs;
		foreach (Teammate2_Leg obj2 in array)
		{
			obj2.lr_Leg.material.SetInt(Transparency, IsLeIinvisible ? 1 : 0);
			obj2.lr_Shadow.gameObject.SetActive(IsLeIinvisible);
			obj2.HideOrShow(IsLeIinvisible);
		}
	}

	public void ClearEssenceLegsEffect()
	{
		for (int i = 0; i < essenceLegs.Length; i++)
		{
			essenceLegs[i].ClearParticle();
		}
	}

	public void SuckOnce(int index)
	{
		legs[index].SuckOnce();
	}
}
