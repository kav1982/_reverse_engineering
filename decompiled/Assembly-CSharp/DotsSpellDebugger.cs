using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class DotsSpellDebugger : MonoBehaviour
{
	private EntityQuery _query;

	private void Start()
	{
		if (!Application.isEditor)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		_query = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(SpellMovementComponentData), typeof(LocalTransform));
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		NativeArray<Entity> nativeArray = _query.ToEntityArray(Allocator.Temp);
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		foreach (Entity item in nativeArray)
		{
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(item);
			SpellMovementComponentData componentData2 = entityManager.GetComponentData<SpellMovementComponentData>(item);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(componentData.Position, componentData2.Direction + componentData.Position);
			Gizmos.DrawCube(componentData.Position, Vector3.one * 0.3f);
			if (componentData2.IsFallSpell)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawSphere(componentData2.FallTargetPosition, 0.3f);
			}
		}
	}
}
