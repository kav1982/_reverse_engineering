using UnityEngine;
using UnityEngine.UI;

public class UIAchievementPopOut : MonoBehaviour
{
	public Image icon;

	public Text achieveName;

	public Text achieveDescription;

	public Animator animator;

	public void Recycle()
	{
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void Init(Sprite sp, string name, string description)
	{
		base.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, -50f);
		animator.Play("New State");
		icon.sprite = sp;
		achieveName.text = name;
		achieveDescription.text = description;
		base.gameObject.SetActive(value: true);
		Debug.Log("完成成就" + name + "::" + description);
		animator.Play("PopOut");
	}
}
