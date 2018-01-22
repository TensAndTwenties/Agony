﻿using UnityEngine;
using System.Collections;

public class GrapplingHookController: MonoBehaviour {

    public GrappleState currentState;
    public float grappleProjectileFiringSpeed;
    public float grappleProjectileReelingSpeed;
    public float grappleProjectileRetractingSpeed;
    public float grapplePlayerSpeed; //
    public float hookHeadOffsetFromBody; // how much is the grapple head offset from the player?
    public bool planted = false;

    private GameObject parentObject; //when the grapple is idle, whats the parent transform object?
    private GameObject playerObject;



	// Use this for initialization
	void Awake() {        
        parentObject = GameObject.Find("DirectionPointer");
        playerObject = GameObject.Find("Player");

        ChangeState(GrappleState.idle);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FireGrapple(Vector3 target, Vector3 playerTarget, GameObject targetCoverObject)
    {

        if (currentState == GrappleState.idle)
        {
            float grappleHeadTravelDistance = (target - this.transform.position).magnitude;
            float playerTravelDistance = (playerTarget - playerObject.transform.position).magnitude;

            float playerTravelTime = playerTravelDistance / grapplePlayerSpeed;
            float grappleHeadTravelTime = grappleHeadTravelDistance / grappleProjectileFiringSpeed;

            ChangeState(GrappleState.firing);
            var seq = LeanTween.sequence();
            seq.append(LeanTween.move(this.gameObject, target, grappleHeadTravelTime));
            seq.append(() =>
            {
                planted = true;
                ChangeState(GrappleState.reeling);
            });
            seq.append(LeanTween.move(playerObject, playerTarget, playerTravelTime).setEase(LeanTweenType.easeInQuad));
            seq.append(() =>
            {
                ChangeState(GrappleState.idle);
            });
            seq.append(() =>
            {
                playerObject.GetComponent<PlayerController>().currentCoverObject = targetCoverObject;
            });
        }
        else if (currentState == GrappleState.firing || currentState == GrappleState.reeling) {
            //RETRACT
            //if its firing and the fire function is triggered again, retract it

            if (currentState == GrappleState.firing) {
                if (LeanTween.isTweening(playerObject))
                {
                    LeanTween.cancel(playerObject, true);
                }
            }

            if (LeanTween.isTweening(this.gameObject)) {
                LeanTween.cancel(this.gameObject, true);
            }

            float grappleHeadTravelDistance = (playerObject.transform.position - this.transform.position).magnitude;
            float grappleHeadTravelTime = grappleHeadTravelDistance / grappleProjectileRetractingSpeed;

            ChangeState(GrappleState.retracting);

            var seq = LeanTween.sequence();
            seq.append(LeanTween.move(this.gameObject, playerObject.transform, grappleHeadTravelTime));
            seq.append(() =>
            {
                ChangeState(GrappleState.idle);
            });
        }
        

        
    }

    private void ChangeState(GrappleState targetState) {

        if (targetState == GrappleState.firing)
        {
            this.gameObject.transform.parent = null;
        }
        else if (targetState == GrappleState.idle) {
            this.gameObject.transform.parent = parentObject.transform;
            this.gameObject.transform.localPosition = new Vector3(0 + hookHeadOffsetFromBody, 0, 0);
            this.planted = false;
        }

        currentState = targetState;
    }

    public enum GrappleState { firing, idle, reeling, retracting }
}
