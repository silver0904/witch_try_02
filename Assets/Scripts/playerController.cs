﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour
{
    //public syncvar variable
    [SyncVar]
    public double hp = 1000;


    // Public Variables
    public float movementSpeed;
    public CharacterController controller;
    public float gravityScale = 0.5F;
    //public float knockBackCounter = 0;
    public GameObject projectileSpawnPoint;
    public GameObject selectedProjectile;


    // Private helper variables
    private double chargingCounter;
    private float rotationSmooth = 7F;
    private Quaternion previousRotation;
    private Vector3 moveDirection;
    private Vector3 actualDirection;
    private Vector3 externalDirection = new Vector3();
    private knockBackHandler KnockBackHandler;
    private shootHandler ShootHandler;
    private GameObject projectileSpawned;


    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        KnockBackHandler = new knockBackHandler();
        ShootHandler = new shootHandler(selectedProjectile, this.gameObject.transform);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority == false)
        {
            // if player doesn't have authority to this Player unit
            // if player is charging to spawn a fireball
            // don't do anything
            return;
        }
        //check and update the situation of the player unit first
        checkDie();
        bool isCharging = ShootHandler.getIsCharging();

        //exterternalDirection refer to force by other projectile
        externalDirection = KnockBackHandler.getUpdatedKnockBack();

        //moveDirection refers to player control movement
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, 0, Input.GetAxis("Vertical") * movementSpeed);

        if (isCharging == false)
        {
            // when player unit is not charging, it movement depends on external force + it's player control
            actualDirection = moveDirection + externalDirection;
        }
        else
        {
            // when player unit is charging, it movement only depends on external force, since it is not allowed to move when charging
            actualDirection = externalDirection;
        }

        //print("knockBackCounter: " + knockBack.getKnockBackCounter());
        //print(externalDirection.x + ", " + externalDirection.y + ", " + externalDirection.z);
        // v = u + at
        actualDirection.y = actualDirection.y + (gravityScale * Physics.gravity.y);

        //after this line, the player move in meter per seconds scale, rather than meter per frame scale
        controller.Move(actualDirection * Time.deltaTime);
        //print(actualDirection.x + ", " + actualDirection.y + ", " + actualDirection.z);


        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(shoot());
        }
        if (isCharging == false)
        {
            // character rotate with move direction
            if (moveDirection == new Vector3(0, 0, 0))
            {
                transform.rotation = previousRotation;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
                previousRotation = Quaternion.LookRotation(moveDirection);
            }
        }
        else
            // when charging, since previous rototation is set to the shooting direction, so just need to maintain that rotation.
            transform.rotation = ShootHandler.getShootingDirection();


    }

    IEnumerator shoot()
    {
        ShootHandler.shoot();
        float chargingTime = ShootHandler.getChargingTime();
        yield return new WaitForSeconds(chargingTime);
        CmdSpawnProjectile();
        yield return null;
    }

    public void chargeProjectile(float chargingTime)
    {
        Invoke("CmdSpawnProjectile", chargingTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            print("other.damage = " +other.GetComponent<Projectile>().getDamage());
            if (other.GetComponent<Projectile>().getPlayerNetId() == netId.Value)
            {
                // do nothing if the emitter of the projectile is yourself
                Debug.Log("this object netID = " + netId.Value);
                Debug.Log("fireball has netID = " + other.GetComponent<Projectile>().getPlayerNetId());
                return;
            }
            print("hit!");
            double kp = other.GetComponent<Projectile>().getKp();
            Vector3 hitDirection = other.transform.position - transform.position;
            KnockBackHandler.setKnockBack(kp, hitDirection);

        }
        print("OnTriggerEnter Finished");
    }

    private void checkDie()
    {
        if (transform.position.y < -20)
        {
            print("someone drop to dead!");
            Destroy(this.gameObject);

        }

    }

    //COMMAND, use to send command to the server
    [Command]
    private void CmdSpawnProjectile()
    {
        projectileSpawned = Instantiate(selectedProjectile);
        projectileSpawned.transform.position = projectileSpawnPoint.transform.position;
        projectileSpawned.transform.rotation = projectileSpawnPoint.transform.rotation;
        projectileSpawned.GetComponent<Projectile>().setPlayerNetId(netId.Value);
        NetworkServer.Spawn(projectileSpawned);
    }

    ////RPC, use to send request to client
    //[ClientRpc]
    //private void RpcSpawnProjectile()
    //{
    //    Debug.Log("RPC");
    //    projectileSpawned = Instantiate(selectedProjectile);
    //    projectileSpawned.transform.position = projectileSpawnPoint.transform.position;
    //    projectileSpawned.transform.rotation = projectileSpawnPoint.transform.rotation;
    //    projectileSpawned.GetComponent<Projectile>().setPlayerNetId(netId.Value);
    //    NetworkServer.Spawn(projectileSpawned);
    //}





}
