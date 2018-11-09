using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatSensor : MonoBehaviour {

    public float refreshTime = 1.0f;
    public float maxSensorDistance = 10.0f;
    public float scanSpeed = 50.0f;
    public float minAngle = -90.0f;
    public float maxAngle = 90.0f;
    public GameObject heartBeatPrefab;


    public GameObject rayObject1;
    public GameObject rayObject2;
    public GameObject rayObject3;
    public GameObject rayObject4;
    public GameObject rayObject5;

    private float angle1;
    private float angle2;
    private float angle3;
    private float angle4;
    private float angle5;

    private float counter = 0;

    //For reference in other scripts
    public static HeartBeatSensor Instance;

    // Use this for initialization
    void Start()
    {
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        angle1 = minAngle;
        angle3 = (minAngle + maxAngle) / 2;
        angle2 = (angle1 + angle3) / 2;
        angle5 = maxAngle;
        angle4 = (angle3 + angle5) / 2;

    }

    // Update is called once per frame
    void Update()
    {
        counter += scanSpeed * Time.deltaTime;

        // Bit shift the index of the layer (14) to get a bit mask
        int layerMask = 1 << 14;

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        RaycastHit hit5;

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle1, transform.up) * transform.forward, out hit1, counter, layerMask))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle1, transform.up) * transform.forward * hit1.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (rayObject1 == null)
            {
                Debug.Log("Setting Object");

                rayObject1 = hit1.collider.gameObject;
                rayObject1.GetComponent<HeartBeat>().heartBeat.SetActive(true);
            }
        }
        else if (rayObject1 != null)
        {
            Debug.Log("Removing Object");

            rayObject1.GetComponent<HeartBeat>().heartBeat.SetActive(false);
            rayObject1 = null;
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle1, transform.up) * transform.forward * counter, Color.white);
            Debug.Log("Did not Hit");
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle2, transform.up) * transform.forward, out hit2, counter, layerMask))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle2, transform.up) * transform.forward * hit2.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (rayObject2 == null)
            {
                Debug.Log("Setting Object");

                rayObject2 = hit2.collider.gameObject;
                rayObject2.GetComponent<HeartBeat>().heartBeat.SetActive(true);
            }
        }
        else if (rayObject2 != null)
        {
            Debug.Log("Removing Object");

            rayObject2.GetComponent<HeartBeat>().heartBeat.SetActive(false);
            rayObject2 = null;
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle2, transform.up) * transform.forward * counter, Color.white);
            Debug.Log("Did not Hit");
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle3, transform.up) * transform.forward, out hit3, counter, layerMask))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle3, transform.up) * transform.forward * hit3.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (rayObject3 == null)
            {
                Debug.Log("Setting Object");

                rayObject3 = hit3.collider.gameObject;
                rayObject3.GetComponent<HeartBeat>().heartBeat.SetActive(true);
            }
        }
        else if (rayObject3 != null)
        {
            Debug.Log("Removing Object");

            rayObject3.GetComponent<HeartBeat>().heartBeat.SetActive(false);
            rayObject3 = null;
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle3, transform.up) * transform.forward * counter, Color.white);
            Debug.Log("Did not Hit");
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle4, transform.up) * transform.forward, out hit4, counter, layerMask))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle4, transform.up) * transform.forward * hit4.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (rayObject4 == null)
            {
                Debug.Log("Setting Object");

                rayObject4 = hit4.collider.gameObject;
                rayObject4.GetComponent<HeartBeat>().heartBeat.SetActive(true);
            }
        }
        else if (rayObject4 != null)
        {
            Debug.Log("Removing Object");

            rayObject4.GetComponent<HeartBeat>().heartBeat.SetActive(false);
            rayObject4 = null;
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle4, transform.up) * transform.forward * counter, Color.white);
            Debug.Log("Did not Hit");
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle5, transform.up) * transform.forward, out hit5, counter, layerMask))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle5, transform.up) * transform.forward * hit5.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (rayObject5 == null)
            {
                Debug.Log("Setting Object");

                rayObject5 = hit5.collider.gameObject;
                rayObject5.GetComponent<HeartBeat>().heartBeat.SetActive(true);
            }
        }
        else if (rayObject5 != null)
        {
            Debug.Log("Removing Object");

            rayObject5.GetComponent<HeartBeat>().heartBeat.SetActive(false);
            rayObject5 = null;
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle5, transform.up) * transform.forward * counter, Color.white);
            Debug.Log("Did not Hit");
        }

        //Reset scan line and clear all heart beats on screen
        if (counter >= maxSensorDistance)
        {
            counter = 0;
        }
    }
}
