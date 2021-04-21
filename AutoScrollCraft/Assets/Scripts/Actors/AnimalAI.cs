using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour {
	NavMeshAgent nav;
	Status status;
	const float updateInterval = 3.0f;
	float updateTimer;
	[SerializeField] GameObject dropItem;

	// Start is called before the first frame update
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		status = GetComponent<Status> ();
		updateTimer = updateInterval;
	}

	// Update is called once per frame
	void Update () {
		updateTimer -= Time.deltaTime;

		if (updateTimer < 0) {
			var pos = transform.position;
			pos.x += Random.Range ( -5.0f, 5.0f );
			pos.z += Random.Range ( -5.0f, 5.0f );
			nav.SetDestination ( pos );
			updateTimer = updateInterval;
		}

		// HPが0になったらアイテムを落としてDestroy
		if (status.Hp <= 0) {
			if (dropItem != null) {
				DropItem.Drop ( transform.position, dropItem, Random.Range ( 1, 4 ) );
			}

			Destroy ( gameObject, 0.01f );
		}
	}

	void OnCollisionEnter ( Collision collision ) {
		if (collision.gameObject.tag == "Projectile") {
			status.Hp -= collision.gameObject.GetComponent<ProjectileBase> ().Damage;
			Destroy ( collision.gameObject );
		}
	}
}
