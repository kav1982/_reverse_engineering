using UnityEngine;

public class EnterTheGungeonBookPageFly : MonoBehaviour
{
	public Animator anima;

	public VariableFloat upForce;

	public float gravity;

	private float currentUpSpeed;

	public VariableFloat horizontalLength;

	public float horizontalTimer;

	public float horizontalForce;

	private int forceDir = 1;

	public Rigidbody Rigid;

	public SpriteRenderer spriteRenderer;

	private float Yoffset;

	private void OnEnable()
	{
		horizontalLength.RandomResult();
		currentUpSpeed += upForce.RandomResult();
		forceDir = Random.Range(0, 2) * 2 - 1;
		horizontalTimer = Random.Range(0f, horizontalLength.result);
		Yoffset = Random.Range(-1.5f, 1.5f);
	}

	private void Update()
	{
		horizontalTimer += Time.deltaTime;
		if (horizontalTimer < horizontalLength.result)
		{
			Rigid.AddForce(Vector3.right * forceDir * horizontalForce * Time.deltaTime);
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(base.transform.position.x, base.transform.position.y + Yoffset, 0f), Time.deltaTime * 0.25f);
		}
		else if (Mathf.Abs(Rigid.linearVelocity.x) > 0.1f)
		{
			Rigid.linearVelocity = Vector3.Lerp(Rigid.linearVelocity, Vector3.zero, Time.deltaTime * 1.5f);
		}
		else if (horizontalTimer > horizontalLength.result * 2f)
		{
			horizontalTimer = 0f;
			forceDir *= -1;
		}
		if (currentUpSpeed > 0f)
		{
			currentUpSpeed += gravity * Time.deltaTime;
		}
		else
		{
			currentUpSpeed += 0.05f * gravity * Time.deltaTime;
		}
		base.transform.position += new Vector3(0f, 0f, 0f - currentUpSpeed) * Time.deltaTime;
		if (base.transform.position.z >= 0f && currentUpSpeed < 0f)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DeadPermanent_Page", base.transform.position).GetComponent<Corpse>().sr.sprite = spriteRenderer.sprite;
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
