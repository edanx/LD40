using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efx : MonoBehaviour {

    Animator Animator;

    float timer = 0f;  


	// Use this for initialization
	void Start () {
        Animator = this.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > 2f) {
            Destroy(this.gameObject);
        }
    }
}
