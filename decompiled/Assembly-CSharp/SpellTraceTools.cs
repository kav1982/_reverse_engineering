using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class SpellTraceTools
{
	public static void CreateTrace(int spellId, Vector3 position, float size, [CanBeNull] string typePostfix = null, float scaleEnterAnimaLength = 0.07f, float recycleTime = 10f)
	{
		string tracePath = GetTracePath(spellId, typePostfix);
		Vector3 traceLayerPosition = GetTraceLayerPosition(position);
		GameObject gO = ObjPoolMgr.Inst.GetGO(tracePath, traceLayerPosition);
		gO.transform.localScale = Vector3.zero;
		gO.transform.DOScale(size, scaleEnterAnimaLength);
		SpellTraceTransparentController component = gO.GetComponent<SpellTraceTransparentController>();
		if ((bool)component)
		{
			float delayTime = component.Settings.Max((SpellTraceEffectSettings e) => e.FadeTime);
			ObjPoolMgr.Inst.RecycleGO(gO, delayTime);
		}
		else
		{
			ObjPoolMgr.Inst.RecycleGO(gO, recycleTime);
		}
	}

	private static string GetTracePath(int spellId, [CanBeNull] string typePostfix = null)
	{
		string text = string.Format("{0}{1}/{2}_Trace", "Prefabs/Spell/", spellId, spellId);
		if (typePostfix != null && typePostfix.Trim().Length > 0)
		{
			text = text + "_" + typePostfix;
		}
		return text;
	}

	private static Vector3 GetTraceLayerPosition(Vector3 position)
	{
		return Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(position), LayerCorrectType.ExplosionTrace);
	}
}
