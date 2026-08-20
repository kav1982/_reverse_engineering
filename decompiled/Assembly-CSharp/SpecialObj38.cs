using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SpecialObj38 : LayerCorrect, IRoomObjExtraData, ITrap, IRoomCtrller, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public UnityEngine.CapsuleCollider thisCollider;

	public Transform tsf_Rotate;

	public MeshRenderer sr_Turntable;

	public LineRenderer lr_Base;

	public LineRenderer lr_Edge;

	public float lrNodeInterval;

	public float lrBaseExtraDis;

	public float rotateSpeed;

	public float radius;

	public float textureScalePerRadius;

	[Header("theme")]
	public Texture tex_T8;

	public Texture tex_T9;

	public UnityEngine.Material mat_BaseT8;

	public UnityEngine.Material mat_BaseT9;

	private List<Entity> innerPpts = new List<Entity>();

	private List<Entity> itemTs = new List<Entity>();

	private RoomController belongRoom;

	private string monster44_1Name = 104401 + "(Clone)";

	private string monster44_2Name = 104402 + "(Clone)";

	private string monster44_3Name = 104403 + "(Clone)";

	private bool isInvalid;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		thisCollider.radius = radius;
		sr_Turntable.transform.localScale = Vector3.one * 2f * radius;
		sr_Turntable.material.SetFloat("_Scale", radius * textureScalePerRadius);
		if (belongRoom.roomCfg.themeType == RoomThemeType.Theme8_Chapter4)
		{
			sr_Turntable.material.SetTexture("_SampleTex", tex_T8);
			lr_Base.material = mat_BaseT8;
		}
		else if (belongRoom.roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2)
		{
			sr_Turntable.material.SetTexture("_SampleTex", tex_T9);
			lr_Base.material = mat_BaseT9;
		}
		else
		{
			Debug.LogError(belongRoom.roomCfg.themeType);
		}
		rotateSpeed = ((UnityEngine.Random.Range(0, 2) == 0) ? rotateSpeed : (0f - rotateSpeed));
		int num = (int)(2f * radius * MathF.PI / lrNodeInterval);
		lr_Base.positionCount = num + 1;
		lr_Edge.positionCount = num + 1;
		for (int i = 0; i < num; i++)
		{
			lr_Edge.SetPosition(i, Tool2D.GetDir(360f / (float)num * (float)i) * radius);
			Vector3 v = Tool2D.GetDir(360f / (float)num * (float)i) * (radius + lrBaseExtraDis);
			lr_Base.SetPosition(i, Tool2D.IgnoreZPoint(v, 1.131f));
		}
		lr_Edge.SetPosition(num, lr_Edge.GetPosition(0));
		lr_Base.SetPosition(num, lr_Base.GetPosition(0));
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 272896u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (isInvalid)
		{
			return;
		}
		tsf_Rotate.Rotate(0f, 0f, rotateSpeed * Time.fixedDeltaTime);
		for (int num = innerPpts.Count - 1; num >= 0; num--)
		{
			if (!UnitDotsSyncSystem.EntityIsValid(innerPpts[num]))
			{
				innerPpts.RemoveAt(num);
			}
			else if (!UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(innerPpts[num]).IsFly)
			{
				LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(innerPpts[num]);
				Vector3 vector = Tool2D.IgnoreZV2ToV1(componentData.Position, base.transform.position);
				Vector3 dir = Tool2D.GetDir(vector, rotateSpeed * Time.fixedDeltaTime);
				componentData.Position += (float3)(dir - vector);
				UnitDotsSyncSystem.SetComponentData(componentData, innerPpts[num]);
			}
		}
		for (int i = 0; i < itemTs.Count; i++)
		{
			if (!UnitDotsSyncSystem.EntityIsValid(itemTs[i]))
			{
				itemTs.RemoveAt(i);
				continue;
			}
			LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(itemTs[i]);
			Vector3 vector2 = Tool2D.IgnoreZV2ToV1(componentData2.Position, base.transform.position);
			Vector3 dir2 = Tool2D.GetDir(vector2, rotateSpeed * Time.fixedDeltaTime);
			componentData2.Position += (float3)(dir2 - vector2);
			UnitDotsSyncSystem.SetComponentData(componentData2, itemTs[i]);
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2048u:
		case 8192u:
		{
			innerPpts.Add(other);
			UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
			componentData.ImmuneGroundAffectRegister();
			UnitDotsSyncSystem.SetComponentData(componentData, other);
			break;
		}
		case 262144u:
			if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(other))
			{
				innerPpts.Add(other);
				UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
				componentData.ImmuneGroundAffectRegister();
				UnitDotsSyncSystem.SetComponentData(componentData, other);
			}
			else
			{
				itemTs.Add(other);
			}
			break;
		}
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
		if (innerPpts.Contains(other))
		{
			if (UnitDotsSyncSystem.EntityIsValid(other))
			{
				UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
				componentData.ImmuneGroundAffectRegister();
				UnitDotsSyncSystem.SetComponentData(componentData, other);
			}
			innerPpts.Remove(other);
		}
		else if (itemTs.Contains(other))
		{
			itemTs.Remove(other);
		}
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 > 0f)
		{
			radius = data1;
		}
		if (data2 > 0f)
		{
			rotateSpeed = data2;
		}
	}

	public void SetTrapInvalid()
	{
		isInvalid = true;
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}
}
