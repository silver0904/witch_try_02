using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour {
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
    public bool isCharging = false;
    public bool isCharged = false;


    // Private helper variables
    private double chargingCounter;
    private float rotationSmooth = 7F;
    private Quaternion previousRotation;
    private Vector3 moveDirection;
    private Vector3 actualDirection;
    private Vector3 externalDirection = new Vector3();
    private knockBackHandler knockBack;
    private GameObject projectileSpawned;


    // Use this for initialization
    void Start () {

        controller = GetComponent<CharacterController>();
        knockBack = new knockBackHandler();
    }
	
	// Update is called once per frame
	void Update () {
        if (hasAuthority == false)
        {
            // if player doesn't have authority to this Player unit
            // if player is charging to spawn a fireball
            // don't do anything
            return;
        }
        //check and update the situation of the player unit first
        checkDie();
        checkCharging();

        //exterternalDirection refer to force by other projectile
        externalDirection = knockBack.getUpdatedKnockBack();

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
        controller.Move(actualDirection*Time.deltaTime);
        //print(actualDirection.x + ", " + actualDirection.y + ", " + actualDirection.z);


        if (Input.GetButtonDown("Fire1") )
        {
            shoot();            
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
            transform.rotation = previousRotation;


    }

    private void shoot()
    {
        // character rotate with mouse direction
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            // change the unit rotation to the mouse position.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
            previousRotation = transform.rotation;
        }


        print("charging time = " + selectedProjectile.GetComponent<Projectile>().getChargingTime());
        print("kp = " + selectedProjectile.GetComponent<Projectile>().getKp());
        //shoot(selectedProjectile.GetComponent<Fireball>().getChargingTime());
        //projectileSpawned = Instantiate(selectedProjectile.transform, projectileSpawnPoint.transform.position, Quaternion.identity);
        // this spawn projectile can be modified later to accomadate other type of projectile
        chargingCounter = 0.3;
        //print("charging Counter = " + chargingCounter);
        //update the isCharging state
        checkCharging();
        
        // wait for some time to invoke spawn projectile
        Invoke("CmdSpawnProjectile",(float)0.3);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            
            if (other.GetComponent<Projectile>().getEmitter() == this.gameObject)
            {
                // do nothing if the emitter of the projectile is yourself
                return;
            }
            print("hit!");
            double kp = other.GetComponent<Projectile>().getKp();
            Vector3 hitDirection = other.transform.position - transform.position;
            knockBack.setKnockBack(kp, hitDirection);

        }
    }

    private void checkDie()
    {
        if (transform.position.y < -20)
        {
            print("someone drop to dead!");
            Destroy(this.gameObject);
            
        }

    }

    private void checkCharging()
    {
        if (chargingCounter > 0)
        {       
            chargingCounter -= 1* Time.deltaTime;
            isCharging = true;
            print("chargingCounter = " + chargingCounter + " isCharging =  " + isCharging);
        }
        else
        {
            chargingCounter = 0;
            isCharging = false;
        }
    }



    //COMMAND, use to send command to the server
    [Command]
    private void CmdSpawnProjectile()
    {
        projectileSpawned = Instantiate(selectedProjectile);
        projectileSpawned.transform.position = projectileSpawnPoint.transform.position;
        projectileSpawned.transform.rotation = projectileSpawnPoint.transform.rotation;
        projectileSpawned.GetComponent<Projectile>().setEmitter(this.gameObject);
        NetworkServer.Spawn(projectileSpawned);
    }
}
