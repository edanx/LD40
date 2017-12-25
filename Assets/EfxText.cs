using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EfxText : MonoBehaviour {

    public Text TextControl;
    // Use this for initialization

    private void Awake()
    {
        TextControl = GetComponentInChildren<Text>();
    }

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
