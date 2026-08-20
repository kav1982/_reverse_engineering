using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialObj223GameDataSO", menuName = "ScriptableObjects/SpecialObj/SO_SpecialObj223GameData", order = 0)]
public class SpecialObj223GameDataSO : ScriptableObject
{
	public int testID = -1;

	public List<SpecialObj223GameData> levels;
}
