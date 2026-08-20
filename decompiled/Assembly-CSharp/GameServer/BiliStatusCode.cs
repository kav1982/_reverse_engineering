namespace GameServer;

public enum BiliStatusCode
{
	Success = 0,
	GlobalGameIdError = -1,
	AccessTokenError = -2,
	SignError = -3,
	UserAgentError = -4,
	AccountNotLogged = -101,
	AccountBaned = -102,
	ParamsError = -400,
	UnknownAPI = -404,
	ServerInternalError = -500,
	RequireSpeedLimited = -503,
	PasswordUnsafe = -628,
	AccountDelete = 500100,
	AccountBeingDelete = 500101,
	AccountNoTester = 500001,
	AccountUnRealName = 500055,
	AccountThreeNos = 500054,
	AccountGuest = 500056,
	AccountAntiAddiction = 500057
}
