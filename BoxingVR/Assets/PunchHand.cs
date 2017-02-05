using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHand : MonoBehaviour {

    public SteamVR_TrackedObject hand; //allows controller to be assigned to hand 
    private Rigidbody rBody;


	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
        rBody.MovePosition(hand.transform.position);
        rBody.MoveRotation(hand.transform.rotation);
	}

    private void OnCollisionEnter(Collision other) //be able to detect the collision
    {
        Debug.LogError("HIT");
        Rigidbody otherR = other.gameObject.GetComponentInChildren<Rigidbody>();
        if (other == null)
            return;

        //average contact point: figures out the contact point (ex: punch collides with different knuckes, where is average point)
        Vector3 avgPoint = Vector3.zero;
        foreach(ContactPoint p in other.contacts)
        {
            avgPoint += p.point;
        }

        avgPoint /= other.contacts.Length;

        //direction we're going to punch it
        Vector3 dir = (avgPoint - transform.position).normalized;
        otherR.AddForceAtPosition(dir * 1000f * rBody.velocity.magnitude, avgPoint); //direction calculation adds force/velocity the faster we arrive/hit an object; 
        StartCoroutine(LongVibration(.1f, .2f)); // this begins the haptic feedback coroutine below
    }


    //length is how long the vibration should go for
    //strength is vibration strength from 0-1
    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            SteamVR_Controller.Input((int)hand.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }


//vibrationCount is how many vibrations
//vibrationLength is how long each vibration should go for
//gapLength is how long to wait between vibrations
//strength is vibration strength from 0-1
IEnumerator LongVibration(int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(gapLength);
            yield return StartCoroutine(LongVibration(vibrationLength, strength));
        }
    }
}
