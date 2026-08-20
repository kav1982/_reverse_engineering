using Unity.Entities;
using UnityEngine.Scripting;

[UpdateBefore(typeof(UnitTakeDamageGroup))]
public class UnitBaseSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public UnitBaseSystemGroup()
	{
	}
}
