using System;
using System.Reflection;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Weapons;
using UnityEngine;

namespace AutoScrollCraft.Items {
	public class ItemFunctions : MonoBehaviour {
		private Player player;

		// 効果
		private GameObject stone;
		private const int PorkEfficacy = 15;
		private const int BeefEfficacy = 10;
		private GameObject arrow;
		private const int AxePower = 10;
		private const int PickaxePower = 8;
		private const int SwordPower = 12;

		private void Awake () {
			player = GetComponent<Player> ();

			stone = (GameObject)Resources.Load ( "Weapons/Stone" );
			arrow = (GameObject)Resources.Load ( "Weapons/Arrow" );
		}

		/// <returns>アイテムを消費するか</returns>
		public bool Exec ( Enums.Items item ) {
			// 同じ名前の関数を実行する
			var m = item.ToString ();
			Type t = GetType ();
			MethodInfo mi = t.GetMethod ( m );
			object o = mi.Invoke ( this, null );

			return Convert.ToBoolean ( o );
		}

		// --------- アイテムの内部処理 ----------

		public bool Wood () {
			return false;
		}

		public bool Stone () {
			InstantiateProjectile ( stone );
			return true;
		}

		public bool Pork () {
			StaminaHeal ( PorkEfficacy );
			return true;
		}

		public bool Beef () {
			HpHeal ( BeefEfficacy );
			return true;
		}

		public bool Arrow () {
			InstantiateProjectile ( arrow );
			return true;
		}

		public bool Axe () {
			player.Interact ( AxePower, Enums.ObjectType.Tree );
			return false;
		}

		public bool Pickaxe () {
			player.Interact ( PickaxePower, Enums.ObjectType.Rock );
			return false;
		}

		public bool Sword () {
			player.Interact ( SwordPower, Enums.ObjectType.Actor );
			return false;
		}

		// 汎用関数
		private void HpHeal ( int efficacy ) {
			// 回復後の値が最大値を上回らない
			if (player.Status.MaxHp - efficacy >= player.Status.Hp) {
				player.Status.Hp += efficacy;
			}
			// ..上回る
			else {
				player.Status.Hp = player.Status.MaxHp;
			}
		}

		private void StaminaHeal ( int efficacy ) {
			// 回復後の値が最大値を上回らない
			if (player.Status.MaxStamina - efficacy >= player.Status.Stamina) {
				player.Status.Stamina += efficacy;
			}
			// ..上回る
			else {
				player.Status.Stamina = player.Status.MaxStamina;
			}
		}

		private void InstantiateProjectile ( GameObject projectile ) {
			// 発射体を生成
			var o = Instantiate ( projectile, player.transform.position + player.transform.forward * 1.5f, player.transform.rotation );
			o.GetComponent<ProjectileBase> ().Master = player.gameObject;
		}
	}
}