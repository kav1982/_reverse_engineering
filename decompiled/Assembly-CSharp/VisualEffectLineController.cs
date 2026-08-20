using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectLineController : MonoBehaviour
{
	public VisualEffect[] Effects;

	public void SetPositions(Vector3[] positions, float speed = 1f, float minStep = 0.1f)
	{
		float[] dis2 = GetDis(positions);
		int index = 0;
		float dis = 0f;
		List<Keyframe> xKeys = new List<Keyframe>();
		List<Keyframe> yKeys = new List<Keyframe>();
		List<Keyframe> zKeys = new List<Keyframe>();
		while (dis <= dis2[^1])
		{
			AddCurrent();
			if (index + 1 < dis2.Length && dis > dis2[index + 1])
			{
				index++;
			}
		}
		AddCurrent();
		AnimationCurve animationCurve = new AnimationCurve(xKeys.ToArray());
		for (int i = 0; i < animationCurve.length; i++)
		{
			animationCurve.SmoothTangents(i, 1f);
		}
		AnimationCurve animationCurve2 = new AnimationCurve(yKeys.ToArray());
		for (int j = 0; j < animationCurve2.length; j++)
		{
			animationCurve2.SmoothTangents(j, 1f);
		}
		AnimationCurve animationCurve3 = new AnimationCurve(zKeys.ToArray());
		for (int k = 0; k < animationCurve3.length; k++)
		{
			animationCurve3.SmoothTangents(k, 1f);
		}
		float[] array = new float[3];
		List<Keyframe> list = xKeys;
		array[0] = list[list.Count - 1].time;
		List<Keyframe> list2 = yKeys;
		array[1] = list2[list2.Count - 1].time;
		List<Keyframe> list3 = zKeys;
		array[2] = list3[list3.Count - 1].time;
		float f = Mathf.Max(array);
		VisualEffect[] effects = Effects;
		foreach (VisualEffect obj in effects)
		{
			obj.SetAnimationCurve("xCurve", animationCurve);
			obj.SetAnimationCurve("yCurve", animationCurve2);
			obj.SetAnimationCurve("zCurve", animationCurve3);
			obj.SetFloat("Time", f);
		}
		void AddCurrent()
		{
			if (xKeys.Count != 0)
			{
				List<Keyframe> list4 = xKeys;
				if (Mathf.Approximately(list4[list4.Count - 1].value, positions[index].x))
				{
					goto IL_0078;
				}
			}
			xKeys.Add(new Keyframe(dis / speed, positions[index].x));
			goto IL_0078;
			IL_00f0:
			if (zKeys.Count != 0)
			{
				List<Keyframe> list5 = zKeys;
				if (Mathf.Approximately(list5[list5.Count - 1].value, positions[index].z))
				{
					goto IL_0168;
				}
			}
			zKeys.Add(new Keyframe(dis / speed, positions[index].z));
			goto IL_0168;
			IL_0168:
			dis += minStep;
			return;
			IL_0078:
			if (yKeys.Count != 0)
			{
				List<Keyframe> list6 = yKeys;
				if (Mathf.Approximately(list6[list6.Count - 1].value, positions[index].y))
				{
					goto IL_00f0;
				}
			}
			yKeys.Add(new Keyframe(dis / speed, positions[index].y));
			goto IL_00f0;
		}
	}

	private float[] GetDis(Vector3[] positions)
	{
		float[] array = new float[positions.Length];
		for (int i = 1; i < positions.Length; i++)
		{
			float num = Vector3.Distance(positions[i], positions[i - 1]);
			array[i] = array[i - 1] + num;
		}
		return array;
	}

	private void Update()
	{
		VisualEffect[] effects = Effects;
		for (int i = 0; i < effects.Length; i++)
		{
			effects[i].transform.position = Tool2D.IgnoreZPoint(CamController.Inst.cam_Main.transform.position);
		}
	}
}
