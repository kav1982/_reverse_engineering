using System;
using System.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

public class Monster3_Child : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	private enum MonsterState
	{
		Hide,
		Explosion,
		Merge
	}

	[Space(50f)]
	public Shadow shadow;

	public float collidWallDecelerateRatio;

	public float mergeSpeedRatio;

	public float mergeVelocityTo0Lerp;

	public VariableFloat cryDelay;

	public VariableFloat cryPitch;

	public VariableFloat cryInterval;

	public TrailRenderer tr_Tail;

	[Header("Audio")]
	public AudioSource as_Cry;

	private MonsterState state = MonsterState.Explosion;

	private Monster3 monster3;

	private float currentMoveSpeed;

	private float cryIntervalTimer;

	private int colorID;

	public Entity thisEntity { get; set; }

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
		as_Cry.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		SetNavMeshArea(32);
	}

	public override void EveryInitialCallback()
	{
		Hide();
		cryInterval.RandomResult();
		as_Cry.pitch = cryPitch.RandomResult();
		colorID = Shader.PropertyToID("_Color");
	}

	public override void Update()
	{
		if (tr_Tail.startColor != myPpt.BaseColor)
		{
			tr_Tail.startColor = myPpt.BaseColor;
			tr_Tail.endColor = myPpt.BaseColor;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (base.CC_Self.enabled)
		{
			cryIntervalTimer += Time.deltaTime;
			if (cryIntervalTimer >= cryInterval.result)
			{
				cryIntervalTimer = 0f;
				as_Cry.Play();
			}
		}
		switch (state)
		{
		case MonsterState.Hide:
		{
			base.transform.position = monster3.transform.position;
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			break;
		}
		case MonsterState.Explosion:
			if (base.Rigid.linearVelocity.sqrMagnitude != currentMoveSpeed * currentMoveSpeed)
			{
				base.Rigid.linearVelocity = base.Rigid.linearVelocity.normalized * currentMoveSpeed;
			}
			break;
		case MonsterState.Merge:
			base.Rigid.linearVelocity = Vector3.Lerp(base.Rigid.linearVelocity, Vector3.zero, mergeVelocityTo0Lerp * Time.deltaTime);
			CheckNavInfo();
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * mergeSpeedRatio);
			if (ToPointDistanceSqr(monster3.transform.position) < (moveThreshold + monster3.myPpt.CC_Self.radius) * (moveThreshold + monster3.myPpt.CC_Self.radius))
			{
				monster3.ChildEnter(this);
				state = MonsterState.Hide;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster3Child_Enter", base.transform.position, 2f);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
		componentData2.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData2);
	}

	public void SetMother(Monster3 monster3)
	{
		this.monster3 = monster3;
		myPpt.IsVelocityDeclice = false;
	}

	public void Hide()
	{
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		shadow.ShadowGO.SetActive(value: false);
		base.Rigid.linearVelocity = Vector3.zero;
		base.CurrentMotion = Vector3.zero;
	}

	public void Show()
	{
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		SetComponentData(componentData);
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		shadow.ShadowGO.SetActive(value: true);
	}

	public void Explosion()
	{
		Show();
		state = MonsterState.Explosion;
		currentMoveSpeed = base.MoveSpeed;
		base.Rigid.linearVelocity = Tool2D.GetDir() * currentMoveSpeed;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		StartCoroutine(Cry(cryDelay.RandomResult()));
	}

	private IEnumerator Cry(float delay)
	{
		yield return new WaitForSeconds(delay);
		as_Cry.Play();
	}

	public void Merge()
	{
		state = MonsterState.Merge;
		GetNavInfo(monster3.transform.position);
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		info.ignoreFloatText = true;
		monster3.ChildTakeDamage(info);
		info.immuneDamage = true;
		info.ignoreFloatText = false;
	}

	void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (DTool.GetColliderBlongsTo(collision.GetOtherEntity(myPpt.myEntity)) == 256)
		{
			currentMoveSpeed *= collidWallDecelerateRatio;
		}
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
