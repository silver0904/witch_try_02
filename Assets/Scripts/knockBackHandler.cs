using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knockBackHandler {
    //variables
    public double knockBackCounter = 0;
    private double threshold = 1.000003;
    private Vector3 hitDirection = new Vector3();
    private Vector3 externalDirection = new Vector3();

    //methods
    public void setKnockBack(double kp, Vector3 hitDirection)
    {
        this.hitDirection = hitDirection;
        knockBackCounter += kp;
        externalDirection = -hitDirection * (float)knockBackCounter;
    }

    public Vector3 getUpdatedKnockBack()
    {
        if (knockBackCounter > threshold)
        {
            knockBackCounter = knockBackCounter/2;
            externalDirection = -hitDirection * (float)knockBackCounter;
            //print(externalDirection.x + ", " + externalDirection.y + ", " + externalDirection.z);

        }
        else
        {
            // reset the knock back counter and external direction so that the object wont keep sliding away.
            knockBackCounter = 0;
            externalDirection = new Vector3(0, 0, 0);
        }
        return externalDirection;
    }
    //public Vector3 getExternalDirection()
    //{
    //    return externalDirection;
    //}
	
	// Update is called once per frame
	public double getKnockBackCounter()
    {
        return knockBackCounter;
    }
}
