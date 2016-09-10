using UnityEngine;
using System.Collections.Generic;

public class TerrainScript : MonoBehaviour
{

    // size of heightmap should be (power of 2) + 1 so pow is the power 2 is raised to
    public int pow;

    // size of heightmap
    public int size;

    // maximum array index
    public int max;

    // heightmap
    public float[,] heights;

    //normals corresponding with heights
    public Vector3[,] normals;

    public Shader shader;
    public PointLight pointLight;

    // Use this for initialization
    void Start()
    {
        // diamond square algorithm
        size = (int)Mathf.Pow(2, pow) + 1;
        max = size - 1;
        heights = new float[size, size];
        // initialize four corners with various heights
        heights[0, 0] = 0;
        heights[max, 0] = max / 2;
        heights[max, max] = 0;
        heights[0, max] = max / 2;
        DiamondSquare(max);

        // generate mesh for terrain based on heightmap
        MeshFilter m = this.gameObject.AddComponent<MeshFilter>();
        m.mesh = this.CreateMesh();
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
    }

    // Update is called once per frame
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }

    // this implementation of the diamond-square algorithm is based on the tutorial at
    // http://www.playfuljs.com/realistic-terrain-in-130-lines/
    void DiamondSquare(int size)
    {
        int half = size / 2;
        if (half < 1) return;

        for (int y = half; y < max; y += size)
        {
            for (int x = half; x < max; x += size)
            {
                SquareStep(x, y, half, Random.value);
            }
        }
        for (int y = 0; y <= max; y += half)
        {
            for (int x = (y + half) % size; x <= max; x += size)
            {
                DiamondStep(x, y, half, Random.value);
            }
        }
        DiamondSquare(size / 2);
    }

    // wraps coordinates around if they are out of bounds
    float Wrap(int x, int y)
    {
        if (x < 0)
        {
            x += max;
        }
        else if (x > max)
        {
            x = x % max;
        }
        else if (y < 0)
        {
            y += max;
        }
        else if (y > max)
        {
            y = y % max;
        }
        return heights[x, y];
    }

    void SquareStep(int x, int y, int size, float offset)
    {
        heights[x, y] = ((Wrap(x - size, y - size) + Wrap(x + size, y - size) + Wrap(x + size, y + size) + Wrap(x - size, y + size)) / 4) + offset;
    }

    void DiamondStep(int x, int y, int size, float offset)
    {
        heights[x, y] = ((Wrap(x, y - size) + Wrap(x + size, y) + Wrap(x, y + size) + Wrap(x - size, y)) / 4) + offset;
    }

    Mesh CreateMesh()
    {
        Mesh m = new Mesh();
        m.name = "Terrain";
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();
        //      List<Vector3> norms = new List<Vector3>();
        // add vertices in order based on heightmap and add triangle ordering
        // to ensure correct rendering
        // subtract one iteration from each loop because we are adding squares, not single points
        for (int i = 0; i < max - 1; i++)
        {
            for (int j = 0; j < max - 1; j++)
            {
                // add a square to the terrain
                // add vertices based on x and z coordinates
                // y coordinate comes from heightmap
                vertices.Add(new Vector3(i, heights[i, j], j));
                AddColor(colors, heights[i, j]);
                vertices.Add(new Vector3(i, heights[i, j + 1], j + 1));
                AddColor(colors, heights[i, j + 1]);
                vertices.Add(new Vector3(i + 1, heights[i + 1, j], j));
                AddColor(colors, heights[i + 1, j]);
                vertices.Add(new Vector3(i + 1, heights[i + 1, j + 1], j + 1));
                AddColor(colors, heights[i + 1, j + 1]);
                // add triangles in correct order of vertices so terrain renders correctly
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 2);


            }
        }


        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.colors = colors.ToArray();
        //Recalculate vertex normals based on new vertices and triangles
        m.RecalculateNormals();
        return m;
    }

    void AddColor(List<Color> c, float height)
    {
        // add vertex color based on height of vertex
        if (height < 40)
        {
            c.Add(Color.green);
        }
        else if (height < 45)
        {
            c.Add(Color.gray);
        }
        else
        {
            c.Add(Color.white);
        }
    }


    }

