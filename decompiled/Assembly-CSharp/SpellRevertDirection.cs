using Unity.Entities;

public struct SpellRevertDirection : IComponentData, IQueryTypeParameter
{
	public bool Revert;
}
