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
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(SceneGroup))]
public struct Tile_T9_Tile9System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_647615470_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public ManagedComponentAccessor<BoundaryT2RoomCtrller> item4_ManagedComponentAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Tile_T9_Tile9_Dots>, InternalCompilerInterface.UncheckedRefRO<TileBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, BoundaryT2RoomCtrller> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Tile_T9_Tile9_Dots>, InternalCompilerInterface.UncheckedRefRO<TileBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, BoundaryT2RoomCtrller>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Tile_T9_Tile9_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<TileBase_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), item4_ManagedComponentAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			[ReadOnly]
			private ComponentTypeHandle<Tile_T9_Tile9_Dots> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<TileBase_Dots> item2_ComponentTypeHandle_RO;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<BoundaryT2RoomCtrller> item4_ManagedComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Tile_T9_Tile9_Dots>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<TileBase_Dots>(isReadOnly: true);
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<BoundaryT2RoomCtrller>(isReadOnly: false);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ManagedComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item4_ManagedComponentTypeHandle_RO, _entityManager);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Tile_T9_Tile9_Dots>, InternalCompilerInterface.UncheckedRefRO<TileBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, BoundaryT2RoomCtrller>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Tile_T9_Tile9_Dots>, InternalCompilerInterface.UncheckedRefRO<TileBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, BoundaryT2RoomCtrller> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Tile_T9_Tile9_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<TileBase_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<BoundaryT2RoomCtrller>();
		}
	}

	private struct TypeHandle
	{
		public IFE_647615470_0.TypeHandle __IFE_647615470_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_647615470_0_TypeHandle = new IFE_647615470_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005C7F_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005C7F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005C7F_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_647615470_0;

	private EntityQuery __query_647615470_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Tile_T9_Tile9_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_647615470_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Tile_T9_Tile9_Dots>, InternalCompilerInterface.UncheckedRefRO<TileBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, BoundaryT2RoomCtrller> item5 in IFE_647615470_0.Query(__query_647615470_0, __TypeHandle.__IFE_647615470_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var _, out var entity);
			InternalCompilerInterface.UncheckedRefRO<Tile_T9_Tile9_Dots> uncheckedRefRO = item;
			InternalCompilerInterface.UncheckedRefRO<TileBase_Dots> uncheckedRefRO2 = item2;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW = item3;
			Entity e = entity;
			uncheckedRefRW.ValueRW.Position = uncheckedRefRO2.ValueRO.roomPosition + uncheckedRefRO2.ValueRO.selfPosition.GetFloat3();
			RefRW<LocalTransform> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRO.ValueRO.ett_LayerBase);
			componentRWAfterCompletingDependency.ValueRW.Position = DTool.GetLayerPosition(in uncheckedRefRW.ValueRW.Position, LayerCorrectType.BoundaryLow);
			componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRO.ValueRO.ett_LayerWall);
			componentRWAfterCompletingDependency.ValueRW.Position = DTool.GetLayerPosition(in uncheckedRefRW.ValueRW.Position, LayerCorrectType.Coordinate);
			ref readonly TileBase_Dots valueRO = ref uncheckedRefRO2.ValueRO;
			Vector2Data offset = new Vector2Data(0f, 1f);
			bool flag = DTool.TileCheck(in valueRO, in offset);
			ref readonly TileBase_Dots valueRO2 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(1f, 1f);
			bool flag2 = DTool.TileCheck(in valueRO2, in offset);
			ref readonly TileBase_Dots valueRO3 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(1f, 0f);
			bool flag3 = DTool.TileCheck(in valueRO3, in offset);
			ref readonly TileBase_Dots valueRO4 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(1f, -1f);
			bool flag4 = DTool.TileCheck(in valueRO4, in offset);
			ref readonly TileBase_Dots valueRO5 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(0f, -1f);
			bool flag5 = DTool.TileCheck(in valueRO5, in offset);
			ref readonly TileBase_Dots valueRO6 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(-1f, -1f);
			bool flag6 = DTool.TileCheck(in valueRO6, in offset);
			ref readonly TileBase_Dots valueRO7 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(-1f, 0f);
			bool flag7 = DTool.TileCheck(in valueRO7, in offset);
			ref readonly TileBase_Dots valueRO8 = ref uncheckedRefRO2.ValueRO;
			offset = new Vector2Data(-1f, 1f);
			bool flag8 = DTool.TileCheck(in valueRO8, in offset);
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			bool flag15 = false;
			bool flag16 = false;
			bool flag17 = false;
			bool flag18 = false;
			bool flag19 = false;
			bool flag20 = false;
			bool flag21 = false;
			bool flag22 = false;
			bool flag23 = false;
			bool flag24 = false;
			bool flag25 = false;
			if (!flag && !flag3 && !flag5 && !flag7)
			{
				flag12 = true;
				Entity e2 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e2, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI / 4f),
					Scale = 1f
				});
			}
			else if (flag && !flag3 && !flag5 && !flag7)
			{
				flag16 = true;
				Entity e3 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Small);
				entityCommandBuffer.SetComponent(e3, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (!flag && flag3 && !flag5 && !flag7)
			{
				flag13 = true;
				Entity e4 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Small);
				entityCommandBuffer.SetComponent(e4, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, 4.712389f),
					Scale = 1f
				});
			}
			else if (!flag && !flag3 && flag5 && !flag7)
			{
				flag9 = true;
				Entity e5 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Small);
				entityCommandBuffer.SetComponent(e5, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI),
					Scale = 1f
				});
			}
			else if (!flag && !flag3 && !flag5 && flag7)
			{
				flag13 = true;
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e6 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Small);
				entityCommandBuffer.SetComponent(e6, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI / 2f),
					Scale = 1f
				});
			}
			else if (!flag && flag3 && !flag5 && flag7)
			{
				flag10 = true;
				Entity e7 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e7, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && !flag3 && flag5 && !flag7)
			{
				flag17 = true;
				Entity e8 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e8, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && flag3 && !flag5 && !flag7)
			{
				flag18 = true;
				if (!flag2)
				{
					flag20 = true;
				}
				Entity e9 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Big);
				entityCommandBuffer.SetComponent(e9, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (!flag && flag3 && flag5 && !flag7)
			{
				flag14 = true;
				if (!flag4)
				{
					flag21 = true;
				}
				Entity e10 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Big);
				entityCommandBuffer.SetComponent(e10, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, 4.712389f),
					Scale = 1f
				});
			}
			else if (!flag && !flag3 && flag5 && flag7)
			{
				flag14 = true;
				if (!flag6)
				{
					flag21 = true;
				}
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e11 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Big);
				entityCommandBuffer.SetComponent(e11, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI),
					Scale = 1f
				});
			}
			else if (flag && !flag3 && !flag5 && flag7)
			{
				flag18 = true;
				if (!flag8)
				{
					flag20 = true;
				}
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e12 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Big);
				entityCommandBuffer.SetComponent(e12, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.Euler(0f, 0f, MathF.PI / 2f),
					Scale = 1f
				});
			}
			else if (flag && flag3 && !flag5 && flag7)
			{
				flag11 = true;
				if (!flag8)
				{
					flag23 = true;
				}
				if (!flag2)
				{
					flag20 = true;
				}
				Entity e13 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e13, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && flag3 && flag5 && !flag7)
			{
				flag19 = true;
				if (!flag2)
				{
					flag20 = true;
				}
				if (!flag4)
				{
					flag21 = true;
				}
				Entity e14 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e14, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (!flag && flag3 && flag5 && flag7)
			{
				flag15 = true;
				if (!flag4)
				{
					flag21 = true;
				}
				if (!flag6)
				{
					flag22 = true;
				}
				Entity e15 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e15, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && !flag3 && flag5 && flag7)
			{
				flag19 = true;
				if (!flag6)
				{
					flag21 = true;
				}
				if (!flag8)
				{
					flag20 = true;
				}
				entityCommandBuffer.AddComponent(e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
				Entity e16 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e16, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			else if (flag && flag3 && flag5 && flag7)
			{
				if (flag2 && flag4 && flag6 && flag8)
				{
					flag25 = true;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRO.ValueRO.ett_LayerWall);
					componentRWAfterCompletingDependency.ValueRW.Position += new float3(0f, 0f, -0.011f);
				}
				else
				{
					flag24 = true;
				}
				if (!flag2)
				{
					flag20 = true;
				}
				if (!flag4)
				{
					flag21 = true;
				}
				if (!flag6)
				{
					flag22 = true;
				}
				if (!flag8)
				{
					flag23 = true;
				}
				Entity e17 = entityCommandBuffer.Instantiate(uncheckedRefRO.ValueRO.ett_Collider_Full);
				entityCommandBuffer.SetComponent(e17, new LocalTransform
				{
					Position = uncheckedRefRW.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
			if (!flag9)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_D);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_D);
			}
			if (!flag10)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_LR);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_LR);
			}
			if (!flag11)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_LUR);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_LUR);
			}
			if (!flag12)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_Null);
			}
			if (!flag13)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_R);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_R);
			}
			if (!flag14)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_RD);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_RD);
			}
			if (!flag15)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_RDL);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_RDL);
			}
			if (!flag16)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_U);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_U);
			}
			if (!flag17)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_UD);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_UD);
			}
			if (!flag18)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_UR);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_UR);
			}
			if (!flag19)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Base_URD);
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_URD);
			}
			if (!flag20)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_Corner_UR);
			}
			if (!flag21)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_Corner_RD);
			}
			if (!flag22)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_Corner_DL);
			}
			if (!flag23)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_Corner_LU);
			}
			if (!flag24)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_Full);
			}
			if (!flag25)
			{
				entityCommandBuffer.DestroyEntity(uncheckedRefRO.ValueRO.ett_Wall_FullFog);
			}
			entityCommandBuffer.SetComponentEnabled<Tile_T9_Tile9_Dots>(e, value: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<TileBase_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Tile_T9_Tile9_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<BoundaryT2RoomCtrller>();
		__query_647615470_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_647615470_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00005C7F_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Tile_T9_Tile9System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Tile_T9_Tile9System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Tile_T9_Tile9System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
