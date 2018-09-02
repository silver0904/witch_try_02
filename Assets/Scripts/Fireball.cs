using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Fireball : Projectile {
    private static float fireballTravelSpeed = 5;
    private static float fireballMaxDistance = 7f;
    private static float fireballBaseDamage = 10;
    private static double fireballBasekp = 30;
    private static float fireballChargingTime = 0.3f;

    public Fireball() : base()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerUnit" && other.GetComponent<NetworkIdentity>().netId.Value != this.playerNetId)
        {
            other.GetComponent<playerController>().drophealth(damage);

            //Invoke("selfDestroy", 2*Time.deltaTime);
            Invoke("selfDestroy", 2 * Time.deltaTime);
        }
        if (other.tag == "Projectile" && other.GetComponent<Projectile>().getPlayerNetId() != this.playerNetId)
        {
            Invoke("selfDestroy", 2 * Time.deltaTime);
        }
    }


    // Use this for initialization
    //every time a projectile is spawn, this will run
    void Start () {
        maxDistance = fireballMaxDistance;
        travelSpeed = fireballTravelSpeed;
        damage = fireballBaseDamage;
        kp = fireballBasekp;
        chargingTime = fireballChargingTime;
        //print("this.chargingTime =" + getChargingTime());
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}


}
