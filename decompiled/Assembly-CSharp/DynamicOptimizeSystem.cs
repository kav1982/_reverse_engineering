using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateBefore(typeof(SpellSystem))]
[CompilerGenerated]
[UpdateInGroup(typeof(SpellPhysicsSystemGroup), OrderFirst = true)]
public class DynamicOptimizeSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_932991965_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpellMovementComponentData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item2_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_932991965_0.TypeHandle __IFE_932991965_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_932991965_0_TypeHandle = new IFE_932991965_0.TypeHandle(ref state);
		}
	}

	public List<float> AvergeFrame = new List<float>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_932991965_0;

	private EntityQuery __query_932991965_1;

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingleton(new DynamicOptimizeData
		{
			CurrentFPS = 60f,
			EnableLowFPSOptimize = false,
			IsMobilePlatform = GameMgr.IsMobile_Static,
			LastFrameTimeScale = 1f
		});
		RequireForUpdate<DynamicOptimizeData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		RefRW<DynamicOptimizeData> singletonRW = __query_932991965_1.GetSingletonRW<DynamicOptimizeData>();
		AvergeFrame.Add(1f / UnityEngine.Time.unscaledDeltaTime);
		if (AvergeFrame.Count >= 10)
		{
			AvergeFrame.RemoveAt(0);
		}
		singletonRW.ValueRW.CurrentFPS = GetAvergeFrame();
		singletonRW.ValueRW.EnableLowFPSOptimize = !ScriptableObjMgr.Inst.testCtrller.DisableLowFrameDynamicOptimize;
		float lowFpsActiveThreshold = SpellTools.GetLowFpsActiveThreshold(GameMgr.IsMobile_Static);
		float lowFpsActiveMinFps = SpellTools.GetLowFpsActiveMinFps(GameMgr.IsMobile_Static);
		if (singletonRW.ValueRO.EnableLowFPSOptimize)
		{
			float x = math.clamp((singletonRW.ValueRO.CurrentFPS - lowFpsActiveMinFps) / (lowFpsActiveThreshold - lowFpsActiveMinFps), 0f, 1f);
			singletonRW.ValueRW.PoolEffectShowRatio = math.pow(x, 1.5f);
		}
		else
		{
			singletonRW.ValueRW.PoolEffectShowRatio = 1f;
		}
		float lowFrameTimeScale = SpellTools.GetLowFrameTimeScale(SpellTools.GetLowFpsMaxBonusTimeScale(GameMgr.IsMobile_Static), singletonRW.ValueRW.CurrentFPS, lowFpsActiveThreshold);
		if (!singletonRW.ValueRW.IsLowFpsOptimizeActive(lowFpsActiveThreshold))
		{
			return;
		}
		foreach (var (uncheckedRefRW, uncheckedRefRW2) in IFE_932991965_0.Query(__query_932991965_0, __TypeHandle.__IFE_932991965_0_TypeHandle, ref base.CheckedStateRef))
		{
			if (DTool.IsSameCamp(uncheckedRefRW2.ValueRW.ShooterType, UnitType.Player))
			{
				uncheckedRefRW.ValueRW.Speed = ((uncheckedRefRW2.ValueRW.DurationTimer <= 0f) ? (uncheckedRefRW.ValueRW.Speed * lowFrameTimeScale) : (uncheckedRefRW.ValueRW.Speed / singletonRW.ValueRW.LastFrameTimeScale * lowFrameTimeScale));
				uncheckedRefRW.ValueRW.Gravity = ((uncheckedRefRW2.ValueRW.DurationTimer <= 0f) ? (uncheckedRefRW.ValueRW.Gravity * lowFrameTimeScale) : (uncheckedRefRW.ValueRW.Gravity / singletonRW.ValueRW.LastFrameTimeScale * lowFrameTimeScale));
				uncheckedRefRW.ValueRW.CurrentFallSpeed = ((uncheckedRefRW2.ValueRW.DurationTimer <= 0f) ? (uncheckedRefRW.ValueRW.CurrentFallSpeed * lowFrameTimeScale) : (uncheckedRefRW.ValueRW.CurrentFallSpeed / singletonRW.ValueRW.LastFrameTimeScale * lowFrameTimeScale));
				if (uncheckedRefRW2.ValueRO.DamageInterval > 0f)
				{
					uncheckedRefRW2.ValueRW.Damage.MulRatio = ((uncheckedRefRW2.ValueRW.DurationTimer <= 0f) ? (uncheckedRefRW2.ValueRW.Damage.MulRatio * lowFrameTimeScale) : (uncheckedRefRW2.ValueRW.Damage.MulRatio / singletonRW.ValueRW.LastFrameTimeScale * lowFrameTimeScale));
				}
				uncheckedRefRW2.ValueRW.DurationTimer += (lowFrameTimeScale - 1f) * base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
			}
		}
		singletonRW.ValueRW.LastFrameTimeScale = lowFrameTimeScale;
	}

	public float GetAvergeFrame()
	{
		if (AvergeFrame.Count < 5)
		{
			return 60f;
		}
		float num = 0f;
		foreach (float item in AvergeFrame)
		{
			num += item;
		}
		return num / (float)AvergeFrame.Count;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithNone<IgnoreDynamicOptimizeTag>();
		__query_932991965_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_932991965_1 = entityQueryBuilder2.Build(ref state);
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
	public DynamicOptimizeSystem()
	{
	}
}
