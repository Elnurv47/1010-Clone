using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private Brick[] _bricks;
    [SerializeField] private SlotUI[] _slots;

    private void Start()
    {
        SpawnBricks();
    }

    public void SpawnBricks()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            Brick brickToSpawn = GetRandomBrick();
            _slots[i].SetBrick(brickToSpawn);
        }
    }

    private Brick GetRandomBrick()
    {
        int randomIndex = Random.Range(0, _bricks.Length);
        return _bricks[randomIndex];
    }
}
