using Server;
using UnityEngine;
using UnityEngine.UI;

public class UIVersion : MonoBehaviour
{
	public Text versionText;

	public static UIVersion Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
		versionText.text = "";
		if (!string.IsNullOrEmpty(VersionSO.Inst.Branch))
		{
			versionText.text = VersionSO.Inst.Branch + " - ";
		}
		versionText.text += VersionUtils.FormatToString(GameVersion.Final);
		if (GameMgr.IsMobile_Static)
		{
			versionText.fontSize = 35;
		}
	}
}
