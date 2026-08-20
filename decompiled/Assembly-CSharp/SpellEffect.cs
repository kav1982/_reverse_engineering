using System;
using Unity.Collections;

[Serializable]
public struct SpellEffect
{
	public FixedString32Bytes Name;

	public LayerCorrectType Layer;

	public float DestroyDelay;

	public SpellEffectSystem.ScaleMode ScaleMode;

	public bool AutoCreate;

	public bool IgnoreColor;

	public bool ClearTrail;

	public bool ClearParticle;

	public bool UseLowFpsOptimize;

	public int MaxInPoolCount;
}
