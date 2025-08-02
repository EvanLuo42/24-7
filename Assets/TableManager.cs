using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public GameObject objectPrefab;

    public List<GameObject> objectSlots = new();
    
    void Start()
    {
        foreach (var slot in objectSlots)
        {
            var obj = Instantiate(objectPrefab, slot.transform, false);
            obj.transform.localPosition = new Vector3(0, obj.GetComponent<MeshCollider>().bounds.size.y);
            obj.transform.localRotation = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
