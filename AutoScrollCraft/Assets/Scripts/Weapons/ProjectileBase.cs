using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {
	[SerializeField] float speed;
	[SerializeField] int damage;
	public int Damage {
		get { return damage; }
	}

	// Start is called before the first frame update
	void Start () {
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -50) {
			Destroy ( gameObject );
		}
	}

	void OnCollisionEnter ( Collision collision ) {
		if (collision.gameObject.tag == "Terrain") {
			gameObject.tag = "DropItem";
		}
	}
}
