using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;

public class LevelManager : BaseManagerUpdatable
{
    [SerializeField] AnimationCurve obstacleSpawnCurve;
    [SerializeField] int minObstacles = 2;
    [SerializeField] int maxObstacles = 5;
    [SerializeField] int maxPlatforms = 10;
    [SerializeField] Transform parentPlatforms;
    [SerializeField] Platform startPlatform;
    [SerializeField] List<Platform> platforms;
    [SerializeField] Platform mostRecentPlatform;

    Coroutine levelSettupCR;

    public override void Initialize()
    {
        base.Initialize();
        GeneratePlatforms();
    }

    protected override void EventFlow()
    {
        GameManager.OnGenerating.AddListener(GeneratePlatforms);
        GameManager.OnPlayerEnterPlatform.AddListener(NextPlatform);
        GameManager.OnMainMenu.AddListener(RestartLevel);
    }

    public override void UpdateManager()
    {
        for (int i = 0; i < platforms.Count; i++)
            platforms[i].ScanForPlayer();
    }

    Platform SpawnPlatform(Vector3 position)
    {
        Platform platform = PoolManager.Instance.SpawnPlaform(position, parentPlatforms);
        platform.Initialize();
        platform.SpawnObstacles(GetRandomObstacleAmount());
        platform.SpawnCollectibles(3);
        mostRecentPlatform = platform;
        platforms.Add(platform);

        return platform;
    }

    void RestartLevel()
    {
        PoolManager.Instance.ClearPlatforms();
        PoolManager.Instance.ClearObstacles();

        GeneratePlatforms();
    }

    [NaughtyAttributes.Button("Generate Platforms")]
    void GeneratePlatforms()
    {
        if (levelSettupCR != null)
            StopCoroutine(levelSettupCR);

        levelSettupCR = StartCoroutine(LevelSettupCR());
    }

    void NextPlatform()
    {
        Platform oldPlatform = platforms.GetFirst();
        platforms.Remove(oldPlatform);
        oldPlatform.Despawn();

        Vector3 spawnPos = mostRecentPlatform.SpawnPosition;
        Platform platform = SpawnPlatform(spawnPos);
    }

    IEnumerator LevelSettupCR()
    {
        // Execution Order:
        // 1) clear all rooms and opponents
        // 2) Spawn rooms
        // 3) Spawn opponents

        // 1) clear all rooms and opponents
        platforms = new List<Platform>();

        PoolManager.Instance.ClearPlatforms();
        //PoolManager.Instance.ClearOpponents();

        // 1.1) Wait for cleanup
        yield return WaitForSecondsManager.GetWait("lvlClean", .01f);

        // 2) Spawn rooms
        // 3) Spawn opponents

        Vector3 spawnPos = startPlatform.SpawnPosition;
        for (int i = 0; i < maxPlatforms; i++)
        {
            // 2.1) Spawn room and get the next spawn position
            Platform platform = SpawnPlatform(spawnPos);
            spawnPos = platform.SpawnPosition;
        }

        // Give it some wait time just to be safe & Done
        yield return WaitForSecondsManager.GetWait("lvlSettup", .1f);
        GameManager.OnGenerationEnd?.Invoke();
    }

    int GetRandomObstacleAmount() => Random.Range(minObstacles, maxObstacles + 1);
}
