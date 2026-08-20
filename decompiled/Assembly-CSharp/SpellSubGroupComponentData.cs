using Unity.Entities;
using UnityEngine.Scripting;

public class SpellSubGroupComponentData : IComponentData, IQueryTypeParameter
{
	public SpellShootGroup SubGroup;

	[Preserve]
	public SpellSubGroupComponentData()
	{
	}
}
