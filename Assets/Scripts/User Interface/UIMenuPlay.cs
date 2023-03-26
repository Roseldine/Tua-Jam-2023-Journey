using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using StardropTools.ScriptableValue;
using StardropTools.Tween;

public class UIMenuPlay : UIMenu
{
    [SerializeField] TMPro.TextMeshProUGUI  distanceTextMesh;
    [SerializeField] Slider                 dragonDistanceSlider;
    [SerializeField] Slider                 boostSlider;
    [SerializeField] TweenComponent         damageTween;

    [SerializeField] ScriptableFloat distanceValue;
    [SerializeField] ScriptableFloat dragonDistanceValue;
    [SerializeField] ScriptableFloat boostValue;

    public override void Initialize()
    {
        base.Initialize();

        distanceValue.OnFloatChanged.AddListener(HandleDistance);
        dragonDistanceValue.OnFloatChanged.AddListener(HandleDragonDistance);
        boostValue.OnFloatChanged.AddListener(HandleBoost);

        GameManager.OnPlayerCollision.AddListener(() => damageTween.StartTween());
    }

    void HandleDistance(float value) => distanceTextMesh.text = value.ToString();

    void HandleDragonDistance(float value) => dragonDistanceSlider.value = value;
    
    void HandleBoost(float value) => boostSlider.value = value;
}