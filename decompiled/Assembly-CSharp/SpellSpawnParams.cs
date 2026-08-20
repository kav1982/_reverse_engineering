using Unity.Entities;
using Unity.Mathematics;

public struct SpellSpawnParams : IBufferElementData
{
	public int PrefabId;

	public float3 SpawnPosition;

	public AttributeValue SpeedAttribute;

	public float2 SourceShootDir;

	public float2 SourceShootTargetPosition;

	public int MultiShootAddictionCount;

	public bool ReserveDirection;

	public float ChargeTimer;

	public float ManaCostRatio;

	public float Offset;

	public float ManaCost;

	public int InShootCountIndex;

	public bool IsSplitSpell;

	public bool DisableShootSound;

	public bool RandomPosFocusMouse;

	public float SpellExtraSizeRatio;

	public bool DisableResize;

	public Entity SubGroupEntity;

	public Entity OverSplitTriggerBufferEntity;

	private Entity _shooter;

	private Entity _ownerUnit;

	public float SpellEfficiency;

	public bool IgnoreSpawnLightningChain;

	public UnityObjectRef<Wand> Wand;

	public UnityObjectRef<Spell4004ChargeStars> ChargeStar;

	public Entity ChargeStarEntity;

	public bool FromPostSlot;

	public bool FromEcho;

	public bool EnableTriggerRedRune;

	public bool OverFlowCriticalChanceToDamage;

	public bool SpellEndTeleport;

	public int HalfLifeTeleportCount;

	public float HalfLifeTeleportRadius;

	public bool IsFuseTeammate;

	public float DropCoinRatioOnKill;

	public float DropCrystalRatioOnKill;

	public float FourDirectionWandAngle;

	public SpellConfigComponentData ConfigComponentData;

	public SpellMovementComponentData MovementComponentData;

	public SpellElementEffectComponentData ElementComponentData;

	public SpellSplitComponentData SplitComponentData;

	public SpellMoveTriggerComponentData MoveTriggerComponentData;

	public SpellHitTriggerComponentData HitTriggerComponentData;

	public SpellOverTriggerComponentData OverTriggerComponentData;

	public SpellTwineTriggerComponentData TwineTriggerComponentData;

	public SpellRefractionData RefractionData;

	public TeammateData TeammateComponentData;

	public float radiuDecreaseRatio;

	public float radiuDcreaseTransIntoDamageRatio;

	public int BonusPenetrate;

	public Entity Shooter => _shooter;

	public Entity OwnerUnit => _ownerUnit;

	public void SetShooter(Entity shooter, Entity ownerUnit)
	{
		_shooter = shooter;
		_ownerUnit = ownerUnit;
	}
}
