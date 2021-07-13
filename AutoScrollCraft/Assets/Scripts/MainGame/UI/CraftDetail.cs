using System.Collections.Generic;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class CraftDetail : MonoBehaviour {
		[SerializeField] private List<RawImage> itemIconList;
		[SerializeField] private Text[] itemAmountList;

		public void UpdateUI ( Player player ) {
			itemIconList.ForEach ( x => x.gameObject.SetActive ( true ) );

			// 選択中のレシピ情報を表示
			var r = Craft.Recipes[player.CurrentSelectOnRecipe];
			for (int i = 0; i < itemIconList.Count; i++) {
				// 必要ない分は非表示
				if (i >= r.Materials.Count) {
					itemIconList[i].gameObject.SetActive ( false );
				}
				else {
					itemIconList[i].texture = ItemList.Instance.GetTexture ( r.Materials[i] );
					itemAmountList[i].text = r.MaterialAmountList[i].ToString ();
				}
			}
		}
	}
}
