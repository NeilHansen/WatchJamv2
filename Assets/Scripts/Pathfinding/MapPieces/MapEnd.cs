using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapEnd : AbstractPiece
{
    MapEnd()
    {
        type = PieceType.End;
        localCenter = new Vector3(0.0f, 0.0f, -1.5f);
        AddOpening(Vector3.back);
        travelCost = 0.0f;
    }
}
