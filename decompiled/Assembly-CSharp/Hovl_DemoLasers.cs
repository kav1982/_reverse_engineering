using UnityEngine;

public class Hovl_DemoLasers : MonoBehaviour
{
	public GameObject FirePoint;

	public Camera Cam;

	public float MaxLength;

	public GameObject[] Prefabs;

	private Ray RayMouse;

	private Vector3 direction;

	private Quaternion rotation;

	[Header("GUI")]
	private float windowDpi;

	private int Prefab;

	private GameObject Instance;

	private Hovl_Laser LaserScript;

	private Hovl_Laser2 LaserScript2;

	private float buttonSaver;

	private void Start()
	{
		if (Screen.dpi < 1f)
		{
			windowDpi = 1f;
		}
		if (Screen.dpi < 200f)
		{
			windowDpi = 1f;
		}
		else
		{
			windowDpi = Screen.dpi / 200f;
		}
		Counter(0);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Object.Destroy(Instance);
			Instance = Object.Instantiate(Prefabs[Prefab], FirePoint.transform.position, FirePoint.transform.rotation);
			Instance.transform.parent = base.transform;
			LaserScript = Instance.GetComponent<Hovl_Laser>();
			LaserScript2 = Instance.GetComponent<Hovl_Laser2>();
		}
		if (Input.GetMouseButtonUp(0))
		{
			if ((bool)LaserScript)
			{
				LaserScript.DisablePrepare();
			}
			if ((bool)LaserScript2)
			{
				LaserScript2.DisablePrepare();
			}
			Object.Destroy(Instance, 1f);
		}
		if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0f) && buttonSaver >= 0.4f)
		{
			buttonSaver = 0f;
			Counter(-1);
		}
		if ((Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0f) && buttonSaver >= 0.4f)
		{
			buttonSaver = 0f;
			Counter(1);
		}
		buttonSaver += Time.deltaTime;
		if (Cam != null)
		{
			Vector3 mousePosition = Input.mousePosition;
			RayMouse = Cam.ScreenPointToRay(mousePosition);
			if (Physics.Raycast(RayMouse.origin, RayMouse.direction, out var hitInfo, MaxLength))
			{
				RotateToMouseDirection(base.gameObject, hitInfo.point);
				return;
			}
			Vector3 point = RayMouse.GetPoint(MaxLength);
			RotateToMouseDirection(base.gameObject, point);
		}
		else
		{
			Debug.Log("No camera");
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f * windowDpi, 5f * windowDpi, 400f * windowDpi, 20f * windowDpi), "Use the keyboard buttons A/<- and D/-> to change lazers!");
		GUI.Label(new Rect(10f * windowDpi, 20f * windowDpi, 400f * windowDpi, 20f * windowDpi), "Use left mouse button for shooting!");
	}

	private void Counter(int count)
	{
		Prefab += count;
		if (Prefab > Prefabs.Length - 1)
		{
			Prefab = 0;
		}
		else if (Prefab < 0)
		{
			Prefab = Prefabs.Length - 1;
		}
	}

	private void RotateToMouseDirection(GameObject obj, Vector3 destination)
	{
		direction = destination - obj.transform.position;
		rotation = Quaternion.LookRotation(direction);
		obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1f);
	}
}
