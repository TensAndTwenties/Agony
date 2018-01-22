﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Vector2 mousePos;
    private GameObject directionPointer; //HUD element
    private GameObject levelController;
    private GameObject pointerTest;
    public GameObject currentCoverObject { get; set; } // what the player is currently using as cover
    private Vector3 currentPointerDirection;
    private Vector3 currentPointerGeoTarget; // point on geo where aim currently is
    private GameObject currentPointerGeoTargetObject; // object where aim currently is 
    private RaycastHit pointerGeoDetectHit;

    private KeyCode previouslyPressed= 0;

    public float playerGrappleOffset; //when the player finished a grapple, how far away are they along the normal vector of impact target?

    private GameObject grapplingHook;
    private Vector3 playerTargetPosition = Vector3.zero; //cover currently aimed at


    // Use this for initialization
    void Start () {
        directionPointer = GameObject.Find("DirectionPointer");
        levelController = GameObject.Find("LevelController");
        pointerTest = GameObject.Find("pointerTest");
        grapplingHook = GameObject.Find("GrapplingHook");

        this.transform.position = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        mousePos = Input.mousePosition;
        //screenPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - camera.transform.position.z));

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
         
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        //direction pointer
        currentPointerDirection = new Vector3(mousePos.x,0, mousePos.y);
        directionPointer.transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
        //target geo detection

        //pointerTest.transform.position = currentPointerDirection;

        if (Physics.Raycast(transform.position, currentPointerDirection, out pointerGeoDetectHit, 2000)) {
            //we want this to return only on cover
            if (pointerGeoDetectHit.transform.gameObject.tag == "Cover") {
                float tileLength = pointerGeoDetectHit.transform.gameObject.GetComponent<Collider>().bounds.size.x;
                Vector3 normal = pointerGeoDetectHit.normal;

                pointerTest.transform.position = pointerGeoDetectHit.transform.position + (normal * tileLength/2);
                pointerTest.transform.rotation = pointerGeoDetectHit.transform.parent.rotation;
                playerTargetPosition = pointerGeoDetectHit.transform.position + (normal * playerGrappleOffset);
                currentPointerGeoTargetObject = pointerGeoDetectHit.transform.gameObject;
                currentPointerGeoTarget = new Vector3(pointerGeoDetectHit.point.x, 0, pointerGeoDetectHit.point.z);

            }
            
        }
        //pointerTest.transform.position = new Vector3(pointerGeoDetectHit.point.x, pointerGeoDetectHit.point.y, 0);
        print("Hit - distance: " + pointerGeoDetectHit.distance + " PointerDirection: (" + currentPointerDirection.x + "," + currentPointerDirection.y + "," + currentPointerDirection.z + ")");
        //print("Mouse: " + mousePos.x + "," + mousePos.y);

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            previouslyPressed = KeyCode.Mouse1;
            grapplingHook.GetComponent<GrapplingHookController>().FireGrapple(currentPointerGeoTarget, playerTargetPosition, currentPointerGeoTargetObject);
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            previouslyPressed = 0;
        }
    }
}
