using System.Collections.Generic;
using CardSystem;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public Objects objectsSo; 

    public List<GameObject> objectSlots = new();

    public void GenerateObjects(int amount)
    {
        var count = 0;
        foreach (var slot in objectSlots)
        {
            if (count == amount) return;
            if (slot.GetComponentInChildren<ObjectController>()) continue;
            var objectPrefab = objectsSo.objectPrefabs[Random.Range(0, objectsSo.objectPrefabs.Count)];
            Instantiate(objectPrefab, slot.transform, false);
            count++;
        }
    }
}
