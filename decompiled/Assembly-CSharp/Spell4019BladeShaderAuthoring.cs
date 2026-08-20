using Unity.Entities;
using UnityEngine;

public class Spell4019BladeShaderAuthoring : MonoBehaviour
{
	public class Spell4019BladeShaderBaker : Baker<Spell4019BladeShaderAuthoring>
	{
		public override void Bake(Spell4019BladeShaderAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4019BladeHideUnderGroundMat>(entity);
			AddComponent<Spell4019BladeColorChangeMat>(entity);
		}
	}
}
