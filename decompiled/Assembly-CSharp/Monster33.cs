using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster33 : UnitBase
{
	public int recoveryHP;

	public float recoveryInterval;

	public float recoveryRadius;

	private List<Vector3> healPoints = new List<Vector3>();

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private float recoveryIntervalTimer;

	public GameObject pfb_recoveryEffect;

	public GameObject pfb_recoveryEffect_H;

	private List<Monster33_EffectFade> recoveryEffect = new List<Monster33_EffectFade>();

	public GameObject model;

	private float bumpTime = 1.25f;

	public float bumpTimeScaled = 1f;

	public float maxBumpTime = 0.5f;

	private float bumpTimer;

	public AudioSource audioSource;

	private Vector3 modelOriginPos;

	[Header("困难减速心脏")]
	public LayerMask checkLayer;

	public float checkInterval;

	private float checkIntervalTimer;

	public float slowDownRatio;

	public AIPattern pattern;

	[Header("和谐模式")]
	public SpriteRenderer sr;

	public Sprite sprite_H;

	private List<UnitDotsSyncSystem.DistanceHitResult> checkResult = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		audioSource.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void EveryInitialCallback()
	{
		modelOriginPos = model.transform.position;
		healPoints.Clear();
		healPoints.Add(base.transform.position);
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		recoveryEffect.Clear();
		for (int i = 0; i < healPoints.Count; i++)
		{
			Monster33_EffectFade monster33_EffectFade = ((!GameMgr.IsHarmony_Static) ? UnityEngine.Object.Instantiate(pfb_recoveryEffect, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster33_EffectFade>() : UnityEngine.Object.Instantiate(pfb_recoveryEffect_H, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster33_EffectFade>());
			recoveryEffect.Add(monster33_EffectFade);
			monster33_EffectFade.transform.position = new Vector3(healPoints[i].x, healPoints[i].y, healPoints[i].z);
		}
		if (GameMgr.IsHarmony_Static)
		{
			sr.sprite = sprite_H;
			audioSource.Stop();
			base.Anima.Play("Idle_H");
		}
		bumpTimer = bumpTime;
	}

	public override void Update()
	{
		if (bumpTimer < bumpTime)
		{
			bumpTimer += Time.deltaTime * (bumpTime / bumpTimeScaled);
		}
		else
		{
			bumpTimer = 0f;
		}
		Vector3 vector = model.transform.position - modelOriginPos;
		for (int i = 0; i < recoveryEffect.Count; i++)
		{
			recoveryEffect[i].spriteRenderer.material.SetFloat("_BumpTime", bumpTimer);
			if (recoveryEffect[i].VeinSurfaceRenderer.material.color != myPpt.BaseColor)
			{
				recoveryEffect[i].VeinSurfaceRenderer.material.color = myPpt.BaseColor;
			}
			for (int j = 0; j < recoveryEffect[i].Veins.Count; j++)
			{
				recoveryEffect[i].Veins[j].transform.position = Tool2D.IgnoreZPoint(vector + healPoints[i]) + new Vector3(0f, 0f, recoveryEffect[i].Veins[j].transform.position.z);
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		float num = Mathf.Lerp(maxBumpTime, bumpTimeScaled, myPpt.unitCfg.currentHP / myPpt.unitCfg.maxHP);
		base.Anima.speed = 1f / num;
		recoveryIntervalTimer += Time.deltaTime;
		if (recoveryIntervalTimer >= recoveryInterval)
		{
			recoveryIntervalTimer = 0f;
			for (int k = 0; k < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; k++)
			{
				Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[k];
				LocalTransform componentData = GetComponentData<LocalTransform>(entity);
				if ((base.transform.position - (Vector3)componentData.Position).sqrMagnitude < recoveryRadius * recoveryRadius && entity != myPpt.myEntity)
				{
					Vector3 vector2 = componentData.Position;
					float num2 = GetComponentData<UnitProperty_Dots>(entity).unitCfg.id - 103300;
					if ((!(num2 > 0f) || !(num2 < 100f)) && ((LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) || (!(vector2.x > roomCenterPoint.x + roomWidth / 2f) && !(vector2.x < roomCenterPoint.x - roomWidth / 2f) && !(vector2.y > roomCenterPoint.y + roomHeight / 2f) && !(vector2.y < roomCenterPoint.y - roomHeight / 2f))))
					{
						UnitDotsSyncSystem.UnitRecoveryHP(entity, recoveryHP, World.DefaultGameObjectInjectionWorld.EntityManager);
					}
				}
			}
		}
		if (pattern == AIPattern.Pattern1)
		{
			return;
		}
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= checkInterval))
		{
			return;
		}
		checkIntervalTimer = 0f;
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, recoveryRadius, GameConst.Filter_Friendly, checkResult);
		for (int l = 0; l < checkResult.Count; l++)
		{
			Entity entity2 = checkResult[l].entity;
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>(entity2);
			if (componentData2.unitCfg.IsSameCamp(UnitType.Player) && !componentData2.IsFly)
			{
				componentData2.SetMucus(checkInterval * 1.1f, slowDownRatio, 1f, changeColor: false);
				SetComponentData(componentData2, entity2);
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < healPoints.Count; i++)
		{
			recoveryEffect[i].GetComponent<Monster33_EffectFade>().StartFade(myPpt);
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "bump")
		{
			if (!GameMgr.IsHarmony_Static)
			{
				audioSource.Play();
			}
		}
		else
		{
			Debug.LogError(animaName);
		}
	}
}
