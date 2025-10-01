using UnityEngine;
using UnityEngine.Tilemaps;

public class HideColliderTilemap : MonoBehaviour
{
    private TilemapRenderer tilemapRender;

    void Awake()
    {
        tilemapRender = GetComponent<TilemapRenderer>();
    }

    void Start()
    {
        tilemapRender.enabled = false;
    }
}
