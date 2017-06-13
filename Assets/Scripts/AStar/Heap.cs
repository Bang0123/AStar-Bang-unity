using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] _array;
    private int _cArrayCount;


    public Heap(int maxHeapSize)
    {
        _array = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = _cArrayCount;
        _array[_cArrayCount] = item;
        SortUp(item);
        _cArrayCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _array[0];
        _cArrayCount--;
        _array[0] = _array[_cArrayCount];
        _array[0].HeapIndex = 0;
        SortDown(_array[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return _cArrayCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(_array[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if (childIndexLeft < _cArrayCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < _cArrayCount)
                {
                    if (_array[childIndexLeft].CompareTo(_array[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(_array[swapIndex]) < 0)
                {
                    Swap(item, _array[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = _array[parentIndex];
            if (item.CompareTo(parentItem) > 0)
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

    void Swap(T itemA, T itemB)
    {
        _array[itemA.HeapIndex] = itemB;
        _array[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }




}
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}