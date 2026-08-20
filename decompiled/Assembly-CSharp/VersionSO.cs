using Server;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/GameVersion")]
public class VersionSO : ScriptableObject
{
	public int Main;

	public int Minor;

	public int Pitch;

	public int Fix;

	[Header("分支名称")]
	[Tooltip("留空表示正式版")]
	public string Branch = "";

	private static VersionSO inst;

	public static VersionSO Inst
	{
		get
		{
			if (!inst)
			{
				return inst = ABResources.LoadAsset<VersionSO>("Version");
			}
			return inst;
		}
	}

	public bool Check()
	{
		int main = Main;
		if (main >= 0 && main <= 99)
		{
			main = Minor;
			if (main >= 0 && main <= 999)
			{
				main = Pitch;
				if (main >= 0 && main <= 99)
				{
					main = Fix;
					if (main >= 0)
					{
						return main <= 99;
					}
					return false;
				}
			}
		}
		return false;
	}

	public int AsInt()
	{
		return VersionUtils.ParseToInt($"{Main}.{Minor}.{Pitch}f{Fix}");
	}

	public string AsString()
	{
		return VersionUtils.FormatToString(AsInt());
	}

	public string AsStringWithBranch()
	{
		return (string.IsNullOrEmpty(Branch) ? "" : (Branch + " - ")) + VersionUtils.FormatToString(AsInt());
	}
}
