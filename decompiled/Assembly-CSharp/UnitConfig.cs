using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;

public struct UnitConfig
{
	public class Initializer : DataMgr.ConfigInitializer<UnitConfig>
	{
		public override void ApplyResult(List<UnitConfig> result)
		{
			list = result;
			if (map.IsCreated)
			{
				map.Dispose();
			}
			map = new NativeHashMap<int, UnitConfig>(list.Count, Allocator.Persistent);
			for (int i = 0; i < list.Count; i++)
			{
				if (!ICJNOGPFMAM.GGPJCCLPBJL && GameMgr.IsMobile_Static)
				{
					UnitConfig value = list[i];
					if (value.id == 500901 || value.id == 501001 || value.id == 501301)
					{
						value.inGallery = false;
					}
					list[i] = value;
				}
				if (GameMgr.IsHarmony_Static)
				{
					UnitConfig value2 = list[i];
					value2.bloodSplatSize = 0f;
					value2.corpseType = CorpseType.None;
					value2.deadPermanentEF = "";
					UnitConfig unitConfig = list[i];
					if (unitConfig.deadEFH != "")
					{
						value2.deadEF = list[i].deadEFH;
					}
					if (value2.id == 300501)
					{
						value2.inGallery = false;
					}
					if (value2.id / 100 == 1041)
					{
						value2.inGallery = false;
					}
					list[i] = value2;
					if (GameMgr.IsChAge14_Static)
					{
						UnitConfig value3 = list[i];
						if (value3.id / 100 == 1056)
						{
							value3.inGallery = false;
						}
						list[i] = value3;
					}
				}
				map.Add(list[i].id, list[i]);
			}
		}
	}

	public static NativeHashMap<int, UnitConfig> map;

	public static List<UnitConfig> list;

	public int id;

	public UnitType unitType;

	public float maxHP;

	public float moveSpeed;

	public float knockbackRatio;

	public float beHitRatio;

	public float frozenTimeRatio;

	public bool immuneMucus;

	public bool immuneVenom;

	public bool isFly;

	public bool isCheckStuck;

	public bool theme6Reposition;

	public bool isBehitColor;

	public bool immediatelyBorn;

	public float bloodSplatSize;

	public CorpseType corpseType;

	public int corpseCount;

	[JsonConverter(typeof(FixedString128BytesConverter))]
	public FixedString128Bytes beHitSE;

	[JsonConverter(typeof(FixedString128BytesConverter))]
	public FixedString128Bytes deadEF;

	[JsonConverter(typeof(FixedString128BytesConverter))]
	public FixedString128Bytes deadEFH;

	[JsonConverter(typeof(FixedString128BytesConverter))]
	public FixedString128Bytes deadPermanentEF;

	[JsonConverter(typeof(FixedString128BytesArrayConverter))]
	public BlobAssetReference<BlobArray<FixedString128Bytes>> deadSEs;

	public bool isDeadSE3D;

	public bool inGallery;

	public bool haveGalleryH;

	public bool triggerDeadEvent;

	public bool loadArchiveCreate;

	public float relicShowHPUIHight;

	public DifficultyType appearDiffi;

	public float appearChapter;

	public bool isHybirdUnit;

	public bool isSolidObj;

	public float currentHP;

	public float shield;

	public float shieldTemp;

	public bool immuneSpike;

	public bool immuneAbyss;

	public static List<UnitConfig> ShallowCopyListSorted()
	{
		List<UnitConfig> obj = new List<UnitConfig>(list);
		obj.Sort(delegate(UnitConfig a, UnitConfig b)
		{
			int num = a.appearChapter.CompareTo(b.appearChapter);
			return (num == 0) ? a.id.CompareTo(b.id) : num;
		});
		return obj;
	}

	public string GetName()
	{
		return (id + 12000000).GetText();
	}

	public string GetDesc()
	{
		return (id + 13000000).GetText();
	}

	public bool IsSameCamp(UnitType unitType)
	{
		switch (this.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			if (unitType == UnitType.Player || unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
			{
				return true;
			}
			return false;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
			if (unitType == UnitType.Monster || unitType == UnitType.Elite || unitType == UnitType.Boss || unitType == UnitType.WillAttack)
			{
				return true;
			}
			return false;
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness)
			{
				return true;
			}
			return false;
		default:
			return true;
		}
	}

	public static bool IsSameCamp(UnitType thisType, UnitType unitType)
	{
		switch (thisType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			if (unitType == UnitType.Player || unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
			{
				return true;
			}
			return false;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
			if (unitType == UnitType.Monster || unitType == UnitType.Elite || unitType == UnitType.Boss || unitType == UnitType.WillAttack)
			{
				return true;
			}
			return false;
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness)
			{
				return true;
			}
			return false;
		default:
			return true;
		}
	}

	public string GetIconPath()
	{
		if (GameMgr.IsHarmony_Static && haveGalleryH)
		{
			return "Textures/UnitIconsH/" + id;
		}
		return "Textures/UnitIcons/" + id;
	}

	public string GetModelPath()
	{
		if (GameMgr.IsHarmony_Static && haveGalleryH)
		{
			return "Textures/UnitModelsH/" + id;
		}
		return "Textures/UnitModels/" + id;
	}
}
