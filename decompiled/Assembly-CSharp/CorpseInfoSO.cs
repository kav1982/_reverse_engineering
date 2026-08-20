using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CorpseInfoSO", menuName = "ScriptableObjects/CorpseInfoSO")]
public class CorpseInfoSO : ScriptableObject
{
	public List<CorpseInfo> corpseInfo = new List<CorpseInfo>();
}
