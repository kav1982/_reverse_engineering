using UnityEngine;

public class SteamCustomMgrs : MonoBehaviour
{
	public SteamAchievementMgr steamAchievementMgr;

	public steamUserSateCustom steamUserSateCustom;

	public SteamFriendsCustom steamFriendsCustom;

	public SteamLeadBoardManager steamLeadBoardManager;

	public SteamScreenshotsTest steamScreenshotsTest;

	private void Awake()
	{
		steamAchievementMgr = GetComponent<SteamAchievementMgr>();
		steamUserSateCustom = GetComponent<steamUserSateCustom>();
		steamFriendsCustom = GetComponent<SteamFriendsCustom>();
		steamLeadBoardManager = GetComponent<SteamLeadBoardManager>();
		steamScreenshotsTest = GetComponent<SteamScreenshotsTest>();
		steamLeadBoardManager.Init();
		steamUserSateCustom.Init();
		steamFriendsCustom.Init();
		steamScreenshotsTest.Init();
	}
}
