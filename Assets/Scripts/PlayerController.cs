using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;
    public SimpleTouchPad touchpad;
    public SimpleTouchAreaButton areaButton;
    public GameObject shot;
    public Transform shotSpawn; // shotSpawn.transform.position
    private Rigidbody myRigid;

    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private GameObject newShot;
    private float myTime = 0.0F;
    private AudioSource myAudio;
    private Vector3 filter;
    private Quaternion calibrationQuaternion;

    private void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();
        CalibrateAccelerometer();
        filter = Vector3.zero;
    }

    void Update()
    {
        myTime = myTime + Time.deltaTime;

        if (areaButton.CanFire() && myTime > nextFire)
        {
            nextFire = myTime + fireDelta;
            //                        newShot = 
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);  //as GameObject;
            myAudio.Play();
            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }
    }

    private void FixedUpdate()
    {

        //Keyboard Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        myRigid.velocity = movement * speed;

        myRigid.position = new Vector3
            (
                Mathf.Clamp(myRigid.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(myRigid.position.z, boundary.zMin, boundary.zMax)
            );

        myRigid.rotation = Quaternion.Euler(0.0f, 0.0f, myRigid.velocity.x * -tilt);
	}


        //Accelerometer Input
        //Vector3 accelerationRaw = Input.acceleration;
        //Vector3 acceleration = FixAcceleration(accelerationRaw);
        //Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);

        ////clamp acceleration vector to unit sphere
        //if (movement.sqrMagnitude > 1)
        //{
        //    movement.Normalize();
        //}
        ////filter = Vector3.Lerp(filter, movement, 3 * Time.deltaTime);
        ////movement = filter * Time.deltaTime;

        //myRigid.velocity = movement * speed;

        //myRigid.position = new Vector3
        //    (
        //        Mathf.Clamp(myRigid.position.x, boundary.xMin, boundary.xMax),
        //        0.0f,
        //        Mathf.Clamp(myRigid.position.z, boundary.zMin, boundary.zMax)
        //    );

        //myRigid.rotation = Quaternion.Euler(0.0f, 0.0f, myRigid.velocity.x * -tilt);



        //Touch Input
//        Vector2 direction = touchpad.GetDirection();
//        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
//
//
//        myRigid.velocity = movement * speed;
//        Debug.Log(movement);
//        myRigid.position = new Vector3
//        (
//            Mathf.Clamp(myRigid.position.x, boundary.xMin, boundary.xMax),
//            0.0f,
//            Mathf.Clamp(myRigid.position.z, boundary.zMin, boundary.zMax)
//        );
//
//        myRigid.rotation = Quaternion.Euler(0.0f, 0.0f, myRigid.velocity.x * -tilt);
//    }

    void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    Vector3 FixAcceleration(Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }
}
