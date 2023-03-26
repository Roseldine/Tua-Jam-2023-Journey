using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;

public class GameManager : BaseGameManager
{
    [SerializeField] Player player;
    protected override void EventFlow()
    {
        OnMainMenuRequest.AddListener(()    => ChangeState(GameState.MainMenu));
        OnPlayRequest.AddListener(()        => ChangeState(GameState.Playing));
        OnPlayEndRequest.AddListener(()     => ChangeState(GameState.PlayResults));
    }

    private void Update()
    {
        UpdateManagers();
        player.UpdatePlayer();
    }

    #region Game Events

    // UI Manager
    public static readonly EventHandler<TargetMenu> OnMenuChange = new EventHandler<TargetMenu>();
    public static readonly EventHandler OnOptions = new EventHandler();

    // Play
    public static readonly EventHandler OnPlayerEnterPlatform = new EventHandler();
    public static readonly EventHandler OnPlayerCollision = new EventHandler();
    public static readonly EventHandler<float> OnCollect = new EventHandler<float>();

    #endregion // events
}
