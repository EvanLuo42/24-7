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

            float randomNumber = Random.Range(0, cardsSo.GetRandomRarity().CardPrefabList.Count - 1);
            Debug.Log("Random Number:" + randomNumber);
            Debug.Log("Asset Length" + cardsSo.GetRandomRarity()
                .CardPrefabList.Count());

            GameObject card;
            var CardList = cardsSo.GetRandomRarity().CardPrefabList;

            if (CardList.Count == 1)
            {
                card = CardList[0];
            }
            else
            {
                card = CardList[Random.Range(0, cardsSo.GetRandomRarity().CardPrefabList.Count-1)];
            }
            
            // 从卡片获取物体
            var objectPrefab = card.GetComponent<ApplyCard>().sourceObject;
            
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
