using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<(T, float)> heap;
    private IComparer<float> comparer;

    public PriorityQueue() : this(Comparer<float>.Default) { }

    public PriorityQueue(IComparer<float> comparer)
    {
        this.heap = new List<(T, float)>();
        this.comparer = comparer;
    }

    public int Count => heap.Count;

    public void Enqueue(T item, float priority)
    {
        heap.Add((item, priority));
        int index = heap.Count - 1;
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (comparer.Compare(heap[parentIndex].Item2, heap[index].Item2) <= 0)
                break;
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    public (T, float) Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");

        (T item, float priority) = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;

        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex > lastIndex)
                break;

            int rightChildIndex = leftChildIndex + 1;
            int minChildIndex = (rightChildIndex <= lastIndex && comparer.Compare(heap[rightChildIndex].Item2, heap[leftChildIndex].Item2) < 0)
                ? rightChildIndex
                : leftChildIndex;

            if (comparer.Compare(heap[parentIndex].Item2, heap[minChildIndex].Item2) <= 0)
                break;

            Swap(parentIndex, minChildIndex);
            parentIndex = minChildIndex;
        }

        return (item, priority);
    }

    public (T, float) Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");

        return heap[0];
    }

    private void Swap(int i, int j)
    {
        (T, float) temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}
