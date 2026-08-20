using Unity.Entities;
using UnityEngine.Scripting;

[UpdateBefore(typeof(UnitTakeDamageGroup))]
public class LiquidSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public LiquidSystemGroup()
	{
	}
}
