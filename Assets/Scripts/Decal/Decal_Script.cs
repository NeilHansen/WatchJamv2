using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal_Script : MonoBehaviour
{
    public GameObject ThisDecal;
    public float LifeTime = 5.0f;
    private GameObject decalRef;
    private bool stained = false;

    // Start is called before the first frame update
    void Start()
    {      

    }

    // Update is called once per frame
    void Update()
    {
        //When Destroyed can stain again
        if (decalRef == null)
            stained = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Monster"))
        {
            if(!stained)
                CreatePrefab(other.transform.position);
        }
    }
    
    void CreatePrefab(Vector3 pos)
    {
        stained = true;
        // instantiate decal
        decalRef = Instantiate(ThisDecal, pos, transform.rotation);
        Destroy(decalRef, LifeTime);
    }
}