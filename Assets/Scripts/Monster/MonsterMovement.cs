using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour {

    private bool doOnce = false;
    private NavMeshAgent agent;
    Vector3 velocity = Vector2.zero;
    private Quaternion r;

    public Camera cam;

    // Use this for initialization
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");

    //    int layerMask = 1 << 15;

    //    RaycastHit hit;

    //    //if (Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon)
    //    {
            
    //        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-45, transform.right) * transform.forward, out hit, 2, layerMask) && !doOnce)
    //        {
    //            doOnce = true;
    //            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-45, transform.right) * transform.forward * 2, Color.yellow);

    //            r = hit.transform.rotation;

    //            agent.SetDestination(hit.point);
    //        }
    //        else if(!doOnce)
    //        {
    //            agent.angularSpeed = 180;
    //            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-45, transform.right) * transform.forward * 2, Color.white);
    //            if (z > -Mathf.Epsilon)
    //            {
    //                velocity = z * agent.speed * transform.forward * Time.deltaTime;
    //                agent.Move(velocity);
    //                agent.SetDestination(transform.position + velocity);
    //            }
    //            else if (z < -Mathf.Epsilon)
    //            {
    //                velocity = z * agent.speed * transform.forward * Time.deltaTime;
    //                agent.Move(velocity);
    //                agent.SetDestination(transform.position + velocity);
    //            }
    //        }

    //        //When we reach our destination
    //        if (!agent.pathPending && doOnce)
    //        {
    //            if (agent.remainingDistance <= agent.stoppingDistance)
    //            {
    //                if (!agent.hasPath && agent.velocity.sqrMagnitude == 0f)
    //                {
    //                    Debug.Log("Done");
    //                    doOnce = false;
    //                    transform.rotation = hit.transform.rotation;
    //                }
    //            }
    //            Debug.Log(doOnce);
    //            Debug.Log("pending" + !agent.pathPending);
    //        }
    //        else
    //        {
    //            transform.Rotate(0, x * agent.angularSpeed * Time.deltaTime, 0);
    //        }
    //    }
    //}

    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        int layerMask = 1 << 15;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(0, transform.right) * transform.forward, out hit, 2, layerMask))
        {
            Debug.Log("Hit");
            doOnce = true;
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(0, transform.right) * transform.forward * 2, Color.yellow);
            
            transform.rotation = hit.transform.rotation;
            transform.position = hit.transform.position;
            transform.Translate(Vector3.up * 0.5f);
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(0, transform.right) * transform.forward * 2, Color.white);
            agent.isStopped = false;
            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);
        }
    }
}
