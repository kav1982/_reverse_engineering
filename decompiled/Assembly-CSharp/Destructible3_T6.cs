using Unity.Collections;
using Unity.Physics;
using UnityEngine;

public class Destructible3_T6 : UnitBase
{
	[Space(50f)]
	public UnityEngine.BoxCollider boxCollider;

	public MeshRenderer mr;

	public MeshRenderer mr_Shadow;

	public Sprite[] sprite_Full;

	public Sprite[] sprite_Null;

	[Range(0f, 1f)]
	public float nullHPRatio;

	private Vector3 originalBoxColliderSize;

	private int spriteIndex;

	public override void SingleInitialCallback()
	{
		originalBoxColliderSize = boxCollider.size;
	}

	public override void EveryInitialCallback()
	{
		boxCollider.size = originalBoxColliderSize;
		spriteIndex = Random.Range(0, sprite_Full.Length);
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Full[spriteIndex].texture);
		mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Full[spriteIndex].texture);
		int value = Random.Range(0, 2) * 2 - 1;
		mr.material.SetInt("_FlipX", value);
		mr_Shadow.material.SetInt("_FlipX", value);
		myPpt.correctType = LayerCorrectType.Coordinate;
		myPpt.CorrectLayerOnce();
	}

	public unsafe override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.AfterTakeDamage_Dots(ref info);
		if (base.CurrentHPRatio <= nullHPRatio && mr.material.GetTexture(GameConstManaged.shaderTextureIndex) != sprite_Null[spriteIndex].texture)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Null[spriteIndex].texture);
			mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Null[spriteIndex].texture);
			boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);
			PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
			Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)componentData.ColliderPtr;
			BoxGeometry geometry = colliderPtr->Geometry;
			geometry.Size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);
			geometry.Center = new Vector3(0f, 0f, 0f);
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData);
			myPpt.correctType = LayerCorrectType.TreeRoot;
			myPpt.CorrectLayerOnce();
			ObjPoolMgr inst = ObjPoolMgr.Inst;
			FixedString128Bytes deadEF = myPpt.unitCfg.deadEF;
			inst.GetGO("Prefabs/EF/" + deadEF.ToString(), base.transform.position, 2f);
			SEMgr.Inst.injured_D3_T0.PlaySE();
		}
	}
}
