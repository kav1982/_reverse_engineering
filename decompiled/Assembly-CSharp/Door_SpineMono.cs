using Spine;
using Spine.Unity;
using UnityEngine;

public class Door_SpineMono : MonoBehaviour
{
	public SkeletonAnimation sAnima;

	public bool considerHVersion;

	public string hSkinName;

	private void OnEnable()
	{
		if (considerHVersion && GameMgr.IsHarmony_Static)
		{
			Skeleton skeleton = sAnima.Skeleton;
			Skin skin = skeleton.Data.FindSkin(hSkinName);
			if (skin == null)
			{
				Debug.LogError("Skin not found: " + hSkinName);
				return;
			}
			skeleton.SetSkin(skin);
			skeleton.SetSlotsToSetupPose();
			sAnima.Update(0f);
		}
		sAnima.AnimationState.SetAnimation(0, "CloseDirect", loop: false);
	}

	public void Open()
	{
		sAnima.AnimationState.SetAnimation(0, "Open", loop: false);
	}

	public void OpenDirect()
	{
		sAnima.AnimationState.SetAnimation(0, "OpenDirect", loop: false);
	}
}
