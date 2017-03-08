using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private bool isChecked;
    public bool Checked {
        get { return isChecked; }
        set { isChecked = value; }
    }

    private MeshRenderer _renderer;

	// Use this for initialization
	void Awake () {
        _renderer = GetComponent<MeshRenderer>();
        //BFS.current.onVisited += MarkTile;
	}
	
    public void MarkTile() {
        _renderer.material.color = Color.gray;
    }

	// Update is called once per frame
	void Update () {
		
	}

    internal void SetColor(Color color) {
        _renderer.material.color = color;
    }
}
