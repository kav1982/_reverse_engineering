using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Elite58MinePillar : MonoBehaviour
{
	private static readonly int GroundHiddenHeight = Shader.PropertyToID("_GroundHiddenHeight");

	private static readonly int IsEnableEffect = Shader.PropertyToID("_IsEnableEffect");

	private static readonly int Bloom = Animator.StringToHash("Bloom");

	private static readonly int End = Animator.StringToHash("End");

	public Animator Anima;

	public List<SpriteRenderer> PillarSprites;

	public List<GameObject> MineGroupsList;

	public Shadow ShadowScipt;

	public ShockParam LandShock;

	public float InitialHeight;

	public float HeightShiftPerMineGroup;

	private bool isLand;

	private bool isInitialize;

	private bool EndSkill;

	private float MinePillarLandDelay;

	private float MinePillarShootInterval;

	private float MineLockMoveSpeed;

	private Vector3 MineLockPosition;

	private float MinePillarLandSpeed;

	private float PillarLandDelayOpenTime;

	private int MineShootRingCount;

	private int BonusMineCountPerRing;

	private int SingleRingMineCount;

	private float RingBonusRadiusPerShoot;

	private float MineFinishShootStartDisappearTime;

	private int shootCount;

	private float shootTimer;

	private float MineExplosionRange;

	private float MineExplosionDamage;

	private float MineTriggerDelayExplosionTime;

	private float MineManaDrainPercent;

	private float MineExistTime;

	public ShockParam ShockParam;

	private bool useFixAngle;

	private float fixAngle;

	private bool useFixGravity;

	private float fixGravity;

	private float baseRadius;

	private void OnEnable()
	{
		isLand = false;
		isInitialize = false;
		EndSkill = false;
		shootTimer = 0f;
		shootCount = 0;
	}

	public void InitialPillarData(float landSpeed, float afterlandDelayOpenTime, int ringMineCount, int ringCount, int bonusminePerRing, float shootInterval, float radiusIncreasePerShoot, float startHideAfterFinishTime, float mineDamage, float mineRadius, float mineExistDuration, float mineDelayTriggerTime, float manaDrainPercent, float fixedAngle = -1f, float fixedGravity = -1f, float baseRadius = 0f)
	{
		MinePillarLandSpeed = landSpeed;
		PillarLandDelayOpenTime = afterlandDelayOpenTime;
		SingleRingMineCount = ringMineCount;
		MineShootRingCount = ringCount;
		BonusMineCountPerRing = bonusminePerRing;
		MinePillarShootInterval = shootInterval;
		RingBonusRadiusPerShoot = radiusIncreasePerShoot;
		MineFinishShootStartDisappearTime = startHideAfterFinishTime;
		MineExplosionDamage = mineDamage;
		MineExplosionRange = mineRadius;
		MineExistTime = mineExistDuration;
		MineTriggerDelayExplosionTime = mineDelayTriggerTime;
		MineManaDrainPercent = manaDrainPercent;
		this.baseRadius = baseRadius;
		useFixAngle = fixedAngle >= 0f;
		fixAngle = fixedAngle;
		useFixGravity = fixedGravity >= 0f;
		fixGravity = fixedGravity;
		foreach (SpriteRenderer pillarSprite in PillarSprites)
		{
			pillarSprite.material.SetFloat(GroundHiddenHeight, base.transform.position.y);
			pillarSprite.material.SetFloat(IsEnableEffect, 1f);
		}
		foreach (GameObject mineGroups in MineGroupsList)
		{
			mineGroups.SetActive(value: true);
		}
		isInitialize = true;
	}

	private void Update()
	{
		if (SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else
		{
			if (!isInitialize)
			{
				return;
			}
			if (!isLand)
			{
				float z = Mathf.Min(base.transform.position.z + MinePillarLandSpeed * Time.deltaTime, 0f);
				if (base.transform.position.z < 0f)
				{
					base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, z);
					return;
				}
				isLand = true;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_BombLand", base.transform.position.IgnoreZ());
				SEMgr.Inst.elite58PillarLand.PlaySE();
				CamController.Inst.SetShock(ShockParam, new Vector3(0f, -1f, 0f));
				return;
			}
			if (PillarLandDelayOpenTime > 0f)
			{
				PillarLandDelayOpenTime -= Time.deltaTime;
				if (PillarLandDelayOpenTime > 0f)
				{
					return;
				}
				Anima.SetTrigger(Bloom);
				shootTimer -= 0.5f;
			}
			if (shootCount < MineShootRingCount)
			{
				shootTimer += Time.deltaTime;
				if (shootTimer >= MinePillarShootInterval)
				{
					shootTimer -= MinePillarShootInterval;
					int num = Mathf.Min(shootCount, MineGroupsList.Count - 1);
					MineGroupsList[num].SetActive(value: false);
					int num2 = SingleRingMineCount + BonusMineCountPerRing * shootCount;
					float degree = Tool2D.GetDegree((LevelMgr.Inst.CurrentRoomCtrller.CenterPoint - base.transform.position).IgnoreZ().normalized);
					if (useFixAngle)
					{
						degree = fixAngle;
					}
					float num3 = (float)(360 / num2) / 2f * (float)num + degree;
					for (int i = 0; i < num2; i++)
					{
						int num4 = 360 / num2 * i;
						float targetRadius = baseRadius + RingBonusRadiusPerShoot * (float)(shootCount + 1);
						float num5 = InitialHeight + HeightShiftPerMineGroup * (float)num;
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_ManaDrainMine", base.transform.position.IgnoreZ() + new Vector3(0f, 0f, 0f - num5)).GetComponent<Elite58ManaDrainMine>().InitialMineData(MineExplosionDamage, MineExplosionRange, MineExistTime, MineTriggerDelayExplosionTime, MineManaDrainPercent, Tool2D.GetDir(num3 + (float)num4), targetRadius, num5, useFixGravity ? fixGravity : (-1f));
					}
					shootCount++;
				}
			}
			else
			{
				shootTimer += Time.deltaTime;
				if (shootTimer >= MineFinishShootStartDisappearTime && !EndSkill)
				{
					EndSkill = true;
					Anima.SetTrigger(End);
					StartCoroutine(EndPillar(1f));
					shootTimer = -999f;
				}
			}
		}
	}

	private IEnumerator EndPillar(float time)
	{
		yield return new WaitForSeconds(0.5f);
		base.transform.DOLocalMoveZ(2f, time - 0.5f);
		ShadowScipt.ShadowGO.SetActive(value: false);
		yield return new WaitForSeconds(time - 0.5f);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
