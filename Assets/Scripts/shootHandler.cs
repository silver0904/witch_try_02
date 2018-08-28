using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootHandler :ScriptableObject{
    private GameObject selectedProjectile;
    private GameObject projectileSpawned;
    private Quaternion shootingDirection;
    private Transform playerUnitTransform;
    private float chargingTime;
    private float chargingCounter;
    private bool isCharging;


    public shootHandler (GameObject projectile,Transform unitTransform)
    {
        this.selectedProjectile = projectile;
        this.playerUnitTransform = unitTransform;
        isCharging = false;
        chargingTime = selectedProjectile.GetComponent<Fireball>().getChargingTime();
        Debug.Log("charging Time get Component" + chargingTime);
        chargingTime = 0.2f + 0.05f;
    }

    public void shoot()
    {
        //chargingCounter = selectedProjectile.GetComponent<Fireball>().getChargingTime();
        chargingCounter = chargingTime;
        //update the isCharging state
        checkCharging();
        setShootingDirection();
        // wait for some time to invoke spawn projectile
        

    }

    public bool getIsCharging()
    {
        checkCharging();
        return this.isCharging;
    }

    public float getChargingTime()
    {
        return this.chargingTime;
    }

    public Quaternion getShootingDirection()
    {
        return this.shootingDirection;
    }

    //this method has to be run every frame, so please be included in playerController update()
    private void checkCharging()
    {
        if (chargingCounter >0)
        {
            chargingCounter -= 1 * Time.deltaTime;
            isCharging = true;
            //print("chargingCounter = " + chargingCounter + " isCharging =  " + isCharging);
        }
        else
        {
            chargingCounter = 0;
            isCharging = false;
        }
    }



    private void setShootingDirection()
    {
        Plane playerPlane = new Plane(Vector3.up, playerUnitTransform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - playerUnitTransform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            shootingDirection = Quaternion.Slerp(playerUnitTransform.rotation, targetRotation, 1);
        }
        
    }



    
   

}
