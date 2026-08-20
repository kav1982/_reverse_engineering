using Unity.Physics;
using UnityEngine;

public class Destructible3_2 : UnitBase
{
	private enum UnitState
	{
		Stage3,
		Stage2,
		Stage1,
		Stage0
	}

	[Space(50f)]
	public MeshRenderer mr;

	public MeshRenderer mr_Shadow;

	public Sprite[] sprite_0s;

	public Sprite[] sprite_1s;

	public Sprite[] sprite_2s;

	public Sprite[] sprite_3s;

	public float efHeight1;

	public float efHeight2;

	public float efHeight3;

	public Destructible3_2DeadEFType deadEFType;

	[Header("dead")]
	public UnityEngine.BoxCollider boxCollider;

	public Vector3 deadColliderSize;

	private Vector3 originalBoxColliderSize;

	private UnitState state;

	private int spriteIndex;

	private void CreateDeadEFAndSE(Vector3 createPoint)
	{
		switch (deadEFType)
		{
		case Destructible3_2DeadEFType.Leaf:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Leaf", createPoint, 2f);
			SEMgr.Inst.injured_D3_T3.PlaySE();
			break;
		case Destructible3_2DeadEFType.Wood:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_WoodLess", createPoint, 2f);
			SEMgr.Inst.dead_Tree.PlaySE();
			break;
		default:
			Debug.LogError(deadEFType);
			break;
		}
	}

	public override void SingleInitialCallback()
	{
		originalBoxColliderSize = boxCollider.size;
	}

	public unsafe override void EveryInitialCallback()
	{
		boxCollider.size = originalBoxColliderSize;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)componentData.ColliderPtr;
		BoxGeometry geometry = colliderPtr->Geometry;
		geometry.Size = boxCollider.size;
		geometry.Center = new Vector3(0f, 0f, 0f);
		colliderPtr->Geometry = geometry;
		SetComponentData(componentData);
		state = UnitState.Stage3;
		spriteIndex = Random.Range(0, sprite_3s.Length);
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_3s[spriteIndex].texture);
		mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_3s[spriteIndex].texture);
		myPpt.correctType = LayerCorrectType.Coordinate;
		myPpt.CorrectLayerOnce();
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.AfterTakeDamage_Dots(ref info);
		if (base.CurrentHPRatio <= 0.3333f)
		{
			if (state == UnitState.Stage2)
			{
				state = UnitState.Stage1;
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_1s[spriteIndex].texture);
				mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_1s[spriteIndex].texture);
				CreateDeadEFAndSE(base.transform.position + new Vector3(0f, 0f, 0f - efHeight2));
			}
		}
		else if (base.CurrentHPRatio <= 0.6666f && state == UnitState.Stage3)
		{
			state = UnitState.Stage2;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_2s[spriteIndex].texture);
			mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_2s[spriteIndex].texture);
			CreateDeadEFAndSE(base.transform.position + new Vector3(0f, 0f, 0f - efHeight1));
		}
	}

	public unsafe override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (mr.material.GetTexture(GameConstManaged.shaderTextureIndex) != sprite_0s[spriteIndex].texture)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_0s[spriteIndex].texture);
			mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_0s[spriteIndex].texture);
			info.stopAnnouncedDeath = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.currentHP = 1f;
			SetComponentData(componentData);
			state = UnitState.Stage0;
			boxCollider.size = deadColliderSize;
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)componentData2.ColliderPtr;
			BoxGeometry geometry = colliderPtr->Geometry;
			geometry.Size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);
			geometry.Center = new Vector3(0f, 0f, 0f);
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData2);
			myPpt.correctType = LayerCorrectType.TreeRoot;
			myPpt.CorrectLayerOnce();
			CreateDeadEFAndSE(base.transform.position + new Vector3(0f, 0f, 0f - efHeight3));
		}
	}
}
