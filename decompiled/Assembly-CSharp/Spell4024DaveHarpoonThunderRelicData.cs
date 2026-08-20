using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct Spell4024DaveHarpoonThunderRelicData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	public float Timer;

	public int Count;

	public float Damage;

	public float DamageRate;

	public Entity CurrentEntity;

	public Entity LastEntity;

	public float Radius;

	public float3 CurrentPos;

	public float3 LastPos;

	public Entity HarpoonEntity;

	public SpellComponentData HarpoonComp;

	public SpellConfigComponentData HarpoonConfig;

	public SpellElementEffectComponentData HarpoonEle;

	public SpellMovementComponentData HarpoonMove;

	public LocalTransform HarpoonTrans;

	public float3 HarpoonStartPos;
}
