using Unity.Entities;
using UnityEngine;

internal class TeammateOwnerAuthoring : MonoBehaviour
{
	private class TeammateOwnerAuthoringBaker : Baker<TeammateOwnerAuthoring>
	{
		public override void Bake(TeammateOwnerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddBuffer<TeammateOwnerInfoBuffer>(entity);
			AddComponent<TeammateOwner>(entity);
		}
	}
}
