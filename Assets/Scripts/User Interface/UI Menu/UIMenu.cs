
using UnityEngine;
using UnityEngine.UI;
using StardropTools.UI;
using StardropTools.Tween;

public class UIMenu : BaseUIObject, IMenu
{
    [SerializeField] Button[] playButtons;
    [SerializeField] Button[] restartButtons;
    [SerializeField] Button[] pauseButtons;
    [SerializeField] Button[] quitButtons;
    [SerializeField] TweenComponent[] tweenComponents;

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < playButtons.Length; i++)
            playButtons[i].onClick.AddListener(() => GameManager.OnPlayRequest?.Invoke());

        for (int i = 0; i < restartButtons.Length; i++)
            restartButtons[i].onClick.AddListener(() => GameManager.OnMainMenuRequest?.Invoke());

        for (int i = 0; i < pauseButtons.Length; i++)
            pauseButtons[i].onClick.AddListener(() => GameManager.OnPauseRequest?.Invoke());

        for (int i = 0; i < quitButtons.Length; i++)
            quitButtons[i].onClick.AddListener(() => Application.Quit());
    }

    public virtual void Open()
    {
        SetActive(true);
        TweenManager.StartTweenComponents(tweenComponents);
    }

    public virtual void Close()
    {
        SetActive(false);
    }
}
