using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoScrollCraft;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Weapons.Projectile;
using UnityEngine;

namespace AutoScrollCraft.Items {
	public class ItemFunctions : MonoBehaviour {
		Player player;

		// 効果
		GameObject stone;
		const int PorkEfficacy = 10;
		const int BeefEfficacy = 15;
		GameObject arrow;

		void Start () {
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

		// アイテムの内部処理

		public bool Wood () {
			return false;
		}

		public bool Stone () {
			var o = Instantiate ( stone, player.transform.position + player.transform.forward * 1.5f, player.transform.rotation );
			o.GetComponent<ProjectileBase> ().Master = player.gameObject;

			return true;
		}

		public bool Pork () {
			StaminaHeal ( PorkEfficacy );

			return true;
		}

		public bool Beef () {
			StaminaHeal ( BeefEfficacy );

			return true;
		}

		public bool Arrow () {
			var o = Instantiate ( arrow, player.transform.position + player.transform.forward * 1.5f, player.transform.rotation );
			o.GetComponent<ProjectileBase> ().Master = player.gameObject;

			return true;
		}

		// 汎用関数
		void HpHeal ( int efficacy ) {
			// 回復後の値が最大値を上回らない
			if (player.Status.MaxHp - efficacy >= player.Status.Hp) {
				player.Status.Hp += efficacy;
			}
			// ..上回る
			else {
				player.Status.Hp = player.Status.MaxHp;
			}
		}

		void StaminaHeal ( int efficacy ) {
			// 回復後の値が最大値を上回らない
			if (player.Status.MaxStamina - efficacy >= player.Status.Stamina) {
				player.Status.Stamina += efficacy;
			}
			// ..上回る
			else {
				player.Status.Stamina = player.Status.MaxStamina;
			}
		}
	}
}