using System.Globalization;
using UnityEngine;

public class RatioValue
{
	public double BaseValue;

	private double baseAddRatio = 1.0;

	private double addRatio = 1.0;

	private double mulRatio = 1.0;

	private double addBase;

	private double addExtra;

	public float CurrentAddBase => (float)addBase;

	public float CurrentAddRatioStartOne => (float)addRatio;

	public float CurrentAddRatioStartZero => (float)addRatio - 1f;

	public float CurrentBaseAddRatioStartZero => (float)baseAddRatio - 1f;

	public float CurrentBaseAddRatioStartOne => (float)baseAddRatio;

	public float CurrentMulRatio => (float)mulRatio;

	public float CurrentFinalRatio => (float)(addRatio * mulRatio);

	public float CurrentAddExtra => (float)addExtra;

	public float Result => (float)ResultDouble;

	public double ResultDouble => (BaseValue + addBase) * baseAddRatio * addRatio * mulRatio + addExtra;

	public int ResultCeilToInt => Mathf.CeilToInt(Result);

	public float ResultRatio => (float)(baseAddRatio * addRatio * mulRatio);

	public RatioValue(double baseValue)
	{
		BaseValue = baseValue;
	}

	public RatioValue Copy()
	{
		return new RatioValue(BaseValue)
		{
			addRatio = addRatio,
			mulRatio = mulRatio,
			addBase = addBase,
			addExtra = addExtra,
			baseAddRatio = baseAddRatio
		};
	}

	public RatioValue AddBase(double val)
	{
		addBase += val;
		return this;
	}

	public RatioValue AddExtra(double val)
	{
		addExtra += val;
		return this;
	}

	public RatioValue AddRatio(double val)
	{
		addRatio += val;
		return this;
	}

	public RatioValue AddBaseAddRatio(double val)
	{
		baseAddRatio += val;
		return this;
	}

	public RatioValue MulRatio(double val)
	{
		mulRatio *= val;
		return this;
	}

	public RatioValue Merge(RatioValue other)
	{
		addRatio += other.CurrentAddRatioStartZero;
		mulRatio *= other.CurrentMulRatio;
		addBase += other.CurrentAddBase;
		addExtra += other.CurrentAddExtra;
		baseAddRatio += other.CurrentBaseAddRatioStartZero;
		return this;
	}

	public void SetBaseValue(float value)
	{
		BaseValue = value;
	}

	public override string ToString()
	{
		return Result.ToString(CultureInfo.InvariantCulture);
	}
}
