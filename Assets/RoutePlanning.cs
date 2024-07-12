using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using System;

// Get the location of the floor and every furniture in the room.
// Given the start point, end point and robot's configurations (like the robot's width), plan a feasible path.
// TODO: 1. When accessing the room and furniture's location, all we get is axis-aligned bounding box (so sometimes it may return 'no feasible path found' even if there exists one; sometimes the robot may choose a sideway out of the room.)
// So this algorithm works better when 1. The room and furnitures are all axis-aligned.

public class RoutePlanning : MonoBehaviour
{
    public MRUK mrukScript;

    public GameObject floor;
    List<Vector3> floorRange;
    List<List<Vector3>> furnitureRange = new List<List<Vector3>>();


    // Start is called before the first frame update
    void Start()
    {
        mrukScript = gameObject.GetComponent<MRUK>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetFullFloorSpace(float edgeReduce){
        MRUKRoom currentRoom = mrukScript.GetRooms()[0];
        floor = currentRoom.gameObject.transform.Find("FLOOR").gameObject;

        if (floor.transform.childCount == 1){
            GameObject floorEffectMesh = floor.transform.GetChild(0).gameObject;
            Bounds floorBounds = floorEffectMesh.GetComponent<MeshCollider>().bounds;
            // Debug.Log("Floor bounds: " + floorBounds.min + " " + floorBounds.max);
            floorRange = new List<Vector3>{floorBounds.min + new Vector3(edgeReduce, 0, edgeReduce), floorBounds.max - new Vector3(edgeReduce, 0, edgeReduce)};
        }
        else {
            // Debug.Log("!!!!!! target FLOOR furniture has more than one child or no child !!!!!!" + floor.transform.childCount);
        }
    }

    public void GetAllFurnitureSpace(){
        furnitureRange.Clear();
        MRUKRoom currentRoom = mrukScript.GetRooms()[0];
        foreach (Transform child in currentRoom.gameObject.transform){
            // Debug.Log("child name: " + child.name);
            if ((child.name != "FLOOR") && (child.name != "WALL_FACE") && (child.name != "WINDOW_FRAME") && (child.name != "CEILING") && (child.name != "DOOR_FRAME") && (child.name != "WALL_ART")){
                // Debug.Log("child name: " + child.name);
                Bounds furnitureBounds = child.GetChild(0).gameObject.GetComponent<BoxCollider>().bounds;
                // Debug.Log("child range: " + furnitureBounds.min + " " + furnitureBounds.max);
                List<Vector3> furnitureVectorRange = new List<Vector3>{furnitureBounds.min, furnitureBounds.max};
                furnitureRange.Add(furnitureVectorRange);
            }
        }
    }


    // New ========================================


    public List<Vector3> FindPath(Vector3 startPosition, Vector3 targetPosition, float robotWidth, float gridResolution, float roomEdgeReduce=0.2f)
    {
        GetFullFloorSpace(roomEdgeReduce);
        GetAllFurnitureSpace();

        List<Vector3> floor = floorRange;
        List<List<Vector3>> furnitures = furnitureRange;


        Vector2 gridMin = new Vector2(floor[0].x, floor[0].z);
        Vector2 gridMax = new Vector2(floor[1].x, floor[1].z);

       
        // debugShowGrid.ShowFurniture(debugShowGrid.lineRenderer, furnitures); // debug: show the furniture's boundary

        Vector2Int startGridPos = WorldToGridInt(startPosition, gridMin, gridResolution);
        Vector2Int targetGridPos = WorldToGridInt(targetPosition, gridMin, gridResolution);

        int gridWidth = Mathf.CeilToInt((gridMax.x - gridMin.x) / gridResolution);
        int gridHeight = Mathf.CeilToInt((gridMax.y - gridMin.y) / gridResolution);

        Debug.Log("gridWidth: " + gridWidth + " gridHeight: " + gridHeight);

        bool[,] grid = new bool[gridWidth, gridHeight];

        // Initialize grid with open cells (true)
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = true;
            }
        }

        // Mark furniture cells as blocked (false)
        foreach (var furniture in furnitures)
        {
            Vector2Int furnitureMin = WorldToGridInt(furniture[0] - new Vector3(robotWidth * 0.1f, 0, robotWidth * 0.1f), gridMin, gridResolution);
            Vector2Int furnitureMax = WorldToGridInt(furniture[1] + new Vector3(robotWidth * 0.1f, 0, robotWidth * 0.1f), gridMin, gridResolution);
            for (int x = Math.Max(0, furnitureMin.x); (x <= furnitureMax.x) && (x < gridWidth); x++)
            {
                for (int y = Math.Max(0, furnitureMin.y); (y <= furnitureMax.y) && (y < gridHeight); y++)
                {
                    grid[x, y] = false;
                }
            }
        }

        // debugShowGrid.ShowGrid(debugShowGrid.lineRenderer, gridMin, gridMax, gridResolution, grid); // debug: show the room's boundary

        // Find path using A* algorithm
        List<Vector3> path = AStarPathfinding(grid, startGridPos, targetGridPos, gridMin, gridResolution, robotWidth);
        path = SmoothPath(path, grid, gridMin, gridResolution);
        
        return path;
    }

    private Vector2 WorldToGrid(Vector3 worldPos, Vector2 gridMin, float gridResolution)
    {
        return new Vector2((worldPos.x - gridMin.x) / gridResolution, (worldPos.z - gridMin.y) / gridResolution);
    }

    private Vector2Int WorldToGridInt(Vector3 worldPos, Vector2 gridMin, float gridResolution)
    {
        Vector2 gridPos = WorldToGrid(worldPos, gridMin, gridResolution);
        return new Vector2Int(Mathf.RoundToInt(gridPos.x), Mathf.RoundToInt(gridPos.y));
    }

    private List<Vector3> AStarPathfinding(bool[,] grid, Vector2Int start, Vector2Int target, Vector2 gridMin, float gridResolution, float robotWidth)
    {
        List<Vector3> path = new List<Vector3>();

        // Priority queue for the open list
        PriorityQueue<Node> openList = new PriorityQueue<Node>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

        Node startNode = new Node(start, 0, Heuristic(start, target));
        openList.Enqueue(startNode);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();
        costSoFar[start] = 0;

        while (openList.Count > 0)
        {
            Node current = openList.Dequeue();

            if (Vector2Int.Distance(current.Position, target) < 1e-2)
            {
                // Reconstruct path
                Vector2Int currentPos = current.Position;
                while (!currentPos.Equals(start))
                {
                    path.Add(GridToWorld(currentPos, gridMin, gridResolution));
                    currentPos = cameFrom[currentPos];
                }
                path.Add(GridToWorld(start, gridMin, gridResolution));
                path.Reverse();
                return path;
            }

            closedList.Add(current.Position);

            foreach (var neighbor in GetNeighbors(current.Position, grid, robotWidth))
            {
                if (closedList.Contains(neighbor)) continue;

                float newCost = costSoFar[current.Position] + 1; // Assume each move cost is 1

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    float priority = newCost + Heuristic(neighbor, target);
                    openList.Enqueue(new Node(neighbor, newCost, priority));
                    cameFrom[neighbor] = current.Position;
                }
            }
        }

        return new List<Vector3>(); // Return empty list if no path found
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int pos, bool[,] grid, float robotWidth)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        foreach (var dir in directions)
        {
            Vector2Int newPos = pos + dir;
            if (newPos.x >= 0 && newPos.x < grid.GetLength(0) && newPos.y >= 0 && newPos.y < grid.GetLength(1) && grid[newPos.x, newPos.y])
            {
                neighbors.Add(newPos);
            }
        }

        return neighbors;
    }

    private Vector3 GridToWorld(Vector2Int gridPos, Vector2 gridMin, float gridResolution)
    {
        return new Vector3(gridPos.x * gridResolution + gridMin.x, 0, gridPos.y * gridResolution + gridMin.y);
    }

    private class Node : IComparable<Node>
    {
        public Vector2Int Position { get; }
        public float Cost { get; }
        public float Priority { get; }

        public Node(Vector2Int position, float cost, float priority)
        {
            Position = position;
            Cost = cost;
            Priority = priority;
        }

        public int CompareTo(Node other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }

    private class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> elements = new List<T>();

        public int Count => elements.Count;

        public void Enqueue(T item)
        {
            elements.Add(item);
            elements.Sort();
        }

        public T Dequeue()
        {
            T item = elements[0];
            elements.RemoveAt(0);
            return item;
        }
    }

    private int GetTurnDirection(Vector3 currentPoint, Vector3 turningPoint1, Vector3 turningPoint2)
    {
        Vector3 dir1 = turningPoint1 - currentPoint;
        Vector3 dir2 = turningPoint2 - turningPoint1;

        Vector3 cross = Vector3.Cross(dir1, dir2);
        if (cross.y > 0)
            return 1;
        else if (cross.y < 0)
            return -1;
        else
            return 0;
    }

    private List<Vector3> SmoothPath_New(List<Vector3> path, bool[,] grid, Vector2 gridMin, float gridResolution)
    {
        // from the start point, check if there are two different turns in the path
        // if so, try to flip the path and check if the new path is valid
        if (path.Count < 2)
            return path;
        List<Vector3> smoothPath = new List<Vector3>();
        smoothPath.Add(path[0]);

        int currentStartIndex = 0;
        while (currentStartIndex < path.Count - 3)
        {
            Vector3 currentPoint = path[currentStartIndex];
            Vector3 turningPoint1 = path[currentStartIndex + 1];
            Vector3 turningPoint2 = path[currentStartIndex + 2];
            Vector3 turningPoint3 = path[currentStartIndex + 3];
            int turningDirectionAtPoint1 = GetTurnDirection(currentPoint, turningPoint1, turningPoint2);
            int turningDirectionAtPoint2 = GetTurnDirection(turningPoint1, turningPoint2, turningPoint3);
            if (turningDirectionAtPoint1 != turningDirectionAtPoint2)
            {
                // try to flip the path
                List<Vector3> newPath = new List<Vector3>();
                newPath.Add(currentPoint);
                newPath.Add(currentPoint + (turningPoint2 - turningPoint1));
                newPath.Add(turningPoint2);
                // This is the new path generated. Then we need to check if this path is valid
                bool isValid = true;
                for (int i = 1; i < newPath.Count; i++)
                {
                    if (!LineOfSight(newPath[i - 1], newPath[i], grid, gridMin, gridResolution))
                    {
                        isValid = false;
                        break;
                    }
                }
                // if valid, we need to update "path" (not "smoothPath" because we may smooth the path again considering the following points)
                if (isValid)
                {
                    path.RemoveRange(currentStartIndex + 1, 2);
                    path.InsertRange(currentStartIndex + 1, newPath.GetRange(1, 1));
                }
                else
                {
                    // if not valid, we need to add the next point to the smoothPath, and start from the next point
                    smoothPath.Add(path[currentStartIndex + 1]);
                    currentStartIndex++;
                }
            }
            else
            {
                // No smooth needed
                smoothPath.Add(path[currentStartIndex + 1]);
                currentStartIndex++;
            }   
        }
        return smoothPath;
    }

    private List<Vector3> SmoothPath(List<Vector3> path, bool[,] grid, Vector2 gridMin, float gridResolution)
    {
        if (path.Count < 2)
            return path;

        List<Vector3> smoothPath = new List<Vector3>();
        smoothPath.Add(path[0]);

        int currentIndex = 0;
        while (currentIndex < path.Count - 1)
        {
            int nextIndex = currentIndex + 1;
            while (nextIndex < path.Count && LineOfSight(smoothPath[smoothPath.Count - 1], path[nextIndex], grid, gridMin, gridResolution))
            {
                nextIndex++;
            }

            smoothPath.Add(path[nextIndex - 1]);
            currentIndex = nextIndex - 1;
        }
        Debug.Log("+++++++++ smoothed path length: " + smoothPath.Count);
       return smoothPath;
   }

   private bool LineOfSight(Vector3 start, Vector3 end, bool[,] grid, Vector2 gridMin, float gridResolution)
   {
       Vector2 startGrid = WorldToGrid(start, gridMin, gridResolution);
       Vector2 endGrid = WorldToGrid(end, gridMin, gridResolution);

       int x0 = Mathf.FloorToInt(startGrid.x);
       int y0 = Mathf.FloorToInt(startGrid.y);
       int x1 = Mathf.FloorToInt(endGrid.x);
       int y1 = Mathf.FloorToInt(endGrid.y);

       int dx = Math.Abs(x1 - x0);
       int dy = Math.Abs(y1 - y0);

       int sx = x0 < x1 ? 1 : -1;
       int sy = y0 < y1 ? 1 : -1;

       int err = dx - dy;
       int e2;

       while (true)
       {
           if (!grid[x0, y0])
               return false;

           if (x0 == x1 && y0 == y1)
               break;

           e2 = 2 * err;
           if (e2 > -dy)
           {
               err -= dy;
               x0 += sx;
           }

           if (e2 < dx)
           {
               err += dx;
               y0 += sy;
           }
       }

       return true;
   }

    // ===================================================



    public void RenderPath(LineRenderer lineRenderer, List<Vector3> path){
        if (path == null || path.Count == 0){
            Debug.Log("No feasible path found, or path == null");
            return;
        }
        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());
        lineRenderer.enabled = true;
        return;
    }

}
