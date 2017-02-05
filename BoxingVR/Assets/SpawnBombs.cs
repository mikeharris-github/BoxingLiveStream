using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBombs : MonoBehaviour {

    public GameObject[] balls; //created an array
    public Transform CameraRig; //created to add the Camera Rig position

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnBombsOverTime());
	}
	
    IEnumerator SpawnBombsOverTime() 
    {
        while (true)
        {
            GameObject ball = Instantiate(balls[Random.Range(0, balls.Length)]); //get random ball from list
            float angle = Random.Range(0f, 360f); // be sure to do the angle first before angle in mathf.sin(angle)
            float radius = Random.Range(1f, 1.25f);
            ball.transform.position = CameraRig.position + new Vector3(radius * Mathf.Sin(angle), Random.Range(1.25f, 1.75f), radius * Mathf.Cos(angle));
            //you want to spawn in the circle around the player. Ball transform starts at camera rig plus X,Y,Z
            //Height (Y coordinate) is set to be around player
            yield return new WaitForSeconds(Random.Range(1f, 3f)); //range between 0 and 2 seconds
        } 
    }
}
