using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour {
	[SerializeField] Image gauge;
	[SerializeField] Status status;
	[SerializeField] StatusType targetStatus;
	float maxValue;
	float currentValue;

	// Start is called before the first frame update
	void Start () {
		maxValue = status.MaxValue ( targetStatus );
	}

	// Update is called once per frame
	void Update () {
		currentValue = status.CurrentValue ( targetStatus );
		gauge.fillAmount = currentValue / maxValue;
	}
}
