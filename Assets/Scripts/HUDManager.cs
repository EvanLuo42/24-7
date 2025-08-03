using System;
using CardSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Components")] 
    public GameObject projectProgress;
    public GameObject energy;
    public GameObject stress;
    public GameObject passion;

    private TextMeshProUGUI _projectProgressNumber;
    private Slider _projectProgressBar;
    
    private TextMeshProUGUI _energyNumber;
    private Slider _energyProgressBar;
    
    private TextMeshProUGUI _stressNumber;
    private Slider _stressProgressBar;
    
    private TextMeshProUGUI _passionNumber;
    private Slider _passionProgressBar;

    private void Start()
    {
        _projectProgressNumber = projectProgress.GetComponentInChildren<TextMeshProUGUI>();
        _projectProgressBar = projectProgress.GetComponentInChildren<Slider>();
        
        _stressNumber = stress.GetComponentInChildren<TextMeshProUGUI>();
        _stressProgressBar = stress.GetComponentInChildren<Slider>();
        
        _energyNumber = energy.GetComponentInChildren<TextMeshProUGUI>();
        _energyProgressBar = energy.GetComponentInChildren<Slider>();
        
        _passionNumber = passion.GetComponentInChildren<TextMeshProUGUI>();
        _passionProgressBar = passion.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        _projectProgressNumber.text = $"{Math.Round(GameContext.Attributes.Productivity * 100f, 1)}%";
        _stressNumber.text = $"{Math.Round(GameContext.Attributes.Stress * 100f, 1)}%";
        _energyNumber.text = $"{Math.Round(GameContext.Attributes.Energy * 100f, 1)}%";
        _passionNumber.text = $"{Math.Round(GameContext.Attributes.Cook * 100f, 1)}%";

        _projectProgressBar.value = GameContext.Attributes.Productivity / 1f;
        _stressProgressBar.value = GameContext.Attributes.Stress / 1f;
        _energyProgressBar.value = GameContext.Attributes.Energy / 1f;
        _passionProgressBar.value = GameContext.Attributes.Cook / 1f;
    }
}
