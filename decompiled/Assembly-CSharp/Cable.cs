using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Cable : MonoBehaviour
{
	[SerializeField]
	private float gravity = 30f;

	[SerializeField]
	private int stiffness = 10;

	[SerializeField]
	private bool startPointLock;

	[SerializeField]
	private bool endPointLock;

	[SerializeField]
	private bool isHair;

	public GameObject endGameobject;

	private LineRenderer lineRenderer;

	private List<Vector3> OriginalPosition = new List<Vector3>();

	public List<Particle> particles = new List<Particle>();

	private List<Stick> sticks = new List<Stick>();

	[Header("RopeGenerate")]
	public float length;

	[Range(2f, 99f)]
	public int Fineness;

	public List<int> Seperate;

	[Header("GameobjectGenerate")]
	public Transform parent;

	public bool generateGameobject;

	public float radius = 0.1f;

	private void OnEnable()
	{
		Initialization();
		Seperate = new List<int>
		{
			0,
			Fineness - 1
		};
	}

	private void FixedUpdate()
	{
		Simulation();
	}

	private void LateUpdate()
	{
		Rendering();
	}

	private void Initialization()
	{
		lineRenderer = GetComponent<LineRenderer>();
		OriginalPosition.Clear();
		for (int i = 0; i < Fineness; i++)
		{
			Vector3 item = new Vector3(0f, (float)(-i) * length / (float)Fineness, 0f);
			OriginalPosition.Add(item);
		}
		lineRenderer.positionCount = Fineness;
		lineRenderer.SetPositions(OriginalPosition.ToArray());
		for (int j = 0; j < OriginalPosition.Count; j++)
		{
			Vector3 vector = OriginalPosition[j];
			Particle particle = new Particle
			{
				position = vector,
				oldPosition = vector
			};
			if (generateGameobject)
			{
				GameObject gameObject = (particle.gameobject = new GameObject());
				gameObject.transform.localScale = new Vector3(radius, radius, radius);
				gameObject.transform.SetParent(parent);
				gameObject.transform.localPosition = particle.position;
				gameObject.name = "RopePart";
				CircleCollider2D circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
				circleCollider2D.isTrigger = true;
				circleCollider2D.radius = 1f;
			}
			particles.Add(particle);
		}
		particles[particles.Count - 1].gameobject.transform.position += new Vector3(0.1f, 0f, 0f);
	}

	public void Simulation()
	{
		SimulatePositionGravity();
		for (int i = 0; i < Seperate.Count - 1; i++)
		{
			SimulatePullPush(Seperate[i], Seperate[i + 1]);
		}
		if (isHair)
		{
			if (generateGameobject)
			{
				particles[0].gameobject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
			else
			{
				particles[0].position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		void SimulatePositionGravity()
		{
			for (int l = 0; l < particles.Count; l++)
			{
				Particle particle = particles[l];
				if (l == 0)
				{
					particle.position = particle.gameobject.transform.localPosition;
					particle.oldPosition = particle.position;
					particle.gameobject.transform.localPosition = particle.position;
				}
				if (!particle.locked)
				{
					if (generateGameobject)
					{
						particle.position = particle.gameobject.transform.localPosition;
						Vector2 position = particle.position;
						particle.position = particle.position + (particle.position - particle.oldPosition) + Time.fixedDeltaTime * Time.fixedDeltaTime * new Vector2(0f, 0f - gravity);
						particle.oldPosition = position;
						particle.gameobject.transform.localPosition = particle.position;
					}
					else
					{
						Vector2 position2 = particle.position;
						particle.position = particle.position + (particle.position - particle.oldPosition) + Time.fixedDeltaTime * Time.fixedDeltaTime * new Vector2(0f, 0f - gravity);
						particle.oldPosition = position2;
					}
				}
			}
		}
		void SimulatePullPush(int start, int end)
		{
			for (int j = 0; j < stiffness; j++)
			{
				for (int k = start; k < end; k++)
				{
					Stick stick = sticks[k];
					Vector2 vector = stick.particleB.position - stick.particleA.position;
					float magnitude = vector.magnitude;
					float num = (magnitude - stick.length) / magnitude;
					if (!stick.particleA.locked)
					{
						stick.particleA.position += 0.3f * num * vector;
					}
					if (!stick.particleB.locked)
					{
						stick.particleB.position -= 0.7f * num * vector;
					}
					if (generateGameobject)
					{
						stick.particleA.gameobject.transform.localPosition = stick.particleA.position;
						stick.particleB.gameobject.transform.localPosition = stick.particleB.position;
					}
				}
			}
		}
	}

	private void Rendering()
	{
		if (generateGameobject)
		{
			for (int i = 0; i < particles.Count; i++)
			{
				lineRenderer.SetPosition(i, particles[i].gameobject.transform.localPosition);
			}
		}
		else
		{
			for (int j = 0; j < particles.Count; j++)
			{
				lineRenderer.SetPosition(j, particles[j].position);
			}
		}
		for (int k = 0; k < particles.Count - 1; k++)
		{
			sticks.Add(new Stick(particles[k], particles[k + 1]));
		}
		if (startPointLock)
		{
			particles[0].locked = true;
		}
		if (endPointLock)
		{
			particles[particles.Count - 1].locked = true;
		}
		if (isHair)
		{
			particles[0].locked = true;
		}
		particles[particles.Count - 1].gameobject.transform.LookAt(particles[particles.Count - 2].gameobject.transform);
		endGameobject.gameObject.transform.LookAt(particles[particles.Count - 2].gameobject.transform);
		if (endGameobject.gameObject.transform.rotation.eulerAngles.y != 90f)
		{
			_ = endGameobject.gameObject.transform.rotation.eulerAngles.y;
			_ = -90f;
		}
	}

	public void ClearSeperate()
	{
		Seperate = new List<int>
		{
			0,
			Fineness - 1
		};
	}
}
