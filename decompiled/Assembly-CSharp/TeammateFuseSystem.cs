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

[BurstCompile]
[CompilerGenerated]
internal struct TeammateFuseSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<TeammateData> __TeammateData_RO_ComponentLookup;

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<Shadow_Dots> __Shadow_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		public ComponentLookup<MatOverrideFuseProgress> __MatOverrideFuseProgress_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public BufferLookup<FuseHeadEntity> __FuseHeadEntity_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<FuseHeadData> __FuseHeadData_RO_ComponentLookup;

		public ComponentLookup<Spell2002StartFuseTag> __Spell2002StartFuseTag_RW_ComponentLookup;

		public BufferLookup<Spell2004PillarBuffer> __Spell2004PillarBuffer_RW_BufferLookup;

		public BufferLookup<Spell2004WallBuffer> __Spell2004WallBuffer_RW_BufferLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<Spell2006FuseTag> __Spell2006FuseTag_RW_ComponentLookup;

		public BufferLookup<Spell2007FuseBuffer> __Spell2007FuseBuffer_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__TeammateData_RO_ComponentLookup = state.GetComponentLookup<TeammateData>(isReadOnly: true);
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Shadow_Dots_RW_ComponentLookup = state.GetComponentLookup<Shadow_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__MatOverrideFuseProgress_RW_ComponentLookup = state.GetComponentLookup<MatOverrideFuseProgress>();
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__FuseHeadEntity_RW_BufferLookup = state.GetBufferLookup<FuseHeadEntity>();
			__FuseHeadData_RO_ComponentLookup = state.GetComponentLookup<FuseHeadData>(isReadOnly: true);
			__Spell2002StartFuseTag_RW_ComponentLookup = state.GetComponentLookup<Spell2002StartFuseTag>();
			__Spell2004PillarBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2004PillarBuffer>();
			__Spell2004WallBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2004WallBuffer>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell2006FuseTag_RW_ComponentLookup = state.GetComponentLookup<Spell2006FuseTag>();
			__Spell2007FuseBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2007FuseBuffer>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00009134_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00009134_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00009134_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00009135_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00009135_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00009135_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1702762929_0;

	private EntityQuery __query_1702762929_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingletonBuffer<TeammateFuseRequestBuffer>();
		state.EntityManager.CreateSingletonBuffer<TeammateFusePairBuffer>();
	}

	[BurstCompile]
	private void SetFuseTeammateData(ref SystemState state, Entity mainTarget, Entity subTarget, TeammateData tempFuseTeammate, TeammateData request)
	{
		__query_1702762929_0.GetSingletonBuffer<TeammateFusePairBuffer>().Add(new TeammateFusePairBuffer
		{
			FuseMainTeammateEntity = mainTarget,
			FuseSubTeammateEntity = subTarget,
			FuseMainTeammateData = tempFuseTeammate,
			FuseSubTeammateData = request
		});
		switch (request.TeammateType)
		{
		case TeammateType.teammate1:
			Teammate1EnterFuseState(ref state, mainTarget);
			Teammate1EnterFuseState(ref state, subTarget);
			break;
		case TeammateType.teammate2:
			Teammate2EnterFuseState(ref state, mainTarget);
			Teammate2EnterFuseState(ref state, subTarget);
			break;
		case TeammateType.teammate3:
			Teammate3EnterFuseState(ref state, mainTarget);
			Teammate3EnterFuseState(ref state, subTarget);
			break;
		case TeammateType.teammate4:
			Teammate4EnterFuseState(ref state, mainTarget);
			Teammate4EnterFuseState(ref state, subTarget);
			break;
		case TeammateType.teammate5:
			Teammate5EnterFuseState(ref state, mainTarget);
			Teammate5EnterFuseState(ref state, subTarget);
			break;
		case TeammateType.teammate6:
			Teammate6EnterFuseState(ref state, mainTarget);
			Teammate6EnterFuseState(ref state, subTarget);
			break;
		case TeammateType.teammate7:
			Teammate7EnterFuseState(ref state, mainTarget);
			Teammate7EnterFuseState(ref state, subTarget);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		__query_1702762929_0.GetSingletonBuffer<TeammateFusePairBuffer>().Clear();
		EntityCommandBuffer CMD = new EntityCommandBuffer(Allocator.Temp);
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		NativeList<TeammateFuseRequestBuffer> targets = new NativeList<TeammateFuseRequestBuffer>(Allocator.Temp);
		foreach (TeammateFuseRequestBuffer item in __query_1702762929_1.GetSingletonBuffer<TeammateFuseRequestBuffer>())
		{
			TeammateFuseRequestBuffer value = item;
			int num = 0;
			if (!state.EntityManager.HasBuffer<TeammateOwnerInfoBuffer>(value.OwnerUnit))
			{
				continue;
			}
			DynamicBuffer<TeammateOwnerInfoBuffer> buffer = state.EntityManager.GetBuffer<TeammateOwnerInfoBuffer>(value.OwnerUnit);
			num = FindValidFuseTeammateIndex(ref state, value.TeammateData.TeammateType, buffer, value);
			if (num < 0 && targets.Length > 0)
			{
				int num2 = FindValidFuseTeammateIndex(ref state, value.TeammateData.TeammateType, targets, value);
				if (num2 >= 0)
				{
					Entity teammateEntity = targets[num2].TeammateEntity;
					Entity teammateEntity2 = value.TeammateEntity;
					SetFuseTeammateData(ref state, teammateEntity, teammateEntity2, targets[num2].TeammateData, value.TeammateData);
					targets.RemoveAt(num2);
					continue;
				}
			}
			if (num < 0)
			{
				targets.Add(in value);
				continue;
			}
			Entity teammateEntity3 = buffer[num].TeammateEntity;
			Entity teammateEntity4 = value.TeammateEntity;
			SetFuseTeammateData(ref state, teammateEntity3, teammateEntity4, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, buffer[num].TeammateEntity), value.TeammateData);
			buffer.RemoveAt(num);
		}
		foreach (TeammateFuseRequestBuffer item2 in targets)
		{
			TeammateFuseRequestBuffer current = item2;
			TeammateData teammateData = current.TeammateData;
			CMD.TeammateRegister(current.ChunkIndex, in current.TeammateEntity, in current.OwnerUnit, in teammateData);
		}
		foreach (Entity item3 in nativeList)
		{
			state.EntityManager.RemoveComponent<PhysicsCollider>(item3);
		}
		CMD.Playback(state.EntityManager);
		CMD.Dispose();
		nativeList.Dispose();
		__query_1702762929_1.GetSingletonBuffer<TeammateFuseRequestBuffer>().Clear();
	}

	private void Teammate1EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref state, target).ValueRW.ett_Shadow;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, ett_Shadow).ValueRW.Scale = 0f;
		SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, target).ValueRW);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
		Entity spellEffectEntity = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, target).SpellEffectEntity;
		EffectsCollectorData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, spellEffectEntity);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect1, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect2, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect3, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect4, value: true);
		if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, target).ColorType == SpellColorType.Fire)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, componentAfterCompletingDependency.Effect1).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, componentAfterCompletingDependency.Effect2).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, componentAfterCompletingDependency.Effect3).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, componentAfterCompletingDependency.Effect4).Effect1, value: true);
		}
	}

	private void Teammate2EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref state, target).ValueRW.ett_Shadow;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, ett_Shadow).ValueRW.Scale = 0f;
		SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, target).ValueRW);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
		Entity spellEffectEntity = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, target).SpellEffectEntity;
		EffectsCollectorData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, spellEffectEntity);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect1, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect3, value: true);
		if (componentAfterCompletingDependency.Effect4 != Entity.Null)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect4, value: true);
		}
		if (componentAfterCompletingDependency.Effect5 != Entity.Null)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency.Effect5, value: true);
		}
		DynamicBuffer<FuseHeadEntity> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__FuseHeadEntity_RW_BufferLookup, ref state, target);
		for (int i = 0; i < bufferAfterCompletingDependency.Length; i++)
		{
			RefRO<FuseHeadData> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__FuseHeadData_RO_ComponentLookup, ref state, bufferAfterCompletingDependency[i].Entity);
			Entity fireEffectEntity = componentROAfterCompletingDependency.ValueRO.FireEffectEntity;
			if (fireEffectEntity != Entity.Null)
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, fireEffectEntity, value: true);
			}
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentROAfterCompletingDependency.ValueRO.HeadEntity, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentROAfterCompletingDependency.ValueRO.SafeHeadEntity, value: true);
		}
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2002StartFuseTag_RW_ComponentLookup, ref state, target, value: true);
	}

	private void Teammate4EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		DynamicBuffer<Spell2004PillarBuffer> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2004PillarBuffer_RW_BufferLookup, ref state, target);
		DynamicBuffer<Spell2004WallBuffer> bufferAfterCompletingDependency2 = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2004WallBuffer_RW_BufferLookup, ref state, target);
		using (NativeArray<Spell2004PillarBuffer>.Enumerator enumerator = bufferAfterCompletingDependency.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EffectsCollectorData effectsCollectorData = InternalCompilerInterface.GetComponentAfterCompletingDependency(entity: enumerator.Current.Entity, componentLookup: ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, state: ref state);
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, effectsCollectorData.Effect3, value: true);
				if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, target).ColorType == SpellColorType.Fire || InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, target).ColorType == SpellColorType.Void)
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, effectsCollectorData.Effect5, value: true);
				}
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, effectsCollectorData.Effect4).ValueRW.Position = new float3(9999f, 9999f, 9999f);
			}
		}
		using (NativeArray<Spell2004WallBuffer>.Enumerator enumerator2 = bufferAfterCompletingDependency2.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				EffectsCollectorData effectsCollectorData2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(entity: enumerator2.Current.Entity, componentLookup: ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, state: ref state);
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, effectsCollectorData2.Effect3, value: true);
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, effectsCollectorData2.Effect4, value: true);
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, effectsCollectorData2.Effect4).ValueRW.Position = new float3(9999f, 9999f, 9999f);
			}
		}
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
	}

	private void Teammate3EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref state, target).ValueRW.ett_Shadow;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, ett_Shadow).ValueRW.Scale = 0f;
		SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, target).ValueRW);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state, target).ValueRW.DurationTimer = 0f;
		DynamicBuffer<Spell2003TentacleEffectData> buffer = state.EntityManager.GetBuffer<Spell2003TentacleEffectData>(target);
		bool flag = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, target).ColorType == SpellColorType.Fire;
		foreach (Spell2003TentacleEffectData item in buffer)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, item.IdleEffectEntity, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, item.AttackEffectEntity, value: true);
			if (flag)
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, item.IdleEffectEntity).Effect1, value: true);
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, item.AttackEffectEntity).Effect1, value: true);
			}
		}
	}

	private void Teammate5EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref state, target).ValueRW.ett_Shadow;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, ett_Shadow).ValueRW.Scale = 0f;
		SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, target).ValueRW);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
		Entity spellEffectEntity = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, target).SpellEffectEntity;
		Entity effect = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, spellEffectEntity).Effect1;
		Entity effect2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, spellEffectEntity).Effect2;
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect).Effect1, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect).Effect2, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect).Effect3, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect2).Effect1, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect2).Effect2, value: true);
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect2).Effect3, value: true);
		if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, target).ColorType == SpellColorType.Fire)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect).Effect1).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect).Effect2).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect).Effect3).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect2).Effect1).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect2).Effect2).Effect1, value: true);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, effect2).Effect3).Effect1, value: true);
		}
	}

	private void Teammate6EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref state, target).ValueRW.ett_Shadow;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, ett_Shadow).ValueRW.Scale = 0f;
		SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, target).ValueRW);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
		InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell2006FuseTag_RW_ComponentLookup, ref state, target, value: true);
	}

	private void Teammate7EnterFuseState(ref SystemState state, Entity target)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, target).ValueRW.IsFuseMaterial = true;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.InvincibleRegister();
		Entity ett_Shadow = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Shadow_Dots_RW_ComponentLookup, ref state, target).ValueRW.ett_Shadow;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, ett_Shadow).ValueRW.Scale = 0f;
		SpellTools.DisableTeammateCollider(in InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, target).ValueRW);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, target).ValueRW.CanBeTarget = false;
		using NativeArray<Spell2007FuseBuffer>.Enumerator enumerator = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell2007FuseBuffer_RW_BufferLookup, ref state, target).GetEnumerator();
		while (enumerator.MoveNext())
		{
			EffectsCollectorData effectsCollectorData = InternalCompilerInterface.GetComponentAfterCompletingDependency(entity: enumerator.Current.Entity, componentLookup: ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, state: ref state);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, effectsCollectorData.Effect2, value: true);
			if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, target).ColorType == SpellColorType.Fire)
			{
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, effectsCollectorData.Effect3).ValueRW.Scale = 0f;
			}
		}
	}

	private int FindValidFuseTeammateIndex(ref SystemState state, TeammateType type, DynamicBuffer<TeammateOwnerInfoBuffer> buffer, TeammateFuseRequestBuffer mainTeammateData)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			if (buffer[i].TeammateType != type)
			{
				continue;
			}
			TeammateData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, buffer[i].TeammateEntity);
			if (!componentAfterCompletingDependency.TeammateDelayDeathEffectActive)
			{
				int num = math.max(componentAfterCompletingDependency.TeammateMaxFuseLevel, mainTeammateData.TeammateData.TeammateMaxFuseLevel);
				if (componentAfterCompletingDependency.TeammateCurrentFuseLevel + mainTeammateData.TeammateData.TeammateCurrentFuseLevel < num)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int FindValidFuseTeammateIndex(ref SystemState state, TeammateType type, NativeList<TeammateFuseRequestBuffer> targets, TeammateFuseRequestBuffer mainTeammateData)
	{
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].TeammateData.TeammateType != type || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, targets[i].TeammateEntity))
			{
				continue;
			}
			TeammateData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, targets[i].TeammateEntity);
			if (!componentAfterCompletingDependency.TeammateDelayDeathEffectActive)
			{
				int num = math.max(componentAfterCompletingDependency.TeammateMaxFuseLevel, mainTeammateData.TeammateData.TeammateMaxFuseLevel);
				if (componentAfterCompletingDependency.TeammateCurrentFuseLevel + mainTeammateData.TeammateData.TeammateCurrentFuseLevel < num)
				{
					return i;
				}
			}
		}
		return -1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateFusePairBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1702762929_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateFuseRequestBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1702762929_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00009134_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00009135_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TeammateFuseSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateFuseSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateFuseSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
