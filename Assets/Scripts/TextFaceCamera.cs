using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFaceCamera : MonoBehaviour {

    // Update is called once per frame
    private void Start()
    {
        this.transform.Rotate(new Vector3(80, 0, 0));
    }
    void Update () {

            //this.transform.LookAt(UnityEngine.Camera.main.transform.position);
            this.transform.eulerAngles =new Vector3(80, 0, 0);

        
        
	}
}
