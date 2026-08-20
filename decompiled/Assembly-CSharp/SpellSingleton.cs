using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

[ChunkSerializable]
[BurstCompile]
public struct SpellSingleton : IComponentData, IQueryTypeParameter, IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void Create_0000855C_0024PostfixBurstDelegate(out SpellSingleton result);

	internal static class Create_0000855C_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<Create_0000855C_0024PostfixBurstDelegate>(Create).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(out SpellSingleton result)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref SpellSingleton, void>)functionPointer)(ref result);
					return;
				}
			}
			Create_0024BurstManaged(out result);
		}
	}

	[NativeDisableContainerSafetyRestriction]
	public NativeHashMap<FixedString128Bytes, Entity> Prefabs;

	[NativeDisableContainerSafetyRestriction]
	public NativeHashMap<int, NativeHashMap<FixedString64Bytes, SpellEffect>> Effects;

	[NativeDisableContainerSafetyRestriction]
	public NativeHashMap<int, SpellConfig_Struct> Configs;

	[NativeDisableContainerSafetyRestriction]
	public NativeHashMap<Entity, SpellSpawnParamsStorage> SpellSpawnParamsStorage;

	public Entity UnfollowingEffectRequireBufferEntity;

	public Entity GlobalParticleBufferEntity;

	public float SpellTransparency;

	public float TeammateTransparency;

	[MarshalAs(UnmanagedType.U1)]
	public bool Harmonious;

	public SpellSpawnParams BuildSpellSpawnParamsPrototype(int spellId, float3 spawnPosition, float3 shootDirection)
	{
		SpellConfig_Struct spellConfig_Struct = Configs[spellId];
		SpellSpawnParams result = default(SpellSpawnParams);
		result.SpawnPosition = spawnPosition;
		result.ConfigComponentData = new SpellConfigComponentData
		{
			Id = spellId,
			Damage = new AttributeValue(spellConfig_Struct.damage),
			DamageInterval = (spellConfig_Struct.isDPS ? spellConfig_Struct.DPSDamageInterval : 0f),
			Duration = new AttributeValue(spellConfig_Struct.duration),
			Float1 = spellConfig_Struct.float1,
			Float2 = spellConfig_Struct.float2,
			Float3 = spellConfig_Struct.float3,
			Int1 = spellConfig_Struct.int1,
			Int2 = spellConfig_Struct.int2,
			Int3 = spellConfig_Struct.int3,
			Knockback = spellConfig_Struct.knockback,
			Level = spellConfig_Struct.level,
			Radius = new RadiusAttributeValue(spellConfig_Struct.radius),
			AbilityType = spellConfig_Struct.abilityType,
			CriticalChance = spellConfig_Struct.criticalChance,
			Scatter = spellConfig_Struct.angle
		};
		result.MovementComponentData = new SpellMovementComponentData
		{
			Speed = spellConfig_Struct.speed,
			Direction = shootDirection,
			Gravity = spellConfig_Struct.gravity
		};
		result.PrefabId = spellId / 10;
		result.SpellEfficiency = 1f;
		return result;
	}

	[BurstCompile]
	public readonly bool TryGetSpellEffectEntity(int spellPrefabId, in FixedString32Bytes effectName, SpellColorType color, out Entity entity)
	{
		color.ColorEnumToString(out var result);
		return TryGetSpellEffectEntity(spellPrefabId, in effectName, result, out entity);
	}

	[BurstCompile]
	public readonly bool TryGetSpellEffectEntity(int spellPrefabId, in FixedString32Bytes effectName, FixedString32Bytes colorName, out Entity entity)
	{
		if (Harmonious && colorName == "Monster" && Prefabs.TryGetValue($"{spellPrefabId}_{effectName}_Monster_H", out entity))
		{
			return true;
		}
		return Prefabs.TryGetValue($"{spellPrefabId}_{effectName}_{colorName}", out entity);
	}

	[AOT.MonoPInvokeCallback(typeof(Create_0000855C_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void Create(out SpellSingleton result)
	{
		Create_0000855C_0024BurstDirectCall.Invoke(out result);
	}

	public void Dispose()
	{
		if (Effects.IsCreated)
		{
			using NativeArray<int> nativeArray = Effects.GetKeyArray(Allocator.Temp);
			foreach (int item in nativeArray)
			{
				if (Effects[item].IsCreated)
				{
					Effects[item].Dispose();
				}
			}
			Effects.Dispose();
		}
		if (Prefabs.IsCreated)
		{
			Prefabs.Dispose();
		}
		if (Configs.IsCreated)
		{
			Configs.Dispose();
		}
		if (SpellSpawnParamsStorage.IsCreated)
		{
			SpellSpawnParamsStorage.Dispose();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void Create_0024BurstManaged(out SpellSingleton result)
	{
		result = default(SpellSingleton);
		result.Prefabs = new NativeHashMap<FixedString128Bytes, Entity>(256, Allocator.Persistent);
		result.Configs = new NativeHashMap<int, SpellConfig_Struct>(128, Allocator.Persistent);
		result.Effects = new NativeHashMap<int, NativeHashMap<FixedString64Bytes, SpellEffect>>(32, Allocator.Persistent);
		result.SpellSpawnParamsStorage = new NativeHashMap<Entity, SpellSpawnParamsStorage>(256, Allocator.Persistent);
	}
}
