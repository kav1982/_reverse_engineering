using Unity.Entities;
using UnityEngine;

public class FrameAnimaAuthoring : MonoBehaviour
{
	private class Baker : Baker<FrameAnimaAuthoring>
	{
		public override void Bake(FrameAnimaAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			FrameAnima component = default(FrameAnima);
			component.xCount = authoring.xCount;
			component.yCount = authoring.yCount;
			component.duration = authoring.duration;
			component.IsRandomDir = authoring.IsRandomDir;
			component.IsLoopAnima = authoring.IsLoopAnima;
			if (authoring.ett_Root != null)
			{
				component.ett_Root = GetEntity(authoring.ett_Root, TransformUsageFlags.Dynamic);
			}
			AddComponent(entity, in component);
		}
	}

	public int xCount;

	public int yCount;

	public float duration;

	public GameObject ett_Root;

	public bool IsRandomDir;

	public bool IsLoopAnima;
}
