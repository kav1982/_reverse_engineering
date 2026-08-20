using System;
using UnityEngine;

[Serializable]
public class PlatLoad
{
	public RectTransform rect;

	[Tooltip("rect为空这里才有作用")]
	public CanvasRect canvasRect;

	public GameObject goPC;

	public GameObject goMobile;

	public GameObject goSteamDeck;
}
