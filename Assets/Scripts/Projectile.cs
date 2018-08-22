using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Variables
    protected float maxDistance;
    protected float traveledDistance;
    protected float travelSpeed;
    public float damage;
    public GameObject emitter;

    // Constructor
    public Projectile(float maxDistance, float travelSpeed, float damage)
    {
        this.maxDistance = maxDistance;
        this.travelSpeed = travelSpeed;
        this.damage = damage;
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
}
