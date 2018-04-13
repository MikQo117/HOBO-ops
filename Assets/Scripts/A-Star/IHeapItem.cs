using System;
using System.Collections;

/// <summary>
/// Item type that can be stored in a heap
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
