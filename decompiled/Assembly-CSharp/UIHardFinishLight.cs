using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHardFinishLight : MonoBehaviour
{
	public List<Text> texsts;

	private void Start()
	{
		for (int i = 0; i < texsts.Count; i++)
		{
			texsts[i].text = (1003601 + i).GetText();
		}
	}
}
