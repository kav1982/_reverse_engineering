using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadSceneSO", menuName = "ScriptableObjects/LoadSceneSO", order = 0)]
public class LoadSceneObjSO : ScriptableObject
{
	public List<LoadUIs> AllObjLoads;

	public void LoadObjsWithoutUI()
	{
		foreach (LoadUIs allObjLoad in AllObjLoads)
		{
			foreach (LoadUI item in allObjLoad)
			{
				if (item.initWhenOpenForUI || item.loadType != 0)
				{
					continue;
				}
				GameObject gameObject = GameMgr.Inst.LoadSceneObj(item);
				if (gameObject != null)
				{
					IGameUISingleton component = gameObject.GetComponent<IGameUISingleton>();
					GameUI component2 = gameObject.GetComponent<GameUI>();
					if (component != null)
					{
						component.InitFromLoadObj();
					}
					else if (component2 != null)
					{
						component2.OnlyInit();
					}
				}
			}
		}
	}

	private void UpdateTitle()
	{
		foreach (LoadUIs allObjLoad in AllObjLoads)
		{
			foreach (LoadUI item in allObjLoad)
			{
				item.ObjName = item.resourcePathPC.path;
			}
		}
	}

	private void OnValidate()
	{
		UpdateTitle();
	}
}
