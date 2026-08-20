using Unity.Entities;
using UnityEngine.Scripting;

public class WandConfigComponent : IComponentData, IQueryTypeParameter
{
	public WandConfig cfg;

	[Preserve]
	public WandConfigComponent()
	{
	}
}
