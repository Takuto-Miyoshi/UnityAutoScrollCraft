using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class CraftWindow : MonoBehaviour {
		[SerializeField] RawImage[] itemSlots;
		[SerializeField] GameObject detail;
		CraftDetail craftDetail;

		public GameObject Detail { get => detail; }

		// Start is called before the first frame update
		void Start () {
			craftDetail = detail.GetComponent<CraftDetail> ();
		}

		// Update is called once per frame
		void Update () {

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
