using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Enums;
using TreeEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

public class Craft : MonoBehaviour {
	public struct CraftData {
		List<Items> materials;  // 素材
		public List<Items> Materials {
			get { return materials; }
			set { materials = value; }
		}
		List<int> materialAmountList;   // 素材数
		public List<int> MaterialAmountList {
			get { return materialAmountList; }
			set { materialAmountList = value; }
		}
		Items result;   // 生成物
		public Items Result {
			get { return result; }
			set { result = value; }
		}
		int resultAmount;   // 生成量
		public int ResultAmount {
			get { return resultAmount; }
			set { resultAmount = value; }
		}
		public void Reset () {
			materials = new List<Items> ();
			MaterialAmountList = new List<int> ();
			result = Items.Wood;
			resultAmount = 1;
		}
	}
	static CraftData tmpRecipe; // 下書き
	static List<CraftData> recipes = new List<CraftData> (); // レシピ集
	static public List<CraftData> Recipes {
		get { return recipes; }
	}
	static int maxRecipeNumber;
	static public int MaxRecipeNumber {
		get { return maxRecipeNumber; }
	}
	static bool loaded = false;

	// Start is called before the first frame update
	void Start () {
		if (loaded == false) {
			loaded = true;
			tmpRecipe.Reset ();
			// ----------ここからレシピ-------------------
			AddMaterial ( Items.Wood, 2 );
			AddMaterial ( Items.Stone, 2 );
			SetResult ( Items.Pork, 5 );
			AddRecipe ();
			AddMaterial ( Items.Wood, 1 );
			SetResult ( Items.Stone, 3 );
			AddRecipe ();
			AddMaterial ( Items.Wood, 1 );
			SetResult ( Items.Wood, 2 );
			AddRecipe ();
			// ------------------------------------------
			maxRecipeNumber = recipes.ToArray ().Length - 1;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	// 下書きのレシピを追加
	static void AddRecipe () {
		recipes.Add ( tmpRecipe );
		tmpRecipe.Reset ();
	}

	// 下書きに素材を追加
	static void AddMaterial ( Items item, int value ) {
		tmpRecipe.Materials.Add ( item );
		tmpRecipe.MaterialAmountList.Add ( value );
	}

	// 下書きの生成物を設定
	static void SetResult ( Items item, int value ) {
		tmpRecipe.Result = item;
		tmpRecipe.ResultAmount = value;
	}

	// アイテムを作れるか
	static public bool CanBeCrafting ( Player.ItemData[] inventory, int recipeNum ) {
		var target = recipes[recipeNum];
		bool[] complete = new bool[target.Materials.ToArray ().Length];
		for (int n = 0; n < target.Materials.ToArray ().Length; n++) {
			for (int m = 0; m < inventory.Length; m++) {
				if (inventory[m].Item == target.Materials[n] && inventory[m].Volume >= target.MaterialAmountList[n]) {
					complete[n] = true;
					break;
				}
				else complete[n] = false;
			}

			if (complete[n] == false) {
				return false;
			}
		}

		return true;
	}
}
