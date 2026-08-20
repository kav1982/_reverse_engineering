using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine;
using UnityEngine.AI;

[BurstCompile]
[CompilerGenerated]
public struct PathFindingSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2087897727_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public InternalCompilerInterface.UncheckedRefRW<PathFinding> Get(int index)
			{
				return InternalCompilerInterface.UnsafeGetUncheckedRefRW<PathFinding>(item1_IntPtr, index);
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<PathFinding> item1_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PathFinding>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<InternalCompilerInterface.UncheckedRefRW<PathFinding>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public InternalCompilerInterface.UncheckedRefRW<PathFinding> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<PathFinding>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2087897727_0.TypeHandle __IFE_2087897727_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2087897727_0_TypeHandle = new IFE_2087897727_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2087897727_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PathFinding>();
	}

	public void OnUpdate(ref SystemState state)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		foreach (InternalCompilerInterface.UncheckedRefRW<PathFinding> item in IFE_2087897727_0.Query(__query_2087897727_0, __TypeHandle.__IFE_2087897727_0_TypeHandle, ref state))
		{
			ref PathFinding valueRW = ref item.ValueRW;
			valueRW.updateIntervalTimer += state.WorldUnmanaged.Time.DeltaTime;
			if (valueRW.moveThreshold * valueRW.moveThreshold > Tool2D.IgnoreZDistanceSqr(valueRW.startPosition, valueRW.walkToPoint) && valueRW.moveThreshold * valueRW.moveThreshold > Tool2D.IgnoreZDistanceSqr(valueRW.trueEndPosition, valueRW.walkToPoint))
			{
				valueRW.allCornerArrived = true;
			}
			else
			{
				valueRW.allCornerArrived = false;
			}
			if (!DTool.IsTotallySame(in valueRW.endPosition, in valueRW.lastEndPosition) && valueRW.updateIntervalTimer >= 0.1f)
			{
				valueRW.lastEndPosition = valueRW.endPosition;
				valueRW.allCornerArrived = false;
			}
			if (!valueRW.allCornerArrived)
			{
				if (valueRW.updateIntervalTimer >= 0.1f)
				{
					if (valueRW.needUpdatePath)
					{
						valueRW.needUpdatePath = false;
						valueRW.updateIntervalTimer = 0f;
						Vector3 navMeshPoint = Tool2D.GetNavMeshPoint(valueRW.startPosition, valueRW.navArea);
						Vector3 navMeshPoint2 = Tool2D.GetNavMeshPoint(valueRW.endPosition, valueRW.navArea);
						valueRW.trueEndPosition = navMeshPoint2;
						if (NavMesh.CalculatePath(navMeshPoint, navMeshPoint2, valueRW.navArea, navMeshPath))
						{
							if (navMeshPath.corners.Length >= 2)
							{
								int num = Mathf.Min(navMeshPath.corners.Length - 2, 2);
								for (int i = 0; i <= num; i++)
								{
									valueRW.SetPathPoints(navMeshPath.corners[i + 1], num - i);
								}
								valueRW.recordPointIndex = num;
								valueRW.walkToPoint = valueRW.GetRecordPoints(valueRW.recordPointIndex);
							}
							else
							{
								valueRW.recordPointIndex = 0;
								valueRW.walkToPoint = navMeshPoint2;
							}
						}
					}
				}
				else if (valueRW.moveThreshold * valueRW.moveThreshold > Tool2D.IgnoreZDistanceSqr(valueRW.startPosition, valueRW.walkToPoint))
				{
					if (valueRW.recordPointIndex == 0)
					{
						if (valueRW.moveThreshold * valueRW.moveThreshold < Tool2D.IgnoreZDistanceSqr(valueRW.trueEndPosition, valueRW.walkToPoint))
						{
							valueRW.updateIntervalTimer = 999f;
						}
					}
					else
					{
						valueRW.recordPointIndex--;
						valueRW.walkToPoint = valueRW.GetRecordPoints(valueRW.recordPointIndex);
					}
				}
			}
			if (valueRW.samplePointRequest.requestState == NavMeshRequestState.Solving)
			{
				ref NavMeshPointRequest samplePointRequest = ref valueRW.samplePointRequest;
				samplePointRequest.requestState = NavMeshRequestState.Completed;
				switch (valueRW.samplePointRequest.requsetType)
				{
				case 0:
					samplePointRequest.result = Tool2D.GetNavMeshPointIngoreZ(samplePointRequest.startPoint, samplePointRequest.navAreaMask);
					break;
				case 1:
					samplePointRequest.result = Tool2D.GetNavMeshPointIngoreZ(samplePointRequest.startPoint, samplePointRequest.radius, samplePointRequest.navAreaMask);
					break;
				case 2:
					samplePointRequest.result = Tool2D.GetNavMeshPointIngoreZ(samplePointRequest.startPoint, samplePointRequest.radiusRange, samplePointRequest.navAreaMask);
					break;
				case 3:
					samplePointRequest.result = Tool2D.GetNavMeshPointIngoreZ(samplePointRequest.startPoint, samplePointRequest.radius, samplePointRequest.from, samplePointRequest.angle, samplePointRequest.navAreaMask);
					break;
				case 4:
					samplePointRequest.result = Tool2D.GetNavMeshPointIngoreZ(samplePointRequest.startPoint, samplePointRequest.radiusRange, samplePointRequest.from, samplePointRequest.angle, samplePointRequest.navAreaMask);
					break;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		__query_2087897727_0 = entityQueryBuilder.WithAllRW<PathFinding>().Build(ref state);
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
		((PathFindingSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((PathFindingSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((PathFindingSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
