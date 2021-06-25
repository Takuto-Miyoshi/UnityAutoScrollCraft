using System.Collections.Generic;
using System.Linq;
using AutoScrollCraft.Actors;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class KeyInfoManager : MonoBehaviour {
		[SerializeField] Player player;
		private const float ShowingTime = 5.0f;
		// -------- moveKey ----------
		[SerializeField] List<Image> moveKeys;
		private bool showingMoveKey = false;
		private float notMovingTime = 0.0f;
		private const float NeedShowingMoveKeyTime = 5.0f;
		// -------- trashKey --------
		[SerializeField] Image trashKey;
		private bool showedTrash = false;
		// -------- interactKey --------
		[SerializeField] Image interactKey;
		private bool showedInteract = false;
		// -------- dashKey --------
		[SerializeField] Image dashKey;
		private bool showedDash = false;
		// -------- itemKey --------
		[SerializeField] Image itemKey;
		private bool showedItem = false;

		private void Start () {
			moveKeys.ForEach ( e => e.gameObject.SetActive ( false ) );
			trashKey.gameObject.SetActive ( false );
			interactKey.gameObject.SetActive ( false );
			dashKey.gameObject.SetActive ( false );
			itemKey.gameObject.SetActive ( false );
		}

		private void Update () {
			// moveKey
			if (player.NotMoving == true) notMovingTime += Time.deltaTime;
			else notMovingTime = 0.0f;

			if (notMovingTime > NeedShowingMoveKeyTime) ShowMove ();

			// trashKey
			if (player.FullInventory == true) ShowTrash ();

			// interactKey
			if (player.NearObject == true) ShowInteract ();

			// itemKey
			if (player.TakeItem == true) ShowItem ();
		}

		private async void ShowMove () {
			if (showingMoveKey == true) return;

			// 移動したら非表示
			showingMoveKey = true;
			moveKeys.ForEach ( e => e.gameObject.SetActive ( true ) );
			moveKeys.ForEach ( e => UIFunctions.FloatingShow ( e, true ) );
			await UniTask.WaitUntil ( () => player.NotMoving == false );
			moveKeys.ForEach ( e => e.gameObject.SetActive ( false ) );
			showingMoveKey = false;

			// ダッシュは１回だけ表示
			if (showedDash == true) return;
			showedDash = true;
			dashKey.gameObject.SetActive ( true );
			UIFunctions.FloatingShow ( dashKey, true );
		}

		private void ShowTrash () {
			if (showedTrash == true) return;

			showedTrash = true;
			trashKey.gameObject.SetActive ( true );
			UIFunctions.FloatingShow ( trashKey, true );
		}

		private void ShowInteract () {
			if (showedInteract == true) return;

			showedInteract = true;
			interactKey.gameObject.SetActive ( true );
			UIFunctions.FloatingShow ( interactKey, true );
		}

		private void ShowItem () {
			if (showedItem == true) return;

			showedItem = true;
			itemKey.gameObject.SetActive ( true );
			UIFunctions.FloatingShow ( itemKey, true );
		}
	}
}
