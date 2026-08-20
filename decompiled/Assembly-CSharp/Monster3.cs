using System;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class Monster3 : UnitBase
{
	private enum UnitState
	{
		Idle,
		ExplosionBefore,
		Explosion,
		ExplosionAfter,
		Merge,
		MergeAfter
	}

	[Space(50f)]
	public Shadow shadow;

	public Transform tsf_Scale;

	public MeshRenderer mr_Body;

	public int summonedID;

	public int summonedCount;

	public float idleTime;

	public VariableFloat repositionRadius;

	[Header("Explosion")]
	public float explosionBeforeTime;

	public VariableFloat explosionBeforeSoundPitch;

	public VariableFloat explosionBeforeAnimaSpeed;

	public float explosionAfterTime;

	[Header("Merge")]
	public float mergeAfterTime;

	public VariableFloat mergeSize;

	public VariableFloat outlineWidth;

	[Header("Audio")]
	public AudioSource as_ExplosionBefore;

	public AudioSource as_Explosion;

	public AudioSource as_ChildEnter;

	private float originalCCRadius;

	private float originlaShadowScale;

	private UnitState state;

	private float explosionBeforeTimer;

	private float explosionAfterTimer;

	private float mergeAfterTimer;

	private int mergeChildCount;

	private List<Monster3_Child> childs = new List<Monster3_Child>();

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
		as_ExplosionBefore.volume = DataMgr.settingData.GetFinalSound();
		as_Explosion.volume = DataMgr.settingData.GetFinalSound();
		as_ChildEnter.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		originalCCRadius = myPpt.CC_Self.radius;
		originlaShadowScale = shadow.shadowScale;
		SetNavMeshArea(8);
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.Idle;
		explosionBeforeTimer = 0f;
		explosionAfterTimer = 0f;
		mergeAfterTimer = 0f;
		mergeChildCount = summonedCount;
		CorrectScale();
		childs.Clear();
		for (int i = 0; i < summonedCount; i++)
		{
			Monster3_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonedID).GetComponent<Monster3_Child>();
			component.transform.position = base.transform.position;
			component.SyncDotsPositionSafe();
			component.SetMother(this);
			childs.Add(component);
		}
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		myPpt.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		shadow.ShadowGO.SetActive(value: true);
		base.Anima.SetTrigger("Idle");
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		SetMove(Vector3.zero);
		switch (state)
		{
		case UnitState.Idle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= idleTime)
			{
				SetExplosionBefore();
			}
			break;
		case UnitState.ExplosionBefore:
		{
			explosionBeforeTimer += Time.deltaTime;
			float t = explosionBeforeTimer / explosionBeforeTime;
			as_ExplosionBefore.pitch = Mathf.Lerp(explosionBeforeSoundPitch.value1, explosionBeforeSoundPitch.value2, t);
			base.Anima.SetFloat("ExplosionBeforeSpeed", Mathf.Lerp(explosionBeforeAnimaSpeed.value1, explosionBeforeAnimaSpeed.value2, t));
			if (explosionBeforeTimer >= explosionBeforeTime)
			{
				explosionBeforeTimer = 0f;
				state = UnitState.ExplosionAfter;
				base.Anima.SetTrigger("Explosion");
				as_Explosion.Play();
				as_ExplosionBefore.Stop();
				as_ExplosionBefore.pitch = 1f;
				base.Anima.SetFloat("ExplosionBeforeSpeed", 1f);
			}
			break;
		}
		case UnitState.ExplosionAfter:
			explosionAfterTimer += Time.deltaTime;
			if (explosionAfterTimer >= explosionAfterTime)
			{
				explosionAfterTimer = 0f;
				state = UnitState.Merge;
				base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, repositionRadius, 8);
				SyncDotsPosition();
				for (int i = 0; i < childs.Count; i++)
				{
					childs[i].Merge();
				}
			}
			break;
		case UnitState.MergeAfter:
			mergeAfterTimer += Time.deltaTime;
			if (mergeAfterTimer >= mergeAfterTime)
			{
				mergeAfterTimer = 0f;
				SetExplosionBefore();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Explosion:
		case UnitState.Merge:
			break;
		}
	}

	private void SetExplosionBefore()
	{
		state = UnitState.ExplosionBefore;
		base.Anima.SetTrigger("ExplosionBefore");
		base.Anima.SetTrigger("RotateStop");
		as_ExplosionBefore.Play();
	}

	private unsafe void CorrectScale()
	{
		float num = Mathf.Lerp(mergeSize.value1, mergeSize.value2, (float)mergeChildCount / (float)summonedCount);
		tsf_Scale.localScale = Vector3.one * num;
		myPpt.CC_Self.radius = originalCCRadius * num;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData.ColliderPtr;
		CapsuleGeometry geometry = colliderPtr->Geometry;
		geometry.Radius = myPpt.CC_Self.radius;
		colliderPtr->Geometry = geometry;
		SetComponentData(componentData);
		shadow.ShadowGO.transform.localScale = Vector3.one * originlaShadowScale * num;
		mr_Body.material.SetFloat("_OutlineWidth", Mathf.Lerp(outlineWidth.value1, outlineWidth.value2, (float)mergeChildCount / (float)summonedCount));
	}

	public void ChildEnter(Monster3_Child child)
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			child.Hide();
			base.Anima.SetTrigger("ChildEnter");
			as_ChildEnter.Play();
			mergeChildCount++;
			if (mergeChildCount == childs.Count)
			{
				state = UnitState.MergeAfter;
			}
			CorrectScale();
			if (!myPpt.tsf_Layer.gameObject.activeSelf)
			{
				myPpt.tsf_Layer.gameObject.SetActive(value: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				SetComponentData(componentData);
				myPpt.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				shadow.ShadowGO.SetActive(value: true);
			}
		}
	}

	public void ChildTakeDamage(TakeDamageInfo_Dots childInfo)
	{
		UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, childInfo);
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "Explosion")
		{
			myPpt.tsf_Layer.gameObject.SetActive(value: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			SetComponentData(componentData);
			myPpt.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			shadow.ShadowGO.SetActive(value: false);
			for (int i = 0; i < childs.Count; i++)
			{
				childs[i].transform.position = base.transform.position;
				childs[i].SyncDotsPositionSafe();
				childs[i].Explosion();
			}
			mergeChildCount = 0;
			state = UnitState.ExplosionAfter;
			base.Anima.SetTrigger("Idle");
			base.Anima.SetTrigger("RotateStart");
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster3_Explosion", base.transform.position, 2f);
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		for (int i = 0; i < childs.Count; i++)
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(childs[i].myPpt.myEntity);
			componentData.SetBeHitColor();
			SetComponentData(componentData, childs[i].myPpt.myEntity);
		}
		if (state == UnitState.Idle)
		{
			SetExplosionBefore();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < childs.Count; i++)
		{
			childs[i].DotsAnnouncedDeath();
		}
	}
}
