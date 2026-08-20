using Unity.Entities;

public struct Spell4025RuneSlashData : IComponentData, IQueryTypeParameter
{
	public bool IsSlashDone;

	public bool IsInitialize;

	public bool IsSpawnSplitSlash;

	public bool NeedSpawnAOESlash;
}
