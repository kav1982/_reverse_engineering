using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct EssenceLegsData : IBufferElementData
{
	public float LegRadius;

	public float LegLerpValue;

	public float3 IdleLookPoint;

	public float3 CurrentEndPoint;

	public float3 HeadPos;

	public float3 MiddlePoint;

	public float3 EndPoint;

	public float3 AttackDir;

	public float3 AttackTarget;

	public float LegsFloatTimer;

	public float StabMiddlePointAngle;

	public float StabMiddlePointDistance;

	public float StabEndPointDistance;

	public float OverStabDistance;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsAttacking;

	[MarshalAs(UnmanagedType.U1)]
	public bool StabBack;

	public float EssenceAttackDuration;

	public float EssenceAttackTimer;

	public int FuseHeadIndex;

	public bool IsFuseLeg => FuseHeadIndex >= 0;

	public void ResetEssenceLegAttackData(ref GlobalRandom random, int chunkIndex)
	{
		StabBack = true;
		float max = math.pow(LegRadius, 0.35f);
		StabMiddlePointAngle = random.NextFloatByChunkIndex(chunkIndex, 105f, 180f);
		StabMiddlePointDistance = random.NextFloatByChunkIndex(chunkIndex, 3.5f, 4.5f) * random.NextFloatByChunkIndex(chunkIndex, 1f, max);
		StabEndPointDistance = random.NextFloatByChunkIndex(chunkIndex, 2f, 3f) * random.NextFloatByChunkIndex(chunkIndex, 1f, max);
		OverStabDistance = random.NextFloatByChunkIndex(chunkIndex, 1.5f, 2.2f) * random.NextFloatByChunkIndex(chunkIndex, 1f, max);
	}

	public void StartAttack()
	{
		EssenceAttackTimer = 0f;
		IsAttacking = true;
		StabBack = false;
		if (EssenceAttackDuration < 0.25f)
		{
			EssenceAttackDuration = 0.25f;
		}
	}
}
