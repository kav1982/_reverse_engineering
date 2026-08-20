using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public static class AIDataMgr
{
	private static EntityQuery _itemQuery;

	private static string _urlPrefix;

	private static HttpListener _listener;

	private static readonly Dictionary<string, (string help, Func<string> func)> _apis = new Dictionary<string, (string, Func<string>)>
	{
		{
			"/spells",
			("获取背包和法杖上的法术，没有法术的格子为null，被多格法术占用的格子为 \"Seal\"", GetSpells)
		},
		{
			"/store",
			("获取房间内在售物品，按照从左向右、从上到下排序", GetStoreGoods)
		},
		{
			"/mp",
			("获取每个法杖的蓝量", GetMp)
		}
	};

	public static void StartHttpServer(int port)
	{
		_itemQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(Item));
		HttpListener listener = _listener;
		if (listener != null && listener.IsListening)
		{
			_listener.Stop();
		}
		_listener = new HttpListener();
		_urlPrefix = $"http://localhost:{port}/";
		_listener.Prefixes.Add(_urlPrefix);
		_listener.Start();
		_listener.BeginGetContext(OnRequest, null);
	}

	private static void OnRequest(IAsyncResult ar)
	{
		if (_listener.IsListening)
		{
			HttpListenerContext httpListenerContext = _listener.EndGetContext(ar);
			_listener.BeginGetContext(OnRequest, null);
			(string, Func<string>) value;
			string text = (_apis.TryGetValue(httpListenerContext.Request.Url.LocalPath, out value) ? value.Item2() : IndexPage());
			httpListenerContext.Response.ContentType = (text.StartsWith("{") ? "application/json" : "text/html");
			httpListenerContext.Response.OutputStream.Write(Encoding.UTF8.GetBytes(text));
			httpListenerContext.Response.Close();
		}
	}

	private static string IndexPage()
	{
		string text = "<html><head><meta charset=\"UTF-8\"></head><body>\n";
		foreach (KeyValuePair<string, (string, Func<string>)> api in _apis)
		{
			api.Deconstruct(out var key, out var value);
			string text2 = key;
			(string, Func<string>) tuple = value;
			text = text + "<a href=\"" + text2 + "\">" + text2 + "</a>" + tuple.Item1 + "<br>\n";
		}
		return text + "</body></html>";
	}

	private static string GetSpells()
	{
		return JsonConvert.SerializeObject(new Dictionary<string, object>
		{
			{
				"Bag",
				GetSpellSlotDataArray(PlayerMgr.Inst.BaData.bagSpellDatas, new bool[PlayerMgr.Inst.BaData.bagSpellDatas.Count])
			},
			{
				"Wands",
				((IEnumerable<Wand>)PlayerMgr.Inst.Wands).Select((Func<Wand, object>)((Wand wand) => (wand == null || wand.WandCfg == null) ? null : new Dictionary<string, object>
				{
					{
						"id",
						wand.WandCfg.id
					},
					{
						"name",
						wand.WandCfg.GetName()
					},
					{
						"max_mp",
						wand.WandCfg.maxMP
					},
					{
						"mp_recover",
						wand.GetWandMpRecoverSpeed()
					},
					{
						"shoot_interval",
						wand.WandCfg.shootInterval
					},
					{
						"cooldown",
						wand.WandCfg.coolDown
					},
					{
						"normal_slots",
						GetSpellSlotDataArray(wand.WandCfg.normalSlots, wand.WandCfg.normalSlotIsLock)
					},
					{
						"post_slots",
						GetSpellSlotDataArray(wand.WandCfg.postSlots, wand.WandCfg.postSlotIsLock)
					}
				}))
			}
		});
	}

	private static object[] GetSpellSlotDataArray(IEnumerable<SlotData> slots, IEnumerable<bool> locks)
	{
		return slots.Zip(locks, (Func<SlotData, bool, object>)delegate(SlotData slot, bool l)
		{
			if (slot == null)
			{
				return null;
			}
			return slot.isSealSlot ? ((IEnumerable)"Seal") : ((IEnumerable)new Dictionary<string, object>
			{
				{ "id", slot.id },
				{
					"name",
					slot.GetConfigIgnoreMimic().GetName()
				},
				{ "lock", l }
			});
		}).ToArray();
	}

	private static string GetStoreGoods()
	{
		EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
		using NativeArray<Entity> nativeArray = _itemQuery.ToEntityArray(Allocator.Domain);
		Vector2Int currentRoomMapPos = LevelMgr.Inst.CurrentRoomMapPos;
		return JsonConvert.SerializeObject(new Dictionary<string, object> { 
		{
			"store",
			(from e in nativeArray
				select (em.GetComponentData<Item>(e), em.GetComponentData<LocalToWorld>(e)) into e
				where e.Item1.isStore && e.Item1.belongRoomMapPos == currentRoomMapPos
				orderby -(int)(e.Item2.Position.y * 100f), (int)(e.Item2.Position.x * 100f)
				select GetItemData(e.Item1)).ToArray()
		} });
	}

	private static object GetItemData(Item item)
	{
		string value = item.info.type switch
		{
			ItemType.Potion => (3000000 + item.info.id).GetText(), 
			ItemType.Relic => (4000000 + item.info.id).GetText(), 
			ItemType.Wand => (5000000 + item.info.id).GetText(), 
			ItemType.Spell => (7000000 + item.info.id).GetText(), 
			ItemType.Resource => (6000000 + item.info.id).GetText(), 
			_ => "", 
		};
		return new Dictionary<string, object>
		{
			{ "name", value },
			{
				"id",
				item.info.id
			},
			{
				"type",
				item.info.type
			},
			{
				"price",
				item.GetPrice(considerDiscount: true)
			}
		};
	}

	private static string GetMp()
	{
		return JsonConvert.SerializeObject(PlayerMgr.Inst.Wands.Select((Wand e) => (e == null || e.WandCfg == null) ? null : new Dictionary<string, object>
		{
			{ "MP", e.CurrentMP },
			{ "MaxMP", e.MaxMP },
			{
				"RecoverMPSpeed",
				e.GetWandMpRecoverSpeed()
			}
		}).ToArray());
	}
}
