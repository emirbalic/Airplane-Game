using UnityEngine;
using System.Collections;

public class Propeller : MonoBehaviour {

    public static float turnSpeed=0;

    static bool Engine;

    void Start()
    {
        
    }

    public void TurnOnEngine()
    {
        
        turnSpeed = 10000;
        Debug.Log("Ludilo");
    }

    public float GetPropelerSpeed()
    {
        return turnSpeed;
    }

    public void TurnOfEngine()
    {
        turnSpeed = turnSpeed - 1;
    }
        

    public void SpeedUp()
    {
        turnSpeed = turnSpeed + 10000;
    }

    public void SpeedDown()
    {
        if(turnSpeed>10000)
            turnSpeed = turnSpeed - 10000;
    }
    
	void Update ()
	{
        transform.RotateAroundLocal(Vector3.forward, turnSpeed * Time.deltaTime);
	}

    
}
