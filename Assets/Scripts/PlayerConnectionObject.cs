using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            // this object belongs to this player
            return;
        }
        Debug.Log("player unit is spawned");
        //Instantiate(playerUnitPrefab);
        CmdSpawnMyUnit();
    }
    public GameObject playerUnitPrefab;
    public GameObject myUnit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (myUnit == null)
                CmdSpawnMyUnit();
            else
                print("cannot spawn when you are still alive");
        }
        
    }


    [Command]
    void CmdSpawnMyUnit()
    {
        myUnit = Instantiate(playerUnitPrefab);
        NetworkServer.SpawnWithClientAuthority(myUnit, connectionToClient);
    }
}
