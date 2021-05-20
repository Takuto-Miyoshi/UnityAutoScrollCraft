using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {
	static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<T> ();

				if (instance == null) {
					var o = new GameObject ();
					o.name = typeof ( T ).Name;
					instance = o.AddComponent<T> ();
				}
			}

			return instance;
		}
	}

	public virtual void Awake () {
		if (instance == null) {
			instance = this as T;
			DontDestroyOnLoad ( gameObject );
		}
		else {
			Destroy ( gameObject );
		}
	}
}
