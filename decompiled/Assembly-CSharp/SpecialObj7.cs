using System.Collections.Generic;
using UnityEngine;

public class SpecialObj7 : LayerCorrect
{
	[Space(50f)]
	public MeshRenderer mr;

	public Sprite sprite_Up;

	public Sprite sprite_UpRight;

	public Sprite sprite_UpDown;

	public Sprite sprite_LeftUpRight;

	public Sprite sprite_Full;

	public SpecialObj7 UpTrack { get; private set; }

	public SpecialObj7 RightTrack { get; private set; }

	public SpecialObj7 DownTrack { get; private set; }

	public SpecialObj7 LeftTrack { get; private set; }

	public bool IsInitialized { get; private set; }

	private void Start()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (IsInitialized)
		{
			return;
		}
		IsInitialized = true;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, 1f, "SpikesTrack");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].transform.position == base.transform.position + new Vector3(0f, 1f, 0f))
			{
				UpTrack = collidersByTag[i].GetComponent<SpecialObj7>();
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(1f, 0f, 0f))
			{
				RightTrack = collidersByTag[i].GetComponent<SpecialObj7>();
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(0f, -1f, 0f))
			{
				DownTrack = collidersByTag[i].GetComponent<SpecialObj7>();
			}
			else if (collidersByTag[i].transform.position == base.transform.position + new Vector3(-1f, 0f, 0f))
			{
				LeftTrack = collidersByTag[i].GetComponent<SpecialObj7>();
			}
		}
		if (!(UpTrack == null) || !(RightTrack == null) || !(DownTrack == null) || !(LeftTrack == null))
		{
			if (UpTrack != null && RightTrack == null && DownTrack == null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Up.texture);
			}
			else if (UpTrack == null && RightTrack != null && DownTrack == null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Up.texture);
				mr.transform.rotation = Tool2D.GetRotation(270f);
			}
			else if (UpTrack == null && RightTrack == null && DownTrack != null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Up.texture);
				mr.transform.rotation = Tool2D.GetRotation(180f);
			}
			else if (UpTrack == null && RightTrack == null && DownTrack == null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Up.texture);
				mr.transform.rotation = Tool2D.GetRotation(90f);
			}
			else if (UpTrack != null && RightTrack != null && DownTrack == null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_UpRight.texture);
			}
			else if (UpTrack == null && RightTrack != null && DownTrack != null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_UpRight.texture);
				mr.transform.rotation = Tool2D.GetRotation(270f);
			}
			else if (UpTrack == null && RightTrack == null && DownTrack != null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_UpRight.texture);
				mr.transform.rotation = Tool2D.GetRotation(180f);
			}
			else if (UpTrack != null && RightTrack == null && DownTrack == null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_UpRight.texture);
				mr.transform.rotation = Tool2D.GetRotation(90f);
			}
			else if (UpTrack != null && RightTrack == null && DownTrack != null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_UpDown.texture);
			}
			else if (UpTrack == null && RightTrack != null && DownTrack == null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_UpDown.texture);
				mr.transform.rotation = Tool2D.GetRotation(90f);
			}
			else if (UpTrack != null && RightTrack != null && DownTrack == null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_LeftUpRight.texture);
			}
			else if (UpTrack != null && RightTrack != null && DownTrack != null && LeftTrack == null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_LeftUpRight.texture);
				mr.transform.rotation = Tool2D.GetRotation(270f);
			}
			else if (UpTrack == null && RightTrack != null && DownTrack != null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_LeftUpRight.texture);
				mr.transform.rotation = Tool2D.GetRotation(180f);
			}
			else if (UpTrack != null && RightTrack == null && DownTrack != null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_LeftUpRight.texture);
				mr.transform.rotation = Tool2D.GetRotation(90f);
			}
			else if (UpTrack != null && RightTrack != null && DownTrack != null && LeftTrack != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Full.texture);
			}
		}
	}
}
