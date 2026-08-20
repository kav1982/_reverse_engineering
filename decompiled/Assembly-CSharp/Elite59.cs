using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Elite59 : UnitBase
{
	public enum Elite59State
	{
		Idle,
		CastingSpell
	}

	public enum Elite59Skills
	{
		FourDirLaser,
		MultiBrokenLine
	}

	public enum LaserSpawnDirection
	{
		Left,
		Up,
		Right,
		Down
	}

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private Elite59State eliteState;

	private Elite59Skills currentSkill;

	private UIEndlessEliteHpBar hpBar;

	public float SkillInterval;

	private float eliteTimer;

	private float skillTimer;

	public SpriteRenderer RightMark;

	public SpriteRenderer DownMark;

	public SpriteRenderer LeftMark;

	public SpriteRenderer UpMark;

	public ParticleSystem ChargeParticle;

	public List<SpriteRenderer> SkillLightSprites;

	[Header("四波单向切割激光")]
	public float FS_LaserDistance;

	public float FS_WaveInterval;

	public float FS_HLaserWaitTime;

	public float FS_HLaserWidth;

	public float FS_VLaserMoveSpeed;

	public float FS_VLaserLaserExistTime;

	public float FS_VLaserLaserWidth;

	public float FS_SkillStartTime;

	public float FS_VLaserDistanceRatio;

	private float FS_CurrentAngle;

	private bool FS_IsClockWiseRotate;

	private float FS_WaveTimer;

	private int FS_WaveCount;

	private bool FS_IsLaserWaveSpawn;

	[Header("多向虚线通行激光阵")]
	public float ML_SkillDuration;

	public int ML_LineCount;

	public int ML_OnewayLaserCount;

	public float ML_OnewayLaserMoveDistance;

	public float ML_OnewayCornerBaseDistance;

	public float ML_ShootInterval;

	public float ML_VLaserMoveSpeed;

	public float ML_VLaserLaserExistTime;

	public float ML_VLaserLaserWidth;

	public float ML_VLaserGroundTrailExistTime;

	[FormerlySerializedAs("ML_AfterSkillBonusWaitTimel")]
	public float ML_AfterSkillBonusWaitTime;

	private int ML_WaveCounter;

	private float ML_ShootTimer;

	private Vector3 ML_ShootBaseDirection;

	private void OnEnable()
	{
		RightMark.material.SetFloat(Progress, 0f);
		LeftMark.material.SetFloat(Progress, 0f);
		UpMark.material.SetFloat(Progress, 0f);
		DownMark.material.SetFloat(Progress, 0f);
		foreach (SpriteRenderer skillLightSprite in SkillLightSprites)
		{
			skillLightSprite.material.SetFloat(Progress, 0f);
		}
	}

	public override void SingleInitialCallback()
	{
		hpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		eliteState = Elite59State.Idle;
		currentSkill = Elite59Skills.MultiBrokenLine;
		FS_CurrentAngle = 90 * UnityEngine.Random.Range(0, 4);
		eliteTimer = 0f;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
	}

	public override void Update()
	{
		base.Update();
		UpdateState();
	}

	private void UpdateState()
	{
		eliteTimer += Time.deltaTime;
		switch (eliteState)
		{
		case Elite59State.Idle:
			if (eliteTimer >= SkillInterval)
			{
				EnterState(Elite59State.CastingSpell);
			}
			break;
		case Elite59State.CastingSpell:
			switch (currentSkill)
			{
			case Elite59Skills.FourDirLaser:
				if (eliteTimer < FS_SkillStartTime)
				{
					break;
				}
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
						EnterState(Elite59State.Idle);
					}
				}
				if (FS_WaveTimer >= FS_WaveInterval)
				{
					FS_WaveCount++;
					FS_WaveTimer = 0f;
					FS_IsLaserWaveSpawn = false;
					FS_CurrentAngle += (FS_IsClockWiseRotate ? (-90f) : 90f);
				}
				break;
			case Elite59Skills.MultiBrokenLine:
				if (ML_ShootTimer == 0f)
				{
					ChargeParticle.Play();
				}
				ML_ShootTimer += Time.deltaTime;
				if (ML_ShootTimer >= ML_ShootInterval)
				{
					ML_ShootTimer = 0f;
					SEMgr.Inst.elite59_Wave.PlaySE();
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite59_Burst", base.transform.position.IgnoreZ(), 5f);
					int num = (ML_LineCount - 2) * 180 / ML_LineCount;
					for (int i = 0; i < ML_LineCount; i++)
					{
						Vector3 dir = Tool2D.GetDir(ML_ShootBaseDirection, 360f / (float)ML_LineCount * ((float)i + (float)ML_WaveCounter / 2f));
						Vector3 vector = base.transform.position + dir * ML_OnewayCornerBaseDistance;
						for (int j = 0; j < ML_OnewayLaserCount; j++)
						{
							float num2 = 0.1f;
							Vector3 spawnPosition = vector + Tool2D.GetDir(-dir, (float)num / 2f) * ML_OnewayLaserMoveDistance * j;
							Boss52VerticalDrone component = GetVerticalLaserDrone(spawnPosition).GetComponent<Boss52VerticalDrone>();
							component.InitDroneData(10f, ML_VLaserLaserWidth, ML_VLaserLaserExistTime, ML_VLaserMoveSpeed, dir, 2f, initialHeightShiftTime: num2, groundDamageAreaRange: ML_VLaserLaserWidth, groundDamageAreaExistTimer: ML_VLaserGroundTrailExistTime, groundDamageAreaDamage: 10f, laserWidth: 0.1f, initialHeight: 0.2f, autoEndRecycle: true, motionType: VerticalLaserDroneMotion.normal, playSE: j == 0);
							component.ShootByOtherSource(myPpt.myEntity);
							if (j != 0)
							{
								spawnPosition = vector + Tool2D.GetDir(-dir, (float)(-num) / 2f) * ML_OnewayLaserMoveDistance * j;
								Boss52VerticalDrone component2 = GetVerticalLaserDrone(spawnPosition).GetComponent<Boss52VerticalDrone>();
								component2.InitDroneData(10f, ML_VLaserLaserWidth, ML_VLaserLaserExistTime, ML_VLaserMoveSpeed, dir, 2f, initialHeightShiftTime: num2, groundDamageAreaRange: ML_VLaserLaserWidth, groundDamageAreaExistTimer: ML_VLaserGroundTrailExistTime, groundDamageAreaDamage: 10f, laserWidth: 0.1f, initialHeight: 0.2f, autoEndRecycle: true, motionType: VerticalLaserDroneMotion.normal, playSE: false);
								component2.ShootByOtherSource(myPpt.myEntity);
							}
						}
					}
					ML_WaveCounter++;
				}
				if (eliteTimer >= ML_SkillDuration)
				{
					EnterState(Elite59State.Idle);
					eliteTimer -= ML_AfterSkillBonusWaitTime;
				}
				break;
			}
			break;
		}
	}

	private void EnterState(Elite59State state)
	{
		eliteState = state;
		eliteTimer = 0f;
		switch (state)
		{
		case Elite59State.Idle:
		{
			foreach (SpriteRenderer skillLightSprite in SkillLightSprites)
			{
				skillLightSprite.material.DOFloat(0f, Progress, 0.5f);
			}
			break;
		}
		case Elite59State.CastingSpell:
			ChargeParticle.Play();
			foreach (SpriteRenderer skillLightSprite2 in SkillLightSprites)
			{
				skillLightSprite2.material.DOFloat(1f, Progress, 0.5f);
			}
			if (currentSkill == Elite59Skills.MultiBrokenLine)
			{
				currentSkill = Elite59Skills.FourDirLaser;
				FS_CurrentAngle = 90 * UnityEngine.Random.Range(0, 4);
				FS_IsClockWiseRotate = !FS_IsClockWiseRotate;
				FS_IsLaserWaveSpawn = false;
				FS_WaveCount = 0;
				FS_WaveTimer = 0f;
			}
			else
			{
				currentSkill = Elite59Skills.MultiBrokenLine;
				ML_ShootTimer = ML_ShootInterval - 1f;
				ML_WaveCounter = 0;
				ML_ShootBaseDirection = Tool2D.GetDir((PlayerMgr.Inst.PlayerPoint - base.transform.position).IgnoreZ().normalized, 360f / (float)ML_LineCount / 2f);
			}
			break;
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
			component.ShootByOtherSource(myPpt.myEntity);
			if (i != 0)
			{
				vector4 = vector - vector3 * i;
				list.Add(vector4);
				Boss52HorizontalDrone component2 = GetHorizontalLaserDrone(vector4).GetComponent<Boss52HorizontalDrone>();
				component2.InitDroneData(0.02f, flag ? LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width : LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height, FS_HLaserWidth, 10f, initialDir, FS_HLaserWaitTime);
				component2.ShootByOtherSource(myPpt.myEntity);
			}
		}
		yield return new WaitForSeconds(FS_HLaserWaitTime);
	}
}
