using UnityEngine;

public class Spell3101EffectController : LayerCorrect
{
	public Transform[] lights;

	private bool setOnce;

	public override void OnEnable()
	{
		base.OnEnable();
		setOnce = false;
	}

	private void Update()
	{
		if (setOnce)
		{
			return;
		}
		for (int i = 0; i < lights.Length; i++)
		{
			if (lights[i].gameObject.activeInHierarchy)
			{
				lights[i].position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Coordinate);
			}
		}
	}
}
