using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxHelper : MonoBehaviour {

    public AudioClip impact;
    public AudioClip hurt;
    public AudioClip attack;
    public AudioClip jump;

    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySFX(string name) {
        if(name == "impact")
            audioSource.PlayOneShot(impact, 0.7F);
        if (name == "hurt")
            audioSource.PlayOneShot(hurt, 0.7F);
        if (name == "attack")
            audioSource.PlayOneShot(attack, 0.7F);
        if (name == "jump")
            audioSource.PlayOneShot(jump, 0.7F);
    }
}
