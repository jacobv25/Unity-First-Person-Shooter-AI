using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
        elements.Sort((x, y) => x.Value.CompareTo(y.Value));
    }

    public T Dequeue()
    {
        var item = elements[0].Key;
        elements.RemoveAt(0);
        return item;
    }

    public bool IsEmpty()
    {
        return elements.Count == 0;
    }
}
