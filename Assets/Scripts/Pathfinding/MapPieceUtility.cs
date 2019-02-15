using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapPieceUtility
{

    public enum PieceType
    {
        End = 0,
        Straight,
        Corner,
        TIntersection,
        XIntersection,
        Curve
    }

    public abstract class AbstractPiece : MonoBehaviour
    {
        //Info variables
        [HideInInspector]
        public PieceType type;
        [HideInInspector]
        public Vector3 localCenter;
        public readonly List<Vector3> openings = new List<Vector3>();
        public readonly List<bool> openingConnected = new List<bool>();
        public readonly List<AbstractPiece> neighbourPieces = new List<AbstractPiece>();
        [HideInInspector]
        public int graphListIndex = -1;

        //Pathfinding variables
        [HideInInspector]
        public float travelCost = 1.0f;
        //[HideInInspector]
        public float accumulatedCost = 0.0f;
        [HideInInspector]
        public float heuristic = 0.0f;
        //[HideInInspector]
        public AbstractPiece pathfindingPrevious = null;
        //[HideInInspector]
        public AbstractPiece pathfindingNext = null;

        protected void AddOpening(Vector3 direction)
        {
            openings.Add(direction);
            openingConnected.Add(false);
            neighbourPieces.Add(null);
        }

        public bool HasAllConnections()
        {
            foreach (bool isConnected in openingConnected)
            {
                if (!isConnected)
                {
                    return false;
                }
            }
            return true;
        }

        public int DirectionTowardsIncomingVector(Vector3 incomingVector)
        {
            for (int i = 0; i < openings.Count; i++)
            {
                //Convert local opening directions to world vectors
                Vector3 worldDirectionVector = transform.rotation * openings[i];
                //If the angle between incoming and outgoing vector is 180, it is the correct outgoing vector
                if (Mathf.Abs(Vector3.Angle(incomingVector, worldDirectionVector)) > 170.0f)
                {
                    return i;
                }
            }
            //Error here
            return -1;
        }

        public void ResetPathfindingVariables()
        {
            accumulatedCost = 0.0f;
            pathfindingPrevious = null;
            pathfindingNext = null;
        }

        public static void CreateConnection(AbstractPiece piece1, int openingListIndex1, AbstractPiece piece2, int openingListIndex2)
        {
            if (openingListIndex1 >= piece1.openings.Count || openingListIndex2 >= piece2.openings.Count)
            {
                Debug.LogError("Index out of bound when generating map graph.");
                return;
            }
            piece1.openingConnected[openingListIndex1] = true;
            piece1.neighbourPieces[openingListIndex1] = piece2;
            piece2.openingConnected[openingListIndex2] = true;
            piece2.neighbourPieces[openingListIndex2] = piece1;
        }

        public static LineRenderer ShowConnectionToNext(AbstractPiece current, AbstractPiece next)
        {
            LineRenderer line = current.gameObject.AddComponent<LineRenderer>();
            line.widthMultiplier = 0.2f;
            line.positionCount = 2;
            line.SetPosition(0, current.transform.TransformPoint(next.localCenter));
            line.SetPosition(1, current.transform.TransformPoint(next.localCenter));
            return line;
        }
    }
}
