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
                _fireballPrefab = (GameObject)Resources.Load("Prefabs/SpellSystem/Spells/Fireball");
            return _fireballPrefab;
        }
    }

    private static GameObject _waterballPrefab;
    public static GameObject WaterballPrefab
    {
        get
        {
            if (_waterballPrefab == null)
                _waterballPrefab = (GameObject)Resources.Load("Prefabs/SpellSystem/Spells/Waterball");
            return _waterballPrefab;
        }
    }

    private static GameObject _earthProjectile;
    public static GameObject EarthProjectile
    {
        get
        {
            if (_earthProjectile == null)
                _earthProjectile = (GameObject)Resources.Load("Prefabs/SpellSystem/Spells/EarthBall");
            return _earthProjectile;
        }
    }

    private static GameObject _windProjectile;
    public static GameObject WindProjectile
    {
        get
        {
            if (_windProjectile == null)
                _windProjectile = (GameObject)Resources.Load("Prefabs/SpellSystem/Spells/WindBall");
            return _windProjectile;
        }
    }
}