using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A heap data structure to reorder arrays more efficiently
/// </summary>
/// <typeparam name="T">Item type to store in the heap, must inherit IHeapItem<T></typeparam>
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    /// <summary>
    /// Adds a item to the heap and sorts the heap upwards.
    /// </summary>
    /// <param name="item">Item to add.</param>
    public void Add(T item)
    {
        //Sets the item to the end of the heap, updating its heap index
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;

        //Sort upwards and adjust count accordingly
        SortUp(item);
        currentItemCount++;
    }

    /// <summary>
    /// Swaps the places of two items in the heap.
    /// </summary>
    /// <param name="itemA"></param>
    /// <param name="itemB"></param>
    private void Swap(T itemA, T itemB)
    {
        //Swap the item's places in the array
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        //Also swap their indexes (fCost)
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

    /// <summary>
    /// Sorts the given item upwards in the heap.
    /// </summary>
    /// <param name="item">Item to sort.</param>
    private void SortUp(T item)
    {
        //Calculate parent index
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            //Get parent item and swap if its fCost is lower than the parent
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) //See compareTo documentation
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    /// <summary>
    /// Sorts the given item downwards in the heap.
    /// </summary>
    /// <param name="item">Item to sort.</param>
    private void SortDown(T item)
    {
        while (true)
        {
            //Calculate left and right child indexes
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            //Item has atleast one child
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                //If the item also has a child on the right
                if (childIndexRight < currentItemCount)
                {
                    //Compare the childrens fCosts
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        //Right child has lower fCost
                        swapIndex = childIndexRight;
                    }
                }

                //If parent has higher fCost than the child
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    //Parent has lower fCost than both if its children
                    return;
                }
            }
            else
            {
                //Parent has no children
                return;
            }
        }
    }

    /// <summary>
    /// Removes the first item in the heap and updates accordingly.
    /// </summary>
    /// <returns></returns>
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /// <summary>
    /// Count of the items currently in the heap.
    /// </summary>
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    /// <summary>
    /// Sorts the given item upwards.
    /// </summary>
    /// <param name="item">Given item.</param>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    /// <summary>
    /// Compares if the two items are equal.
    /// </summary>
    /// <param name="item">Item to compare to.</param>
    /// <returns>Returns true if equal, otherwise false.</returns>
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }
}
