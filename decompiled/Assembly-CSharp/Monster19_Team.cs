using System;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class Monster19_Team : UnitBase
{
	public VariableFloat shootInterval;

	public float baseRotateSpeed;

	public float rotateSpeedPerChild;

	public float childRadius;

	public float speedCorrectThreshold;

	public int addHPPerChild;

	private float currentRotateValue;

	private List<Monster19> childs = new List<Monster19>();

	private float currentRotateSpeed;

	private float childDistanceToCenter;

	private float shootIntervalTimer;

	[Header("移动端削弱")]
	public float mobileRotateSpeedRatio;

	public float mobileMoveSpeedRatio;

	public override void EveryInitialCallback()
	{
		currentRotateSpeed = 0f;
		childDistanceToCenter = 0f;
		shootIntervalTimer = 0f;
		childs.Clear();
		shootInterval.RandomResult();
		base.Rigid.linearVelocity = Tool2D.GetDir() * base.MoveSpeed;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
		componentData2.IsVelocityDeclice = false;
		componentData2.unitCfg.maxHP = UnitConfig.map[componentData2.unitCfg.id].maxHP;
		componentData2.unitCfg.currentHP = componentData2.unitCfg.maxHP;
		componentData2.CanTouch = false;
		SetComponentData(componentData2);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		currentRotateValue += currentRotateSpeed * Time.deltaTime * (GameMgr.IsMobile_Static ? mobileRotateSpeedRatio : 1f);
		if (Mathf.Abs(base.Rigid.linearVelocity.sqrMagnitude - base.MoveSpeed * base.MoveSpeed) > speedCorrectThreshold)
		{
			base.Rigid.linearVelocity = base.Rigid.linearVelocity.normalized * base.MoveSpeed * (GameMgr.IsMobile_Static ? mobileRotateSpeedRatio : 1f);
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData);
		}
		shootIntervalTimer += Time.deltaTime;
		if (shootIntervalTimer >= shootInterval.result)
		{
			shootIntervalTimer = 0f;
			shootInterval.RandomResult();
			for (int i = 0; i < childs.Count; i++)
			{
				childs[i].Attack();
			}
		}
		for (int num = childs.Count - 1; num >= 0; num--)
		{
			if (!childs[num].gameObject.activeSelf)
			{
				childs.RemoveAt(num);
				Correct();
			}
		}
		if (childs.Count <= 1)
		{
			ChildBlast();
			DotsAnnouncedDeath();
			return;
		}
		for (int j = 0; j < childs.Count; j++)
		{
			float num2 = currentRotateValue + 360f / (float)childs.Count * (float)j;
			Vector3 point = base.transform.position + Tool2D.GetDir(num2) * childDistanceToCenter;
			childs[j].SetPoint(point, num2);
		}
	}

	private unsafe void Correct()
	{
		if (childs.Count > 1)
		{
			currentRotateSpeed = baseRotateSpeed + (float)childs.Count * rotateSpeedPerChild;
			childDistanceToCenter = childRadius / Mathf.Sin((float)(180 / childs.Count) * (MathF.PI / 180f));
			myPpt.unitCfg.maxHP += addHPPerChild;
			myPpt.unitCfg.currentHP += addHPPerChild;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.maxHP += addHPPerChild;
			componentData.unitCfg.currentHP += addHPPerChild;
			SetComponentData(componentData);
			base.CC_Self.radius = childDistanceToCenter + childRadius;
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData2.ColliderPtr;
			CapsuleGeometry geometry = colliderPtr->Geometry;
			geometry.Radius = myPpt.CC_Self.radius;
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData2);
		}
	}

	private void ChildBlast()
	{
		for (int i = 0; i < childs.Count; i++)
		{
			childs[i].SetBlast();
		}
	}

	public bool MonsterEnter(Monster19 monster19)
	{
		if (!base.gameObject.activeSelf)
		{
			return false;
		}
		childs.Add(monster19);
		Correct();
		return true;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		ChildBlast();
	}
}
