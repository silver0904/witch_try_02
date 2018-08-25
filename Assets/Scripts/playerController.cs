using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour {

    // Public Variables
    public float movementSpeed;
    public CharacterController controller;
    public float gravityScale = 0.5F;
    public float knockBackCounter = 0;
    public GameObject projectileSpawnPoint;
    public GameObject selectedProjectile;

    
    // Private helper variables
    private float rotationSmooth = 7F;
    private Quaternion previousRotation;
    private Vector3 moveDirection;
    private Vector3 actualDirection;
    private Vector3 externalDirection = new Vector3();

    private GameObject projectileSpawned;

    //public Rigidbody body;

    // Use this for initialization
    void Start () {

        controller = GetComponent<CharacterController>();
        

    }
	
	// Update is called once per frame
	void Update () {
        if (hasAuthority == false)
        {
            // if player doesn't have authority to this Player unit
            return;
        }
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, 0, Input.GetAxis("Vertical") * movementSpeed);
        actualDirection = moveDirection + externalDirection;
        // v = u + at
        actualDirection.y = actualDirection.y + (gravityScale * Physics.gravity.y);
        controller.Move(actualDirection*Time.deltaTime);



        if (Input.GetMouseButtonDown(0))
        {
            
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;

            if (playerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
                previousRotation = transform.rotation;
            }
            shoot();
        }
        else
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


    }

    private void shoot()
    {
        //projectileSpawned = Instantiate(selectedProjectile.transform, projectileSpawnPoint.transform.position, Quaternion.identity);
        // this spawn projectile can be modified later to accomadate other type of projectile
        CmdSpawnProjectile();
        
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
            knockBack(other);
        }
    }

    // this function handle knock back sitation
    private void knockBack(Collider other)
    {
        float kp = other.GetComponent<Projectile>().getKp();
        //if (knockBackCounter > 0) knockBackCounter = ;
        //else  
        knockBackCounter += kp;
        while (knockBackCounter > 0)
        {

            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            hitDirection.y = 0;
            externalDirection = (-hitDirection * knockBackCounter);
            print(externalDirection.x + ", " + externalDirection.y + ", " + externalDirection.z);
            knockBackCounter -= kp * 0.25f;
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
