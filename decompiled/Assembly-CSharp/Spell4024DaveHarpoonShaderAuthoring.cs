using Unity.Entities;
using UnityEngine;

public class Spell4024DaveHarpoonShaderAuthoring : MonoBehaviour
{
	public class Spell4024HarpoonShaderBaker : Baker<Spell4024DaveHarpoonShaderAuthoring>
	{
		public override void Bake(Spell4024DaveHarpoonShaderAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4024HarpoonHideUnderGroundMat>(entity);
			AddComponent<Spell4024HarpoonOverlayColor>(entity);
		}
	}
}
