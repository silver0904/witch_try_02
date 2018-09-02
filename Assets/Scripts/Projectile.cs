using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    // Variables
    protected  float maxDistance;
    protected  float traveledDistance;
    protected  float travelSpeed;
    protected  float damage;
    protected  double kp;
    protected  float chargingTime;
    [SyncVar]
    public  uint playerNetId;

    // Constructor
    public Projectile()
    {

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
    public float getChargingTime()
    {

        return chargingTime;
    }

    public void selfDestroy()
    {
        CmdDestroy();
    }
    //Command send to server
    private void CmdDestroy()
    {
        Destroy(this.gameObject);
        NetworkServer.Destroy(this.gameObject);
    }
}
