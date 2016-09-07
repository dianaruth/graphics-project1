using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(new Vector3(64, 0, 64));
	}
}
