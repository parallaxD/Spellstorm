using UnityEngine;

public static class Constants
{
    private static Camera _mainCamera;
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    private static Transform _playerTransform;
    public static Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null)
                FindPlayer();
            return _playerTransform;
        }
    }
    private static void FindPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;
    }

    private static GameObject _fireballPrefab;
    public static GameObject FireballPrefab
    {
        get
        {
            if (_fireballPrefab == null)
                _fireballPrefab = (GameObject)Resources.Load("Prefabs/Fireball");
            return _fireballPrefab;
        }
    }
}