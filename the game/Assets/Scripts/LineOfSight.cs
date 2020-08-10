using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineOfSight : MonoBehaviour {

    public Transform hips, headTransform;
    private float playerDist;
    private bool didSee;
    public Enemy1Script enemy1Script;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public LayerMask groundLayer, playerLayer;

    void OnTriggerStay(Collider other) {
        bool player = Physics.Raycast(headTransform.position, hips.position - headTransform.position, out RaycastHit hit, playerDist, playerLayer);
        bool ground = Physics.Raycast(headTransform.position, hips.position - headTransform.position, out RaycastHit playerHit, playerDist, groundLayer);
        if (other.tag == "Player" && player && !ground) {
            didSee = true;
        }
    }

    void Update() {
        playerDist = Vector3.Distance(transform.parent.transform.position, hips.position);

        if (playerDist <= 7) {
            navMeshAgent.isStopped = true;
            animator.SetTrigger("Isn't Walking");
        } 
        else if (playerDist > 7 && didSee) {
            navMeshAgent.destination = hips.position;
            navMeshAgent.isStopped = false;
            animator.SetTrigger("Is Walking");
        }
    }
}
