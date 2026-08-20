using UnityEngine;

public class GroundDetail : MonoBehaviour
{
	public GameObject pfb_GroundDetail;

	public Transform tsf_Tsf;

	public Sprite[] sprite_Details;

	public Animator animator;

	public SpriteRenderer sr;

	public VariableInt extraCount;

	public float offset;

	private bool canSplit = true;

	private void Start()
	{
		sr.sprite = sprite_Details[Random.Range(0, sprite_Details.Length)];
		if (canSplit)
		{
			extraCount.RandomResult();
			for (int i = 0; i < extraCount.result; i++)
			{
				Object.Instantiate(pfb_GroundDetail, base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<GroundDetail>().SetCantSplit();
			}
		}
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		tsf_Tsf.position = Tool2D.GetLayerPoint(tsf_Tsf);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.IsPlayerTrigger() || other.tag == "Monster")
		{
			animator.SetTrigger("Shake");
		}
	}

	public void SetCantSplit()
	{
		canSplit = false;
	}
}
