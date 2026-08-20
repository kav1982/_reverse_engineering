using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;
using UnityEngine.Scripting;

[BurstCompile]
[Preserve]
internal class __UnmanagedPostProcessorOutput__1221673671587648887
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public unsafe static void EarlyInit()
	{
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(NewISystemScript), BurstRuntime.GetHashCode64<NewISystemScript>(), delegate(IntPtr self, IntPtr state)
		{
			NewISystemScript.__codegen__OnCreate_00000141_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			NewISystemScript.__codegen__OnUpdate_00000142_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((NewISystemScript*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "NewISystemScript", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj400System), BurstRuntime.GetHashCode64<SpecialObj400System>(), SpecialObj400System.__codegen__OnCreate, SpecialObj400System.__codegen__OnUpdate, null, null, null, null, "SpecialObj400System", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GlobalParticleEmitterEntitySystem), BurstRuntime.GetHashCode64<GlobalParticleEmitterEntitySystem>(), delegate(IntPtr self, IntPtr state)
		{
			((GlobalParticleEmitterEntitySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((GlobalParticleEmitterEntitySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((GlobalParticleEmitterEntitySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "GlobalParticleEmitterEntitySystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Curse_RandomBombSystem), BurstRuntime.GetHashCode64<Curse_RandomBombSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Curse_RandomBombSystem.__codegen__OnCreate_00004F04_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Curse_RandomBombSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Curse_RandomBombSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Curse_RandomBombSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Curse_RevengeGhostSystem), BurstRuntime.GetHashCode64<Curse_RevengeGhostSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Curse_RevengeGhostSystem.__codegen__OnCreate_00004F1C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Curse_RevengeGhostSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Curse_RevengeGhostSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Curse_RevengeGhostSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Relic_BlockSpellSystem), BurstRuntime.GetHashCode64<Relic_BlockSpellSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Relic_BlockSpellSystem.__codegen__OnCreate_00004F34_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Relic_BlockSpellSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Relic_BlockSpellSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Relic_BlockSpellSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Relic_FollowGhostSystem), BurstRuntime.GetHashCode64<Relic_FollowGhostSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Relic_FollowGhostSystem.__codegen__OnCreate_00004F4C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Relic_FollowGhostSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Relic_FollowGhostSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Relic_FollowGhostSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Relic_GluttonousSnakeBodySystem), BurstRuntime.GetHashCode64<Relic_GluttonousSnakeBodySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Relic_GluttonousSnakeBodySystem.__codegen__OnCreate_00004F64_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Relic_GluttonousSnakeBodySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Relic_GluttonousSnakeBodySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Relic_GluttonousSnakeBodySystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Relic_ShowUnitHPSystem), BurstRuntime.GetHashCode64<Relic_ShowUnitHPSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Relic_ShowUnitHPSystem.__codegen__OnCreate_00004F82_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Relic_ShowUnitHPSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Relic_ShowUnitHPSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Relic_ShowUnitHPSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AnimaPlaySystem), BurstRuntime.GetHashCode64<AnimaPlaySystem>(), delegate(IntPtr self, IntPtr state)
		{
			AnimaPlaySystem.__codegen__OnCreate_00004FB9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AnimaPlaySystem.__codegen__OnUpdate_00004FBA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AnimaPlaySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AnimaPlaySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AudioSourceInDotsSystem), BurstRuntime.GetHashCode64<AudioSourceInDotsSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((AudioSourceInDotsSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((AudioSourceInDotsSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((AudioSourceInDotsSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AudioSourceInDotsSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AudioSourceInDotsSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AutoDestroySystem), BurstRuntime.GetHashCode64<AutoDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			AutoDestroySystem.__codegen__OnCreate_00005004_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AutoDestroySystem.__codegen__OnUpdate_00005005_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AutoDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AutoDestroySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(BackCampPortalSystem), BurstRuntime.GetHashCode64<BackCampPortalSystem>(), delegate(IntPtr self, IntPtr state)
		{
			BackCampPortalSystem.__codegen__OnCreate_0000501C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((BackCampPortalSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((BackCampPortalSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "BackCampPortalSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(BattleFinishDropSystem), BurstRuntime.GetHashCode64<BattleFinishDropSystem>(), delegate(IntPtr self, IntPtr state)
		{
			BattleFinishDropSystem.__codegen__OnCreate_00005034_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((BattleFinishDropSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((BattleFinishDropSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "BattleFinishDropSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(BedroomDoorSystem), BurstRuntime.GetHashCode64<BedroomDoorSystem>(), delegate(IntPtr self, IntPtr state)
		{
			BedroomDoorSystem.__codegen__OnCreate_0000504C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((BedroomDoorSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((BedroomDoorSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "BedroomDoorSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(CampMirrorSystem), BurstRuntime.GetHashCode64<CampMirrorSystem>(), delegate(IntPtr self, IntPtr state)
		{
			CampMirrorSystem.__codegen__OnCreate_00005070_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((CampMirrorSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((CampMirrorSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "CampMirrorSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(CampSkinChangerSystem), BurstRuntime.GetHashCode64<CampSkinChangerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			CampSkinChangerSystem.__codegen__OnCreate_00005088_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((CampSkinChangerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((CampSkinChangerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "CampSkinChangerSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ClearRotationOnceSystem), BurstRuntime.GetHashCode64<ClearRotationOnceSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			ClearRotationOnceSystem.__codegen__OnUpdate_0000509F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ClearRotationOnceSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ClearRotationOnceSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(CreateNavMeshObstacleSystem), BurstRuntime.GetHashCode64<CreateNavMeshObstacleSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((CreateNavMeshObstacleSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((CreateNavMeshObstacleSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((CreateNavMeshObstacleSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((CreateNavMeshObstacleSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "CreateNavMeshObstacleSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EndlessGallerySystem), BurstRuntime.GetHashCode64<EndlessGallerySystem>(), delegate(IntPtr self, IntPtr state)
		{
			EndlessGallerySystem.__codegen__OnCreate_0000513B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessGallerySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EndlessGallerySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EndlessGallerySystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EndlessItemPickSystem), BurstRuntime.GetHashCode64<EndlessItemPickSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EndlessItemPickSystem.__codegen__OnCreate_00005155_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessItemPickSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			EndlessItemPickSystem.__codegen__OnDestroy_00005157_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EndlessItemPickSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EndlessItemPickSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EndlessRankingListSystem), BurstRuntime.GetHashCode64<EndlessRankingListSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EndlessRankingListSystem.__codegen__OnCreate_00005186_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessRankingListSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EndlessRankingListSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EndlessRankingListSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EntityCampSkinKeepSystem), BurstRuntime.GetHashCode64<EntityCampSkinKeepSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EntityCampSkinKeepSystem.__codegen__OnCreate_000051A2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((EntityCampSkinKeepSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EntityCampSkinKeepSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EntityCampSkinKeepSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EntityHarmoniousKeepSystem), BurstRuntime.GetHashCode64<EntityHarmoniousKeepSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EntityHarmoniousKeepSystem.__codegen__OnCreate_000051BA_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((EntityHarmoniousKeepSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EntityHarmoniousKeepSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EntityHarmoniousKeepSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EntityRandomFlipSystem), BurstRuntime.GetHashCode64<EntityRandomFlipSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EntityRandomFlipSystem.__codegen__OnCreate_000051D2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			EntityRandomFlipSystem.__codegen__OnUpdate_000051D3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EntityRandomFlipSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EntityRandomFlipSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EntityRandomKeepSystem), BurstRuntime.GetHashCode64<EntityRandomKeepSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EntityRandomKeepSystem.__codegen__OnCreate_000051EA_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			EntityRandomKeepSystem.__codegen__OnUpdate_000051EB_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EntityRandomKeepSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EntityRandomKeepSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EntityRandomRotateSystem), BurstRuntime.GetHashCode64<EntityRandomRotateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EntityRandomRotateSystem.__codegen__OnCreate_00005202_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			EntityRandomRotateSystem.__codegen__OnUpdate_00005203_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EntityRandomRotateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EntityRandomRotateSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(FollowEntitySystem), BurstRuntime.GetHashCode64<FollowEntitySystem>(), delegate(IntPtr self, IntPtr state)
		{
			FollowEntitySystem.__codegen__OnCreate_0000521B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			FollowEntitySystem.__codegen__OnUpdate_0000521C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((FollowEntitySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "FollowEntitySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(FrameAnimaSystem), BurstRuntime.GetHashCode64<FrameAnimaSystem>(), delegate(IntPtr self, IntPtr state)
		{
			FrameAnimaSystem.__codegen__OnCreate_0000524C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			FrameAnimaSystem.__codegen__OnUpdate_0000524D_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((FrameAnimaSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "FrameAnimaSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GallerySystem), BurstRuntime.GetHashCode64<GallerySystem>(), delegate(IntPtr self, IntPtr state)
		{
			GallerySystem.__codegen__OnCreate_0000527C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((GallerySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((GallerySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "GallerySystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GearSystem), BurstRuntime.GetHashCode64<GearSystem>(), delegate(IntPtr self, IntPtr state)
		{
			GearSystem.__codegen__OnCreate_00005296_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((GearSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			GearSystem.__codegen__OnDestroy_00005298_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((GearSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "GearSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GetGOByJobSysytem), BurstRuntime.GetHashCode64<GetGOByJobSysytem>(), delegate(IntPtr self, IntPtr state)
		{
			GetGOByJobSysytem.__codegen__OnCreate_000052C7_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((GetGOByJobSysytem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((GetGOByJobSysytem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "GetGOByJobSysytem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GetGOSystem), BurstRuntime.GetHashCode64<GetGOSystem>(), delegate(IntPtr self, IntPtr state)
		{
			GetGOSystem.__codegen__OnCreate_000052CF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((GetGOSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((GetGOSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "GetGOSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GuideRoomSystem), BurstRuntime.GetHashCode64<GuideRoomSystem>(), GuideRoomSystem.__codegen__OnCreate, GuideRoomSystem.__codegen__OnUpdate, null, null, null, null, "GuideRoomSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ItemDelayActiveTriggerSystem), BurstRuntime.GetHashCode64<ItemDelayActiveTriggerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ItemDelayActiveTriggerSystem.__codegen__OnCreate_00005305_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((ItemDelayActiveTriggerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ItemDelayActiveTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ItemDelayActiveTriggerSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ItemDropSystem), BurstRuntime.GetHashCode64<ItemDropSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ItemDropSystem.__codegen__OnCreate_0000531D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((ItemDropSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ItemDropSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ItemDropSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ItemFlashSystem), BurstRuntime.GetHashCode64<ItemFlashSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ItemFlashSystem.__codegen__OnCreate_00005336_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			ItemFlashSystem.__codegen__OnUpdate_00005337_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ItemFlashSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ItemFlashSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ItemInitialPopSystem), BurstRuntime.GetHashCode64<ItemInitialPopSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ItemInitialPopSystem.__codegen__OnCreate_00005363_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			ItemInitialPopSystem.__codegen__OnUpdate_00005364_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ItemInitialPopSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ItemInitialPopSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ItemSystem), BurstRuntime.GetHashCode64<ItemSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ItemSystem.__codegen__OnCreate_0000537B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((ItemSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ItemSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ItemSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(LayerCorrectSystem), BurstRuntime.GetHashCode64<LayerCorrectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			LayerCorrectSystem.__codegen__OnCreate_0000539B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			LayerCorrectSystem.__codegen__OnUpdate_0000539C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((LayerCorrectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "LayerCorrectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(LevelRewardSystem), BurstRuntime.GetHashCode64<LevelRewardSystem>(), delegate(IntPtr self, IntPtr state)
		{
			LevelRewardSystem.__codegen__OnCreate_000053CD_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((LevelRewardSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((LevelRewardSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "LevelRewardSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(BloodSplatSystem), BurstRuntime.GetHashCode64<BloodSplatSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((BloodSplatSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			BloodSplatSystem.__codegen__OnUpdate_000053F0_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((BloodSplatSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "BloodSplatSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(LockMotionZBeforeSystem), BurstRuntime.GetHashCode64<LockMotionZBeforeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			LockMotionZBeforeSystem.__codegen__OnCreate_00005484_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			LockMotionZBeforeSystem.__codegen__OnUpdate_00005485_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			LockMotionZBeforeSystem.__codegen__OnDestroy_00005486_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((LockMotionZBeforeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "LockMotionZBeforeSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(LockMotionZAfterSystem), BurstRuntime.GetHashCode64<LockMotionZAfterSystem>(), delegate(IntPtr self, IntPtr state)
		{
			LockMotionZAfterSystem.__codegen__OnCreate_000054B4_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			LockMotionZAfterSystem.__codegen__OnUpdate_000054B5_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			LockMotionZAfterSystem.__codegen__OnDestroy_000054B6_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((LockMotionZAfterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "LockMotionZAfterSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(RandomFlipSystem), BurstRuntime.GetHashCode64<RandomFlipSystem>(), delegate(IntPtr self, IntPtr state)
		{
			RandomFlipSystem.__codegen__OnCreate_00005510_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			RandomFlipSystem.__codegen__OnUpdate_00005511_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((RandomFlipSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "RandomFlipSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(RankingListSystem), BurstRuntime.GetHashCode64<RankingListSystem>(), delegate(IntPtr self, IntPtr state)
		{
			RankingListSystem.__codegen__OnCreate_00005540_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((RankingListSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((RankingListSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "RankingListSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ResourceChangerSystem), BurstRuntime.GetHashCode64<ResourceChangerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ResourceChangerSystem.__codegen__OnCreate_00005558_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((ResourceChangerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ResourceChangerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ResourceChangerSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(RotateSpellLerpCorrectSystem), BurstRuntime.GetHashCode64<RotateSpellLerpCorrectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			RotateSpellLerpCorrectSystem.__codegen__OnCreate_00005570_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((RotateSpellLerpCorrectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			RotateSpellLerpCorrectSystem.__codegen__OnDestroy_00005572_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((RotateSpellLerpCorrectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "RotateSpellLerpCorrectSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ShadowSystem), BurstRuntime.GetHashCode64<ShadowSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ShadowSystem.__codegen__OnCreate_000055C9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			ShadowSystem.__codegen__OnUpdate_000055CA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ShadowSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ShadowSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell10201CoinSystem), BurstRuntime.GetHashCode64<Spell10201CoinSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell10201CoinSystem.__codegen__OnCreate_000055F9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell10201CoinSystem.__codegen__OnUpdate_000055FA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell10201CoinSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell10201CoinSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ToiletSystem), BurstRuntime.GetHashCode64<ToiletSystem>(), delegate(IntPtr self, IntPtr state)
		{
			ToiletSystem.__codegen__OnCreate_00005611_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((ToiletSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((ToiletSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "ToiletSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UITextFloatByJobSysytem), BurstRuntime.GetHashCode64<UITextFloatByJobSysytem>(), delegate(IntPtr self, IntPtr state)
		{
			UITextFloatByJobSysytem.__codegen__OnCreate_00005619_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((UITextFloatByJobSysytem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UITextFloatByJobSysytem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UITextFloatByJobSysytem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AccessCampSystem), BurstRuntime.GetHashCode64<AccessCampSystem>(), delegate(IntPtr self, IntPtr state)
		{
			AccessCampSystem.__codegen__OnCreate_0000568A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((AccessCampSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AccessCampSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AccessCampSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AccessTriggerGuideSystem), BurstRuntime.GetHashCode64<AccessTriggerGuideSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((AccessTriggerGuideSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((AccessTriggerGuideSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AccessTriggerGuideSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AccessTriggerGuideSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AccessTriggerSystem), BurstRuntime.GetHashCode64<AccessTriggerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			AccessTriggerSystem.__codegen__OnCreate_000056BB_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((AccessTriggerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((AccessTriggerSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AccessTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AccessTriggerSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T0System), BurstRuntime.GetHashCode64<Access_T0System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T0System.__codegen__OnCreate_000056D4_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T0System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T0System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T10System), BurstRuntime.GetHashCode64<Access_T10System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T10System.__codegen__OnCreate_000056EC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T10System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T10System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T10System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T13System), BurstRuntime.GetHashCode64<Access_T13System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T13System.__codegen__OnCreate_00005704_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T13System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T13System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T13System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T15System), BurstRuntime.GetHashCode64<Access_T15System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T15System.__codegen__OnCreate_0000571D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T15System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T15System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T15System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T1System), BurstRuntime.GetHashCode64<Access_T1System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T1System.__codegen__OnCreate_00005735_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T1System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T1System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T1System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T26System), BurstRuntime.GetHashCode64<Access_T26System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T26System.__codegen__OnCreate_0000574D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T26System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T26System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T26System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T27System), BurstRuntime.GetHashCode64<Access_T27System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T27System.__codegen__OnCreate_00005765_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T27System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T27System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T27System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T28System), BurstRuntime.GetHashCode64<Access_T28System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T28System.__codegen__OnCreate_0000577D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T28System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T28System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T28System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T29System), BurstRuntime.GetHashCode64<Access_T29System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T29System.__codegen__OnCreate_00005795_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T29System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T29System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T29System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T6System), BurstRuntime.GetHashCode64<Access_T6System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T6System.__codegen__OnCreate_000057AD_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T6System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T6System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T6System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Access_T8System), BurstRuntime.GetHashCode64<Access_T8System>(), delegate(IntPtr self, IntPtr state)
		{
			Access_T8System.__codegen__OnCreate_000057C6_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Access_T8System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Access_T8System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Access_T8System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary2_T0System), BurstRuntime.GetHashCode64<Boundary2_T0System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary2_T0System.__codegen__OnCreate_000057DF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary2_T0System.__codegen__OnUpdate_000057E0_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary2_T0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary2_T0System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary2_T10System), BurstRuntime.GetHashCode64<Boundary2_T10System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary2_T10System.__codegen__OnCreate_00005810_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary2_T10System.__codegen__OnUpdate_00005811_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary2_T10System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary2_T10System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T0System), BurstRuntime.GetHashCode64<Boundary_T0System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T0System.__codegen__OnCreate_00005844_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary_T0System.__codegen__OnUpdate_00005845_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T0System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T10System), BurstRuntime.GetHashCode64<Boundary_T10System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T10System.__codegen__OnCreate_00005876_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary_T10System.__codegen__OnUpdate_00005877_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T10System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T10System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T11System), BurstRuntime.GetHashCode64<Boundary_T11System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T11System.__codegen__OnCreate_000058A8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T11System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T11System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T11System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T13System), BurstRuntime.GetHashCode64<Boundary_T13System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T13System.__codegen__OnCreate_000058C1_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary_T13System.__codegen__OnUpdate_000058C2_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T13System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T13System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T1System), BurstRuntime.GetHashCode64<Boundary_T1System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T1System.__codegen__OnCreate_000058F2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary_T1System.__codegen__OnUpdate_000058F3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T1System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T1System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T2System), BurstRuntime.GetHashCode64<Boundary_T2System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T2System.__codegen__OnCreate_00005924_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T2System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T2System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T2System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T3System), BurstRuntime.GetHashCode64<Boundary_T3System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T3System.__codegen__OnCreate_0000593D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T3System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T3System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T3System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T4System), BurstRuntime.GetHashCode64<Boundary_T4System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T4System.__codegen__OnCreate_0000596F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary_T4System.__codegen__OnUpdate_00005970_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T4System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T4System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Boundary_T5System), BurstRuntime.GetHashCode64<Boundary_T5System>(), delegate(IntPtr self, IntPtr state)
		{
			Boundary_T5System.__codegen__OnCreate_000059A1_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Boundary_T5System.__codegen__OnUpdate_000059A2_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Boundary_T5System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Boundary_T5System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(DoorCampGuideSystem), BurstRuntime.GetHashCode64<DoorCampGuideSystem>(), delegate(IntPtr self, IntPtr state)
		{
			DoorCampGuideSystem.__codegen__OnCreate_000059D8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((DoorCampGuideSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((DoorCampGuideSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "DoorCampGuideSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(DoorCampSystem), BurstRuntime.GetHashCode64<DoorCampSystem>(), delegate(IntPtr self, IntPtr state)
		{
			DoorCampSystem.__codegen__OnCreate_000059ED_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((DoorCampSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((DoorCampSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "DoorCampSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(DoorEndlessCampSystem), BurstRuntime.GetHashCode64<DoorEndlessCampSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((DoorEndlessCampSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((DoorEndlessCampSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((DoorEndlessCampSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "DoorEndlessCampSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Door_T0System), BurstRuntime.GetHashCode64<Door_T0System>(), delegate(IntPtr self, IntPtr state)
		{
			Door_T0System.__codegen__OnCreate_00005A0F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Door_T0System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Door_T0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Door_T0System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Door_T11System), BurstRuntime.GetHashCode64<Door_T11System>(), delegate(IntPtr self, IntPtr state)
		{
			Door_T11System.__codegen__OnCreate_00005A27_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Door_T11System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Door_T11System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Door_T11System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Door_T3_GuideSystem), BurstRuntime.GetHashCode64<Door_T3_GuideSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Door_T3_GuideSystem.__codegen__OnCreate_00005A3F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Door_T3_GuideSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Door_T3_GuideSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Door_T3_GuideSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Door_T6System), BurstRuntime.GetHashCode64<Door_T6System>(), delegate(IntPtr self, IntPtr state)
		{
			Door_T6System.__codegen__OnCreate_00005A57_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Door_T6System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Door_T6System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Door_T6System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Door_T8System), BurstRuntime.GetHashCode64<Door_T8System>(), delegate(IntPtr self, IntPtr state)
		{
			Door_T8System.__codegen__OnCreate_00005A6F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Door_T8System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Door_T8System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Door_T8System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T0_Tile0System), BurstRuntime.GetHashCode64<Tile_T0_Tile0System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T0_Tile0System.__codegen__OnCreate_00005A96_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T0_Tile0System.__codegen__OnUpdate_00005A97_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T0_Tile0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T0_Tile0System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T0_Tile6System), BurstRuntime.GetHashCode64<Tile_T0_Tile6System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T0_Tile6System.__codegen__OnCreate_00005AC7_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T0_Tile6System.__codegen__OnUpdate_00005AC8_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T0_Tile6System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T0_Tile6System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T10_Tile0System), BurstRuntime.GetHashCode64<Tile_T10_Tile0System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T10_Tile0System.__codegen__OnCreate_00005AF8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T10_Tile0System.__codegen__OnUpdate_00005AF9_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T10_Tile0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T10_Tile0System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T12_Tile5System), BurstRuntime.GetHashCode64<Tile_T12_Tile5System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T12_Tile5System.__codegen__OnCreate_00005B29_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T12_Tile5System.__codegen__OnUpdate_00005B2A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T12_Tile5System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T12_Tile5System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T1_Tile0System), BurstRuntime.GetHashCode64<Tile_T1_Tile0System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T1_Tile0System.__codegen__OnCreate_00005B5A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T1_Tile0System.__codegen__OnUpdate_00005B5B_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T1_Tile0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T1_Tile0System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T3_Tile0System), BurstRuntime.GetHashCode64<Tile_T3_Tile0System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T3_Tile0System.__codegen__OnCreate_00005B8B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T3_Tile0System.__codegen__OnUpdate_00005B8C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T3_Tile0System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T3_Tile0System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T3_Tile1System), BurstRuntime.GetHashCode64<Tile_T3_Tile1System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T3_Tile1System.__codegen__OnCreate_00005BBC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T3_Tile1System.__codegen__OnUpdate_00005BBD_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T3_Tile1System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T3_Tile1System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T3_Tile2System), BurstRuntime.GetHashCode64<Tile_T3_Tile2System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T3_Tile2System.__codegen__OnCreate_00005BED_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T3_Tile2System.__codegen__OnUpdate_00005BEE_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T3_Tile2System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T3_Tile2System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T8_Tile5System), BurstRuntime.GetHashCode64<Tile_T8_Tile5System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T8_Tile5System.__codegen__OnCreate_00005C1E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T8_Tile5System.__codegen__OnUpdate_00005C1F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T8_Tile5System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T8_Tile5System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T8_Tile9System), BurstRuntime.GetHashCode64<Tile_T8_Tile9System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T8_Tile9System.__codegen__OnCreate_00005C4F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Tile_T8_Tile9System.__codegen__OnUpdate_00005C50_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T8_Tile9System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T8_Tile9System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Tile_T9_Tile9System), BurstRuntime.GetHashCode64<Tile_T9_Tile9System>(), delegate(IntPtr self, IntPtr state)
		{
			Tile_T9_Tile9System.__codegen__OnCreate_00005C7F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T9_Tile9System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Tile_T9_Tile9System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Tile_T9_Tile9System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(WandInStoreSystem), BurstRuntime.GetHashCode64<WandInStoreSystem>(), delegate(IntPtr self, IntPtr state)
		{
			WandInStoreSystem.__codegen__OnCreate_00005C97_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((WandInStoreSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((WandInStoreSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "WandInStoreSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AllMixedEttSystem), BurstRuntime.GetHashCode64<AllMixedEttSystem>(), delegate(IntPtr self, IntPtr state)
		{
			AllMixedEttSystem.__codegen__OnCreate_00005CB2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AllMixedEttSystem.__codegen__OnUpdate_00005CB3_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AllMixedEttSystem.__codegen__OnDestroy_00005CB4_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AllMixedEttSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AllMixedEttSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AllSpecialObjEttSystem), BurstRuntime.GetHashCode64<AllSpecialObjEttSystem>(), delegate(IntPtr self, IntPtr state)
		{
			AllSpecialObjEttSystem.__codegen__OnCreate_00005CCF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AllSpecialObjEttSystem.__codegen__OnUpdate_00005CD0_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AllSpecialObjEttSystem.__codegen__OnDestroy_00005CD1_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AllSpecialObjEttSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AllSpecialObjEttSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(AllUnitEttSystem), BurstRuntime.GetHashCode64<AllUnitEttSystem>(), delegate(IntPtr self, IntPtr state)
		{
			AllUnitEttSystem.__codegen__OnCreate_00005CE9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			AllUnitEttSystem.__codegen__OnUpdate_00005CEA_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((AllUnitEttSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((AllUnitEttSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "AllUnitEttSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(GlobalRandomSystem), BurstRuntime.GetHashCode64<GlobalRandomSystem>(), GlobalRandomSystem.__codegen__OnCreate, null, null, null, null, null, "GlobalRandomSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(PlayerControllerDataSyncSystem), BurstRuntime.GetHashCode64<PlayerControllerDataSyncSystem>(), delegate(IntPtr self, IntPtr state)
		{
			PlayerControllerDataSyncSystem.__codegen__OnCreate_00005D10_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((PlayerControllerDataSyncSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((PlayerControllerDataSyncSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "PlayerControllerDataSyncSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(HybirdInveractiveObjSystem), BurstRuntime.GetHashCode64<HybirdInveractiveObjSystem>(), delegate(IntPtr self, IntPtr state)
		{
			HybirdInveractiveObjSystem.__codegen__OnCreate_00005D32_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((HybirdInveractiveObjSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((HybirdInveractiveObjSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "HybirdInveractiveObjSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SOFlipWhenRoomFlipSystem), BurstRuntime.GetHashCode64<SOFlipWhenRoomFlipSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SOFlipWhenRoomFlipSystem.__codegen__OnCreate_00005D4D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SOFlipWhenRoomFlipSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SOFlipWhenRoomFlipSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SOFlipWhenRoomFlipSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj101CompoundSystem), BurstRuntime.GetHashCode64<SpecialObj101CompoundSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj101CompoundSystem.__codegen__OnCreate_00005D65_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj101CompoundSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj101CompoundSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj101CompoundSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj101MoreInOneSystem), BurstRuntime.GetHashCode64<SpecialObj101MoreInOneSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj101MoreInOneSystem.__codegen__OnCreate_00005D7D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj101MoreInOneSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj101MoreInOneSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj101MoreInOneSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj101RerollSystem), BurstRuntime.GetHashCode64<SpecialObj101RerollSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj101RerollSystem.__codegen__OnCreate_00005D96_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj101RerollSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj101RerollSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj101RerollSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj103System), BurstRuntime.GetHashCode64<SpecialObj103System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj103System.__codegen__OnCreate_00005DAE_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj103System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj103System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj103System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj10System), BurstRuntime.GetHashCode64<SpecialObj10System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj10System.__codegen__OnCreate_00005DCB_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj10System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj10System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj10System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj16System), BurstRuntime.GetHashCode64<SpecialObj16System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj16System.__codegen__OnCreate_00005DE4_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpecialObj16System.__codegen__OnUpdate_00005DE5_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj16System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj16System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj17System), BurstRuntime.GetHashCode64<SpecialObj17System>(), delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj17System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj17System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj17System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj17System", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj17_StatueSystem), BurstRuntime.GetHashCode64<SpecialObj17_StatueSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj17_StatueSystem.__codegen__OnCreate_00005E2E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj17_StatueSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj17_StatueSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj17_StatueSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj18System), BurstRuntime.GetHashCode64<SpecialObj18System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj18System.__codegen__OnCreate_00005E46_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj18System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj18System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj18System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj19System), BurstRuntime.GetHashCode64<SpecialObj19System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj19System.__codegen__OnCreate_00005E6B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpecialObj19System.__codegen__OnUpdate_00005E6C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj19System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj19System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj21System), BurstRuntime.GetHashCode64<SpecialObj21System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj21System.__codegen__OnCreate_00005E86_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj21System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj21System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj21System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj2System), BurstRuntime.GetHashCode64<SpecialObj2System>(), delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj2System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj2System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj2System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj2System", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj30System), BurstRuntime.GetHashCode64<SpecialObj30System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj30System.__codegen__OnCreate_00005ECF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj30System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj30System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj30System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj3System), BurstRuntime.GetHashCode64<SpecialObj3System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj3System.__codegen__OnCreate_00005EEC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj3System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj3System*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj3System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj3System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj3_DaveSystem), BurstRuntime.GetHashCode64<SpecialObj3_DaveSystem>(), SpecialObj3_DaveSystem.__codegen__OnCreate, SpecialObj3_DaveSystem.__codegen__OnUpdate, null, null, null, null, "SpecialObj3_DaveSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj40System), BurstRuntime.GetHashCode64<SpecialObj40System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj40System.__codegen__OnCreate_00005F4F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj40System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj40System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj40System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj43System), BurstRuntime.GetHashCode64<SpecialObj43System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj43System.__codegen__OnCreate_00005F6A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj43System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj43System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj43System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj44BloodRoomSystem), BurstRuntime.GetHashCode64<SpecialObj44BloodRoomSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj44BloodRoomSystem.__codegen__OnCreate_00005F85_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj44BloodRoomSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj44BloodRoomSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj44BloodRoomSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj44System), BurstRuntime.GetHashCode64<SpecialObj44System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj44System.__codegen__OnCreate_00005F9A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj44System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj44System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj44System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj45BloodRoomSystem), BurstRuntime.GetHashCode64<SpecialObj45BloodRoomSystem>(), SpecialObj45BloodRoomSystem.__codegen__OnCreate, SpecialObj45BloodRoomSystem.__codegen__OnUpdate, null, null, null, null, "SpecialObj45BloodRoomSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj45System), BurstRuntime.GetHashCode64<SpecialObj45System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj45System.__codegen__OnCreate_00005FB9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj45System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj45System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj45System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj48System), BurstRuntime.GetHashCode64<SpecialObj48System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj48System.__codegen__OnCreate_00005FED_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj48System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj48System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj48System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj8EachThemeSystem), BurstRuntime.GetHashCode64<SpecialObj8EachThemeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj8EachThemeSystem.__codegen__OnCreate_00006036_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpecialObj8EachThemeSystem.__codegen__OnUpdate_00006037_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj8EachThemeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj8EachThemeSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj8MotherSystem), BurstRuntime.GetHashCode64<SpecialObj8MotherSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj8MotherSystem.__codegen__OnCreate_0000604E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj8MotherSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj8MotherSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj8MotherSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpecialObj8_T6System), BurstRuntime.GetHashCode64<SpecialObj8_T6System>(), delegate(IntPtr self, IntPtr state)
		{
			SpecialObj8_T6System.__codegen__OnCreate_00006066_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj8_T6System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpecialObj8_T6System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpecialObj8_T6System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1002RollBallSystem), BurstRuntime.GetHashCode64<Spell1002RollBallSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1002RollBallSystem.__codegen__OnCreate_00006095_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1002RollBallSystem.__codegen__OnUpdate_00006096_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1002RollBallSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1002RollBallSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1003ButterFlyBeAttackedSystem), BurstRuntime.GetHashCode64<Spell1003ButterFlyBeAttackedSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1003ButterFlyBeAttackedSystem.__codegen__OnCreate_000060D3_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1003ButterFlyBeAttackedSystem.__codegen__OnUpdate_000060D4_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1003ButterFlyBeAttackedSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1003ButterFlyBeAttackedSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1003ButterFlySystem), BurstRuntime.GetHashCode64<Spell1003ButterFlySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1003ButterFlySystem.__codegen__OnCreate_00006102_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1003ButterFlySystem.__codegen__OnUpdate_00006103_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1003ButterFlySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1003ButterFlySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1004LaserSystem), BurstRuntime.GetHashCode64<Spell1004LaserSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1004LaserSystem.__codegen__OnCreate_0000616E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1004LaserSystem.__codegen__OnUpdate_0000616F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1004LaserSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1004LaserSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1005PreFireworkSystem), BurstRuntime.GetHashCode64<Spell1005PreFireworkSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1005PreFireworkSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1005PreFireworkSystem.__codegen__OnUpdate_000061C2_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1005PreFireworkSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1005PreFireworkSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1006GhostFirePullSystem), BurstRuntime.GetHashCode64<Spell1006GhostFirePullSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1006GhostFirePullSystem.__codegen__OnCreate_000061DA_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1006GhostFirePullSystem.__codegen__OnUpdate_000061DB_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1006GhostFirePullSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1006GhostFirePullSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1006GhostFireSystem), BurstRuntime.GetHashCode64<Spell1006GhostFireSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1006GhostFireSystem.__codegen__OnCreate_00006208_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1006GhostFireSystem.__codegen__OnUpdate_00006209_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1006GhostFireSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1006GhostFireSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1007BlackHoleSystem), BurstRuntime.GetHashCode64<Spell1007BlackHoleSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1007BlackHoleSystem.__codegen__OnCreate_00006247_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1007BlackHoleSystem.__codegen__OnUpdate_00006248_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1007BlackHoleSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1007BlackHoleSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1007EffectRotateSystem), BurstRuntime.GetHashCode64<Spell1007EffectRotateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1007EffectRotateSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1007EffectRotateSystem.__codegen__OnUpdate_000062AB_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1007EffectRotateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1007EffectRotateSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1008ArcaneEffectSystem), BurstRuntime.GetHashCode64<Spell1008ArcaneEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1008ArcaneEffectSystem.__codegen__OnCreate_000062E2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1008ArcaneEffectSystem.__codegen__OnUpdate_000062E3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1008ArcaneEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1008ArcaneEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1008ArcaneExplosionSystem), BurstRuntime.GetHashCode64<Spell1008ArcaneExplosionSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1008ArcaneExplosionSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1008ArcaneExplosionSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1008ArcaneExplosionSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1008ArcaneExplosionSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1009BackMpNoBurstSystem), BurstRuntime.GetHashCode64<Spell1009BackMpNoBurstSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1009BackMpNoBurstSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1009BackMpNoBurstSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1009BackMpNoBurstSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1009BackMpNoBurstSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1009BackMpSystem), BurstRuntime.GetHashCode64<Spell1009BackMpSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1009BackMpSystem.__codegen__OnCreate_000063E4_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1009BackMpSystem.__codegen__OnUpdate_000063E5_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1009BackMpSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1009BackMpSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1009HitEffectSystem), BurstRuntime.GetHashCode64<Spell1009HitEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1009HitEffectSystem.__codegen__OnCreate_000063FA_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1009HitEffectSystem.__codegen__OnUpdate_000063FB_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1009HitEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1009HitEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1010SnakeBeforeMoveSystem), BurstRuntime.GetHashCode64<Spell1010SnakeBeforeMoveSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1010SnakeBeforeMoveSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1010SnakeBeforeMoveSystem.__codegen__OnUpdate_00006440_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1010SnakeBeforeMoveSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1010SnakeBeforeMoveSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1010SnakeFallDamageSystem), BurstRuntime.GetHashCode64<Spell1010SnakeFallDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1010SnakeFallDamageSystem.__codegen__OnCreate_0000646F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1010SnakeFallDamageSystem.__codegen__OnUpdate_00006470_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1010SnakeFallDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1010SnakeFallDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1010SnakeWalkSystem), BurstRuntime.GetHashCode64<Spell1010SnakeWalkSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1010SnakeWalkSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1010SnakeWalkSystem.__codegen__OnUpdate_000064EC_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1010SnakeWalkSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1010SnakeWalkSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1011DisintegrationRaySystem), BurstRuntime.GetHashCode64<Spell1011DisintegrationRaySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1011DisintegrationRaySystem.__codegen__OnCreate_00006546_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1011DisintegrationRaySystem.__codegen__OnUpdate_00006547_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1011DisintegrationRaySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1011DisintegrationRaySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1012TraceSystem), BurstRuntime.GetHashCode64<Spell1012TraceSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1012TraceSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1012TraceSystem.__codegen__OnUpdate_000065CF_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1012TraceSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1012TraceSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1012TrickMineSystem), BurstRuntime.GetHashCode64<Spell1012TrickMineSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1012TrickMineSystem.__codegen__OnCreate_000065FF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1012TrickMineSystem.__codegen__OnUpdate_00006600_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1012TrickMineSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1012TrickMineSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1013MeteorSystem), BurstRuntime.GetHashCode64<Spell1013MeteorSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1013MeteorSystem.__codegen__OnCreate_00006640_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1013MeteorSystem.__codegen__OnUpdate_00006641_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1013MeteorSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1013MeteorSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1014RainbowFallEffectSystem), BurstRuntime.GetHashCode64<Spell1014RainbowFallEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1014RainbowFallEffectSystem.__codegen__OnCreate_00006672_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1014RainbowFallEffectSystem.__codegen__OnUpdate_00006673_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1014RainbowFallEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1014RainbowFallEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1014RainbowSystem), BurstRuntime.GetHashCode64<Spell1014RainbowSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1014RainbowSystem.__codegen__OnCreate_000066A0_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1014RainbowSystem.__codegen__OnUpdate_000066A1_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1014RainbowSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1014RainbowSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1016DashEndSystem), BurstRuntime.GetHashCode64<Spell1016DashEndSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashEndSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashEndSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashEndSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1016DashEndSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1016DashHitEffectSystem), BurstRuntime.GetHashCode64<Spell1016DashHitEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashHitEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1016DashHitEffectSystem.__codegen__OnUpdate_00006743_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashHitEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1016DashHitEffectSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1016DashOverheatSystem), BurstRuntime.GetHashCode64<Spell1016DashOverheatSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashOverheatSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1016DashOverheatSystem.__codegen__OnUpdate_00006770_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashOverheatSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1016DashOverheatSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1016DashSystem), BurstRuntime.GetHashCode64<Spell1016DashSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1016DashSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1016DashSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1017DeathAdderEffectSystem), BurstRuntime.GetHashCode64<Spell1017DeathAdderEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1017DeathAdderEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1017DeathAdderSystem), BurstRuntime.GetHashCode64<Spell1017DeathAdderSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1017DeathAdderSystem.__codegen__OnCreate_0000680B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1017DeathAdderSystem.__codegen__OnUpdate_0000680C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1017DeathAdderSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1017DeathAdderTraceSystem), BurstRuntime.GetHashCode64<Spell1017DeathAdderTraceSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderTraceSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderTraceSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1017DeathAdderTraceSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1017DeathAdderTraceSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1018ChainSystem), BurstRuntime.GetHashCode64<Spell1018ChainSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1018ChainSystem.__codegen__OnCreate_00006855_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1018ChainSystem.__codegen__OnUpdate_00006856_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1018ChainSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1018ChainSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1018ThunderAuraSystem), BurstRuntime.GetHashCode64<Spell1018ThunderAuraSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1018ThunderAuraSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1018ThunderAuraSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1018ThunderAuraSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1018ThunderAuraSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1019HighPressureInitializedSystem), BurstRuntime.GetHashCode64<Spell1019HighPressureInitializedSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1019HighPressureInitializedSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1019HighPressureInitializedSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1019HighPressureInitializedSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1019HighPressureInitializedSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1019HighPressureSystem), BurstRuntime.GetHashCode64<Spell1019HighPressureSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1019HighPressureSystem.__codegen__OnCreate_00006917_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1019HighPressureSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1019HighPressureSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1019HighPressureSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1019HighPressureDestroySystem), BurstRuntime.GetHashCode64<Spell1019HighPressureDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1019HighPressureDestroySystem.__codegen__OnCreate_0000699E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1019HighPressureDestroySystem.__codegen__OnUpdate_0000699F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1019HighPressureDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1019HighPressureDestroySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1021MagicBreakerDamageSystem), BurstRuntime.GetHashCode64<Spell1021MagicBreakerDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1021MagicBreakerDamageSystem.__codegen__OnCreate_00006A0A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1021MagicBreakerDamageSystem.__codegen__OnUpdate_00006A0B_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1021MagicBreakerDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1021MagicBreakerDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1021MagicBreakerFallDamageSystem), BurstRuntime.GetHashCode64<Spell1021MagicBreakerFallDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1021MagicBreakerFallDamageSystem.__codegen__OnCreate_00006A39_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1021MagicBreakerFallDamageSystem.__codegen__OnUpdate_00006A3A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1021MagicBreakerFallDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1021MagicBreakerFallDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1022BoomerangDestroySystem), BurstRuntime.GetHashCode64<Spell1022BoomerangDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1022BoomerangDestroySystem.__codegen__OnCreate_00006AED_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1022BoomerangDestroySystem.__codegen__OnUpdate_00006AEE_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1022BoomerangDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1022BoomerangDestroySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1022BoomerangSystem), BurstRuntime.GetHashCode64<Spell1022BoomerangSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1022BoomerangSystem.__codegen__OnCreate_00006B1B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1022BoomerangSystem.__codegen__OnUpdate_00006B1C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1022BoomerangSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1022BoomerangSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1023FallGroundSystem), BurstRuntime.GetHashCode64<Spell1023FallGroundSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1023FallGroundSystem.__codegen__OnCreate_00006B51_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1023FallGroundSystem.__codegen__OnUpdate_00006B52_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1023FallGroundSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1023FallGroundSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1023JudgementBladeFadeOutSystem), BurstRuntime.GetHashCode64<Spell1023JudgementBladeFadeOutSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1023JudgementBladeFadeOutSystem.__codegen__OnCreate_00006B86_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1023JudgementBladeFadeOutSystem.__codegen__OnUpdate_00006B87_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1023JudgementBladeFadeOutSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1023JudgementBladeFadeOutSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1023JudgementBladeSystem), BurstRuntime.GetHashCode64<Spell1023JudgementBladeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1023JudgementBladeSystem.__codegen__OnCreate_00006BBC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1023JudgementBladeSystem.__codegen__OnUpdate_00006BBD_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1023JudgementBladeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1023JudgementBladeSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1024GiantBubbleEffectSystem), BurstRuntime.GetHashCode64<Spell1024GiantBubbleEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1024GiantBubbleEffectSystem.__codegen__OnCreate_00006C0F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1024GiantBubbleEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1024GiantBubbleEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1024GiantBubbleEffectSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1024GiantBubbleSystem), BurstRuntime.GetHashCode64<Spell1024GiantBubbleSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1024GiantBubbleSystem.__codegen__OnCreate_00006C25_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1024GiantBubbleSystem.__codegen__OnUpdate_00006C26_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1024GiantBubbleSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1024GiantBubbleSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1025DragonBreathCreateEffectSystem), BurstRuntime.GetHashCode64<Spell1025DragonBreathCreateEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathCreateEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1025DragonBreathCreateEffectSystem.__codegen__OnUpdate_00006C5D_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathCreateEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1025DragonBreathCreateEffectSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1025DragonBreathDamageSystem), BurstRuntime.GetHashCode64<Spell1025DragonBreathDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1025DragonBreathDamageSystem.__codegen__OnCreate_00006C8B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1025DragonBreathDamageSystem.__codegen__OnUpdate_00006C8C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1025DragonBreathDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1025FireLinePointSystem), BurstRuntime.GetHashCode64<Spell1025FireLinePointSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1025FireLinePointSystem.__codegen__OnCreate_00006CCD_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025FireLinePointSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025FireLinePointSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1025FireLinePointSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1025DragonBreathEffectSystem), BurstRuntime.GetHashCode64<Spell1025DragonBreathEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1025DragonBreathEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1025DragonBreathSystem), BurstRuntime.GetHashCode64<Spell1025DragonBreathSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1025DragonBreathSystem.__codegen__OnCreate_00006DA7_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1025DragonBreathSystem.__codegen__OnUpdate_00006DA8_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1025DragonBreathSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1025DragonBreathSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1026ShiningStarDisableColliderSystem), BurstRuntime.GetHashCode64<Spell1026ShiningStarDisableColliderSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1026ShiningStarDisableColliderSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1026ShiningStarDisableColliderSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1026ShiningStarDisableColliderSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1026ShiningStarDisableColliderSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1026ShiningStarEndChargingSystem), BurstRuntime.GetHashCode64<Spell1026ShiningStarEndChargingSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1026ShiningStarEndChargingSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1026ShiningStarEndChargingSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1026ShiningStarEndChargingSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1026ShiningStarEndChargingSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1026FallEffectSystem), BurstRuntime.GetHashCode64<Spell1026FallEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1026FallEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1026FallEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1026FallEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1026FallEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1027SuperNovaSystem), BurstRuntime.GetHashCode64<Spell1027SuperNovaSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1027SuperNovaSystem.__codegen__OnCreate_00006EB8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1027SuperNovaSystem.__codegen__OnUpdate_00006EB9_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1027SuperNovaSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1027SuperNovaSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1028MrBingArrowCorpseDisableSystem), BurstRuntime.GetHashCode64<Spell1028MrBingArrowCorpseDisableSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			Spell1028MrBingArrowCorpseDisableSystem.__codegen__OnUpdate_00006EED_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1028MrBingArrowCorpseDisableSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1028MrBingArrowCorpseDisableSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1028MrBingArrowHitEffectSystem), BurstRuntime.GetHashCode64<Spell1028MrBingArrowHitEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1028MrBingArrowHitEffectSystem.__codegen__OnCreate_00006F05_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1028MrBingArrowHitEffectSystem.__codegen__OnUpdate_00006F06_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1028MrBingArrowHitEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1028MrBingArrowHitEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1028MrBingArrowSystem), BurstRuntime.GetHashCode64<Spell1028MrBingArrowSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1028MrBingArrowSystem.__codegen__OnCreate_00006F32_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1028MrBingArrowSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1028MrBingArrowSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1028MrBingArrowSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1028MrbingSubArrowEmitterSystem), BurstRuntime.GetHashCode64<Spell1028MrbingSubArrowEmitterSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1028MrbingSubArrowEmitterSystem.__codegen__OnCreate_00006F58_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1028MrbingSubArrowEmitterSystem.__codegen__OnUpdate_00006F59_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1028MrbingSubArrowEmitterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1028MrbingSubArrowEmitterSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1029DimensionTravellerFallDamageSystem), BurstRuntime.GetHashCode64<Spell1029DimensionTravellerFallDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1029DimensionTravellerFallDamageSystem.__codegen__OnCreate_00006F89_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1029DimensionTravellerFallDamageSystem.__codegen__OnUpdate_00006F8A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1029DimensionTravellerFallDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1029DimensionTravellerFallDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1029DimensionTravellerHitEffectSystem), BurstRuntime.GetHashCode64<Spell1029DimensionTravellerHitEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1029DimensionTravellerHitEffectSystem.__codegen__OnCreate_00006FD9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1029DimensionTravellerHitEffectSystem.__codegen__OnUpdate_00006FDA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1029DimensionTravellerHitEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1029DimensionTravellerHitEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1031DaveShotgunDestroySystem), BurstRuntime.GetHashCode64<Spell1031DaveShotgunDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell1031DaveShotgunDestroySystem.__codegen__OnCreate_0000701D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell1031DaveShotgunDestroySystem.__codegen__OnUpdate_0000701E_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1031DaveShotgunDestroySystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1031DaveShotgunHitCountSystem), BurstRuntime.GetHashCode64<Spell1031DaveShotgunHitCountSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunHitCountSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunHitCountSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunHitCountSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1031DaveShotgunHitCountSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell1031DaveShotgunSystem), BurstRuntime.GetHashCode64<Spell1031DaveShotgunSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell1031DaveShotgunSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell1031DaveShotgunSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2001BoBoSystem), BurstRuntime.GetHashCode64<Spell2001BoBoSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2001BoBoSystem.__codegen__OnCreate_000070A8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell2001BoBoSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2001BoBoSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2001BoBoSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2002SetDamageByWandMpSystem), BurstRuntime.GetHashCode64<Spell2002SetDamageByWandMpSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell2002SetDamageByWandMpSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell2002SetDamageByWandMpSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2002SetDamageByWandMpSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2002SetDamageByWandMpSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2002System), BurstRuntime.GetHashCode64<Spell2002System>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2002System.__codegen__OnCreate_0000716B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell2002System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2002System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2002System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2003SummonSystem), BurstRuntime.GetHashCode64<Spell2003SummonSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2003SummonSystem.__codegen__OnCreate_00007206_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2003SummonSystem.__codegen__OnUpdate_00007207_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2003SummonSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2003SummonSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2003SplitTentacleShooterSystem), BurstRuntime.GetHashCode64<Spell2003SplitTentacleShooterSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2003SplitTentacleShooterSystem.__codegen__OnCreate_00007245_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2003SplitTentacleShooterSystem.__codegen__OnUpdate_00007246_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2003SplitTentacleShooterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2003SplitTentacleShooterSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2003InvisibleTentacleShooterSystem), BurstRuntime.GetHashCode64<Spell2003InvisibleTentacleShooterSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell2003InvisibleTentacleShooterSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell2003InvisibleTentacleShooterSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2003InvisibleTentacleShooterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2003InvisibleTentacleShooterSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2004PillarOfLightTriggerSystem), BurstRuntime.GetHashCode64<Spell2004PillarOfLightTriggerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2004PillarOfLightTriggerSystem.__codegen__OnCreate_0000727D_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2004PillarOfLightTriggerSystem.__codegen__OnUpdate_0000727E_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2004PillarOfLightTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2004PillarOfLightTriggerSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2005System), BurstRuntime.GetHashCode64<Spell2005System>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2005System.__codegen__OnCreate_00007354_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2005System.__codegen__OnUpdate_00007355_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2005System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2005System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2007SuicideBugNestSystem), BurstRuntime.GetHashCode64<Spell2007SuicideBugNestSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugNestSystem.__codegen__OnCreate_00007418_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugNestSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugNestSystem.__codegen__OnDestroy_0000741A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugNestSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2007SuicideBugNestSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2007SuicideBugNestDeadSystem), BurstRuntime.GetHashCode64<Spell2007SuicideBugNestDeadSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugNestDeadSystem.__codegen__OnCreate_00007454_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugNestDeadSystem.__codegen__OnUpdate_00007455_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugNestDeadSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2007SuicideBugNestDeadSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2007SuicideBugNestBeHurtSystem), BurstRuntime.GetHashCode64<Spell2007SuicideBugNestBeHurtSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugNestBeHurtSystem.__codegen__OnCreate_00007482_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugNestBeHurtSystem.__codegen__OnUpdate_00007483_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugNestBeHurtSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2007SuicideBugNestBeHurtSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2007SuicideBugDestroySystem), BurstRuntime.GetHashCode64<Spell2007SuicideBugDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugDestroySystem.__codegen__OnCreate_000074AF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugDestroySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2007SuicideBugDestroySystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell2007SuicideBugSystem), BurstRuntime.GetHashCode64<Spell2007SuicideBugSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugSystem.__codegen__OnCreate_000074C5_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell2007SuicideBugSystem.__codegen__OnUpdate_000074C6_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell2007SuicideBugSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell2007SuicideBugSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3007LightningChainAttackSystem), BurstRuntime.GetHashCode64<Spell3007LightningChainAttackSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell3007LightningChainAttackSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell3007LightningChainAttackSystem.__codegen__OnUpdate_00007509_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3007LightningChainAttackSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3007LightningChainAttackSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3015WormSystem), BurstRuntime.GetHashCode64<Spell3015WormSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell3015WormSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3015WormSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3015WormSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3015WormSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3015WormSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3110LifeLineSystem), BurstRuntime.GetHashCode64<Spell3110LifeLineSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell3110LifeLineSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3110LifeLineSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3110LifeLineSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3110LifeLineSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3110LifeLineSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3110LivingTiePositionSyncSystem), BurstRuntime.GetHashCode64<Spell3110LivingTiePositionSyncSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell3110LivingTiePositionSyncSystem.__codegen__OnCreate_000075A7_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell3110LivingTiePositionSyncSystem.__codegen__OnUpdate_000075A8_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3110LivingTiePositionSyncSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3110LivingTiePositionSyncSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3110LivingTieSystem), BurstRuntime.GetHashCode64<Spell3110LivingTieSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell3110LivingTieSystem.__codegen__OnCreate_000075D5_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell3110LivingTieSystem.__codegen__OnUpdate_000075D6_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3110LivingTieSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3110LivingTieSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3118SelfSacrificeSystem), BurstRuntime.GetHashCode64<Spell3118SelfSacrificeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell3118SelfSacrificeSystem.__codegen__OnCreate_0000760B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3118SelfSacrificeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3118SelfSacrificeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3118SelfSacrificeSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellRecheckRefractSystem), BurstRuntime.GetHashCode64<SpellRecheckRefractSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpellRecheckRefractSystem.__codegen__OnCreate_00007613_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellRecheckRefractSystem.__codegen__OnUpdate_00007614_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellRecheckRefractSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellRecheckRefractSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3127SoulMateSystem), BurstRuntime.GetHashCode64<Spell3127SoulMateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell3127SoulMateSystem.__codegen__OnCreate_0000761E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3127SoulMateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3127SoulMateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3127SoulMateSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3129VoidExplosionSystem), BurstRuntime.GetHashCode64<Spell3129VoidExplosionSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell3129VoidExplosionSystem.__codegen__OnCreate_00007633_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell3129VoidExplosionSystem.__codegen__OnUpdate_00007634_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3129VoidExplosionSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3129VoidExplosionSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3129VoidExplosionDamageSystem), BurstRuntime.GetHashCode64<Spell3129VoidExplosionDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell3129VoidExplosionDamageSystem.__codegen__OnCreate_0000763B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell3129VoidExplosionDamageSystem.__codegen__OnUpdate_0000763C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3129VoidExplosionDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3129VoidExplosionDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3129VoidTrailSystem), BurstRuntime.GetHashCode64<Spell3129VoidTrailSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell3129VoidTrailSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3129VoidTrailSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3129VoidTrailSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3129VoidTrailSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4013RuneHammerDamageSystem), BurstRuntime.GetHashCode64<Spell4013RuneHammerDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4013RuneHammerDamageSystem.__codegen__OnCreate_000076D6_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4013RuneHammerDamageSystem.__codegen__OnUpdate_000076D7_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4013RuneHammerDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4013RuneHammerDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4013MoveSystem), BurstRuntime.GetHashCode64<Spell4013MoveSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4013MoveSystem.__codegen__OnCreate_00007730_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4013MoveSystem.__codegen__OnUpdate_00007731_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4013MoveSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4013MoveSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4014CrystalChangeSystem), BurstRuntime.GetHashCode64<Spell4014CrystalChangeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4014CrystalChangeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014CrystalChangeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014CrystalChangeSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014CrystalChangeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4014CrystalChangeSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4014LaserCrystalSystem), BurstRuntime.GetHashCode64<Spell4014LaserCrystalSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4014LaserCrystalSystem.__codegen__OnCreate_00007807_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014LaserCrystalSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4014LaserCrystalSystem.__codegen__OnDestroy_00007809_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014LaserCrystalSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4014LaserCrystalSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4014LaserDamageApplySystem), BurstRuntime.GetHashCode64<Spell4014LaserDamageApplySystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4014LaserDamageApplySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014LaserDamageApplySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4014LaserDamageApplySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4014LaserDamageApplySystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4019BiAnBladeFallSystem), BurstRuntime.GetHashCode64<Spell4019BiAnBladeFallSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4019BiAnBladeFallSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4019BiAnBladeFallSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4019BiAnBladeFallSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4019BiAnBladeFallSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4019BiAnBladeSystem), BurstRuntime.GetHashCode64<Spell4019BiAnBladeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4019BiAnBladeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4019BiAnBladeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4019BiAnBladeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4019BiAnBladeSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4024DaveHarpoonCatchSystem), BurstRuntime.GetHashCode64<Spell4024DaveHarpoonCatchSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4024DaveHarpoonCatchSystem.__codegen__OnCreate_00007982_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonCatchSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonCatchSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4024DaveHarpoonCatchSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4024DaveHarpoonSystem), BurstRuntime.GetHashCode64<Spell4024DaveHarpoonSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4024DaveHarpoonSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4024DaveHarpoonThunderRelicEffectSystem), BurstRuntime.GetHashCode64<Spell4024DaveHarpoonThunderRelicEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonThunderRelicEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonThunderRelicEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonThunderRelicEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4024DaveHarpoonThunderRelicEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4024DaveHarpoonThunderRelicSystem), BurstRuntime.GetHashCode64<Spell4024DaveHarpoonThunderRelicSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4024DaveHarpoonThunderRelicSystem.__codegen__OnCreate_00007AD2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4024DaveHarpoonThunderRelicSystem.__codegen__OnUpdate_00007AD3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4024DaveHarpoonThunderRelicSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4024DaveHarpoonThunderRelicSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4025RedRuneTriggerSystem), BurstRuntime.GetHashCode64<Spell4025RedRuneTriggerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4025RedRuneTriggerSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4025RedRuneTriggerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4025RedRuneTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4025RedRuneTriggerSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4025System), BurstRuntime.GetHashCode64<Spell4025System>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4025System.__codegen__OnCreate_00007B0E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4025System.__codegen__OnUpdate_00007B0F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4025System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4025System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4025FallEffectSystem), BurstRuntime.GetHashCode64<Spell4025FallEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4025FallEffectSystem.__codegen__OnCreate_00007B3C_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4025FallEffectSystem.__codegen__OnUpdate_00007B3D_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4025FallEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4025FallEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4026RecordSystem), BurstRuntime.GetHashCode64<Spell4026RecordSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4026RecordSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4026RecordSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4026RecordSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4026RecordSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4026RecordSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4026System), BurstRuntime.GetHashCode64<Spell4026System>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4026System.__codegen__OnCreate_00007B93_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4026System.__codegen__OnUpdate_00007B94_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4026System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4026System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4026FallEffectSystem), BurstRuntime.GetHashCode64<Spell4026FallEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell4026FallEffectSystem.__codegen__OnCreate_00007BCE_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell4026FallEffectSystem.__codegen__OnUpdate_00007BCF_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4026FallEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4026FallEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4027ProcessSystem), BurstRuntime.GetHashCode64<Spell4027ProcessSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4027ProcessSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027ProcessSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027ProcessSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4027ProcessSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4027ActiveColliderSystem), BurstRuntime.GetHashCode64<Spell4027ActiveColliderSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4027ActiveColliderSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027ActiveColliderSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027ActiveColliderSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4027ActiveColliderSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4027BackMpSystem), BurstRuntime.GetHashCode64<Spell4027BackMpSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4027BackMpSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027BackMpSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027BackMpSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4027BackMpSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell4027UnitTakeDamageUpSystem), BurstRuntime.GetHashCode64<Spell4027UnitTakeDamageUpSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell4027UnitTakeDamageUpSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027UnitTakeDamageUpSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell4027UnitTakeDamageUpSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell4027UnitTakeDamageUpSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9002BoBoBombSystem), BurstRuntime.GetHashCode64<Spell9002BoBoBombSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BoBoBombSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BoBoBombSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BoBoBombSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9002BoBoBombSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9002BounceBoneEffectSystem), BurstRuntime.GetHashCode64<Spell9002BounceBoneEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BounceBoneEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BounceBoneEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BounceBoneEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9002BounceBoneEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9002BounceBoneSystem), BurstRuntime.GetHashCode64<Spell9002BounceBoneSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9002BounceBoneSystem.__codegen__OnCreate_00007C9E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BounceBoneSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9002BounceBoneSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9002BounceBoneSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9003LongTrailEffectSystem), BurstRuntime.GetHashCode64<Spell9003LongTrailEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9003LongTrailEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9003LongTrailEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9003LongTrailEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9003LongTrailEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9003LongTrailSystem), BurstRuntime.GetHashCode64<Spell9003LongTrailSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9003LongTrailSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9003LongTrailSystem.__codegen__OnUpdate_00007D18_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9003LongTrailSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9003LongTrailSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9004SoundWaveEffectSystem), BurstRuntime.GetHashCode64<Spell9004SoundWaveEffectSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9004SoundWaveEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9004SoundWaveEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9004SoundWaveEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9004SoundWaveSystem), BurstRuntime.GetHashCode64<Spell9004SoundWaveSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9004SoundWaveSystem.__codegen__OnCreate_00007D5E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9004SoundWaveSystem.__codegen__OnUpdate_00007D5F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9004SoundWaveSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9004SoundWaveSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9004SoundWoundHitEffectScaleSystem), BurstRuntime.GetHashCode64<Spell9004SoundWoundHitEffectScaleSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9004SoundWoundHitEffectScaleSystem.__codegen__OnCreate_00007D8F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9004SoundWoundHitEffectScaleSystem.__codegen__OnUpdate_00007D90_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9004SoundWoundHitEffectScaleSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9004SoundWoundHitEffectScaleSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9006TrickLongTrailEffectSystem), BurstRuntime.GetHashCode64<Spell9006TrickLongTrailEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9006TrickLongTrailEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9006TrickLongTrailEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9006TrickLongTrailEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9006TrickLongTrailEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9008SinWaveSpeedEffectSystem), BurstRuntime.GetHashCode64<Spell9008SinWaveSpeedEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9008SinWaveSpeedEffectSystem.__codegen__OnCreate_00007DD8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9008SinWaveSpeedEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9008SinWaveSpeedEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9008SinWaveSpeedEffectSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9008SingWaveSpeedSystem), BurstRuntime.GetHashCode64<Spell9008SingWaveSpeedSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9008SingWaveSpeedSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9008SingWaveSpeedSystem.__codegen__OnUpdate_00007DEF_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9008SingWaveSpeedSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9008SingWaveSpeedSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9011RotateArrowEffectSystem), BurstRuntime.GetHashCode64<Spell9011RotateArrowEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9011RotateArrowEffectSystem.__codegen__OnCreate_00007E1F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9011RotateArrowEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9011RotateArrowEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9011RotateArrowEffectSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9011RotateArrowSystem), BurstRuntime.GetHashCode64<Spell9011RotateArrowSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9011RotateArrowSystem.__codegen__OnCreate_00007E35_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9011RotateArrowSystem.__codegen__OnUpdate_00007E36_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9011RotateArrowSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9011RotateArrowSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9014SpearEffectSystem), BurstRuntime.GetHashCode64<Spell9014SpearEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9014SpearEffectSystem.__codegen__OnCreate_00007E65_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9014SpearEffectSystem.__codegen__OnUpdate_00007E66_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9014SpearEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9014SpearEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9014SpearSystem), BurstRuntime.GetHashCode64<Spell9014SpearSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9014SpearSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9014SpearSystem.__codegen__OnUpdate_00007E7C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9014SpearSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9014SpearSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9015IceBallSystem), BurstRuntime.GetHashCode64<Spell9015IceBallSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9015IceBallSystem.__codegen__OnCreate_00007EAD_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9015IceBallSystem.__codegen__OnUpdate_00007EAE_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9015IceBallSystem.__codegen__OnDestroy_00007EAF_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9015IceBallSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9015IceBallSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9017Chapter3BulletEffectSystem), BurstRuntime.GetHashCode64<Spell9017Chapter3BulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9017Chapter3BulletEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9017Chapter3BulletEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9017Chapter3BulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9017Chapter3BulletEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9020WallTRoughBulletEffectSystem), BurstRuntime.GetHashCode64<Spell9020WallTRoughBulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9020WallTRoughBulletEffectSystem.__codegen__OnCreate_00007EF5_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9020WallTRoughBulletEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9020WallTRoughBulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9020WallTRoughBulletEffectSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9021SlowDownBulletSystem), BurstRuntime.GetHashCode64<Spell9021SlowDownBulletSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9021SlowDownBulletSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9021SlowDownBulletSystem.__codegen__OnUpdate_00007F26_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9021SlowDownBulletSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9021SlowDownBulletSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9022FlowerBulletEffectSystem), BurstRuntime.GetHashCode64<Spell9022FlowerBulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9022FlowerBulletEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9022FlowerBulletEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9022FlowerBulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9022FlowerBulletEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9024RotateOutSystem), BurstRuntime.GetHashCode64<Spell9024RotateOutSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9024RotateOutSystem.__codegen__OnCreate_00007F6F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9024RotateOutSystem.__codegen__OnUpdate_00007F70_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9024RotateOutSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9024RotateOutSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9026Elite15EffectSystem), BurstRuntime.GetHashCode64<Spell9026Elite15EffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9026Elite15EffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9026Elite15EffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9026Elite15EffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9026Elite15EffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9027Elite14BulletEffectSystem), BurstRuntime.GetHashCode64<Spell9027Elite14BulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9027Elite14BulletEffectSystem.__codegen__OnCreate_00007FB8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9027Elite14BulletEffectSystem.__codegen__OnUpdate_00007FB9_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9027Elite14BulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9027Elite14BulletEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9027Elite14BulletSystem), BurstRuntime.GetHashCode64<Spell9027Elite14BulletSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9027Elite14BulletSystem.__codegen__OnCreate_00007FCE_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9027Elite14BulletSystem.__codegen__OnUpdate_00007FCF_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9027Elite14BulletSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9027Elite14BulletSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9028EnterTheGungeonBulletEffectSystem), BurstRuntime.GetHashCode64<Spell9028EnterTheGungeonBulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9028EnterTheGungeonBulletEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9028EnterTheGungeonBulletEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9028EnterTheGungeonBulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9028EnterTheGungeonBulletEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9038WaterBollEffectSystem), BurstRuntime.GetHashCode64<Spell9038WaterBollEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9038WaterBollEffectSystem.__codegen__OnCreate_00008017_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9038WaterBollEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9038WaterBollEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9038WaterBollEffectSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9039SquidBulletEffectSystem), BurstRuntime.GetHashCode64<Spell9039SquidBulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9039SquidBulletEffectSystem.__codegen__OnCreate_0000802F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9039SquidBulletEffectSystem.__codegen__OnUpdate_00008030_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9039SquidBulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9039SquidBulletEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9039SquidBulletSystem), BurstRuntime.GetHashCode64<Spell9039SquidBulletSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9039SquidBulletSystem.__codegen__OnCreate_00008045_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9039SquidBulletSystem.__codegen__OnUpdate_00008046_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9039SquidBulletSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9039SquidBulletSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9043GrabageBulletEffectSystem), BurstRuntime.GetHashCode64<Spell9043GrabageBulletEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Spell9043GrabageBulletEffectSystem.__codegen__OnCreate_00008076_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell9043GrabageBulletEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9043GrabageBulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9043GrabageBulletEffectSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9044SlowDownBullet2System), BurstRuntime.GetHashCode64<Spell9044SlowDownBullet2System>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9044SlowDownBullet2System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9044SlowDownBullet2System.__codegen__OnUpdate_00008092_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9044SlowDownBullet2System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9044SlowDownBullet2System", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell9045RotateOut2System), BurstRuntime.GetHashCode64<Spell9045RotateOut2System>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell9045RotateOut2System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Spell9045RotateOut2System.__codegen__OnUpdate_000080C3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell9045RotateOut2System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell9045RotateOut2System", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SelfPeriodRandomRotateSystem), BurstRuntime.GetHashCode64<SelfPeriodRandomRotateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SelfPeriodRandomRotateSystem.__codegen__OnCreate_000081C9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SelfPeriodRandomRotateSystem.__codegen__OnUpdate_000081CA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SelfPeriodRandomRotateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SelfPeriodRandomRotateSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SelfScaleRepeatChangeSystem), BurstRuntime.GetHashCode64<SelfScaleRepeatChangeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SelfScaleRepeatChangeSystem.__codegen__OnCreate_000081F7_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SelfScaleRepeatChangeSystem.__codegen__OnUpdate_000081F8_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SelfScaleRepeatChangeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SelfScaleRepeatChangeSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SelfScaleShakeSystem), BurstRuntime.GetHashCode64<SelfScaleShakeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SelfScaleShakeSystem.__codegen__OnCreate_00008225_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SelfScaleShakeSystem.__codegen__OnUpdate_00008226_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SelfScaleShakeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SelfScaleShakeSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellChargeSystem), BurstRuntime.GetHashCode64<SpellChargeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellChargeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellChargeSystem.__codegen__OnUpdate_00008253_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellChargeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellChargeSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellCleanupSystem), BurstRuntime.GetHashCode64<SpellCleanupSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellCleanupSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellCleanupSystem.__codegen__OnUpdate_0000827B_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellCleanupSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellCleanupSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellDestroySystem), BurstRuntime.GetHashCode64<SpellDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellDestroySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellDestroySystem.__codegen__OnUpdate_000082A0_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellDestroySystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellEffectTagSystem), BurstRuntime.GetHashCode64<SpellEffectTagSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			SpellEffectTagSystem.__codegen__OnUpdate_000082F2_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellEffectTagSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellEffectTagSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellFallDamageSystem), BurstRuntime.GetHashCode64<SpellFallDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellFallDamageSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellFallDamageSystem.__codegen__OnUpdate_00008345_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellFallDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellFallDamageSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellHalfLifeTeleportSystem), BurstRuntime.GetHashCode64<SpellHalfLifeTeleportSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpellHalfLifeTeleportSystem.__codegen__OnCreate_00008371_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellHalfLifeTeleportSystem.__codegen__OnUpdate_00008372_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellHalfLifeTeleportSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellHalfLifeTeleportSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellHoverDamageSystem), BurstRuntime.GetHashCode64<SpellHoverDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpellHoverDamageSystem.__codegen__OnCreate_0000839B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellHoverDamageSystem.__codegen__OnUpdate_0000839C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellHoverDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellHoverDamageSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellKeepCastingAttachSystem), BurstRuntime.GetHashCode64<SpellKeepCastingAttachSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpellKeepCastingAttachSystem.__codegen__OnCreate_000083C9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpellKeepCastingAttachSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellKeepCastingAttachSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellKeepCastingAttachSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellLayerCorrectSystem), BurstRuntime.GetHashCode64<SpellLayerCorrectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellLayerCorrectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellLayerCorrectSystem.__codegen__OnUpdate_000083F8_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellLayerCorrectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellLayerCorrectSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellMoveSystem), BurstRuntime.GetHashCode64<SpellMoveSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpellMoveSystem.__codegen__OnCreate_00008425_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellMoveSystem.__codegen__OnUpdate_00008426_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellMoveSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellMoveSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellParabolaSystem), BurstRuntime.GetHashCode64<SpellParabolaSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellParabolaSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpellParabolaSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellParabolaSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellParabolaSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellRemoveDuplicatesHitEntityBufferSystem), BurstRuntime.GetHashCode64<SpellRemoveDuplicatesHitEntityBufferSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			((SpellRemoveDuplicatesHitEntityBufferSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellRemoveDuplicatesHitEntityBufferSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellRemoveDuplicatesHitEntityBufferSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellResizeSystem), BurstRuntime.GetHashCode64<SpellResizeSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellResizeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			SpellResizeSystem.__codegen__OnUpdate_00008519_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellResizeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellResizeSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellShootSystem), BurstRuntime.GetHashCode64<SpellShootSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellShootSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpellShootSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellShootSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellShootSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellSystem), BurstRuntime.GetHashCode64<SpellSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpellSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellTakeDamageResultSystem), BurstRuntime.GetHashCode64<SpellTakeDamageResultSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((SpellTakeDamageResultSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpellTakeDamageResultSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellTakeDamageResultSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellTakeDamageResultSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(SpellTransparentSystem), BurstRuntime.GetHashCode64<SpellTransparentSystem>(), delegate(IntPtr self, IntPtr state)
		{
			SpellTransparentSystem.__codegen__OnCreate_000085CC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((SpellTransparentSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((SpellTransparentSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "SpellTransparentSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EntityAutoRotateSystem), BurstRuntime.GetHashCode64<EntityAutoRotateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			EntityAutoRotateSystem.__codegen__OnCreate_0000866B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			EntityAutoRotateSystem.__codegen__OnUpdate_0000866C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EntityAutoRotateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EntityAutoRotateSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(RandomRotateSystem), BurstRuntime.GetHashCode64<RandomRotateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			RandomRotateSystem.__codegen__OnCreate_00008699_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			RandomRotateSystem.__codegen__OnUpdate_0000869A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((RandomRotateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "RandomRotateSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(CurrentRoomEntitiesSingletonSystem), BurstRuntime.GetHashCode64<CurrentRoomEntitiesSingletonSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "CurrentRoomEntitiesSingletonSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Destructible1_T3System), BurstRuntime.GetHashCode64<Destructible1_T3System>(), delegate(IntPtr self, IntPtr state)
		{
			Destructible1_T3System.__codegen__OnCreate_000086EE_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Destructible1_T3System.__codegen__OnUpdate_000086EF_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Destructible1_T3System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Destructible1_T3System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Destructible4_T3System), BurstRuntime.GetHashCode64<Destructible4_T3System>(), delegate(IntPtr self, IntPtr state)
		{
			Destructible4_T3System.__codegen__OnCreate_0000871F_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Destructible4_T3System.__codegen__OnUpdate_00008720_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Destructible4_T3System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Destructible4_T3System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(DotsUnitTriggerSystem), BurstRuntime.GetHashCode64<DotsUnitTriggerSystem>(), delegate(IntPtr self, IntPtr state)
		{
			DotsUnitTriggerSystem.__codegen__OnCreate_00008752_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			DotsUnitTriggerSystem.__codegen__OnUpdate_00008753_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			DotsUnitTriggerSystem.__codegen__OnDestroy_00008754_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((DotsUnitTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "DotsUnitTriggerSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EndlessMonsterDeadSystem), BurstRuntime.GetHashCode64<EndlessMonsterDeadSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((EndlessMonsterDeadSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessMonsterDeadSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessMonsterDeadSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EndlessMonsterDeadSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EndlessMonsterDeadSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(EndlessSpawnEffectSystem), BurstRuntime.GetHashCode64<EndlessSpawnEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((EndlessSpawnEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessSpawnEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((EndlessSpawnEffectSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((EndlessSpawnEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "EndlessSpawnEffectSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster16System), BurstRuntime.GetHashCode64<Monster16System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster16System.__codegen__OnCreate_000087EB_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster16System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster16System.__codegen__OnDestroy_000087ED_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster16System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster16System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster1System), BurstRuntime.GetHashCode64<Monster1System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster1System.__codegen__OnCreate_00008805_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster1System.__codegen__OnUpdate_00008806_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster1System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster1System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster21System), BurstRuntime.GetHashCode64<Monster21System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster21System.__codegen__OnCreate_00008836_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster21System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster21System.__codegen__OnDestroy_00008838_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster21System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster21System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster23System), BurstRuntime.GetHashCode64<Monster23System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster23System.__codegen__OnCreate_0000888E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster23System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster23System.__codegen__OnDestroy_00008890_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster23System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster23System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster301System), BurstRuntime.GetHashCode64<Monster301System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster301System.__codegen__OnCreate_000088AC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster301System.__codegen__OnUpdate_000088AD_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster301System.__codegen__OnDestroy_000088AE_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster301System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster301System", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster302System), BurstRuntime.GetHashCode64<Monster302System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster302System.__codegen__OnCreate_000088E1_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster302System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster302System.__codegen__OnDestroy_000088E3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster302System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster302System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster303System), BurstRuntime.GetHashCode64<Monster303System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster303System.__codegen__OnCreate_00008916_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster303System.__codegen__OnUpdate_00008917_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster303System.__codegen__OnDestroy_00008918_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster303System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster303System", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster304System), BurstRuntime.GetHashCode64<Monster304System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster304System.__codegen__OnCreate_00008949_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster304System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster304System.__codegen__OnDestroy_0000894B_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster304System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster304System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster305System), BurstRuntime.GetHashCode64<Monster305System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster305System.__codegen__OnCreate_0000897E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster305System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster305System.__codegen__OnDestroy_00008980_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster305System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster305System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster306System), BurstRuntime.GetHashCode64<Monster306System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster306System.__codegen__OnCreate_000089B3_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster306System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster306System.__codegen__OnDestroy_000089B5_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster306System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster306System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster308System), BurstRuntime.GetHashCode64<Monster308System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster308System.__codegen__OnCreate_000089E8_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster308System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster308System.__codegen__OnDestroy_000089EA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster308System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster308System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster308DashSystem), BurstRuntime.GetHashCode64<Monster308DashSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Monster308DashSystem.__codegen__OnCreate_00008A25_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster308DashSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster308DashSystem.__codegen__OnDestroy_00008A27_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster308DashSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster308DashSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster309System), BurstRuntime.GetHashCode64<Monster309System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster309System.__codegen__OnCreate_00008A5A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster309System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster309System.__codegen__OnDestroy_00008A5C_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster309System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster309System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster310System), BurstRuntime.GetHashCode64<Monster310System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster310System.__codegen__OnCreate_00008A8E_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster310System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster310System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster310System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster311System), BurstRuntime.GetHashCode64<Monster311System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster311System.__codegen__OnCreate_00008AC2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster311System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster311System.__codegen__OnDestroy_00008AC4_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster311System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster311System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster312System), BurstRuntime.GetHashCode64<Monster312System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster312System.__codegen__OnCreate_00008AF6_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster312System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster312System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster312System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster313System), BurstRuntime.GetHashCode64<Monster313System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster313System.__codegen__OnCreate_00008B28_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster313System.__codegen__OnUpdate_00008B29_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster313System.__codegen__OnDestroy_00008B2A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster313System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster313System", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster313DeadSystem), BurstRuntime.GetHashCode64<Monster313DeadSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Monster313DeadSystem.__codegen__OnCreate_00008B57_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster313DeadSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster313DeadSystem.__codegen__OnDestroy_00008B59_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster313DeadSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster313DeadSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster314System), BurstRuntime.GetHashCode64<Monster314System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster314System.__codegen__OnCreate_00008B74_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster314System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster314System.__codegen__OnDestroy_00008B76_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster314System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster314System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster315System), BurstRuntime.GetHashCode64<Monster315System>(), delegate(IntPtr self, IntPtr state)
		{
			((Monster315System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster315System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster315System.__codegen__OnDestroy_00008BBC_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster315System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster315System", 4);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster315ShieldSystem), BurstRuntime.GetHashCode64<Monster315ShieldSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Monster315ShieldSystem.__codegen__OnCreate_00008BEB_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster315ShieldSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster315ShieldSystem.__codegen__OnDestroy_00008BED_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster315ShieldSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster315ShieldSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster315ShieldEffectSystem), BurstRuntime.GetHashCode64<Monster315ShieldEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Monster315ShieldEffectSystem.__codegen__OnCreate_00008C1B_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster315ShieldEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster315ShieldEffectSystem.__codegen__OnDestroy_00008C1D_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster315ShieldEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster315ShieldEffectSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster316BuffSystem), BurstRuntime.GetHashCode64<Monster316BuffSystem>(), delegate(IntPtr self, IntPtr state)
		{
			Monster316BuffSystem.__codegen__OnCreate_00008C53_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster316BuffSystem.__codegen__OnUpdate_00008C54_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster316BuffSystem.__codegen__OnDestroy_00008C55_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster316BuffSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster316BuffSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster316System), BurstRuntime.GetHashCode64<Monster316System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster316System.__codegen__OnCreate_00008C82_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster316System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster316System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster316System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster317System), BurstRuntime.GetHashCode64<Monster317System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster317System.__codegen__OnCreate_00008CD0_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster317System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster317System.__codegen__OnDestroy_00008CD2_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster317System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster317System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster319System), BurstRuntime.GetHashCode64<Monster319System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster319System.__codegen__OnCreate_00008D05_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster319System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster319System.__codegen__OnDestroy_00008D07_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster319System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster319System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster320System), BurstRuntime.GetHashCode64<Monster320System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster320System.__codegen__OnCreate_00008D38_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster320System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster320System.__codegen__OnDestroy_00008D3A_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster320System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster320System", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster321System), BurstRuntime.GetHashCode64<Monster321System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster321System.__codegen__OnCreate_00008D6A_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster321System.__codegen__OnUpdate_00008D6B_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster321System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster321System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster321DeadEventSystem), BurstRuntime.GetHashCode64<Monster321DeadEventSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			Monster321DeadEventSystem.__codegen__OnUpdate_00008D96_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster321DeadEventSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster321DeadEventSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster321ExplosionStateSystem), BurstRuntime.GetHashCode64<Monster321ExplosionStateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Monster321ExplosionStateSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster321ExplosionStateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster321ExplosionStateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster321ExplosionStateSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster322System), BurstRuntime.GetHashCode64<Monster322System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster322System.__codegen__OnCreate_00008DA9_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster322System.__codegen__OnUpdate_00008DAA_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster322System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster322System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster325System), BurstRuntime.GetHashCode64<Monster325System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster325System.__codegen__OnCreate_00008DDC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster325System.__codegen__OnUpdate_00008DDD_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster325System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster325System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster326System), BurstRuntime.GetHashCode64<Monster326System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster326System.__codegen__OnCreate_00008E11_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			Monster326System.__codegen__OnUpdate_00008E12_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster326System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster326System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster327System), BurstRuntime.GetHashCode64<Monster327System>(), delegate(IntPtr self, IntPtr state)
		{
			((Monster327System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster327System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster327System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster327System", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster327_MissileSystem), BurstRuntime.GetHashCode64<Monster327_MissileSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Monster327_MissileSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster327_MissileSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster327_MissileSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster327_MissileSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster5System), BurstRuntime.GetHashCode64<Monster5System>(), delegate(IntPtr self, IntPtr state)
		{
			Monster5System.__codegen__OnCreate_00008EFC_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster5System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster5System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster5System", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster7System), BurstRuntime.GetHashCode64<Monster7System>(), delegate(IntPtr self, IntPtr state)
		{
			((Monster7System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Monster7System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Monster7System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Monster7System", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Monster8System), BurstRuntime.GetHashCode64<Monster8System>(), Monster8System.__codegen__OnCreate, Monster8System.__codegen__OnUpdate, null, null, null, null, "Monster8System", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(NPCSystem), BurstRuntime.GetHashCode64<NPCSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			((NPCSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((NPCSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "NPCSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(PathFindingSystem), BurstRuntime.GetHashCode64<PathFindingSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((PathFindingSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((PathFindingSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((PathFindingSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "PathFindingSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TakeDamageInfoPreProcessSystem), BurstRuntime.GetHashCode64<TakeDamageInfoPreProcessSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TakeDamageInfoPreProcessSystem.__codegen__OnCreate_00009012_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((TakeDamageInfoPreProcessSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TakeDamageInfoPreProcessSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TakeDamageInfoPreProcessSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateCounterSystem), BurstRuntime.GetHashCode64<TeammateCounterSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateCounterSystem.__codegen__OnCreate_00009040_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((TeammateCounterSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateCounterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateCounterSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem), BurstRuntime.GetHashCode64<TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem>(), delegate(IntPtr self, IntPtr state)
		{
			((TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(Spell3127EffectDestroySystem), BurstRuntime.GetHashCode64<Spell3127EffectDestroySystem>(), delegate(IntPtr self, IntPtr state)
		{
			((Spell3127EffectDestroySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((Spell3127EffectDestroySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((Spell3127EffectDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Spell3127EffectDestroySystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateBeforeDeadEventSystem), BurstRuntime.GetHashCode64<TeammateBeforeDeadEventSystem>(), null, TeammateBeforeDeadEventSystem.__codegen__OnUpdate, null, null, null, null, "TeammateBeforeDeadEventSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateDeadEventSystem), BurstRuntime.GetHashCode64<TeammateDeadEventSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((TeammateDeadEventSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateDeadEventSystem.__codegen__OnUpdate_00009094_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateDeadEventSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateDeadEventSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateDelayDeathSystem), BurstRuntime.GetHashCode64<TeammateDelayDeathSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateDelayDeathSystem.__codegen__OnCreate_000090CF_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateDelayDeathSystem.__codegen__OnUpdate_000090D0_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateDelayDeathSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateDelayDeathSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateFuseShineEffectProcessSystem), BurstRuntime.GetHashCode64<TeammateFuseShineEffectProcessSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateFuseShineEffectProcessSystem.__codegen__OnCreate_000090FD_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateFuseShineEffectProcessSystem.__codegen__OnUpdate_000090FE_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateFuseShineEffectProcessSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateFuseShineEffectProcessSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateFuseSystem), BurstRuntime.GetHashCode64<TeammateFuseSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateFuseSystem.__codegen__OnCreate_00009134_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateFuseSystem.__codegen__OnUpdate_00009135_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateFuseSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateFuseSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateUpdateGhostEffectSystem), BurstRuntime.GetHashCode64<TeammateUpdateGhostEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((TeammateUpdateGhostEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateUpdateGhostEffectSystem.__codegen__OnUpdate_00009145_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateUpdateGhostEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateUpdateGhostEffectSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateGhostFireVisualEffectSystem), BurstRuntime.GetHashCode64<TeammateGhostFireVisualEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateGhostFireVisualEffectSystem.__codegen__OnCreate_00009172_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateGhostFireVisualEffectSystem.__codegen__OnUpdate_00009173_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateGhostFireVisualEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateGhostFireVisualEffectSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateOwnerClearInvalidChildSystem), BurstRuntime.GetHashCode64<TeammateOwnerClearInvalidChildSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateOwnerClearInvalidChildSystem.__codegen__OnCreate_000091A2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateOwnerClearInvalidChildSystem.__codegen__OnUpdate_000091A3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateOwnerClearInvalidChildSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateOwnerClearInvalidChildSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateRegisterSystem), BurstRuntime.GetHashCode64<TeammateRegisterSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateRegisterSystem.__codegen__OnCreate_000091E6_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			TeammateRegisterSystem.__codegen__OnUpdate_000091E7_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateRegisterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateRegisterSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(TeammateSystem), BurstRuntime.GetHashCode64<TeammateSystem>(), delegate(IntPtr self, IntPtr state)
		{
			TeammateSystem.__codegen__OnCreate_00009214_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((TeammateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((TeammateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "TeammateSystem", 1);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitAfterTakeDamageSystem), BurstRuntime.GetHashCode64<UnitAfterTakeDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			UnitAfterTakeDamageSystem.__codegen__OnCreate_00009246_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitAfterTakeDamageSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			UnitAfterTakeDamageSystem.__codegen__OnDestroy_00009248_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitAfterTakeDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitAfterTakeDamageSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitAttachEffectSystem), BurstRuntime.GetHashCode64<UnitAttachEffectSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((UnitAttachEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitAttachEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			UnitAttachEffectSystem.__codegen__OnDestroy_0000925F_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitAttachEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitAttachEffectSystem", 4);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitBaseSystem), BurstRuntime.GetHashCode64<UnitBaseSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((UnitBaseSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitBaseSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitBaseSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitBaseSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitBeforeAnnounceDeathSystem), BurstRuntime.GetHashCode64<UnitBeforeAnnounceDeathSystem>(), delegate(IntPtr self, IntPtr state)
		{
			((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitBeforeAnnounceDeathSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitBeforeTakeDamageSystem), BurstRuntime.GetHashCode64<UnitBeforeTakeDamageSystem>(), delegate(IntPtr self, IntPtr state)
		{
			UnitBeforeTakeDamageSystem.__codegen__OnCreate_00009301_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitBeforeTakeDamageSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			UnitBeforeTakeDamageSystem.__codegen__OnDestroy_00009303_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitBeforeTakeDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitBeforeTakeDamageSystem", 5);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitFrameAnimaSystem), BurstRuntime.GetHashCode64<UnitFrameAnimaSystem>(), delegate(IntPtr self, IntPtr state)
		{
			UnitFrameAnimaSystem.__codegen__OnCreate_00009387_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			UnitFrameAnimaSystem.__codegen__OnUpdate_00009388_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			UnitFrameAnimaSystem.__codegen__OnDestroy_00009389_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitFrameAnimaSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitFrameAnimaSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitPropertySystem), BurstRuntime.GetHashCode64<UnitPropertySystem>(), delegate(IntPtr self, IntPtr state)
		{
			((UnitPropertySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}, delegate(IntPtr self, IntPtr state)
		{
			((UnitPropertySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitPropertySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitPropertySystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(UnitTakeDamageClearSystem), BurstRuntime.GetHashCode64<UnitTakeDamageClearSystem>(), null, delegate(IntPtr self, IntPtr state)
		{
			UnitTakeDamageClearSystem.__codegen__OnUpdate_0000945D_0024BurstDirectCall.Invoke(self, state);
		}, null, null, null, delegate(IntPtr self, IntPtr state)
		{
			((UnitTakeDamageClearSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "UnitTakeDamageClearSystem", 2);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(StatefulCollisionEventBufferSystem), BurstRuntime.GetHashCode64<StatefulCollisionEventBufferSystem>(), delegate(IntPtr self, IntPtr state)
		{
			StatefulCollisionEventBufferSystem.__codegen__OnCreate_000094C1_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			StatefulCollisionEventBufferSystem.__codegen__OnUpdate_000094C2_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			StatefulCollisionEventBufferSystem.__codegen__OnDestroy_000094C3_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((StatefulCollisionEventBufferSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Unity.Physics.Stateful.StatefulCollisionEventBufferSystem", 7);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(StatefulTriggerEventBufferSystem), BurstRuntime.GetHashCode64<StatefulTriggerEventBufferSystem>(), delegate(IntPtr self, IntPtr state)
		{
			StatefulTriggerEventBufferSystem.__codegen__OnCreate_00009512_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			StatefulTriggerEventBufferSystem.__codegen__OnUpdate_00009513_0024BurstDirectCall.Invoke(self, state);
		}, delegate(IntPtr self, IntPtr state)
		{
			StatefulTriggerEventBufferSystem.__codegen__OnDestroy_00009514_0024BurstDirectCall.Invoke(self, state);
		}, null, null, delegate(IntPtr self, IntPtr state)
		{
			((StatefulTriggerEventBufferSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}, "Unity.Physics.Stateful.StatefulTriggerEventBufferSystem", 7);
	}
}
