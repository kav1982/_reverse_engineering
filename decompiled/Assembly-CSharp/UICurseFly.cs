using UnityEngine;

public class UICurseFly : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public GameObject go_Image;

	public ParticleSystem ps_Paush;

	public GameObject go_FlyFinish;

	public Vector3 middlePointOffset;

	public Vector3 middlePointOffsetMobile;

	public float lerpSPeed;

	public float waitDestroyTime;

	private int curseID;

	private Vector3 originalPoint;

	private Vector3 moveToPoint;

	private Vector3 middlePoint;

	private bool addGallery;

	private float currentLerp;

	private bool flyFinish;

	private float waitDestroyTimer;

	private void Update()
	{
		if (flyFinish)
		{
			waitDestroyTimer += Time.unscaledDeltaTime;
			if (waitDestroyTimer >= waitDestroyTime)
			{
				base.gameObject.SetActive(value: false);
			}
			return;
		}
		currentLerp += lerpSPeed * Time.unscaledDeltaTime;
		rtsf_Self.anchoredPosition = GeneralTool.QuadraticBezierCurve(originalPoint, middlePoint, moveToPoint, currentLerp);
		if (currentLerp >= 1f)
		{
			flyFinish = true;
			go_FlyFinish.SetActive(value: true);
			go_Image.gameObject.SetActive(value: false);
			ps_Paush.Stop();
			PlayerMgr.Inst.ItemCtrller.CurseAdd(curseID, addGallery);
			SEMgr.Inst.uiCurseFlyFinish.PlaySE();
		}
	}

	public void Initialize(int curseID, Vector3 moveToPoint, bool addGallery)
	{
		this.curseID = curseID;
		this.moveToPoint = moveToPoint;
		this.addGallery = addGallery;
		originalPoint = rtsf_Self.anchoredPosition;
		if (GameMgr.IsMobile_Static)
		{
			middlePoint = originalPoint + middlePointOffsetMobile;
		}
		else
		{
			middlePoint = originalPoint + middlePointOffset;
		}
		currentLerp = 0f;
		flyFinish = false;
		waitDestroyTimer = 0f;
		go_Image.SetActive(value: true);
		go_FlyFinish.SetActive(value: false);
		SEMgr.Inst.uiCurseFly.PlaySE();
	}
}
