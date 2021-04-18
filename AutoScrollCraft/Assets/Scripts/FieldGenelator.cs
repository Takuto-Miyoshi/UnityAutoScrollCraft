using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FieldGenelator : MonoBehaviour {
	[SerializeField] int xSize;
	[SerializeField] int zSize;
	[SerializeField] GameObject grassBlock;

	// Start is called before the first frame update
	void Start () {
		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < zSize; z++) {
				Instantiate ( grassBlock, new Vector3 ( x, 0, z ), Quaternion.identity );
			}
		}
	}
}
