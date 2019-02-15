using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapManager : MonoBehaviour {

    List<Color> debugLineColours = new List<Color>();

    List<AbstractPiece> MapGraph = new List<AbstractPiece>();
    int mapLayer;

    // Use this for initialization
    void Start () {
        mapLayer = LayerMask.GetMask("Walkable"); 

        debugLineColours.Add(Color.red);
        debugLineColours.Add(Color.blue);
        debugLineColours.Add(Color.green);
        debugLineColours.Add(Color.magenta);
        debugLineColours.Add(Color.cyan);
        debugLineColours.Add(Color.yellow);

        foreach (Transform mapPiece in transform)
        {
            AbstractPiece mapPieceScript = mapPiece.GetComponent<AbstractPiece>();
            mapPieceScript.graphListIndex = MapGraph.Count;
            MapGraph.Add(mapPieceScript);
        }

        Debug.Log(MapGraph.Count + " map pieces found.");

        CreateConnections();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateConnections()
    {

        int lineColourCount = 0;

        foreach (AbstractPiece mapPiece in MapGraph)
        {
            //Ignore if piece is fully connected
            if (mapPiece.HasAllConnections())
                continue;

            for (int i = 0; i < mapPiece.openingConnected.Count; i++)
            {
                //Only look for connection if not connected
                if (!mapPiece.openingConnected[i])
                {
                    RaycastHit hit;
                    Vector3 rayStartPoint = mapPiece.transform.TransformPoint(mapPiece.localCenter);
                    Vector3 rayDirection = mapPiece.transform.TransformDirection(mapPiece.openings[i]);
                    Ray ray = new Ray(rayStartPoint, rayDirection);
                    if (Physics.Raycast(ray, out hit, 20, mapLayer))
                    {
                        //Debug.Log(mapPiece.name + " at " + mapPiece.transform.position + " looking in direction " + rayDirection + " found " + hit.collider.name);
                        AbstractPiece neighbourPiece = hit.collider.GetComponent<AbstractPiece>();

                        //Hard code fix
                        //****************************************************************
                        if (hit.collider.transform.parent.tag == "MapPiece")
                            neighbourPiece = hit.collider.transform.parent.GetComponent<AbstractPiece>();
                        //****************************************************************

                        int neighbourPieceDirection = neighbourPiece.DirectionTowardsIncomingVector(rayDirection);
                        if (neighbourPieceDirection == -1)
                        {
                            Debug.LogError("Error when getting direction from neighbouring piece.");
                            return;
                        }
                        AbstractPiece.ConnectTwoPieces(mapPiece, i, neighbourPiece, neighbourPieceDirection);

                        Debug.DrawLine(rayStartPoint, neighbourPiece.transform.TransformPoint(neighbourPiece.localCenter), debugLineColours[lineColourCount % 6], 1000.0f);
                        lineColourCount++;
                    }
                    else
                    {
                        Debug.LogError(mapPiece.name + " Error when raycasting for neighbouring piece.");
                        return;
                    }
                }
            }
        }
    }
}
