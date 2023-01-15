using Utils;
using UnityEngine;
using System.Collections.Generic;

public class GridXY : MonoBehaviour, IGrid
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellSize;
    [SerializeField] private Vector3 _origin;

    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private Transform _nodeContainer;

    [SerializeField] private bool _debug;

    private Node[,] _gridArray;

    private TextMesh[,] _gridDebugArray;

    public int Width { get => _width; }
    public int Height { get => _height; }

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _gridArray = new Node[_width, _height];
        _gridDebugArray = new TextMesh[_width, _height];

        #region Debugging

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                GridIndex gridIndex = new GridIndex(x, y, 0);

                GameObject spawnedNodeObject = Instantiate(_nodePrefab, GetNodeCenter(new GridIndex(x, y, 0)), Quaternion.identity, _nodeContainer);

                Node spawnedNode = spawnedNodeObject.GetComponent<Node>();
                SetNode(spawnedNode, gridIndex);

                if (_debug)
                {
                    _gridDebugArray[x, y] = Utility.CreateWorldText(
                        _gridArray[x, y].ToString(),
                        position: GetNodeOrigin(new GridIndex(x, y, 0)) + new Vector3(0.5f, 0.5f, 0f) * _cellSize,
                        localScale: new Vector3(0.05f, 0.05f, 0.05f),
                        color: Color.white,
                        fontSize: 40
                    );

                    Debug.DrawLine(GetNodeOriginDebug(new GridIndex(x, y, 0)), GetNodeOriginDebug(new GridIndex(x, y + 1, 0)), Color.white, 1000f);
                    Debug.DrawLine(GetNodeOriginDebug(new GridIndex(x, y, 0)), GetNodeOriginDebug(new GridIndex(x + 1, y, 0)), Color.white, 1000f);
                }
            }
        }

        if (_debug)
        {
            Debug.DrawLine(GetNodeOriginDebug(new GridIndex(0, _height, 0)), GetNodeOriginDebug(new GridIndex(_width, _height, 0)), Color.white, 1000f);
            Debug.DrawLine(GetNodeOriginDebug(new GridIndex(_width, 0, 0)), GetNodeOriginDebug(new GridIndex(_width, _height, 0)), Color.white, 1000f);
        }

        #endregion
    }

    private Vector3 GetNodeOriginDebug(GridIndex gridIndex)
    {
        return gridIndex.ToVector3() * _cellSize + _origin;
    }

    public Vector3 GetNodeOrigin(Vector3 nodePosition)
    {
        GridIndex gridIndex = GetGridIndex(nodePosition);
        return GetNodeOrigin(gridIndex);
    }

    public Vector3 GetNodeOrigin(GridIndex gridIndex)
    {
        if (!IsValidCoordinate(gridIndex))
        {
            GridIndex clampedGridIndex = ClampToGrid(gridIndex);
            return GetNodeOrigin(clampedGridIndex);
        }

        return gridIndex.ToVector3() * _cellSize + _origin;
    }

    private GridIndex ClampToGrid(GridIndex gridIndex)
    {
        gridIndex.X = (gridIndex.X < 0) ? 0 : gridIndex.X;
        gridIndex.X = (gridIndex.X >= _width) ? _width - 1 : gridIndex.X;
        gridIndex.Y = (gridIndex.Y < 0) ? 0 : gridIndex.Y;
        gridIndex.Y = (gridIndex.Y >= _height) ? _height - 1 : gridIndex.Y;

        return gridIndex;
    }

    public Vector3 GetNodeCenter(GridIndex gridIndex)
    {
        if (!IsValidCoordinate(gridIndex))
        {
            gridIndex = ClampToGrid(gridIndex);
        }

        Vector3 nodeOrigin = GetNodeOrigin(gridIndex);
        Vector3 offsetBetweenNodeOriginAndCenter = GetOffsetBetweenNodeOriginAndCenter_Math();
        Vector3 nodeCenter = nodeOrigin + offsetBetweenNodeOriginAndCenter;

        return nodeCenter;
    }

    public Vector3 GetNodeCenter(Vector3 nodePosition)
    {
        GridIndex gridIndex = GetGridIndex(nodePosition);
        return GetNodeCenter(gridIndex);
    }

    private Vector3 GetOffsetBetweenNodeOriginAndCenter_Math()
    {
        return new Vector3(_cellSize / 2, _cellSize / 2, 0);
    }

    public void SetNode(Node node, Vector3 nodePosition)
    {
        GridIndex gridIndexAtPosition = GetGridIndex(nodePosition);
        SetNode(node, gridIndexAtPosition);
    }

    private void SetNode(Node node, GridIndex gridIndex)
    {
        if (!IsValidCoordinate(gridIndex)) return;

        node.Initialize(this, gridIndex);

        _gridArray[gridIndex.X, gridIndex.Y] = node;
    }

    public GridIndex GetGridIndex(Vector3 nodeWorldPosition)
    {
        int x = Mathf.FloorToInt((nodeWorldPosition - _origin).x / _cellSize);
        int y = Mathf.FloorToInt((nodeWorldPosition - _origin).y / _cellSize);

        GridIndex gridIndex = new GridIndex(x, y, 0);

        return IsValidCoordinate(gridIndex) ? gridIndex : ClampToGrid(gridIndex);
    }

    public Node GetNode(Vector3 cellWorldPosition)
    {
        GridIndex gridIndex = GetGridIndex(cellWorldPosition);
        return IsValidCoordinate(gridIndex) ? _gridArray[gridIndex.X, gridIndex.Y] : default;
    }

    private bool IsValidCoordinate(GridIndex gridIndex)
    {
        return gridIndex.X >= 0 && gridIndex.Y >= 0 && gridIndex.X < _width && gridIndex.Y < _height;
    }

    public List<Node> GetRowAt(int rowIndex)
    {
        List<Node> column = new List<Node>();

        for (int col = 0; col < _gridArray.GetLength(0); col++)
        {
            Node gridObject = _gridArray[col, rowIndex];
            column.Add(gridObject);
        }

        return column;
    }
}
