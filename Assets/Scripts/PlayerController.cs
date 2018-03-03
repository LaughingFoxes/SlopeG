using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var h = Input.GetAxis("Horizontal") * 150 * Time.deltaTime * 150f;
        var v = Input.GetAxis("Vertical") * 150 * Time.deltaTime * 3.0f;

        GetComponent<Rigidbody2D>().AddForce(new Vector3(h, v, 0));

    }
}
