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
    public enum DrawOptions
    {
        None = 0,
        DrawGraph,
        DrawRooms
    }
    public DrawOptions debugDrawOption = DrawOptions.None;
    public bool debugShowArrow = true;

    //Map graph variables
    List<AbstractPiece> MapGraph = new List<AbstractPiece>();
    List<Room> RoomList = new List<Room>();
    int mapLayer;
    //Pathfinding variables
    bool[] visited;
    bool[] neighbourAdded;
    List<AbstractPiece> neighbouring = new List<AbstractPiece>();
    //Path display variables
    public GameObject arrowLinePrefab;
    List<GameObject> arrowLineRenderers = new List<GameObject>();
    public float lightInterval = 1.5f;

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
            if (mapPieceScript == null)
            {
                Debug.LogError(mapPiece.gameObject.name + " is not a map piece.");
                break;
            }
            mapPieceScript.graphListIndex = MapGraph.Count;
            MapGraph.Add(mapPieceScript);
        }

        Debug.Log(MapGraph.Count + " map pieces found.");

        visited = new bool[MapGraph.Count];
        neighbourAdded = new bool[MapGraph.Count];

        GenerateMapGraph();

        FindRooms();
        
        if (FindPath(pathStart, pathEnd))
            EnablePath(pathStart);
        
    }

    #region Map Generation
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
                        AbstractPiece neighbourPiece = hit.collider.GetComponent<AbstractPiece>();

                        //Hard code fix
                        //****************************************************************

                      
                            if (hit.collider.transform.parent.tag == "MapPiece")
                                neighbourPiece = hit.collider.transform.parent.GetComponent<AbstractPiece>();
                        
                        //****************************************************************

                        int neighbourPieceDirection = neighbourPiece.DirectionTowardsIncomingVector(rayDirection);
                        if (neighbourPieceDirection == -1)
                        {
                            Debug.LogError("Error when getting direction from neighbouring piece." , mapPiece);
                            return;
                        }
                        AbstractPiece.CreateConnection(mapPiece, i, neighbourPiece, neighbourPieceDirection);

                        if (debugDrawOption == DrawOptions.DrawGraph)
                        {
                            Debug.DrawLine(rayStartPoint, neighbourPiece.transform.TransformPoint(neighbourPiece.localCenter), debugLineColours[lineColourCount % 6], 1000.0f);
                        }
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

    //Find rooms in the map
    void FindRooms()
    {
        int lineColourCount = 0;

        foreach (AbstractPiece mapPiece in MapGraph)
        {
            if (mapPiece.roomIndex != -1)
                continue;

            if (mapPiece.neighbourPieces.Count >= 2 && !mapPiece.GetType().Equals(typeof(MapStraight)))
            {
                Room room = null;
                bool found2ndNeighbour = false;
                AbstractPiece start = mapPiece;
                AbstractPiece n1 = mapPiece.neighbourPieces[0];
                AbstractPiece n2 = mapPiece.neighbourPieces[1];
                AbstractPiece nn = null;
                //Loop through all pair neighbours and check if a common secondary neighbour can be found
                for (int i = 0; i < start.neighbourPieces.Count - 1; i++)
                {
                    for (int j = i + 1; j < start.neighbourPieces.Count; j++)
                    {
                        n1 = start.neighbourPieces[i];
                        n2 = start.neighbourPieces[j];
                        //Previously checked if belongs to a room
                        if (n1.roomIndex != -1 || n2.roomIndex != -1)
                        {
                            continue;
                        }
                        foreach (AbstractPiece n1n in n1.neighbourPieces)
                        {
                            foreach (AbstractPiece n2n in n2.neighbourPieces)
                            {
                                //Found common secondary neighbour
                                if (n1n.Equals(n2n) && !n1n.Equals(start))
                                {
                                    room = new Room(RoomList.Count);
                                    room.AddPiece(start);
                                    room.AddPiece(n1);
                                    room.AddPiece(n2);
                                    room.AddPiece(n1n);
                                    if (debugDrawOption == DrawOptions.DrawRooms)
                                    {
                                        Debug.DrawLine(start.transform.position, n1.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                        Debug.DrawLine(start.transform.position, n2.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                        Debug.DrawLine(n1.transform.position, n1n.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                        Debug.DrawLine(n2.transform.position, n1n.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                    }
                                    nn = n1n;
                                    found2ndNeighbour = true;
                                    goto Found2ndNeighbour;
                                }
                            }
                        }
                    }
                }
                Found2ndNeighbour:
                    //If found new room look further to find entire room
                    if (found2ndNeighbour)
                    {
                        //Init new room fronts
                        Queue<RoomFront> roomFronts = new Queue<RoomFront>();
                        roomFronts.Enqueue(new RoomFront(start, n1));
                        roomFronts.Enqueue(new RoomFront(start, n2));
                        roomFronts.Enqueue(new RoomFront(n1, nn));
                        roomFronts.Enqueue(new RoomFront(n2, nn));
                        while(roomFronts.Count > 0)
                        {
                            RoomFront current = roomFronts.Dequeue();
                            bool expendRoom = false;
                            AbstractPiece f1n = null;
                            AbstractPiece f2n = null;
                            //Check if secondary neighbour of one is the neighbour of the other
                            foreach (AbstractPiece f1np in current.front1.neighbourPieces)
                            {
                                //Ignore overlaps
                                if (f1np.Equals(current.front2))
                                    continue;

                                foreach (AbstractPiece f1npn in f1np.neighbourPieces)
                                {
                                    //Ignore overlaps
                                    if (f1np.roomIndex != -1 && f1npn.roomIndex != -1)
                                        continue;

                                    foreach (AbstractPiece f2np in current.front2.neighbourPieces)
                                    {
                                        //Found new pieces to expand room size
                                        if (f1npn.Equals(f2np) && (f2np.roomIndex == -1))
                                        {
                                            f1n = f1np;
                                            f2n = f2np;
                                            expendRoom = true;
                                            goto ExpandRoom;
                                        }
                                    }
                                }
                            }
                            ExpandRoom:
                                //Add new pieces to current room and add new room fronts to queue
                                if (expendRoom)
                                {
                                    room.AddPiece(f1n);
                                    room.AddPiece(f2n);
                                    if (debugDrawOption == DrawOptions.DrawRooms)
                                    {
                                        Debug.DrawLine(current.front1.transform.position, f1n.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                        Debug.DrawLine(current.front2.transform.position, f2n.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                        Debug.DrawLine(f1n.transform.position, f2n.transform.position, debugLineColours[lineColourCount % 6], 1000.0f);
                                    }
                                    roomFronts.Enqueue(new RoomFront(current.front1, f1n));
                                    roomFronts.Enqueue(new RoomFront(current.front2, f2n));
                                    roomFronts.Enqueue(new RoomFront(f1n, f2n));
                                }
                        }

                        RoomList.Add(room);
                        lineColourCount++;
                    }
            }
        }
    }
    #endregion

    #region Pathfinding
    //A* pathfinding function, nagivate the path with pathfindingNext and pathfindingPrevious variables in AbstractPiece
    //Once function is called, previous path will be forgotten
    //Returns true if path was found, returns false elsewise (including conditions where start equals end)
    bool FindPath(AbstractPiece start, AbstractPiece end)
    {
        //Reset arrays to track pathfinding progress
        neighbouring.Clear();
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

    public void ChangePathStart(int index)
    {
        pathStart = MapGraph[index];
        if (pathStart != null && pathEnd != null)
        {
            foreach (GameObject map in arrowLineRenderers)
            {
                Destroy(map);
            }
            arrowLineRenderers.Clear();
            if (FindPath(pathStart, pathEnd))
                EnablePath(pathStart);
        }
    }

    public void ChangePathEnd(int index)
    {
        pathEnd = MapGraph[index];
        if (pathStart != null && pathEnd != null)
        {
            foreach (GameObject map in arrowLineRenderers)
            {
                Destroy(map);
            }
            arrowLineRenderers.Clear();
            if (FindPath(pathStart, pathEnd))
                EnablePath(pathStart);
        }
    }
    #endregion

    #region Path Lighting
    void EnablePath(AbstractPiece start)
    {
        AbstractPiece current = start;
        while (current.pathfindingNext != null)
        {
            if (debugShowArrow)
                arrowLineRenderers.AddRange(AbstractPiece.ShowConnectionToNext(current, current.pathfindingNext, arrowLinePrefab));
            current = current.pathfindingNext;
        }
        StopAllCoroutines();
        StartCoroutine(SendPulseDownPath(start));
        if (current.roomIndex != -1)
        {
            foreach(AbstractPiece piece in RoomList[current.roomIndex].roomPieces)
            {
                piece.StartLightFlash();
            }
        }
    }

    void DisablePath(AbstractPiece start)
    {
        StopAllCoroutines();
        AbstractPiece end = start;
        while (end.pathfindingNext != null)
        {
            end.ResetLight(true);
            end = end.pathfindingNext;
        }
        if (end.roomIndex != -1)
        {
            foreach (AbstractPiece piece in RoomList[end.roomIndex].roomPieces)
            {
                end.ResetLight(true);
            }
        }
        foreach (GameObject map in arrowLineRenderers)
        {
            Destroy(map);
        }
        arrowLineRenderers.Clear();
    }

    IEnumerator SendPulseDownPath(AbstractPiece start)
    {
        while (true)
        {
            yield return new WaitForSeconds(lightInterval);
            start.StartLightPulse();
        }
    }
    #endregion
}
