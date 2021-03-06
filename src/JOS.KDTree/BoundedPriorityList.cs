﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace JOS.KDTree
{
    /// <summary>
    /// A list of limited length that remains sorted by <typeparamref name="TPriority"/>.
    /// Useful for keeping track of items in nearest neighbor searches. Insert is O(log n). Retrieval is O(1)
    /// </summary>
    /// <typeparam name="TElement">The type of element the list maintains.</typeparam>
    /// <typeparam name="TPriority">The type the elements are prioritized by.</typeparam>
    public class BoundedPriorityList<TElement, TPriority> : IEnumerable<TElement>
        where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// The list holding the actual elements
        /// </summary>
        private readonly List<TElement> _elementList;

        /// <summary>
        /// The list of priorities for each element.
        /// There is a one-to-one correspondence between the
        /// priority list ad the element list.
        /// </summary>
        private readonly List<TPriority> _priorityList;

        /// <summary>
        /// Gets the element with the largest priority.
        /// </summary>
        public TElement MaxElement => _elementList[_elementList.Count - 1];

        /// <summary>
        /// Gets the largest priority.
        /// </summary>
        public TPriority MaxPriority => _priorityList[_priorityList.Count - 1];

        /// <summary>
        /// Gets the element with the lowest priority.
        /// </summary>
        public TElement MinElement => _elementList[0];

        /// <summary>
        /// Gets the smallest priority.
        /// </summary>
        public TPriority MinPriority => _priorityList[0];

        /// <summary>
        /// Gets the maximum allows capacity for the <see cref="BoundedPriorityList{TElement,TPriority}"/>
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Returns true if the list is at maximum capacity.
        /// </summary>
        public bool IsFull => Count == Capacity;

        /// <summary>
        /// Returns the count of items currently in the list.
        /// </summary>
        public int Count => _priorityList.Count;

        /// <summary>
        /// Indexer for the internal element array.
        /// </summary>
        /// <param name="index">The index in the array.</param>
        /// <returns>The element at the specified index.</returns>
        public TElement this[int index] => _elementList[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedPriorityList{TElement, TPriority}"/> class.
        /// Note: You should not have <paramref name="allocate"/> set to true, and the capacity set to a very large number.
        /// Especially if you will be creating and destroying many <see cref="BoundedPriorityList{TElement,TPriority}"/> very rapidly.
        /// If you ignore this advice you will create lots of memory pressure. If you don't understand why this is a problem you should
        /// understand the garbage collector. Please read: https://msdn.microsoft.com/en-us/library/ee787088.aspx
        /// </summary>
        /// <param name="capacity">The maximum capacity of the list.</param>
        /// <param name="allocate">If true, initializes the internal lists for the <see cref="BoundedPriorityList{TElement,TPriority}"/> with an initial capacity of <paramref name="capacity"/>.</param>
        public BoundedPriorityList(int capacity, bool allocate = false)
        {
            Capacity = capacity;
            if (allocate)
            {
                _priorityList = new List<TPriority>(capacity);
                _elementList = new List<TElement>(capacity);
            }
            else
            {
                _priorityList = new List<TPriority>();
                _elementList = new List<TElement>();
            }
        }

        /// <summary>
        /// Attempts to add the provided  <paramref name="item"/>. If the list
        /// is currently at maximum capacity and the elements priority is greater
        /// than or equal to the highest priority, the <paramref name = "item"/> is not inserted. If the
        /// <paramref name = "item"/> is eligible for insertion, the upon insertion the <paramref name = "item"/> that previously
        /// had the largest priority is removed from the list.
        /// This is an O(log n) operation.
        /// </summary>
        /// <param name="item">The item to be inserted</param>
        /// <param name="priority">The priority of th given item.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TElement item, TPriority priority)
        {
            if (Count >= Capacity)
            {
                if (_priorityList[_priorityList.Count - 1].CompareTo(priority) < 0)
                {
                    return;
                }

                var index = _priorityList.BinarySearch(priority);
                index = index >= 0 ? index : ~index;

                _priorityList.Insert(index, priority);
                _elementList.Insert(index, item);

                _priorityList.RemoveAt(_priorityList.Count - 1);
                _elementList.RemoveAt(_elementList.Count - 1);
            }
            else
            {
                var index = _priorityList.BinarySearch(priority);
                index = index >= 0 ? index : ~index;

                _priorityList.Insert(index, priority);
                _elementList.Insert(index, item);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return _elementList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
