using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PainterScript : MonoBehaviour
{

    private List<SpriteRenderer> rend;
    private List<GameObject>     objects;
    private List<Vector3>        positions;
    public bool                  sorting;

    // Use this for initialization
    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Sortable").ToList();
        /*for (int i = 0; i < objects.Count; i++)
        {
            positions.Add(objects[i].transform.position);
        }

        for (int i = 0; i < objects.Count; i++)
        {
            rend.Add(objects[i].GetComponent<SpriteRenderer>());
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (sorting)
        {
            //Sort positions list
            objects = objects.OrderByDescending(v => v.transform.position.y).ThenByDescending(v => v.transform.position.x).ToList();

            //for through and set order in layer
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].GetComponent<SpriteRenderer>().sortingOrder = i;
            }
        }

        //Simple, right?
    }
}
