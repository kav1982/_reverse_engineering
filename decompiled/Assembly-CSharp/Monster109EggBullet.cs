using UnityEngine;

public class Monster109EggBullet : MonoBehaviour
{
	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public void Init(UnitProperty ppt)
	{
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(90281);
		sipBullet.spelldataConfig.speed = spellSpeed;
		sipBullet.spelldataConfig.duration = spellDuration;
		sipBullet.spelldataConfig.damage = spellDamage;
		sipBullet.ownerPpt = ppt;
	}

	public void Crack(Vector3 position)
	{
		for (int i = 0; i < 12; i++)
		{
			sipBullet.shootDirection = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, position), Random.Range(-10, 10));
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, new Vector3(0f, -0.2f, 0f - spellHeight) + position + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f)).GetComponent<SpellBase>().Initialize(sipBullet);
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
