using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour {

    Dialogue dialogue;
    DialogueImplementation imp;

	// Use this for initialization
	void Start () {
        dialogue = GameObject.Find("IntroDialogue").GetComponent<Dialogue>();
        imp = GameObject.Find("IntroDialogue").GetComponent<DialogueImplementation>();
        dialogue.Run(imp.defaultDialogue);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
