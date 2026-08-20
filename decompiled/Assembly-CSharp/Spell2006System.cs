using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
public class Spell2006System : SystemBase
{
	private struct SyncData
	{
		public Entity Entity;

		public GameObject TargetObject;

		public Teammate6Sync Teammate6SyncScript;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_295452819_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public SpellAspect.ResolvedChunk item3_ResolvedChunk;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, SpellAspect> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, SpellAspect>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2006Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item2_IntPtr, index), item3_ResolvedChunk[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2006Data> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<TeammateData> item2_ComponentTypeHandle_RW;

			private SpellAspect.TypeHandle item3_AspectTypeHandle;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2006Data>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
				item3_AspectTypeHandle = new SpellAspect.TypeHandle(ref systemState);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_AspectTypeHandle.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_ResolvedChunk = item3_AspectTypeHandle.Resolve(archetypeChunk);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, SpellAspect>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, SpellAspect> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<Spell2006Data>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
			default(SpellAspect).CompleteDependencyBeforeRW(ref state);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_295452819_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public SpellAspect.ResolvedChunk item6_ResolvedChunk;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, SpellAspect) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2006Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitBase_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PathFinding>(item5_IntPtr, index), item6_ResolvedChunk[index]);
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2006Data> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitProperty_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitBase_Dots> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<TeammateData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PathFinding> item5_ComponentTypeHandle_RW;

			private SpellAspect.TypeHandle item6_AspectTypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2006Data>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitBase_Dots>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PathFinding>();
				item6_AspectTypeHandle = new SpellAspect.TypeHandle(ref systemState);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_AspectTypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_ResolvedChunk = item6_AspectTypeHandle.Resolve(archetypeChunk);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, SpellAspect)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, SpellAspect) Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<Spell2006Data>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitBase_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
			state.EntityManager.CompleteDependencyBeforeRW<PathFinding>();
			default(SpellAspect).CompleteDependencyBeforeRW(ref state);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_295452819_2
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<TeammateData> item1_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateData>> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_295452819_0.TypeHandle __IFE_295452819_0_TypeHandle;

		public IFE_295452819_1.TypeHandle __IFE_295452819_1_TypeHandle;

		public IFE_295452819_2.TypeHandle __IFE_295452819_2_TypeHandle;

		[ReadOnly]
		public ComponentLookup<Spell2006GhostTag> __Spell2006GhostTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell2006FuseTag> __Spell2006FuseTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<TeammateDeadTag> __TeammateDeadTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellradiuDcreaseTransIntoDamageData> __SpellradiuDcreaseTransIntoDamageData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<TeammateData> __TeammateData_RO_ComponentLookup;

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<Spell2006Data> __Spell2006Data_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<Shadow_Dots> __Shadow_Dots_RW_ComponentLookup;

		public BufferLookup<Spell2004PillarBuffer> __Spell2004PillarBuffer_RW_BufferLookup;

		public BufferLookup<Spell2004WallBuffer> __Spell2004WallBuffer_RW_BufferLookup;

		public BufferLookup<Spell2007FuseBuffer> __Spell2007FuseBuffer_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_295452819_0_TypeHandle = new IFE_295452819_0.TypeHandle(ref state);
			__IFE_295452819_1_TypeHandle = new IFE_295452819_1.TypeHandle(ref state);
			__IFE_295452819_2_TypeHandle = new IFE_295452819_2.TypeHandle(ref state);
			__Spell2006GhostTag_RO_ComponentLookup = state.GetComponentLookup<Spell2006GhostTag>(isReadOnly: true);
			__Spell2006FuseTag_RO_ComponentLookup = state.GetComponentLookup<Spell2006FuseTag>(isReadOnly: true);
			__TeammateDeadTag_RO_ComponentLookup = state.GetComponentLookup<TeammateDeadTag>(isReadOnly: true);
			__SpellradiuDcreaseTransIntoDamageData_RO_ComponentLookup = state.GetComponentLookup<SpellradiuDcreaseTransIntoDamageData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__TeammateData_RO_ComponentLookup = state.GetComponentLookup<TeammateData>(isReadOnly: true);
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell2006Data_RW_ComponentLookup = state.GetComponentLookup<Spell2006Data>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Shadow_Dots_RW_ComponentLookup = state.GetComponentLookup<Shadow_Dots>();
			__Spell2004PillarBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2004PillarBuffer>();
			__Spell2004WallBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2004WallBuffer>();
			__Spell2007FuseBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2007FuseBuffer>();
		}
	}

	private static readonly int CloseAttack = Animator.StringToHash("CloseAttack");

	private static readonly int Catch = Animator.StringToHash("Catch");

	private static readonly int ThrowHook = Animator.StringToHash("throwHook");

	private static readonly int FastReload = Animator.StringToHash("FastReload");

	private readonly List<SyncData> syncData = new List<SyncData>();

	private static int TeammateID = 0;

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	private TypeHandle __TypeHandle;

	private EntityQuery __query_295452819_0;

	private EntityQuery __query_295452819_1;

	private EntityQuery __query_295452819_2;

	private EntityQuery __query_295452819_3;

	private EntityQuery __query_295452819_4;

	private EntityQuery __query_295452819_5;

	private EntityQuery __query_295452819_6;

	private EntityQuery __query_295452819_7;

	[Preserve]
	protected override void OnCreate()
	{
		TeammateID = 0;
	}

	[Preserve]
	protected override void OnUpdate()
	{
		float deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
		EntityCommandBuffer cMD = __query_295452819_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.EntityManager.WorldUnmanaged);
		EntityCommandBuffer.ParallelWriter cmd = cMD.AsParallelWriter();
		DynamicBuffer<GlobalParticleEmitParams> singletonBuffer = __query_295452819_4.GetSingletonBuffer<GlobalParticleEmitParams>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2006Data>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, SpellAspect> item9 in IFE_295452819_0.Query(__query_295452819_0, __TypeHandle.__IFE_295452819_0_TypeHandle, ref base.CheckedStateRef))
		{
			item9.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2006Data> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<TeammateData> uncheckedRefRW2 = item2;
			SpellAspect spell2 = item3;
			Entity entity2 = entity;
			int num = this.syncData.FindIndex((SyncData x) => x.Entity == spell2.Entity);
			if (num < 0)
			{
				continue;
			}
			Teammate6Sync teammate6SyncScript = this.syncData[num].Teammate6SyncScript;
			if (!uncheckedRefRW.ValueRO.ActiveGhostEffect && InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2006GhostTag_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				teammate6SyncScript.OnEnterDelayDeathEvent();
				uncheckedRefRW.ValueRW.ActiveGhostEffect = true;
			}
			if (!uncheckedRefRW.ValueRO.ActiveFuseEffect && InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2006FuseTag_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				teammate6SyncScript.OnEnterFuseStateEvent();
				uncheckedRefRW.ValueRW.ActiveFuseEffect = true;
				CheckBombExplosionState(teammate6SyncScript, spell2, cMD, cmd, killAllBomb: true);
			}
			if (uncheckedRefRW2.ValueRO.IsHoldByTeammate6 && teammate6SyncScript.HookTeammateDataList.Count > 0)
			{
				foreach (var hookTeammateData in teammate6SyncScript.HookTeammateDataList)
				{
					PlayerMgr.Inst.MiniPool.RecycleGO(hookTeammateData.Hook.gameObject);
				}
				teammate6SyncScript.HookTeammateDataList.Clear();
			}
			if (!InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RO_ComponentLookup, ref base.CheckedStateRef, entity2) && (!uncheckedRefRW2.ValueRO.IsHoldByTeammate6 || teammate6SyncScript.teammateBombList.Count <= 0))
			{
				continue;
			}
			foreach (Teammate6Sync.HoldingTeammateData teammateBomb in teammate6SyncScript.teammateBombList)
			{
				teammateBomb.BombPosition = teammateBomb.BombOutlookObject.transform.position;
			}
			CheckBombExplosionState(teammate6SyncScript, spell2, cMD, cmd, killAllBomb: true);
			teammate6SyncScript.teammateBombList.Clear();
			foreach (var hookTeammateData2 in teammate6SyncScript.HookTeammateDataList)
			{
				PlayerMgr.Inst.MiniPool.RecycleGO(hookTeammateData2.Hook.gameObject);
			}
			teammate6SyncScript.HookTeammateDataList.Clear();
		}
		for (int num2 = this.syncData.Count - 1; num2 >= 0; num2--)
		{
			if (!base.EntityManager.HasComponent<LocalTransform>(this.syncData[num2].Entity) || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RO_ComponentLookup, ref base.CheckedStateRef, this.syncData[num2].Entity))
			{
				Teammate6Sync teammate6SyncScript2 = this.syncData[num2].Teammate6SyncScript;
				foreach (Teammate6Sync.HoldingTeammateData teammateBomb2 in teammate6SyncScript2.teammateBombList)
				{
					if (!teammateBomb2.BombOutlookObject.IsDestroyed())
					{
						teammateBomb2.BombPosition = teammateBomb2.BombOutlookObject.transform.position;
						KillTargetBomb(teammateBomb2, cMD);
					}
				}
				teammate6SyncScript2.teammateBombList.Clear();
				foreach (var hookTeammateData3 in teammate6SyncScript2.HookTeammateDataList)
				{
					if (!hookTeammateData3.Hook.IsDestroyed())
					{
						PlayerMgr.Inst.MiniPool.RecycleGO(hookTeammateData3.Hook.gameObject);
					}
				}
				teammate6SyncScript2.HookTeammateDataList.Clear();
				if (!this.syncData[num2].TargetObject.IsDestroyed())
				{
					PlayerMgr.Inst.MiniPool.RecycleGO(this.syncData[num2].TargetObject);
				}
				this.syncData.RemoveAt(num2);
			}
		}
		foreach (var item10 in IFE_295452819_1.Query(__query_295452819_1, __TypeHandle.__IFE_295452819_1_TypeHandle, ref base.CheckedStateRef))
		{
			InternalCompilerInterface.UncheckedRefRW<Spell2006Data> item4 = item10.Item1;
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> item5 = item10.Item2;
			InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots> item6 = item10.Item3;
			InternalCompilerInterface.UncheckedRefRW<TeammateData> item7 = item10.Item4;
			InternalCompilerInterface.UncheckedRefRW<PathFinding> item8 = item10.Item5;
			SpellAspect spell = item10.Item6;
			Entity entity3 = spell.Entity;
			RefRW<SpellMovementComponentData> movement = spell.Movement;
			RefRW<SpellConfigComponentData> config = spell.Config;
			RefRW<LocalTransform> transform = spell.Transform;
			if (!item4.ValueRW.IsInitialized)
			{
				item4.ValueRW.IsInitialized = true;
				GameObject gO = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/2006/2006_Body");
				gO.transform.localScale = transform.ValueRO.Scale * Vector3.one;
				Teammate6Sync component = gO.GetComponent<Teammate6Sync>();
				component.DataInitialize(item7.ValueRW, config);
				this.syncData.Add(new SyncData
				{
					Entity = entity3,
					TargetObject = gO,
					Teammate6SyncScript = component
				});
				item4.ValueRW.CloseAttackRange = config.ValueRO.Radius.CalculateWithNewBaseValue(2.8f);
				item4.ValueRW.MeleeAttackDamage = spell.Config.ValueRW.Damage.CalculateWithNewBaseValue(96f * (float)(item7.ValueRW.TeammateCurrentFuseLevel + 1));
				item4.ValueRW.SoulBombRange = config.ValueRO.Radius.CalculateWithNewBaseValue(3.8f);
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellradiuDcreaseTransIntoDamageData_RO_ComponentLookup, ref base.CheckedStateRef, entity3))
				{
					SpellradiuDcreaseTransIntoDamageData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellradiuDcreaseTransIntoDamageData_RO_ComponentLookup, ref base.CheckedStateRef, entity3);
					item4.ValueRW.MeleeAttackDamage *= GeneralTool.GetSpellRadiusToDamageRatio(item4.ValueRW.CloseAttackRange, componentAfterCompletingDependency.radiuDecreaseRatio, componentAfterCompletingDependency.radiuDcreaseTransIntoDamageRatio);
					item4.ValueRW.SBDecreaseRadiusToDamageRatio = GeneralTool.GetSpellRadiusToDamageRatio(item4.ValueRW.SoulBombRange, componentAfterCompletingDependency.radiuDecreaseRatio, componentAfterCompletingDependency.radiuDcreaseTransIntoDamageRatio);
				}
				item4.ValueRW.MaxHookCount = (item7.ValueRO.TeammateCurrentFuseLevel + 1) * item7.ValueRO.AdvanceSkillLevel * 4;
				item4.ValueRW.KillCounter = spell.Config.ValueRO.Int1;
				item4.ValueRW.UID = TeammateID;
				TeammateID++;
			}
			if (item4.ValueRW.CurrentKillCounter != item4.ValueRW.KillCounter)
			{
				float num3 = (1f + (float)item4.ValueRW.KillCounter / 100f) / (1f + (float)item4.ValueRW.CurrentKillCounter / 100f);
				int num4 = item4.ValueRW.KillCounter - item4.ValueRW.CurrentKillCounter;
				item4.ValueRW.CurrentKillCounter = item4.ValueRW.KillCounter;
				float num5 = item5.ValueRW.unitCfg.currentHP / item5.ValueRW.unitCfg.maxHP;
				item5.ValueRW.unitCfg.maxHP *= num3;
				item5.ValueRW.unitCfg.currentHP = item5.ValueRW.unitCfg.maxHP * num5;
				spell.Config.ValueRW.CriticalChance += 0.01f * (float)num4;
			}
			int num6 = this.syncData.FindIndex((SyncData x) => x.Entity == spell.Entity);
			if (num6 < 0)
			{
				continue;
			}
			SyncData syncData = this.syncData[num6];
			GameObject targetObject = syncData.TargetObject;
			Teammate6Sync teammate6SyncScript3 = syncData.Teammate6SyncScript;
			bool flag = item4.ValueRO.IsFaceRight;
			CheckBombExplosionState(teammate6SyncScript3, spell, cMD, cmd);
			UpdateBodyTransparency(teammate6SyncScript3);
			deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime * item7.ValueRO.TeammateSpeedRatio;
			deltaTime = ((UnityEngine.Time.timeScale != 0f) ? (deltaTime / UnityEngine.Time.timeScale) : 0f);
			if (!base.EntityManager.HasComponent<LocalTransform>(syncData.Entity) || targetObject.IsDestroyed() || !targetObject.activeInHierarchy || !teammate6SyncScript3.isActiveAndEnabled || item7.ValueRO.IsHoldByTeammate6 || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RO_ComponentLookup, ref base.CheckedStateRef, spell.Entity) || targetObject == null || !targetObject.activeInHierarchy)
			{
				continue;
			}
			Teammate6State currentState = item4.ValueRW.CurrentState;
			if (currentState == Teammate6State.Idle || currentState == Teammate6State.Move || currentState == Teammate6State.SeekingAmmo || currentState == Teammate6State.CloseAttack)
			{
				item4.ValueRW.RecheckTargetTeammateTimer += deltaTime;
				if (item4.ValueRO.TargetTeammate == Entity.Null || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item4.ValueRO.TargetTeammate) || item4.ValueRO.RecheckTargetTeammateTimer >= 0.1f)
				{
					item4.ValueRW.RecheckTargetTeammateTimer = 0f;
					FindNearestSacrificebleTeammateEntity(item4, entity3, transform.ValueRO.Position, spell.Config);
				}
			}
			switch (item4.ValueRW.CurrentState)
			{
			case Teammate6State.Idle:
				item6.ValueRW.SetMove(Vector3.zero);
				item4.ValueRW.IdleTimer = 0f;
				item4.ValueRW.CurrentState = Teammate6State.Move;
				item4.ValueRW.RecheckTargetTimer = 0.1f;
				item4.ValueRW.IdleInterval = GetRandomIdleInterval();
				item4.ValueRW.IsIdleWalkCoolDown = true;
				item4.ValueRW.IdleWalkTimer = 0.3f;
				break;
			case Teammate6State.Move:
				if (item4.ValueRO.IsIdleWalkCoolDown)
				{
					item4.ValueRW.IdleWalkTimer += deltaTime;
					if (item4.ValueRW.IdleWalkTimer >= 1f)
					{
						item4.ValueRW.IsIdleWalkCoolDown = false;
						item4.ValueRW.IdleWalkTimer = 0f;
						teammate6SyncScript3.Anima.SetTrigger("Move");
						teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "Walk", isLoop: true);
						teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "Walk", isLoop: true);
						item4.ValueRW.TargetIdleWalkPoint = Tool2D.GetNavMeshPoint(transform.ValueRO.Position, UnityEngine.Random.Range(2f, 5f));
						item8.ValueRW.UpdatePath(transform.ValueRW.Position, item4.ValueRW.TargetIdleWalkPoint, item5.ValueRW.navAreaMask);
					}
				}
				else
				{
					if (!item8.ValueRW.allCornerArrived && math.abs(item8.ValueRW.walkToPoint.x - transform.ValueRW.Position.x) >= 0.1f)
					{
						flag = item8.ValueRW.walkToPoint.x >= transform.ValueRW.Position.x;
					}
					UpdatePathAndVelocity(item8, transform.ValueRW.Position, item4.ValueRW.TargetIdleWalkPoint, item6, movement.ValueRW.Speed * 0.4f, item5.ValueRW.navAreaMask);
					item4.ValueRW.IdleWalkTimer += deltaTime;
					if (item4.ValueRW.IdleWalkTimer >= item4.ValueRW.IdleWalkDuration || item8.ValueRW.allCornerArrived)
					{
						item4.ValueRW.IsIdleWalkCoolDown = true;
						item4.ValueRW.IdleWalkTimer = 0f;
						item4.ValueRW.CurrentState = Teammate6State.Idle;
						item6.ValueRW.SetMove(float3.zero);
						item4.ValueRW.IdleWalkDuration = UnityEngine.Random.Range(2f, 3f);
						teammate6SyncScript3.Anima.SetTrigger("Idle");
						teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "Idle", isLoop: true);
						teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "Idle", isLoop: true);
					}
				}
				item4.ValueRW.RecheckTargetTimer += deltaTime;
				if (!(item4.ValueRW.RecheckTargetTimer >= 0.1f))
				{
					break;
				}
				item4.ValueRW.RecheckTargetTimer -= 0.1f;
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRW.ChaseTarget))
				{
					if (teammate6SyncScript3.CheckIfCannonHasValidAmmo())
					{
						EnterState(item4, Teammate6State.ReadyToShootFindingTarget, item8, teammate6SyncScript3, movement, item6);
					}
					else if (item7.ValueRO.AdvanceSkillLevel > 0 && teammate6SyncScript3.CheckBackUpAmmoState())
					{
						EnterState(item4, Teammate6State.QuickReload, item8, teammate6SyncScript3, movement, item6);
					}
					else if (item4.ValueRW.TargetTeammate != Entity.Null)
					{
						EnterState(item4, Teammate6State.SeekingAmmo, item8, teammate6SyncScript3, movement, item6);
						item8.ValueRW.samplePointRequest.SetRequest(InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item4.ValueRW.TargetTeammate).ValueRO.Position);
					}
					else
					{
						EnterState(item4, Teammate6State.CloseAttack, item8, teammate6SyncScript3, movement, item6);
						item8.ValueRW.samplePointRequest.SetRequest(InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRW.ChaseTarget).ValueRO.Position);
					}
				}
				else
				{
					FindNearestEnemyTarget(transform.ValueRO.Position, movement);
				}
				break;
			case Teammate6State.SeekingAmmo:
			{
				if ((item4.ValueRW.TargetTeammate == Entity.Null || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item4.ValueRW.TargetTeammate)) && teammate6SyncScript3.teammateBombList.Count <= 0 && ((!item4.ValueRW.IsPickingTeammateP1 && item7.ValueRO.AdvanceSkillLevel <= 0) || (item7.ValueRO.AdvanceSkillLevel > 0 && !item4.ValueRW.IsStartThrowingHook)))
				{
					EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
					break;
				}
				float num10 = spell.Config.ValueRO.Radius.CalculateWithNewBaseValue(2.2f + (float)item7.ValueRO.TeammateCurrentFuseLevel * 0.5f);
				float3 @float = transform.ValueRO.Position;
				if (item7.ValueRO.AdvanceSkillLevel > 0)
				{
					if (!item4.ValueRO.IsStartThrowingHook)
					{
						NativeList<Entity> result4 = new NativeList<Entity>(Allocator.Temp);
						ref readonly float3 position4 = ref transform.ValueRO.Position;
						float checkRadius = item4.ValueRO.HookDetectRange * 0.8f;
						ComponentLookup<UnitProperty_Dots> cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
						PhysicsWorldSingleton pws = __query_295452819_5.GetSingleton<PhysicsWorldSingleton>();
						DTool.GetEnemyEntityInRange(in position4, checkRadius, UnitType.Monster, containsBrittleness: true, in cluUnitPpt, in pws, ref result4);
						if (result4.Length >= 0)
						{
							teammate6SyncScript3.Anima.SetTrigger(ThrowHook);
							teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "ThrowHookAfter", isLoop: true);
							teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "ThrowHookAfter", isLoop: true);
							item4.ValueRW.IsStartThrowingHook = true;
							item6.ValueRW.SetMove(Vector3.zero);
						}
					}
					if (!item4.ValueRW.IsStartThrowingHook)
					{
						break;
					}
					item4.ValueRW.ThrowHookTimer += deltaTime;
					if (!item4.ValueRW.IsHookOut && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item4.ValueRW.TargetTeammate))
					{
						@float = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item4.ValueRW.TargetTeammate).Position;
						flag = @float.x >= transform.ValueRO.Position.x;
					}
					if (!item4.ValueRW.IsHookOut && item4.ValueRO.ThrowHookTimer >= 1f)
					{
						item4.ValueRW.IsHookOut = true;
						NativeList<Entity> result5 = new NativeList<Entity>(Allocator.Temp);
						ref readonly float3 position5 = ref transform.ValueRO.Position;
						float hookDetectRange = item4.ValueRO.HookDetectRange;
						ComponentLookup<UnitProperty_Dots> cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
						PhysicsWorldSingleton pws = __query_295452819_5.GetSingleton<PhysicsWorldSingleton>();
						DTool.GetEnemyEntityInRange(in position5, hookDetectRange, UnitType.Monster, containsBrittleness: true, in cluUnitPpt, in pws, ref result5);
						if (result5.Length <= 0)
						{
							EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
						}
						int num11 = item4.ValueRO.MaxHookCount;
						foreach (Entity item11 in result5)
						{
							if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref base.CheckedStateRef, item11))
							{
								RefRW<TeammateData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, item11);
								RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, item11);
								RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, spell.Entity);
								if (item11 != spell.Entity && CheckTargetTeammateIsCatchable(componentRWAfterCompletingDependency.ValueRW, spell.Entity, item11) && (componentRWAfterCompletingDependency.ValueRO.TeammateType != TeammateType.teammate6 || IsTargetTeammate6Stronger(componentRWAfterCompletingDependency.ValueRO, componentRWAfterCompletingDependency3.ValueRO, componentRWAfterCompletingDependency2.ValueRO, item4.ValueRO, InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RW_ComponentLookup, ref base.CheckedStateRef, item11).ValueRO)))
								{
									teammate6SyncScript3.ThrowHook(item11, 0.4f);
									num11--;
								}
								if (num11 <= 0)
								{
									break;
								}
							}
						}
					}
					if (!item4.ValueRW.IsHookCatchTarget && item4.ValueRO.ThrowHookTimer >= 1.2f && item4.ValueRW.IsHookOut)
					{
						item4.ValueRW.IsHookCatchTarget = true;
						int num12 = 0;
						foreach (var hookTeammateData4 in teammate6SyncScript3.HookTeammateDataList)
						{
							if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, hookTeammateData4.TargetEntity))
							{
								RefRW<TeammateData> componentRWAfterCompletingDependency4 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, hookTeammateData4.TargetEntity);
								RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency5 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, hookTeammateData4.TargetEntity);
								RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency6 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, hookTeammateData4.TargetEntity);
								if (CheckTargetTeammateIsCatchable(componentRWAfterCompletingDependency4.ValueRW, spell.Entity, hookTeammateData4.TargetEntity))
								{
									componentRWAfterCompletingDependency4.ValueRW.IsHoldByTeammate6 = true;
									componentRWAfterCompletingDependency4.ValueRW.SummonFollowOwnerThroughMapChance += 1f;
									Teammate6Sync.HoldingTeammateData holdingTeammateData2 = new Teammate6Sync.HoldingTeammateData
									{
										SoulBombDamage = (config.ValueRO.Damage.CalculateWithNewBaseValue(componentRWAfterCompletingDependency5.ValueRO.unitCfg.maxHP * config.ValueRO.Float3) - config.ValueRO.Damage.Extra) * item4.ValueRO.SBDecreaseRadiusToDamageRatio + config.ValueRO.Damage.Extra,
										SoulBombRange = item4.ValueRW.SoulBombRange,
										BombTeammateEntity = hookTeammateData4.TargetEntity,
										targetCannonScript = teammate6SyncScript3.CannonControllerList[teammate6SyncScript3.GetCannonData(num12).cannonIndex],
										BombOutlookObject = teammate6SyncScript3.SpawnTeammateBall(transform.ValueRO, componentRWAfterCompletingDependency4.ValueRO, componentRWAfterCompletingDependency6.ValueRO.ColorType),
										BombType = componentRWAfterCompletingDependency4.ValueRO.TeammateType,
										Movement = movement.ValueRO,
										BombOwnerEntity = entity3,
										BombScale = 1f,
										state = Teammate6BombState.Hook_Backing,
										LinkedHook = hookTeammateData4.Hook
									};
									HideTargetTeammateBomb(hookTeammateData4.TargetEntity, holdingTeammateData2);
									teammate6SyncScript3.teammateBombList.Add(holdingTeammateData2);
									num12++;
								}
							}
						}
					}
					if (!(item4.ValueRO.ThrowHookTimer >= 1.416f))
					{
						break;
					}
					teammate6SyncScript3.UpdateHoldingTeammateState();
					foreach (Teammate6Sync.HoldingTeammateData teammateBomb3 in teammate6SyncScript3.teammateBombList)
					{
						teammateBomb3.state = Teammate6BombState.Holding_BackUpAmmo;
					}
					foreach (var hookTeammateData5 in teammate6SyncScript3.HookTeammateDataList)
					{
						PlayerMgr.Inst.MiniPool.RecycleGO(hookTeammateData5.Hook.gameObject);
					}
					teammate6SyncScript3.HookTeammateDataList.Clear();
					if (teammate6SyncScript3.teammateBombList.Count > 0)
					{
						EnterState(item4, Teammate6State.QuickReload, item8, teammate6SyncScript3, movement, item6);
					}
					else
					{
						EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
					}
					break;
				}
				if (!item4.ValueRO.IsPickingTeammateP1)
				{
					@float = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item4.ValueRW.TargetTeammate).Position;
					flag = @float.x >= transform.ValueRO.Position.x;
					UpdatePathAndVelocity(item8, transform.ValueRW.Position, @float, item6, movement.ValueRW.Speed * 1.2f, item5.ValueRW.navAreaMask);
				}
				if (!item4.ValueRO.IsPickingTeammateP1 && DTool.IgnoreZDistanceSqr(in transform.ValueRO.Position, in @float) <= num10 * num10)
				{
					teammate6SyncScript3.Anima.SetTrigger(Catch);
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "PickUpTeammate1", isLoop: true);
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "PickUpTeammate1", isLoop: true);
					item6.ValueRW.SetMove(Vector3.zero);
					item4.ValueRW.IsPickingTeammateP1 = true;
				}
				if (!item4.ValueRW.IsPickingTeammateP1)
				{
					break;
				}
				item4.ValueRW.PickingTeammateTimer += deltaTime;
				Spell2006Data valueRW = item4.ValueRW;
				if (valueRW.PickingTeammateTimer >= 0.44f && !valueRW.IsPickingTeammateP2)
				{
					item4.ValueRW.IsPickingTeammateP2 = true;
					SEMgr.Inst.teammate6RangeLoad.PlaySE();
					int num13 = 0;
					int num14 = 0;
					while (item4.ValueRO.TargetTeammate != Entity.Null && num13 <= item7.ValueRO.TeammateCurrentFuseLevel && num14 <= item7.ValueRO.TeammateCurrentFuseLevel)
					{
						num14++;
						FindNearestSacrificebleTeammateEntity(item4, entity3, transform.ValueRO.Position, InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, entity3));
						Entity targetTeammate = item4.ValueRW.TargetTeammate;
						if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref base.CheckedStateRef, targetTeammate))
						{
							RefRW<TeammateData> componentRWAfterCompletingDependency7 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, targetTeammate);
							RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency8 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetTeammate);
							RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency9 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, targetTeammate);
							RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency10 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, spell.Entity);
							if (num13 <= item7.ValueRO.TeammateCurrentFuseLevel && CheckTargetTeammateIsCatchable(componentRWAfterCompletingDependency7.ValueRW, spell.Entity, targetTeammate) && (componentRWAfterCompletingDependency7.ValueRO.TeammateType != TeammateType.teammate6 || IsTargetTeammate6Stronger(componentRWAfterCompletingDependency7.ValueRO, componentRWAfterCompletingDependency10.ValueRO, componentRWAfterCompletingDependency9.ValueRO, item4.ValueRO, InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RW_ComponentLookup, ref base.CheckedStateRef, targetTeammate).ValueRO)))
							{
								componentRWAfterCompletingDependency7.ValueRW.IsHoldByTeammate6 = true;
								componentRWAfterCompletingDependency7.ValueRW.SummonFollowOwnerThroughMapChance = 1f;
								Teammate6Sync.HoldingTeammateData holdingTeammateData3 = new Teammate6Sync.HoldingTeammateData
								{
									SoulBombDamage = (config.ValueRO.Damage.CalculateWithNewBaseValue(componentRWAfterCompletingDependency8.ValueRO.unitCfg.maxHP * config.ValueRO.Float3) - config.ValueRO.Damage.Extra) * item4.ValueRO.SBDecreaseRadiusToDamageRatio + config.ValueRO.Damage.Extra,
									SoulBombRange = item4.ValueRW.SoulBombRange,
									BombTeammateEntity = targetTeammate,
									targetCannonScript = teammate6SyncScript3.CannonControllerList[num13],
									BombOutlookObject = teammate6SyncScript3.SpawnTeammateBall(transform.ValueRO, componentRWAfterCompletingDependency7.ValueRO, componentRWAfterCompletingDependency9.ValueRO.ColorType),
									BombType = componentRWAfterCompletingDependency7.ValueRO.TeammateType,
									Movement = movement.ValueRO,
									BombOwnerEntity = entity3,
									BombScale = 1f
								};
								HideTargetTeammateBomb(targetTeammate, holdingTeammateData3);
								num13++;
								teammate6SyncScript3.teammateBombList.Add(holdingTeammateData3);
							}
						}
					}
				}
				if (item4.ValueRW.PickingTeammateTimer >= 1f)
				{
					if (teammate6SyncScript3.teammateBombList.Count > 0)
					{
						FindNearestEnemyTarget(transform.ValueRO.Position, movement);
						EnterState(item4, Teammate6State.ReadyToShootFindingTarget, item8, teammate6SyncScript3, movement, item6);
					}
					else
					{
						EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
					}
				}
				break;
			}
			case Teammate6State.ReadyToShootFindingTarget:
			{
				item4.ValueRW.RecheckTargetTimer += deltaTime;
				if (item4.ValueRW.RecheckTargetTimer >= 0.1f)
				{
					item4.ValueRW.RecheckTargetTimer -= 0.1f;
					FindNearestEnemyTarget(transform.ValueRO.Position, movement);
				}
				if (!IsEnemyTargetValid(movement.ValueRO.ChaseTarget))
				{
					FindNearestEnemyTarget(transform.ValueRO.Position, movement);
					if (!IsEnemyTargetValid(movement.ValueRO.ChaseTarget))
					{
						ResetShootState(item4, teammate6SyncScript3);
						EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
						break;
					}
				}
				if (!item4.ValueRW.IsStartShoot)
				{
					teammate6SyncScript3.Anima.SetTrigger("Shoot");
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "Shoot", isLoop: true);
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "Shoot", isLoop: true);
					float3 position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget).Position;
					foreach (Teammate6Sync.HoldingTeammateData teammateBomb4 in teammate6SyncScript3.teammateBombList)
					{
						if (teammateBomb4.state == Teammate6BombState.Holding_Barrel)
						{
							teammateBomb4.BombTargetEndPosition = position;
							teammateBomb4.duration = 3f + config.ValueRO.Duration.Extra;
							teammateBomb4.BombPosition = teammateBomb4.targetCannonScript.ShootPosition.position;
							teammateBomb4.direction = Tool2D.IgnoreZV2ToV1(position, teammateBomb4.BombPosition).normalized;
						}
					}
					teammate6SyncScript3.barrelLockingTarget = true;
					item4.ValueRW.IsStartShoot = true;
				}
				if (!item4.ValueRW.IsStartShoot)
				{
					break;
				}
				float3 position2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget).Position;
				foreach (Teammate6Sync.HoldingTeammateData teammateBomb5 in teammate6SyncScript3.teammateBombList)
				{
					teammateBomb5.BombTargetEndPosition = position2;
				}
				teammate6SyncScript3.CannonLookingTargetPosition = position2;
				flag = position2.x >= transform.ValueRO.Position.x;
				item4.ValueRW.ShootingTimer += deltaTime;
				if (item4.ValueRO.ShootingTimer >= 0.53f && !item4.ValueRW.IsBombShoot)
				{
					item4.ValueRW.IsBombShoot = true;
					teammate6SyncScript3.barrelLockingTarget = false;
					foreach (Teammate6Sync.HoldingTeammateData teammateBomb6 in teammate6SyncScript3.teammateBombList)
					{
						if (teammateBomb6.state == Teammate6BombState.Holding_Barrel)
						{
							Vector3 position3 = teammateBomb6.targetCannonScript.ShootPosition.position;
							teammateBomb6.state = Teammate6BombState.Shooting;
							Vector3 normalized = Tool2D.IgnoreZV2ToV1(teammateBomb6.BombTargetEndPosition, position3).normalized;
							teammateBomb6.BombPosition = position3;
							teammateBomb6.direction = normalized.normalized;
							spell.Config.ValueRO.ColorType.ColorEnumToString(out var result3);
							GlobalParticleEmitParams elem2 = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"2006_CannonFIre_{result3}", position3)
							{
								Size = transform.ValueRO.Scale,
								Velocity = normalized
							};
							singletonBuffer.Add(elem2);
						}
					}
				}
				else if (item4.ValueRO.ShootingTimer >= 1f)
				{
					item4.ValueRW.IsBombShoot = false;
					item4.ValueRW.IsStartShoot = false;
					item4.ValueRW.ShootingTimer = 0f;
					EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
				}
				break;
			}
			case Teammate6State.CloseAttack:
			{
				if (item4.ValueRW.IsCloseAttacking)
				{
					item4.ValueRW.CloseAttackTimer += deltaTime;
				}
				if (item4.ValueRW.CloseAttackTimer >= 0.5f && !item4.ValueRO.SpawnCloseAttackShockWave)
				{
					item4.ValueRW.SpawnCloseAttackShockWave = true;
					NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
					Vector3 vector = SpellTools.IgnoreZ(transform.ValueRW.Position);
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget))
					{
						vector += ((Vector3)(InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget).Position - transform.ValueRO.Position)).normalized.IgnoreZ() * item4.ValueRO.CloseAttackRange / 2f;
					}
					float3 startPoint = vector;
					float closeAttackRange = item4.ValueRO.CloseAttackRange;
					ComponentLookup<UnitProperty_Dots> cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
					PhysicsWorldSingleton pws = __query_295452819_5.GetSingleton<PhysicsWorldSingleton>();
					DTool.GetEnemyEntityInRange(in startPoint, closeAttackRange, UnitType.Player, containsBrittleness: true, in cluUnitPpt, in pws, ref result);
					TakeDamageInfo_Dots.NewInfo(spell.Entity, CostPenetrate: false, in spell.Config.ValueRW, in spell.Movement.ValueRW, in spell.Transform.ValueRO, in spell.ElementEffect.ValueRO, in spell.Data.ValueRW, out var info);
					info.spell.Config.AbilityType = SpellAbilityType.Summon6;
					info.damage = item4.ValueRO.MeleeAttackDamage;
					foreach (Entity item12 in result)
					{
						Entity target = item12;
						cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
						ComponentLookup<SpellConfigComponentData> spellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef);
						cmd.TryAttackEntity(0, in target, in info, in cluUnitPpt, in spellConfigLookup);
					}
					spell.Config.ValueRO.ColorType.ColorEnumToString(out var result2);
					GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"2006_MeleeAttack_{result2}", vector)
					{
						Size = item4.ValueRO.CloseAttackRange
					};
					singletonBuffer.Add(elem);
					FixedString32Bytes seName = "SE_Teammate6_MeleeAttack";
					__query_295452819_6.GetSingletonBuffer<SEData>().Add(new SEData(seName, SEPlayMode.Replay, 3, 0.2f));
				}
				else if (item4.ValueRW.CloseAttackTimer >= 1f)
				{
					item4.ValueRW.IsCloseAttacking = false;
					item4.ValueRW.CloseAttackTimer = 0f;
					item4.ValueRW.SpawnCloseAttackShockWave = false;
					FindNearestEnemyTarget(transform.ValueRO.Position, movement);
					teammate6SyncScript3.Anima.SetTrigger("Move");
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "Walk", isLoop: true);
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "Walk", isLoop: true);
				}
				if (item4.ValueRW.IsCloseAttacking)
				{
					break;
				}
				if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget) || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget) || !InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget).CanBeTarget || InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget).IsInvincible || item4.ValueRW.TargetTeammate != Entity.Null)
				{
					EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
					break;
				}
				item4.ValueRW.RecheckTargetTeammateTimer += deltaTime;
				if (item4.ValueRO.RecheckTargetTeammateTimer >= 0.2f)
				{
					item4.ValueRW.RecheckTargetTeammateTimer = 0f;
					FindNearestEnemyTarget(transform.ValueRO.Position, movement);
				}
				UnitProperty_Dots componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget);
				LocalTransform componentAfterCompletingDependency3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget);
				float num8 = 0.3f * transform.ValueRO.Scale + item4.ValueRO.CloseAttackRange / 2f + componentAfterCompletingDependency2.size / 2f;
				bool num9 = Tool2D.IgnoreZDistanceSqr(transform.ValueRO.Position, componentAfterCompletingDependency3.Position) <= num8 * num8;
				flag = componentAfterCompletingDependency3.Position.x >= transform.ValueRO.Position.x;
				if (!num9)
				{
					UpdatePathAndVelocity(item8, transform.ValueRW.Position, componentAfterCompletingDependency3.Position, item6, movement.ValueRW.Speed * 0.8f, item5.ValueRW.navAreaMask);
				}
				if (num9)
				{
					item6.ValueRW.SetMove(float3.zero);
					teammate6SyncScript3.Anima.SetTrigger(CloseAttack);
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnima, "CloseRangeAttacking", isLoop: true);
					teammate6SyncScript3.ChangeSpineAnimationState(teammate6SyncScript3.SAnimaHand, "CloseRangeAttacking", isLoop: true);
					item4.ValueRW.IsCloseAttacking = true;
				}
				break;
			}
			case Teammate6State.QuickReload:
			{
				if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, movement.ValueRO.ChaseTarget))
				{
					EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
					break;
				}
				int num7 = 0;
				if (!item4.ValueRO.IsQuickReloading)
				{
					for (int i = 0; i < teammate6SyncScript3.teammateBombList.Count; i++)
					{
						Teammate6Sync.HoldingTeammateData holdingTeammateData = teammate6SyncScript3.teammateBombList[i];
						if (holdingTeammateData.state == Teammate6BombState.Holding_BackUpAmmo)
						{
							holdingTeammateData.targetCannonScript.CannonLoadBackUpAmmo();
							holdingTeammateData.state = Teammate6BombState.Holding_Barrel;
							num7++;
						}
						if (num7 > item7.ValueRO.TeammateCurrentFuseLevel)
						{
							break;
						}
					}
					if (num7 > 0)
					{
						teammate6SyncScript3.Anima.SetTrigger(FastReload);
						item4.ValueRW.IsQuickReloading = true;
					}
					else
					{
						EnterState(item4, Teammate6State.Idle, item8, teammate6SyncScript3, movement, item6);
					}
				}
				if (item4.ValueRO.IsQuickReloading)
				{
					if (item4.ValueRW.QuickReloadTimer >= 0.166f)
					{
						EnterState(item4, Teammate6State.ReadyToShootFindingTarget, item8, teammate6SyncScript3, movement, item6);
					}
					item4.ValueRW.QuickReloadTimer += deltaTime;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			case Teammate6State.LoadingMagazine:
				break;
			}
			teammate6SyncScript3.UpdateSummonFaceDirection(flag);
			item4.ValueRW.IsFaceRight = flag;
		}
		foreach (SyncData syncDatum in this.syncData)
		{
			if (base.EntityManager.HasComponent<LocalTransform>(syncDatum.Entity))
			{
				syncDatum.TargetObject.transform.position = base.EntityManager.GetComponentData<LocalTransform>(syncDatum.Entity).Position;
			}
		}
	}

	private void CheckBombExplosionState(Teammate6Sync teammmateScript, SpellAspect spell, EntityCommandBuffer CMD, EntityCommandBuffer.ParallelWriter ParallelCMD, bool killAllBomb = false)
	{
		DynamicBuffer<GlobalParticleEmitParams> singletonBuffer = __query_295452819_4.GetSingletonBuffer<GlobalParticleEmitParams>();
		for (int num = teammmateScript.teammateBombList.Count - 1; num >= 0; num--)
		{
			Teammate6Sync.HoldingTeammateData holdingTeammateData = teammmateScript.teammateBombList[num];
			NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
			float3 startPoint = holdingTeammateData.BombPosition;
			float checkRadius = holdingTeammateData.SoulBombRange * 0.6f;
			ComponentLookup<UnitProperty_Dots> cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
			PhysicsWorldSingleton pws = __query_295452819_5.GetSingleton<PhysicsWorldSingleton>();
			DTool.GetEnemyEntityInRange(in startPoint, checkRadius, UnitType.Player, containsBrittleness: false, in cluUnitPpt, in pws, ref result);
			if ((holdingTeammateData.state == Teammate6BombState.Shooting && (holdingTeammateData.duration <= 0f || result.Length > 0)) || killAllBomb || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, holdingTeammateData.BombTeammateEntity) || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RO_ComponentLookup, ref base.CheckedStateRef, holdingTeammateData.BombTeammateEntity))
			{
				result.Clear();
				float soulBombRange = holdingTeammateData.SoulBombRange;
				cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
				pws = __query_295452819_5.GetSingleton<PhysicsWorldSingleton>();
				DTool.GetEnemyEntityInRange(in startPoint, soulBombRange, UnitType.Player, containsBrittleness: true, in cluUnitPpt, in pws, ref result);
				TakeDamageInfo_Dots.NewInfo(spell.Entity, CostPenetrate: false, in spell.Config.ValueRW, in spell.Movement.ValueRW, in spell.Transform.ValueRO, in spell.ElementEffect.ValueRO, in spell.Data.ValueRW, out var info);
				info.spell.Config.AbilityType = SpellAbilityType.Summon6;
				info.damage = holdingTeammateData.SoulBombDamage;
				foreach (Entity item in result)
				{
					Entity target = item;
					cluUnitPpt = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
					ComponentLookup<SpellConfigComponentData> spellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef);
					ParallelCMD.TryAttackEntity(0, in target, in info, in cluUnitPpt, in spellConfigLookup);
				}
				spell.Config.ValueRO.ColorType.ColorEnumToString(out var result2);
				GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"2006_SoulBomb_{result2}", startPoint)
				{
					Size = holdingTeammateData.SoulBombRange
				};
				singletonBuffer.Add(elem);
				FixedString32Bytes seName = "SE_Teammate6_CannonShoot";
				__query_295452819_6.GetSingletonBuffer<SEData>().Add(new SEData(seName, SEPlayMode.Replay, 3, 0.2f));
				KillTargetBomb(holdingTeammateData, CMD);
				teammmateScript.teammateBombList.RemoveAt(num);
			}
		}
	}

	private void HideTargetTeammateBomb(Entity targetEntity, Teammate6Sync.HoldingTeammateData data)
	{
		RefRW<TeammateData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.InvincibleRegister();
		if (componentRWAfterCompletingDependency.ValueRW.TeammateType != TeammateType.teammate4)
		{
			SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW);
		}
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.CanBeTarget = false;
		switch (componentRWAfterCompletingDependency.ValueRW.TeammateType)
		{
		case TeammateType.teammate1:
		{
			Entity spellEffectEntity2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref base.CheckedStateRef, targetEntity).SpellEffectEntity;
			data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity2).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity2).ValueRW.Scale = 0.001f;
			Entity ett_Shadow4 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			data.BombShadowScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow4).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow4).ValueRW.Scale = 0.001f;
			break;
		}
		case TeammateType.teammate2:
		{
			data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.Scale = 0.001f;
			Entity ett_Shadow2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			data.BombShadowScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow2).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow2).ValueRW.Scale = 0.001f;
			break;
		}
		case TeammateType.teammate3:
		{
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.DurationTimer = 0.001f;
			Entity ett_Shadow5 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			data.BombShadowScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow5).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow5).ValueRW.Scale = 0.001f;
			{
				foreach (Spell2003TentacleEffectData item in base.EntityManager.GetBuffer<Spell2003TentacleEffectData>(targetEntity))
				{
					data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item.EffectEntity).ValueRW.Scale;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item.EffectEntity).ValueRW.Scale = 0.001f;
				}
				break;
			}
		}
		case TeammateType.teammate4:
		{
			DynamicBuffer<Spell2004PillarBuffer> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2004PillarBuffer_RW_BufferLookup, ref base.CheckedStateRef, targetEntity);
			DynamicBuffer<Spell2004WallBuffer> bufferAfterCompletingDependency2 = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2004WallBuffer_RW_BufferLookup, ref base.CheckedStateRef, targetEntity);
			foreach (Spell2004PillarBuffer item2 in bufferAfterCompletingDependency)
			{
				data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item2.Entity).ValueRW.Scale;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item2.Entity).ValueRW.Scale = 0.001f;
			}
			{
				foreach (Spell2004WallBuffer item3 in bufferAfterCompletingDependency2)
				{
					data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item3.Entity).ValueRW.Scale;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item3.Entity).ValueRW.Scale = 0.001f;
				}
				break;
			}
		}
		case TeammateType.teammate5:
		{
			Entity ett_Shadow3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			data.BombShadowScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow3).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow3).ValueRW.Scale = 0.001f;
			Entity spellEffectEntity = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref base.CheckedStateRef, targetEntity).SpellEffectEntity;
			data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity).ValueRW.Scale = 0.001f;
			break;
		}
		case TeammateType.teammate6:
		{
			int num = syncData.FindIndex((SyncData x) => x.Entity == targetEntity);
			if (num >= 0)
			{
				Teammate6Sync teammate6SyncScript = syncData[num].Teammate6SyncScript;
				data.BombScale = teammate6SyncScript.transform.localScale.x;
				teammate6SyncScript.transform.localScale = Vector3.one * 0.001f;
			}
			break;
		}
		case TeammateType.teammate7:
		{
			Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			data.BombShadowScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow).ValueRW.Scale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow).ValueRW.Scale = 0.001f;
			{
				foreach (Spell2007FuseBuffer item4 in InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2007FuseBuffer_RW_BufferLookup, ref base.CheckedStateRef, targetEntity))
				{
					data.BombScale = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item4.Entity).ValueRW.Scale;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item4.Entity).ValueRW.Scale = 0.001f;
				}
				break;
			}
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void ShowTargetTeammateBomb(Entity targetEntity, Teammate6Sync.HoldingTeammateData data)
	{
		if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, targetEntity))
		{
			return;
		}
		RefRW<TeammateData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.InvincibleUnregister();
		switch (componentRWAfterCompletingDependency.ValueRW.TeammateType)
		{
		case TeammateType.teammate1:
		{
			Entity spellEffectEntity2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref base.CheckedStateRef, targetEntity).SpellEffectEntity;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity2).ValueRW.Scale = data.BombScale;
			Entity ett_Shadow4 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow4).ValueRW.Scale = data.BombShadowScale;
			break;
		}
		case TeammateType.teammate2:
		{
			Entity ett_Shadow2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow2).ValueRW.Scale = data.BombShadowScale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.Scale = data.BombScale;
			break;
		}
		case TeammateType.teammate3:
		{
			Entity ett_Shadow5 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow5).ValueRW.Scale = data.BombShadowScale;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.DurationTimer = data.BombScale;
			using NativeArray<Spell2003TentacleEffectData>.Enumerator enumerator4 = base.EntityManager.GetBuffer<Spell2003TentacleEffectData>(targetEntity).GetEnumerator();
			while (enumerator4.MoveNext())
			{
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(entity: enumerator4.Current.EffectEntity, componentLookup: ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, state: ref base.CheckedStateRef).ValueRW.Scale = data.BombScale;
			}
			break;
		}
		case TeammateType.teammate4:
		{
			DynamicBuffer<Spell2004PillarBuffer> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2004PillarBuffer_RW_BufferLookup, ref base.CheckedStateRef, targetEntity);
			DynamicBuffer<Spell2004WallBuffer> bufferAfterCompletingDependency2 = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2004WallBuffer_RW_BufferLookup, ref base.CheckedStateRef, targetEntity);
			using (NativeArray<Spell2004PillarBuffer>.Enumerator enumerator2 = bufferAfterCompletingDependency.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(entity: enumerator2.Current.Entity, componentLookup: ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, state: ref base.CheckedStateRef).ValueRW.Scale = data.BombScale;
				}
			}
			using NativeArray<Spell2004WallBuffer>.Enumerator enumerator3 = bufferAfterCompletingDependency2.GetEnumerator();
			while (enumerator3.MoveNext())
			{
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(entity: enumerator3.Current.Entity, componentLookup: ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, state: ref base.CheckedStateRef).ValueRW.Scale = data.BombScale;
			}
			break;
		}
		case TeammateType.teammate5:
		{
			Entity ett_Shadow3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow3).ValueRW.Scale = data.BombShadowScale;
			Entity spellEffectEntity = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref base.CheckedStateRef, targetEntity).SpellEffectEntity;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity).ValueRW.Scale = data.BombScale;
			break;
		}
		case TeammateType.teammate6:
		{
			int num = syncData.FindIndex((SyncData x) => x.Entity == targetEntity);
			if (num >= 0)
			{
				syncData[num].Teammate6SyncScript.transform.localScale = data.BombScale * Vector3.one;
			}
			break;
		}
		case TeammateType.teammate7:
		{
			Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref base.CheckedStateRef, targetEntity).ValueRW.ett_Shadow;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, ett_Shadow).ValueRW.Scale = data.BombShadowScale;
			using NativeArray<Spell2007FuseBuffer>.Enumerator enumerator = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2007FuseBuffer_RW_BufferLookup, ref base.CheckedStateRef, targetEntity).GetEnumerator();
			while (enumerator.MoveNext())
			{
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(entity: enumerator.Current.Entity, componentLookup: ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, state: ref base.CheckedStateRef).ValueRW.Scale = data.BombScale;
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void KillTargetBomb(Teammate6Sync.HoldingTeammateData bomb, EntityCommandBuffer CMD)
	{
		PlayerMgr.Inst.MiniPool.RecycleGO(bomb.BombOutlookObject);
		TakeDamageInfo_Dots element = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
		element.damage = 100000000f;
		element.ignoreFloatText = true;
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, bomb.BombTeammateEntity))
		{
			ShowTargetTeammateBomb(bomb.BombTeammateEntity, bomb);
			RefRW<TeammateData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, bomb.BombTeammateEntity);
			componentRWAfterCompletingDependency.ValueRW.IsHoldByTeammate6 = false;
			componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, bomb.BombTeammateEntity);
			componentRWAfterCompletingDependency.ValueRW.SummonFollowOwnerThroughMapChance -= 1f;
			CMD.AppendToBuffer(bomb.BombTeammateEntity, element);
		}
	}

	private void FindNearestEnemyTarget(float3 startPosition, RefRW<SpellMovementComponentData> movement)
	{
		__query_295452819_7.GetSingleton<CurrentRoomEntitiesSingleton>().FindNearestTarget(startPosition, UnitType.Teammate, out movement.ValueRW.ChaseTarget, out var _, out var _);
	}

	private bool IsEnemyTargetValid(Entity target)
	{
		if (target == Entity.Null || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, target) || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, target))
		{
			return false;
		}
		UnitProperty_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, target);
		if (componentAfterCompletingDependency.CanBeTarget && !componentAfterCompletingDependency.IsInvincible && !componentAfterCompletingDependency.isDead)
		{
			return componentAfterCompletingDependency.unitCfg.currentHP > 0f;
		}
		return false;
	}

	private void ResetShootState(RefRW<Spell2006Data> data, Teammate6Sync teammmateScript)
	{
		data.ValueRW.IsBombShoot = false;
		data.ValueRW.IsStartShoot = false;
		data.ValueRW.ShootingTimer = 0f;
		teammmateScript.barrelLockingTarget = false;
	}

	private void UpdatePathAndVelocity(RefRW<PathFinding> pathFinding, float3 startPos, float3 targetPos, RefRW<UnitBase_Dots> unitBase, float speed, int navMask)
	{
		pathFinding.ValueRW.UpdatePath(startPos, targetPos, navMask);
		unitBase.ValueRW.SetMove(Tool2D.IgnoreZPoint(pathFinding.ValueRO.walkToPoint - startPos).normalized * speed);
	}

	private void FindNearestSacrificebleTeammateEntity(RefRW<Spell2006Data> data, Entity myEntity, float3 myPos, RefRW<SpellConfigComponentData> myConfig)
	{
		data.ValueRW.TargetTeammate = Entity.Null;
		float num = 999f;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateData>> item2 in IFE_295452819_2.Query(__query_295452819_2, __TypeHandle.__IFE_295452819_2_TypeHandle, ref base.CheckedStateRef))
		{
			item2.Deconstruct(out var item, out var entity);
			InternalCompilerInterface.UncheckedRefRW<TeammateData> uncheckedRefRW = item;
			Entity entity2 = entity;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, entity2) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef, entity2);
				LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, entity2);
				float num2 = DTool.IgnoreZDistance(in componentAfterCompletingDependency.Position, in myPos);
				bool flag = false;
				if (uncheckedRefRW.ValueRO.TeammateType == TeammateType.teammate6)
				{
					int num3 = (int)componentRWAfterCompletingDependency.ValueRO.Damage.CalculateWithNewBaseValue(100f);
					int num4 = (int)myConfig.ValueRO.Damage.CalculateWithNewBaseValue(100f);
					flag = ((num3 == num4) ? (InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RW_ComponentLookup, ref base.CheckedStateRef, entity2).ValueRO.UID > data.ValueRO.UID) : (num3 < num4));
				}
				if (num2 < num && CheckTargetTeammateIsCatchableRW(uncheckedRefRW, myEntity, entity2) && (uncheckedRefRW.ValueRO.TeammateType != TeammateType.teammate6 || flag))
				{
					data.ValueRW.TargetTeammate = entity2;
					num = num2;
				}
			}
		}
	}

	private void UpdateBodyTransparency(Teammate6Sync bodySync)
	{
		bodySync.SAnima.CustomMaterialOverride[bodySync.bodyBaseMaterial].SetFloat(Transparency, DataMgr.settingData.SummonTransparent);
	}

	private bool CheckTargetTeammateIsCatchable(TeammateData data, Entity shooterEntity, Entity targetEntity)
	{
		if (shooterEntity == targetEntity || data.IsHoldByTeammate6 || data.IsFuseMaterial || data.TeammateDelayDeathEffectActive)
		{
			return false;
		}
		return true;
	}

	private bool CheckTargetTeammateIsCatchableRW(RefRW<TeammateData> data, Entity shooterEntity, Entity targetEntity)
	{
		if (shooterEntity == targetEntity || data.ValueRW.IsHoldByTeammate6 || data.ValueRW.IsFuseMaterial || data.ValueRW.TeammateDelayDeathEffectActive)
		{
			return false;
		}
		return true;
	}

	private bool IsTargetTeammate6Stronger(TeammateData mydata, SpellConfigComponentData myConfig, SpellConfigComponentData targetConfig, Spell2006Data my2006Data, Spell2006Data target2006Data)
	{
		if (mydata.TeammateType != TeammateType.teammate6)
		{
			return false;
		}
		int num = (int)targetConfig.Damage.CalculateWithNewBaseValue(100f);
		int num2 = (int)myConfig.Damage.CalculateWithNewBaseValue(100f);
		if (num == num2)
		{
			return target2006Data.UID > my2006Data.UID;
		}
		return num < num2;
	}

	private float GetRandomIdleInterval()
	{
		return UnityEngine.Random.Range(1f, 2f);
	}

	private void EnterState(RefRW<Spell2006Data> data, Teammate6State newState, RefRW<PathFinding> pathFinding, Teammate6Sync teammmateScript, RefRW<SpellMovementComponentData> movement, RefRW<UnitBase_Dots> unitbase)
	{
		data.ValueRW.CurrentState = newState;
		switch (newState)
		{
		case Teammate6State.Idle:
			movement.ValueRW.ChaseTarget = Entity.Null;
			teammmateScript.barrelLockingTarget = false;
			teammmateScript.Anima.SetTrigger("Idle");
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnima, "Idle", isLoop: true);
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnimaHand, "Idle", isLoop: true);
			break;
		case Teammate6State.SeekingAmmo:
			teammmateScript.Anima.SetTrigger("Move");
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnima, "Walk", isLoop: true);
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnimaHand, "Walk", isLoop: true);
			data.ValueRW.IsPickingTeammateP1 = false;
			data.ValueRW.IsPickingTeammateP2 = false;
			data.ValueRW.PickingTeammateTimer = 0f;
			data.ValueRW.IsStartThrowingHook = false;
			data.ValueRW.IsHookOut = false;
			data.ValueRW.IsHookCatchTarget = false;
			data.ValueRW.ThrowHookTimer = 0f;
			break;
		case Teammate6State.ReadyToShootFindingTarget:
			unitbase.ValueRW.SetMove(Vector3.zero);
			teammmateScript.Anima.SetTrigger("Idle");
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnima, "Idle", isLoop: true);
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnimaHand, "Idle", isLoop: true);
			break;
		case Teammate6State.CloseAttack:
			data.ValueRW.IdleInterval = GetRandomIdleInterval();
			teammmateScript.Anima.SetTrigger("Move");
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnima, "Walk", isLoop: true);
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnimaHand, "Walk", isLoop: true);
			break;
		case Teammate6State.QuickReload:
			data.ValueRW.QuickReloadTimer = 0f;
			data.ValueRW.IsQuickReloading = false;
			teammmateScript.Anima.SetTrigger("Idle");
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnima, "Idle", isLoop: true);
			teammmateScript.ChangeSpineAnimationState(teammmateScript.SAnimaHand, "Idle", isLoop: true);
			break;
		default:
			throw new ArgumentOutOfRangeException("newState", newState, null);
		case Teammate6State.Move:
		case Teammate6State.LoadingMagazine:
			break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell2006Data>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
		__query_295452819_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell2006Data>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
		__query_295452819_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateData>();
		__query_295452819_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_295452819_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_295452819_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_295452819_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_295452819_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_295452819_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public Spell2006System()
	{
	}
}
