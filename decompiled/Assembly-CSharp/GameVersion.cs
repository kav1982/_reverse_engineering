using UnityEngine;

public class GameVersion : ScriptableObject
{
	public static int Build => VersionSO.Inst.AsInt();

	public static int HotFix => HotFixBundle.Loaded.MaxOrDefault((HotFixBundle e) => e.Version, 0);

	public static int Final => Mathf.Max(HotFix, Build);
}
