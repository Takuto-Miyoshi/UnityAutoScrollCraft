using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class CraftDetail : MonoBehaviour {
		[SerializeField] private RawImage[] itemIconList;
		[SerializeField] private Text[] itemAmountList;

		public void UpdateUI ( Player player ) {
			foreach (var i in itemIconList) {
				i.gameObject.SetActive ( true );
			}

			var r = Craft.Recipes[player.CurrentSelectOnRecipe];
			for (int i = 0; i < itemIconList.Length; i++) {
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
