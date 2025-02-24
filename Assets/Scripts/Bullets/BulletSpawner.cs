using DG.Tweening;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Transform indicatorFill;
    [SerializeField]
    public float timeToPatternSpawn = 1f;

    private int patternType = 0;

    public void StartPattern(int _patternType)
    {
        patternType = _patternType;
        indicatorFill.DOLocalMoveY(0f, timeToPatternSpawn).onComplete += SpawnBullets;
    }

    public void SpawnBullets()
    {
        //Spawn pattern
        //TODO: Do better DX
        switch (patternType)
        {
            case 0: //Horizontal
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, -90f)); //Right
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 90f)); //Left
                break;
            case 1: //Vertical
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f)); //Up
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 180f)); //Down
                break;
            case 2: //Cross
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f)); //Up
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 90f)); //Left
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 180f)); //Down
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, -90f)); //Right
                break;
            case 3: //X
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, -45f)); //UR
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 45f)); //UL
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 135f)); //DL
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, -135f)); //DR
                break;
            default:
                Debug.LogError($"Invalid bullet pattern received: {patternType}!");
                break;
        }

        Destroy(gameObject);
    }
}
