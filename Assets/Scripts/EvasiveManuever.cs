using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManuever : MonoBehaviour {

    public float dodge;

    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public float smoothing;
    public float tilt;
    public Boundary boundary;

    private float targetManeuver;
    private Rigidbody myRigid;
    private float currentSpeed;


	// Use this for initialization
	void Start () {
        myRigid = GetComponent<Rigidbody>();
        currentSpeed = myRigid.velocity.z;
        StartCoroutine(Evade());
        
	}
	
    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        float newManeuver = Mathf.MoveTowards(myRigid.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        myRigid.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        myRigid.position = new Vector3
            (
            Mathf.Clamp(myRigid.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(myRigid.position.z, boundary.zMin, boundary.zMax)
            );

        myRigid.rotation = Quaternion.Euler(0.0f, 0.0f, myRigid.velocity.x * -tilt);
	}
}
