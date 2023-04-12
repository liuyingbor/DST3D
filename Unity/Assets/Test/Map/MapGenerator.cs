using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class MapGenerator: MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;

        public int mapHeight;
        public int mapWdith;
        public float cellSize;

        MapGrid grid;

        /// <summary>
        /// 生成地图
        /// </summary>
        [Button("生成地图")]
        public void GenerateMap()
        {
            // 生成Mesh
            meshFilter.mesh = GenerateMapMesh(mapHeight, mapWdith, cellSize);
            // 生成网格
            grid = new MapGrid(mapHeight, mapWdith, cellSize);
        }

        public GameObject testObj;

        [Button("测试顶点")]
        public void TestVertex()
        {
            print(grid.GetVertexByWorldPosition(testObj.transform.position).Position);
        }

        /// <summary>
        /// 生成地形Mesh
        /// </summary>
        private Mesh GenerateMapMesh(int height, int wdith, float cellSize)
        {
            Mesh mesh = new Mesh();
            // 确定顶点在哪里
            mesh.vertices = new Vector3[]
            {
                new Vector3(0, 0, 0), new Vector3(0, 0, height * cellSize), new Vector3(wdith * cellSize, 0, height * cellSize),
                new Vector3(wdith * cellSize, 0, 0),
            };
            // 确定哪些点形成三角形
            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            mesh.uv = new Vector2[] { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0), };
            // 计算法线
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}