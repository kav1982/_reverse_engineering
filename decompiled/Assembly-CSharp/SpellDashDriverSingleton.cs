using Unity.Collections;
using Unity.Entities;

[ChunkSerializable]
public struct SpellDashDriverSingleton : IComponentData, IQueryTypeParameter
{
	public NativeHashSet<Entity> OnDashDriver;

	public float DashRemainingTime;

	public float TotalDashTime;

	public bool IsDashing => DashRemainingTime > 0f;

	public bool IsShooterDriving(Entity entity)
	{
		return OnDashDriver.Contains(entity);
	}

	public void ShooterDrive(Entity shooter)
	{
		OnDashDriver.Add(shooter);
	}

	public void ShooterDriveEnd(Entity shooter)
	{
		OnDashDriver.Remove(shooter);
	}

	public void SetDashTime(float duration)
	{
		TotalDashTime = duration;
		DashRemainingTime = TotalDashTime;
	}
}
