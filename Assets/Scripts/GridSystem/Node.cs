using UnityEngine;

public class Node : MonoBehaviour
{
    private IGrid _grid;
    private Brick _holdBrick;
    public Brick HoldBrick { get => _holdBrick; set => _holdBrick = value; }

    private GridIndex _gridIndex;
    public GridIndex GridIndex { get => _gridIndex; }

    public void Initialize(IGrid grid, GridIndex gridIndex)
    {
        _grid = grid;
        _gridIndex = gridIndex;
        GetComponent<Renderer>().material.color = Color.gray;
    }

    public void SetObject(Brick brick)
    {
        _holdBrick = brick;
    }

    public bool IsEmpty() { return _holdBrick == null; }

    public override string ToString()
    {
        return _gridIndex.X + ", " + _gridIndex.Y;
    }
}