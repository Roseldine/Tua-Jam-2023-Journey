
using UnityEngine;
using StardropTools;

public class CameraManager : BaseManagerUpdatable
{
    [SerializeField] float speed = 10;
    [SerializeField] Player player;
    [SerializeField] Transform pivot;
    [SerializeField] new Camera camera;
    [SerializeField] bool onlyFollowZAxis;
    Vector3 targetPos;

    protected override void EventFlow()
    {

    }

    public override void UpdateManager()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        targetPos = Vector3.Lerp(pivot.position, player.Position, speed * Time.deltaTime);
        
        if (onlyFollowZAxis == true)
            targetPos.x = 0;

        pivot.position = targetPos;
    }
}
