using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;

[BurstCompile]
public readonly struct SpellAspect : IAspect, IQueryTypeParameter, IAspectCreate<SpellAspect>
{
	public struct Lookup : InternalCompilerInterface.IAspectLookup<SpellAspect>
	{
		private ComponentLookup<SpellComponentData> SpellAspect_DataCAc;

		private ComponentLookup<SpellConfigComponentData> SpellAspect_ConfigCAc;

		[ReadOnly]
		private ComponentLookup<SpellElementEffectComponentData> SpellAspect_ElementEffectCAc;

		private ComponentLookup<SpellMovementComponentData> SpellAspect_MovementCAc;

		private ComponentLookup<LocalTransform> SpellAspect_TransformCAc;

		public SpellAspect this[Entity entity] => new SpellAspect(InternalCompilerInterface.GetComponentRefRW(ref SpellAspect_DataCAc, entity), InternalCompilerInterface.GetComponentRefRW(ref SpellAspect_ConfigCAc, entity), InternalCompilerInterface.GetComponentRefRO(ref SpellAspect_ElementEffectCAc, entity), InternalCompilerInterface.GetComponentRefRW(ref SpellAspect_MovementCAc, entity), entity, InternalCompilerInterface.GetComponentRefRW(ref SpellAspect_TransformCAc, entity));

		public Lookup(ref SystemState state)
		{
			SpellAspect_DataCAc = state.GetComponentLookup<SpellComponentData>();
			SpellAspect_ConfigCAc = state.GetComponentLookup<SpellConfigComponentData>();
			SpellAspect_ElementEffectCAc = state.GetComponentLookup<SpellElementEffectComponentData>(isReadOnly: true);
			SpellAspect_MovementCAc = state.GetComponentLookup<SpellMovementComponentData>();
			SpellAspect_TransformCAc = state.GetComponentLookup<LocalTransform>();
		}

		public void Update(ref SystemState state)
		{
			SpellAspect_DataCAc.Update(ref state);
			SpellAspect_ConfigCAc.Update(ref state);
			SpellAspect_ElementEffectCAc.Update(ref state);
			SpellAspect_MovementCAc.Update(ref state);
			SpellAspect_TransformCAc.Update(ref state);
		}
	}

	public struct ResolvedChunk
	{
		public NativeArray<SpellComponentData> SpellAspect_DataNaC;

		public NativeArray<SpellConfigComponentData> SpellAspect_ConfigNaC;

		public NativeArray<SpellElementEffectComponentData> SpellAspect_ElementEffectNaC;

		public NativeArray<SpellMovementComponentData> SpellAspect_MovementNaC;

		public NativeArray<Entity> SpellAspect_EntityNaE;

		public NativeArray<LocalTransform> SpellAspect_TransformNaC;

		public int Length;

		public SpellAspect this[int index] => new SpellAspect(new RefRW<SpellComponentData>(SpellAspect_DataNaC, index), new RefRW<SpellConfigComponentData>(SpellAspect_ConfigNaC, index), new RefRO<SpellElementEffectComponentData>(SpellAspect_ElementEffectNaC, index), new RefRW<SpellMovementComponentData>(SpellAspect_MovementNaC, index), SpellAspect_EntityNaE[index], new RefRW<LocalTransform>(SpellAspect_TransformNaC, index));
	}

	public struct TypeHandle
	{
		private ComponentTypeHandle<SpellComponentData> SpellAspect_DataCAc;

		private ComponentTypeHandle<SpellConfigComponentData> SpellAspect_ConfigCAc;

		[ReadOnly]
		private ComponentTypeHandle<SpellElementEffectComponentData> SpellAspect_ElementEffectCAc;

		private ComponentTypeHandle<SpellMovementComponentData> SpellAspect_MovementCAc;

		private EntityTypeHandle SpellAspect_EntityEAc;

		private ComponentTypeHandle<LocalTransform> SpellAspect_TransformCAc;

		public TypeHandle(ref SystemState state)
		{
			SpellAspect_DataCAc = state.GetComponentTypeHandle<SpellComponentData>();
			SpellAspect_ConfigCAc = state.GetComponentTypeHandle<SpellConfigComponentData>();
			SpellAspect_ElementEffectCAc = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
			SpellAspect_MovementCAc = state.GetComponentTypeHandle<SpellMovementComponentData>();
			SpellAspect_EntityEAc = state.GetEntityTypeHandle();
			SpellAspect_TransformCAc = state.GetComponentTypeHandle<LocalTransform>();
		}

		public void Update(ref SystemState state)
		{
			SpellAspect_DataCAc.Update(ref state);
			SpellAspect_ConfigCAc.Update(ref state);
			SpellAspect_ElementEffectCAc.Update(ref state);
			SpellAspect_MovementCAc.Update(ref state);
			SpellAspect_EntityEAc.Update(ref state);
			SpellAspect_TransformCAc.Update(ref state);
		}

		public ResolvedChunk Resolve(ArchetypeChunk chunk)
		{
			ResolvedChunk result = default(ResolvedChunk);
			result.SpellAspect_DataNaC = chunk.GetNativeArray(ref SpellAspect_DataCAc);
			result.SpellAspect_ConfigNaC = chunk.GetNativeArray(ref SpellAspect_ConfigCAc);
			result.SpellAspect_ElementEffectNaC = chunk.GetNativeArray(ref SpellAspect_ElementEffectCAc);
			result.SpellAspect_MovementNaC = chunk.GetNativeArray(ref SpellAspect_MovementCAc);
			result.SpellAspect_EntityNaE = chunk.GetNativeArray(SpellAspect_EntityEAc);
			result.SpellAspect_TransformNaC = chunk.GetNativeArray(ref SpellAspect_TransformCAc);
			result.Length = chunk.Count;
			return result;
		}
	}

	public struct Enumerator : IEnumerator<SpellAspect>, IEnumerator, IDisposable, IEnumerable<SpellAspect>, IEnumerable
	{
		private ResolvedChunk _Resolved;

		private InternalEntityQueryEnumerator _QueryEnumerator;

		private TypeHandle _Handle;

		public SpellAspect Current => _Resolved[_QueryEnumerator.IndexInChunk];

		object IEnumerator.Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		internal Enumerator(EntityQuery query, TypeHandle typeHandle)
		{
			_QueryEnumerator = new InternalEntityQueryEnumerator(query);
			_Handle = typeHandle;
			_Resolved = default(ResolvedChunk);
		}

		public void Dispose()
		{
			_QueryEnumerator.Dispose();
		}

		public bool MoveNext()
		{
			if (_QueryEnumerator.MoveNextHotLoop())
			{
				return true;
			}
			return MoveNextCold();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNextCold()
		{
			ArchetypeChunk chunk;
			bool num = _QueryEnumerator.MoveNextColdLoop(out chunk);
			if (num)
			{
				_Resolved = _Handle.Resolve(chunk);
			}
			return num;
		}

		public Enumerator GetEnumerator()
		{
			return this;
		}

		void IEnumerator.Reset()
		{
			throw new NotImplementedException();
		}

		IEnumerator<SpellAspect> IEnumerable<SpellAspect>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}

	public readonly Entity Entity;

	public readonly RefRW<LocalTransform> Transform;

	public readonly RefRW<SpellComponentData> Data;

	public readonly RefRW<SpellConfigComponentData> Config;

	public readonly RefRW<SpellMovementComponentData> Movement;

	public readonly RefRO<SpellElementEffectComponentData> ElementEffect;

	[BurstCompile]
	public TakeDamageInfo_Dots MakeDamageInfo(bool costPenetrate)
	{
		TakeDamageInfo_Dots.NewInfo(Entity, costPenetrate, in Config.ValueRO, in Movement.ValueRO, in Transform.ValueRO, in ElementEffect.ValueRO, in Data.ValueRO, out var info);
		return info;
	}

	public SpellAspect(RefRW<SpellComponentData> spellaspect_dataRef, RefRW<SpellConfigComponentData> spellaspect_configRef, RefRO<SpellElementEffectComponentData> spellaspect_elementeffectRef, RefRW<SpellMovementComponentData> spellaspect_movementRef, Entity spellaspect_entityE, RefRW<LocalTransform> spellaspect_transformRef)
	{
		Data = spellaspect_dataRef;
		Config = spellaspect_configRef;
		ElementEffect = spellaspect_elementeffectRef;
		Movement = spellaspect_movementRef;
		Entity = spellaspect_entityE;
		Transform = spellaspect_transformRef;
	}

	public SpellAspect CreateAspect(Entity entity, ref SystemState systemState)
	{
		return new Lookup(ref systemState)[entity];
	}

	public void AddComponentRequirementsTo(ref UnsafeList<ComponentType> all)
	{
		UnsafeList<ComponentType> unsafeList = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
		ComponentType value = ComponentType.ReadWrite<SpellComponentData>();
		unsafeList.Add(in value);
		ComponentType value2 = ComponentType.ReadWrite<SpellConfigComponentData>();
		unsafeList.Add(in value2);
		ComponentType value3 = ComponentType.ReadOnly<SpellElementEffectComponentData>();
		unsafeList.Add(in value3);
		ComponentType value4 = ComponentType.ReadWrite<SpellMovementComponentData>();
		unsafeList.Add(in value4);
		ComponentType value5 = ComponentType.ReadWrite<LocalTransform>();
		unsafeList.Add(in value5);
		UnsafeList<ComponentType> withThese = unsafeList;
		InternalCompilerInterface.MergeWith(ref all, ref withThese);
		withThese.Dispose();
	}

	public static int GetRequiredComponentTypeCount()
	{
		return 5;
	}

	public static void AddRequiredComponentTypes(ref Span<ComponentType> componentTypes)
	{
		componentTypes[0] = ComponentType.ReadWrite<SpellComponentData>();
		componentTypes[1] = ComponentType.ReadWrite<SpellConfigComponentData>();
		componentTypes[2] = ComponentType.ReadOnly<SpellElementEffectComponentData>();
		componentTypes[3] = ComponentType.ReadWrite<SpellMovementComponentData>();
		componentTypes[4] = ComponentType.ReadWrite<LocalTransform>();
	}

	public static Enumerator Query(EntityQuery query, TypeHandle typeHandle)
	{
		return new Enumerator(query, typeHandle);
	}

	public void CompleteDependencyBeforeRO(ref SystemState state)
	{
		state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
		state.EntityManager.CompleteDependencyBeforeRO<SpellElementEffectComponentData>();
		state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
		state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
	}

	public void CompleteDependencyBeforeRW(ref SystemState state)
	{
		state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
		state.EntityManager.CompleteDependencyBeforeRO<SpellElementEffectComponentData>();
		state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
		state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
	}
}
