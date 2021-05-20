using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using AutoScrollCraft.Enums;
using UnityEngine;

namespace AutoScrollCraft.Enums {
	public enum StatusType {
		Hp,
		Stamina,
	};
}

namespace AutoScrollCraft.Actors {
	public class Status : MonoBehaviour {
		[SerializeField] int maxHp;
		public int MaxHp {
			get { return maxHp; }
		}
		[SerializeField] int hp;
		public int Hp {
			get { return hp; }
			set { hp = value; }
		}
		[SerializeField] int maxStamina;
		public int MaxStamina {
			get { return maxStamina; }
		}
		[SerializeField] int stamina;
		public int Stamina {
			get { return stamina; }
			set { stamina = value; }
		}
		[SerializeField] int attackPower;
		public int AttackPower {
			get { return attackPower; }
		}

		// Start is called before the first frame update
		void Start () {
			hp = maxHp;
			stamina = maxStamina;
		}

		// Update is called once per frame
		void Update () {

		}

		public int MaxValue ( StatusType statusType ) {
			switch (statusType) {
				case StatusType.Hp: return maxHp;
				case StatusType.Stamina: return maxStamina;
				default: return 0;
			}
		}

		public int CurrentValue ( StatusType statusType ) {
			switch (statusType) {
				case StatusType.Hp: return hp;
				case StatusType.Stamina: return stamina;
				default: return 0;
			}
		}
	}
}
