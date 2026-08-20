using Unity.Entities;
using UnityEngine;

public class Spell1019LineDataAuthoring : MonoBehaviour
{
	private class SSpell1019LineDataAuthoringBaker : Baker<Spell1019LineDataAuthoring>
	{
		public override void Bake(Spell1019LineDataAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1019LineData>(entity);
		}
	}
}
