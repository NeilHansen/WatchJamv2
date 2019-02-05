using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]

public class arrowrenderer : MonoBehaviour
{

    [Tooltip("The percent of the line that is consumed by the arrowhead")]
    [Range(0, 1)]
    public float PercentHead = 0.4f;
    public Vector3 ArrowOrigin;
    public Vector3 ArrowTarget;
    public Vector3 ArrowCurrent;
    public float ArrowWidth = 1;
    private LineRenderer cachedLineRenderer;
    public float speed = 1;
    float lerpTime = 1f;
    float timer = 0f;
    public void Update()
    {
        timer += Time.deltaTime;
        if(timer >= lerpTime)
        {
            timer = 0;
        }
        ArrowCurrent = Vector3.Lerp (ArrowOrigin, ArrowTarget, timer/lerpTime);
      
        UpdateArrow();
    }
    void Start()
    {
        lerpTime = Vector3.Distance(ArrowOrigin, ArrowTarget) * speed;

        UpdateArrow();
    }
    private void OnValidate()
    {
        UpdateArrow();
    }
    [ContextMenu("UpdateArrow")]
    void UpdateArrow()
    {
        float AdaptiveSize = (float)(PercentHead / Vector3.Distance(ArrowOrigin, ArrowCurrent));
        
        if (cachedLineRenderer == null)
        {
            cachedLineRenderer = this.GetComponent<LineRenderer>();
        }
        cachedLineRenderer.positionCount = 4;
        cachedLineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, ArrowWidth/2)
            , new Keyframe(0.999f - AdaptiveSize , ArrowWidth/2)  // neck of arrow
            , new Keyframe(1 - AdaptiveSize , ArrowWidth)  // max width of arrow head
            , new Keyframe(1, 0f));  // tip of arrow
        cachedLineRenderer.SetPositions(new Vector3[] {
               ArrowOrigin
               , Vector3.Lerp(ArrowOrigin, ArrowCurrent, 0.999f - AdaptiveSize)
               , Vector3.Lerp(ArrowOrigin, ArrowCurrent, 1 - AdaptiveSize)
               , ArrowCurrent });
    }
}
