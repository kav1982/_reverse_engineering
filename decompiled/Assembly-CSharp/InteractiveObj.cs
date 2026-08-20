using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

public class InteractiveObj : LayerCorrect
{
	public Vector3 interactOffset;

	public virtual void Select()
	{
	}

	public virtual void Unselect()
	{
	}

	public virtual void Interact()
	{
	}

	public unsafe Entity RegisterDotsInteractiveObj(UnityEngine.BoxCollider collider, InteractiveObjType type)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("HybirdInteractiveObj", base.transform.position);
		InteractiveObj_Dots componentData = entityManager.GetComponentData<InteractiveObj_Dots>(entity);
		componentData.type = type;
		componentData.uiOffset = interactOffset;
		entityManager.SetComponentData(entity, componentData);
		InteractiveObjRef componentData2 = entityManager.GetComponentData<InteractiveObjRef>(entity);
		componentData2.obj = this;
		entityManager.SetComponentData(entity, componentData2);
		PhysicsCollider collider2 = entityManager.GetComponentData<PhysicsCollider>(entity);
		collider2.MakeUnique(in entity, entityManager);
		Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)collider2.ColliderPtr;
		BoxGeometry geometry = colliderPtr->Geometry;
		geometry.Center = collider.center;
		geometry.Size = collider.size;
		colliderPtr->Geometry = geometry;
		if (base.gameObject.tag == "InteractiveObj")
		{
			DTool.SetCollider(in collider2, 33554432u);
		}
		else
		{
			DTool.SetCollider(in collider2, 512u);
		}
		entityManager.SetComponentData(entity, collider2);
		return entity;
	}

	public unsafe Entity RegisterDotsInteractiveObj(UnityEngine.CapsuleCollider collider, InteractiveObjType type)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("HybirdInteractiveObjCapsule", base.transform.position);
		InteractiveObj_Dots componentData = entityManager.GetComponentData<InteractiveObj_Dots>(entity);
		componentData.type = type;
		componentData.uiOffset = interactOffset;
		entityManager.SetComponentData(entity, componentData);
		InteractiveObjRef componentData2 = entityManager.GetComponentData<InteractiveObjRef>(entity);
		componentData2.obj = this;
		entityManager.SetComponentData(entity, componentData2);
		PhysicsCollider collider2 = entityManager.GetComponentData<PhysicsCollider>(entity);
		collider2.MakeUnique(in entity, entityManager);
		Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)collider2.ColliderPtr;
		CapsuleGeometry geometry = colliderPtr->Geometry;
		geometry.Radius = collider.radius;
		geometry.Vertex0 = collider.center + new Vector3(0f, 0f, collider.height / 2f);
		geometry.Vertex1 = collider.center - new Vector3(0f, 0f, collider.height / 2f);
		colliderPtr->Geometry = geometry;
		if (collider.isTrigger)
		{
			collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
		}
		if (base.gameObject.tag == "InteractiveObj")
		{
			DTool.SetCollider(in collider2, 33554432u);
		}
		else
		{
			DTool.SetCollider(in collider2, 512u);
		}
		entityManager.SetComponentData(entity, collider2);
		return entity;
	}

	public void SetDotsObjLayer(Entity entity, bool isOpen)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		PhysicsCollider pc = entityManager.GetComponentData<PhysicsCollider>(entity);
		if (isOpen)
		{
			DTool.SetCollider(in pc, 33554432u);
		}
		else
		{
			uint collideWith = 262656u;
			DTool.SetCollider(in pc, 256u, collideWith);
		}
		entityManager.SetComponentData(entity, pc);
	}

	public void CloseDotsObj(Entity entity)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.HasComponent<PhysicsCollider>(entity))
		{
			PhysicsCollider pc = entityManager.GetComponentData<PhysicsCollider>(entity);
			DTool.SetCollider(in pc, 0u, 0u);
			entityManager.SetComponentData(entity, pc);
		}
	}
}
