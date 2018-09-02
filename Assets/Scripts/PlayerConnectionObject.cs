using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{
    [SyncVar]
    public NetworkInstanceId unitNetId;
    public uint idTest;
    public GameObject playerUnitPrefab;
    public GameObject myUnit;
    
    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            // this object does not belong to this player
            return;
        }
        //Debug.Log("this is the before the cmdspqwnMyunit");
        GameObject unitToBeSpawned = Instantiate(playerUnitPrefab) as GameObject;


        CmdSpawnMyUnit(unitToBeSpawned);
        idTest = unitNetId.Value;
        //Debug.Log("this is the after the cmdspqwnMyunit");
        //Debug.Log("myUnit situation at the end of start = " + (myUnit == null ? "null" : "not null"));
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("myUnit situation in runtime = " + (myUnit == null ? "null" : "not null"));
        if (myUnit == null)
        {
            myUnit = NetworkServer.FindLocalObject(unitNetId);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (myUnit == null) {
                GameObject unitToBeSpawned = Instantiate(playerUnitPrefab) as GameObject;
                CmdSpawnMyUnit(unitToBeSpawned);
        }
        else
            print("cannot spawn when you are still alive");
        }

    }

    // Command function executed in server side
    [Command]
    void CmdSpawnMyUnit(GameObject unitToBeSpawned)
    {
        Debug.Log("this is the begging of cmdspqwnMyunit");
        myUnit = unitToBeSpawned;
        
        //print("myUnit name = " + myUnit.name);
        //Debug.Log("myUnit situation in cmd = " + (myUnit == null ? "null" : "not null"));
        NetworkServer.SpawnWithClientAuthority(myUnit as GameObject, connectionToClient);
        unitNetId = myUnit.GetComponent<NetworkIdentity>().netId;
        RpcAssignUnit(unitNetId);
        //Debug.Log("myUnit netId = " + unitNetId);
        //Debug.Log("this is the end of cmdspqwnMyunit");
    }

    [ClientRpc]
    private void RpcAssignUnit(NetworkInstanceId id)
    {

        myUnit = NetworkServer.FindLocalObject(id);
    }

}