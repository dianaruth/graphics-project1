using UnityEngine;
using System.Collections;

public class SunScript : MonoBehaviour {

    public int bound;

    public int speed;

    void Start ()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(new Vector3(0, 0, bound), Vector3.right, speed * Time.deltaTime);
    }
}
