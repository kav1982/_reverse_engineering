using Unity.Entities;
using Unity.Mathematics;

public struct SpellHitTriggerComponentData : IEnableableComponent, IComponentData, IQueryTypeParameter
{
	public float SubGroupMp;

	public float Cooldown;

	public bool NeedTrigger;

	public float3 TriggerPoint;

	private float CooldownTimer;

	public bool CooldownOver => CooldownTimer <= 0f;

	public void ResetCooldown()
	{
		CooldownTimer = Cooldown;
	}

	public void UpdateCooldown(float step)
	{
		CooldownTimer -= step;
	}
}
