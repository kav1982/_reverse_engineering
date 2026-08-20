using System.Collections.Generic;
using UnityEngine;

public class SpecialObj209 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public int space;

	public SpecialObj209Box pfb_bluebox;

	public SpecialObj209Box pfb_redbox;

	public SpecialObj209Box pfb_greenbox;

	public SpecialObj209Button pfb_button;

	public SpecialObj1 pfb_block;

	private List<SpecialObj209Box> blueBoxs = new List<SpecialObj209Box>();

	private List<SpecialObj209Box> redBoxs = new List<SpecialObj209Box>();

	private List<SpecialObj209Box> greenBoxs = new List<SpecialObj209Box>();

	private List<SpecialObj209Button> Buttons = new List<SpecialObj209Button>();

	private RoomController belongCtrller;

	private int xoffset;

	private void Start()
	{
		CreateMap();
		foreach (Transform item in belongCtrller.tsf_Thing.transform)
		{
			if (item.gameObject.GetComponent<SpecialObj205>() != null)
			{
				item.gameObject.GetComponent<SpecialObj205>().OnGameClear += GameEnd;
				break;
			}
		}
	}

	private void CreateMap()
	{
		int[,] array = new int[11, 15]
		{
			{
				0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 7, 0, 1, 0, 0, 0, 2, 0, 9,
				0, 1, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				4, 4, 4, 0, 3, 3, 3, 0, 4, 4,
				4, 0, 6, 6, 6
			},
			{
				0, 0, 0, 5, 0, 0, 0, 3, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 0, 0, 5, 0, 8, 0, 3, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 0, 0, 5, 0, 0, 0, 3, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				4, 4, 4, 0, 4, 4, 4, 0, 1, 1,
				1, 0, 6, 6, 6
			},
			{
				0, 0, 0, 2, 0, 0, 0, 1, 0, 0,
				0, 4, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 1, 0, 0,
				0, 4, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 1, 0, 0,
				0, 4, 0, 0, 0
			}
		};
		int[,] array2 = new int[11, 15]
		{
			{
				0, 0, 0, 2, 0, 0, 0, 2, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 2, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 2, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				5, 5, 5, 0, 3, 3, 3, 0, 1, 1,
				1, 0, 2, 2, 2
			},
			{
				0, 0, 0, 1, 0, 0, 0, 3, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 7, 0, 3, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 3, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				4, 4, 4, 0, 4, 4, 4, 0, 5, 5,
				5, 0, 6, 6, 6
			},
			{
				0, 0, 0, 2, 0, 0, 0, 1, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 8, 0, 1, 0, 9,
				0, 2, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 1, 0, 0,
				0, 2, 0, 0, 0
			}
		};
		int[,] array3 = new int[11, 15]
		{
			{
				0, 0, 0, 1, 0, 0, 0, 6, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 9, 0, 1, 0, 0, 0, 6, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 6, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				6, 6, 6, 0, 5, 5, 5, 0, 4, 4,
				4, 0, 3, 3, 3
			},
			{
				0, 0, 0, 3, 0, 0, 0, 1, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 0, 0, 3, 0, 8, 0, 1, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 0, 0, 3, 0, 0, 0, 1, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				2, 2, 2, 0, 4, 4, 4, 0, 3, 3,
				3, 0, 5, 5, 5
			},
			{
				0, 0, 0, 1, 0, 0, 0, 5, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 7, 0, 1, 0, 0, 0, 5, 0, 0,
				0, 2, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 5, 0, 0,
				0, 2, 0, 0, 0
			}
		};
		int[,] array4 = new int[11, 15]
		{
			{
				0, 0, 0, 6, 0, 0, 0, 5, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 8, 0, 6, 0, 0, 0, 5, 0, 9,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 6, 0, 0, 0, 5, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				1, 1, 1, 0, 3, 3, 3, 0, 4, 4,
				4, 0, 1, 1, 1
			},
			{
				0, 0, 0, 4, 0, 0, 0, 6, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 4, 0, 0, 0, 6, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 4, 0, 0, 0, 6, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				3, 3, 3, 0, 2, 2, 2, 0, 5, 5,
				5, 0, 6, 6, 6
			},
			{
				0, 0, 0, 5, 0, 0, 0, 1, 0, 0,
				0, 4, 0, 0, 0
			},
			{
				0, 0, 0, 5, 0, 0, 0, 1, 0, 7,
				0, 4, 0, 0, 0
			},
			{
				0, 0, 0, 5, 0, 0, 0, 1, 0, 0,
				0, 4, 0, 0, 0
			}
		};
		int[,] array5 = new int[11, 15]
		{
			{
				0, 0, 0, 1, 0, 0, 0, 5, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 9, 0, 1, 0, 7, 0, 5, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 5, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				2, 2, 2, 0, 3, 3, 3, 0, 4, 4,
				4, 0, 4, 4, 4
			},
			{
				0, 0, 0, 6, 0, 0, 0, 5, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 6, 0, 0, 0, 5, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 6, 0, 0, 0, 5, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				4, 4, 4, 0, 4, 4, 4, 0, 3, 3,
				3, 0, 2, 2, 2
			},
			{
				0, 0, 0, 3, 0, 0, 0, 5, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 9, 0, 3, 0, 8, 0, 5, 0, 0,
				0, 1, 0, 0, 0
			},
			{
				0, 0, 0, 3, 0, 0, 0, 5, 0, 0,
				0, 1, 0, 0, 0
			}
		};
		int[,] array6 = new int[11, 15]
		{
			{
				0, 0, 0, 4, 0, 0, 0, 2, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 8, 0, 4, 0, 0, 0, 2, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 4, 0, 0, 0, 2, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				5, 5, 5, 0, 5, 5, 5, 0, 2, 2,
				2, 0, 2, 2, 2
			},
			{
				0, 0, 0, 4, 0, 0, 0, 6, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 4, 0, 9, 0, 6, 0, 7,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 4, 0, 0, 0, 6, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				1, 1, 1, 0, 4, 4, 4, 0, 3, 3,
				3, 0, 5, 5, 5
			},
			{
				0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
				0, 6, 0, 0, 0
			}
		};
		int[,] array7 = new int[11, 15]
		{
			{
				0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 7, 0, 1, 0, 9, 0, 2, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				4, 4, 4, 0, 3, 3, 3, 0, 4, 4,
				4, 0, 4, 4, 4
			},
			{
				0, 0, 0, 2, 0, 0, 0, 3, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 3, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				0, 0, 0, 2, 0, 0, 0, 3, 0, 0,
				0, 6, 0, 0, 0
			},
			{
				6, 6, 6, 0, 5, 5, 5, 0, 2, 2,
				2, 0, 2, 2, 2
			},
			{
				0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 8, 0, 1, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
				0, 5, 0, 0, 0
			}
		};
		int[,] array8 = new int[11, 15]
		{
			{
				0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
				0, 4, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 9, 0, 2, 0, 0,
				0, 4, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
				0, 4, 0, 0, 0
			},
			{
				3, 3, 3, 0, 3, 3, 3, 0, 1, 1,
				1, 0, 2, 2, 2
			},
			{
				0, 0, 0, 1, 0, 0, 0, 4, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 8, 0, 4, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				0, 0, 0, 1, 0, 0, 0, 4, 0, 0,
				0, 3, 0, 0, 0
			},
			{
				2, 2, 2, 0, 6, 6, 6, 0, 5, 5,
				5, 0, 4, 4, 4
			},
			{
				0, 0, 0, 6, 0, 0, 0, 1, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 6, 0, 7, 0, 1, 0, 0,
				0, 5, 0, 0, 0
			},
			{
				0, 0, 0, 6, 0, 0, 0, 1, 0, 0,
				0, 5, 0, 0, 0
			}
		};
		int[,] array9 = new int[11, 15];
		array9 = Random.Range(0, 8) switch
		{
			0 => array, 
			1 => array2, 
			2 => array3, 
			3 => array4, 
			4 => array5, 
			5 => array6, 
			6 => array7, 
			7 => array8, 
			_ => array, 
		};
		if (belongCtrller.roomCfg.isFlipped)
		{
			xoffset = -1;
		}
		else
		{
			xoffset = 1;
		}
		for (int i = 0; i < 15; i++)
		{
			for (int j = -4; j < 7; j++)
			{
				switch (array9[10 - j - 4, i])
				{
				case 1:
				{
					SpecialObj209Box component9 = Object.Instantiate(pfb_bluebox, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Box>();
					component9.Up();
					blueBoxs.Add(component9);
					break;
				}
				case 2:
				{
					SpecialObj209Box component8 = Object.Instantiate(pfb_bluebox, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Box>();
					component8.Down();
					blueBoxs.Add(component8);
					break;
				}
				case 3:
				{
					SpecialObj209Box component7 = Object.Instantiate(pfb_redbox, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Box>();
					component7.Up();
					redBoxs.Add(component7);
					break;
				}
				case 4:
				{
					SpecialObj209Box component6 = Object.Instantiate(pfb_redbox, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Box>();
					component6.Down();
					redBoxs.Add(component6);
					break;
				}
				case 5:
				{
					SpecialObj209Box component5 = Object.Instantiate(pfb_greenbox, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Box>();
					component5.Up();
					greenBoxs.Add(component5);
					break;
				}
				case 6:
				{
					SpecialObj209Box component4 = Object.Instantiate(pfb_greenbox, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Box>();
					component4.Down();
					greenBoxs.Add(component4);
					break;
				}
				case 7:
				{
					SpecialObj209Button component3 = Object.Instantiate(pfb_button, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Button>();
					component3.Initialize(this, SpecialObj209Button.ButtonColor.Blue);
					Buttons.Add(component3);
					break;
				}
				case 8:
				{
					SpecialObj209Button component2 = Object.Instantiate(pfb_button, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Button>();
					component2.Initialize(this, SpecialObj209Button.ButtonColor.Red);
					Buttons.Add(component2);
					break;
				}
				case 9:
				{
					SpecialObj209Button component = Object.Instantiate(pfb_button, base.transform.position + new Vector3(i * xoffset, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj209Button>();
					component.Initialize(this, SpecialObj209Button.ButtonColor.Green);
					Buttons.Add(component);
					break;
				}
				}
			}
		}
	}

	public void BlueChange()
	{
		for (int i = 0; i < blueBoxs.Count; i++)
		{
			if (blueBoxs[i] != null)
			{
				if (blueBoxs[i].isdown)
				{
					blueBoxs[i].Up();
				}
				else
				{
					blueBoxs[i].Down();
				}
			}
		}
	}

	public void RedChange()
	{
		for (int i = 0; i < redBoxs.Count; i++)
		{
			if (redBoxs[i] != null)
			{
				if (redBoxs[i].isdown)
				{
					redBoxs[i].Up();
				}
				else
				{
					redBoxs[i].Down();
				}
			}
		}
	}

	public void GreenChange()
	{
		for (int i = 0; i < greenBoxs.Count; i++)
		{
			if (greenBoxs[i] != null)
			{
				if (greenBoxs[i].isdown)
				{
					greenBoxs[i].Up();
				}
				else
				{
					greenBoxs[i].Down();
				}
			}
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}

	public void GameEnd()
	{
		for (int i = 0; i < greenBoxs.Count; i++)
		{
			if (greenBoxs[i] != null && !greenBoxs[i].isdown)
			{
				greenBoxs[i].Down();
			}
		}
		for (int j = 0; j < blueBoxs.Count; j++)
		{
			if (blueBoxs[j] != null && !blueBoxs[j].isdown)
			{
				blueBoxs[j].Down();
			}
		}
		for (int k = 0; k < redBoxs.Count; k++)
		{
			if (redBoxs[k] != null && !redBoxs[k].isdown)
			{
				redBoxs[k].Down();
			}
		}
		for (int l = 0; l < Buttons.Count; l++)
		{
			Buttons[l].SetInvalid();
		}
	}
}
