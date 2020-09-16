using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimateGrapple : MonoBehaviour {
    
    private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;

    public HammerWeapon grapplingGun;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;

    void Awake() {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    void DrawRope() {
        if (Input.GetMouseButtonUp(1)) {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        else if (!grapplingGun.isRegularGrappling && !grapplingGun.isEnemyGrappling && !grapplingGun.isRetracting && !grapplingGun.isDirectGrappling) {
            currentGrapplePosition = grapplingGun.gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0) {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Updatef(Time.deltaTime);

        var grapplePoint = grapplingGun.grapplePoint;
        var gunTipPosition = grapplingGun.gunTip.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        if (grapplingGun.isRetracting) {
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, gunTipPosition, Time.deltaTime * 12f);

            lr.SetPosition(0, gunTipPosition);
            lr.SetPosition(1, currentGrapplePosition);

            if (Vector3.Distance(currentGrapplePosition, gunTipPosition) < 0.1) {
                grapplingGun.isRetracting = false;
            }
        }

        else {
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);
        }

        if (grapplingGun.isRetracting) {
            for (var i = 0; i < quality + 1; i++) {
                var delta = i / (float)quality;
                var offset = up * (waveHeight / 4) * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                affectCurve.Evaluate(delta);

                lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
            }
        }

        else {
            for (var i = 0; i < quality + 1; i++) {
                var delta = i / (float)quality;
                var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                affectCurve.Evaluate(delta);

                lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
            }
        }
    }
}