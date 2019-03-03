using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapTIntersection : AbstractPiece
{
    MapTIntersection()
    {
        type = PieceType.TIntersection;
        localCenter = Vector3.zero;
        AddOpening(Vector3.forward);
        AddOpening(Vector3.back);
        AddOpening(Vector3.left);
    }
}
