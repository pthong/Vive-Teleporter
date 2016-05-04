﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BorderRenderer : MonoBehaviour {
    private Mesh[] CachedMeshes;
    public Material BorderMaterial;

    public Vector3[][] Points {
        get
        {
            return _Points;
        }
        set
        {
            _Points = value;
            RegenerateMesh();
        }
    }
    private Vector3[][] _Points;

    public float BorderHeight
    {
        get
        {
            return _BorderHeight;
        }
        set
        {
            _BorderHeight = value;
            RegenerateMesh();
        }
    }
    [SerializeField]
    private float _BorderHeight;

    void Update()
    {
        if (CachedMeshes == null || BorderMaterial == null)
            return;

        foreach(Mesh m in CachedMeshes)
            Graphics.DrawMesh(m, Matrix4x4.identity, BorderMaterial, 0, null, 0, null, false, false);
    }

    void OnValidate()
    {
        RegenerateMesh();
    }

    public void RegenerateMesh()
    {
        if (Points == null)
            return;
        CachedMeshes = new Mesh[Points.Length];
        for(int x=0;x<CachedMeshes.Length;x++)
            CachedMeshes[x] = GenerateMeshForPoints(Points[x]);
    }
	
	private Mesh GenerateMeshForPoints(Vector3[] Points)
    {
        Vector3[] verts = new Vector3[Points.Length * 2];
        Vector2[] uv = new Vector2[Points.Length * 2];
        for(int x=0;x<Points.Length;x++)
        {
            verts[2 * x] = Points[x];
            verts[2 * x + 1] = Points[x] + Vector3.up * BorderHeight;

            uv[2 * x] = new Vector2(x % 2, 0);
            uv[2 * x + 1] = new Vector2(x % 2, 1);
        }

        int[] indices = new int[2 * 3 * (verts.Length - 2)];
        for(int x=0;x<verts.Length/2-1;x++)
        {
            int p1 = 2*x;
            int p2 = 2*x + 1;
            int p3 = 2*x + 2;
            int p4 = 2*x + 3;

            indices[12 * x] = p1;
            indices[12 * x + 1] = p2;
            indices[12 * x + 2] = p3;
            indices[12 * x + 3] = p3;
            indices[12 * x + 4] = p2;
            indices[12 * x + 5] = p4;

            indices[12 * x + 6] = p3;
            indices[12 * x + 7] = p2;
            indices[12 * x + 8] = p1;
            indices[12 * x + 9] = p4;
            indices[12 * x + 10] = p2;
            indices[12 * x + 11] = p3;
        }

        Mesh m = new Mesh();
        m.vertices = verts;
        m.uv = uv;
        m.triangles = indices;
        m.RecalculateBounds();
        m.RecalculateNormals();
        return m;
    }
}
