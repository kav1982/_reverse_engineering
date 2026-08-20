using Unity.Physics;
using UnityEngine;

public class Destructible3 : UnitBase
{
	private enum UnitState
	{
		Stage3,
		Stage2,
		Stage1,
		Stage0
	}

	[Space(50f)]
	public UnityEngine.BoxCollider boxCollider;

	public MeshRenderer mr;

	public MeshRenderer mrShadow;

	public Sprite sprite_33;

	public Sprite sprite_23;

	public Sprite sprite_13;

	public Sprite sprite_03;

	public float efHeight1;

	public float efHeight2;

	public float efHeight3;

	private Vector3 originalBoxColliderSize;

	private UnitState state;

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
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_33.texture);
		mrShadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_33.texture);
		myPpt.correctType = LayerCorrectType.Coordinate;
		myPpt.CorrectLayerOnce();
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (base.CurrentHPRatio <= 0.3333f)
		{
			if (state == UnitState.Stage2)
			{
				state = UnitState.Stage1;
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_13.texture);
				mrShadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_13.texture);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", base.transform.position + new Vector3(0f, 0f, 0f - efHeight2), 2f);
				SEMgr.Inst.injured_D3_T0.PlaySE();
			}
		}
		else if (base.CurrentHPRatio <= 0.6666f && state == UnitState.Stage3)
		{
			state = UnitState.Stage2;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_23.texture);
			mrShadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_23.texture);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", base.transform.position + new Vector3(0f, 0f, 0f - efHeight1), 2f);
			SEMgr.Inst.injured_D3_T0.PlaySE();
		}
	}

	public unsafe override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (mr.material.GetTexture(GameConstManaged.shaderTextureIndex) != sprite_03.texture)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_03.texture);
			mrShadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_03.texture);
			info.stopAnnouncedDeath = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.currentHP = 1f;
			SetComponentData(componentData);
			state = UnitState.Stage0;
			boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)componentData2.ColliderPtr;
			BoxGeometry geometry = colliderPtr->Geometry;
			geometry.Size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);
			geometry.Center = new Vector3(0f, 0f, 0f);
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData2);
			myPpt.correctType = LayerCorrectType.TreeRoot;
			myPpt.CorrectLayerOnce();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", base.transform.position + new Vector3(0f, 0f, 0f - efHeight3), 2f);
			SEMgr.Inst.injured_D3_T0.PlaySE();
		}
	}
}
