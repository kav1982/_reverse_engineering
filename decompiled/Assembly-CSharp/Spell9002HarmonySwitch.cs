using UnityEngine;

public class Spell9002HarmonySwitch : MonoBehaviour
{
	public SpriteRenderer sr;

	public SpriteRenderer sr_Border;

	public SpriteRenderer sr_H;

	public SpriteRenderer sr_BorderH;

	public SpriteRenderer sr_Shadow;

	public SpriteRenderer sr_ShadowH;

	private void Start()
	{
		if (!GameMgr.IsHarmony_Static)
		{
			sr.enabled = true;
			sr_Border.enabled = true;
			sr_Shadow.enabled = true;
			sr_H.enabled = false;
			sr_BorderH.enabled = false;
			sr_ShadowH.enabled = false;
		}
		else
		{
			sr.enabled = false;
			sr_Border.enabled = false;
			sr_Shadow.enabled = false;
			sr_H.enabled = true;
			sr_BorderH.enabled = true;
			sr_ShadowH.enabled = true;
		}
	}
}
