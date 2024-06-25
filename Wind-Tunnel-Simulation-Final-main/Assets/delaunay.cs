using System;
using System.Collections.Generic;
using System.Linq;
using MIConvexHull;
using UnityEngine;

public static class DelaunayTriangulationUtility
{
    public static Vector3[] GetVerticesAfterTriangulate(GameObject model, Vector3 modelPosition, Vector3 modelScale, Quaternion rotationOfModel)
    {
        Debug.Log("Delaunay Triangulation Example started.");

        // Get the mesh from the model
        Mesh mesh = CombineMeshes.CombineAllChildMeshes(model);
        if (mesh == null)
        {
            Debug.LogError("No MeshFilter or SkinnedMeshRenderer found on the model.");
            return null;
        }

        Debug.Log($"Mesh found with {mesh.vertexCount} vertices.");

        // Convert mesh vertices to a list of Vector3 points
        Vector3[] vertices = mesh.vertices;
        List<Vector3> points = vertices.ToList();

        if (points.Count == 0)
        {
            Debug.LogError("No vertices found in the mesh.");
            return null;
        }

        Debug.Log($"Number of vertices to process: {points.Count}");

        // Transform vertices based on the model's position and scale
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = rotationOfModel * points[i];
            points[i] = Vector3.Scale(points[i], modelScale) + modelPosition;
        }

        // Perform Delaunay triangulation
        double planeDistanceTolerance = 0.001;
        var delaunayTriangulation = DelaunayTriangulation<MyVertex, MyCell>.Create(
            points.Select(v => new MyVertex(v.x, v.y, v.z)).ToList(),
            planeDistanceTolerance
        );

        Debug.Log($"Number of cells created: {delaunayTriangulation.Cells.Count()}");

        // Collect the surface triangles
        List<Vector3> surfaceTriangles = new List<Vector3>();
        HashSet<MyFace> boundaryFaces = new HashSet<MyFace>();

        foreach (var cell in delaunayTriangulation.Cells)
        {
            foreach (var face in GetCellFaces(cell))
            {
                if (!boundaryFaces.Add(face)) // If already added, it's an internal face
                {
                    boundaryFaces.Remove(face); // Remove from boundary set if already present
                }
            }
        }

        foreach (var face in boundaryFaces)
        {
            foreach (var vertex in face.Vertices)
            {
                surfaceTriangles.Add(new Vector3((float)vertex.Position[0], (float)vertex.Position[1], (float)vertex.Position[2]));
            }
        }

        Vector3[] triangleVertices = surfaceTriangles.ToArray();

        // Ensure the length of the array is a multiple of 3
        if (triangleVertices.Length % 3 != 0)
        {
            int newSize = (triangleVertices.Length / 3) * 3;
            Array.Resize(ref triangleVertices, newSize);
            Debug.LogWarning($"Trimmed the triangleVertices array to a multiple of 3. New length: {triangleVertices.Length}");
        }

        Debug.Log($"Number of surface triangles: {triangleVertices.Length / 3}");
        Debug.Log("Delaunay Triangulation completed.");

        // Create a new GameObject to visualize the surface mesh
        GameObject newMeshObject = new GameObject("SurfaceMesh");
        MeshFilter meshFilter = newMeshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newMeshObject.AddComponent<MeshRenderer>();

        // Assign a basic material (this requires you to have a material named "Default-Material" in your project)
        Material material = Resources.Load<Material>("Default-Material");
        if (material != null)
        {
            meshRenderer.material = material;
        }
        else
        {
            Debug.LogWarning("Default-Material not found. Please assign a material to the mesh manually.");
        }

        // Create a new mesh
        Mesh surfaceMesh = new Mesh();

        // Set the vertices
        surfaceMesh.vertices = triangleVertices;

        // Create an array for the triangles
        int[] triangles = new int[triangleVertices.Length];
        for (int i = 0; i < triangleVertices.Length; i++)
        {
            triangles[i] = i;
        }

        // Set the triangles
        surfaceMesh.triangles = triangles;

        // Recalculate normals for proper lighting
        surfaceMesh.RecalculateNormals();

        // Assign the new mesh to the MeshFilter
        //meshFilter.mesh = surfaceMesh;

        return triangleVertices;
    }

    private static Mesh GetMeshFromModel(GameObject obj)
    {
        // Get the Mesh from a MeshFilter component if available
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            return meshFilter.sharedMesh; // Return mesh from MeshFilter
        }

        // Get the Mesh from a SkinnedMeshRenderer component if available
        SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            return skinnedMeshRenderer.sharedMesh; // Return mesh from SkinnedMeshRenderer
        }

        // Recursively search for a mesh in the child objects
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Mesh mesh = GetMeshFromModel(obj.transform.GetChild(i).gameObject);
            if (mesh != null)
            {
                return mesh;
            }
        }

        return null; // Return null if no mesh is found
    }

    private static IEnumerable<MyFace> GetCellFaces(MyCell cell)
    {
        var vertices = cell.Vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            yield return new MyFace(vertices.Where((v, idx) => idx != i).ToArray());
        }
    }

    public class MyVertex : IVertex
    {
        public double[] Position { get; set; } // Position of the vertex in 3D space

        public MyVertex(double x, double y, double z)
        {
            Position = new double[] { x, y, z }; // Initialize the position
        }
    }

    public class MyCell : TriangulationCell<MyVertex, MyCell> { }

    public class MyFace
    {
        public IEnumerable<MyVertex> Vertices { get; }

        public MyFace(IEnumerable<MyVertex> vertices)
        {
            Vertices = vertices;
        }

        public override bool Equals(object obj)
        {
            if (obj is MyFace otherFace)
            {
                return Vertices.OrderBy(v => v.Position[0])
                               .ThenBy(v => v.Position[1])
                               .ThenBy(v => v.Position[2])
                               .SequenceEqual(otherFace.Vertices.OrderBy(v => v.Position[0])
                                                                .ThenBy(v => v.Position[1])
                                                                .ThenBy(v => v.Position[2]));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Vertices.OrderBy(v => v.Position[0])
                           .ThenBy(v => v.Position[1])
                           .ThenBy(v => v.Position[2])
                           .Aggregate(17, (current, vertex) => current * 23 + vertex.GetHashCode());
        }
    }
}
