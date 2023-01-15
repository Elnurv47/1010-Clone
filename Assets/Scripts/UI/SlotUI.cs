using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Brick _holdBrick;

    [SerializeField] private Image _brickSpriteHolder;
    [SerializeField] private BrickPlacer _brickPlacer;

    public void SetBrick(Brick brick)
    {
        _holdBrick = brick;
        _brickSpriteHolder.sprite = brick.Sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _holdBrick = Spawner.Spawn(_holdBrick, transform.position, Quaternion.identity);
        _brickSpriteHolder.sprite = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPointWithCameraZ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 worldPoint = new Vector3(worldPointWithCameraZ.x, worldPointWithCameraZ.y, 0);
        _holdBrick.SetPosition(worldPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool _canBrickBePlaced = _brickPlacer.PlaceBrick(_holdBrick, Camera.main.ScreenToWorldPoint(eventData.position));

        if (!_canBrickBePlaced)
        {
            _holdBrick.SetPosition(transform.position);
            _brickSpriteHolder.sprite = _holdBrick.Sprite;
        }
    }
}
