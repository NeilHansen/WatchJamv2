using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDrain : MonoBehaviour {

    public MonsterController monster;
    private MeshRenderer meshRender;

    public Material drainMaterial;
    private Material defaultMaterial;

    private GameObject hitObject = null;
    private CapsuleCollider drainCollider;

    // Use this for initialization
    void Start () {
        drainCollider = GetComponent<CapsuleCollider>();
        meshRender = GetComponent<MeshRenderer>();
        defaultMaterial = meshRender.material;
    }

    // Update is called once per frame
    void Update () {
        MonsterDrain();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Security")
        {
            hitObject = other.gameObject;

            MonsterUI.Instance.MonsterDrainUI(monster);

            monster.isHittingPlayer = true;
            meshRender.material = drainMaterial;

            //Debug.Log(hitObject.gameObject.name + " Draining Power");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Security")
        {
            hitObject = other.gameObject;

            monster.isHittingPlayer = false;
            meshRender.material = defaultMaterial;
        }
    }

    //Handles turning on and off of the drain beam
    void MonsterDrain()
    {
        //Monster drain
        if (monster.player.GetButtonDown("Drain") && !monster.isDraining)
        {
            monster.isDraining = true;

            //Reset Material to default just in case
            meshRender.material = defaultMaterial;

            //To stop draining
            MonsterUI.Instance.StopDraining(monster);
        }
        //is in draining mode
        else if (monster.isDraining && !monster.drainCooldown)
        {
            drainCollider.gameObject.GetComponent<MeshRenderer>().enabled = true;
            drainCollider.enabled = true;
        }
        else
        {
            drainCollider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            drainCollider.enabled = false;
        }
    }


}
