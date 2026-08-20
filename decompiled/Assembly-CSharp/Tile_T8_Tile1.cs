using UnityEngine;

public class Tile_T8_Tile1 : LayerCorrect
{
	[Space(50f)]
	public GameObject[] randomKeepGOs;

	public Transform tsf_Flip;

	public VariableFloat scale;

	public float offset;

	public override void OnEnable()
	{
		if (randomKeepGOs.Length != 0)
		{
			int num = Random.Range(0, randomKeepGOs.Length);
			for (int num2 = randomKeepGOs.Length - 1; num2 >= 0; num2--)
			{
				if (num2 != num)
				{
					Object.Destroy(randomKeepGOs[num2]);
				}
			}
		}
		if (Random.Range(0, 2) == 0)
		{
			tsf_Flip.localScale = new Vector3(-1f, 1f, 1f);
		}
		tsf_Flip.localScale *= scale.RandomResult();
		tsf_Flip.position += Tool2D.GetDir() * Random.Range(0f, offset);
		base.OnEnable();
	}
}
