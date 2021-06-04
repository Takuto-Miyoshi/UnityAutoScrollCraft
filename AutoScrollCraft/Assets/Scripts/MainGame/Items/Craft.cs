using System.Collections.Generic;
using UnityEngine;

namespace AutoScrollCraft.Items {
	public class Craft : MonoBehaviour {
		public struct CraftData {
			private List<Enums.Items> materials;  // 素材
			public List<Enums.Items> Materials { get => materials; set => materials = value; }
			private List<int> materialAmountList;   // 素材数
			public List<int> MaterialAmountList { get => materialAmountList; set => materialAmountList = value; }
			private Enums.Items result;   // 生成物
			public Enums.Items Result { get => result; set => result = value; }
			private int resultAmount;   // 生成量
			public int ResultAmount { get => resultAmount; set => resultAmount = value; }
			private int score;
			public int Score { get => score; set => score = value; }
			public void Reset () {
				materials = new List<Enums.Items> ();
				materialAmountList = new List<int> ();
				result = Enums.Items.Wood;
				resultAmount = 1;
				score = 0;
			}
		}
		private static CraftData tmpRecipe; // 下書き
		private static List<CraftData> recipes = new List<CraftData> (); // レシピ集
		public static List<CraftData> Recipes { get => recipes; }
		private static int maxRecipeNumber;
		public static int MaxRecipeNumber { get => maxRecipeNumber; }
		private static bool loaded = false;

		// Start is called before the first frame update
		private void Start () {
			if (loaded == false) {
				loaded = true;
				tmpRecipe.Reset ();
				// ----------ここからレシピ-------------------
				// 斧
				AddMaterial ( Enums.Items.Wood, 2 );
				AddMaterial ( Enums.Items.Stone, 3 );
				SetResult ( Enums.Items.Axe, 1 );
				SetScore ( 30 );
				AddRecipe ();
				// つるはし
				AddMaterial ( Enums.Items.Wood, 2 );
				AddMaterial ( Enums.Items.Stone, 4 );
				SetResult ( Enums.Items.Pickaxe, 1 );
				AddRecipe ();
				// 剣
				AddMaterial ( Enums.Items.Wood, 1 );
				AddMaterial ( Enums.Items.Stone, 4 );
				SetResult ( Enums.Items.Sword, 1 );
				AddRecipe ();
				// ------------------------------------------
				maxRecipeNumber = recipes.ToArray ().Length - 1;
			}
		}

		// 下書きのレシピを追加
		private static void AddRecipe () {
			recipes.Add ( tmpRecipe );
			tmpRecipe.Reset ();
		}

		// 下書きに素材を追加
		private static void AddMaterial ( Enums.Items item, int value ) {
			tmpRecipe.Materials.Add ( item );
			tmpRecipe.MaterialAmountList.Add ( value );
		}

		// 下書きの生成物を設定
		private static void SetResult ( Enums.Items item, int value ) {
			tmpRecipe.Result = item;
			tmpRecipe.ResultAmount = value;
		}

		// 下書きにスコアを設定
		private static void SetScore ( int score ) {
			tmpRecipe.Score = score;
		}

		// アイテムを作れるか
		public static bool CanBeCrafting ( Actors.Player.ItemData[] inventory, int recipeNum ) {
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
}
