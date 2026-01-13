using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GenerationManager generationManager;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<SpawnZone> spawnZones = new List<SpawnZone>();
    private bool isSpawningActive = false;
    

    private void Start()
    {

        if (generationManager == null)
        {
            generationManager = FindObjectOfType<GenerationManager>();
        }

        if (enemyContainer == null)
        {
            enemyContainer = new GameObject("Enemies").transform;
        }
    }

    public void StartSpawningForLocation(LocationType location, LevelData levelData)
    {

        StopAllSpawning();
        ClearSpawnZones();

        if (location != LocationType.Hub)
        {
            switch(location)
            {
                case LocationType.BurningWastelands:
                    levelData.enemySpawnData[0].enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemies/Fanatic");
                    levelData.enemySpawnData[1].enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemies/Ash_Wisp");
                    break;

                case LocationType.CrystalSteppes:
                    levelData.enemySpawnData[0].enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemies/WindStalker");
                    break;

                case LocationType.FloodedTemples:
                    levelData.enemySpawnData[0].enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemies/BogGuardian");
                    break;

            }
        }

        if (location == LocationType.Hub)
        {           
            ClearAllEnemies();
            isSpawningActive = false;
            return;
        }

        ClearAllEnemies();
        ClearSpawnZones();

        CreateSpawnZones(levelData);

        isSpawningActive = true;
        StartCoroutine(SpawnEnemiesRoutine(levelData));
    }

    private void CreateSpawnZones(LevelData levelData)
    {
        spawnZones.Clear();

        for (int i = 0; i < levelData.spawnZonesCount; i++)
        {
            GameObject zoneObj = new GameObject($"SpawnZone_{i}");
            zoneObj.transform.SetParent(transform);

            SpawnZone zone = zoneObj.AddComponent<SpawnZone>();
            zone.Initialize(
                zoneName: $"{levelData.locationName}_Zone_{i}",
                zoneSize: levelData.spawnZoneSize,
                groundLayer: groundLayer,
                obstacleLayer: obstacleLayer,
                spawnCheckRadius: levelData.spawnCheckRadius
            );

            if (TryFindValidZonePosition(out Vector3 zonePosition))
            {
                zoneObj.transform.position = zonePosition;
                spawnZones.Add(zone);
            }
            else
            {
                Destroy(zoneObj);
            }
        }

        Debug.Log($"Создано зон спавна: {spawnZones.Count}");
    }

    private bool TryFindValidZonePosition(out Vector3 position)
    {
        position = Vector3.zero;
        int maxAttempts = 30;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = GetRandomMapPosition();

            if (IsValidGroundPosition(randomPos))
            {
                bool tooCloseToOtherZone = false;
                foreach (var zone in spawnZones)
                {
                    if (Vector3.Distance(randomPos, zone.transform.position) < 15f)
                    {
                        tooCloseToOtherZone = true;
                        break;
                    }
                }

                if (!tooCloseToOtherZone)
                {
                    position = randomPos;
                    return true;
                }
            }
        }

        return false;
    }

    private Vector3 GetRandomMapPosition()
    {

        float levelWidth = 50f; 
        float levelHeight = 30f; 

        return new Vector3(
            Random.Range(-levelWidth / 2, levelWidth / 2),
            Random.Range(-levelHeight / 2, levelHeight / 2),
            0
        );
    }

    private bool IsValidGroundPosition(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 10f, groundLayer);
        return hit.collider != null;
    }

    private IEnumerator SpawnEnemiesRoutine(LevelData levelData)
    {

        SpawnInitialEnemies(levelData);

        while (isSpawningActive)
        {
            yield return new WaitForSeconds(1f);

            if (activeEnemies.Count < levelData.maxTotalEnemies && spawnZones.Count > 0)
            {
                TrySpawnAdditionalEnemy(levelData);
            }
        }
    }

    private void SpawnInitialEnemies(LevelData levelData)
    {
        foreach (var spawnData in levelData.enemySpawnData)
        {
            int enemiesToSpawn = Random.Range(spawnData.minCount, spawnData.maxCount + 1);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (Random.value <= spawnData.spawnChance && spawnZones.Count > 0)
                {                 
                    TrySpawnEnemy(spawnData.enemyPrefab);
                }
            }
        }
    }

    private void TrySpawnAdditionalEnemy(LevelData levelData)
    {
        print(levelData.locationType);
        if (levelData.enemySpawnData.Count == 0 || spawnZones.Count == 0) return;

        float totalChance = 0f;
        foreach (var data in levelData.enemySpawnData)
        {
            totalChance += data.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float currentChance = 0f;

        GameObject enemyPrefab = null;
        foreach (var data in levelData.enemySpawnData)
        {
            currentChance += data.spawnChance;
            if (randomValue <= currentChance)
            {
                enemyPrefab = data.enemyPrefab;
                break;
            }
        }

        if (enemyPrefab != null)
        {
            TrySpawnEnemy(enemyPrefab);
        }
    }

    private bool TrySpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnZones.Count == 0) return false;

        SpawnZone zone = spawnZones[Random.Range(0, spawnZones.Count)];

        if (zone.TryGetValidSpawnPosition(out Vector3 spawnPosition))
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemyContainer);
            activeEnemies.Add(enemy);

            var enemyBase = enemy.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                StartCoroutine(MonitorEnemyDeath(enemy));
            }

            return true;
        }

        return false;
    }

    private IEnumerator MonitorEnemyDeath(GameObject enemy)
    {
        var enemyBase = enemy.GetComponent<EnemyBase>();

        while (enemy != null && enemyBase != null && enemyBase.IsAlive)
        {
            yield return new WaitForSeconds(1f);
        }

        if (enemy != null)
        {
            activeEnemies.Remove(enemy);
        }
    }

    private void ClearAllEnemies()
    {
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        activeEnemies.Clear();

        if (enemyContainer != null)
        {
            foreach (Transform child in enemyContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void ClearSpawnZones()
    {
        foreach (var zone in spawnZones)
        {
            if (zone != null && zone.gameObject != null)
                Destroy(zone.gameObject);
        }
        spawnZones.Clear();
    }

    public void StopAllSpawning()
    {
        isSpawningActive = false;
        StopAllCoroutines();
    }
}