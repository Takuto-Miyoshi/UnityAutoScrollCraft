using System.Linq;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class Inventory : MonoBehaviour {
		[SerializeField] private RawImage[] itemSlots;
		[SerializeField] private Text[] itemNumTexts;
		[SerializeField] private RectTransform cursor;

		// インベントリUIを更新
		public void UpdateInventoryUI ( Player player ) {
			var l = ItemList.Instance.Names.ToList ();

			for (int n = 0; n < player.Inventory.Length; n++) {
				// １個以上持っているなら個数表示を更新
				if (player.Inventory[n].Volume >= 1) {
					itemNumTexts[n].text = player.Inventory[n].Volume.ToString ();
				}
				// 所持数0以下なら個数表示を消してアイテムを空にする
				else if (player.Inventory[n].Volume <= 0) {
					itemNumTexts[n].text = "";
					player.Inventory[n].Item = Enums.Items.Null;
					player.Inventory[n].Volume = 0;
				}

				// テクスチャを更新
				var i = l.FindIndex ( x => x == player.Inventory[n].Item.ToString () );
				itemSlots[n].texture = ItemList.Instance.Textures[i];
			}
		}

		public void UpdateCursorUI ( Player player ) {
			var pos = cursor.localPosition;
			pos.x = player.CurrentSelectOnInventory * cursor.sizeDelta.x;
			cursor.localPosition = pos;
		}
	}
}