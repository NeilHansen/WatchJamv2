using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar]
    public Color colour;

    private void Start()
    {
        GetComponent<Renderer>().material.color = colour;
    }

    // Update is called once per frame
    void Update ()
    {
		if (hasAuthority)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(x, y, 0) * 10.0f * Time.deltaTime);
        }
    }
}
