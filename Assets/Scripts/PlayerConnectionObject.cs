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

    // Update is called once per frame
    void Update()
    {

    }


    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject myUnit = Instantiate(playerUnitPrefab);
        NetworkServer.SpawnWithClientAuthority(myUnit, connectionToClient);
    }
}
