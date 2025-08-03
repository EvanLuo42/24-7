using CardSystem.Data;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Text Fields")]
    public TextMeshProUGUI productivity;
    public TextMeshProUGUI stress;
    public TextMeshProUGUI energy;
    public TextMeshProUGUI sleepingHours;

    private void Update()
    {
        productivity.text = $"Productivity: {GameContext.Attributes.Productivity * 100f}%";
        stress.text = $"Stress: {GameContext.Attributes.Stress * 100f}%";
        energy.text = $"Energy: {GameContext.Attributes.Energy * 100f}%";
        sleepingHours.text = $"Sleeping Hours: {GameContext.Attributes.SleepingHours}";
    }
}
