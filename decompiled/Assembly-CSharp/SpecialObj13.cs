using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SpecialObj13 : LayerCorrect, IRoomCtrller, ITrap, IRoomObjExtraData, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public FourDir dir;

	public Transform tsf_Model;

	public MeshRenderer mr;

	public MeshRenderer mr_Frame;

	public Sprite sprite_Frame_StartSideHave;

	public Sprite sprite_Frame_TwoSideHave;

	public Sprite sprite_Mask_StartSideHave;

	public Sprite sprite_Mask_EndSideHave;

	public Sprite sprite_Mask_TwoSide;

	public float speed;

	public float otherBeltCheckRadius;

	public UnityEngine.BoxCollider thisCollider;

	private List<SpecialObj13> otherBelts = new List<SpecialObj13>();

	private List<Entity> standColliders = new List<Entity>();

	private RoomController belongRoom;

	private Vector3 motion;

	private bool isWork = true;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		mr.material.SetFloat("_Speed", speed);
		switch (dir)
		{
		case FourDir.Up:
			motion = Vector3.up * speed;
			break;
		case FourDir.Left:
			motion = Vector3.left * speed;
			break;
		case FourDir.Down:
			motion = Vector3.down * speed;
			break;
		case FourDir.Right:
			motion = Vector3.right * speed;
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		bool flag = false;
		bool flag2 = false;
		UnityEngine.Collider[] array = Physics.OverlapSphere(base.transform.position, otherBeltCheckRadius);
		for (int i = 0; i < array.Length; i++)
		{
			SpecialObj13 component = array[i].GetComponent<SpecialObj13>();
			if (!(component != null) || !(component != this))
			{
				continue;
			}
			otherBelts.Add(component);
			if (component.dir != dir)
			{
				continue;
			}
			switch (dir)
			{
			case FourDir.Up:
				if (component.transform.position == base.transform.position + new Vector3(0f, -1f, 0f))
				{
					flag = true;
				}
				else if (component.transform.position == base.transform.position + new Vector3(0f, 1f, 0f))
				{
					flag2 = true;
				}
				break;
			case FourDir.Right:
				if (component.transform.position == base.transform.position + new Vector3(-1f, 0f, 0f))
				{
					flag = true;
				}
				else if (component.transform.position == base.transform.position + new Vector3(1f, 0f, 0f))
				{
					flag2 = true;
				}
				break;
			case FourDir.Down:
				if (component.transform.position == base.transform.position + new Vector3(0f, 1f, 0f))
				{
					flag = true;
				}
				else if (component.transform.position == base.transform.position + new Vector3(0f, -1f, 0f))
				{
					flag2 = true;
				}
				break;
			case FourDir.Left:
				if (component.transform.position == base.transform.position + new Vector3(1f, 0f, 0f))
				{
					flag = true;
				}
				else if (component.transform.position == base.transform.position + new Vector3(-1f, 0f, 0f))
				{
					flag2 = true;
				}
				break;
			default:
				Debug.LogError(dir);
				break;
			}
		}
		if (!(flag && flag2))
		{
			if (flag)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Mask_StartSideHave.texture);
				mr_Frame.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Frame_StartSideHave.texture);
			}
			else if (flag2)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Mask_EndSideHave.texture);
				mr_Frame.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Frame_StartSideHave.texture);
				mr.material.SetInt("_FlipY", 1);
			}
			else
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Mask_TwoSide.texture);
				mr_Frame.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Frame_TwoSideHave.texture);
			}
		}
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

	public bool IsColliderStand(Entity checkEntity)
	{
		if (standColliders.Contains(checkEntity))
		{
			return true;
		}
		return false;
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		if (roomCtrller.roomCfg.isFlipped)
		{
			if (dir == FourDir.Left)
			{
				dir = FourDir.Right;
				tsf_Model.rotation = Tool2D.GetRotation(270f);
			}
			else if (dir == FourDir.Right)
			{
				dir = FourDir.Left;
				tsf_Model.rotation = Tool2D.GetRotation(90f);
			}
		}
	}

	public void SetTrapInvalid()
	{
		isWork = false;
		mr.material.SetFloat("_Speed", 0f);
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 > 0f)
		{
			speed = data1;
		}
	}

	private bool NextPosIsInWall(float3 nextPos)
	{
		return UnitDotsSyncSystem.pws.CastRay(new RaycastInput
		{
			Start = nextPos + new float3(0f, 0f, 10f),
			End = nextPos + new float3(0f, 0f, -10f),
			Filter = new CollisionFilter
			{
				BelongsTo = 1073741824u,
				CollidesWith = 256u,
				GroupIndex = 0
			}
		});
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
		if (!isWork || !UnitDotsSyncSystem.EntityIsValid(other))
		{
			return;
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2048u:
		case 8192u:
		{
			for (int j = 0; j < otherBelts.Count; j++)
			{
				if (otherBelts[j].IsColliderStand(other))
				{
					return;
				}
			}
			if (!standColliders.Contains(other) && UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				if (result.IsFly)
				{
					break;
				}
				standColliders.Add(other);
			}
			LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
			float3 float2 = componentData2.Position + (float3)motion * Time.deltaTime;
			if (!NextPosIsInWall(float2))
			{
				componentData2.Position = float2;
				UnitDotsSyncSystem.SetComponentData(componentData2, other);
			}
			break;
		}
		case 262144u:
		{
			for (int i = 0; i < otherBelts.Count; i++)
			{
				if (otherBelts[i].IsColliderStand(other))
				{
					return;
				}
			}
			if (!standColliders.Contains(other))
			{
				standColliders.Add(other);
			}
			LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
			float3 @float = componentData.Position + (float3)motion * Time.deltaTime;
			if (!NextPosIsInWall(@float))
			{
				componentData.Position = @float;
				UnitDotsSyncSystem.SetComponentData(componentData, other);
			}
			break;
		}
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
		if (standColliders.Contains(other))
		{
			standColliders.Remove(other);
		}
	}
}
