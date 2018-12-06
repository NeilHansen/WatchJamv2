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

            UIManager.Instance.MonsterDrainUI();

            hitObject.transform.GetChild(2).GetComponent<FlashlightController>().useTime -= 2 * Time.deltaTime;
            hitObject.transform.GetChild(2).GetComponent<FlashlightController>().maxTime -= 2 * Time.deltaTime;
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

            //Debug.Log(hitObject.gameObject.name + " Not Draining Power");
        }
    }

    //Handles turning on and off of the drain beam
    void MonsterDrain()
    {
        //Monster drain
        if (monster.player.GetButtonDown("Drain") && !monster.isDraining)
        {
            monster.isDraining = true;

            //To stop draining
            StartCoroutine(stopDrain());
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

    //To stop draining
    IEnumerator stopDrain()
    {
        //Keep color semi - transparent
        UIManager.Instance.drainColor.a = 0.35f;
        UIManager.Instance.drainIcon.color = UIManager.Instance.drainColor;

        yield return new WaitForSeconds(monster.drainLength);

        //Keep transparent when on cooldown
        UIManager.Instance.drainColor.a = 0.35f;
        UIManager.Instance.drainIcon.color = UIManager.Instance.drainColor;

        monster.drainCooldown = true;

        yield return new WaitForSeconds(monster.drainCooldownLength);

        //Turn off
        UIManager.Instance.drainColor.a = 1.0f;
        UIManager.Instance.drainIcon.color = UIManager.Instance.drainColor;

        monster.isDraining = false;
        monster.drainCooldown = false;
    }
}
