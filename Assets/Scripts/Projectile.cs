using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    // Variables
    [SyncVar]
    protected float maxDistance;
    [SyncVar]
    protected float traveledDistance;
    [SyncVar]
    protected float travelSpeed;
    [SyncVar]
    protected float damage;
    [SyncVar]
    protected double kp;
    [SyncVar]
    protected double chargingTime;
    [SyncVar]
    protected uint playerNetId;

    // Constructor
    public Projectile(float maxDistance, float travelSpeed, float damage, double kp, double chargingTime)
    {
        this.maxDistance = maxDistance;
        this.travelSpeed = travelSpeed;
        this.damage = damage;
        this.kp = kp;
        this.chargingTime = chargingTime;
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
    public uint getPlayerNetId()
    {
        return playerNetId;
    }
    public void setPlayerNetId(uint NetId)
    {
        this.playerNetId = NetId;
    }
    public double getChargingTime()
    {

        return chargingTime;
    }
}
