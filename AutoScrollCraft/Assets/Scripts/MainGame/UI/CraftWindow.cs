using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class CraftWindow : MonoBehaviour {
		private CraftDetail craftDetail;
		[SerializeField] private RawImage[] itemSlots;
		[SerializeField] private GameObject detail;
		public GameObject Detail { get => detail; }

		private void Start () {
			craftDetail = detail.GetComponent<CraftDetail> ();
		}

		public void UpdateCraftUI ( Player player ) {
			for (int i = 0; i < itemSlots.Length; i++) {
				// 選択中のレシピとその前後のレシピを表示する
				var n = player.CurrentSelectOnRecipe;
				n = UIFunctions.RevisionValue ( n - 2 + i, Craft.MaxRecipeNumber, UIFunctions.RevisionMode.Loop );
				itemSlots[i].texture = ItemList.Instance.GetTexture ( Craft.Recipes[n].Result );
			}

			craftDetail.UpdateUI ( player );
		}
	}
}
