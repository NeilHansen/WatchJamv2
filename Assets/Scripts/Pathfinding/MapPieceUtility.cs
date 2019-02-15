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

        public static List<GameObject> ShowConnectionToNext(AbstractPiece current, AbstractPiece next, GameObject linePrefab)
        {
            //Notes:
            //Assume arc radius is r.
            //Curve arc center local position: localCenter + (-r, 0, r)
            //Angle of 2 outer arcs: (3.0f * Mathf.PI - 4.0f) / (36.0f * Mathf.PI), angle of 4 inner arcs: (3.0f * Mathf.PI + 2.0f) / (36.0f * Mathf.PI)

            List<GameObject> lines = new List<GameObject>();

            //LineRenderer line = current.gameObject.AddComponent<LineRenderer>();
            //line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //line.receiveShadows = false;
            //line.widthMultiplier = 0.2f;
            if (current.type == PieceType.Curve)
            {
                float innerArcAngle = (3.0f * Mathf.PI + 2.0f) / (36.0f * Mathf.PI);
                Vector3[] tempPoints = new Vector3[4];
                Vector3 leavingCurveVector = next.transform.TransformPoint(next.localCenter) - current.transform.TransformPoint(current.localCenter);
                if (current.DirectionTowardsIncomingVector(-leavingCurveVector) == 0)
                {
                    //Leaving curve towards local up
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 point = current.localCenter + 7.5f * new Vector3(-1.0f + Mathf.Cos((-0.25f + innerArcAngle * i) * Mathf.PI), 0.0f, 1.0f + Mathf.Sin((-0.25f + innerArcAngle * i) * Mathf.PI));
                        tempPoints[i] = current.transform.TransformPoint(point);
                        //line.SetPosition(i, current.transform.TransformPoint(point));
                    }
                }
                else if (current.DirectionTowardsIncomingVector(-leavingCurveVector) == 1)
                {
                    //Leaving curve towards local left
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 point = current.localCenter + 7.5f * new Vector3(-1.0f + Mathf.Cos((-0.25f - innerArcAngle * i) * Mathf.PI), 0.0f, 1.0f + Mathf.Sin((-0.25f - innerArcAngle * i) * Mathf.PI));
                        tempPoints[i] = current.transform.TransformPoint(point);
                        //line.SetPosition(i, current.transform.TransformPoint(point));
                    }
                }
                tempPoints[3] = next.transform.TransformPoint(next.localCenter);
                //line.SetPosition(3, next.transform.TransformPoint(next.localCenter));
                for (int i = 0; i < tempPoints.Length - 1; i++)
                {
                    GameObject newLine = Instantiate(linePrefab);
                    LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
                    lineRenderer.SetPosition(0, tempPoints[i]);
                    lineRenderer.SetPosition(1, tempPoints[i + 1]);
                    lines.Add(newLine);
                }
            }
            else if (next.type == PieceType.Curve)
            {
                float outerArcAngle = (3.0f * Mathf.PI - 4.0f) / (36.0f * Mathf.PI);
                float innerArcAngle = (3.0f * Mathf.PI + 2.0f) / (36.0f * Mathf.PI);
                Vector3[] tempPoints = new Vector3[4];
                tempPoints[0] = current.transform.TransformPoint(current.localCenter);
                //line.SetPosition(0, current.transform.TransformPoint(current.localCenter));
                Vector3 towardsCurveVector = next.transform.TransformPoint(next.localCenter) - current.transform.TransformPoint(current.localCenter);
                if (next.DirectionTowardsIncomingVector(towardsCurveVector) == 0)
                {
                    //Coming into curve from local up
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 point = next.localCenter + 7.5f * new Vector3(-1.0f + Mathf.Cos((-outerArcAngle - innerArcAngle * i) * Mathf.PI), 0.0f, 1.0f + Mathf.Sin((-outerArcAngle - innerArcAngle * i) * Mathf.PI));
                        tempPoints[i + 1] = next.transform.TransformPoint(point);
                        //line.SetPosition(i + 1, next.transform.TransformPoint(point));
                    }
                }
                else if (next.DirectionTowardsIncomingVector(towardsCurveVector) == 1)
                {
                    //Coming into curve from local left
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 point = next.localCenter + 7.5f * new Vector3(-1.0f + Mathf.Cos((-0.5f + outerArcAngle + innerArcAngle * i) * Mathf.PI), 0.0f, 1.0f + Mathf.Sin((-0.5f + outerArcAngle + innerArcAngle * i) * Mathf.PI));
                        tempPoints[i + 1] = next.transform.TransformPoint(point);
                        //line.SetPosition(i + 1, next.transform.TransformPoint(point));
                    }
                }
                for (int i = 0; i < tempPoints.Length - 1; i++)
                {
                    GameObject newLine = Instantiate(linePrefab);
                    LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
                    lineRenderer.SetPosition(0, tempPoints[i]);
                    lineRenderer.SetPosition(1, tempPoints[i + 1]);
                    lines.Add(newLine);
                }
            }
            else
            {
                GameObject newLine = Instantiate(linePrefab);
                LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, current.transform.TransformPoint(current.localCenter));
                lineRenderer.SetPosition(1, next.transform.TransformPoint(next.localCenter));
                lines.Add(newLine);
            }
            return lines;
        }
    }
}
