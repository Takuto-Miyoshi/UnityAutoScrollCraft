using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft.Actors.AI;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Archer : NPCBase {
		[SerializeField] SearchObject searchArea;
		[SerializeField] GameObject arrow;
		protected override void Update () {
			base.Update ();

			if (CanBeAction == true) {
				if (searchArea.Detected == true) {
					AI.ShootProjectile ( this, searchArea.Target.transform.position, arrow );
				}
				else {
					AI.Wandering ( this );
				}
			}

			IsDead ();
		}
	}
}
