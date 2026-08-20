using System;
using UnityEngine;

[Serializable]
public class LoadUI
{
	public enum LoadType
	{
		UI,
		Entity
	}

	[InspectorReadOnly]
	public string ObjName;

	[Tooltip("针对UI,打开再初始化")]
	public bool initWhenOpenForUI;

	public mainRoot mainRoot;

	public LoadType loadType;

	public string pcRoot;

	public string mobileRoot;

	public string steamDeckRoot;

	[ResourcePath]
	public ResourcePath resourcePathPC;

	[ResourcePath]
	public ResourcePath resourcePathMobile;

	[ResourcePath]
	public ResourcePath resourcePathSteamdeck;

	public bool overrideSiblings;

	public int overrideSiblingindex = -1;

	public bool skipLoadMobile;

	public bool ovverideMobileScale;

	public float mobileScaler = 1f;

	public Vector3 mobilePositionOffset;

	public bool ovverideSteamDeckScale;

	public float steamDeckScaler = 1f;

	public string currentPlatformObjectName
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return resourcePathPC.path.Split('/')[^1];
			}
			return resourcePathMobile.path.Split('/')[^1];
		}
	}
}
