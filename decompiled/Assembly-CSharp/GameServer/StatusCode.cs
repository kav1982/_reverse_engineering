namespace GameServer;

public enum StatusCode
{
	Success = 0,
	NoVerifyInfo = 101,
	DisabledAccount = 102,
	LoginTooOften = 103,
	UnknownArea = 201,
	ServerUnderMaintenance = 301,
	ServerNotUnderMaintenance = 302,
	ServerUnderMaintenanceDisableTester = 303,
	NoAIHelpToken = 402,
	VersionExists = 1001,
	LostFiles = 1002,
	VersionNotFound = 1003,
	AccountNotFound = 2001,
	RoleAlreadyCreated = 2002,
	CDKeyNotFound = 3001,
	CDKeyUnavailable = 3002,
	CDKeyAlreadyUsed = 3003,
	NoticeTitleExists = 4001,
	NoticeNotFound = 4002,
	CommodityNotFound = 5001,
	CommodityAlreadyHave = 5002,
	OrderNotFound = 6001,
	RedirectionNotFound = 7001,
	ServerGroupNotFound = 8001,
	ServerGroupExists = 8002
}
