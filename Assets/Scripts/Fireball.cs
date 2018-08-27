using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile {
    private static float fireballTravelSpeed = 5;
    private static float fireballMaxDistance = 7f;
    private static float fireballBaseDamage = 10;
    private static double fireballBasekp = 100;
    private static double fireballChargingTime = 0.3;

    public Fireball(float maxDistance , float travelSpeed, float damage) : base(fireballMaxDistance, fireballTravelSpeed, fireballBaseDamage, fireballBasekp, fireballChargingTime)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerUnit")
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        maxDistance = fireballMaxDistance;
        travelSpeed = fireballTravelSpeed;
        damage = fireballBaseDamage;
        kp = fireballBasekp;
        chargingTime = fireballChargingTime;
        print("this.chargingTime =" + getChargingTime());
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
