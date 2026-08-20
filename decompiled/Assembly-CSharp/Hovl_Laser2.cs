using UnityEngine;

public class Hovl_Laser2 : MonoBehaviour
{
	public float laserScale = 1f;

	public Color laserColor = new Vector4(1f, 1f, 1f, 1f);

	public GameObject HitEffect;

	public GameObject FlashEffect;

	public float HitOffset;

	public float MaxLength;

	private bool UpdateSaver;

	private ParticleSystem laserPS;

	private ParticleSystem[] Flash;

	private ParticleSystem[] Hit;

	private Material laserMat;

	private int particleCount;

	private ParticleSystem.Particle[] particles;

	private Vector3[] particlesPositions;

	private float dissovleTimer;

	private bool startDissovle;

	private void Start()
	{
		laserPS = GetComponent<ParticleSystem>();
		laserMat = GetComponent<ParticleSystemRenderer>().material;
		Flash = FlashEffect.GetComponentsInChildren<ParticleSystem>();
		Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
		laserMat.SetFloat("_Scale", laserScale);
	}

	private void Update()
	{
		if (laserPS != null && !UpdateSaver)
		{
			laserMat.SetVector("_StartPoint", base.transform.position);
			if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out var hitInfo, MaxLength))
			{
				particleCount = Mathf.RoundToInt(hitInfo.distance / (2f * laserScale));
				if ((float)particleCount < hitInfo.distance / (2f * laserScale))
				{
					particleCount++;
				}
				particlesPositions = new Vector3[particleCount];
				AddParticles();
				laserMat.SetFloat("_Distance", hitInfo.distance);
				laserMat.SetVector("_EndPoint", hitInfo.point);
				if (Hit != null)
				{
					HitEffect.transform.position = hitInfo.point + hitInfo.normal * HitOffset;
					HitEffect.transform.LookAt(hitInfo.point);
					ParticleSystem[] hit = Hit;
					foreach (ParticleSystem particleSystem in hit)
					{
						if (!particleSystem.isPlaying)
						{
							particleSystem.Play();
						}
					}
					hit = Flash;
					foreach (ParticleSystem particleSystem2 in hit)
					{
						if (!particleSystem2.isPlaying)
						{
							particleSystem2.Play();
						}
					}
				}
			}
			else
			{
				Vector3 vector = base.transform.position + base.transform.forward * MaxLength;
				float num = Vector3.Distance(vector, base.transform.position);
				particleCount = Mathf.RoundToInt(num / (2f * laserScale));
				if ((float)particleCount < num / (2f * laserScale))
				{
					particleCount++;
				}
				particlesPositions = new Vector3[particleCount];
				AddParticles();
				laserMat.SetFloat("_Distance", num);
				laserMat.SetVector("_EndPoint", vector);
				if (Hit != null)
				{
					HitEffect.transform.position = vector;
					ParticleSystem[] hit = Hit;
					foreach (ParticleSystem particleSystem3 in hit)
					{
						if (particleSystem3.isPlaying)
						{
							particleSystem3.Stop();
						}
					}
				}
			}
		}
		if (startDissovle)
		{
			dissovleTimer += Time.deltaTime;
			laserMat.SetFloat("_Dissolve", dissovleTimer * 5f);
		}
	}

	private void AddParticles()
	{
		particles = new ParticleSystem.Particle[particleCount];
		for (int i = 0; i < particleCount; i++)
		{
			particlesPositions[i] = new Vector3(0f, 0f, 0f) + new Vector3(0f, 0f, (float)(i * 2) * laserScale);
			particles[i].position = particlesPositions[i];
			particles[i].startSize3D = new Vector3(0.001f, 0.001f, 2f * laserScale);
			particles[i].startColor = laserColor;
		}
		laserPS.SetParticles(particles, particles.Length);
	}

	public void DisablePrepare()
	{
		base.transform.parent = null;
		dissovleTimer = 0f;
		startDissovle = true;
		UpdateSaver = true;
		if (Flash == null || Hit == null)
		{
			return;
		}
		ParticleSystem[] hit = Hit;
		foreach (ParticleSystem particleSystem in hit)
		{
			if (particleSystem.isPlaying)
			{
				particleSystem.Stop();
			}
		}
		hit = Flash;
		foreach (ParticleSystem particleSystem2 in hit)
		{
			if (particleSystem2.isPlaying)
			{
				particleSystem2.Stop();
			}
		}
	}
}
