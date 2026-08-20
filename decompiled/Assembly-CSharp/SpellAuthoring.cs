using Unity.Entities;
using UnityEngine;

public class SpellAuthoring : MonoBehaviour
{
	private class SpellConfigAuthoringBaker : Baker<SpellAuthoring>
	{
		public override void Bake(SpellAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpellComponentData component = new SpellComponentData
			{
				ReduceSizeWhenLifeEnd = authoring.ReduceSizeWhenLifeEnd,
				CanThroughWall = authoring.CanThroughWall,
				PlayHitSE = authoring.PlayHitSE,
				TrailLayer = authoring.TrailLayer,
				DisableAutoCreateFallEffect = authoring.DisableAutoCreateFallEffect,
				DisableDefaultFallDamage = authoring.DisableDefaultFallDamage,
				DisableAutoClearFallGroundedTag = authoring.DisableAutoClearFallGroundedTag,
				PlayHitSEOnFallGroundedTag = authoring.PlayHitSEOnFallGrounded,
				GlobalParticleHitEffect = authoring.UseGlobalParticleHitEffect,
				UseGlobalHitParticleWithSpellDirection = authoring.UseGlobalHitParticleWithSpellDirection
			};
			AddComponent(entity, in component);
			AddComponent<SpellConfigComponentData>(entity);
			AddComponent<SpellMovementComponentData>(entity);
			AddComponent<SpellGroundedTag>(entity);
			SetComponentEnabled<SpellGroundedTag>(entity, enabled: false);
			AddComponent<SpellElementEffectComponentData>(entity);
			AddComponent<SpellSplitComponentData>(entity);
			AddComponent<SpellDestroyTag>(entity);
			SetComponentEnabled<SpellDestroyTag>(entity, enabled: false);
			AddComponent<SpellMoveTriggerComponentData>(entity);
			SetComponentEnabled<SpellMoveTriggerComponentData>(entity, enabled: false);
			AddComponent<SpellOverSplitTriggerComponentData>(entity);
			SetComponentEnabled<SpellOverSplitTriggerComponentData>(entity, enabled: false);
			AddComponent<SpellHitTriggerComponentData>(entity);
			SetComponentEnabled<SpellHitTriggerComponentData>(entity, enabled: false);
			AddComponent<SpellOverTriggerComponentData>(entity);
			SetComponentEnabled<SpellOverTriggerComponentData>(entity, enabled: false);
			AddComponent<SpellTwineTriggerComponentData>(entity);
			SetComponentEnabled<SpellTwineTriggerComponentData>(entity, enabled: false);
			AddComponent<EnterDoorDestroy>(entity);
			SpellNeedResize component2 = new SpellNeedResize
			{
				ResizeByDamage = authoring.ResizeByDamage
			};
			AddComponent(entity, in component2);
			SetComponentEnabled<SpellNeedResize>(entity, enabled: true);
			AddBuffer<SpellHitEntity>(entity);
			if (authoring.DisableTriggerDamage)
			{
				AddComponent<SpellDisableTriggerDamageTag>(entity);
			}
			if (authoring.SpellLookDirection)
			{
				AddComponent<SpellEffectTag_SpellLookDirection>(entity);
			}
			if (authoring.TrailSizeByRadius)
			{
				AddComponent<SpellEffectTag_TrailSizeByRadius>(entity);
			}
			if (authoring.PlayShootSE)
			{
				AddComponent<SpellPlayShootSETag>(entity);
			}
		}
	}

	[Tooltip("是否自动设置Spell特效的朝向为飞行方向")]
	[Header("鼠标悬停在字段名上可以查看解释")]
	public bool SpellLookDirection;

	[Tooltip("是否根据影响半径设置Dots的Trail大小")]
	public bool TrailSizeByRadius;

	[Tooltip("是否在法术持续时间结束时播放缩小体积的动画")]
	public bool ReduceSizeWhenLifeEnd;

	[Tooltip("是否可穿墙，如果启用，还要把法术的碰撞器删掉")]
	public bool CanThroughWall;

	[Tooltip("法术是否会根据法术伤害变大变小")]
	public bool ResizeByDamage = true;

	[Tooltip("关闭触发器产生的伤害，就是关闭类似魔法弹那种撞击伤害")]
	public bool DisableTriggerDamage;

	[Tooltip("关闭自动创建坠落特效，如果法术有特殊的特效逻辑，可以勾上。\n但如果只是换个特效，可以去创建一个对应法术的Fall特效")]
	public bool DisableAutoCreateFallEffect;

	[Tooltip("关闭默认的坠落伤害逻辑，如果法术有特殊的坠落逻辑，可以勾上")]
	public bool DisableDefaultFallDamage;

	[Tooltip("自否自动播放全局粒子版的Hit特效")]
	public bool UseGlobalParticleHitEffect;

	[Tooltip("上方选项选中时才会生效 决定自动播放全局的粒子版的Hit特效是否使用法术的方向")]
	public bool UseGlobalHitParticleWithSpellDirection;

	[Tooltip("关闭自动取消坠落标记（在SpellFallDamageSystem中的逻辑）")]
	public bool DisableAutoClearFallGroundedTag;

	[Tooltip("是否自动播放射击音效")]
	public bool PlayShootSE;

	[Tooltip("是否自动播放命中音效")]
	public bool PlayHitSE;

	[Tooltip("是否在坠落落地的时候自动播放命中音效")]
	public bool PlayHitSEOnFallGrounded;

	[Tooltip("Dots拖尾的层级")]
	public LayerCorrectType TrailLayer = LayerCorrectType.Coordinate;
}
