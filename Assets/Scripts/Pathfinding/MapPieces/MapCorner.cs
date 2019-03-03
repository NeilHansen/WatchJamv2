using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapCorner : AbstractPiece {

	MapCorner()
    {
        type = PieceType.Corner;
        localCenter = Vector3.zero;
        AddOpening(Vector3.forward);
        AddOpening(Vector3.left);
    }
}
