using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		//destroy the beam
		Destroy(col.gameObject);
	}

}
