using UnityEngine;

public class BrickPlacer : MonoBehaviour
{
    private const int SLOT_COUNT = 3;
    private int _placedBrickCount = 0;

    [SerializeField] private GridXY _grid;
    [SerializeField] private LayerMask _nodeLayerMask;
    [SerializeField] private BrickSpawner _brickSpawner;

    public bool PlaceBrick(Brick brick, Vector3 clickPosition)
    {
        BrickPiece[] brickPieces = brick.Pieces;

        foreach (var piece in brickPieces)
        {
            Vector3 pieceSnapPosition = _grid.GetNodeCenter(piece.transform.position - piece.transform.localScale / 2);
            Collider2D collidedNodeCollider = Physics2D.OverlapBox(pieceSnapPosition, piece.transform.localScale, 0, _nodeLayerMask);
            Node collidedNode = collidedNodeCollider.GetComponent<Node>();

            if (!collidedNode.IsEmpty())
            {
                return false;
            }
            else
            {
                collidedNode.HoldBrick = brick;
            }
        }

        Vector3 clickedNodeOrigin = _grid.GetNodeOrigin(clickPosition);
        brick.SetPosition(clickedNodeOrigin);

        _placedBrickCount++;

        if (_placedBrickCount == SLOT_COUNT)
        {
            _placedBrickCount = 0;
            _brickSpawner.SpawnBricks();
        }

        return true;
    }
}
