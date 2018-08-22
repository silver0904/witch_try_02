using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour {
    public float health;
    public bool isDestructable;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (health <0 && isDestructable == true)
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
            this.health -= projectile.GetComponent<Projectile>().damage;
            Destroy(projectile);
        }
    }
}
