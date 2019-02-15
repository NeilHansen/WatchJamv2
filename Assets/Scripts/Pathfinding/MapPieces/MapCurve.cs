using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapCurve : AbstractPiece
{
    MapCurve()
    {
        type = PieceType.Curve;
        localCenter = new Vector3(2.0f, 0.0f, -2.0f);
        AddOpening(Vector3.forward);
        AddOpening(Vector3.left);
    }
}
