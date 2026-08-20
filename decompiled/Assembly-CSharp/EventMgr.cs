using System;

public class EventMgr
{
	public static Action AncienBloodChange;

	public static Action ChaosCoreChange;

	public static Action MagicCrystalChange;

	public static Action GearChange;

	public static Action InputChange;

	public static Action PlayerDead;

	public static Action PlayerHPOrShiledChange;

	public static Action<float> PotionUse_Discount;

	public static Action ScarecrowChange;

	public static Action coinCountChange;

	public static Action DestroyAllTeammate;

	public static Action EndlessStageStart;

	public static Action EndlessStageClear;

	public static Action DotsSystemReset;

	public static Action LanguageChange;

	public static Action SoundVolumeChange;

	public static Action MusicVolumeChange;

	public static Action OnChangeResolution;

	public static Action ControlChange;

	public static Action SpellTransparencyChange;

	public static Action SafeModeStateChange;

	public static Action SteamConected;

	public static Action GetLeaderBoard;

	public static Action GetUserName;

	public static Action GetMyRank;

	public static Action GetLeaderBoardFail;

	public static Action FriendsUserStateUpdate;

	public static Action FriendsUserStateUpdateComplete;

	public static Action FriendsUserStateUpdateStart;

	public static Action RoleItemChange;
}
