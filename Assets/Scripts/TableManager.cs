using System.Collections.Generic;
using System.Linq;
using CardSystem;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public Cards cardsSo;


    public List<GameObject> objectSlots = new();

    public void GenerateObjects(int amount)
    {
        var count = 0;
        foreach (var slot in objectSlots)
        {
            if (count == amount) return;
            if (slot.GetComponentInChildren<ObjectController>()) continue;

            GameObject Card = cardsSo.GetRandomRarity()
                .CardPrefabList[Random.Range(0, cardsSo.GetRandomRarity().CardPrefabList.Count-1)];
            
            // 从卡片获取物体
            var objectPrefab = Card.GetComponent<ApplyCard>().sourceObject;
            
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
