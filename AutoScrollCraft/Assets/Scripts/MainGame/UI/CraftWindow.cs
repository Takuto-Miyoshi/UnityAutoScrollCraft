using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class CraftWindow : MonoBehaviour {
		[SerializeField] private RawImage[] itemSlots;
		[SerializeField] private GameObject detail;
		private CraftDetail craftDetail;

		public GameObject Detail { get => detail; }

		// Start is called before the first frame update
		private void Start () {
			craftDetail = detail.GetComponent<CraftDetail> ();
		}

		public void UpdateCraftUI ( Player player ) {
			for (int i = 0; i < itemSlots.Length; i++) {
				var n = player.CurrentSelectOnRecipe;
				n = UIFunctions.RevisionValue ( n - 2 + i, Craft.MaxRecipeNumber, UIFunctions.RevisionMode.Loop );
				itemSlots[i].texture = ItemList.Instance.GetTexture ( Craft.Recipes[n].Result );
			}

			craftDetail.UpdateUI ( player );
		}
	}
}
