using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile {
    private static float fireballTravelSpeed = 5;
    private static float fireballMaxDistance = 7f;
    private static float fireballBaseDamage = 10;
    private float damage ;

    public Fireball(float maxDistance , float travelSpeed, float damage) : base(fireballMaxDistance, fireballTravelSpeed, fireballBaseDamage)
    {
    }

	// Use this for initialization
	void Start () {
        maxDistance = fireballMaxDistance;
        travelSpeed = fireballTravelSpeed;
        damage = fireballBaseDamage;
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
