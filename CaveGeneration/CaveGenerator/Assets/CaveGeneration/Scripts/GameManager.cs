using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject playerPrefab;
    public MapGenerator mapGenerator;

	// Use this for initialization
	void Awake () {
        mapGenerator.GenerateMap();
        mapGenerator.PlacePlayerOnMap(playerPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
