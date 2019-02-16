using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public class Edge : IEquatable<Edge>
    {
        public PathTest startNode;
        public PathTest endNode;

        public Edge(PathTest startNode, PathTest endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }

        public bool Equals(Edge other)
        {
            return other != null &&
                   EqualityComparer<PathTest>.Default.Equals(startNode, other.startNode) &&
                   EqualityComparer<PathTest>.Default.Equals(endNode, other.endNode);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(Edge edge1, Edge edge2)
        {
            return EqualityComparer<Edge>.Default.Equals(edge1, edge2);
        }

        public static bool operator !=(Edge edge1, Edge edge2)
        {
            return !(edge1 == edge2);
        }
    }
    public List<Edge> edges;
    public PathTest[] allNodes;
    List<PathTest> visited3;
    List<Edge> visited2;
    private GameObject ArrowHolder;
    public int destinationRoom = 0;

    
    // Use this for initialization
    void Start()
    {
        edges = new List<Edge>();
        if (Application.isPlaying)
        {
            visited3 = new List<PathTest>();
            foreach (PathTest t in allNodes)
            {
                AddEdge(t, t.ob2);
            }
            List<Edge> edge = GetEdge(allNodes[destinationRoom]);
            foreach(Edge e in edge)
            {
               // createArrow2(e.startNode, e.endNode);

            }
            StartSearch();
            // createArrow(RoomNode[destinationRoom].ob2, RoomNode[destinationRoom]);
            // createArrow(RoomNode[destinationRoom].ob2.ob2, RoomNode[destinationRoom].ob2);
        }
    }
    [ContextMenu("Start Search")]
    public void StartSearch()
    {
        Breadth_First_Search(allNodes[destinationRoom]);

    }
    // Update is called once per frame
    void Update ()
    {
		
	}
    List<Edge> GetEdge(PathTest node)
    {
        List<Edge> Tedges = new List<Edge>();
        foreach (Edge e in edges)
        {
            if (e.startNode == node || e.endNode == node)
            {
                Tedges.Add(e);
              
            }
        }
        return Tedges;
    }
    void AddEdge(PathTest startNode, PathTest endNode)
    {
        if(startNode != null && endNode != null)
        {
            edges.Add(new Edge(startNode, endNode));

        }
    }
    public List<PathTest> getNeighbours2(PathTest toCheck)
    {
        List<PathTest> list = new List<PathTest>();
        foreach (PathTest t in allNodes)
        {

            if (t.ob2 == toCheck)
            {
                list.Add(t);
            }
        }

        return list;
    }
    public List< PathTest>  getNeighbours(PathTest toCheck)
    {
        List<PathTest> list = new List<PathTest>();
        List<Edge> edge = GetEdge(toCheck);
        foreach (Edge e in edge)
        {
            if(e.startNode != toCheck)
            {
                list.Add(e.startNode);
            }
            else if(e.endNode != toCheck)
            {
                list.Add(e.endNode);

            }
        }
        
        return list;
    }

    public void Breadth_First_Search(PathTest start)
    {
        Queue<PathTest> frontier = new Queue<PathTest>();
        frontier.Enqueue(start);

        Dictionary<PathTest,PathTest> cameFrom = new Dictionary<PathTest, PathTest>();
        cameFrom.Add(start,null);
     //   print("starting ");

        while (frontier.Count != 0)
        {
            PathTest current = frontier.Peek();
            frontier.Dequeue();
           // print("visiting " + current.name);
            foreach (PathTest next in getNeighbours(current))
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                   
                }
            }
        }
        if(ArrowHolder!= null)
        {
            Destroy(ArrowHolder);
        }
        ArrowHolder = new GameObject("arrow holder");
        ArrowHolder.transform.SetParent(this.transform);
        foreach (KeyValuePair<PathTest, PathTest> link in cameFrom)
        {
            if( link.Key != null && link.Value != null)
            {
               // print(link.Key.name + " came from " + link.Value.name);

            }

            createArrow2(link.Value, link.Key);
        }
    }
    public void createArrow2(PathTest start, PathTest end)
    {
        if (Application.isPlaying)
        {

            GameObject arrow;
            if (start != null && end != null)
            {
                arrow = new GameObject("arrow");
                LineRenderer line = arrow.AddComponent<LineRenderer>();
                arrowrenderer render = arrow.AddComponent<arrowrenderer>();
                line.startColor = end.lineColor;
                line.endColor = end.lineColor;
                render.ArrowWidth = end.lineWidth * 2f;
                line.alignment = LineAlignment.TransformZ;

                line.material = end.lineMaterial;
                arrow.transform.position = end.transform.position;
                arrow.transform.rotation = Quaternion.Euler(90,0,0);
                arrow.transform.SetParent(ArrowHolder.transform);
                render.ArrowOrigin = end.transform.position;
                render.ArrowTarget = start.transform.position;
                render.ArrowCurrent = render.ArrowTarget;



            }
        }

    }



    public void createArrow(PathTest start, PathTest end, int rLevel = 0)
    {
        if (Application.isPlaying)
        {
           
            GameObject arrow;
            if(start!= null && end != null)
            {
                arrow = new GameObject("arrow");
                LineRenderer line = arrow.AddComponent<LineRenderer>();
                arrowrenderer render = arrow.AddComponent<arrowrenderer>();
                line.startColor = end.lineColor;
                line.endColor = end.lineColor;
                line.startWidth = end.lineWidth * 2;
                line.endWidth = end.lineWidth * 2;
                line.material = end.lineMaterial;
                arrow.transform.position = start.transform.position;
                arrow.transform.SetParent(this.transform);
                render.ArrowOrigin = start.transform.position;
                render.ArrowTarget = end.transform.position;
                render.ArrowCurrent = render.ArrowTarget;

                visited3.Add(start);
                visited3.Add(end);

                if (rLevel < 10)
                {
                    if (start.ob2 != null)
                    {
                        createArrow(start.ob2, start, rLevel++);
                    }
                  
                    foreach (PathTest t in getNeighbours(start))
                    {
                        if (!visited3.Contains(t) && t!= start.ob2)
                        {
                            if (t != null && t.ob2 != null)
                            {
                                createArrow(t, t.ob2, rLevel++);
                            }
                        }

                    }
                   
                }
            }              
        }

    }


}
