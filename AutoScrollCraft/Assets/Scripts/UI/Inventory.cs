using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Enums;
using UnityEngine;
using UnityEngine.UI;

// InventoryといいながらInventoryのUIだけ管理する

public class Inventory : MonoBehaviour {
	[SerializeField] RawImage[] itemSlots;
	[SerializeField] Text[] itemNumTexts;
	static string[] itemNames;
	static Texture[] textures;
	static bool loaded = false;
	[SerializeField] RectTransform cursor;

	// Start is called before the first frame update
	void Start () {
		// アイテムの画像をロードする
		if (loaded == false) {
			loaded = true;
			Array.Resize ( ref textures, System.Enum.GetValues ( typeof ( Items ) ).Length );
			itemNames = System.Enum.GetNames ( typeof ( Items ) );
			for (int i = 0; i < itemNames.Length; i++) {
				textures[i] = (Texture)Resources.Load ( "Textures/UI/ItemIcons/" + itemNames[i] );
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}

	// インベントリUIを更新
	public void UpdateInventoryUI ( Player player ) {
		var l = itemNames.ToList ();

		for (int n = 0; n < player.Inventory.Length; n++) {
			// １個以上持っているなら個数表示を更新
			if (player.Inventory[n].Volume >= 1) {
				itemNumTexts[n].text = player.Inventory[n].Volume.ToString ();
			}
			// 所持数0以下なら個数表示を消してアイテムを空にする
			else if (player.Inventory[n].Volume <= 0) {
				itemNumTexts[n].text = "";
				player.Inventory[n].Item = Items.Null;
				player.Inventory[n].Volume = 0;
			}

			// テクスチャを更新
			var i = l.FindIndex ( x => x.ToString () == player.Inventory[n].Item.ToString () );
			itemSlots[n].texture = textures[i];
		}
	}

	public void UpdateCursorUI ( Player player ) {
		var pos = cursor.localPosition;
		pos.x = player.CurrentSelect * cursor.sizeDelta.x;
		cursor.localPosition = pos;
	}
}
