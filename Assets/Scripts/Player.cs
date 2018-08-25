using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Variables
    public float movementSpeed;
    public float rotationSmooth = 7f;
    public float waitTime;
    public GameObject camera;
    public GameObject projectileSpawnPoint;
    public GameObject playerObj;
    public GameObject projectile;

    private Transform projectileSpawned;
    // Methods
    public void Update()
    {
        ////player facing mouse
        //Plane playerPlane = new Plane(Vector3.up, transform.position);
        //Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        //float hitDist = 0.0f;

        //if (playerPlane.Raycast(ray, out hitDist))
        //{
        //    Vector3 targetPoint = ray.GetPoint(hitDist);
        //    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        //    targetRotation.x = 0;
        //    targetRotation.z = 0;
        //    playerObj.transform.rotation = Quaternion.Slerp(playerObj.transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
        //}
        playerMovementControl();

 
        
    }

    // This method control the player movement
    private void playerMovementControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);

        }
        if (Input.GetMouseButtonDown(0))
        {
            //shoot();
        }

        
    }

    //private void shoot()
    //{
    //    projectileSpawned = Instantiate(projectile.transform, projectileSpawnPoint.transform.position, Quaternion.identity);
    //    projectileSpawned.GetComponent<Projectile>().emitter = this.gameObject;
    //    projectileSpawned.rotation = projectileSpawnPoint.transform.rotation;
    //}

}
