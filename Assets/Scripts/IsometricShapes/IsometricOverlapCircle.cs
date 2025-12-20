using UnityEngine;
using System.Collections.Generic;

public class IsometricOverlapCircle : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask targetLayers;
    //[SerializeField] private bool drawGizmos = true;

    private Matrix4x4 isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public Collider2D[] OverlapCircleIsometric(Vector3 position)
    {        Vector3 isoPos = ToIsometric(position);

        return Physics2D.OverlapCircleAll(new Vector2(isoPos.x, isoPos.y), radius, targetLayers);
    }

    public List<Collider2D> OverlapCircleIsometricCustom(Vector3 position)
    {
        List<Collider2D> results = new List<Collider2D>();
        Vector3 isoCenter = ToIsometric(position);

        Collider2D[] allColliders = Physics2D.OverlapCircleAll(
            new Vector2(isoCenter.x, isoCenter.y), radius * 2f, targetLayers);

        foreach (var collider in allColliders)
        {
            Vector3 colliderIsoPos = ToIsometric(collider.transform.position);
            float distance = Vector2.Distance(
                new Vector2(isoCenter.x, isoCenter.y),
                new Vector2(colliderIsoPos.x, colliderIsoPos.y)
            );

            if (distance <= radius)
            {
                results.Add(collider);
            }
        }

        return results;
    }

    private Vector3 ToIsometric(Vector3 worldPos)
    {
        return isometricMatrix.MultiplyPoint3x4(worldPos);
    }

    private Vector3 FromIsometric(Vector3 isoPos)
    {
        return isometricMatrix.inverse.MultiplyPoint3x4(isoPos);
    }
}