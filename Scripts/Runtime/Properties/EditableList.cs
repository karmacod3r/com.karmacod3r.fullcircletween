using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class EditableList<T> : IList<T>
    {
        [SerializeField] private List<T> list;

        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        public bool IsReadOnly => false;

        public EditableList()
        {
            list = new List<T>();
        }

        /// <summary><para>Initializes a new list that is empty and has the default initial capacity.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.List%601">`List` on docs.go-mono.com</a></footer>
        public EditableList(IEnumerable<T> collection)
        {
            list = new List<T>(collection);
        }

        /// <summary><para>Initializes a new list that is empty and has the default initial capacity.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.List%601">`List` on docs.go-mono.com</a></footer>
        public EditableList(int capacity)
        {
            list = new List<T>(capacity);
        }

        /// <summary><para>Gets or sets the number of elements the current instance can contain.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Capacity">`List.Capacity` on docs.go-mono.com</a></footer>
        public int Capacity
        {
            get => list.Capacity;
            set => list.Capacity = value;
        }

        /// <summary><para>Gets the number of elements contained in the current instance.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Count">`List.Count` on docs.go-mono.com</a></footer>
        public int Count => list.Count;

        /// <summary><para>Gets or sets the element at the specified index of the current instance.</para></summary>
        /// <param name="index">The zero-based index of the element in the current instance to get or set.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Item">`List.Item` on docs.go-mono.com</a></footer>
        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        bool ICollection<T>.IsReadOnly => false;

        /// <summary><para>Adds an item to the end of the list.</para></summary>
        /// <param name="item">The item to add to the end of the list. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Add">`List.Add` on docs.go-mono.com</a></footer>
        public void Add(T item) => list.Add(item);

        /// <summary><para>Adds the elements of the specified collection to the end of the list.</para></summary>
        /// <param name="collection">The collection whose elements are added to the end of the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.AddRange">`List.AddRange` on docs.go-mono.com</a></footer>
        public void AddRange(IEnumerable<T> collection) => list.AddRange(collection);

        /// <summary><para>Returns a read-only wrapper to the current List.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.AsReadOnly">`List.AsReadOnly` on docs.go-mono.com</a></footer>
        public ReadOnlyCollection<T> AsReadOnly() => list.AsReadOnly();

        /// <summary><para>Searches the entire sorted list for an element using the default comparer, and returns the zero-based index of the element.</para></summary>
        /// <param name="item">The element for which to search. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.BinarySearch">`List.BinarySearch` on docs.go-mono.com</a></footer>
        public int BinarySearch(T item) => list.BinarySearch(item);

        /// <summary><para>Searches the entire sorted list for an element using the specified comparer and returns the zero-based index of the element.</para></summary>
        /// <param name="item">The element for which to search. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.BinarySearch">`List.BinarySearch` on docs.go-mono.com</a></footer>
        public int BinarySearch(T item, IComparer<T> comparer) => list.BinarySearch(item, comparer);

        /// <summary><para>Searches a range of elements in the sorted list for an element using the specified comparer and returns the zero-based index of the element.</para></summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.BinarySearch">`List.BinarySearch` on docs.go-mono.com</a></footer>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer) => list.BinarySearch(index, count, item, comparer);

        /// <summary><para>Removes all elements from the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Clear">`List.Clear` on docs.go-mono.com</a></footer>
        public void Clear() => list.Clear();

        /// <summary><para>Determines whether the list contains a specific value.</para></summary>
        /// <param name="item">The object to locate in the current collection. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Contains">`List.Contains` on docs.go-mono.com</a></footer>
        public bool Contains(T item) => list.Contains(item);

        /// <summary>To be added.</summary>
        /// <typeparam name="TOutput">To be added.</typeparam>
        /// <param name="converter">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.ConvertAll">`List.ConvertAll` on docs.go-mono.com</a></footer>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => list.ConvertAll(converter);

        /// <summary><para>Copies the entire list to an array.</para></summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements copied from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        public void CopyTo(T[] array) => list.CopyTo(array);

        /// <summary><para>Copies the elements of the list to an array, starting at a particular index.</para></summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements copied from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        /// <summary><para>Copies a range of elements of the list to an array, starting at a particular index in the target array.</para></summary>
        /// <param name="index">The zero-based index in the source list at which copying begins.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        public void CopyTo(int index, T[] array, int arrayIndex, int count) => list.CopyTo(index, array, arrayIndex, count);

        /// <summary><para>Determines whether the List contains elements that match the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Exists">`List.Exists` on docs.go-mono.com</a></footer>
        public bool Exists(Predicate<T> match) => list.Exists(match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Find">`List.Find` on docs.go-mono.com</a></footer>
        public T Find(Predicate<T> match) => list.Find(match);

        /// <summary><para>Retrieves all the elements that match the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindAll">`List.FindAll` on docs.go-mono.com</a></footer>
        public List<T> FindAll(Predicate<T> match) => FindAll(match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the List that starts at the specified index and contains the specified number of elements.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindIndex">`List.FindIndex` on docs.go-mono.com</a></footer>
        public int FindIndex(int startIndex, int count, Predicate<T> match) => list.FindIndex(startIndex, count, match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the List that extends from the specified index to the last element.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindIndex">`List.FindIndex` on docs.go-mono.com</a></footer>
        public int FindIndex(int startIndex, Predicate<T> match) => list.FindIndex(startIndex, match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindIndex">`List.FindIndex` on docs.go-mono.com</a></footer>
        public int FindIndex(Predicate<T> match) => FindIndex(match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLast">`List.FindLast` on docs.go-mono.com</a></footer>
        public T FindLast(Predicate<T> match) => list.FindLast(match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the List that starts at the specified index and contains the specified number of elements going backwards.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLastIndex">`List.FindLastIndex` on docs.go-mono.com</a></footer>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => list.FindLastIndex(startIndex, count, match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the List that extends from the specified index to the first element.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLastIndex">`List.FindLastIndex` on docs.go-mono.com</a></footer>
        public int FindLastIndex(int startIndex, Predicate<T> match) => list.FindLastIndex(startIndex, match);

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLastIndex">`List.FindLastIndex` on docs.go-mono.com</a></footer>
        public int FindLastIndex(Predicate<T> match) => list.FindLastIndex(match);

        /// <summary><para>Performs the specified action on each element of the List.</para></summary>
        /// <param name="action">The action delegate to perform on each element of the List.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.ForEach">`List.ForEach` on docs.go-mono.com</a></footer>
        public void ForEach(Action<T> action) => list.ForEach(action);

        /// <summary><para>Returns an enumerator, in index order, that can be used to iterate over the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetEnumerator">`List.GetEnumerator` on docs.go-mono.com</a></footer>
        public List<T>.Enumerator GetEnumerator() => list.GetEnumerator();

        /// <summary><para>Creates a shallow copy of a range of elements in the current List.</para></summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetRange">`List.GetRange` on docs.go-mono.com</a></footer>
        public List<T> GetRange(int index, int count) => list.GetRange(index, count);

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the entire list.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        public int IndexOf(T item) => list.IndexOf(item);

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the list that extends from the specified index to the last element.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        public int IndexOf(T item, int index) => list.IndexOf(item, index);

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the list that starts at the specified index and contains the specified number of elements.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        public int IndexOf(T item, int index, int count) => list.IndexOf(item, index, count);

        /// <summary><para>Inserts an item to the List at the specified position.</para></summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> is to be inserted.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Insert">`List.Insert` on docs.go-mono.com</a></footer>
        public void Insert(int index, T item) => list.Insert(index, item);

        /// <summary><para>Inserts the elements of a collection in the List at the specified position.</para></summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.InsertRange">`List.InsertRange` on docs.go-mono.com</a></footer>
        public void InsertRange(int index, IEnumerable<T> collection) => list.InsertRange(index, collection);

        /// <summary><para>Searches for the specified object and returns the zero-based index of the last occurrence within the entire list.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.LastIndexOf">`List.LastIndexOf` on docs.go-mono.com</a></footer>
        public int LastIndexOf(T item) => list.LastIndexOf(item);

        /// <summary><para>Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the list that extends from the specified index to the last element.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.LastIndexOf">`List.LastIndexOf` on docs.go-mono.com</a></footer>
        public int LastIndexOf(T item, int index) => list.LastIndexOf(item, index);

        /// <summary><para>Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the list that starts at the specified index and contains the specified number of elements.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.LastIndexOf">`List.LastIndexOf` on docs.go-mono.com</a></footer>
        public int LastIndexOf(T item, int index, int count) => list.LastIndexOf(item, index, count);

        /// <summary><para>Removes the first occurrence of the specified object from the list.</para></summary>
        /// <param name="item">The object to be removed from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Remove">`List.Remove` on docs.go-mono.com</a></footer>
        public bool Remove(T item) => list.Remove(item);

        /// <summary><para>Removes the all the elements that match the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the elements to remove.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.RemoveAll">`List.RemoveAll` on docs.go-mono.com</a></footer>
        public int RemoveAll(Predicate<T> match) => list.RemoveAll(match);

        /// <summary><para>Removes the item at the specified index of the list.</para></summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.RemoveAt">`List.RemoveAt` on docs.go-mono.com</a></footer>
        public void RemoveAt(int index) => list.RemoveAt(index);

        /// <summary><para>Removes a range of elements from the list.</para></summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.RemoveRange">`List.RemoveRange` on docs.go-mono.com</a></footer>
        public void RemoveRange(int index, int count) => list.RemoveRange(index, count);

        /// <summary><para>Reverses the order of the elements in the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Reverse">`List.Reverse` on docs.go-mono.com</a></footer>
        public void Reverse() => list.Reverse();

        /// <summary><para>Reverses the order of the elements in the specified element range of the list.</para></summary>
        /// <param name="index">The zero-based starting index of the range of elements to reverse.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Reverse">`List.Reverse` on docs.go-mono.com</a></footer>
        public void Reverse(int index, int count) => list.Reverse(index, count);

        /// <summary><para>Sorts the elements in the list using the default comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort() => list.Sort();

        /// <summary><para>Sorts the elements in the list using the specified comparer.</para></summary>
        /// <param name="comparer"><para>The <see cref="T:System.Collections.Generic.IComparer&lt;T&gt;" />  implementation to use when comparing elements.</para><para>-or-</para><para><see langword="null" /> to use the default comparer.</para></param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort(IComparer<T> comparer) => list.Sort(comparer);

        /// <summary><para>Sorts the elements in the list using the specified comparison.</para></summary>
        /// <param name="comparison"><para>The comparison to use when comparing elements.</para></param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort(Comparison<T> comparison) => list.Sort(comparison);

        /// <summary><para>Sorts the elements in the list using the specified comparer.</para></summary>
        /// <param name="index">The zero-based starting index of the range of elements to sort.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort(int index, int count, IComparer<T> comparer) => list.Sort(index, count, comparer);

        /// <summary><para>Returns an enumerator, in index order, that can be used to iterate over the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetEnumerator">`List.GetEnumerator` on docs.go-mono.com</a></footer>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => list.GetEnumerator();

        /// <summary><para>Copies the elements in the list to a new array.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.ToArray">`List.ToArray` on docs.go-mono.com</a></footer>
        public T[] ToArray() => list.ToArray();

        /// <summary><para>Suggests that the capacity be reduced to the actual number of elements in the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.TrimExcess">`List.TrimExcess` on docs.go-mono.com</a></footer>
        public void TrimExcess() => list.TrimExcess();

        /// <summary><para>Determines whether every element in the List matches the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the check against the elements.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.TrueForAll">`List.TrueForAll` on docs.go-mono.com</a></footer>
        public bool TrueForAll(Predicate<T> match) => list.TrueForAll(match);
    }
}