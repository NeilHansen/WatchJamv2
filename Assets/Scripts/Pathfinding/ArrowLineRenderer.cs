using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLineRenderer : MonoBehaviour {

    float normailizedHeadPosition = 0.0f;

    void Update () {
        UpdateArrowHead(Time.deltaTime);
    }

    public void UpdateArrowHead(float increment)
    {
        normailizedHeadPosition += increment;
        LineRenderer line = GetComponent<LineRenderer>();
        if (line != null)
        {
            int arrowStartIndex = 2;
            Vector3 arrowheadWorldPosition = Vector3.Lerp(line.GetPosition(0), line.GetPosition(1), Mathf.Clamp(normailizedHeadPosition, 0.0f, 1.0f));
            Vector3 lineDirection = (line.GetPosition(1) - line.GetPosition(0)).normalized;
            Vector3 arrowLeftWorldPosition = arrowheadWorldPosition + Quaternion.Euler(0, -150, 0) * lineDirection * 1.0f;
            Vector3 arrowRightWorldPosition = arrowheadWorldPosition + Quaternion.Euler(0, 150, 0) * lineDirection * 1.0f;
            line.SetPosition(arrowStartIndex, arrowheadWorldPosition);
            line.SetPosition(arrowStartIndex + 1, arrowLeftWorldPosition);
            line.SetPosition(arrowStartIndex + 2, arrowheadWorldPosition);
            line.SetPosition(arrowStartIndex + 3, arrowRightWorldPosition);
            line.SetPosition(arrowStartIndex + 4, arrowheadWorldPosition);
        }
        if (normailizedHeadPosition > 1.0f)
            normailizedHeadPosition = 0.0f;
    }
}
