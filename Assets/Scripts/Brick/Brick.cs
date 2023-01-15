using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite { get => _sprite; }

    [SerializeField] private BrickPiece[] _pieces;
    public BrickPiece[] Pieces { get => _pieces; }

    internal void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }
}
