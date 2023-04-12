using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 网格，主要包含顶点和格子
    /// </summary>
    public class MapGrid
    {
        // 顶点数据
        public Dictionary<Vector2Int, MapVertex> vertexDic = new Dictionary<Vector2Int, MapVertex>();

        // 格子数据
        public Dictionary<Vector2Int, MapCell> cellDic = new Dictionary<Vector2Int, MapCell>();

        public MapGrid(int mapHeight, int mapWdith, float cellSize)
        {
            MapHeight = mapHeight;
            MapWdith = mapWdith;
            CellSize = cellSize;

            // 生成顶点数据
            for (int x = 1; x < mapWdith; x++)
            {
                for (int z = 1; z < mapHeight; z++)
                {
                    AddVertex(x, z);
                    AddCell(x, z);
                }
            }

            // 增加一行一列
            for (int x = 1; x <= mapWdith; x++)
            {
                AddCell(x, mapHeight);
            }

            for (int z = 1; z < mapWdith; z++)
            {
                AddCell(mapWdith, z);
            }

            #region 测试代码

            foreach (var item in vertexDic.Values)
            {
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                temp.transform.position = item.Position;
                temp.transform.localScale = Vector3.one * 0.25f;
            }

            foreach (var item in cellDic.Values)
            {
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.position = item.Position - new Vector3(0, 0.49f, 0);
                temp.transform.localScale = new Vector3(CellSize, 1, CellSize);
            }

            #endregion
        }

        public int MapHeight { get; private set; }
        public int MapWdith { get; private set; }
        public float CellSize { get; private set; }

        #region 顶点

        private void AddVertex(int x, int y)
        {
            vertexDic.Add(new Vector2Int(x, y)
                , new MapVertex() { Position = new Vector3(x * CellSize, 0, y * CellSize) });
        }

        public MapVertex GetVertex(Vector2Int index)
        {
            return vertexDic[index];
        }

        public MapVertex GetVertex(int x, int y)
        {
            return GetVertex(new Vector2Int(x, y));
        }

        /// <summary>
        /// 通过世界坐标获取顶点
        /// </summary>
        public MapVertex GetVertexByWorldPosition(Vector3 position)
        {
            int x = Mathf.Clamp(Mathf.RoundToInt(position.x / CellSize), 1, MapWdith);
            int y = Mathf.Clamp(Mathf.RoundToInt(position.z / CellSize), 1, MapHeight);
            return GetVertex(x, y);
        }

        #endregion

        #region 格子

        private void AddCell(int x, int y)
        {
            float offset = CellSize / 2;
            cellDic.Add(new Vector2Int(x, y),
                new MapCell() { Position = new Vector3(x * CellSize - offset, 0, y * CellSize - offset) });
        }

        public MapCell GetCell(Vector2Int index)
        {
            return cellDic[index];
        }

        public MapCell GetCell(int x, int y)
        {
            return GetCell(x, y);
        }

        /// <summary>
        /// 获取左下角格子
        /// </summary>
        public MapCell GetLeftBottomMapCell(Vector2Int vertexIndex)
        {
            return cellDic[vertexIndex];
        }

        /// <summary>
        /// 获取右下角格子
        /// </summary>
        public MapCell GetRightBottomMapCell(Vector2Int vertexIndex)
        {
            return cellDic[new Vector2Int(vertexIndex.x + 1, vertexIndex.y)];
        }

        /// <summary>
        /// 获取左上角格子
        /// </summary>
        public MapCell GetLeftTopMapCell(Vector2Int vertexIndex)
        {
            return cellDic[new Vector2Int(vertexIndex.x, vertexIndex.y + 1)];
        }

        /// <summary>
        /// 获取右上角格子
        /// </summary>
        public MapCell GetRightTopMapCell(Vector2Int vertexIndex)
        {
            return cellDic[new Vector2Int(vertexIndex.x + 1, vertexIndex.y + 1)];
        }

        #endregion
    }

    /// <summary>
    /// 地图顶点
    /// </summary>
    public class MapVertex
    {
        public Vector3 Position;
    }

    /// <summary>
    /// 地图格子
    /// </summary>
    public class MapCell
    {
        public Vector3 Position;
    }
}