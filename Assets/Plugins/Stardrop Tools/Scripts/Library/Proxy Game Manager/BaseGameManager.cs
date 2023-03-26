
using UnityEngine;
using StardropTools;

public abstract class BaseGameManager : ManagerInitializer
{
    #region Singleton
    /// <summary>
	/// The instance.
	/// </summary>
	private static BaseGameManager instance;


    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static BaseGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BaseGameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(BaseGameManager).Name;
                    instance = obj.AddComponent<BaseGameManager>();
                }
            }
            return instance;
        }
    }

    public void SingletonInitialization()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        	Destroy(gameObject);
    }
    #endregion // singleton


    [SerializeField] GameState gameState;
    public GameState GameState => gameState;


    #region Events

    public static readonly EventHandler OnInitialized    = new EventHandler();

    public static readonly EventHandler OnMainMenu       = new EventHandler();
    public static readonly EventHandler OnPlayStart      = new EventHandler();
    public static readonly EventHandler OnPlayEnd        = new EventHandler();

    public static readonly EventHandler OnPaused         = new EventHandler();
    public static readonly EventHandler OnResumed        = new EventHandler();

    public static readonly EventHandler OnGenerating     = new EventHandler();
    public static readonly EventHandler OnGenerationEnd  = new EventHandler();

    public static readonly EventHandler OnMainMenuRequest   = new EventHandler();
    public static readonly EventHandler OnPlayRequest       = new EventHandler();
    public static readonly EventHandler OnPlayEndRequest    = new EventHandler();
    public static readonly EventHandler OnPauseRequest      = new EventHandler();

    #endregion // events


    protected override void Awake()
    {
        base.Awake();
        SingletonInitialization();
        InitializeManagers();
        EventFlow();
    }

    protected virtual void EventFlow() { }


    protected override void OnValidate()
    {
        base.OnValidate();

        if (gameState != GameState.Initializing)
            gameState = GameState.Initializing;
    }

    protected void ChangeState(GameState targetState)
    {
        if (gameState == targetState)
            return;

        GameState prevState = gameState;
        gameState = targetState;

        switch (prevState)
        {
            case GameState.Initializing:
                OnInitialized?.Invoke();
                break;

            case GameState.MainMenu:
                break;

            case GameState.Playing:
                break;

            case GameState.PlayResults:
                break;

            case GameState.Generating:
                break;

            case GameState.Paused:
                if (targetState == GameState.Playing)
                    OnResumed?.Invoke();
                break;
        }

        switch (targetState)
        {
            case GameState.MainMenu:
                OnMainMenu?.Invoke();
                break;

            case GameState.Playing:
                if (prevState != GameState.Paused)
                    OnPlayStart?.Invoke();
                break;

            case GameState.PlayResults:
                if (prevState == GameState.Playing)
                    OnPlayEnd?.Invoke();
                break;

            case GameState.Generating:
                OnGenerating?.Invoke();
                break;

            case GameState.Paused:
                OnPaused?.Invoke();
                break;
        }
    }
}
