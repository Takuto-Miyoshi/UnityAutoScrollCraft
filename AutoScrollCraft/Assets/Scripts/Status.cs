using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Start is called before the first frame update
	void Start () {
		hp = maxHp;
	}

	// Update is called once per frame
	void Update () {

	}
}
