using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[CompilerGenerated]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
internal struct Spell2003InvisibleTentacleShooterSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__SpellElementEffectComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>(isReadOnly: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1475872498_0;

	private EntityQuery __query_1475872498_1;

	private EntityQuery __query_1475872498_2;

	private EntityQuery __query_1475872498_3;

	private EntityQuery __query_1475872498_4;

	private EntityQuery __query_1475872498_5;

	private EntityQuery __query_1475872498_6;

	private EntityQuery __query_1475872498_7;

	private EntityQuery __query_1475872498_8;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<Teammate3InvisibleTentacleSpawnerData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if (!state.EntityManager.HasBuffer<Teammate3InvisibleTentacleSpawnerData>(__query_1475872498_0.GetSingletonEntity()))
		{
			return;
		}
		DynamicBuffer<Teammate3InvisibleTentacleSpawnerData> buffer = state.EntityManager.GetBuffer<Teammate3InvisibleTentacleSpawnerData>(__query_1475872498_0.GetSingletonEntity());
		if (buffer.Length <= 0)
		{
			return;
		}
		EntityCommandBuffer.ParallelWriter cmd = __query_1475872498_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		DynamicBuffer<SEData> singletonBuffer = __query_1475872498_2.GetSingletonBuffer<SEData>();
		GlobalRandom singleton = __query_1475872498_3.GetSingleton<GlobalRandom>();
		float deltaTime = state.WorldUnmanaged.Time.DeltaTime;
		SpellSingleton spellSingleton = __query_1475872498_4.GetSingleton<SpellSingleton>();
		PhysicsWorldSingleton physics = __query_1475872498_5.GetSingleton<PhysicsWorldSingleton>();
		PlayerController_Dots singleton2 = __query_1475872498_6.GetSingleton<PlayerController_Dots>();
		Entity singletonEntity = __query_1475872498_7.GetSingletonEntity();
		for (int num = buffer.Length - 1; num >= 0; num--)
		{
			Teammate3InvisibleTentacleSpawnerData value = buffer[num];
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter))
			{
				buffer.RemoveAt(num);
			}
			else
			{
				value.RemainDuration -= deltaTime;
				value.SpawnTimer += deltaTime;
				if (value.SpawnTimer >= 0.06f)
				{
					value.SpawnTimer -= 0.06f;
					SpellConfigComponentData config = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, value.Shooter);
					SpellMovementComponentData spellMovement = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state, value.Shooter);
					SpellComponentData data = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, value.Shooter);
					float3 end;
					switch (spellMovement.Type)
					{
					case SpellSpecialMovementType.ChaseEnemy:
						if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Target))
						{
							LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Target);
							ref float3 direction2 = ref value.Direction;
							end = DTool.IgnoreZDir(in componentAfterCompletingDependency.Position, in value.CurrentPosition);
							value.Direction = DTool.DirMoveTowardsIgnoreZ(in direction2, in end, value.Speed * spellMovement.ChaseRotateSpeed * deltaTime);
						}
						else
						{
							__query_1475872498_8.GetSingleton<CurrentRoomEntitiesSingleton>().FindNearestTarget(value.CurrentPosition, UnitType.Teammate, out spellMovement.ChaseTarget, out end, out var _);
						}
						break;
					case SpellSpecialMovementType.ChaseMouse:
					{
						ref float3 direction = ref value.Direction;
						end = DTool.IgnoreZDir(in singleton2.mousePosition, in value.CurrentPosition);
						value.Direction = DTool.Lerp(in direction, in end, deltaTime * spellMovement.ChaseMouseLerpSpeed * value.Speed * 0.8f);
						break;
					}
					case SpellSpecialMovementType.Rotation:
					{
						value.Direction = float3.zero;
						float num2 = 360f / (MathF.PI * 2f * spellMovement.AroundRadius / value.Speed) * deltaTime;
						value.RotateAngle += num2;
						float3 @float = Tool2D.GetDir(value.RotateAngle + 90f);
						value.CurrentPosition = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter).Position + @float * spellMovement.AroundRadius;
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
					case SpellSpecialMovementType.Normal:
					case SpellSpecialMovementType.ChaseOwner:
						break;
					}
					value.CurrentPosition += value.Direction * value.Speed * state.WorldUnmanaged.Time.DeltaTime;
					end = singleton.random.NextFloat3Direction();
					float3 float2 = DTool.IgnoreZPosition(in end);
					float num3 = singleton.random.NextFloat(0f, 0.15f);
					float3 position = value.CurrentPosition + float2 * num3;
					FixedString32Bytes EffectName = "Spike";
					cmd.CreateSpellEffect(0, in spellSingleton, in data, in config, in position, in EffectName, 0.8f, in float3.zero);
					EffectName = "SpikeHit";
					FixedString32Bytes colorName = "General";
					cmd.CreateSpecificSpellEffect(0, in EffectName, in colorName, in spellSingleton, in config, in position, in float3.zero, 0.8f);
					NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
					ref float3 currentPosition = ref value.CurrentPosition;
					float radius = 0.7f;
					ref UnitType shooterType = ref config.ShooterType;
					ComponentLookup<UnitProperty_Dots> unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state);
					ComponentLookup<SpellConfigComponentData> SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state);
					SpellTools.GetAttackableEntitiesInRange(in currentPosition, in radius, in shooterType, containsBrittleness: true, in unitPptLookup, in SpellConfigLookup, in physics, ref entities);
					SpellElementEffectComponentData spellElementEffect = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state, value.Shooter);
					LocalTransform spellTransform = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter);
					cmd.CheckFallThunderDamage(0, singletonEntity, position, InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state), physics, in config, in spellMovement, in spellTransform, in spellElementEffect, in data, value.Shooter);
					if (entities.Length > 0)
					{
						EffectName = "Attack";
						singletonBuffer.Add(new SEData(DTool.GetSpellSEName(2003, in EffectName)));
					}
					foreach (Entity item in entities)
					{
						Entity target = item;
						Entity shooter = value.Shooter;
						LocalTransform transform = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value.Shooter);
						SpellElementEffectComponentData elementEffect = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state, value.Shooter);
						TakeDamageInfo_Dots.NewInfo(shooter, CostPenetrate: false, in config, in spellMovement, in transform, in elementEffect, in data, out var info);
						info.spell.CostPenetrate = false;
						info.spell.CostRefraction = false;
						info.damageRecordId = 3120;
						AttributeValue damage = config.Damage;
						damage.MulRatio *= 0.8f;
						info.damage = damage.Calculate();
						unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state);
						SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state);
						cmd.TryAttackEntity(0, in target, in info, in unitPptLookup, in SpellConfigLookup);
					}
				}
				buffer[num] = value;
				if (value.RemainDuration <= 0f)
				{
					buffer.RemoveAt(num);
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Teammate3InvisibleTentacleSpawnerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475872498_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((Spell2003InvisibleTentacleShooterSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell2003InvisibleTentacleShooterSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell2003InvisibleTentacleShooterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
