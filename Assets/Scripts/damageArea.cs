using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class damageArea : NetworkBehaviour {

    public static float damageScale = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay(Collision collision)
    {
        //print(collision.gameObject.name);
        if (collision.gameObject.tag == "PlayerUnit")
        {
            print("Stay!");
            
            collision.gameObject.GetComponent<playerController>().drophealth(damageScale * Time.deltaTime);
            
        }
    }
}
