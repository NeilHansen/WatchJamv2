using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapXIntersection : AbstractPiece
{
	MapXIntersection()
    {
        type = PieceType.XIntersection;
        localCenter = Vector3.zero;
        AddOpening(Vector3.forward);
        AddOpening(Vector3.back);
        AddOpening(Vector3.left);
        AddOpening(Vector3.right);
    }
}
