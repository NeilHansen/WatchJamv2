using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour {

    public Renderer renderer;
    public MaterialPropertyBlock mpb;
    public float lightInterval;
    Color litColour;
    Color offColour;
    const float intensity = 0.6020513f;
    Texture fullLitTex;
    public Texture[] tranverseTex;

    // Use this for initialization
    void Awake () {
        mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        litColour = renderer.materials[0].GetColor("_EmissionColor");
        offColour = Color.black;
        fullLitTex = renderer.materials[0].GetTexture("_EmissionMap");
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            TraverseLight();
        }
	}

    public void FlashLight()
    {
        StartCoroutine(Flashing());
    }

    public void TraverseLight()
    {
        StartCoroutine(Traversing());
    }

    IEnumerator Flashing()
    {
        mpb.SetColor("_EmissionColor", offColour);
        renderer.SetPropertyBlock(mpb);
        yield return new WaitForSeconds(lightInterval);
        mpb.SetColor("_EmissionColor", litColour);
        renderer.SetPropertyBlock(mpb);
    }

    IEnumerator Traversing()
    {
        foreach (Texture tex in tranverseTex)
        {
            mpb.SetTexture("_EmissionMap", tex);
            renderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(lightInterval);
        }
        mpb.SetTexture("_EmissionMap", fullLitTex);
        renderer.SetPropertyBlock(mpb);
    }
}
