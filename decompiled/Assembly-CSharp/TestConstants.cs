using System.Net;
using Steamworks;

public class TestConstants
{
	public readonly CSteamID k_SteamId_Group_SteamUniverse = new CSteamID(103582791434672565uL);

	public readonly CSteamID k_SteamId_rlabrecque = new CSteamID(76561197991230424uL);

	public readonly AppId_t k_AppId_TeamFortress2 = new AppId_t(440u);

	public readonly AppId_t k_AppId_PieterwTestDLC = new AppId_t(110902u);

	public readonly AppId_t k_AppId_FreeToPlay = new AppId_t(343450u);

	public readonly PublishedFileId_t k_PublishedFileId_Champions = new PublishedFileId_t(280762427uL);

	public readonly SteamIPAddress_t k_IpAddress127_0_0_1 = new SteamIPAddress_t(IPAddress.Parse("127.0.0.1"));

	public const uint k_IpAddress127_0_0_1_uint = 2130706433u;

	public readonly SteamIPAddress_t k_IpAddress208_78_165_233 = new SteamIPAddress_t(IPAddress.Parse("208.78.165.233"));

	public const uint k_IpAddress208_78_165_233_uint = 3494815209u;

	public const ushort k_Port27015 = 27015;

	private static TestConstants _instance;

	public static TestConstants Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TestConstants();
			}
			return _instance;
		}
	}

	private TestConstants()
	{
	}
}
