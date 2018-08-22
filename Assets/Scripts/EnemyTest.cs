using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour {
    private float knockBackCounter;
    private float knockBackTime = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            print("hi");
            float kp = other.GetComponent<Projectile>().kp;
            knockBack(other, kp);

        }
    }
    

    private void knockBack(Collider other, float kp)
    {
        print(kp);
        knockBackCounter = 10;
        while (knockBackCounter > 0) {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            hitDirection.y = 0;
            transform.Translate(-hitDirection * kp * Time.deltaTime);
            knockBackCounter -= 1;
        }
    }
}
