using System.Linq;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class Inventory : MonoBehaviour {
		[SerializeField] private RawImage[] itemSlots;
		[SerializeField] private Text[] itemNumberString;
		[SerializeField] private RectTransform cursor;

		// インベントリUIを更新
		public void UpdateInventoryUI ( Player player ) {
			var itemNameList = ItemList.Instance.ItemNameList.ToList ();

			for (int n = 0; n < player.Inventory.Length; n++) {
				// １個以上持っているなら個数表示を更新
				if (player.Inventory[n].Amount >= 1) {
					itemNumberString[n].text = player.Inventory[n].Amount.ToString ();
				}
				// 所持数0以下なら個数表示を消してアイテムを空にする
				else if (player.Inventory[n].Amount <= 0) {
					itemNumberString[n].text = "";
					player.Inventory[n].Item = Enums.Items.Null;
					player.Inventory[n].Amount = 0;
				}

				// テクスチャを更新
				var i = itemNameList.FindIndex ( x => x == player.Inventory[n].Item.ToString () );
				itemSlots[n].texture = ItemList.Instance.Images[i];
			}
		}

		public void UpdateCursorUI ( Player player ) {
			var pos = cursor.localPosition;
			pos.x = player.CurrentSelectOnInventory * cursor.sizeDelta.x;
			cursor.localPosition = pos;
		}
	}
}