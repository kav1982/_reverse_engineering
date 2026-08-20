using System;

[Serializable]
public class SpellSpriteEffectLayerSettings
{
	public enum BaseType
	{
		Spell,
		AttachTarget
	}

	public bool EnableLayerCorrect = true;

	public LayerCorrectType Layer = LayerCorrectType.Coordinate;

	public float OffsetZ = -0.3f;

	public BaseType BaseMode;
}
