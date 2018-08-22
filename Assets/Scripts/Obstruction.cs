using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour {
    public float hp;
    public bool isDestructable;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (hp <=0 && isDestructable == true)
        {
            destroy();
        }
	}

    private void destroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            
            GameObject projectile = other.gameObject;
            this.hp -= projectile.GetComponent<Projectile>().damage;
            print(this.hp);
            Destroy(projectile);
        }
    }
}
