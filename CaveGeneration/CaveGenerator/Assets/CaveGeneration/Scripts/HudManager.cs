using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

    private static HudManager instance = null;

    public static HudManager Instance {
        get { return instance; }
    }



    public Slider jetPackSlider;
    // Use this for initialization
    void Awake() {
        if(instance != null) {
            Destroy(instance.gameObject);
        } else {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateSliderValue(float value) {
        
        value = value / 100;
        jetPackSlider.value = value;
    }
}
