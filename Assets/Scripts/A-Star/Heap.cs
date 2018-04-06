using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {

    }
    //WIP
    public interface IHeapItem<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}
