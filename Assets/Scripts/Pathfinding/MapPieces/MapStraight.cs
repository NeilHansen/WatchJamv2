using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapStraight : AbstractPiece
{
    MapStraight()
    {
        type = PieceType.Straight;
        localCenter = Vector3.zero;
        AddOpening(Vector3.forward);
        AddOpening(Vector3.back);
    }
}
