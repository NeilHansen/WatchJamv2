using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapManager : MonoBehaviour {

    public static MapManager Instance;

    //Debug variables
    List<Color> debugLineColours = new List<Color>();
    [SerializeField]
    AbstractPiece pathStart;
    [SerializeField]
    AbstractPiece pathEnd;

    //Map graph variables
    List<AbstractPiece> MapGraph = new List<AbstractPiece>();
    int mapLayer;
    //Pathfinding variables
    bool[] visited;
    bool[] neighbourAdded;
    List<AbstractPiece> neighbouring = new List<AbstractPiece>();
    public GameObject arrowLinePrefab;
    List<GameObject> arrowLineRenderers = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start () {
        mapLayer = LayerMask.GetMask("MapPiece"); 

        debugLineColours.Add(Color.red);
        debugLineColours.Add(Color.blue);
        debugLineColours.Add(Color.green);
        debugLineColours.Add(Color.magenta);
        debugLineColours.Add(Color.cyan);
        debugLineColours.Add(Color.yellow);

        foreach (Transform mapPiece in GameObject.Find("LevelWalls").transform)
        {
            AbstractPiece mapPieceScript = mapPiece.GetComponent<AbstractPiece>();
            mapPieceScript.graphListIndex = MapGraph.Count;
            MapGraph.Add(mapPieceScript);
        }

        Debug.Log(MapGraph.Count + " map pieces found.");

        visited = new bool[MapGraph.Count];
        neighbourAdded = new bool[MapGraph.Count];

        GenerateMapGraph();

    }
	
    //Generate map graph
    void GenerateMapGraph()
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
                    if (Physics.Raycast(ray, out hit, 20, mapLayer, QueryTriggerInteraction.Collide))
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
                        AbstractPiece.CreateConnection(mapPiece, i, neighbourPiece, neighbourPieceDirection);

                        //Debug.DrawLine(rayStartPoint, neighbourPiece.transform.TransformPoint(neighbourPiece.localCenter), debugLineColours[lineColourCount % 6], 1000.0f);
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

    //A* pathfinding function, nagivate the path with pathfindingNext and pathfindingPrevious variables in AbstractPiece
    //Once function is called, previous path will be forgotten
    //Returns true if path was found, returns false elsewise (including conditions where start equals end)
    bool FindPath(AbstractPiece start, AbstractPiece end)
    {
        //Create arrays to track pathfinding progress
        for (int i = 0; i < MapGraph.Count; i++)
        {
            MapGraph[i].ResetPathfindingVariables();
            visited[i] = false;
            neighbourAdded[i] = false;
        }

        //Start equals end return immediately
        if (start.Equals(end))
        {
            Debug.Log("Pathfinding start equals end.");
            return false;
        }
            
        //Manually visit start
        VisitMapPiece(start, end);
        //A* pathfinding
        bool foundEnd = false;
        while(!foundEnd && neighbouring.Count != 0)
        {
            int lowestCostIndex = -1;
            int lowestHeuristicIndex = -1;
            for (int j = 0; j < neighbouring.Count; j++)
            {
                //First neighbour or found neighbour with lower accumulated cost
                if (lowestCostIndex == -1 || neighbouring[j].accumulatedCost < neighbouring[lowestCostIndex].accumulatedCost)
                {
                    lowestCostIndex = j;
                    lowestHeuristicIndex = j;
                    continue;
                }

                //If same accumulated cost then compare heuristics
                if (neighbouring[j].accumulatedCost == neighbouring[lowestCostIndex].accumulatedCost)
                {
                    if (neighbouring[j].heuristic < neighbouring[lowestHeuristicIndex].heuristic)
                        lowestHeuristicIndex = j;
                }
            }
            foundEnd = VisitMapPiece(neighbouring[lowestHeuristicIndex], end);
            neighbouring.RemoveAt(lowestHeuristicIndex);
        }
        
        if (foundEnd)
        {
            
            AbstractPiece current = end;
            while (current.pathfindingPrevious != null)
            {
                current.pathfindingPrevious.pathfindingNext = current;
                current = current.pathfindingPrevious;
            }
            
        }
        else
        {
            Debug.LogError("No path from " + start.name + " to " + end.name + " exists.");
        }

        return foundEnd;
    }

    //Helper function for A* pathfinding, only called in FindPath()
    //Returns true if a neighbour is the end, else false
    bool VisitMapPiece(AbstractPiece toVisit, AbstractPiece end)
    {
        visited[toVisit.graphListIndex] = true;
        foreach (AbstractPiece neighbour in toVisit.neighbourPieces)
        {
            //If visited skip this iteration
            if (visited[neighbour.graphListIndex])
                continue;

            float costToNeighbour = toVisit.accumulatedCost + (toVisit.travelCost + neighbour.travelCost) * 0.5f;

            if (!neighbourAdded[neighbour.graphListIndex])
            {
                //Add to neighbour list if is new neighbour
                neighbouring.Add(neighbour);
                neighbourAdded[neighbour.graphListIndex] = true;
            }
            else if (neighbour.accumulatedCost < costToNeighbour)
            {
                //Skip value updates if previous path to here is shorter
                continue;
            }
            //Update cost/heuristic and the previous map piece to get here
            neighbour.accumulatedCost = costToNeighbour;
            neighbour.heuristic = Vector3.Distance(neighbour.transform.TransformPoint(neighbour.localCenter), end.transform.TransformPoint(end.localCenter));
            neighbour.pathfindingPrevious = toVisit;

            //Check if neightbour is destination
            if (neighbour.Equals(end))
            {
                return true;
            }
        }
        return false;
    }

    void ShowPath(AbstractPiece start)
    {
        AbstractPiece current = start;
        int lineColourCount = 0;
        while (current.pathfindingNext != null)
        {
            arrowLineRenderers.AddRange(AbstractPiece.ShowConnectionToNext(current, current.pathfindingNext, arrowLinePrefab));
            //Debug.DrawLine(current.transform.TransformPoint(current.localCenter), current.pathfindingNext.transform.TransformPoint(current.pathfindingNext.localCenter), debugLineColours[lineColourCount % 6], 1000.0f);
            current = current.pathfindingNext;
            lineColourCount++;
        }
    }

    public void ChangePathStart(int index)
    {
        pathStart = MapGraph[index];
        if (pathStart != null && pathEnd != null)
        {
            if (FindPath(pathStart, pathEnd))
                ShowPath(pathStart);
        }
    }

    public void ChangePathEnd(int index)
    {
        pathEnd = MapGraph[index];
        if (pathStart != null && pathEnd != null)
        {
            if (FindPath(pathStart, pathEnd))
                ShowPath(pathStart);
        }
    }
}
