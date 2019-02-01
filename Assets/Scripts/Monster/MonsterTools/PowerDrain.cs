using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDrain : MonoBehaviour {

    public MonsterController monster;

    public CapsuleCollider drainCollider;
    public MeshRenderer meshRender;

    public Material drainMaterial;
    public Material defaultMaterial;

    // Use this for initialization
    void Start () {
        defaultMaterial = meshRender.material;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Security")
        {
            monster.isDrainHitting = true;
            meshRender.material = drainMaterial;
            monster.CmdRemoveDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Security")
        {
            monster.isDrainHitting = false;
            meshRender.material = defaultMaterial;
        }
    }

    //Handles turning on and off of the drain beam
    public void MonsterDrain()
    {
        //Monster drain
        if (monster.player.GetButtonDown("Drain") && !monster.isDraining)
        {
            monster.isDraining = true;

            //Reset Material to default just in case
            meshRender.material = defaultMaterial;

            //To stop draining
            StartCoroutine(stopDraining());
        }
        //is in draining mode
        else if (monster.isDraining && !monster.drainCooldown)
        {
            drainCollider.gameObject.GetComponent<MeshRenderer>().enabled = true;
            drainCollider.enabled = true;
        }
        else
        {
            monster.isDrainHitting = false;
            drainCollider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            drainCollider.enabled = false;
        }
    }

    //To stop draining
    private IEnumerator stopDraining()
    {
        //Keep color semi - transparent
        MonsterUI.Instance.SetDrainIcon();

        yield return new WaitForSeconds(monster.drainLength);

        //Keep transparent when on cooldown
        MonsterUI.Instance.SetDrainIcon();

        monster.isDrainHitting = false;
        monster.drainCooldown = true;

        yield return new WaitForSeconds(monster.drainCooldownLength);

        //Turn off
        MonsterUI.Instance.SetDrainIcon(1.0f);

        monster.isDraining = false;
        monster.drainCooldown = false;
    }
}
