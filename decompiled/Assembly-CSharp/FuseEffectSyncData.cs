using Unity.Entities;
using UnityEngine;

public struct FuseEffectSyncData
{
	public Entity MainEntity;

	public Entity SubEntity;

	public Spell3115ForceController FuseParticleController1;

	public Spell3115ForceController FuseParticleController2;

	public TeammateFusePairBuffer FuseData;

	public Vector3 FusePosition;

	public float FuseTimer;
}
