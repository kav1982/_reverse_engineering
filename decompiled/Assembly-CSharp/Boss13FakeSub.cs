using System;
using UnityEngine;

public class Boss13FakeSub : MonoBehaviour
{
	public enum SubState
	{
		FadeIn,
		Attack,
		FadeOut,
		SwitchToStage2,
		SwitchToStage2Stay,
		SwitchToStage2FadeOut,
		SwitchToStage3,
		Escape
	}

	public GameObject root;

	public MeshRenderer shadowMR;

	public TestController controller;

	public SubState _state;

	public MeshRenderer[] meshRenderers;

	public ParticleSystem[] jetParticles;

	public Texture2D damagedTexture;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("\ufffdƶ\ufffd\ufffdͱ\ufffd\ufffd\ufffd")]
	public Transform modelRotateRoot;

	public Transform modelTsfRoot;

	public Transform modelFloatRoot;

	public Transform modelTiltRoot;

	public Transform shadowRotateRoot;

	public Transform shadowTsfRoot;

	public Transform shadowTiltRoot;

	public Vector3 lookDir;

	[Header("ת\ufffd\u05f6\ufffd")]
	public float toStage2Time;

	public AnimationCurve toStage2DistanceCurve;

	public float toStage2FadeTime;

	public float toStage2FadeOutMoveSpeed;

	public float switchStageTime;

	public float switchStageHeight;

	public AnimationCurve heightCurve;

	public float switchStageDistance;

	public AnimationCurve distanceCurve;

	public float switchStageAngle;

	public AnimationCurve angleCurve;

	private bool dialogueActive;

	[Header("ɨ\ufffd\ufffd\ufffd\ufffd\ufffd")]
	public float shadowFadeTime;

	public float strafeMoveSpeed;

	public float bulletGenerateInterval;

	private float intervalTimer;

	public float strafeDuration;

	public VariableFloat strafeAreaRadius;

	public Vector3 strafeMoveDir;

	public float rotateSpeed;

	public float strafeHeight;

	public Vector3 strafeGeneratePos;

	public int strafeAmount;

	[Header("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
	public float deadStageTime;

	public float deadDistance;

	public float deadHeight;

	public float deadAngle;

	public AnimationCurve deadHeightCurve;

	public AnimationCurve deadDistanceCurve;

	public AnimationCurve deadAngleCurve;

	private bool setModeInitialize;

	[Header("\ufffd\ufffdЧ")]
	public AudioSource AS_BackGround;

	public SubState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		AS_BackGround.volume = DataMgr.settingData.GetFinalSound();
	}

	public void SetMode(int typeIndex)
	{
		if (typeIndex <= 1)
		{
			modelTsfRoot.gameObject.SetActive(value: false);
		}
		else
		{
			modelTsfRoot.gameObject.SetActive(value: true);
		}
		switch (typeIndex)
		{
		case 0:
			base.transform.position = strafeGeneratePos;
			state = SubState.FadeIn;
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
			break;
		case 1:
			state = SubState.SwitchToStage2;
			base.transform.position = Boss13.Inst.transform.position;
			lookDir = Tool2D.GetDir();
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
			break;
		case 2:
			state = SubState.SwitchToStage3;
			base.transform.position = Boss13_Stage2.Inst.transform.position + new Vector3(switchStageDistance, 0f, 0f - switchStageHeight - 0.5f);
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
			break;
		case 3:
			state = SubState.Escape;
			base.transform.position = Boss13_Stage3.Inst.transform.position;
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
			break;
		}
		shadowMR.transform.localScale = new Vector3(Boss13.shadowMRScale.x, Boss13.shadowMRScale.y, 1f);
		shadowMR.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		shadowTsfRoot.position = Tool2D.IgnoreZPoint(base.transform.position, -2079.7f);
		Boss13.camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		shadowMR.material.SetTexture("_MainTex", Boss13.shadowRT);
		setModeInitialize = true;
		Update();
	}

	public void Update()
	{
		shadowMR.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		Boss13.camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		modelRotateRoot.localEulerAngles = new Vector3(0f, (0f - Mathf.Atan2(lookDir.y, lookDir.x)) * 57.29578f - 90f, 0f);
		shadowRotateRoot.localEulerAngles = modelRotateRoot.localEulerAngles;
		shadowTsfRoot.position = Tool2D.IgnoreZPoint(base.transform.position, -2079.7f);
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		if (setModeInitialize)
		{
			setModeInitialize = false;
			stateExistTime = 0f;
		}
		switch (state)
		{
		case SubState.FadeIn:
			if (changedState)
			{
				SEMgr.Inst.boss13DashBig.PlaySE();
			}
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f * stateExistTime / shadowFadeTime));
			base.transform.position += strafeMoveDir * strafeMoveSpeed * Time.deltaTime;
			intervalTimer += Time.deltaTime;
			if (intervalTimer > bulletGenerateInterval)
			{
				intervalTimer = 0f;
				Vector3 vector7 = base.transform.position + Tool2D.GetDir(strafeMoveDir, 90f) * strafeAreaRadius.RandomResult();
				Boss13StrafeBullet component3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13StrafeBullet", vector7 + new Vector3(0f, 0f, strafeHeight)).GetComponent<Boss13StrafeBullet>();
				component3.fallDir = Tool2D.IgnoreZV2ToV1Normal(vector7, component3.transform.position);
			}
			if (stateExistTime > shadowFadeTime)
			{
				state = SubState.Attack;
			}
			break;
		case SubState.Attack:
			base.transform.position += strafeMoveDir * strafeMoveSpeed * Time.deltaTime;
			intervalTimer += Time.deltaTime;
			if (intervalTimer > bulletGenerateInterval)
			{
				intervalTimer = 0f;
				Vector3 vector6 = base.transform.position + Tool2D.GetDir(strafeMoveDir, 90f) * strafeAreaRadius.RandomResult();
				Boss13StrafeBullet component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13StrafeBullet", vector6 + new Vector3(0f, 0f, strafeHeight)).GetComponent<Boss13StrafeBullet>();
				component2.fallDir = Tool2D.IgnoreZV2ToV1Normal(vector6, component2.transform.position);
			}
			if (Vector3.Dot(strafeMoveDir, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position)) < 0f)
			{
				strafeMoveDir = Tool2D.RotateTowardsAroundZAxis(strafeMoveDir, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), rotateSpeed * Time.deltaTime);
				lookDir = strafeMoveDir;
			}
			if (stateExistTime > strafeDuration)
			{
				state = SubState.FadeOut;
			}
			break;
		case SubState.FadeOut:
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f * (1f - stateExistTime / shadowFadeTime)));
			base.transform.position += strafeMoveDir * strafeMoveSpeed * Time.deltaTime;
			intervalTimer += Time.deltaTime;
			if (intervalTimer > bulletGenerateInterval)
			{
				intervalTimer = 0f;
				Vector3 vector2 = base.transform.position + Tool2D.GetDir(strafeMoveDir, 90f) * strafeAreaRadius.RandomResult();
				Boss13StrafeBullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13StrafeBullet", vector2 + new Vector3(0f, 0f, strafeHeight)).GetComponent<Boss13StrafeBullet>();
				component.fallDir = Tool2D.IgnoreZV2ToV1Normal(vector2, component.transform.position);
			}
			if (!(stateExistTime > shadowFadeTime))
			{
				break;
			}
			strafeAmount--;
			if (strafeAmount > 0)
			{
				Vector3 dir = Tool2D.GetDir();
				Vector3 vector3 = PlayerMgr.Inst.PlayerPoint - dir * 10f;
				Vector3 vector4 = PlayerMgr.Inst.PlayerCtrller.CurrentMotion + vector3;
				Vector3 vector5 = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(vector4 + dir * 7.5f) - dir * 7.5f;
				lookDir = dir;
				strafeGeneratePos = vector5;
				strafeMoveDir = dir;
				SetMode(0);
			}
			else
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				if (Boss13.Inst.gameObject.activeInHierarchy)
				{
					Boss13.Inst.isStrafing = false;
				}
				if (Boss13_Stage2.Inst != null && Boss13_Stage2.Inst.gameObject.activeInHierarchy)
				{
					Boss13_Stage2.Inst.isStrafing = false;
				}
			}
			break;
		case SubState.SwitchToStage2:
		{
			if (changedState)
			{
				SEMgr.Inst.boss13DashBig.PlaySE();
			}
			float time2 = stateExistTime / toStage2Time;
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f * (stateExistTime / toStage2FadeTime)));
			shadowMR.transform.localScale = new Vector3(8f * toStage2DistanceCurve.Evaluate(time2), 8f * toStage2DistanceCurve.Evaluate(time2), 1f);
			if (stateExistTime > toStage2Time)
			{
				state = SubState.SwitchToStage2Stay;
				Boss13.Inst.state = Boss13.MonsterState.DeadAnimation;
			}
			break;
		}
		case SubState.SwitchToStage2FadeOut:
		{
			float num = stateExistTime / toStage2Time;
			shadowMR.transform.localScale = new Vector3(8f * toStage2DistanceCurve.Evaluate(1f - num), 8f * toStage2DistanceCurve.Evaluate(1f - num), 1f);
			shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f * (1f - stateExistTime / toStage2FadeTime)));
			if (stateExistTime > toStage2FadeTime)
			{
				shadowMR.transform.localScale = new Vector3(8f, 8f, 1f);
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
		case SubState.SwitchToStage3:
		{
			ref Vector3 reference3 = ref varMgr.RegV3(0);
			if (changedState)
			{
				lookDir = Vector3.left;
				reference3 = Tool2D.IgnoreZPoint(Boss13_Stage2.Inst.transform.position);
				shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
				SEMgr.Inst.boss13DashBig.PlaySE();
				for (int k = 0; k < jetParticles.Length; k++)
				{
					jetParticles[k].Play();
				}
			}
			float time3 = stateExistTime / switchStageTime;
			float x2 = switchStageAngle * angleCurve.Evaluate(time3);
			Vector3 vector8 = new Vector3(switchStageDistance * distanceCurve.Evaluate(time3), 0f, (0f - switchStageHeight) * heightCurve.Evaluate(time3) - 0.5f);
			base.transform.position = reference3 + vector8;
			modelTiltRoot.localEulerAngles = new Vector3(x2, 0f, 0f);
			shadowTiltRoot.localEulerAngles = new Vector3(x2, 0f, 0f);
			if (stateExistTime > switchStageTime + 0.5f && !dialogueActive)
			{
				dialogueActive = true;
				ChangeStage();
			}
			break;
		}
		case SubState.Escape:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref bool reference2 = ref varMgr.RegBool(0);
			if (changedState)
			{
				reference = Tool2D.IgnoreZPoint(Boss13_Stage3.Inst.transform.position);
				shadowMR.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
				for (int i = 0; i < meshRenderers.Length; i++)
				{
					meshRenderers[i].material.SetTexture(GameConstManaged.baseMapIndex, damagedTexture);
				}
				for (int j = 0; j < jetParticles.Length; j++)
				{
					jetParticles[j].Play();
				}
			}
			if (stateExistTime > 0.1f && !reference2)
			{
				reference2 = true;
				SEMgr.Inst.boss13DashBig.PlaySE();
				if (!controller.skipDaveDialogue)
				{
					Debug.Log(DataMgr.selectedWorldData.daveKilledBoss);
					if (!DataMgr.selectedWorldData.IsDave)
					{
						DataMgr.selectedWorldData.daveKilledBoss = true;
						DataMgr.SaveSelectedWorldData();
					}
					else
					{
						GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(DataMgr.selectedWorldData.daveKilledBoss ? 313 : 311, (Action)delegate
						{
							DataMgr.selectedWorldData.daveKilledBoss = true;
							DataMgr.SaveSelectedWorldData();
						});
					}
				}
			}
			lookDir = Tool2D.RotateTowardsAroundZAxis(lookDir, Vector3.left, rotateSpeed * Time.deltaTime);
			float time = stateExistTime / deadStageTime;
			float x = deadAngle * deadAngleCurve.Evaluate(time);
			Vector3 vector = new Vector3(deadDistance * deadDistanceCurve.Evaluate(time), 0f, (0f - deadHeight) * deadHeightCurve.Evaluate(time) - 0.5f);
			base.transform.position = reference + vector;
			modelTiltRoot.localEulerAngles = new Vector3(x, 0f, 0f);
			shadowTiltRoot.localEulerAngles = new Vector3(x, 0f, 0f);
			if (stateExistTime > deadStageTime + 0.5f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
		case SubState.SwitchToStage2Stay:
			break;
		}
	}

	public void ChangeStage()
	{
		foreach (Boss13Stage3FollowMissile followMissile in Boss13Stage3FollowMissile.followMissiles)
		{
			followMissile.DotsAnnouncedDeath();
		}
		if (controller.skipDaveDialogue)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/501331", Tool2D.IgnoreZPoint(base.transform.position));
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			Boss13_Stage2.Inst.DotsAnnouncedDeath();
		}
		else if (DataMgr.selectedWorldData.IsDave)
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(309, NextStage);
		}
		else
		{
			NextStage();
		}
		void NextStage()
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/501331", Tool2D.IgnoreZPoint(base.transform.position));
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			Boss13_Stage2.Inst.DotsAnnouncedDeath();
		}
	}
}
