using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Spell2003SplitTentacleShooterSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__SpellElementEffectComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007245_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007245_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007245_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00007246_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007246_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007246_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1475872369_0;

	private EntityQuery __query_1475872369_1;

	private EntityQuery __query_1475872369_2;

	private EntityQuery __query_1475872369_3;

	private EntityQuery __query_1475872369_4;

	private EntityQuery __query_1475872369_5;

	private EntityQuery __query_1475872369_6;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<Teammate3SplitTentacleSpawnerData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		if (!state.EntityManager.HasBuffer<Teammate3SplitTentacleSpawnerData>(__query_1475872369_0.GetSingletonEntity()))
		{
			return;
		}
		DynamicBuffer<Teammate3SplitTentacleSpawnerData> buffer = state.EntityManager.GetBuffer<Teammate3SplitTentacleSpawnerData>(__query_1475872369_0.GetSingletonEntity());
		if (buffer.Length <= 0)
		{
			return;
		}
		EntityCommandBuffer.ParallelWriter cmd = __query_1475872369_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		DynamicBuffer<SEData> singletonBuffer = __query_1475872369_2.GetSingletonBuffer<SEData>();
		GlobalRandom singleton = __query_1475872369_3.GetSingleton<GlobalRandom>();
		float deltaTime = state.WorldUnmanaged.Time.DeltaTime;
		SpellSingleton spellSingleton = __query_1475872369_4.GetSingleton<SpellSingleton>();
		PhysicsWorldSingleton physics = __query_1475872369_5.GetSingleton<PhysicsWorldSingleton>();
		for (int num = buffer.Length - 1; num >= 0; num--)
		{
			Teammate3SplitTentacleSpawnerData value = buffer[num];
			value.SpawnDelayTimer -= deltaTime;
			buffer[num] = value;
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter))
			{
				buffer.RemoveAt(num);
			}
			else if (!(value.SpawnDelayTimer > 0f))
			{
				NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
				ref float3 targetPosition = ref value.TargetPosition;
				float radius = 3f;
				UnitType selfCamp = UnitType.Player;
				ComponentLookup<UnitProperty_Dots> unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state);
				ComponentLookup<SpellConfigComponentData> SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state);
				SpellTools.GetAttackableEntitiesInRange(in targetPosition, in radius, in selfCamp, containsBrittleness: true, in unitPptLookup, in SpellConfigLookup, in physics, ref entities);
				FixedString32Bytes seName = "Attack";
				singletonBuffer.Add(new SEData(DTool.GetSpellSEName(2003, in seName), SEPlayMode.Replay, 3, 0.1f));
				SpellConfigComponentData config = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, value.Shooter);
				SpellMovementComponentData spellMovement = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state, value.Shooter);
				SpellComponentData data = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, value.Shooter);
				Entity singletonEntity = __query_1475872369_6.GetSingletonEntity();
				SpellElementEffectComponentData spellElementEffect = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state, value.Shooter);
				LocalTransform spellTransform = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter);
				for (int i = 0; i < value.SplitCount; i++)
				{
					FixedString32Bytes colorName;
					if (i >= entities.Length)
					{
						float3 position = singleton.random.NextFloat3Direction();
						float3 @float = DTool.IgnoreZPosition(in position);
						float num2 = singleton.random.NextFloat(0.5f, 3f);
						float3 position2 = value.TargetPosition + @float * num2;
						seName = "Spike";
						cmd.CreateSpellEffect(0, in spellSingleton, in data, in config, in position2, in seName, 0.8f, in float3.zero);
						seName = "SpikeHit";
						colorName = "General";
						cmd.CreateSpecificSpellEffect(0, in seName, in colorName, in spellSingleton, in config, in position2, in float3.zero, 0.8f);
						cmd.CheckFallThunderDamage(0, singletonEntity, position2, InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state), physics, in config, in spellMovement, in spellTransform, in spellElementEffect, in data, value.Shooter);
						continue;
					}
					Entity shooter = value.Shooter;
					LocalTransform transform = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter);
					SpellElementEffectComponentData elementEffect = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state, value.Shooter);
					TakeDamageInfo_Dots.NewInfo(shooter, CostPenetrate: false, in config, in spellMovement, in transform, in elementEffect, in data, out var info);
					info.spell.CostPenetrate = false;
					info.spell.CostRefraction = false;
					AttributeValue damage = config.Damage;
					damage.MulRatio *= 0.35f;
					info.damage = damage.Calculate();
					Entity target = entities[i];
					unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state);
					SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state);
					cmd.TryAttackEntity(0, in target, in info, in unitPptLookup, in SpellConfigLookup, checkCamp: false);
					float3 position3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entities[i]).Position;
					seName = "Spike";
					cmd.CreateSpellEffect(0, in spellSingleton, in data, in config, in position3, in seName, 0.8f, in float3.zero);
					seName = "SpikeHit";
					colorName = "General";
					cmd.CreateSpecificSpellEffect(0, in seName, in colorName, in spellSingleton, in config, in position3, in float3.zero, 0.8f);
					cmd.CheckFallThunderDamage(0, singletonEntity, position3, InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state), physics, in config, in spellMovement, in spellTransform, in spellElementEffect, in data, value.Shooter);
				}
				buffer[num] = value;
				buffer.RemoveAt(num);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Teammate3SplitTentacleSpawnerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872369_6 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00007245_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007246_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell2003SplitTentacleShooterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2003SplitTentacleShooterSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2003SplitTentacleShooterSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
