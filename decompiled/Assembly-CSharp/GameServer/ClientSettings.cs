namespace GameServer;

public static class ClientSettings
{
	public const int Version = 0;

	public const int MaxRetryTime = 3;

	public const int Timeout = 8;

	public static int BrandId;

	public static int ChannelId;

	public static int AreaId;

	public static string Uid = string.Empty;

	public static string Token = string.Empty;

	public static string[] Servers = new string[2] { "https://le4-qa-all-gs-magicraft.bilibiligame.net/", "https://le3-qa-all-gs-magicraft.bilibiligame.net/" };

	public static string SignKey = "P=Bo3PREtoZw_Dx0";

	public static bool Encrypt = true;

	public static string EncryptKey = "GbJf779pXcbt4L5gjW";
}
