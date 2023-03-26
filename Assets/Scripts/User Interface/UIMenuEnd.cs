
using UnityEngine;
using StardropTools.ScriptableValue;

public class UIMenuEnd : UIMenu
{
    [SerializeField] TMPro.TextMeshProUGUI scoreDistanceTextMesh;
    [SerializeField] ScriptableFloat distanceValue;

    public override void Initialize()
    {
        base.Initialize();
        distanceValue.OnFloatChanged.AddListener(UpdateScoreDistance);
    }

    void UpdateScoreDistance(float value) => scoreDistanceTextMesh.text = value.ToString();
}