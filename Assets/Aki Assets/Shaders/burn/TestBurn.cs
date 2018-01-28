using UnityEngine;
using System.Collections;

public class TestBurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Burn animation: 10s second
        // name of fuction to invoke after the animation is over
        this.BurnObject(10f,DestroyAfter);
	}

    void DestroyAfter (TestBurn obj)
    {
        Destroy(obj);
    }
}
