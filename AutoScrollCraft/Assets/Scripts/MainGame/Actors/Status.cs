using AutoScrollCraft.Enums;
using UnityEngine;

namespace AutoScrollCraft.Enums {
	public enum StatusType {
		Hp,
		Stamina,
	};

	public enum ObjectType {
		None,
		Actor,
		Rock,
		Tree,
	}
}

namespace AutoScrollCraft.Actors {
	public class Status : MonoBehaviour {
		[SerializeField] private int maxHp;
		public int MaxHp { get => maxHp; }
		[SerializeField] private int hp;
		public int Hp { get => hp; set => hp = value; }
		[SerializeField] private int maxStamina;
		public int MaxStamina { get => maxStamina; }
		[SerializeField] private int stamina;
		public int Stamina { get => stamina; set => stamina = value; }
		[SerializeField] private int attackPower;
		public int AttackPower { get => attackPower; }
		[SerializeField] private int score;
		public int Score { get => score; set => score = value; }
		[SerializeField] private ObjectType objectType;
		public ObjectType ObjectType { get => objectType; set => objectType = value; }

		private void Awake () {
			hp = maxHp;
			stamina = maxStamina;
		}

		public int MaxValue ( StatusType statusType ) {
			return statusType switch
			{
				StatusType.Hp => maxHp,
				StatusType.Stamina => maxStamina,
				_ => 0
			};
		}

		public int CurrentValue ( StatusType statusType ) {
			return statusType switch
			{
				StatusType.Hp => hp,
				StatusType.Stamina => stamina,
				_ => 0
			};
		}
	}
}
