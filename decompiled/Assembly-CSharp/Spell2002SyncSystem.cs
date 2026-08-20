using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateAfter(typeof(TeammateDeadEventSystem))]
[CompilerGenerated]
[UpdateInGroup(typeof(SpellEndSystemGroup))]
public class Spell2002SyncSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public BufferAccessor<LegsData> item2_BufferAccessor;

			public BufferAccessor<EssenceLegsData> item3_BufferAccessor;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<Spell2002Data>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item1_IntPtr, index), item2_BufferAccessor[index], item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell2002Data>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RO;

			private BufferTypeHandle<LegsData> item2_BufferTypeHandle_RW;

			private BufferTypeHandle<EssenceLegsData> item3_BufferTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<Spell2002Data> item5_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<LegsData>();
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<EssenceLegsData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2002Data>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<Spell2002Data>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<LegsData>();
			state.EntityManager.CompleteDependencyBeforeRW<EssenceLegsData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<Spell2002Data>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2002Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2002Data> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2002Data>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell2002Data>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_2
	{
		public struct ResolvedChunk
		{
			public BufferAccessor<LegsData> item1_BufferAccessor;

			public BufferAccessor<EssenceLegsData> item2_BufferAccessor;

			public BufferAccessor<LegsAttackData> item3_BufferAccessor;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, DynamicBuffer<LegsAttackData>, InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, DynamicBuffer<LegsAttackData>, InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(item1_BufferAccessor[index], item2_BufferAccessor[index], item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2002Data>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private BufferTypeHandle<LegsData> item1_BufferTypeHandle_RW;

			private BufferTypeHandle<EssenceLegsData> item2_BufferTypeHandle_RW;

			private BufferTypeHandle<LegsAttackData> item3_BufferTypeHandle_RW;

			private ComponentTypeHandle<Spell2002Data> item4_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item5_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<LegsData>();
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<EssenceLegsData>();
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<LegsAttackData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2002Data>();
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item1_BufferTypeHandle_RW);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, DynamicBuffer<LegsAttackData>, InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, DynamicBuffer<LegsAttackData>, InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<LegsData>();
			state.EntityManager.CompleteDependencyBeforeRW<EssenceLegsData>();
			state.EntityManager.CompleteDependencyBeforeRW<LegsAttackData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell2002Data>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_3
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell2002Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell2002Data> item1_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2002Data>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell2002Data>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_4
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2002Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<TeammateData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2002Data> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<TeammateData> item2_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2002Data>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell2002Data>();
			state.EntityManager.CompleteDependencyBeforeRO<TeammateData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_5
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell2002Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell2002Data> item1_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2002Data>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell2002Data>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1144294945_6
	{
		public struct ResolvedChunk
		{
			public BufferAccessor<FuseHeadEntity> item1_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<DynamicBuffer<FuseHeadEntity>> Get(int index)
			{
				return new QueryEnumerableWithEntity<DynamicBuffer<FuseHeadEntity>>(item1_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private BufferTypeHandle<FuseHeadEntity> item1_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<FuseHeadEntity>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item1_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<DynamicBuffer<FuseHeadEntity>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<DynamicBuffer<FuseHeadEntity>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<FuseHeadEntity>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1144294945_0.TypeHandle __IFE_1144294945_0_TypeHandle;

		public IFE_1144294945_1.TypeHandle __IFE_1144294945_1_TypeHandle;

		public IFE_1144294945_2.TypeHandle __IFE_1144294945_2_TypeHandle;

		public IFE_1144294945_3.TypeHandle __IFE_1144294945_3_TypeHandle;

		public IFE_1144294945_4.TypeHandle __IFE_1144294945_4_TypeHandle;

		public IFE_1144294945_5.TypeHandle __IFE_1144294945_5_TypeHandle;

		public IFE_1144294945_6.TypeHandle __IFE_1144294945_6_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		public ComponentLookup<Spell2002InitTag> __Spell2002InitTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<Spell2002StartFuseTag> __Spell2002StartFuseTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<FuseHeadData> __FuseHeadData_RO_ComponentLookup;

		public ComponentLookup<MatOverrideGhostEffect> __MatOverrideGhostEffect_RW_ComponentLookup;

		public ComponentLookup<Spell2002StartGhostTag> __Spell2002StartGhostTag_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1144294945_0_TypeHandle = new IFE_1144294945_0.TypeHandle(ref state);
			__IFE_1144294945_1_TypeHandle = new IFE_1144294945_1.TypeHandle(ref state);
			__IFE_1144294945_2_TypeHandle = new IFE_1144294945_2.TypeHandle(ref state);
			__IFE_1144294945_3_TypeHandle = new IFE_1144294945_3.TypeHandle(ref state);
			__IFE_1144294945_4_TypeHandle = new IFE_1144294945_4.TypeHandle(ref state);
			__IFE_1144294945_5_TypeHandle = new IFE_1144294945_5.TypeHandle(ref state);
			__IFE_1144294945_6_TypeHandle = new IFE_1144294945_6.TypeHandle(ref state);
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
			__Spell2002InitTag_RW_ComponentLookup = state.GetComponentLookup<Spell2002InitTag>();
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Spell2002StartFuseTag_RW_ComponentLookup = state.GetComponentLookup<Spell2002StartFuseTag>();
			__FuseHeadData_RO_ComponentLookup = state.GetComponentLookup<FuseHeadData>(isReadOnly: true);
			__MatOverrideGhostEffect_RW_ComponentLookup = state.GetComponentLookup<MatOverrideGhostEffect>();
			__Spell2002StartGhostTag_RW_ComponentLookup = state.GetComponentLookup<Spell2002StartGhostTag>();
		}
	}

	private static Dictionary<Entity, Teammate2Show> Teammates;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1144294945_0;

	private EntityQuery __query_1144294945_1;

	private EntityQuery __query_1144294945_2;

	private EntityQuery __query_1144294945_3;

	private EntityQuery __query_1144294945_4;

	private EntityQuery __query_1144294945_5;

	private EntityQuery __query_1144294945_6;

	[Preserve]
	protected override void OnCreate()
	{
		base.OnCreate();
		Teammates = new Dictionary<Entity, Teammate2Show>();
		EventMgr.DestroyAllTeammate = (Action)Delegate.Combine(EventMgr.DestroyAllTeammate, (Action)delegate
		{
			foreach (KeyValuePair<Entity, Teammate2Show> teammate in Teammates)
			{
				if (!teammate.Value.gameObject.IsDestroyed())
				{
					teammate.Value.OnSpellDestroy();
				}
			}
			Teammates.Clear();
		});
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, (Action)delegate
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			foreach (KeyValuePair<Entity, Teammate2Show> teammate2 in Teammates)
			{
				Entity spellEffectEntity = entityManager.GetComponentData<SpellComponentData>(teammate2.Key).SpellEffectEntity;
				Entity effect = entityManager.GetComponentData<EffectsCollectorData>(spellEffectEntity).Effect1;
				Entity effect2 = entityManager.GetComponentData<EffectsCollectorData>(spellEffectEntity).Effect3;
				LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(effect);
				LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(effect2);
				componentData.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
				componentData2.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
				entityManager.SetComponentData(effect, componentData);
				entityManager.SetComponentData(effect2, componentData2);
			}
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(FuseHeadData));
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				Entity entity = nativeArray[i];
				FuseHeadData componentData3 = entityManager.GetComponentData<FuseHeadData>(entity);
				LocalTransform componentData4 = entityManager.GetComponentData<LocalTransform>(componentData3.HeadEntity);
				LocalTransform componentData5 = entityManager.GetComponentData<LocalTransform>(componentData3.SafeHeadEntity);
				componentData4.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
				componentData5.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
				entityManager.SetComponentData(componentData3.HeadEntity, componentData4);
				entityManager.SetComponentData(componentData3.SafeHeadEntity, componentData5);
			}
		});
	}

	[Preserve]
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Teammates = null;
		EventMgr.DestroyAllTeammate = (Action)Delegate.Remove(EventMgr.DestroyAllTeammate, (Action)delegate
		{
			foreach (KeyValuePair<Entity, Teammate2Show> teammate in Teammates)
			{
				if (!teammate.Value.gameObject.IsDestroyed())
				{
					teammate.Value.OnSpellDestroy();
				}
			}
			Teammates.Clear();
		});
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, (Action)delegate
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			foreach (KeyValuePair<Entity, Teammate2Show> teammate2 in Teammates)
			{
				Entity spellEffectEntity = entityManager.GetComponentData<SpellComponentData>(teammate2.Key).SpellEffectEntity;
				Entity effect = entityManager.GetComponentData<EffectsCollectorData>(spellEffectEntity).Effect1;
				Entity effect2 = entityManager.GetComponentData<EffectsCollectorData>(spellEffectEntity).Effect3;
				LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(effect);
				LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(effect2);
				componentData.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
				componentData2.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
				entityManager.SetComponentData(effect, componentData);
				entityManager.SetComponentData(effect2, componentData2);
			}
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(FuseHeadData));
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				Entity entity = nativeArray[i];
				FuseHeadData componentData3 = entityManager.GetComponentData<FuseHeadData>(entity);
				LocalTransform componentData4 = entityManager.GetComponentData<LocalTransform>(componentData3.HeadEntity);
				LocalTransform componentData5 = entityManager.GetComponentData<LocalTransform>(componentData3.SafeHeadEntity);
				componentData4.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
				componentData5.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
				entityManager.SetComponentData(componentData3.HeadEntity, componentData4);
				entityManager.SetComponentData(componentData3.SafeHeadEntity, componentData5);
			}
		});
	}

	private void DestroyAllTeammate2()
	{
		foreach (KeyValuePair<Entity, Teammate2Show> teammate in Teammates)
		{
			if (!teammate.Value.gameObject.IsDestroyed())
			{
				teammate.Value.OnSpellDestroy();
			}
		}
		Teammates.Clear();
	}

	private void SetSafeMode()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		foreach (KeyValuePair<Entity, Teammate2Show> teammate in Teammates)
		{
			Entity spellEffectEntity = entityManager.GetComponentData<SpellComponentData>(teammate.Key).SpellEffectEntity;
			Entity effect = entityManager.GetComponentData<EffectsCollectorData>(spellEffectEntity).Effect1;
			Entity effect2 = entityManager.GetComponentData<EffectsCollectorData>(spellEffectEntity).Effect3;
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(effect);
			LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(effect2);
			componentData.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
			componentData2.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
			entityManager.SetComponentData(effect, componentData);
			entityManager.SetComponentData(effect2, componentData2);
		}
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(FuseHeadData));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Entity entity = nativeArray[i];
			FuseHeadData componentData3 = entityManager.GetComponentData<FuseHeadData>(entity);
			LocalTransform componentData4 = entityManager.GetComponentData<LocalTransform>(componentData3.HeadEntity);
			LocalTransform componentData5 = entityManager.GetComponentData<LocalTransform>(componentData3.SafeHeadEntity);
			componentData4.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
			componentData5.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
			entityManager.SetComponentData(componentData3.HeadEntity, componentData4);
			entityManager.SetComponentData(componentData3.SafeHeadEntity, componentData5);
		}
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		InternalCompilerInterface.UncheckedRefRO<LocalTransform> item;
		DynamicBuffer<LegsData> item2;
		DynamicBuffer<EssenceLegsData> item3;
		InternalCompilerInterface.UncheckedRefRO<Spell2002Data> item5;
		Entity entity;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> item11 in IFE_1144294945_0.Query(__query_1144294945_0, __TypeHandle.__IFE_1144294945_0_TypeHandle, ref base.CheckedStateRef))
		{
			item11.Deconstruct(out item, out item2, out item3, out var item4, out item5, out entity);
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item;
			DynamicBuffer<LegsData> legsData = item2;
			DynamicBuffer<EssenceLegsData> essenceLegsData = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO2 = item4;
			InternalCompilerInterface.UncheckedRefRO<Spell2002Data> uncheckedRefRO3 = item5;
			Entity entity2 = entity;
			GameObject gameObject = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/2002/Teammate2"));
			gameObject.SetActive(value: true);
			Teammate2Show component = gameObject.GetComponent<Teammate2Show>();
			component.transform.position = uncheckedRefRO.ValueRO.Position;
			component.colorType = uncheckedRefRO2.ValueRO.ColorType;
			component.mainHeadRootPos = uncheckedRefRO3.ValueRO.MainHeadRootPos;
			DynamicBuffer<FuseHeadEntity> buffer = entityManager.GetBuffer<FuseHeadEntity>(entity2);
			for (int i = 0; i < buffer.Length; i++)
			{
				RefRO<LocalToWorld> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, buffer[i].LegsRoot);
				component.fuseHeadRootPos.Add(componentROAfterCompletingDependency.ValueRO.Position);
			}
			component.Init(legsData, essenceLegsData, uncheckedRefRO3.ValueRO.DamageScaleRatio);
			Teammates.Add(entity2, component);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2002InitTag_RW_ComponentLookup, ref base.CheckedStateRef, entity2, value: false);
		}
		InternalCompilerInterface.UncheckedRefRW<Spell2002Data> item6;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item12 in IFE_1144294945_1.Query(__query_1144294945_1, __TypeHandle.__IFE_1144294945_1_TypeHandle, ref base.CheckedStateRef))
		{
			item12.Deconstruct(out item6, out var item7, out item, out entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2002Data> uncheckedRefRW = item6;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO4 = item7;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO5 = item;
			Entity entity3 = entity;
			if (Teammates.TryGetValue(entity3, out var value))
			{
				DynamicBuffer<FuseHeadEntity> buffer2 = entityManager.GetBuffer<FuseHeadEntity>(entity3);
				value.fuseHeadRootPos.Clear();
				for (int j = 0; j < buffer2.Length; j++)
				{
					RefRO<LocalToWorld> componentROAfterCompletingDependency2 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, buffer2[j].LegsRoot);
					RefRO<LocalToWorld> componentROAfterCompletingDependency3 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, buffer2[j].Entity);
					FuseHeadEntity value2 = buffer2[j];
					value2.HeadPos = componentROAfterCompletingDependency3.ValueRO.Position;
					buffer2[j] = value2;
					value.fuseHeadRootPos.Add(componentROAfterCompletingDependency2.ValueRO.Position);
				}
				EffectsCollectorData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRO4.ValueRO.SpellEffectEntity);
				LocalTransform componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRO4.ValueRO.SpellEffectEntity);
				RefRO<LocalToWorld> componentROAfterCompletingDependency4 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency.Effect1);
				RefRO<LocalTransform> componentROAfterCompletingDependency5 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency.Effect2);
				value.rootScale = componentAfterCompletingDependency2.Scale;
				value.spellScale = uncheckedRefRO5.ValueRO.Scale;
				uncheckedRefRW.ValueRW.MainHeadPos = componentROAfterCompletingDependency4.ValueRO.Position;
				uncheckedRefRW.ValueRW.MainHeadRootPos = componentROAfterCompletingDependency5.ValueRO.Position;
				value.mainHeadRootPos = uncheckedRefRO5.ValueRO.Position + uncheckedRefRO5.ValueRO.Scale * componentAfterCompletingDependency2.Scale * new float3(0f, 0f, 0f - componentROAfterCompletingDependency5.ValueRO.Position.y);
			}
		}
		foreach (QueryEnumerableWithEntity<DynamicBuffer<LegsData>, DynamicBuffer<EssenceLegsData>, DynamicBuffer<LegsAttackData>, InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item13 in IFE_1144294945_2.Query(__query_1144294945_2, __TypeHandle.__IFE_1144294945_2_TypeHandle, ref base.CheckedStateRef))
		{
			item13.Deconstruct(out item2, out item3, out var item8, out item6, out item, out entity);
			DynamicBuffer<LegsData> legsData2 = item2;
			DynamicBuffer<EssenceLegsData> legsData3 = item3;
			DynamicBuffer<LegsAttackData> legsAttackData = item8;
			InternalCompilerInterface.UncheckedRefRW<Spell2002Data> uncheckedRefRW2 = item6;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO6 = item;
			Entity key = entity;
			if (Teammates.TryGetValue(key, out var value3) && !(value3 == null) && !value3.gameObject.IsDestroyed() && value3.gameObject.activeInHierarchy)
			{
				value3.transform.position = uncheckedRefRO6.ValueRO.Position;
				value3.SyncLegsData(legsData2);
				value3.SyncEssenceLegsData(legsData3);
				if (uncheckedRefRW2.ValueRO.IsPortal)
				{
					uncheckedRefRW2.ValueRW.IsPortal = false;
					value3.ClearEssenceLegsEffect();
				}
				SyncAttackEffect(legsAttackData, value3);
			}
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> item14 in IFE_1144294945_3.Query(__query_1144294945_3, __TypeHandle.__IFE_1144294945_3_TypeHandle, ref base.CheckedStateRef))
		{
			item14.Deconstruct(out item5, out entity);
			DestroyMono(entity);
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>> item15 in IFE_1144294945_4.Query(__query_1144294945_4, __TypeHandle.__IFE_1144294945_4_TypeHandle, ref base.CheckedStateRef))
		{
			item15.Deconstruct(out item6, out var item9, out entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2002Data> uncheckedRefRW3 = item6;
			InternalCompilerInterface.UncheckedRefRO<TeammateData> uncheckedRefRO7 = item9;
			Entity key2 = entity;
			if (uncheckedRefRW3.ValueRO.IsLegInvisible == uncheckedRefRO7.ValueRO.IsHoldByTeammate6 && Teammates.TryGetValue(key2, out var value4))
			{
				uncheckedRefRW3.ValueRW.IsLegInvisible = !uncheckedRefRW3.ValueRO.IsLegInvisible;
				value4.HideOrShowLeg(uncheckedRefRW3.ValueRO.IsLegInvisible);
			}
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>> item16 in IFE_1144294945_5.Query(__query_1144294945_5, __TypeHandle.__IFE_1144294945_5_TypeHandle, ref base.CheckedStateRef))
		{
			item16.Deconstruct(out item5, out entity);
			Entity entity4 = entity;
			if (Teammates.TryGetValue(entity4, out var value5))
			{
				value5.StartFuse();
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2002StartFuseTag_RW_ComponentLookup, ref base.CheckedStateRef, entity4, value: false);
			}
		}
		foreach (QueryEnumerableWithEntity<DynamicBuffer<FuseHeadEntity>> item17 in IFE_1144294945_6.Query(__query_1144294945_6, __TypeHandle.__IFE_1144294945_6_TypeHandle, ref base.CheckedStateRef))
		{
			item17.Deconstruct(out var item10, out entity);
			DynamicBuffer<FuseHeadEntity> dynamicBuffer = item10;
			Entity entity5 = entity;
			if (!Teammates.TryGetValue(entity5, out var value6))
			{
				continue;
			}
			value6.StartGhost();
			using (NativeArray<FuseHeadEntity>.Enumerator enumerator8 = dynamicBuffer.GetEnumerator())
			{
				while (enumerator8.MoveNext())
				{
					FuseHeadData fuseHeadData = InternalCompilerInterface.GetComponentAfterCompletingDependency(entity: enumerator8.Current.Entity, componentLookup: ref __TypeHandle.__FuseHeadData_RO_ComponentLookup, state: ref base.CheckedStateRef);
					RefRW<MatOverrideGhostEffect> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideGhostEffect_RW_ComponentLookup, ref base.CheckedStateRef, fuseHeadData.HeadEntity);
					RefRW<MatOverrideGhostEffect> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideGhostEffect_RW_ComponentLookup, ref base.CheckedStateRef, fuseHeadData.SafeHeadEntity);
					componentRWAfterCompletingDependency.ValueRW.ApplyGhostEffect = 1f;
					componentRWAfterCompletingDependency2.ValueRW.ApplyGhostEffect = 1f;
				}
			}
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2002StartGhostTag_RW_ComponentLookup, ref base.CheckedStateRef, entity5, value: false);
		}
	}

	private void SyncAttackEffect(DynamicBuffer<LegsAttackData> legsAttackData, Teammate2Show teammate2)
	{
		for (int i = 0; i < legsAttackData.Length; i++)
		{
			LegsAttackData legsAttackData2 = legsAttackData[i];
			if (legsAttackData2.AttackType == LegsAttackType.Suck)
			{
				teammate2.SuckOnce(legsAttackData2.LegIndex);
			}
		}
	}

	public static void DestroyMono(Entity entity)
	{
		if (Teammates.TryGetValue(entity, out var value))
		{
			if (!value.IsDestroyed())
			{
				value.OnSpellDestroy();
			}
			Teammates.Remove(entity);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2002InitTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell2002Data>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EssenceLegsData>();
		__query_1144294945_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2002Data>();
		__query_1144294945_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EssenceLegsData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsAttackData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2002Data>();
		__query_1144294945_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDestroyTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell2002Data>();
		__query_1144294945_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2002Data>();
		__query_1144294945_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2002StartFuseTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell2002Data>();
		__query_1144294945_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2002StartGhostTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<FuseHeadEntity>();
		__query_1144294945_6 = entityQueryBuilder2.Build(ref state);
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
	public Spell2002SyncSystem()
	{
	}
}
