using CardSystem.Data;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool inverse;
    private Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();
        UpdateLight();
    }

    private void Update()
    {
        UpdateLight();
    }

    private void UpdateLight()
    {
        var isDay = GameContext.currentPhase == LoopManager.LoopPhase.Day || GameContext.currentPhase == LoopManager.LoopPhase.Dawn;
        _light.enabled = inverse ? !isDay : isDay;
    }
}
