using Spine.Unity;
using UnityEngine;

public class Access_T6Mono : MonoBehaviour
{
	public SkeletonAnimation sAnima;

	public bool hasNokeySkin;

	public GameObject go_Rune;

	public GameObject go_RuneBoss;

	private void OnEnable()
	{
		CloseDirect();
	}

	public void SetSkinToNoKey()
	{
		if (hasNokeySkin)
		{
			sAnima.skeleton.SetSkin("T6_Access_NotNeedKey");
			sAnima.skeleton.SetSlotsToSetupPose();
		}
	}

	public void SetIsBossOrBloodRelic(bool isBoss)
	{
		if ((bool)go_Rune)
		{
			go_Rune.SetActive(value: false);
			go_RuneBoss.SetActive(value: false);
			if (isBoss)
			{
				go_RuneBoss.SetActive(value: true);
			}
			else
			{
				go_Rune.SetActive(value: true);
			}
		}
	}

	public void Open()
	{
		sAnima.AnimationState.SetAnimation(0, "Open", loop: false);
	}

	public void OpenDirect()
	{
		sAnima.AnimationState.SetAnimation(0, "OpenDirect", loop: false);
	}

	public void Close()
	{
		sAnima.AnimationState.SetAnimation(0, "Close", loop: false);
	}

	public void CloseDirect()
	{
		sAnima.AnimationState.SetAnimation(0, "CloseDirect", loop: false);
	}
}
