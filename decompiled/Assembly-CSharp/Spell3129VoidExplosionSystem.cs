using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;

[UpdateInGroup(typeof(EnhanceSpellSystemGroup), OrderLast = true)]
[BurstCompile]
[CompilerGenerated]
internal struct Spell3129VoidExplosionSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007633_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007633_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007633_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00007634_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007634_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007634_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnUpdate_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1012693555_0;

	private EntityQuery __query_1012693555_1;

	private EntityQuery __query_1012693555_2;

	private EntityQuery __query_1012693555_3;

	private EntityQuery __query_1012693555_4;

	private EntityQuery __query_1012693555_5;

	private EntityQuery __query_1012693555_6;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.EntityManager.CreateSingletonBuffer<Spell3129DamageRequestBuffer>();
		state.RequireForUpdate<Spell3129DamageRequestBuffer>();
		state.RequireForUpdate<Spell3129DamageInfoBuffer>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency.Complete();
		float deltaTime = state.WorldUnmanaged.Time.DeltaTime;
		DynamicBuffer<Spell3129DamageRequestBuffer> singletonBuffer = __query_1012693555_0.GetSingletonBuffer<Spell3129DamageRequestBuffer>();
		DynamicBuffer<Spell3129DamageInfoBuffer> singletonBuffer2 = __query_1012693555_1.GetSingletonBuffer<Spell3129DamageInfoBuffer>();
		DynamicBuffer<Spell3129TrailBuffer> singletonBuffer3 = __query_1012693555_2.GetSingletonBuffer<Spell3129TrailBuffer>();
		PhysicsWorldSingleton pws = __query_1012693555_3.GetSingleton<PhysicsWorldSingleton>();
		GlobalRandom singleton = __query_1012693555_4.GetSingleton<GlobalRandom>();
		DynamicOptimizeData singleton2 = __query_1012693555_5.GetSingleton<DynamicOptimizeData>();
		DynamicBuffer<SEData> singletonBuffer4 = __query_1012693555_6.GetSingletonBuffer<SEData>();
		for (int num = singletonBuffer.Length - 1; num >= 0; num--)
		{
			Spell3129DamageRequestBuffer value = singletonBuffer[num];
			value.Timer += deltaTime;
			if (value.Timer < 0.2f)
			{
				singletonBuffer[num] = value;
			}
			else
			{
				singletonBuffer4.Add(new SEData("SE_Spell3129VoidExplosion"));
				NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
				ref float3 spawnPosition = ref value.SpawnPosition;
				float explosionRange = value.voidExplosionData.ExplosionRange;
				ComponentLookup<UnitProperty_Dots> cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state);
				DTool.GetEnemyEntityInRange(in spawnPosition, explosionRange, UnitType.Player, containsBrittleness: false, in cluUnitPpt, in pws, ref result);
				if (result.Length <= 0)
				{
					singletonBuffer.RemoveAt(num);
				}
				else
				{
					Unity.Mathematics.Random random = singleton.NewRandom();
					result.ShuffleNativeList(ref random);
					int num2 = math.min(5, result.Length);
					float damage = value.MaxHp * value.voidExplosionData.HpToDmgRatio / (float)num2;
					for (int i = 0; i < num2 && i < result.Length; i++)
					{
						bool flag = true;
						if (singleton2.IsLowFpsOptimizeActive(10f))
						{
							flag = false;
						}
						else if (singleton2.IsLowFpsOptimizeActive(30f))
						{
							flag = singleton.NewRandom().NextFloat(0f, 1f) <= singleton2.CurrentFPS / 30f;
						}
						float num3 = singleton.NewRandom().NextFloat(0.1f, 0.2f);
						if (flag)
						{
							singletonBuffer3.Add(new Spell3129TrailBuffer
							{
								StartPos = value.SpawnPosition,
								Duration = num3,
								TargetEntity = result[i]
							});
						}
						singletonBuffer2.Add(new Spell3129DamageInfoBuffer
						{
							Damage = damage,
							DamageRemainTimer = num3 + 0.3f,
							voidExplosionData = singletonBuffer[num].voidExplosionData,
							TargetEntity = result[i]
						});
					}
					singletonBuffer.RemoveAt(num);
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3129DamageRequestBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3129DamageInfoBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3129TrailBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1012693555_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00007633_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007634_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell3129VoidExplosionSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell3129VoidExplosionSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell3129VoidExplosionSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
