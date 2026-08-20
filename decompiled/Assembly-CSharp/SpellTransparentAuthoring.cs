using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class SpellTransparentAuthoring : MonoBehaviour
{
	private class SpellTransparentAuthoringBaker : Baker<SpellTransparentAuthoring>
	{
		public override void Bake(SpellTransparentAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			switch (authoring.Type)
			{
			case TransparentType.Spell:
			{
				SpellTransparentSystem.SpellTag component3 = default(SpellTransparentSystem.SpellTag);
				AddComponent(entity, in component3);
				break;
			}
			case TransparentType.Teammate:
			{
				SpellTransparentSystem.TeammateTag component2 = default(SpellTransparentSystem.TeammateTag);
				AddComponent(entity, in component2);
				break;
			}
			case TransparentType.SpellOrTeammate_ConsiderShooterType:
			{
				SpellTransparentSystem.ByShooterTypeTag component = default(SpellTransparentSystem.ByShooterTypeTag);
				AddComponent(entity, in component);
				break;
			}
			}
			AddComponent<SpellTransparentSystem.SpellTransparentMaterialOverride>(entity);
			if (authoring.MaybeShootFromMonster)
			{
				AddComponent<SpellTransparentSystem.MaybeShootFromMonsterTag>(entity);
			}
		}
	}

	public enum TransparentType
	{
		Spell,
		Teammate,
		SpellOrTeammate_ConsiderShooterType
	}

	[Header("选择采用哪个透明度")]
	public TransparentType Type;

	[Header("这个特效是否有可能从敌人那边发射出来")]
	public bool MaybeShootFromMonster;
}
