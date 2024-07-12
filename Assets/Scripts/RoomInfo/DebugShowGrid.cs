using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugShowGrid : MonoBehaviour
{
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGrid(LineRenderer lineRenderer, Vector2 gridMin, Vector2 gridMax, float gridResolution, bool[,] grid) 
    {
        // Calculate the number of horizontal and vertical lines 
        int numHorizontalLines = Mathf.CeilToInt((gridMax.y - gridMin.y) / gridResolution) + 1; 
        int numVerticalLines = Mathf.CeilToInt((gridMax.x - gridMin.x) / gridResolution) + 1; 
        // Calculate total number of points needed 
        int totalPoints = 2 * (numHorizontalLines + numVerticalLines); 
        // Create an array to hold the positions of the line renderer
        Vector3[] positions = new Vector3[totalPoints]; 
        int index = 0; 
        // Add horizontal lines
        for (int i = 0; i < numHorizontalLines; i++) 
        { 
            float y = gridMin.y + i * gridResolution; 
            if (i % 2 == 0){
                positions[index++] = new Vector3(gridMin.x, 0, y);
                positions[index++] = new Vector3(gridMax.x, 0, y); 
            }
            else{
                positions[index++] = new Vector3(gridMax.x, 0, y);
                positions[index++] = new Vector3(gridMin.x, 0, y);
            }
        } 
        // Add vertical lines 
        for (int i = 0; i < numVerticalLines; i++) 
        { 
            float x = gridMin.x + i * gridResolution; 
            if (i % 2 == 0){
                positions[index++] = new Vector3(x, 0, gridMin.y);
                positions[index++] = new Vector3(x, 0, gridMax.y); 
            }
            else{
                positions[index++] = new Vector3(x, 0, gridMax.y);
                positions[index++] = new Vector3(x, 0, gridMin.y);
            }
        } 
        // Set the positions to the line renderer 
        lineRenderer.positionCount = totalPoints; 
        lineRenderer.SetPositions(positions);
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.enabled = true;

        // check each grid cell, if it is false, add a small cube at the middle of the cell
        // First create an empty object called 'GridAvailable' in the scene, then all small cubes will be added as its children. 
        // First check if the object 'GridAvailable' exists, if so, clear all its children
        // GameObject gridAvailable = GameObject.Find("GridAvailable");
        // if (gridAvailable != null)
        // {
        //     foreach (Transform child in gridAvailable.transform)
        //     {
        //         Destroy(child.gameObject);
        //     }
        // }
        // else
        // {
        //     gridAvailable = new GameObject("GridAvailable");
        // }
        // for (int i = 0; i < grid.GetLength(0); i++)
        // {
        //     for (int j = 0; j < grid.GetLength(1); j++)
        //     {
        //         if (!grid[i, j])
        //         {
        //             GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //             cube.transform.position = new Vector3(gridMin.x + i * gridResolution + gridResolution / 2, 0.1f, gridMin.y + j * gridResolution + gridResolution / 2);
        //             cube.transform.localScale = new Vector3(gridResolution * 0.5f, gridResolution * 0.5f, gridResolution * 0.5f);
        //             cube.transform.parent = gridAvailable.transform;
        //         }
        //     }
        // }
                    
    }

    public void ShowFurniture(LineRenderer lr, List<List<Vector3>> furnitures)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (var furniture in furnitures)
        {
            positions.Add(furniture[0]);
            positions.Add(new Vector3(furniture[0].x, 0, furniture[1].z));
            positions.Add(furniture[1]);
            positions.Add(new Vector3(furniture[1].x, 0, furniture[0].z));
            positions.Add(furniture[0]);
            // add a nan to separate different furnitures
            // positions.Add(new Vector3(float.NaN, float.NaN, float.NaN));
            // break;
        }
        lr.positionCount = positions.Count;
        lr.SetPositions(positions.ToArray());
        lr.enabled = true;
    }

}
