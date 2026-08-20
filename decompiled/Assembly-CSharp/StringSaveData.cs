using System;
using System.Collections.Generic;

[Serializable]
public class StringSaveData
{
	public List<WandConfig> w = new List<WandConfig>();

	public SlotData[] b;

	public List<RelicConfig> r = new List<RelicConfig>();
}
