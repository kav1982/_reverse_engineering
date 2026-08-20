using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class PhysicsMassBehaviourAuthoring : MonoBehaviour
{
	private class Baker : Baker<PhysicsMassBehaviourAuthoring>
	{
		private bool HasPhysics()
		{
			if (!(GetComponent<Rigidbody>() != null))
			{
				return GetComponent<PhysicsBodyAuthoring>() != null;
			}
			return true;
		}

		public override void Bake(PhysicsMassBehaviourAuthoring authoring)
		{
			if (HasPhysics())
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				if (authoring.IsKinematic || authoring.SetVelocityToZero)
				{
					PhysicsMassOverride component = new PhysicsMassOverride
					{
						IsKinematic = (byte)(authoring.IsKinematic ? 1u : 0u),
						SetVelocityToZero = (byte)(authoring.SetVelocityToZero ? 1u : 0u)
					};
					AddComponent(entity, in component);
				}
				SetPhysicsMassBaking component2 = new SetPhysicsMassBaking
				{
					InfiniteInertiaX = authoring.InfiniteInertiaX,
					InfiniteInertiaY = authoring.InfiniteInertiaY,
					InfiniteInertiaZ = authoring.InfiniteInertiaZ,
					InfiniteMass = authoring.InfiniteMass
				};
				AddComponent(entity, in component2);
			}
		}
	}

	[TemporaryBakingType]
	public struct SetPhysicsMassBaking : IComponentData, IQueryTypeParameter
	{
		public bool InfiniteInertiaX;

		public bool InfiniteInertiaY;

		public bool InfiniteInertiaZ;

		public bool InfiniteMass;
	}

	[Header("Physics Mass")]
	public bool InfiniteInertiaX;

	public bool InfiniteInertiaY;

	public bool InfiniteInertiaZ;

	public bool InfiniteMass;

	[Header("Physics Mass Override")]
	public bool IsKinematic;

	public bool SetVelocityToZero;
}
