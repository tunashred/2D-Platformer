using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    
    // starting position for the parallax object
    private Vector2 startingPosition;
    
    // start z value for the parallax object
    private float startingZ;
    
    // '=>' means it updates itself every frame, so no need to add it in the Update function
    // with this we figure in a XY only basis how far the camera has moved away from the starting position
    // of this parallax object
    private Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    private float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    
    // if the parallax object is in front of the player, it's going to have a negative value; so use nearClipPlane
    // otherwise, if it's behind the player, it uses farClipPlane
    float clippingPlane => cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);

    private float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
