using AutoScrollCraft.Actors;
using AutoScrollCraft.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class Gauge : MonoBehaviour {
		[SerializeField] private Image gauge;
		[SerializeField] private Status status;
		[SerializeField] private StatusType targetStatus;
		private float maxValue;
		private float currentValue;

		private void Start () {
			maxValue = status.MaxValue ( targetStatus );
		}

		private void Update () {
			currentValue = status.CurrentValue ( targetStatus );
			gauge.fillAmount = currentValue / maxValue;
		}
	}
}
