using Unity.Entities;

public struct Spell3007DamageCoolDownBuffer : IBufferElementData
{
	public Entity EnemyEntity;

	public float CoolDownTimer;
}
