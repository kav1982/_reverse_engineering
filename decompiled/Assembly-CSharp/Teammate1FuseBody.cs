using UnityEngine;

public class Teammate1FuseBody : MonoBehaviour
{
	public GameObject fireObj;

	public Animator Anima;

	public GameObject[] ColorfulBody;

	protected AnimaEvent animaEvent;

	public SpriteRenderer sr { get; set; }

	public Teammate1FuseController Controller { get; set; }
}
