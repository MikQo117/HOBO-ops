using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PainterScript : MonoBehaviour
{

    private List<GameObject>     objects;
    private List<SortObject>     sortList;
    public bool                  Sorting;

    // Use this for initialization
    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Sortable").ToList();
        sortList = new List<SortObject>();
        foreach (GameObject item in objects)
        {
            sortList.Add(new SortObject(item.transform, item.GetComponent<SpriteRenderer>()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Sorting)
        {
            //Sort positions list
            sortList = sortList.OrderByDescending(v => v.Transform.position.y).ThenByDescending(v => v.Transform.position.x).ToList();

            //for through and set order in layer
            for (int i = 0; i < sortList.Count; i++)
            {
                sortList[i].SpriteRenderer.sortingOrder = i;
            }
        }
    }

    /// <summary>
    /// Struct to hold necessary data to sort objects in Unity.
    /// </summary>
    private struct SortObject
    {
        public Transform      Transform;
        public SpriteRenderer SpriteRenderer;

        public SortObject(Transform transform, SpriteRenderer spriteRenderer)
        {
            Transform = transform;
            SpriteRenderer = spriteRenderer;
        }
    }
}