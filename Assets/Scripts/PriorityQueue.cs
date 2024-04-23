using System;
using System.Collections.Generic;




/// <summary>A minimum priority queue.</summary>
/// <typeparam name="TElement">The type of elements.</typeparam>
/// <typeparam name="TPriority">The type of priorities.</typeparam>
/// <remarks>Implemented as a binary heap.</remarks>
public class PriorityQueue<TElement, TPriority> where TPriority : IComparable
{
    /// <summary>The underlying heap of elements.</summary>
    private readonly List<PQElement> elements = new();

    /// <summary>
    /// The comparer used to compare the priorities of elements.
    /// </summary>
    private readonly IComparer<TPriority> comparer = Comparer<TPriority>.Default;




    /// <summary>Creates a new priority queue.</summary>
    public PriorityQueue()
    {
    }

    /// <summary>
    /// Creates a new priority queue with the specified comparer.
    /// </summary>
    /// <param name="comparer">The comparer to use.</param>
    public PriorityQueue(IComparer<TPriority> comparer)
    {
        this.comparer = comparer;
    }




    /// <summary>The number of elements.</summary>
    public int Count { get { return elements.Count; } }

    /// <summary>
    /// The comparer used to compare the priorities of elements.
    /// </summary>
    public IComparer<TPriority> Comparer { get { return comparer; } }




    /// <summary>Adds the element with the priority.</summary>
    /// <param name="element">The element to add.</param>
    /// <param name="priority">The priority of the element.</param>
    public void Push(TElement element, TPriority priority)
    {
        elements.Add(new PQElement(element, priority));
        int childIndex = Count - 1;
        int parentIndex = (childIndex - 1) / 2;
        while (childIndex > 0 && IsLowerPriority(childIndex, parentIndex))
        {
            Swap(parentIndex, childIndex);
            childIndex = parentIndex;
            parentIndex = (childIndex - 1) / 2;
        }
    }

    /// <summary>
    /// Removes and returns the element with the minimum priority.
    /// </summary>
    /// <returns>The element with the minimum priority.</returns>
    public TElement Pop()
    {
        TElement rootElement = elements[0].Element;
        Swap(0, Count - 1);
        elements.RemoveAt(Count - 1);
        int parentIndex = 0;
        int childIndex = GetIndexOfMinChild(parentIndex);
        while (childIndex < Count && IsLowerPriority(childIndex, parentIndex))
        {
            Swap(parentIndex, childIndex);
            parentIndex = childIndex;
            childIndex = GetIndexOfMinChild(parentIndex);
        }
        return rootElement;
    }

    /// <summary>
    /// Returns the element with the minimum priority without removing it.
    /// </summary>
    /// <returns>The element with the minimum priority.</returns>
    public TElement Peek()
    {
        return elements[0].Element;
    }


    /// <summary>
    /// If the <c>PriorityQueue</c> is not empty, removes
    /// the element with the minimum priority, copying it
    /// and it's priority to the provided parameters.
    /// </summary>
    /// <param name="element">
    /// Used to return the element with the minimum priority.
    /// </param>
    /// <param name="priority">
    /// Used to return the priority of the returned element.
    /// </param>
    /// <returns>
    /// <c>true</c> if there is an element to remove;
    /// <c>false</c> if the <c>PriorityQueue</c> is empty.
    /// </returns>
    public bool TryPop(out TElement element, out TPriority priority)
    {
        if (Count == 0)
        {
            element = default;
            priority = default;
            return false;
        }
        element = elements[0].Element;
        priority = elements[0].Priority;
        Pop();
        return true;
    }

    /// <summary>
    /// If the <c>PriorityQueue</c> is not empty, copies the element with
    /// the minimum priority and it's priority to the provided parameters.
    /// </summary>
    /// <param name="element">
    /// Used to return the element with the minimum priority.
    /// </param>
    /// <param name="priority">
    /// Used to return the priority of the returned element.
    /// </param>
    /// <returns>
    /// <c>true</c> if there is an element to peek;
    /// <c>false</c> if the <c>PriorityQueue</c> is empty.
    /// </returns>
    public bool TryPeek(out TElement element, out TPriority priority)
    {
        if (Count == 0)
        {
            element = default;
            priority = default;
            return false;
        }
        element = elements[0].Element;
        priority = elements[0].Priority;
        return true;
    }


    /// <summary>Removes all elements.</summary>
    public void Clear()
    {
        elements.Clear();
    }




    /// <summary>
    /// Gets the index of the child with the minimum priority.
    /// </summary>
    /// <param name="parentIndex">The index of the parent.</param>
    /// <returns>
    /// The index of the child with the minimum priority.
    /// If the parent only has one child, returns the index of the child.
    /// If the parent has no children, returns an index out of bounds.
    /// </returns>
    private int GetIndexOfMinChild(int parentIndex)
    {
        int leftIndex = parentIndex * 2 + 1;
        int rightIndex = parentIndex * 2 + 2;
        if (rightIndex < Count && IsLowerPriority(rightIndex, leftIndex))
        {
            return rightIndex;
        }
        return leftIndex;
    }

    /// <summary>
    /// Determines if <c>elements[i]</c> has a
    /// lower priority than <c>elements[j]</c>.
    /// </summary>
    /// <param name="i">The index of the first element to consider.</param>
    /// <param name="j">The index of the second element to consider.</param>
    /// <returns>
    /// <c>true</c> if <c>elements[i]</c> has a lower priority
    /// than <c>elements[j]</c>; otherwise, <c>false</c>.
    /// </returns>
    private bool IsLowerPriority(int i, int j)
    {
        TPriority priorityI = elements[i].Priority;
        TPriority priorityJ = elements[j].Priority;
        return comparer.Compare(priorityI, priorityJ) < 0;
    }

    /// <summary>
    /// Swaps <c>elements[i]</c> with <c>elements[j]</c>.
    /// </summary>
    /// <param name="i">The index of the first element to swap.</param>
    /// <param name="j">The index of the second element to swap.</param>
    private void Swap(int i, int j)
    {
        (elements[i], elements[j]) = (elements[j], elements[i]);
    }




    /// <summary>An element in the underlying heap.</summary>
    private readonly struct PQElement
    {
        /// <summary>The element.</summary>
        public TElement Element { get; }

        /// <summary>The priority of the element.</summary>
        public TPriority Priority { get; }


        /// <summary>Creates an element.</summary>
        /// <param name="element">The element.</param>
        /// <param name="priority">The priority of the element.</param>
        public PQElement(TElement element, TPriority priority)
        {
            Element = element;
            Priority = priority;
        }
    }
}
