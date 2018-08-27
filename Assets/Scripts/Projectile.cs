using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    // Variables
    protected float maxDistance;
    protected float traveledDistance;
    protected float travelSpeed;
    protected float damage;
    protected double kp;
    protected GameObject emitter;

    // Constructor
    public Projectile(float maxDistance, float travelSpeed, float damage, double kp)
    {
        this.maxDistance = maxDistance;
        this.travelSpeed = travelSpeed;
        this.damage = damage;
        this.kp = kp;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * travelSpeed);
        traveledDistance += travelSpeed * Time.deltaTime;

        if (traveledDistance >= maxDistance)
            Destroy(this.gameObject);
    }

    public double getKp()
    {
        return kp;
    }

    public float getDamage()
    {
        return damage;
    }
    public GameObject getEmitter()
    {
        return emitter;
    }
    public void setEmitter(GameObject Object)
    {
        emitter = Object;
    }
}
