using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Enums;
using UnityEngine;

public class ItemFunctions : MonoBehaviour {
	Player player;

	// 効果
	GameObject stone;
	const int porkEfficacy = 10;

	void Start () {
		player = GetComponent<Player> ();
		stone = (GameObject)Resources.Load ( "Weapons/Stone" );
	}

	/// <returns>アイテムを消費するか</returns>
	public bool ExecItem ( Items item ) {
		// 同じ名前の関数を実行する
		var m = item.ToString ();
		Type t = GetType ();
		MethodInfo mi = t.GetMethod ( m );
		object o = mi.Invoke ( this, null );

		return Convert.ToBoolean ( o );
	}

	public bool Wood () {
		return false;
	}

	public bool Stone () {
		Instantiate ( stone, player.transform.position + player.transform.forward * 1.5f, player.transform.rotation );
		return true;
	}

	public bool Pork () {
		// 回復後の値が最大値を上回らない
		if (player.Status.MaxStamina - porkEfficacy >= player.Status.Stamina) {
			player.Status.Stamina += porkEfficacy;
		}
		// ..上回る
		else {
			player.Status.Stamina = player.Status.MaxStamina;
		}

		return true;
	}
}
