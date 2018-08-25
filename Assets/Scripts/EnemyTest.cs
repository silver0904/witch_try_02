using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{

    // Public Variables
    public float movementSpeed;
    public CharacterController controller;
    public float gravityScale = 0.5F;
    public GameObject selectedProjectile;
    public float knockBackCounter = 0;


    // Private helper variables
    private float rotationSmooth = 7F;
    private Quaternion previousRotation;
    private Vector3 moveDirection ;
    private Vector3 externalDirection = new Vector3();
    private Vector3 actualDirection;
    public GameObject projectileSpawnPoint;
    private Transform projectileSpawned;


    // Use this for initialization
    void Start()
    {

        controller = GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {

        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, 0, Input.GetAxis("Vertical") * movementSpeed);
        actualDirection = moveDirection + externalDirection;
        // v = u + at
        actualDirection.y = actualDirection.y + (gravityScale * Physics.gravity.y);
        controller.Move(actualDirection * Time.deltaTime);

        


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            print("hit");
            knockBack(other);
        }
    }

    private void knockBack(Collider other)
    {
        float kp = other.GetComponent<Projectile>().getKp();
        //if (knockBackCounter > 0) knockBackCounter = ;
        //else  
        knockBackCounter += kp;
        while (knockBackCounter > 0) {
            
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            hitDirection.y = 0;
            externalDirection = (-hitDirection * knockBackCounter);
            print(externalDirection.x + ", " + externalDirection.y + ", " + externalDirection.z);
            knockBackCounter -= kp*0.25f;
        }
    }
}
