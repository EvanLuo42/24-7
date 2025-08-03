using System.Collections.Generic;
using System.Linq;
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
            
            // 根据稀有度加卡
            var objectPrefab = objectsSo.objectPrefabs[Random.Range(0, objectsSo.objectPrefabs.Count)];
            Instantiate(objectPrefab, slot.transform, false);
            count++;
        }
    }

    public void AddObjectByPrefab(GameObject prefab)
    {
        foreach (var slot in objectSlots.Where(slot => !slot.GetComponentInChildren<ObjectController>()))
        {
            Instantiate(prefab, slot.transform, false);
            return;
        }
    } 
}
