using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Archer : NPCBase {
		[SerializeField] private SearchObject searchArea;
		[SerializeField] private GameObject arrow;
		protected override async void Update () {
			base.Update ();

			if (CanBeAction == true) {
				if (searchArea.Detected == true) {
					AI.ShootProjectile ( this, searchArea.Target.transform.position, arrow, 0.3f );
				}
				else {
					AI.Wandering ( this );
				}

				CanBeAction = false;
				await UniTask.Delay ( TimeSpan.FromSeconds ( UpdateInterval ) );
				CanBeAction = true;
			}
		}
	}
}
