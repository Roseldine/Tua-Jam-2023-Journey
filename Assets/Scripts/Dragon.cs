using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;
using StardropTools.ScriptableValue;

public class Dragon : BaseManagerUpdatable
{
    [NaughtyAttributes.Expandable][SerializeField] PlayerSettingsSO playerSettings;
    [SerializeField] AnimatorHandler animator;
    [SerializeField] BoxColliderDetector obstacleChecker;
    [SerializeField] BoxColliderDetector playerChecker;
    [SerializeField] Transform selfTransform;
    [SerializeField] Player player;
    float distance = 10;
    Coroutine distanceCR;
    bool followPlayer;

    protected override void EventFlow()
    {
        GameManager.OnPlayStart.AddListener(StartFollow);
        GameManager.OnMainMenu.AddListener(ResetDragon);
        GameManager.OnPlayEnd.AddListener(StopFollow);

        obstacleChecker.OnColliderEnter.AddListener(DestroyObstacle);
        playerChecker.OnColliderEnter.AddListener(PlayerCaught);
        player.OnHealthChanged.AddListener(PlayerLoseHealth);

        PlayerLoseHealth(player.Health);
    }

    
    public override void UpdateManager()
    {
        if (followPlayer == false)
            return;

        obstacleChecker.SearchForColliders();
        playerChecker.SearchForColliders();
        Vector3 targetPos = player.Position + Vector3.back * distance;
        targetPos.x = 0;
        selfTransform.position = Vector3.Lerp(selfTransform.position, targetPos, playerSettings.dragonSpeed * Time.deltaTime);
    }

    void ResetDragon()
    {
        StopFollow();
        selfTransform.position = Vector3.back * 25;
    }

    void StartFollow()
    {
        followPlayer = true;
        PlayMove();
    }

    void StopFollow()
    {
        followPlayer = false;
        PlayIdle();
    }

    void PlayerLoseHealth(int health)
    {
        float targetDist = playerSettings.distances[health];
        ChangeDistanceTo(targetDist);
    }

    void DestroyObstacle(Collider obstacleCollider)
    {
        Obstacle obstacle = obstacleCollider.GetComponent<Obstacle>();
        if (obstacle == null)
            return;

        PoolManager.Instance.SpawnEffect(0, obstacle.Position + Vector3.up, Quaternion.identity, null, .5f);
        obstacle.Despawn();
    }

    void PlayerCaught(Collider playerCollider)
    {
        GameManager.OnPlayEndRequest?.Invoke();
    }


    public void ChangeDistanceTo(float targetDistance)
    {
        if (distanceCR != null)
            StopCoroutine(distanceCR);

        distanceCR = StartCoroutine(DistanceLerpCR(targetDistance));
    }

    IEnumerator DistanceLerpCR(float targetDistance)
    {
        float t = 0;
        float dur = .2f;
        float percent = 0;
        float startDistance = distance;

        while (t < dur)
        {
            percent = dur / t;
            distance = Mathf.Lerp(startDistance, targetDistance, percent);
            yield return null;
        }

        distance = targetDistance;
    }


    public void PlayAnimation(int animID) => animator.CrossFadeAnimation(animID);
    public void PlayIdle() => PlayAnimation(0);
    public void PlayMove() => PlayAnimation(1);
}
