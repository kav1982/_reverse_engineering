public interface IHoldingSpell
{
	bool IsHolding { get; set; }

	bool IsSkipHolding { get; set; }

	float HoldingTime { get; set; }

	void StartHolding();

	void StopHolding();

	void HoldingUpdate();

	bool HoldingCondition()
	{
		return PlayerMgr.Inst.PlayerCtrller.isHoldMouse0;
	}

	bool NeedSkipHolding(SpellBase spell)
	{
		ShootCause shootCause = spell.SIP.ShootCause;
		if (!(shootCause is ShootCause.BySplit) && !(shootCause is ShootCause.BySpell))
		{
			return spell.SIP.ChargeStar != null;
		}
		return true;
	}
}
