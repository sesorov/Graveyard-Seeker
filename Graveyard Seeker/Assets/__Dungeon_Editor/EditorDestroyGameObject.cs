using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDestroyGameObject : MonoBehaviour {
	// Destroy this.gameObject when the scene starts
	void Start () {
        Destroy(gameObject);
	}
}
