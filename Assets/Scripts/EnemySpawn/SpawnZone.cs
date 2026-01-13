using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private string zoneName;
    private Vector2 zoneSize;
    private LayerMask groundLayer;
    private LayerMask obstacleLayer;
    private float spawnCheckRadius;

    public void Initialize(string zoneName, Vector2 zoneSize, LayerMask groundLayer,
                         LayerMask obstacleLayer, float spawnCheckRadius = 1f)
    {
        this.zoneName = zoneName;
        this.zoneSize = zoneSize;
        this.groundLayer = groundLayer;
        this.obstacleLayer = obstacleLayer;
        this.spawnCheckRadius = spawnCheckRadius;
    }

    public bool IsPointInZone(Vector3 point)
    {
        Vector3 localPoint = transform.InverseTransformPoint(point);
        return Mathf.Abs(localPoint.x) <= zoneSize.x / 2f &&
               Mathf.Abs(localPoint.y) <= zoneSize.y / 2f;
    }

    public bool TryGetValidSpawnPosition(out Vector3 spawnPosition, int maxAttempts = 30)
    {
        spawnPosition = Vector3.zero;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomLocalPos = new Vector3(
                Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f),
                Random.Range(-zoneSize.y / 2f, zoneSize.y / 2f),
                0f
            );

            Vector3 worldPos = transform.TransformPoint(randomLocalPos);

            RaycastHit2D groundHit = Physics2D.Raycast(worldPos, Vector2.down, 10f, groundLayer);
            if (groundHit.collider != null)
            {
                Vector3 spawnPoint = new Vector3(
                    groundHit.point.x,
                    groundHit.point.y + 0.5f,
                    0f
                );

                Collider2D[] obstacles = Physics2D.OverlapCircleAll(spawnPoint, spawnCheckRadius, obstacleLayer);
                if (obstacles.Length == 0)
                {
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(spawnPoint, spawnCheckRadius);
                    bool hasEnemy = false;
                    foreach (var col in enemies)
                    {
                        if (col.GetComponent<EnemyBase>() != null)
                        {
                            hasEnemy = true;
                            break;
                        }
                    }

                    if (!hasEnemy)
                    {
                        spawnPosition = spawnPoint;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (zoneSize != Vector2.zero)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, new Vector3(zoneSize.x, zoneSize.y, 1f));

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(zoneSize.x, zoneSize.y, 1f));
        }
    }
}