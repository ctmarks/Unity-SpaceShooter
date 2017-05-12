using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody myRigid;
    public float speed;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        myRigid.velocity = transform.forward * speed;
    }

}
