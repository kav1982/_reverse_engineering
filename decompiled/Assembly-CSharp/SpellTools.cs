using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public static class SpellTools
{
	public enum HitType
	{
		Unknown,
		SameCamp,
		IgnoreSpell,
		Unit,
		Brittleness,
		RollBall,
		Butterfly
	}

	private struct LazyAllocAttackableCollector : ICollector<Unity.Physics.RaycastHit>
	{
		private NativeList<Unity.Physics.RaycastHit> _list;

		private readonly Allocator _allocatorIfHit;

		private ComponentLookup<UnitProperty_Dots> _unitPptLookup;

		private ComponentLookup<SpellConfigComponentData> _spellCfgLookup;

		public bool EarlyOutOnFirstHit => false;

		public float MaxFraction { get; private set; }

		public int NumHits { get; private set; }

		public LazyAllocAttackableCollector(Allocator allocatorIfHit, ComponentLookup<UnitProperty_Dots> unitPptLookup, ComponentLookup<SpellConfigComponentData> spellCfgLookup)
		{
			_allocatorIfHit = allocatorIfHit;
			_unitPptLookup = unitPptLookup;
			_spellCfgLookup = spellCfgLookup;
			_list = default(NativeList<Unity.Physics.RaycastHit>);
			MaxFraction = 1f;
			NumHits = 0;
		}

		public bool AddHit(Unity.Physics.RaycastHit hit)
		{
			bool num = _unitPptLookup.HasComponent(hit.Entity);
			bool flag = _spellCfgLookup.HasComponent(hit.Entity);
			if (num || flag)
			{
				if (!_list.IsCreated)
				{
					_list = new NativeList<Unity.Physics.RaycastHit>(_allocatorIfHit);
				}
				_list.Add(in hit);
				NumHits++;
			}
			return true;
		}

		public NativeList<Unity.Physics.RaycastHit> Release()
		{
			NativeList<Unity.Physics.RaycastHit> list = _list;
			_list = default(NativeList<Unity.Physics.RaycastHit>);
			return list;
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DisableSpellTrigger_00002012_0024PostfixBurstDelegate(in PhysicsCollider collider);

	internal static class DisableSpellTrigger_00002012_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<DisableSpellTrigger_00002012_0024PostfixBurstDelegate>(DisableSpellTrigger).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in PhysicsCollider collider)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref PhysicsCollider, void>)functionPointer)(ref collider);
					return;
				}
			}
			DisableSpellTrigger_0024BurstManaged(in collider);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EnableSpellTrigger_00002013_0024PostfixBurstDelegate(in PhysicsCollider collider);

	internal static class EnableSpellTrigger_00002013_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<EnableSpellTrigger_00002013_0024PostfixBurstDelegate>(EnableSpellTrigger).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in PhysicsCollider collider)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref PhysicsCollider, void>)functionPointer)(ref collider);
					return;
				}
			}
			EnableSpellTrigger_0024BurstManaged(in collider);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DisableSpellReboundCollider_00002015_0024PostfixBurstDelegate(in PhysicsCollider collider);

	internal static class DisableSpellReboundCollider_00002015_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<DisableSpellReboundCollider_00002015_0024PostfixBurstDelegate>(DisableSpellReboundCollider).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in PhysicsCollider collider)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref PhysicsCollider, void>)functionPointer)(ref collider);
					return;
				}
			}
			DisableSpellReboundCollider_0024BurstManaged(in collider);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EnableSpellReboundCollider_00002016_0024PostfixBurstDelegate(in PhysicsCollider collider);

	internal static class EnableSpellReboundCollider_00002016_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<EnableSpellReboundCollider_00002016_0024PostfixBurstDelegate>(EnableSpellReboundCollider).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in PhysicsCollider collider)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref PhysicsCollider, void>)functionPointer)(ref collider);
					return;
				}
			}
			EnableSpellReboundCollider_0024BurstManaged(in collider);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DisableTeammateCollider_00002017_0024PostfixBurstDelegate(in PhysicsCollider collider);

	internal static class DisableTeammateCollider_00002017_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<DisableTeammateCollider_00002017_0024PostfixBurstDelegate>(DisableTeammateCollider).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in PhysicsCollider collider)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref PhysicsCollider, void>)functionPointer)(ref collider);
					return;
				}
			}
			DisableTeammateCollider_0024BurstManaged(in collider);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateSpellGlobalParticle_00002019_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in float3 position, in float scale, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, in SpellSingleton spellSingleton, in float3 velocity);

	internal static class CreateSpellGlobalParticle_00002019_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateSpellGlobalParticle_00002019_0024PostfixBurstDelegate>(CreateSpellGlobalParticle).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in float3 position, in float scale, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, in SpellSingleton spellSingleton, in float3 velocity)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref FixedString32Bytes, ref float3, ref float, ref SpellConfigComponentData, ref SpellComponentData, ref SpellSingleton, ref float3, void>)functionPointer)(ref cmd, chunkIndex, ref effectName, ref position, ref scale, ref spellConfig, ref spellData, ref spellSingleton, ref velocity);
					return;
				}
			}
			cmd.CreateSpellGlobalParticle_0024BurstManaged(chunkIndex, in effectName, in position, in scale, in spellConfig, in spellData, in spellSingleton, in velocity);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateSpellHitEffect_0000201A_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in SpellComponentData data, in float3 position, in float3 direction, float scale);

	internal static class CreateSpellHitEffect_0000201A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateSpellHitEffect_0000201A_0024PostfixBurstDelegate>(CreateSpellHitEffect).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in SpellComponentData data, in float3 position, in float3 direction, float scale)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref SpellSingleton, ref SpellConfigComponentData, ref SpellComponentData, ref float3, ref float3, float, void>)functionPointer)(ref cmd, chunkIndex, ref spellSingleton, ref config, ref data, ref position, ref direction, scale);
					return;
				}
			}
			cmd.CreateSpellHitEffect_0024BurstManaged(chunkIndex, in spellSingleton, in config, in data, in position, in direction, scale);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateOriginalSpellHitEffect_0000201B_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, int spellId, SpellColorType color, in float3 position, in float3 direction, float scale, out Entity inBufferEntity);

	internal static class CreateOriginalSpellHitEffect_0000201B_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateOriginalSpellHitEffect_0000201B_0024PostfixBurstDelegate>(CreateOriginalSpellHitEffect).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, int spellId, SpellColorType color, in float3 position, in float3 direction, float scale, out Entity inBufferEntity)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref SpellSingleton, int, SpellColorType, ref float3, ref float3, float, ref Entity, void>)functionPointer)(ref cmd, chunkIndex, ref spellSingleton, spellId, color, ref position, ref direction, scale, ref inBufferEntity);
					return;
				}
			}
			cmd.CreateOriginalSpellHitEffect_0024BurstManaged(chunkIndex, in spellSingleton, spellId, color, in position, in direction, scale, out inBufferEntity);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateSpecificSpellEffect_0000201C_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position, in float3 direction = default(float3), float scale = 1f);

	internal static class CreateSpecificSpellEffect_0000201C_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateSpecificSpellEffect_0000201C_0024PostfixBurstDelegate>(CreateSpecificSpellEffect).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position, in float3 direction = default(float3), float scale = 1f)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref FixedString32Bytes, ref FixedString32Bytes, ref SpellSingleton, ref SpellConfigComponentData, ref float3, ref float3, float, void>)functionPointer)(ref cmd, chunkIndex, ref effectName, ref colorName, ref spellSingleton, ref config, ref position, ref direction, scale);
					return;
				}
			}
			cmd.CreateSpecificSpellEffect_0024BurstManaged(chunkIndex, in effectName, in colorName, in spellSingleton, in config, in position, in direction, scale);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateSpecificSpellGlobalEffect_0000201D_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position);

	internal static class CreateSpecificSpellGlobalEffect_0000201D_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateSpecificSpellGlobalEffect_0000201D_0024PostfixBurstDelegate>(CreateSpecificSpellGlobalEffect).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref FixedString32Bytes, ref FixedString32Bytes, ref SpellSingleton, ref SpellConfigComponentData, ref float3, void>)functionPointer)(ref cmd, chunkIndex, ref effectName, ref colorName, ref spellSingleton, ref config, ref position);
					return;
				}
			}
			cmd.CreateSpecificSpellGlobalEffect_0024BurstManaged(chunkIndex, in effectName, in colorName, in spellSingleton, in config, in position);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateSimpleGlobalEffect_0000201E_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in SpellSingleton spellSingleton, in float3 position, float scale = 1f);

	internal static class CreateSimpleGlobalEffect_0000201E_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateSimpleGlobalEffect_0000201E_0024PostfixBurstDelegate>(CreateSimpleGlobalEffect).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in SpellSingleton spellSingleton, in float3 position, float scale = 1f)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref FixedString32Bytes, ref SpellSingleton, ref float3, float, void>)functionPointer)(ref cmd, chunkIndex, ref effectName, ref spellSingleton, ref position, scale);
					return;
				}
			}
			cmd.CreateSimpleGlobalEffect_0024BurstManaged(chunkIndex, in effectName, in spellSingleton, in position, scale);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateSpellEffect_0000201F_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellComponentData data, in SpellConfigComponentData config, in float3 position, in FixedString32Bytes EffectName, float scale, in float3 rotation);

	internal static class CreateSpellEffect_0000201F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateSpellEffect_0000201F_0024PostfixBurstDelegate>(CreateSpellEffect).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellComponentData data, in SpellConfigComponentData config, in float3 position, in FixedString32Bytes EffectName, float scale, in float3 rotation)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref SpellSingleton, ref SpellComponentData, ref SpellConfigComponentData, ref float3, ref FixedString32Bytes, float, ref float3, void>)functionPointer)(ref cmd, chunkIndex, ref spellSingleton, ref data, ref config, ref position, ref EffectName, scale, ref rotation);
					return;
				}
			}
			cmd.CreateSpellEffect_0024BurstManaged(chunkIndex, in spellSingleton, in data, in config, in position, in EffectName, scale, in rotation);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetSpell1014RainbowColor_00002020_0024PostfixBurstDelegate(int index, out FixedString32Bytes result);

	internal static class GetSpell1014RainbowColor_00002020_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetSpell1014RainbowColor_00002020_0024PostfixBurstDelegate>(GetSpell1014RainbowColor).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(int index, out FixedString32Bytes result)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<int, ref FixedString32Bytes, void>)functionPointer)(index, ref result);
					return;
				}
			}
			GetSpell1014RainbowColor_0024BurstManaged(index, out result);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void ActiveGravitationalForceCrystal_00002021_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, float damage, in float3 startPosition, in Entity GravitationalBufferEntity, in CurrentRoomEntitiesSingleton currentRoomEntities, in SpellAspect spell, in Entity sourceEntity);

	internal static class ActiveGravitationalForceCrystal_00002021_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<ActiveGravitationalForceCrystal_00002021_0024PostfixBurstDelegate>(ActiveGravitationalForceCrystal).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, float damage, in float3 startPosition, in Entity GravitationalBufferEntity, in CurrentRoomEntitiesSingleton currentRoomEntities, in SpellAspect spell, in Entity sourceEntity)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, float, ref float3, ref Entity, ref CurrentRoomEntitiesSingleton, ref SpellAspect, ref Entity, void>)functionPointer)(ref cmd, chunkIndex, damage, ref startPosition, ref GravitationalBufferEntity, ref currentRoomEntities, ref spell, ref sourceEntity);
					return;
				}
			}
			cmd.ActiveGravitationalForceCrystal_0024BurstManaged(chunkIndex, damage, in startPosition, in GravitationalBufferEntity, in currentRoomEntities, in spell, in sourceEntity);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void TryRecordRedRune_00002022_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellAspect spell, in Entity TargetEntity, in float3 position, in Entity RedRuneCheckBufferEntity, bool isCritical);

	internal static class TryRecordRedRune_00002022_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<TryRecordRedRune_00002022_0024PostfixBurstDelegate>(TryRecordRedRune).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellAspect spell, in Entity TargetEntity, in float3 position, in Entity RedRuneCheckBufferEntity, bool isCritical)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref SpellAspect, ref Entity, ref float3, ref Entity, bool, void>)functionPointer)(ref cmd, chunkIndex, ref spell, ref TargetEntity, ref position, ref RedRuneCheckBufferEntity, isCritical);
					return;
				}
			}
			cmd.TryRecordRedRune_0024BurstManaged(chunkIndex, in spell, in TargetEntity, in position, in RedRuneCheckBufferEntity, isCritical);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate HitType TryAttackEntity_00002029_0024PostfixBurstDelegate(in EntityCommandBuffer.ParallelWriter cmd, int sortKey, in Entity target, in TakeDamageInfo_Dots damage, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool recordTargetToSpell = true);

	internal static class TryAttackEntity_00002029_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<TryAttackEntity_00002029_0024PostfixBurstDelegate>(TryAttackEntity).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static HitType Invoke(in EntityCommandBuffer.ParallelWriter cmd, int sortKey, in Entity target, in TakeDamageInfo_Dots damage, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool recordTargetToSpell = true)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref Entity, ref TakeDamageInfo_Dots, ref ComponentLookup<UnitProperty_Dots>, ref ComponentLookup<SpellConfigComponentData>, bool, bool, HitType>)functionPointer)(ref cmd, sortKey, ref target, ref damage, ref unitLookup, ref spellConfigLookup, checkCamp, recordTargetToSpell);
				}
			}
			return cmd.TryAttackEntity_0024BurstManaged(sortKey, in target, in damage, in unitLookup, in spellConfigLookup, checkCamp, recordTargetToSpell);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate HitType GetEntityHitType_0000202A_0024PostfixBurstDelegate(in Entity target, in UnitType shooterType, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool checkSolidObj = false);

	internal static class GetEntityHitType_0000202A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetEntityHitType_0000202A_0024PostfixBurstDelegate>(GetEntityHitType).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static HitType Invoke(in Entity target, in UnitType shooterType, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool checkSolidObj = false)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<ref Entity, ref UnitType, ref ComponentLookup<UnitProperty_Dots>, ref ComponentLookup<SpellConfigComponentData>, bool, bool, HitType>)functionPointer)(ref target, ref shooterType, ref unitLookup, ref spellConfigLookup, checkCamp, checkSolidObj);
				}
			}
			return GetEntityHitType_0024BurstManaged(in target, in shooterType, in unitLookup, in spellConfigLookup, checkCamp, checkSolidObj);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetAttackableEntitiesInBox_0000202B_0024PostfixBurstDelegate(in float3 boxCenter, in float3 HalfBoxSize, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true, in Quaternion quaternion = default(Quaternion));

	internal static class GetAttackableEntitiesInBox_0000202B_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetAttackableEntitiesInBox_0000202B_0024PostfixBurstDelegate>(GetAttackableEntitiesInBox).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in float3 boxCenter, in float3 HalfBoxSize, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true, in Quaternion quaternion = default(Quaternion))
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref float3, ref float3, ref UnitType, bool, ref ComponentLookup<UnitProperty_Dots>, ref ComponentLookup<SpellConfigComponentData>, ref PhysicsWorldSingleton, ref NativeList<Entity>, bool, ref Quaternion, void>)functionPointer)(ref boxCenter, ref HalfBoxSize, ref selfCamp, containsBrittleness, ref unitPptLookup, ref spellConfigLookup, ref physics, ref entities, checkUnitCamp, ref quaternion);
					return;
				}
			}
			GetAttackableEntitiesInBox_0024BurstManaged(in boxCenter, in HalfBoxSize, in selfCamp, containsBrittleness, in unitPptLookup, in spellConfigLookup, in physics, ref entities, checkUnitCamp, in quaternion);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetAttackableEntitiesInRange_0000202C_0024PostfixBurstDelegate(in float3 position, in float radius, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true);

	internal static class GetAttackableEntitiesInRange_0000202C_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetAttackableEntitiesInRange_0000202C_0024PostfixBurstDelegate>(GetAttackableEntitiesInRange).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in float3 position, in float radius, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref float3, ref float, ref UnitType, bool, ref ComponentLookup<UnitProperty_Dots>, ref ComponentLookup<SpellConfigComponentData>, ref PhysicsWorldSingleton, ref NativeList<Entity>, bool, void>)functionPointer)(ref position, ref radius, ref selfCamp, containsBrittleness, ref unitPptLookup, ref SpellConfigLookup, ref physics, ref entities, checkUnitCamp);
					return;
				}
			}
			GetAttackableEntitiesInRange_0024BurstManaged(in position, in radius, in selfCamp, containsBrittleness, in unitPptLookup, in SpellConfigLookup, in physics, ref entities, checkUnitCamp);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool GetAttackablesInRaycast_0000202E_0024PostfixBurstDelegate(in float3 rayStart, in float3 rayEnd, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, out NativeList<Unity.Physics.RaycastHit> hitList, Allocator allocatorIfHit = Allocator.Temp);

	internal static class GetAttackablesInRaycast_0000202E_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetAttackablesInRaycast_0000202E_0024PostfixBurstDelegate>(GetAttackablesInRaycast).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static bool Invoke(in float3 rayStart, in float3 rayEnd, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, out NativeList<Unity.Physics.RaycastHit> hitList, Allocator allocatorIfHit = Allocator.Temp)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<ref float3, ref float3, ref UnitType, bool, ref ComponentLookup<UnitProperty_Dots>, ref ComponentLookup<SpellConfigComponentData>, ref PhysicsWorldSingleton, ref NativeList<Unity.Physics.RaycastHit>, Allocator, bool>)functionPointer)(ref rayStart, ref rayEnd, ref selfCamp, containsBrittleness, ref unitPptLookup, ref spellConfigLookup, ref physics, ref hitList, allocatorIfHit);
				}
			}
			return GetAttackablesInRaycast_0024BurstManaged(in rayStart, in rayEnd, in selfCamp, containsBrittleness, in unitPptLookup, in spellConfigLookup, in physics, out hitList, allocatorIfHit);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void CreateOtherCampFilter_IncludeSpell_0000202F_0024PostfixBurstDelegate(UnitType self, bool containsBrittleness, out CollisionFilter res);

	internal static class CreateOtherCampFilter_IncludeSpell_0000202F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CreateOtherCampFilter_IncludeSpell_0000202F_0024PostfixBurstDelegate>(CreateOtherCampFilter_IncludeSpell).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(UnitType self, bool containsBrittleness, out CollisionFilter res)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<UnitType, bool, ref CollisionFilter, void>)functionPointer)(self, containsBrittleness, ref res);
					return;
				}
			}
			CreateOtherCampFilter_IncludeSpell_0024BurstManaged(self, containsBrittleness, out res);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetAttackableEntitiesInSphereCast_00002030_0024PostfixBurstDelegate(in float3 rayStart, in float3 rayEnd, in float width, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<ColliderCastHit> hitList, bool checkUnitCamp = true, bool checkUnit = true);

	internal static class GetAttackableEntitiesInSphereCast_00002030_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetAttackableEntitiesInSphereCast_00002030_0024PostfixBurstDelegate>(GetAttackableEntitiesInSphereCast).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in float3 rayStart, in float3 rayEnd, in float width, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<ColliderCastHit> hitList, bool checkUnitCamp = true, bool checkUnit = true)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref float3, ref float3, ref float, ref UnitType, bool, ref ComponentLookup<UnitProperty_Dots>, ref ComponentLookup<SpellConfigComponentData>, ref PhysicsWorldSingleton, ref NativeList<ColliderCastHit>, bool, bool, void>)functionPointer)(ref rayStart, ref rayEnd, ref width, ref selfCamp, containsBrittleness, ref unitPptLookup, ref spellConfigLookup, ref physics, ref hitList, checkUnitCamp, checkUnit);
					return;
				}
			}
			GetAttackableEntitiesInSphereCast_0024BurstManaged(in rayStart, in rayEnd, in width, in selfCamp, containsBrittleness, in unitPptLookup, in spellConfigLookup, in physics, ref hitList, checkUnitCamp, checkUnit);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void RemoveCannotAttackSpell_00002031_0024PostfixBurstDelegate(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<SpellConfigComponentData> configLookup);

	internal static class RemoveCannotAttackSpell_00002031_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<RemoveCannotAttackSpell_00002031_0024PostfixBurstDelegate>(RemoveCannotAttackSpell).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<SpellConfigComponentData> configLookup)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref NativeList<Entity>, UnitType, ref ComponentLookup<SpellConfigComponentData>, void>)functionPointer)(ref entities, selfCamp, ref configLookup);
					return;
				}
			}
			RemoveCannotAttackSpell_0024BurstManaged(ref entities, selfCamp, in configLookup);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void RemoveSameCampUnitAndNotAttackUnit_00002032_0024PostfixBurstDelegate(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<UnitProperty_Dots> UnitPropertyLookup);

	internal static class RemoveSameCampUnitAndNotAttackUnit_00002032_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<RemoveSameCampUnitAndNotAttackUnit_00002032_0024PostfixBurstDelegate>(RemoveSameCampUnitAndNotAttackUnit).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<UnitProperty_Dots> UnitPropertyLookup)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref NativeList<Entity>, UnitType, ref ComponentLookup<UnitProperty_Dots>, void>)functionPointer)(ref entities, selfCamp, ref UnitPropertyLookup);
					return;
				}
			}
			RemoveSameCampUnitAndNotAttackUnit_0024BurstManaged(ref entities, selfCamp, in UnitPropertyLookup);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int CalculateSpellComplexity_00002034_0024PostfixBurstDelegate(SpellAbilityType spellAbility);

	internal static class CalculateSpellComplexity_00002034_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<CalculateSpellComplexity_00002034_0024PostfixBurstDelegate>(CalculateSpellComplexity).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static int Invoke(SpellAbilityType spellAbility)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<SpellAbilityType, int>)functionPointer)(spellAbility);
				}
			}
			return CalculateSpellComplexity_0024BurstManaged(spellAbility);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float GetLowFpsActiveThreshold_00002036_0024PostfixBurstDelegate(bool isMobile);

	internal static class GetLowFpsActiveThreshold_00002036_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetLowFpsActiveThreshold_00002036_0024PostfixBurstDelegate>(GetLowFpsActiveThreshold).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(bool isMobile)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<bool, float>)functionPointer)(isMobile);
				}
			}
			return GetLowFpsActiveThreshold_0024BurstManaged(isMobile);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float GetLowFpsActiveMinFps_00002037_0024PostfixBurstDelegate(bool isMobile);

	internal static class GetLowFpsActiveMinFps_00002037_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetLowFpsActiveMinFps_00002037_0024PostfixBurstDelegate>(GetLowFpsActiveMinFps).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(bool isMobile)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<bool, float>)functionPointer)(isMobile);
				}
			}
			return GetLowFpsActiveMinFps_0024BurstManaged(isMobile);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float GetLowFpsMaxBonusTimeScale_00002038_0024PostfixBurstDelegate(bool isMobile);

	internal static class GetLowFpsMaxBonusTimeScale_00002038_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetLowFpsMaxBonusTimeScale_00002038_0024PostfixBurstDelegate>(GetLowFpsMaxBonusTimeScale).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(bool isMobile)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<bool, float>)functionPointer)(isMobile);
				}
			}
			return GetLowFpsMaxBonusTimeScale_0024BurstManaged(isMobile);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float GetMaxOptimizeFPSThreshold_00002039_0024PostfixBurstDelegate(bool isMobile);

	internal static class GetMaxOptimizeFPSThreshold_00002039_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetMaxOptimizeFPSThreshold_00002039_0024PostfixBurstDelegate>(GetMaxOptimizeFPSThreshold).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(bool isMobile)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<bool, float>)functionPointer)(isMobile);
				}
			}
			return GetMaxOptimizeFPSThreshold_0024BurstManaged(isMobile);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate float GetLowFrameTimeScale_0000203A_0024PostfixBurstDelegate(float maxTimescale, float currentFPS, float lowFPSThreshold);

	internal static class GetLowFrameTimeScale_0000203A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetLowFrameTimeScale_0000203A_0024PostfixBurstDelegate>(GetLowFrameTimeScale).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static float Invoke(float maxTimescale, float currentFPS, float lowFPSThreshold)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					return ((delegate* unmanaged[Cdecl]<float, float, float, float>)functionPointer)(maxTimescale, currentFPS, lowFPSThreshold);
				}
			}
			return GetLowFrameTimeScale_0024BurstManaged(maxTimescale, currentFPS, lowFPSThreshold);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SpawnSuicideBug_0000203B_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, int SpawnCount, in SpellComponentData data, in SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, in TeammateData teammateData, in SpellElementEffectComponentData element, in SpellSingleton spellSingleton, float HpToDmgRatio, float ExplosionRange, float InstantKillRatio, bool ConstVoidEffect, float maxHp, float wormPower);

	internal static class SpawnSuicideBug_0000203B_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<SpawnSuicideBug_0000203B_0024PostfixBurstDelegate>(SpawnSuicideBug).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, int SpawnCount, in SpellComponentData data, in SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, in TeammateData teammateData, in SpellElementEffectComponentData element, in SpellSingleton spellSingleton, float HpToDmgRatio, float ExplosionRange, float InstantKillRatio, bool ConstVoidEffect, float maxHp, float wormPower)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, int, ref SpellComponentData, ref SpellMovementComponentData, ref SpellConfigComponentData, ref LocalTransform, ref TeammateData, ref SpellElementEffectComponentData, ref SpellSingleton, float, float, float, bool, float, float, void>)functionPointer)(ref CMD, chunkIndex, SpawnCount, ref data, ref movement, ref config, ref transform, ref teammateData, ref element, ref spellSingleton, HpToDmgRatio, ExplosionRange, InstantKillRatio, ConstVoidEffect, maxHp, wormPower);
					return;
				}
			}
			CMD.SpawnSuicideBug_0024BurstManaged(chunkIndex, SpawnCount, in data, in movement, in config, in transform, in teammateData, in element, in spellSingleton, HpToDmgRatio, ExplosionRange, InstantKillRatio, ConstVoidEffect, maxHp, wormPower);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetTeammateSacrificeRange_0000203F_0024PostfixBurstDelegate(float baseRange, in SpellConfigComponentData spellConfig, out float result);

	internal static class GetTeammateSacrificeRange_0000203F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetTeammateSacrificeRange_0000203F_0024PostfixBurstDelegate>(GetTeammateSacrificeRange).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(float baseRange, in SpellConfigComponentData spellConfig, out float result)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<float, ref SpellConfigComponentData, ref float, void>)functionPointer)(baseRange, ref spellConfig, ref result);
					return;
				}
			}
			GetTeammateSacrificeRange_0024BurstManaged(baseRange, in spellConfig, out result);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void TeammateRegister_00002043_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData);

	internal static class TeammateRegister_00002043_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<TeammateRegister_00002043_0024PostfixBurstDelegate>(TeammateRegister).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, ref Entity, ref Entity, ref TeammateData, void>)functionPointer)(ref CMD, chunkIndex, ref entity, ref ownerUnit, ref teammateData);
					return;
				}
			}
			CMD.TeammateRegister_0024BurstManaged(chunkIndex, in entity, in ownerUnit, in teammateData);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void TeammateRegister_00002044_0024PostfixBurstDelegate(ref EntityCommandBuffer CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData);

	internal static class TeammateRegister_00002044_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<TeammateRegister_00002044_0024PostfixBurstDelegate>(TeammateRegister).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(ref EntityCommandBuffer CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer, int, ref Entity, ref Entity, ref TeammateData, void>)functionPointer)(ref CMD, chunkIndex, ref entity, ref ownerUnit, ref teammateData);
					return;
				}
			}
			CMD.TeammateRegister_0024BurstManaged(chunkIndex, in entity, in ownerUnit, in teammateData);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StopKeepCasting_00002045_0024PostfixBurstDelegate(in EntityManager em, in Entity attachTarget, in Entity spell);

	internal static class StopKeepCasting_00002045_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<StopKeepCasting_00002045_0024PostfixBurstDelegate>(StopKeepCasting).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in EntityManager em, in Entity attachTarget, in Entity spell)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityManager, ref Entity, ref Entity, void>)functionPointer)(ref em, ref attachTarget, ref spell);
					return;
				}
			}
			StopKeepCasting_0024BurstManaged(in em, in attachTarget, in spell);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void AttackRollball_00002046_0024PostfixBurstDelegate(in EntityCommandBuffer.ParallelWriter CMD, int sortKey, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity);

	internal static class AttackRollball_00002046_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<AttackRollball_00002046_0024PostfixBurstDelegate>(AttackRollball).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in EntityCommandBuffer.ParallelWriter CMD, int sortKey, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, int, float, ref SpellConfigComponentData, ref Entity, void>)functionPointer)(ref CMD, sortKey, damage, ref rollballConfig, ref rollballEntity);
					return;
				}
			}
			CMD.AttackRollball_0024BurstManaged(sortKey, damage, ref rollballConfig, in rollballEntity);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void AttackRollball_00002047_0024PostfixBurstDelegate(in EntityManager manager, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity);

	internal static class AttackRollball_00002047_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<AttackRollball_00002047_0024PostfixBurstDelegate>(AttackRollball).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in EntityManager manager, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityManager, float, ref SpellConfigComponentData, ref Entity, void>)functionPointer)(ref manager, damage, ref rollballConfig, ref rollballEntity);
					return;
				}
			}
			AttackRollball_0024BurstManaged(in manager, damage, ref rollballConfig, in rollballEntity);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetSpellElementDataWithTimeScale_00002048_0024PostfixBurstDelegate(in SpellElementEffectComponentData element, in DynamicOptimizeData optimizeData, out SpellElementEffectComponentData result);

	internal static class GetSpellElementDataWithTimeScale_00002048_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<GetSpellElementDataWithTimeScale_00002048_0024PostfixBurstDelegate>(GetSpellElementDataWithTimeScale).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(in SpellElementEffectComponentData element, in DynamicOptimizeData optimizeData, out SpellElementEffectComponentData result)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref SpellElementEffectComponentData, ref DynamicOptimizeData, ref SpellElementEffectComponentData, void>)functionPointer)(ref element, ref optimizeData, ref result);
					return;
				}
			}
			GetSpellElementDataWithTimeScale_0024BurstManaged(in element, in optimizeData, out result);
		}
	}

	private static readonly SpellColorType[] baseColors = new SpellColorType[3]
	{
		SpellColorType.Frozen,
		SpellColorType.Venom,
		SpellColorType.Mucus
	};

	public static SpellColorType? ToSpellColor(this SpellAbilityType ability)
	{
		return ability switch
		{
			SpellAbilityType.MucusCrystal => SpellColorType.Mucus, 
			SpellAbilityType.VenomCrystal => SpellColorType.Venom, 
			SpellAbilityType.Frozen => SpellColorType.Frozen, 
			SpellAbilityType.FireCrystal => SpellColorType.Fire, 
			SpellAbilityType.DeathInfect => SpellColorType.Void, 
			_ => null, 
		};
	}

	public static int? ToSpellId(this SpellColorType color, int level)
	{
		return color switch
		{
			SpellColorType.Frozen => 30140 + level, 
			SpellColorType.Mucus => 30040 + level, 
			SpellColorType.Venom => 30050 + level, 
			SpellColorType.Fire => 31111, 
			SpellColorType.Thunder => 31010 + level, 
			SpellColorType.Void => 31290 + level, 
			_ => null, 
		};
	}

	public static TOut MaxOrDefault<TIn, TOut>(this IEnumerable<TIn> input, Func<TIn, TOut> func, TOut def)
	{
		TOut[] array = input.Select(func).ToArray();
		if (array.Length != 0)
		{
			return array.Max();
		}
		return def;
	}

	public static SpellColorType GetRandomBaseColor()
	{
		return baseColors[UnityEngine.Random.Range(0, baseColors.Length)];
	}

	public static SpellColorType GetRandomColor()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
		{
			return SpellColorType.Fire;
		}
		if (UnityEngine.Random.Range(0f, 1f) <= 0.1f)
		{
			return SpellColorType.Thunder;
		}
		if (UnityEngine.Random.Range(0, 6) == 0)
		{
			return SpellColorType.Player;
		}
		return GetRandomBaseColor();
	}

	public static SpellColorType GetRandomColorIncludeVoid()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.15f)
		{
			return SpellColorType.Fire;
		}
		if (UnityEngine.Random.Range(0f, 1f) <= 0.15f)
		{
			return SpellColorType.Void;
		}
		if (UnityEngine.Random.Range(0f, 1f) <= 0.15f)
		{
			return SpellColorType.Thunder;
		}
		if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
		{
			return SpellColorType.Player;
		}
		return GetRandomBaseColor();
	}

	public static SpellColorType GetSpellColor(SpellShootData shoot, SpellColorType currentColor, bool useRandomColorByWand)
	{
		if (currentColor != SpellColorType.Thunder)
		{
			float num = (from e in shoot.EnhanceList
				select e.GetFinalConfig() into e
				where e.abilityType == SpellAbilityType.ThunderCrystal
				select e).MaxOrDefault((SpellConfig e) => e.float3, 0f) / 100f;
			if (UnityEngine.Random.Range(0f, 1f) < num)
			{
				return SpellColorType.Thunder;
			}
		}
		if (useRandomColorByWand)
		{
			return currentColor;
		}
		SpellColorType[] array = (from e in shoot.EnhanceList
			select e.GetFinalConfig().abilityType.ToSpellColor() into e
			where e.HasValue && e.Value != SpellColorType.Thunder
			select e.Value).Distinct().ToArray();
		if (array.Length != 0)
		{
			return array[UnityEngine.Random.Range(0, array.Length)];
		}
		return SpellColorType.Player;
	}

	public static bool LogIf(this UnityEngine.Object val, object message, bool error = false)
	{
		return ((bool)val).LogIf(message, error);
	}

	public static bool LogIfNot(this UnityEngine.Object val, object message, bool error = false)
	{
		return ((bool)val).LogIfNot(message, error);
	}

	public static bool LogIf(this bool val, object message, bool error = false)
	{
		if (val)
		{
			if (error)
			{
				Debug.LogError(message);
			}
			else
			{
				Debug.LogWarning(message);
			}
		}
		return val;
	}

	public static bool LogIfNot(this bool val, object message, bool error = false)
	{
		if (!val)
		{
			if (error)
			{
				Debug.LogError(message);
			}
			else
			{
				Debug.LogWarning(message);
			}
		}
		return !val;
	}

	public static bool CompareAnyTag(this GameObject go, params string[] tags)
	{
		for (int i = 0; i < tags.Length; i++)
		{
			if (go.CompareTag(tags[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static Vector3 IgnoreZ(this Vector3 self)
	{
		self.z = 0f;
		return self;
	}

	public static float CallulateSpellScaleByDamage(float originDamage, float finalDamage)
	{
		float x = 0f;
		if (originDamage >= 0f && finalDamage >= 0f)
		{
			x = math.pow(finalDamage / originDamage, 0.3333f);
		}
		return math.max(x, 0.1f);
	}

	public static IEnumerable<(T Left, Y Right)> Zip<T, Y>(this IEnumerable<T> self, IEnumerable<Y> right)
	{
		return self.Zip(right, (T a, Y b) => (a, b));
	}

	[AOT.MonoPInvokeCallback(typeof(DisableSpellTrigger_00002012_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void DisableSpellTrigger(in PhysicsCollider collider)
	{
		DisableSpellTrigger_00002012_0024BurstDirectCall.Invoke(in collider);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(EnableSpellTrigger_00002013_0024PostfixBurstDelegate))]
	public static void EnableSpellTrigger(in PhysicsCollider collider)
	{
		EnableSpellTrigger_00002013_0024BurstDirectCall.Invoke(in collider);
	}

	public static void KillAllChildTeammates(EntityManager entityMgr, in Entity childEntity)
	{
		DynamicBuffer<TeammateOwnerInfoBuffer> buffer = entityMgr.GetBuffer<TeammateOwnerInfoBuffer>(childEntity);
		for (int i = 0; i < buffer.Length; i++)
		{
			if (entityMgr.HasComponent<TeammateDeadTag>(buffer[i].TeammateEntity))
			{
				entityMgr.SetComponentEnabled<TeammateDeadTag>(buffer[i].TeammateEntity, value: true);
				if (entityMgr.HasComponent<TeammateOwner>(buffer[i].TeammateEntity))
				{
					EntityManager entityMgr2 = entityMgr;
					TeammateOwnerInfoBuffer teammateOwnerInfoBuffer = buffer[i];
					KillAllChildTeammates(entityMgr2, in teammateOwnerInfoBuffer.TeammateEntity);
				}
			}
		}
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(DisableSpellReboundCollider_00002015_0024PostfixBurstDelegate))]
	public static void DisableSpellReboundCollider(in PhysicsCollider collider)
	{
		DisableSpellReboundCollider_00002015_0024BurstDirectCall.Invoke(in collider);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(EnableSpellReboundCollider_00002016_0024PostfixBurstDelegate))]
	public static void EnableSpellReboundCollider(in PhysicsCollider collider)
	{
		EnableSpellReboundCollider_00002016_0024BurstDirectCall.Invoke(in collider);
	}

	[AOT.MonoPInvokeCallback(typeof(DisableTeammateCollider_00002017_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void DisableTeammateCollider(in PhysicsCollider collider)
	{
		DisableTeammateCollider_00002017_0024BurstDirectCall.Invoke(in collider);
	}

	public static void ColorEnumToString(this SpellColorType colorType, out FixedString32Bytes result)
	{
		result = colorType switch
		{
			SpellColorType.Player => "Player", 
			SpellColorType.Frozen => "Frozen", 
			SpellColorType.Monster => "Monster", 
			SpellColorType.Mucus => "Mucus", 
			SpellColorType.Venom => "Venom", 
			SpellColorType.Fire => "Fire", 
			SpellColorType.Thunder => "Thunder", 
			SpellColorType.Void => "Void", 
			_ => throw new ArgumentOutOfRangeException("colorType", colorType, null), 
		};
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(CreateSpellGlobalParticle_00002019_0024PostfixBurstDelegate))]
	public static void CreateSpellGlobalParticle(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in float3 position, in float scale, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, in SpellSingleton spellSingleton, in float3 velocity)
	{
		CreateSpellGlobalParticle_00002019_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in effectName, in position, in scale, in spellConfig, in spellData, in spellSingleton, in velocity);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(CreateSpellHitEffect_0000201A_0024PostfixBurstDelegate))]
	public static void CreateSpellHitEffect(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in SpellComponentData data, in float3 position, in float3 direction, float scale)
	{
		CreateSpellHitEffect_0000201A_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in spellSingleton, in config, in data, in position, in direction, scale);
	}

	[AOT.MonoPInvokeCallback(typeof(CreateOriginalSpellHitEffect_0000201B_0024PostfixBurstDelegate))]
	[BurstCompile]
	private static void CreateOriginalSpellHitEffect(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, int spellId, SpellColorType color, in float3 position, in float3 direction, float scale, out Entity inBufferEntity)
	{
		CreateOriginalSpellHitEffect_0000201B_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in spellSingleton, spellId, color, in position, in direction, scale, out inBufferEntity);
	}

	[AOT.MonoPInvokeCallback(typeof(CreateSpecificSpellEffect_0000201C_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void CreateSpecificSpellEffect(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position, in float3 direction = default(float3), float scale = 1f)
	{
		CreateSpecificSpellEffect_0000201C_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in effectName, in colorName, in spellSingleton, in config, in position, in direction, scale);
	}

	[AOT.MonoPInvokeCallback(typeof(CreateSpecificSpellGlobalEffect_0000201D_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void CreateSpecificSpellGlobalEffect(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position)
	{
		CreateSpecificSpellGlobalEffect_0000201D_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in effectName, in colorName, in spellSingleton, in config, in position);
	}

	[AOT.MonoPInvokeCallback(typeof(CreateSimpleGlobalEffect_0000201E_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void CreateSimpleGlobalEffect(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in SpellSingleton spellSingleton, in float3 position, float scale = 1f)
	{
		CreateSimpleGlobalEffect_0000201E_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in effectName, in spellSingleton, in position, scale);
	}

	[AOT.MonoPInvokeCallback(typeof(CreateSpellEffect_0000201F_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void CreateSpellEffect(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellComponentData data, in SpellConfigComponentData config, in float3 position, in FixedString32Bytes EffectName, float scale, in float3 rotation)
	{
		CreateSpellEffect_0000201F_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in spellSingleton, in data, in config, in position, in EffectName, scale, in rotation);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetSpell1014RainbowColor_00002020_0024PostfixBurstDelegate))]
	public static void GetSpell1014RainbowColor(int index, out FixedString32Bytes result)
	{
		GetSpell1014RainbowColor_00002020_0024BurstDirectCall.Invoke(index, out result);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(ActiveGravitationalForceCrystal_00002021_0024PostfixBurstDelegate))]
	public static void ActiveGravitationalForceCrystal(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, float damage, in float3 startPosition, in Entity GravitationalBufferEntity, in CurrentRoomEntitiesSingleton currentRoomEntities, in SpellAspect spell, in Entity sourceEntity)
	{
		ActiveGravitationalForceCrystal_00002021_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, damage, in startPosition, in GravitationalBufferEntity, in currentRoomEntities, in spell, in sourceEntity);
	}

	[AOT.MonoPInvokeCallback(typeof(TryRecordRedRune_00002022_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void TryRecordRedRune(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellAspect spell, in Entity TargetEntity, in float3 position, in Entity RedRuneCheckBufferEntity, bool isCritical)
	{
		TryRecordRedRune_00002022_0024BurstDirectCall.Invoke(ref cmd, chunkIndex, in spell, in TargetEntity, in position, in RedRuneCheckBufferEntity, isCritical);
	}

	public static void SpellInitialFallData(ref SpellSpawnParams spawnParams, SpellInitialParameter sip, float spellSpeed)
	{
		float3 targetPos = sip.finalShootSpatialInfo.Target.Value;
		SpellInitialFallData(ref spawnParams, targetPos, spellSpeed);
	}

	public static void SpellInitialFallData(ref SpellSpawnParams spawnParams, float3 targetPos, float spellSpeed)
	{
		spawnParams.MovementComponentData.Gravity = math.max(43f * spellSpeed / 16f, 12f);
		spawnParams.MovementComponentData.OriginalSpellHorizontalSpeed = spellSpeed;
		spawnParams.MovementComponentData.CurrentFallSpeed = spellSpeed;
		float3 input = targetPos;
		float3 @float = -spawnParams.MovementComponentData.Direction;
		spawnParams.SpawnPosition = input + @float * 2f + new float3(0f, 0f, -7f);
		float3 x = spawnParams.SpawnPosition.IgnoreZ();
		float3 y = input.IgnoreZ();
		spawnParams.MovementComponentData.Speed *= math.distance(x, y) / 7f;
	}

	public static bool TryRefract(in float3 position, UnitType selfUnitType, ref SpellRefractionData refractionData, in DynamicBuffer<SpellRefractionHitEntities> refractedEntities, ref SpellMovementComponentData movement, in CurrentRoomEntitiesSingleton CurrentRoomEntities, in NativeArray<Entity> theEntitiesHitByThisDamage, out float3 targetRefractPosition)
	{
		targetRefractPosition = float3.zero;
		if (refractionData.RemainCount <= 0)
		{
			return false;
		}
		foreach (Entity item in theEntitiesHitByThisDamage)
		{
			refractedEntities.Add(new SpellRefractionHitEntities
			{
				Entity = item
			});
		}
		NativeHashSet<Entity> ignoreEntities = new NativeHashSet<Entity>(8, Allocator.Temp);
		foreach (SpellRefractionHitEntities refractedEntity in refractedEntities)
		{
			ignoreEntities.Add(refractedEntity.Entity);
		}
		Entity target;
		float3 targetPosition;
		UnitProperty_Dots targetPpt;
		bool flag = CurrentRoomEntities.FindReflectionTarget(position, selfUnitType, in ignoreEntities, out target, out targetPosition, out targetPpt);
		if (!flag)
		{
			refractedEntities.Clear();
			ignoreEntities.Clear();
			foreach (Entity item2 in theEntitiesHitByThisDamage)
			{
				refractedEntities.Add(new SpellRefractionHitEntities
				{
					Entity = item2
				});
				ignoreEntities.Add(item2);
			}
			flag = CurrentRoomEntities.FindReflectionTarget(position, selfUnitType, in ignoreEntities, out target, out targetPosition, out targetPpt);
		}
		if (flag)
		{
			refractionData.RemainCount--;
			float3 input = math.normalizesafe(targetPosition - position);
			movement.Direction = input.IgnoreZ();
			if (movement.IsFallSpell)
			{
				movement.FallTargetPosition = targetPosition;
				movement.ReboundFallSpeed();
			}
		}
		targetRefractPosition = targetPosition;
		return flag;
	}

	public static bool TryReboundWhenFall(ref SpellMovementComponentData movement)
	{
		if (movement.ReboundCount <= 0)
		{
			return false;
		}
		movement.ReboundCount--;
		movement.ReboundFallSpeed();
		return true;
	}

	public static bool TryRefractOrReboundWhenFall(in Entity spellEntity, in float3 position, UnitType selfUnitType, in ComponentLookup<SpellRefractionData> refractionDataLookup, in BufferLookup<SpellRefractionHitEntities> refractedEntitiesLookup, ref SpellMovementComponentData movement, in CurrentRoomEntitiesSingleton CurrentRoomEntities, in NativeArray<Entity> theEntitiesHitByThisDamage, bool hitUnitInThisDamage)
	{
		if (refractionDataLookup.HasComponent(spellEntity) && hitUnitInThisDamage)
		{
			ref SpellRefractionData valueRW = ref refractionDataLookup.GetRefRW(spellEntity).ValueRW;
			DynamicBuffer<SpellRefractionHitEntities> refractedEntities = refractedEntitiesLookup[spellEntity];
			if (TryRefract(in position, selfUnitType, ref valueRW, in refractedEntities, ref movement, in CurrentRoomEntities, in theEntitiesHitByThisDamage, out var _))
			{
				return true;
			}
		}
		return TryReboundWhenFall(ref movement);
	}

	public static int GetFinalSpawnCountWithLimitCount(int threshold1, int limit1, int threshold2, int limit2, int currentShootCounter, int nextShootCount)
	{
		if (currentShootCounter < threshold1)
		{
			return nextShootCount;
		}
		nextShootCount = math.max(1, nextShootCount);
		return math.min((currentShootCounter >= threshold2) ? limit2 : limit1, nextShootCount);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(TryAttackEntity_00002029_0024PostfixBurstDelegate))]
	public static HitType TryAttackEntity(this in EntityCommandBuffer.ParallelWriter cmd, int sortKey, in Entity target, in TakeDamageInfo_Dots damage, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool recordTargetToSpell = true)
	{
		return TryAttackEntity_00002029_0024BurstDirectCall.Invoke(in cmd, sortKey, in target, in damage, in unitLookup, in spellConfigLookup, checkCamp, recordTargetToSpell);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetEntityHitType_0000202A_0024PostfixBurstDelegate))]
	public static HitType GetEntityHitType(in Entity target, in UnitType shooterType, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool checkSolidObj = false)
	{
		return GetEntityHitType_0000202A_0024BurstDirectCall.Invoke(in target, in shooterType, in unitLookup, in spellConfigLookup, checkCamp, checkSolidObj);
	}

	[AOT.MonoPInvokeCallback(typeof(GetAttackableEntitiesInBox_0000202B_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void GetAttackableEntitiesInBox(in float3 boxCenter, in float3 HalfBoxSize, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true, in Quaternion quaternion = default(Quaternion))
	{
		GetAttackableEntitiesInBox_0000202B_0024BurstDirectCall.Invoke(in boxCenter, in HalfBoxSize, in selfCamp, containsBrittleness, in unitPptLookup, in spellConfigLookup, in physics, ref entities, checkUnitCamp, in quaternion);
	}

	[AOT.MonoPInvokeCallback(typeof(GetAttackableEntitiesInRange_0000202C_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void GetAttackableEntitiesInRange(in float3 position, in float radius, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true)
	{
		GetAttackableEntitiesInRange_0000202C_0024BurstDirectCall.Invoke(in position, in radius, in selfCamp, containsBrittleness, in unitPptLookup, in SpellConfigLookup, in physics, ref entities, checkUnitCamp);
	}

	[BurstCompile]
	public static void ShuffleNativeList<T>(this NativeList<T> list, ref Unity.Mathematics.Random random) where T : unmanaged
	{
		for (int num = list.Length - 1; num > 0; num--)
		{
			int num2 = random.NextInt(0, num + 1);
			int index = num;
			int index2 = num2;
			T val = list[num2];
			T val2 = list[num];
			T val4 = (list[index] = val);
			val4 = (list[index2] = val2);
		}
	}

	[AOT.MonoPInvokeCallback(typeof(GetAttackablesInRaycast_0000202E_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static bool GetAttackablesInRaycast(in float3 rayStart, in float3 rayEnd, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, out NativeList<Unity.Physics.RaycastHit> hitList, Allocator allocatorIfHit = Allocator.Temp)
	{
		return GetAttackablesInRaycast_0000202E_0024BurstDirectCall.Invoke(in rayStart, in rayEnd, in selfCamp, containsBrittleness, in unitPptLookup, in spellConfigLookup, in physics, out hitList, allocatorIfHit);
	}

	[AOT.MonoPInvokeCallback(typeof(CreateOtherCampFilter_IncludeSpell_0000202F_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void CreateOtherCampFilter_IncludeSpell(UnitType self, bool containsBrittleness, out CollisionFilter res)
	{
		CreateOtherCampFilter_IncludeSpell_0000202F_0024BurstDirectCall.Invoke(self, containsBrittleness, out res);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetAttackableEntitiesInSphereCast_00002030_0024PostfixBurstDelegate))]
	public static void GetAttackableEntitiesInSphereCast(in float3 rayStart, in float3 rayEnd, in float width, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<ColliderCastHit> hitList, bool checkUnitCamp = true, bool checkUnit = true)
	{
		GetAttackableEntitiesInSphereCast_00002030_0024BurstDirectCall.Invoke(in rayStart, in rayEnd, in width, in selfCamp, containsBrittleness, in unitPptLookup, in spellConfigLookup, in physics, ref hitList, checkUnitCamp, checkUnit);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(RemoveCannotAttackSpell_00002031_0024PostfixBurstDelegate))]
	public static void RemoveCannotAttackSpell(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<SpellConfigComponentData> configLookup)
	{
		RemoveCannotAttackSpell_00002031_0024BurstDirectCall.Invoke(ref entities, selfCamp, in configLookup);
	}

	[AOT.MonoPInvokeCallback(typeof(RemoveSameCampUnitAndNotAttackUnit_00002032_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void RemoveSameCampUnitAndNotAttackUnit(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<UnitProperty_Dots> UnitPropertyLookup)
	{
		RemoveSameCampUnitAndNotAttackUnit_00002032_0024BurstDirectCall.Invoke(ref entities, selfCamp, in UnitPropertyLookup);
	}

	[BurstDiscard]
	public static void SpawnChild(in SpellSingleton singleton, in EntityManager entityManager, int spellPrefabId, FixedString32Bytes effectName, FixedString32Bytes colorName, in Entity Parent, out Entity child)
	{
		if (!singleton.Prefabs.TryGetValue($"{spellPrefabId}_{effectName}_{colorName}", out var item))
		{
			Debug.LogError($"预制体{spellPrefabId}_{effectName}_{colorName}不存在");
			child = Entity.Null;
			return;
		}
		child = entityManager.Instantiate(item);
		entityManager.AddComponent<Parent>(child);
		entityManager.SetComponentData(child, new Parent
		{
			Value = Parent
		});
		if (!entityManager.HasBuffer<LinkedEntityGroup>(Parent))
		{
			entityManager.AddBuffer<LinkedEntityGroup>(Parent);
		}
		entityManager.GetBuffer<LinkedEntityGroup>(Parent).Add(new LinkedEntityGroup
		{
			Value = child
		});
	}

	[AOT.MonoPInvokeCallback(typeof(CalculateSpellComplexity_00002034_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static int CalculateSpellComplexity(SpellAbilityType spellAbility)
	{
		return CalculateSpellComplexity_00002034_0024BurstDirectCall.Invoke(spellAbility);
	}

	[BurstDiscard]
	public static void SpawnChildIgnoreColor(in SpellSingleton singleton, in EntityManager entityManager, int spellPrefabId, FixedString32Bytes effectName, in Entity Parent, out Entity child)
	{
		if (!singleton.Prefabs.TryGetValue($"{spellPrefabId}_{effectName}", out var item))
		{
			Debug.LogError($"预制体{spellPrefabId}_{effectName}不存在");
			child = Entity.Null;
			return;
		}
		child = entityManager.Instantiate(item);
		entityManager.AddComponent<Parent>(child);
		entityManager.SetComponentData(child, new Parent
		{
			Value = Parent
		});
		if (!entityManager.HasBuffer<LinkedEntityGroup>(Parent))
		{
			entityManager.AddBuffer<LinkedEntityGroup>(Parent);
		}
		entityManager.GetBuffer<LinkedEntityGroup>(Parent).Add(new LinkedEntityGroup
		{
			Value = child
		});
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetLowFpsActiveThreshold_00002036_0024PostfixBurstDelegate))]
	public static float GetLowFpsActiveThreshold(bool isMobile)
	{
		return GetLowFpsActiveThreshold_00002036_0024BurstDirectCall.Invoke(isMobile);
	}

	[AOT.MonoPInvokeCallback(typeof(GetLowFpsActiveMinFps_00002037_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static float GetLowFpsActiveMinFps(bool isMobile)
	{
		return GetLowFpsActiveMinFps_00002037_0024BurstDirectCall.Invoke(isMobile);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetLowFpsMaxBonusTimeScale_00002038_0024PostfixBurstDelegate))]
	public static float GetLowFpsMaxBonusTimeScale(bool isMobile)
	{
		return GetLowFpsMaxBonusTimeScale_00002038_0024BurstDirectCall.Invoke(isMobile);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetMaxOptimizeFPSThreshold_00002039_0024PostfixBurstDelegate))]
	public static float GetMaxOptimizeFPSThreshold(bool isMobile)
	{
		return GetMaxOptimizeFPSThreshold_00002039_0024BurstDirectCall.Invoke(isMobile);
	}

	[AOT.MonoPInvokeCallback(typeof(GetLowFrameTimeScale_0000203A_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static float GetLowFrameTimeScale(float maxTimescale, float currentFPS, float lowFPSThreshold)
	{
		return GetLowFrameTimeScale_0000203A_0024BurstDirectCall.Invoke(maxTimescale, currentFPS, lowFPSThreshold);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(SpawnSuicideBug_0000203B_0024PostfixBurstDelegate))]
	public static void SpawnSuicideBug(this ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, int SpawnCount, in SpellComponentData data, in SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, in TeammateData teammateData, in SpellElementEffectComponentData element, in SpellSingleton spellSingleton, float HpToDmgRatio, float ExplosionRange, float InstantKillRatio, bool ConstVoidEffect, float maxHp, float wormPower)
	{
		SpawnSuicideBug_0000203B_0024BurstDirectCall.Invoke(ref CMD, chunkIndex, SpawnCount, in data, in movement, in config, in transform, in teammateData, in element, in spellSingleton, HpToDmgRatio, ExplosionRange, InstantKillRatio, ConstVoidEffect, maxHp, wormPower);
	}

	public static void ApplyVoidExplosion(this ref EntityCommandBuffer CMD, Entity globalParticleEmitterEntity, float3 position, float maxHp, Spell3129VoidExplosion.VoidExplosionData_Dots voidExplosionData, Entity voidEntity, float particleSize = 0.8f)
	{
		CMD.AppendToBuffer(voidEntity, new Spell3129DamageRequestBuffer
		{
			MaxHp = maxHp,
			voidExplosionData = voidExplosionData,
			Timer = 0f,
			SpawnPosition = position
		});
		GlobalParticleEmitParams globalParticleEmitParams = new GlobalParticleEmitParams(GlobalParticleType.Spell, "3129_Explosion", position);
		globalParticleEmitParams.Size = particleSize;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(globalParticleEmitterEntity, element);
		SEMgr.Inst.spell3129Charge.PlaySE();
	}

	public static TeammateType GetSpellTeammateType(SpellAbilityType type)
	{
		switch (type)
		{
		case SpellAbilityType.Summon1:
			return TeammateType.teammate1;
		case SpellAbilityType.Summon2:
			return TeammateType.teammate2;
		case SpellAbilityType.Summon3:
			return TeammateType.teammate3;
		case SpellAbilityType.Summon4:
			return TeammateType.teammate4;
		case SpellAbilityType.Summon5:
			return TeammateType.teammate5;
		case SpellAbilityType.Summon6:
			return TeammateType.teammate6;
		case SpellAbilityType.Summon7:
			return TeammateType.teammate7;
		default:
			Debug.LogError("嘛类型啊+ " + type);
			return TeammateType.teammate1;
		}
	}

	public static bool CheckIsTargetTeammateReachSummonLimit(in EntityManager entityMgr, TeammateType type, in Entity ownerEntity)
	{
		if (!entityMgr.HasBuffer<TeammateOwnerInfoBuffer>(ownerEntity))
		{
			return false;
		}
		DynamicBuffer<TeammateOwnerInfoBuffer> buffer = entityMgr.GetBuffer<TeammateOwnerInfoBuffer>(ownerEntity);
		int num = 0;
		switch (type)
		{
		case TeammateType.teammate1:
			num = 5;
			break;
		case TeammateType.teammate2:
			num = 1;
			break;
		case TeammateType.teammate3:
			num = 1000;
			break;
		case TeammateType.teammate4:
			num = 24;
			break;
		case TeammateType.teammate5:
			num = 3;
			break;
		case TeammateType.teammate6:
			num = 1;
			break;
		case TeammateType.teammate7:
			num = 2;
			break;
		}
		int num2 = 0;
		foreach (TeammateOwnerInfoBuffer item in buffer)
		{
			if (item.TeammateType == type)
			{
				num2++;
			}
		}
		return num2 < num;
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(GetTeammateSacrificeRange_0000203F_0024PostfixBurstDelegate))]
	public static void GetTeammateSacrificeRange(float baseRange, in SpellConfigComponentData spellConfig, out float result)
	{
		GetTeammateSacrificeRange_0000203F_0024BurstDirectCall.Invoke(baseRange, in spellConfig, out result);
	}

	public static void CheckFallThunderDamage(this EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, Entity spell3101BufferEntity, float3 position, ComponentLookup<UnitProperty_Dots> UnitPropertyLookup, PhysicsWorldSingleton physics, in SpellConfigComponentData spellConfig, in SpellMovementComponentData spellMovement, in LocalTransform spellTransform, in SpellElementEffectComponentData spellElementEffect, in SpellComponentData spellComponentData, Entity spellEntity)
	{
		if (spellConfig.ColorType != SpellColorType.Thunder)
		{
			return;
		}
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		DTool.GetEnemyEntityInRange(in position, spellElementEffect.ThunderHitRadius, spellConfig.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in physics, ref result);
		Spell3101NewThunderHitData spell3101NewThunderHitData = default(Spell3101NewThunderHitData);
		spell3101NewThunderHitData.StartPos = position;
		spell3101NewThunderHitData.Scale = spellElementEffect.ThunderHitRadius;
		Spell3101NewThunderHitData element = spell3101NewThunderHitData;
		CMD.AppendToBuffer(chunkIndex, spell3101BufferEntity, element);
		TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in spellConfig, in spellMovement, in spellTransform, in spellElementEffect, in spellComponentData, out var info);
		info.spell.Config.AbilityType = SpellAbilityType.ThunderCrystal;
		info.damage = spellConfig.Damage.Calculate() * spellElementEffect.ThunderHitDamageRatio;
		foreach (Entity item in result)
		{
			CMD.AppendToBuffer(chunkIndex, item, info);
		}
	}

	public static void GenerateThunderDamage(this EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, Entity spell3101BufferEntity, float damage, float3 position, ComponentLookup<UnitProperty_Dots> UnitPropertyLookup, PhysicsWorldSingleton physics, in SpellConfigComponentData spellConfig, in SpellMovementComponentData spellMovement, in LocalTransform spellTransform, in SpellElementEffectComponentData spellElementEffect, in SpellComponentData spellComponentData, Entity spellEntity)
	{
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		DTool.GetEnemyEntityInRange(in position, spellElementEffect.ThunderHitRadius, spellConfig.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in physics, ref result);
		Spell3101NewThunderHitData spell3101NewThunderHitData = default(Spell3101NewThunderHitData);
		spell3101NewThunderHitData.StartPos = position;
		spell3101NewThunderHitData.Scale = spellElementEffect.ThunderHitRadius;
		Spell3101NewThunderHitData element = spell3101NewThunderHitData;
		CMD.AppendToBuffer(chunkIndex, spell3101BufferEntity, element);
		TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in spellConfig, in spellMovement, in spellTransform, in spellElementEffect, in spellComponentData, out var info);
		info.spell.Config.AbilityType = SpellAbilityType.ThunderCrystal;
		info.damage = damage * spellElementEffect.ThunderHitDamageRatio;
		foreach (Entity item in result)
		{
			CMD.AppendToBuffer(chunkIndex, item, info);
		}
	}

	public static void TeammateDeadTryActiveTeammateDelayDeathEffect(this ref EntityCommandBuffer.ParallelWriter CMD, ref UnitProperty_Dots unitPpt, ref ComponentLookup<TeammateData> TeammateDataLookup, Entity entity, Entity SpellEffectEntity, int chunkIndex, ComponentLookup<PhysicsCollider> ColliderLookUp, Entity effectBufferApplyEntity)
	{
		if (!TeammateDataLookup.HasComponent(entity) || TeammateDataLookup[entity].TeammateDelayDeathEffectActive)
		{
			return;
		}
		RefRW<TeammateData> refRW = TeammateDataLookup.GetRefRW(entity);
		refRW.ValueRW.TeammateDelayDeathEffectActive = true;
		if (TeammateDataLookup[entity].TeammateDelayDeathTime <= 0f)
		{
			CMD.SetComponentEnabled<TeammateDeadTag>(chunkIndex, entity, value: true);
			return;
		}
		unitPpt.unitCfg.currentHP = math.max(1f, unitPpt.unitCfg.currentHP);
		unitPpt.InvincibleRegister();
		unitPpt.CanBeTarget = false;
		CMD.AppendToBuffer(chunkIndex, effectBufferApplyEntity, new TeammateGhostEffectData
		{
			Entity = entity
		});
		RefRW<PhysicsCollider> refRW2;
		if (refRW.ValueRO.TeammateType != TeammateType.teammate4)
		{
			refRW2 = ColliderLookUp.GetRefRW(entity);
			DisableTeammateCollider(in refRW2.ValueRW);
		}
		else if (ColliderLookUp.HasComponent(entity))
		{
			refRW2 = ColliderLookUp.GetRefRW(entity);
			ref BlobAssetReference<Unity.Physics.Collider> value = ref refRW2.ValueRW.Value;
			CollisionFilter collisionFilter = new CollisionFilter
			{
				BelongsTo = 0u,
				CollidesWith = 0u,
				GroupIndex = 0
			};
			value.Value.SetCollisionFilter(collisionFilter);
		}
	}

	[AOT.MonoPInvokeCallback(typeof(TeammateRegister_00002043_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void TeammateRegister(this ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData)
	{
		TeammateRegister_00002043_0024BurstDirectCall.Invoke(ref CMD, chunkIndex, in entity, in ownerUnit, in teammateData);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(TeammateRegister_00002044_0024PostfixBurstDelegate))]
	public static void TeammateRegister(this ref EntityCommandBuffer CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData)
	{
		TeammateRegister_00002044_0024BurstDirectCall.Invoke(ref CMD, chunkIndex, in entity, in ownerUnit, in teammateData);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(StopKeepCasting_00002045_0024PostfixBurstDelegate))]
	public static void StopKeepCasting(in EntityManager em, in Entity attachTarget, in Entity spell)
	{
		StopKeepCasting_00002045_0024BurstDirectCall.Invoke(in em, in attachTarget, in spell);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(AttackRollball_00002046_0024PostfixBurstDelegate))]
	public static void AttackRollball(this in EntityCommandBuffer.ParallelWriter CMD, int sortKey, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity)
	{
		AttackRollball_00002046_0024BurstDirectCall.Invoke(in CMD, sortKey, damage, ref rollballConfig, in rollballEntity);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(AttackRollball_00002047_0024PostfixBurstDelegate))]
	public static void AttackRollball(in EntityManager manager, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity)
	{
		AttackRollball_00002047_0024BurstDirectCall.Invoke(in manager, damage, ref rollballConfig, in rollballEntity);
	}

	[AOT.MonoPInvokeCallback(typeof(GetSpellElementDataWithTimeScale_00002048_0024PostfixBurstDelegate))]
	[BurstCompile]
	public static void GetSpellElementDataWithTimeScale(in SpellElementEffectComponentData element, in DynamicOptimizeData optimizeData, out SpellElementEffectComponentData result)
	{
		GetSpellElementDataWithTimeScale_00002048_0024BurstDirectCall.Invoke(in element, in optimizeData, out result);
	}

	public static bool IsChargingSpell(this SpellAbilityType abilityType)
	{
		if (abilityType != SpellAbilityType.ShiningStar)
		{
			return abilityType == SpellAbilityType.SuperNova;
		}
		return true;
	}

	public static bool Find(this DynamicBuffer<SpellGameObjectEffectLink> effects, FixedString32Bytes name, out UnityObjectRef<GameObject> gameObject)
	{
		foreach (SpellGameObjectEffectLink item in effects)
		{
			SpellGameObjectEffectLink current = item;
			if (current.EffectName == name)
			{
				gameObject = current.GameObject;
				return true;
			}
		}
		gameObject = default(UnityObjectRef<GameObject>);
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal unsafe static void DisableSpellTrigger_0024BurstManaged(in PhysicsCollider collider)
	{
		if (collider.ColliderPtr->Type != ColliderType.Compound)
		{
			return;
		}
		CompoundCollider* colliderPtr = (CompoundCollider*)collider.ColliderPtr;
		for (int i = 0; i < colliderPtr->NumChildren; i++)
		{
			Unity.Physics.Collider* collider2 = colliderPtr->Children[i].Collider;
			if (collider2->GetCollisionResponse() == CollisionResponsePolicy.RaiseTriggerEvents)
			{
				collider2->SetCollisionResponse(CollisionResponsePolicy.None);
			}
		}
		colliderPtr->RefreshCollisionFilter();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal unsafe static void EnableSpellTrigger_0024BurstManaged(in PhysicsCollider collider)
	{
		if (collider.ColliderPtr->Type != ColliderType.Compound)
		{
			return;
		}
		CompoundCollider* colliderPtr = (CompoundCollider*)collider.ColliderPtr;
		for (int i = 0; i < colliderPtr->NumChildren; i++)
		{
			Unity.Physics.Collider* collider2 = colliderPtr->Children[i].Collider;
			if (collider2->GetCollisionFilter().CollidesWith != 256)
			{
				collider2->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
			}
		}
		colliderPtr->RefreshCollisionFilter();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal unsafe static void DisableSpellReboundCollider_0024BurstManaged(in PhysicsCollider collider)
	{
		if (collider.ColliderPtr->Type != ColliderType.Compound)
		{
			return;
		}
		CompoundCollider* colliderPtr = (CompoundCollider*)collider.ColliderPtr;
		for (int i = 0; i < colliderPtr->NumChildren; i++)
		{
			uint collidesWith = colliderPtr->Children[i].Collider->GetCollisionFilter().CollidesWith;
			if (collidesWith == 256 || collidesWith == 65792)
			{
				colliderPtr->Children[i].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
			}
		}
		colliderPtr->RefreshCollisionFilter();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal unsafe static void EnableSpellReboundCollider_0024BurstManaged(in PhysicsCollider collider)
	{
		if (collider.ColliderPtr->Type != ColliderType.Compound)
		{
			return;
		}
		CompoundCollider* colliderPtr = (CompoundCollider*)collider.ColliderPtr;
		for (int i = 0; i < colliderPtr->NumChildren; i++)
		{
			uint collidesWith = colliderPtr->Children[i].Collider->GetCollisionFilter().CollidesWith;
			if (collidesWith == 256 || collidesWith == 65792)
			{
				colliderPtr->Children[i].Collider->SetCollisionResponse(CollisionResponsePolicy.CollideRaiseCollisionEvents);
			}
		}
		colliderPtr->RefreshCollisionFilter();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal unsafe static void DisableTeammateCollider_0024BurstManaged(in PhysicsCollider collider)
	{
		Unity.Physics.Collider* colliderPtr = collider.ColliderPtr;
		CollisionResponsePolicy collisionResponse = colliderPtr->GetCollisionResponse();
		if (collisionResponse == CollisionResponsePolicy.RaiseTriggerEvents || collisionResponse == CollisionResponsePolicy.CollideRaiseCollisionEvents)
		{
			colliderPtr->SetCollisionResponse(CollisionResponsePolicy.None);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateSpellGlobalParticle_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in float3 position, in float scale, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, in SpellSingleton spellSingleton, in float3 velocity)
	{
		spellConfig.ColorType.ColorEnumToString(out var result);
		GlobalParticleEmitParams globalParticleEmitParams = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"{spellData.PrefabId}_{effectName}_{result}", position);
		globalParticleEmitParams.Size = scale;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		if (velocity.x != 0f || velocity.y != 0f || velocity.z != 0f)
		{
			element.Velocity = velocity;
		}
		if (DTool.IsSameCamp(spellConfig.ShooterType, UnitType.Player))
		{
			element.Alpha = spellSingleton.SpellTransparency;
		}
		cmd.AppendToBuffer(chunkIndex, spellSingleton.GlobalParticleBufferEntity, element);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateSpellHitEffect_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in SpellComponentData data, in float3 position, in float3 direction, float scale)
	{
		switch (config.AbilityType)
		{
		case SpellAbilityType.Rainbow:
			if (config.ColorType == SpellColorType.Player)
			{
				GetSpell1014RainbowColor(data.InShootIndex, out var result2);
				FixedString32Bytes effectName = "Hit";
				cmd.CreateSpecificSpellGlobalEffect(chunkIndex, in effectName, in result2, in spellSingleton, in config, in position);
				return;
			}
			break;
		case SpellAbilityType.ShiningStar:
		{
			int num = math.clamp(Mathf.CeilToInt(config.CriticalChance / 0.33f), 1, 4);
			for (int i = 1; i <= num; i++)
			{
				FixedString32Bytes effectName = $"Hit_{i}";
				float3 velocity = ((i == 2) ? direction : float3.zero);
				cmd.CreateSpellGlobalParticle(chunkIndex, in effectName, in position, in scale, in config, in data, in spellSingleton, in velocity);
			}
			return;
		}
		case SpellAbilityType.ShotGun:
		{
			float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
			config.ColorType.ColorEnumToString(out var result);
			cmd.AppendToBuffer(chunkIndex, spellSingleton.GlobalParticleBufferEntity, new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1031_Hit_{result}", layerPosition + position)
			{
				Size = scale,
				Velocity = direction
			});
			return;
		}
		case SpellAbilityType.SuperNova:
			return;
		}
		if (data.GlobalParticleHitEffect)
		{
			float3 velocity2 = (data.UseGlobalHitParticleWithSpellDirection ? direction : float3.zero);
			FixedString32Bytes effectName = "Hit";
			float3 velocity = position + new float3(0f, 0.3f, 0f);
			cmd.CreateSpellGlobalParticle(chunkIndex, in effectName, in velocity, in scale, in config, in data, in spellSingleton, in velocity2);
			return;
		}
		if (config.Id == 90041 || config.Id == 90042)
		{
			scale = 1f;
		}
		cmd.CreateOriginalSpellHitEffect(chunkIndex, in spellSingleton, config.Id, config.ColorType, in position, in direction, scale, out var inBufferEntity);
		if (inBufferEntity != Entity.Null && DTool.IsSameCamp(config.ShooterType, UnitType.Monster))
		{
			cmd.AddComponent<SpellTransparentSystem.ShootFromMonsterTag>(chunkIndex, inBufferEntity);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateOriginalSpellHitEffect_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, int spellId, SpellColorType color, in float3 position, in float3 direction, float scale, out Entity inBufferEntity)
	{
		inBufferEntity = Entity.Null;
		float2 dir = direction.xy;
		quaternion quaternion = DTool.DirectionToRotation(in dir);
		int spellPrefabId = spellId / 10;
		FixedString32Bytes effectName = "Hit";
		NativeHashMap<FixedString64Bytes, SpellEffect> item;
		SpellEffect item2;
		if (spellSingleton.TryGetSpellEffectEntity(spellPrefabId, in effectName, color, out var entity))
		{
			inBufferEntity = cmd.Instantiate(chunkIndex, entity);
			cmd.SetComponent(chunkIndex, inBufferEntity, LocalTransform.FromPositionRotationScale(position, quaternion, scale));
		}
		else if (spellSingleton.Effects.TryGetValue(spellId / 10, out item) && item.TryGetValue("Hit", out item2))
		{
			color.ColorEnumToString(out var result);
			cmd.AppendToBuffer(chunkIndex, spellSingleton.UnfollowingEffectRequireBufferEntity, new SpellEffectSystem.UnfollowingRequire
			{
				Color = result,
				Scale = scale,
				SpellId = spellId / 10,
				Settings = item2,
				StartPosition = position,
				StartRotation = quaternion
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateSpecificSpellEffect_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position, in float3 direction = default(float3), float scale = 1f)
	{
		if (spellSingleton.TryGetSpellEffectEntity(config.Id / 10, in effectName, colorName, out var entity))
		{
			Entity e = cmd.Instantiate(chunkIndex, entity);
			float3 position2 = position;
			float2 dir = direction.xy;
			cmd.SetComponent(chunkIndex, e, LocalTransform.FromPositionRotationScale(position2, DTool.DirectionToRotation(in dir), scale));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateSpecificSpellGlobalEffect_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in FixedString32Bytes colorName, in SpellSingleton spellSingleton, in SpellConfigComponentData config, in float3 position)
	{
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"{config.Id / 10}_{effectName}_{colorName}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = position + layerPosition;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		cmd.AppendToBuffer(chunkIndex, spellSingleton.GlobalParticleBufferEntity, element);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateSimpleGlobalEffect_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in FixedString32Bytes effectName, in SpellSingleton spellSingleton, in float3 position, float scale = 1f)
	{
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = effectName;
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = position + layerPosition;
		globalParticleEmitParams.Size = scale;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		cmd.AppendToBuffer(chunkIndex, spellSingleton.GlobalParticleBufferEntity, element);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateSpellEffect_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellSingleton spellSingleton, in SpellComponentData data, in SpellConfigComponentData config, in float3 position, in FixedString32Bytes EffectName, float scale, in float3 rotation)
	{
		if (spellSingleton.TryGetSpellEffectEntity(data.PrefabId, in EffectName, config.ColorType, out var entity))
		{
			Entity e = cmd.Instantiate(chunkIndex, entity);
			LocalTransform component = LocalTransform.FromPosition(position);
			float3 @float = rotation;
			@float.z = 0f;
			float z = math.atan2(@float.y, @float.x);
			component.Rotation = quaternion.Euler(0f, 0f, z);
			component.Scale = scale;
			cmd.SetComponent(chunkIndex, e, component);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void GetSpell1014RainbowColor_0024BurstManaged(int index, out FixedString32Bytes result)
	{
		if (index > 6)
		{
			Debug.LogError("彩虹的 Index 不对啊");
			index %= 7;
		}
		result = index switch
		{
			0 => "Red", 
			1 => "Orange", 
			2 => "Yellow", 
			3 => "Green", 
			4 => "Blue", 
			5 => "Cyan", 
			6 => "Purple", 
			_ => throw new Exception(), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void ActiveGravitationalForceCrystal_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, float damage, in float3 startPosition, in Entity GravitationalBufferEntity, in CurrentRoomEntitiesSingleton currentRoomEntities, in SpellAspect spell, in Entity sourceEntity)
	{
		if (spell.Config.ValueRO.GravitationalMaxApplyCount <= 0)
		{
			return;
		}
		currentRoomEntities.FindValidTargetsInRange(startPosition, spell.Config.ValueRO.GravitationalAttackRange, UnitType.Player, out var target, out var targetPosition, out var _);
		int num = spell.Config.ValueRO.GravitationalMaxApplyCount;
		for (int i = 0; i < target.Length; i++)
		{
			if (!(target[i] == sourceEntity))
			{
				TakeDamageInfo_Dots.NewInfo(spell.Entity, CostPenetrate: false, in spell.Config.ValueRO, in spell.Movement.ValueRO, in spell.Transform.ValueRO, in spell.ElementEffect.ValueRO, in spell.Data.ValueRO, out var info, CostRefraction: false);
				info.damage = damage;
				info.isDamageCritical = true;
				info.spell.Config.AbilityType = SpellAbilityType.PullForceCrystal;
				info.spell.Config.CriticalChance = 1f;
				float3 @float = targetPosition[i];
				info.knockbackForce = math.normalizesafe(startPosition - @float) * spell.Config.ValueRO.GravitationalAttackPullForce;
				cmd.AppendToBuffer(chunkIndex, GravitationalBufferEntity, new Spell3112NewChainSingleton
				{
					StartPos = startPosition,
					EndPos = @float,
					ColorType = spell.Config.ValueRO.ColorType,
					LineScale = spell.Transform.ValueRO.Scale * new float3(1f, 1f, 1f),
					TargetEntity = target[i],
					Info = info
				});
				num--;
				if (num <= 0)
				{
					break;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void TryRecordRedRune_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter cmd, int chunkIndex, in SpellAspect spell, in Entity TargetEntity, in float3 position, in Entity RedRuneCheckBufferEntity, bool isCritical)
	{
		cmd.AppendToBuffer(chunkIndex, RedRuneCheckBufferEntity, new Spell4025RuneSlashSpawnData
		{
			SpellEntity = spell.Entity,
			TargetPosition = position,
			IsCriticalHit = isCritical
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static HitType TryAttackEntity_0024BurstManaged(this in EntityCommandBuffer.ParallelWriter cmd, int sortKey, in Entity target, in TakeDamageInfo_Dots damage, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool recordTargetToSpell = true)
	{
		if (unitLookup.TryGetComponent(target, out var componentData))
		{
			if (checkCamp && DTool.IsSameCamp(damage.spell.Config.ShooterType, componentData.unitCfg.unitType))
			{
				return HitType.SameCamp;
			}
			if (componentData.unitCfg.unitType == UnitType.Brittleness)
			{
				cmd.AppendToBuffer(sortKey, target, damage);
				return HitType.Brittleness;
			}
			if (componentData.isDead)
			{
				return HitType.Unknown;
			}
			if (componentData.unitCfg.isSolidObj && !damage.isUndifferDamage)
			{
				return HitType.Unknown;
			}
			if (recordTargetToSpell)
			{
				cmd.AppendToBuffer(sortKey, damage.spell.Entity, new SpellHitEntity
				{
					Entity = target
				});
			}
			cmd.AppendToBuffer(sortKey, target, damage);
			return HitType.Unit;
		}
		if (spellConfigLookup.TryGetComponent(target, out var componentData2))
		{
			if (checkCamp && DTool.IsSameCamp(damage.spell.Config.ShooterType, componentData2.ShooterType))
			{
				return HitType.SameCamp;
			}
			if (componentData2.AbilityType == SpellAbilityType.Rollball && componentData2.Float1 > 0f)
			{
				cmd.AttackRollball(sortKey, damage.damage, ref spellConfigLookup.GetRefRW(target).ValueRW, in target);
				return HitType.RollBall;
			}
			if (componentData2.AbilityType == SpellAbilityType.Butterfly)
			{
				cmd.SetComponentEnabled<Spell1003ButterflyBeAttackedTag>(sortKey, target, value: true);
				return HitType.Butterfly;
			}
			return HitType.IgnoreSpell;
		}
		return HitType.Unknown;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static HitType GetEntityHitType_0024BurstManaged(in Entity target, in UnitType shooterType, in ComponentLookup<UnitProperty_Dots> unitLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, bool checkCamp = true, bool checkSolidObj = false)
	{
		if (unitLookup.TryGetComponent(target, out var componentData))
		{
			if (checkCamp && DTool.IsSameCamp(shooterType, componentData.unitCfg.unitType))
			{
				return HitType.SameCamp;
			}
			if (componentData.unitCfg.unitType == UnitType.Brittleness)
			{
				return HitType.Brittleness;
			}
			if (componentData.isDead)
			{
				return HitType.Unknown;
			}
			if (componentData.unitCfg.isSolidObj && !checkSolidObj)
			{
				return HitType.Unknown;
			}
			return HitType.Unit;
		}
		if (spellConfigLookup.TryGetComponent(target, out var componentData2))
		{
			if (checkCamp && DTool.IsSameCamp(shooterType, componentData2.ShooterType))
			{
				return HitType.SameCamp;
			}
			if (componentData2.AbilityType == SpellAbilityType.Rollball && componentData2.Float1 > 0f)
			{
				return HitType.RollBall;
			}
			if (componentData2.AbilityType == SpellAbilityType.Butterfly)
			{
				return HitType.Butterfly;
			}
			return HitType.IgnoreSpell;
		}
		return HitType.Unknown;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void GetAttackableEntitiesInBox_0024BurstManaged(in float3 boxCenter, in float3 HalfBoxSize, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true, in Quaternion quaternion = default(Quaternion))
	{
		if (checkUnitCamp)
		{
			float3 boxSize = HalfBoxSize;
			CollisionFilter filter = DTool.CreateOtherCampFilter(selfCamp, containsBrittleness);
			DTool.GetEnemyEntityInBox(in boxCenter, boxSize, in filter, in unitPptLookup, in physics, ref entities, quaternion);
		}
		else
		{
			CollisionFilter filter2 = DTool.CreateOtherCampFilter(UnitType.Player, containsBrittleness: true);
			CollisionFilter collisionFilter = DTool.CreateOtherCampFilter(UnitType.Monster, containsBrittleness: true);
			filter2.BelongsTo |= collisionFilter.BelongsTo;
			filter2.CollidesWith |= collisionFilter.CollidesWith;
			DTool.GetEnemyEntityInBox(in boxCenter, HalfBoxSize, in filter2, in unitPptLookup, in physics, ref entities, quaternion);
		}
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = (DTool.IsSameCamp(selfCamp, UnitType.Player) ? 8388608u : 16777216u);
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		physics.OverlapBox(boxCenter, quaternion, HalfBoxSize, ref outHits, @default);
		foreach (DistanceHit item in outHits)
		{
			if (spellConfigLookup.HasComponent(item.Entity))
			{
				Entity value = item.Entity;
				entities.Add(in value);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void GetAttackableEntitiesInRange_0024BurstManaged(in float3 position, in float radius, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<Entity> entities, bool checkUnitCamp = true)
	{
		if (checkUnitCamp)
		{
			DTool.GetEnemyEntityInRange(in position, radius, selfCamp, containsBrittleness, in unitPptLookup, in physics, ref entities);
		}
		else
		{
			CollisionFilter filter = DTool.CreateOtherCampFilter(UnitType.Player, containsBrittleness: true);
			CollisionFilter collisionFilter = DTool.CreateOtherCampFilter(UnitType.Monster, containsBrittleness: true);
			filter.BelongsTo |= collisionFilter.BelongsTo;
			filter.CollidesWith |= collisionFilter.CollidesWith;
			DTool.GetUnitEntityInRange(in position, radius, in filter, in unitPptLookup, in physics, ref entities);
		}
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = (DTool.IsSameCamp(selfCamp, UnitType.Player) ? 8388608u : 16777216u);
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		physics.OverlapSphere(position, radius, ref outHits, @default);
		foreach (DistanceHit item in outHits)
		{
			if (SpellConfigLookup.HasComponent(item.Entity))
			{
				Entity value = item.Entity;
				entities.Add(in value);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static bool GetAttackablesInRaycast_0024BurstManaged(in float3 rayStart, in float3 rayEnd, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, out NativeList<Unity.Physics.RaycastHit> hitList, Allocator allocatorIfHit = Allocator.Temp)
	{
		CreateOtherCampFilter_IncludeSpell(selfCamp, containsBrittleness, out var res);
		RaycastInput raycastInput = default(RaycastInput);
		raycastInput.Start = rayStart;
		raycastInput.End = rayEnd;
		raycastInput.Filter = res;
		RaycastInput input = raycastInput;
		LazyAllocAttackableCollector collector = new LazyAllocAttackableCollector(allocatorIfHit, unitPptLookup, spellConfigLookup);
		physics.CollisionWorld.CastRay(input, ref collector);
		hitList = collector.Release();
		return collector.NumHits > 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void CreateOtherCampFilter_IncludeSpell_0024BurstManaged(UnitType self, bool containsBrittleness, out CollisionFilter res)
	{
		res = CollisionFilter.Default;
		res.BelongsTo = 1073741824u;
		if (DTool.IsSameCamp(self, UnitType.Player))
		{
			res.BelongsTo |= 512u;
			res.CollidesWith = 8402944u;
		}
		else
		{
			res.BelongsTo |= 2048u;
			res.CollidesWith = 18874880u;
		}
		if (containsBrittleness)
		{
			res.CollidesWith |= 163840u;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void GetAttackableEntitiesInSphereCast_0024BurstManaged(in float3 rayStart, in float3 rayEnd, in float width, in UnitType selfCamp, bool containsBrittleness, in ComponentLookup<UnitProperty_Dots> unitPptLookup, in ComponentLookup<SpellConfigComponentData> spellConfigLookup, in PhysicsWorldSingleton physics, ref NativeList<ColliderCastHit> hitList, bool checkUnitCamp = true, bool checkUnit = true)
	{
		if (checkUnit)
		{
			if (checkUnitCamp)
			{
				DTool.GetEnemyHitInSpherer(in rayStart, rayEnd, width, selfCamp, containsBrittleness, in unitPptLookup, in physics, ref hitList);
			}
			else
			{
				CollisionFilter filter = DTool.CreateOtherCampFilter(UnitType.Player, containsBrittleness: true);
				CollisionFilter collisionFilter = DTool.CreateOtherCampFilter(UnitType.Monster, containsBrittleness: true);
				filter.BelongsTo |= collisionFilter.BelongsTo;
				filter.CollidesWith |= collisionFilter.CollidesWith;
				DTool.GetUnitEntityOnSphereCast(in rayStart, in rayEnd, in width, in filter, in physics, ref hitList);
			}
		}
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = (DTool.IsSameCamp(selfCamp, UnitType.Player) ? 8388608u : 16777216u);
		NativeList<ColliderCastHit> outHits = new NativeList<ColliderCastHit>(Allocator.Temp);
		float num = math.distance(rayStart, rayEnd);
		if (num == 0f || !physics.SphereCastAll(rayStart, width, math.normalize(rayEnd - rayStart), num, ref outHits, @default))
		{
			return;
		}
		foreach (ColliderCastHit item in outHits)
		{
			ColliderCastHit value = item;
			if (spellConfigLookup.HasComponent(value.Entity))
			{
				hitList.Add(in value);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void RemoveCannotAttackSpell_0024BurstManaged(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<SpellConfigComponentData> configLookup)
	{
		for (int i = 0; i < entities.Length; i++)
		{
			if (configLookup.TryGetComponent(entities[i], out var componentData))
			{
				SpellAbilityType abilityType = componentData.AbilityType;
				if ((abilityType != SpellAbilityType.Rollball && abilityType != SpellAbilityType.Butterfly) || DTool.IsSameCamp(componentData.ShooterType, selfCamp))
				{
					entities.RemoveAt(i);
					i--;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void RemoveSameCampUnitAndNotAttackUnit_0024BurstManaged(ref NativeList<Entity> entities, UnitType selfCamp, in ComponentLookup<UnitProperty_Dots> UnitPropertyLookup)
	{
		for (int i = 0; i < entities.Length; i++)
		{
			if (UnitPropertyLookup.TryGetComponent(entities[i], out var componentData))
			{
				UnitType unitType = componentData.unitCfg.unitType;
				if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness || DTool.IsSameCamp(componentData.unitCfg.unitType, selfCamp))
				{
					entities.RemoveAt(i);
					i--;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static int CalculateSpellComplexity_0024BurstManaged(SpellAbilityType spellAbility)
	{
		int result = 1;
		switch (spellAbility)
		{
		case SpellAbilityType.Bullet:
		case SpellAbilityType.PreFirework:
		case SpellAbilityType.BackMP:
		case SpellAbilityType.Boomerang:
		case SpellAbilityType.MrBingArrow:
			result = 1;
			break;
		case SpellAbilityType.ArcaneExplosion:
		case SpellAbilityType.ManaCoin:
		case SpellAbilityType.JudgementBlade:
		case SpellAbilityType.RedRune:
		case SpellAbilityType.BlueRune:
			result = 2;
			break;
		case SpellAbilityType.Rollball:
		case SpellAbilityType.Butterfly:
		case SpellAbilityType.Laser:
		case SpellAbilityType.HoverTorch:
		case SpellAbilityType.ThunderAura:
		case SpellAbilityType.ShotGun:
		case SpellAbilityType.ParasiticWorm:
		case SpellAbilityType.GreenRune:
			result = 3;
			break;
		case SpellAbilityType.BlackHole:
		case SpellAbilityType.SnakeWalk:
		case SpellAbilityType.DisintegrationRay:
		case SpellAbilityType.MagicBreaker:
			result = 4;
			break;
		case SpellAbilityType.FireBall:
		case SpellAbilityType.Meteor:
		case SpellAbilityType.ArcaneNova:
			result = 5;
			break;
		case SpellAbilityType.DeathAdder:
		case SpellAbilityType.ShiningStar:
			result = 6;
			break;
		case SpellAbilityType.Rainbow:
		case SpellAbilityType.Dash:
			result = 7;
			break;
		case SpellAbilityType.GiantBubble:
		case SpellAbilityType.DimensionTraveller:
			result = 9;
			break;
		case SpellAbilityType.DragonBreath:
		case SpellAbilityType.SuperNova:
			result = 10;
			break;
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float GetLowFpsActiveThreshold_0024BurstManaged(bool isMobile)
	{
		if (!isMobile)
		{
			return 35f;
		}
		return 20f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float GetLowFpsActiveMinFps_0024BurstManaged(bool isMobile)
	{
		if (!isMobile)
		{
			return 10f;
		}
		return 10f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float GetLowFpsMaxBonusTimeScale_0024BurstManaged(bool isMobile)
	{
		if (!isMobile)
		{
			return 3f;
		}
		return 5f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float GetMaxOptimizeFPSThreshold_0024BurstManaged(bool isMobile)
	{
		if (!isMobile)
		{
			return 10f;
		}
		return 5f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static float GetLowFrameTimeScale_0024BurstManaged(float maxTimescale, float currentFPS, float lowFPSThreshold)
	{
		if (lowFPSThreshold <= 0f)
		{
			return 1f;
		}
		return 1f + math.max(0f, maxTimescale * (1f - currentFPS / lowFPSThreshold));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void SpawnSuicideBug_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, int SpawnCount, in SpellComponentData data, in SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, in TeammateData teammateData, in SpellElementEffectComponentData element, in SpellSingleton spellSingleton, float HpToDmgRatio, float ExplosionRange, float InstantKillRatio, bool ConstVoidEffect, float maxHp, float wormPower)
	{
		config.ColorType.ColorEnumToString(out var result);
		for (int i = 0; i < SpawnCount; i++)
		{
			Entity e = CMD.Instantiate(chunkIndex, spellSingleton.Prefabs[$"2007_Bug_{result}"]);
			SpellConfigComponentData component = config;
			component.Damage.Base = config.Float1;
			component.Damage.AddBase = 0f;
			component.Radius.Base = config.Float3;
			component.Radius = config.Radius;
			component.Damage.MulRatio *= wormPower;
			SpellMovementComponentData component2 = movement;
			component2.Speed = (component2.Speed + 2.1f) * teammateData.TeammateSpeedRatio;
			component2.AroundAngle = SpawnCount * i;
			component2.ChaseOwnerPosition = transform.Position + new float3(0f, 0f, -0.1f);
			SpellComponentData component3 = data;
			component3.DisableSplitEffect = true;
			component3.SpellEffectEntity = Entity.Null;
			component3.TrailEffectEntity = Entity.Null;
			SpellElementEffectComponentData component4 = element;
			component4.VenomApplyCount *= wormPower;
			CMD.AddComponent(chunkIndex, e, component);
			CMD.AddComponent(chunkIndex, e, component2);
			CMD.AddComponent(chunkIndex, e, component3);
			CMD.AddComponent(chunkIndex, e, component4);
			CMD.AddBuffer<SpellHitEntity>(chunkIndex, e);
			CMD.SetComponent(chunkIndex, e, new Spell2007SuicideBugData
			{
				LifeTime = ((teammateData.AdvanceSkillLevel > 0) ? 10f : (6f + config.Duration.AddBase)) * config.Duration.MulRatio,
				Scale = transform.Scale,
				ExplodeRadius = teammateData.ExplodeRange,
				BugInheritHp = maxHp * 0.3f,
				BugSacrificeExplosionDamageRatio = teammateData.ExplodeHpDamageRatio,
				VoidExplosionData = new Spell3129VoidExplosion.VoidExplosionData_Dots
				{
					HpToDmgRatio = HpToDmgRatio,
					ExplosionRange = ExplosionRange,
					InstantKillRatio = InstantKillRatio,
					ConstVoidEffect = ConstVoidEffect
				}
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void GetTeammateSacrificeRange_0024BurstManaged(float baseRange, in SpellConfigComponentData spellConfig, out float result)
	{
		result = (baseRange + spellConfig.Radius.FallRadius) * (spellConfig.Radius.AddRatio + 1f) * spellConfig.Radius.MulRatio;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void TeammateRegister_0024BurstManaged(this ref EntityCommandBuffer.ParallelWriter CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData)
	{
		CMD.AppendToBuffer(chunkIndex, ownerUnit, new TeammateOwnerInfoBuffer
		{
			TeammateType = teammateData.TeammateType,
			TeammateEntity = entity
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void TeammateRegister_0024BurstManaged(this ref EntityCommandBuffer CMD, int chunkIndex, in Entity entity, in Entity ownerUnit, in TeammateData teammateData)
	{
		CMD.AppendToBuffer(ownerUnit, new TeammateOwnerInfoBuffer
		{
			TeammateType = teammateData.TeammateType,
			TeammateEntity = entity
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void StopKeepCasting_0024BurstManaged(in EntityManager em, in Entity attachTarget, in Entity spell)
	{
		if (em.HasBuffer<UnitKeepCastingSpells>(attachTarget))
		{
			DynamicBuffer<UnitKeepCastingSpells> buffer = em.GetBuffer<UnitKeepCastingSpells>(attachTarget);
			for (int i = 0; i < buffer.Length; i++)
			{
				if (!(buffer[i].Spell != spell))
				{
					buffer.RemoveAt(i);
					break;
				}
			}
			em.SetComponentData(spell, new SpellKeepCastingCleanup
			{
				OwnerUnit = Entity.Null
			});
		}
		if (em.HasComponent<SpellKeepCastingAttach>(spell))
		{
			em.SetComponentEnabled<SpellKeepCastingAttach>(spell, value: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void AttackRollball_0024BurstManaged(this in EntityCommandBuffer.ParallelWriter CMD, int sortKey, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity)
	{
		if (rollballConfig.Float1 > 0f)
		{
			rollballConfig.Float1 -= damage;
			CMD.SetComponent(sortKey, rollballEntity, new Spell1002RollBallBeHitTimer
			{
				BeHitTimer = 0.1f
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void AttackRollball_0024BurstManaged(in EntityManager manager, float damage, ref SpellConfigComponentData rollballConfig, in Entity rollballEntity)
	{
		if (rollballConfig.Float1 > 0f)
		{
			rollballConfig.Float1 -= damage;
			manager.SetComponentData(rollballEntity, new Spell1002RollBallBeHitTimer
			{
				BeHitTimer = 0.1f
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void GetSpellElementDataWithTimeScale_0024BurstManaged(in SpellElementEffectComponentData element, in DynamicOptimizeData optimizeData, out SpellElementEffectComponentData result)
	{
		result = element;
		float lowFpsActiveThreshold = GetLowFpsActiveThreshold(optimizeData.IsMobilePlatform);
		if (optimizeData.IsLowFpsOptimizeActive(lowFpsActiveThreshold))
		{
			result.VenomApplyCount *= optimizeData.LastFrameTimeScale;
		}
	}
}
