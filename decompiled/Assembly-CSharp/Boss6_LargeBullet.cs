using UnityEngine;

public class Boss6_LargeBullet : MonoBehaviour
{
	private Vector3 diration;

	public float speed;

	public float shootBulletInterval;

	private float shootBulletTimer;

	public float existTime;

	private float existTimer;

	private Vector3 shootDiration;

	public float shootDirationInterval;

	private float rotateRight;

	public int spellDamage;

	public float spellSpeed;

	public float spellDuration;

	public float spellHeight;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public Boss6 master;

	public void Initialize(Boss6 master, Vector3 diration)
	{
		this.master = master;
		this.diration = diration.normalized;
		existTimer = 0f;
		rotateRight = ((!(Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
		shootDiration = Vector3.up;
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(90222);
		sipBullet.spelldataConfig.speed = spellSpeed;
		sipBullet.spelldataConfig.duration = spellDuration;
		sipBullet.spelldataConfig.damage = spellDamage;
		sipBullet.ownerPpt = master.myPpt;
	}

	private void Update()
	{
		base.transform.position += Time.deltaTime * speed * diration;
		existTimer += Time.deltaTime;
		shootBulletTimer += Time.deltaTime;
		if (shootBulletTimer > shootBulletInterval)
		{
			shootBulletTimer = 0f;
			shootDiration = Tool2D.GetDir(shootDiration, rotateRight * shootDirationInterval);
			Shoot();
		}
		if (existTimer > existTime)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void Shoot()
	{
		for (int i = 0; i < 4; i++)
		{
			sipBullet.spelldataConfig.speed = spellSpeed;
			sipBullet.shootDirection = Tool2D.GetDir(shootDiration.normalized, 90 * i);
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - spellHeight)).GetComponent<SpellBase>().Initialize(sipBullet);
		}
	}
}
