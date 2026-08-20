namespace PlayerLogger.Events;

public class UnlockNPCLogger : InWorldEventModel
{
	public int npc_id;

	public override string GetEventName()
	{
		return "unlock_npc";
	}
}
