using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftDetail : MonoBehaviour {
	[SerializeField] RawImage[] itemIconList;
	[SerializeField] Text[] itemAmountList;

	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

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
				itemIconList[i].texture = ItemList.GetTexture ( r.Materials[i] );
				itemAmountList[i].text = r.MaterialAmountList[i].ToString ();
			}
		}
	}
}
