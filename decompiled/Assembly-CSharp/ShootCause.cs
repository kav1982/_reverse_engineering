public class ShootCause
{
	public class ByWand : ShootCause
	{
		public TempFrameData<Wand> Wand { get; private set; }

		public WandConfig wandCfgCopy { get; private set; }

		public ByWand(Wand wand)
		{
			Wand = new TempFrameData<Wand>(wand);
			wandCfgCopy = wand.WandCfg.Copy();
		}
	}

	public class ByUnit : ShootCause
	{
		public TempFrameData<UnitBase> Unit { get; private set; }

		public ByUnit(UnitBase unit)
		{
			Unit = new TempFrameData<UnitBase>(unit);
		}
	}

	public class BySpell : ShootCause
	{
		public TempFrameData<SpellBase> Spell { get; private set; }

		public bool ByTrigger { get; private set; }

		public BySpell(SpellBase spell, bool byTrigger)
		{
			Spell = new TempFrameData<SpellBase>(spell);
			ByTrigger = byTrigger;
		}
	}

	public class BySplit : ShootCause
	{
		public TempFrameData<SpellBase> Spell { get; private set; }

		public BySplit(SpellBase spell)
		{
			Spell = new TempFrameData<SpellBase>(spell);
		}
	}
}
