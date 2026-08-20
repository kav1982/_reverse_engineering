using UnityEngine;

public class GORandomKeep : MonoBehaviour
{
	public GameObject[] gos;

	private void Start()
	{
		if (gos.Length != 0)
		{
			int num = Random.Range(0, gos.Length);
			for (int num2 = gos.Length - 1; num2 >= 0; num2--)
			{
				if (num2 != num)
				{
					Object.Destroy(gos[num2]);
				}
			}
		}
		Object.Destroy(this);
	}
}
