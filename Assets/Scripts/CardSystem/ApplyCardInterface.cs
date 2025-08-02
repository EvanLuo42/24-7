using System.Collections.Generic;
using UnityEngine;

public class ApplyCardInterface : MonoBehaviour
{
    public List<CardEffect> CE_ToApplly;

    public void Apply()
    {
        foreach (var CE in CE_ToApplly)
        {
            CE.ApplyEffect();
        }
    }
}
