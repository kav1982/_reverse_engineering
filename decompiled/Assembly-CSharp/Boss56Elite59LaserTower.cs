using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite59LaserTower : MonoBehaviour
{
	private enum TLR_SimpleType
	{
		UpToDown,
		DownToUp,
		ToDownOrTop,
		ToCenter
	}

	private enum TLR_HardType
	{
		DoubleSideSmallHole
	}

	public enum LaserSpawnDirection
	{
		Left,
		Up,
		Right,
		Down
	}

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private float eliteTimer;

	private float skillTimer;

	public SpriteRenderer RightMark;

	public SpriteRenderer DownMark;

	public SpriteRenderer LeftMark;

	public SpriteRenderer UpMark;

	public ParticleSystem ChargeParticle;

	public List<SpriteRenderer> SkillLightSprites;

	public SpriteRenderer WaveRightMark;

	public SpriteRenderer WaveLeftMark;

	[Header("四波单向切割激光")]
	public float FS_LaserDistance;

	public float FS_WaveInterval;

	public float FS_HLaserWaitTime;

	public float FS_HLaserWidth;

	public float FS_VLaserDistanceRatio;

	private float FS_CurrentAngle;

	private bool FS_IsClockWiseRotate;

	private float FS_WaveTimer;

	private int FS_WaveCount;

	private bool FS_IsLaserWaveSpawn;

	private Entity shooterEntity;

	private float TLR_LaserWidth;

	private float TLR_LaserGroundAreaExistTime;

	private float TLR_LaserMoveSpeed;

	private float TLR_LaserLife;

	private float TLR_LaserRoadPercent;

	private float TLR_LaserSpawnInterval;

	private float TLR_LaserDistance;

	public float TLR_BonusRoadRatio;

	public float TLR_TinyHoleToPlayerMaxDistance;

	public float TLR_DelayStartTimer;

	public float TLR_SafeLaserDuration;

	private Vector3 TLR_SafeLaserDistance;

	private bool enterCombinePhase;

	private List<TLR_SimpleType> simpleMoveTypeList = new List<TLR_SimpleType>
	{
		TLR_SimpleType.UpToDown,
		TLR_SimpleType.ToDownOrTop,
		TLR_SimpleType.ToCenter,
		TLR_SimpleType.DownToUp
	};

	private int simpleTypeCounter;

	private List<TLR_HardType> HardMoveTypeList = new List<TLR_HardType> { TLR_HardType.DoubleSideSmallHole };

	private int hardTypeCounter;

	private void OnEnable()
	{
		RightMark.material.SetFloat(Progress, 0f);
		LeftMark.material.SetFloat(Progress, 0f);
		UpMark.material.SetFloat(Progress, 0f);
		DownMark.material.SetFloat(Progress, 0f);
		WaveRightMark.material.SetFloat(Progress, 0f);
		WaveLeftMark.material.SetFloat(Progress, 0f);
		foreach (SpriteRenderer skillLightSprite in SkillLightSprites)
		{
			skillLightSprite.material.SetFloat(Progress, 0f);
		}
		FS_CurrentAngle = 90 * UnityEngine.Random.Range(0, 4);
		eliteTimer = 0f;
		shooterEntity = Entity.Null;
		GeneralTool.ListShuffle(ref simpleMoveTypeList);
		simpleTypeCounter = 0;
		enterCombinePhase = false;
	}

	public void InitialData(Entity shooterEntity, float laserWidth, float laserGroundEffectExistTime, float laserMoveSpeed, float laserLife, float roadPercent, float roadSpawnInterval, float laserDistance)
	{
		this.shooterEntity = shooterEntity;
		TLR_LaserMoveSpeed = laserMoveSpeed;
		TLR_LaserWidth = laserWidth;
		TLR_LaserGroundAreaExistTime = laserGroundEffectExistTime;
		TLR_LaserLife = laserLife + TLR_SafeLaserDuration;
		TLR_LaserRoadPercent = roadPercent;
		TLR_LaserSpawnInterval = roadSpawnInterval;
		TLR_LaserDistance = laserDistance;
		TLR_SafeLaserDistance = new Vector3(TLR_LaserMoveSpeed * TLR_SafeLaserDuration, 0f, 0f);
	}

	public void CastLaserRoadSkill(bool shootFromLeft, bool isHardMode = false)
	{
		Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		float num = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		float num2 = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		int targetCount = Mathf.CeilToInt(num / TLR_LaserDistance);
		int blockRemainCount = 3;
		if (!enterCombinePhase)
		{
			enterCombinePhase = true;
			ChargeParticle.Play();
			foreach (SpriteRenderer skillLightSprite in SkillLightSprites)
			{
				skillLightSprite.material.DOFloat(1f, Progress, 0.5f);
			}
		}
		if (isHardMode)
		{
			StartCoroutine(ChangeMarkProgress(WaveLeftMark, 0.8f, 0.2f));
			StartCoroutine(ChangeMarkProgress(WaveRightMark, 0.8f, 0.2f));
			if (0 == 0)
			{
				Vector3 roomCornerPoint = Tool2D.GetRoomCornerPoint(MapCornerType.UpperLeft);
				Vector3 roomCornerPoint2 = Tool2D.GetRoomCornerPoint(MapCornerType.UpperRight);
				int shootCount = Mathf.CeilToInt(num / TLR_LaserDistance);
				bool shootLeftFirst = centerPoint.x >= PlayerMgr.Inst.PlayerPoint.x;
				float num3 = UnityEngine.Random.Range(TLR_TinyHoleToPlayerMaxDistance * 0.33f, TLR_TinyHoleToPlayerMaxDistance * 0.5f) * (float)((UnityEngine.Random.Range(0f, 1f) >= 0.5f) ? 1 : (-1));
				float num4 = 0.5f;
				float num5 = Mathf.Abs(PlayerMgr.Inst.PlayerPoint.x - centerPoint.x) / (num2 / 2f);
				if (num5 >= num4)
				{
					num3 *= 1f - (num5 - num4) / (1f - num4) * 0.8f;
				}
				float num6 = Mathf.Clamp(PlayerMgr.Inst.PlayerPoint.y + num3, centerPoint.y - num / 2f, centerPoint.y + num / 2f);
				num3 = UnityEngine.Random.Range(TLR_TinyHoleToPlayerMaxDistance * 0.6f, TLR_TinyHoleToPlayerMaxDistance * 0.7f) * (float)((UnityEngine.Random.Range(0f, 1f) >= 0.5f) ? 1 : (-1));
				float secondHoleHeight = Mathf.Clamp(num6 + num3, centerPoint.y - num / 2f + TLR_TinyHoleToPlayerMaxDistance, centerPoint.y + num / 2f - TLR_TinyHoleToPlayerMaxDistance);
				StartCoroutine(StartLR_ToDoubleTinyHole(roomCornerPoint, roomCornerPoint2, shootCount, shootLeftFirst, 0.7f, num6, secondHoleHeight, isFirstShoot: true));
				return;
			}
			throw new ArgumentOutOfRangeException();
		}
		TLR_SimpleType tLR_SimpleType = simpleMoveTypeList[simpleTypeCounter];
		StartCoroutine(ChangeMarkProgress(shootFromLeft ? WaveLeftMark : WaveRightMark, 0.8f, 0.2f));
		if (tLR_SimpleType == TLR_SimpleType.UpToDown || tLR_SimpleType == TLR_SimpleType.DownToUp)
		{
			tLR_SimpleType = ((PlayerMgr.Inst.PlayerPoint.y <= centerPoint.y) ? TLR_SimpleType.DownToUp : TLR_SimpleType.UpToDown);
		}
		if (tLR_SimpleType == TLR_SimpleType.ToCenter || tLR_SimpleType == TLR_SimpleType.ToDownOrTop)
		{
			tLR_SimpleType = ((Mathf.Abs(PlayerMgr.Inst.PlayerPoint.y - centerPoint.y) <= num / 3f) ? TLR_SimpleType.ToDownOrTop : TLR_SimpleType.ToCenter);
		}
		switch (tLR_SimpleType)
		{
		case TLR_SimpleType.UpToDown:
		{
			float num10 = ((PlayerMgr.Inst.PlayerPoint.y >= centerPoint.y + num / 4f) ? (TLR_LaserRoadPercent - 0.25f) : (TLR_LaserRoadPercent - 0.2f));
			int blockStartCount2 = Mathf.CeilToInt(num / TLR_LaserDistance * num10);
			Vector3 currentPos2 = (shootFromLeft ? Tool2D.GetRoomCornerPoint(MapCornerType.UpperLeft) : Tool2D.GetRoomCornerPoint(MapCornerType.UpperRight));
			StartCoroutine(StartLR_ToDown(currentPos2, 0, shootFromLeft, targetCount, blockStartCount2, blockRemainCount));
			break;
		}
		case TLR_SimpleType.DownToUp:
		{
			float num7 = ((PlayerMgr.Inst.PlayerPoint.y <= centerPoint.y - num / 4f) ? (TLR_LaserRoadPercent - 0.25f) : (TLR_LaserRoadPercent - 0.2f));
			int blockStartCount = Mathf.CeilToInt(num / TLR_LaserDistance * num7);
			Vector3 currentPos = (shootFromLeft ? Tool2D.GetRoomCornerPoint(MapCornerType.LowerLeft) : Tool2D.GetRoomCornerPoint(MapCornerType.LowerRight));
			StartCoroutine(StartLR_ToUp(currentPos, 0, shootFromLeft, targetCount, blockStartCount, blockRemainCount));
			break;
		}
		case TLR_SimpleType.ToDownOrTop:
		{
			float num8 = TLR_LaserRoadPercent - 0.2f;
			int num9 = Mathf.CeilToInt(num / TLR_LaserDistance / 2f * num8);
			if (num9 % 2 == 0)
			{
				num9++;
			}
			Vector3 startPos = (shootFromLeft ? Tool2D.GetRoomCornerPoint(MapCornerType.MiddleLeft) : Tool2D.GetRoomCornerPoint(MapCornerType.MiddleRight));
			StartCoroutine(StartLR_ToUpDownSide(startPos, num9, shootFromLeft, 0));
			break;
		}
		case TLR_SimpleType.ToCenter:
		{
			int targetCount2 = Mathf.CeilToInt(num / TLR_LaserDistance / 2f * (TLR_LaserRoadPercent + TLR_BonusRoadRatio));
			Vector3 startPosU = (shootFromLeft ? Tool2D.GetRoomCornerPoint(MapCornerType.UpperLeft) : Tool2D.GetRoomCornerPoint(MapCornerType.UpperRight));
			Vector3 startPosD = (shootFromLeft ? Tool2D.GetRoomCornerPoint(MapCornerType.LowerLeft) : Tool2D.GetRoomCornerPoint(MapCornerType.LowerRight));
			StartCoroutine(StartLR_ToCenter(startPosU, startPosD, targetCount2, shootFromLeft, 0));
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		simpleTypeCounter++;
		if (simpleTypeCounter >= simpleMoveTypeList.Count)
		{
			simpleTypeCounter = 0;
			GeneralTool.ListShuffle(ref simpleMoveTypeList);
		}
	}

	private IEnumerator StartLR_ToDown(Vector3 currentPos, int counter, bool shootFromLeft, int targetCount, int blockStartCount, int blockRemainCount)
	{
		if (counter > blockStartCount && blockRemainCount > 0)
		{
			blockRemainCount--;
		}
		else
		{
			Boss52VerticalDrone component = GetVerticalLaserDrone(currentPos + TLR_SafeLaserDistance * ((!shootFromLeft) ? 1 : (-1))).GetComponent<Boss52VerticalDrone>();
			component.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootFromLeft ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime, groundDamageAreaDamage: 10f);
			component.ShootByOtherSource(shooterEntity);
		}
		counter++;
		currentPos += new Vector3(0f, 0f - TLR_LaserDistance, 0f);
		yield return new WaitForSeconds(TLR_LaserSpawnInterval);
		if (counter < targetCount)
		{
			StartCoroutine(StartLR_ToDown(currentPos, counter, shootFromLeft, targetCount, blockStartCount, blockRemainCount));
		}
	}

	private IEnumerator StartLR_ToUp(Vector3 currentPos, int counter, bool shootFromLeft, int targetCount, int blockStartCount, int blockRemainCount)
	{
		if (counter > blockStartCount && blockRemainCount > 0)
		{
			blockRemainCount--;
		}
		else
		{
			Boss52VerticalDrone component = GetVerticalLaserDrone(currentPos + TLR_SafeLaserDistance * ((!shootFromLeft) ? 1 : (-1))).GetComponent<Boss52VerticalDrone>();
			component.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootFromLeft ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime, groundDamageAreaDamage: 10f);
			component.ShootByOtherSource(shooterEntity);
		}
		counter++;
		currentPos += new Vector3(0f, TLR_LaserDistance, 0f);
		yield return new WaitForSeconds(TLR_LaserSpawnInterval);
		if (counter < targetCount)
		{
			StartCoroutine(StartLR_ToUp(currentPos, counter, shootFromLeft, targetCount, blockStartCount, blockRemainCount));
		}
	}

	private IEnumerator StartLR_ToUpDownSide(Vector3 startPos, int targetCount, bool shootFromLeft, int currentShootCount)
	{
		Boss52VerticalDrone component = GetVerticalLaserDrone(startPos + new Vector3(TLR_SafeLaserDistance.x * (float)((!shootFromLeft) ? 1 : (-1)), TLR_LaserDistance * (float)currentShootCount, 0f)).GetComponent<Boss52VerticalDrone>();
		component.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootFromLeft ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime, groundDamageAreaDamage: 10f);
		component.ShootByOtherSource(shooterEntity);
		if (currentShootCount != 0)
		{
			Boss52VerticalDrone component2 = GetVerticalLaserDrone(startPos - new Vector3(TLR_SafeLaserDistance.x * (float)(shootFromLeft ? 1 : (-1)), TLR_LaserDistance * (float)currentShootCount, 0f)).GetComponent<Boss52VerticalDrone>();
			component2.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootFromLeft ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime, groundDamageAreaDamage: 10f);
			component2.ShootByOtherSource(shooterEntity);
		}
		currentShootCount++;
		yield return new WaitForSeconds(TLR_LaserSpawnInterval);
		if (currentShootCount < targetCount)
		{
			StartCoroutine(StartLR_ToUpDownSide(startPos, targetCount, shootFromLeft, currentShootCount));
		}
	}

	private IEnumerator StartLR_ToCenter(Vector3 startPosU, Vector3 startPosD, int targetCount, bool shootFromLeft, int currentShootCount)
	{
		Boss52VerticalDrone component = GetVerticalLaserDrone(startPosU - new Vector3(TLR_SafeLaserDistance.x * (float)(shootFromLeft ? 1 : (-1)), TLR_LaserDistance * (float)currentShootCount, 0f)).GetComponent<Boss52VerticalDrone>();
		component.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootFromLeft ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime, groundDamageAreaDamage: 10f);
		component.ShootByOtherSource(shooterEntity);
		Boss52VerticalDrone component2 = GetVerticalLaserDrone(startPosD + new Vector3(TLR_SafeLaserDistance.x * (float)((!shootFromLeft) ? 1 : (-1)), TLR_LaserDistance * (float)currentShootCount, 0f)).GetComponent<Boss52VerticalDrone>();
		component2.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootFromLeft ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime, groundDamageAreaDamage: 10f);
		component2.ShootByOtherSource(shooterEntity);
		currentShootCount++;
		yield return new WaitForSeconds(TLR_LaserSpawnInterval);
		if (currentShootCount < targetCount)
		{
			StartCoroutine(StartLR_ToCenter(startPosU, startPosD, targetCount, shootFromLeft, currentShootCount));
		}
	}

	private IEnumerator StartLR_ToDoubleTinyHole(Vector3 upLeftPos, Vector3 upRightPos, int shootCount, bool shootLeftFirst, float secondShootDelayTime, float firstHoleHeight, float secondHoleHeight, bool isFirstShoot)
	{
		Vector3 spawnPosition = (shootLeftFirst ? (upLeftPos - TLR_SafeLaserDistance) : (upRightPos + TLR_SafeLaserDistance));
		int value = Mathf.Abs(Mathf.RoundToInt((spawnPosition.y - (isFirstShoot ? firstHoleHeight : secondHoleHeight)) / TLR_LaserDistance));
		int num = 1;
		value = Mathf.Clamp(value, num, shootCount - num);
		for (int i = 0; i < shootCount; i++)
		{
			if (Mathf.Abs(value - i) > num)
			{
				Boss52VerticalDrone component = GetVerticalLaserDrone(spawnPosition).GetComponent<Boss52VerticalDrone>();
				component.InitDroneData(10f, TLR_LaserWidth, TLR_LaserLife, TLR_LaserMoveSpeed, new Vector3(shootLeftFirst ? 1 : (-1), 0f, 0f), 2f, initialHeightShiftTime: TLR_DelayStartTimer, groundDamageAreaRange: TLR_LaserWidth, groundDamageAreaExistTimer: TLR_LaserGroundAreaExistTime * 0.75f, groundDamageAreaDamage: 10f);
				component.ShootByOtherSource(shooterEntity);
			}
			spawnPosition -= new Vector3(0f, TLR_LaserDistance, 0f);
		}
		yield return new WaitForSeconds(secondShootDelayTime);
		if (isFirstShoot)
		{
			StartCoroutine(StartLR_ToDoubleTinyHole(upLeftPos, upRightPos, shootCount, !shootLeftFirst, secondShootDelayTime, firstHoleHeight, secondHoleHeight, isFirstShoot: false));
		}
	}

	public void Update()
	{
		UpdateFourDirLaserEffect();
	}

	private void UpdateFourDirLaserEffect()
	{
		FS_WaveTimer += Time.deltaTime;
		if (!FS_IsLaserWaveSpawn)
		{
			SpriteRenderer rightMark = RightMark;
			StartCoroutine(ChangeMarkProgress(GetLaserSpawnDirection(FS_CurrentAngle) switch
			{
				LaserSpawnDirection.Left => LeftMark, 
				LaserSpawnDirection.Up => UpMark, 
				LaserSpawnDirection.Right => RightMark, 
				LaserSpawnDirection.Down => DownMark, 
				_ => throw new ArgumentOutOfRangeException(), 
			}, 0.8f, 0.2f));
			SEMgr.Inst.elite59Alert.PlaySE();
			StartCoroutine(ShootLaserWave(GetLaserSpawnDirection(FS_CurrentAngle)));
			FS_IsLaserWaveSpawn = true;
			if (FS_WaveCount == 3)
			{
				FS_WaveCount = 0;
			}
		}
		if (FS_WaveTimer >= FS_WaveInterval)
		{
			FS_WaveCount++;
			FS_WaveTimer = 0f;
			FS_IsLaserWaveSpawn = false;
			FS_CurrentAngle += (FS_IsClockWiseRotate ? (-90f) : 90f);
		}
	}

	private LaserSpawnDirection GetLaserSpawnDirection(float angle)
	{
		int num = Mathf.RoundToInt(angle % 360f / 90f);
		if (num < 0)
		{
			num += 4;
		}
		return (LaserSpawnDirection)num;
	}

	private GameObject GetHorizontalLaserDrone(Vector3 spawnPosition)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_HorizontalLaserDrone", spawnPosition);
	}

	private GameObject GetVerticalLaserDrone(Vector3 spawnPosition)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", spawnPosition);
	}

	private IEnumerator ChangeMarkProgress(SpriteRenderer targetSprite, float markKeepDuration, float changeDuration)
	{
		targetSprite.material.DOFloat(1f, Progress, changeDuration);
		yield return new WaitForSeconds(markKeepDuration);
		targetSprite.material.DOFloat(0f, Progress, changeDuration);
	}

	private IEnumerator ShootLaserWave(LaserSpawnDirection type)
	{
		bool flag = type == LaserSpawnDirection.Left || type == LaserSpawnDirection.Right;
		int num = (flag ? LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height : LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width);
		float num2 = FS_LaserDistance;
		if (!flag)
		{
			num2 *= FS_VLaserDistanceRatio;
		}
		int num3 = Mathf.CeilToInt((float)num / 2f / num2) + 1;
		Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		Vector3 vector2 = new Vector3(flag ? 0f : UnityEngine.Random.Range((0f - num2) / 2f, num2 / 2f), flag ? UnityEngine.Random.Range((0f - num2) / 2f, num2 / 2f) : 0f, 0f);
		Vector3 vector3 = (flag ? new Vector3(0f, num2, 0f) : new Vector3(num2, 0f, 0f));
		Vector3 initialDir = Vector3.zero;
		List<Vector3> list = new List<Vector3>();
		switch (type)
		{
		case LaserSpawnDirection.Left:
			vector = Tool2D.GetRoomCornerPoint(MapCornerType.MiddleRight);
			initialDir = new Vector3(-1f, 0f, 0f);
			break;
		case LaserSpawnDirection.Right:
			vector = Tool2D.GetRoomCornerPoint(MapCornerType.MiddleLeft);
			initialDir = new Vector3(1f, 0f, 0f);
			break;
		case LaserSpawnDirection.Up:
			vector = Tool2D.GetRoomCornerPoint(MapCornerType.LowerCenter);
			initialDir = new Vector3(0f, 1f, 0f);
			break;
		case LaserSpawnDirection.Down:
			vector = Tool2D.GetRoomCornerPoint(MapCornerType.UpperCenter);
			initialDir = new Vector3(0f, -1f, 0f);
			break;
		}
		vector += vector2;
		for (int i = 0; i < num3; i++)
		{
			Vector3 vector4 = vector + vector3 * i;
			list.Add(vector4);
			Boss52HorizontalDrone component = GetHorizontalLaserDrone(vector4).GetComponent<Boss52HorizontalDrone>();
			component.InitDroneData(0.02f, flag ? LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width : LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height, FS_HLaserWidth, 10f, initialDir, FS_HLaserWaitTime);
			component.ShootByOtherSource(shooterEntity);
			if (i != 0)
			{
				vector4 = vector - vector3 * i;
				list.Add(vector4);
				Boss52HorizontalDrone component2 = GetHorizontalLaserDrone(vector4).GetComponent<Boss52HorizontalDrone>();
				component2.InitDroneData(0.02f, flag ? LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width : LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height, FS_HLaserWidth, 10f, initialDir, FS_HLaserWaitTime);
				component2.ShootByOtherSource(shooterEntity);
			}
		}
		yield return new WaitForSeconds(FS_HLaserWaitTime);
	}
}
