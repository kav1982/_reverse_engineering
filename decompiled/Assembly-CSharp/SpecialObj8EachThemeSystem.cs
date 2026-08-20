using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(SceneGroup))]
public struct SpecialObj8EachThemeSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_382765618_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj8EachTheme>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj8EachTheme>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj8EachTheme>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj8EachTheme> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj8EachTheme>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj8EachTheme>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj8EachTheme>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj8EachTheme>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
		}
	}

	private struct TypeHandle
	{
		public IFE_382765618_0.TypeHandle __IFE_382765618_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_382765618_0_TypeHandle = new IFE_382765618_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006036_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006036_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006036_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00006037_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006037_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006037_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_382765618_0;

	private EntityQuery __query_382765618_1;

	private EntityQuery __query_382765618_2;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpecialObj8EachTheme>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		PhysicsWorldSingleton singleton = __query_382765618_1.GetSingleton<PhysicsWorldSingleton>();
		EntityCommandBuffer entityCommandBuffer = __query_382765618_2.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj8EachTheme>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> item4 in IFE_382765618_0.Query(__query_382765618_0, __TypeHandle.__IFE_382765618_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj8EachTheme> uncheckedRefRW = item;
			LocalTransform localTransform = item2;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW2 = item3;
			Entity e = entity;
			if (uncheckedRefRW.ValueRW.waitTimeForInitial < 0.1f)
			{
				uncheckedRefRW.ValueRW.waitTimeForInitial += state.WorldUnmanaged.Time.DeltaTime;
				continue;
			}
			if (uncheckedRefRW.ValueRW.waitFrameForInitial < 5)
			{
				uncheckedRefRW.ValueRW.waitFrameForInitial++;
				continue;
			}
			RefRW<LocalTransform> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Layer);
			componentRWAfterCompletingDependency.ValueRW.Position = DTool.GetLayerPosition(in localTransform.Position, LayerCorrectType.SO8_Abyss);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			CollisionFilter collisionFilter = default(CollisionFilter);
			collisionFilter.BelongsTo = uint.MaxValue;
			collisionFilter.CollidesWith = 1024u;
			collisionFilter.GroupIndex = 0;
			CollisionFilter filter = collisionFilter;
			NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
			if (singleton.OverlapSphere(localTransform.Position, 1.42f, ref outHits, filter))
			{
				using NativeArray<DistanceHit>.Enumerator enumerator2 = outHits.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					float3 f = InternalCompilerInterface.GetComponentAfterCompletingDependency(entity: enumerator2.Current.Entity, componentLookup: ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, state: ref state).Position;
					float3 f2 = localTransform.Position + new float3(0f, 1f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag = true;
						continue;
					}
					f2 = localTransform.Position + new float3(1f, 1f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag2 = true;
						continue;
					}
					f2 = localTransform.Position + new float3(1f, 0f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag3 = true;
						continue;
					}
					f2 = localTransform.Position + new float3(1f, -1f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag4 = true;
						continue;
					}
					f2 = localTransform.Position + new float3(0f, -1f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag5 = true;
						continue;
					}
					f2 = localTransform.Position + new float3(-1f, -1f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag6 = true;
						continue;
					}
					f2 = localTransform.Position + new float3(-1f, 0f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag7 = true;
						continue;
					}
					f2 = localTransform.Position + new float3(-1f, 1f, 0f);
					if (DTool.IsTotallySame(in f, in f2))
					{
						flag8 = true;
					}
				}
			}
			outHits.Dispose();
			if (flag && flag3 && !flag5 && !flag7)
			{
				uncheckedRefRW.ValueRW.keepUR = true;
			}
			else if (!flag && flag3 && flag5 && !flag7)
			{
				uncheckedRefRW.ValueRW.keepRD = true;
			}
			else if (!flag && !flag3 && flag5 && flag7)
			{
				uncheckedRefRW.ValueRW.keepRD = true;
				uncheckedRefRW.ValueRW.isFliped = true;
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
			else if (flag && !flag3 && !flag5 && flag7)
			{
				uncheckedRefRW.ValueRW.keepUR = true;
				uncheckedRefRW.ValueRW.isFliped = true;
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
			else if (flag && flag3 && !flag5 && flag7)
			{
				uncheckedRefRW.ValueRW.keepLUR = true;
			}
			else if (flag && flag3 && flag5 && !flag7)
			{
				uncheckedRefRW.ValueRW.keepURD = true;
			}
			else if (!flag && flag3 && flag5 && flag7)
			{
				uncheckedRefRW.ValueRW.keepRDL = true;
			}
			else if (flag && !flag3 && flag5 && flag7)
			{
				uncheckedRefRW.ValueRW.keepURD = true;
				uncheckedRefRW.ValueRW.isFliped = true;
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
			else if (flag && flag3 && flag5 && flag7)
			{
				bool flag9 = true;
				if (!flag4 && !flag8)
				{
					flag9 = false;
					uncheckedRefRW.ValueRW.keepCornerURLD = true;
					componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.002f);
					entityCommandBuffer.AddComponent(uncheckedRefRW.ValueRW.ett_CornerURLD, new PostTransformMatrix
					{
						Value = float4x4.Scale(-1f, 1f, 1f)
					});
				}
				else if (!flag2 && !flag6)
				{
					flag9 = false;
					uncheckedRefRW.ValueRW.keepCornerURLD = true;
					uncheckedRefRW.ValueRW.isFliped = true;
					componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.002f);
				}
				else
				{
					if (!flag2)
					{
						flag9 = false;
						uncheckedRefRW.ValueRW.keepCornerUR = true;
						componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.001f);
					}
					if (!flag4)
					{
						flag9 = false;
						uncheckedRefRW.ValueRW.keepCornerRD = true;
						componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.001f);
					}
					if (!flag6)
					{
						flag9 = false;
						uncheckedRefRW.ValueRW.keepCornerRD = true;
						uncheckedRefRW.ValueRW.isFliped = true;
						componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.001f);
						entityCommandBuffer.AddComponent(e, new PostTransformMatrix
						{
							Value = float4x4.Scale(-1f, 1f, 1f)
						});
					}
					if (!flag8)
					{
						flag9 = false;
						uncheckedRefRW.ValueRW.keepCornerUR = true;
						uncheckedRefRW.ValueRW.isFliped = true;
						componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.001f);
						entityCommandBuffer.AddComponent(e, new PostTransformMatrix
						{
							Value = float4x4.Scale(-1f, 1f, 1f)
						});
					}
				}
				if (flag9)
				{
					uncheckedRefRW.ValueRW.keepFull = true;
				}
			}
			if (!uncheckedRefRW.ValueRW.keepCornerURLD)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_CornerURLD);
			}
			if (!uncheckedRefRW.ValueRW.keepCornerUR)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_CornerUR);
			}
			if (!uncheckedRefRW.ValueRW.keepCornerRD)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_CornerRD);
			}
			if (!uncheckedRefRW.ValueRW.keepUR)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_UR);
			}
			if (!uncheckedRefRW.ValueRW.keepRD)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_RD);
			}
			if (!uncheckedRefRW.ValueRW.keepLUR)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_LUR);
			}
			if (!uncheckedRefRW.ValueRW.keepURD)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_URD);
			}
			if (!uncheckedRefRW.ValueRW.keepRDL)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_RDL);
			}
			if (!uncheckedRefRW.ValueRW.keepFull)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRW.ValueRW.ett_Full);
			}
			if (uncheckedRefRW.ValueRW.waitFrameForChangeCollider < 3)
			{
				uncheckedRefRW.ValueRW.waitFrameForChangeCollider++;
				continue;
			}
			BoxGeometry geometry = default(BoxGeometry);
			geometry.Orientation = quaternion.identity;
			geometry.BevelRadius = 0f;
			RefRW<LocalTransform> componentRWAfterCompletingDependency2;
			if (uncheckedRefRW.ValueRW.keepCornerURLD)
			{
				if (!uncheckedRefRW.ValueRW.isFliped)
				{
					Entity entity3 = state.EntityManager.Instantiate(uncheckedRefRW.ValueRW.pfb_801BoxCollider);
					componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3);
					componentRWAfterCompletingDependency2.ValueRW.Position = localTransform.Position + new float3(0.25f, 0.25f, 0f);
					entityCommandBuffer.AddComponent(entity3, default(AbyssTag));
					geometry.Center = new float3(-0.25f, -0.25f, 0f);
					geometry.Size = new float3(0.5f, 0.5f, 2f);
				}
				else
				{
					Entity entity4 = state.EntityManager.Instantiate(uncheckedRefRW.ValueRW.pfb_801BoxCollider);
					componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity4);
					componentRWAfterCompletingDependency2.ValueRW.Position = localTransform.Position + new float3(-0.25f, 0.25f, 0f);
					entityCommandBuffer.AddComponent(entity4, default(AbyssTag));
					geometry.Center = new float3(0.25f, -0.25f, 0f);
					geometry.Size = new float3(0.5f, 0.5f, 2f);
				}
			}
			else if (uncheckedRefRW.ValueRW.keepCornerUR)
			{
				geometry.Center = new float3(uncheckedRefRW.ValueRW.isFliped ? 0.25f : (-0.25f), 0f, 0f);
				geometry.Size = new float3(0.5f, 1f, 2f);
				Entity entity5 = state.EntityManager.Instantiate(uncheckedRefRW.ValueRW.pfb_801BoxCollider);
				componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity5);
				componentRWAfterCompletingDependency2.ValueRW.Position = localTransform.Position + new float3(uncheckedRefRW.ValueRW.isFliped ? (-0.25f) : 0.25f, -0.25f, 0f);
				entityCommandBuffer.AddComponent(entity5, default(AbyssTag));
			}
			else if (uncheckedRefRW.ValueRW.keepCornerRD)
			{
				geometry.Center = new float3(uncheckedRefRW.ValueRW.isFliped ? 0.25f : (-0.25f), 0f, 0f);
				geometry.Size = new float3(0.5f, 1f, 2f);
				Entity entity6 = state.EntityManager.Instantiate(uncheckedRefRW.ValueRW.pfb_801BoxCollider);
				componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity6);
				componentRWAfterCompletingDependency2.ValueRW.Position = localTransform.Position + new float3(uncheckedRefRW.ValueRW.isFliped ? (-0.25f) : 0.25f, 0.25f, 0f);
				entityCommandBuffer.AddComponent(entity6, default(AbyssTag));
			}
			else if (uncheckedRefRW.ValueRW.keepUR)
			{
				geometry.Center = new float3(uncheckedRefRW.ValueRW.isFliped ? (-0.25f) : 0.25f, 0.25f, 0f);
				geometry.Size = new float3(0.5f, 0.5f, 2f);
			}
			else if (uncheckedRefRW.ValueRW.keepRD)
			{
				geometry.Center = new float3(uncheckedRefRW.ValueRW.isFliped ? (-0.25f) : 0.25f, -0.25f, 0f);
				geometry.Size = new float3(0.5f, 0.5f, 2f);
			}
			else if (uncheckedRefRW.ValueRW.keepLUR)
			{
				geometry.Center = new float3(0f, 0.25f, 0f);
				geometry.Size = new float3(1f, 0.5f, 2f);
			}
			else if (uncheckedRefRW.ValueRW.keepURD)
			{
				geometry.Center = new float3(uncheckedRefRW.ValueRW.isFliped ? (-0.25f) : 0.25f, 0f, 0f);
				geometry.Size = new float3(0.5f, 1f, 2f);
			}
			else if (uncheckedRefRW.ValueRW.keepRDL)
			{
				geometry.Center = new float3(0f, -0.25f, 0f);
				geometry.Size = new float3(1f, 0.5f, 2f);
			}
			else if (uncheckedRefRW.ValueRW.keepFull)
			{
				geometry.Center = new float3(0f, 0f, 0f);
				geometry.Size = new float3(1f, 1f, 2f);
			}
			uncheckedRefRW2.ValueRW.Value = BoxCollider.Create(geometry, new CollisionFilter
			{
				BelongsTo = 1024u,
				CollidesWith = 262144u,
				GroupIndex = 0
			});
			entityCommandBuffer.AddComponent(e, default(AbyssTag));
			entityCommandBuffer.SetComponentEnabled<SpecialObj8EachTheme>(e, value: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj8EachTheme>();
		__query_382765618_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_382765618_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_382765618_2 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00006036_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006037_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpecialObj8EachThemeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpecialObj8EachThemeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpecialObj8EachThemeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
