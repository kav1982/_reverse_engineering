using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;

public struct SpellComponentData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsSplitSpell;

	[MarshalAs(UnmanagedType.U1)]
	public bool FromPostSlot;

	[MarshalAs(UnmanagedType.U1)]
	public bool ReduceSizeWhenLifeEnd;

	[MarshalAs(UnmanagedType.U1)]
	public bool PlayHitSE;

	[MarshalAs(UnmanagedType.U1)]
	public bool CanThroughWall;

	[MarshalAs(UnmanagedType.U1)]
	public bool PlayHitSEOnFallGroundedTag;

	[MarshalAs(UnmanagedType.U1)]
	public bool DisableAutoCreateFallEffect;

	[MarshalAs(UnmanagedType.U1)]
	public bool DisableDefaultFallDamage;

	[MarshalAs(UnmanagedType.U1)]
	public bool DisableAutoClearFallGroundedTag;

	[MarshalAs(UnmanagedType.U1)]
	public bool DisableSplitEffect;

	[MarshalAs(UnmanagedType.U1)]
	public bool GlobalParticleHitEffect;

	[MarshalAs(UnmanagedType.U1)]
	public bool UseGlobalHitParticleWithSpellDirection;

	[MarshalAs(UnmanagedType.U1)]
	public bool EnableTriggerRedRune;

	[MarshalAs(UnmanagedType.U1)]
	public bool EnableConvertOverFlowCCToDamage;

	public Entity SubGroupEntity;

	public Entity SpellEffectEntity;

	public Entity TrailEffectEntity;

	public UnityObjectRef<GameObject> SpellEffectGameObject;

	public UnityObjectRef<GameObject> TrailEffectGameObject;

	public Entity Shooter;

	public Entity OwnerEntity;

	public UnityObjectRef<Wand> Wand;

	public int InShootIndex;

	public int PrefabId;

	public float SpellEfficiency;

	public LayerCorrectType TrailLayer;
}
