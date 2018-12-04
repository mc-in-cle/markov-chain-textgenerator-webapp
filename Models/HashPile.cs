
/*
 * A hot and loose data structure that must be used with caution.
 * HashPile is like a HashSet, but can store multiple distinct
 * objects that are equal.
 * Permits constant-time remove and contains operations.
 * "Remove" is constant-time on average. Worst case is linear time,
 * which occurs all the objects in the HashPile are Equal.
 * 
 * Caution: HashPile is not appropriate for collections of objects
 * that will be mutated. Objects that are equal when they are stored 
 * in the HashPile must continue to be equal, or the HashPile will 
 * fail to behave correctly.
 * 
 * M.C. 2018
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace markov_chain_generator_webapp.Models
{
    public class HashPile<T> : IEnumerable<T>
    {
        Dictionary<T, Wad<T>> pile = new Dictionary<T, Wad<T>>();

        public int Count { get; private set; }

        public void Clear()
        {
            pile.Clear();
            Count = 0;
        }

        public void Add(T item)
        {
            if (!pile.ContainsKey(item))
            {
                pile[item] = new Wad<T>(item);
            }
            else
            {
                pile[item].Add(item);
            }
            Count++;
        }

        public bool Contains(T item)
        {
            if (pile.ContainsKey(item))
                return pile[item].Contains(item);
            return false;
        }

        /*
         * Remove the exact item 'item'. Unlike the remove() specified
         * in Collections, this method will not remove the first item
         * found which is equal to item; it will only remove the same item.
         */
        public void RemoveExactly(T item)
        {
            if (item == null || !pile.ContainsKey(item))
            {
                return;
            }
            else
            {
                Wad<T> wad = pile[item];
                int before = wad.Count;
                wad.RemoveExactly(item);
                Count -= before - wad.Count;
                if (wad.Count == 0)
                    pile.Remove(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /*
         * This enumerator will always group like items together.
         */
        public IEnumerator<T> GetEnumerator()
        {
            foreach (Wad<T> wad in pile.Values)
            {
                foreach (T t in wad)
                {
                    yield return t;
                }
            }
        }

        //A list of objects that are Equal, but may or may not be the same.
        public class Wad<U> : IEnumerable<U>
        {
            LinkedList<WrappedItem<U>> list;
            public int Count => list.Count;

            public Wad(U item)
            {
                list = new LinkedList<WrappedItem<U>>();
                list.AddLast(new WrappedItem<U>(item));
            }

            //Ignores items that are not equal to the other items in the Wad.
            public void Add(U item)
            {
                if (!list.First.Value.Item.Equals(item))
                    return;
                else
                    list.AddLast(new WrappedItem<U>(item));
            }

            public void RemoveExactly(U item)
            {
                WrappedItem<U> toRemove = null;
                foreach (WrappedItem<U> u in list)
                {
                    if (ReferenceEquals(item, u.Item))
                    {
                        toRemove = u;
                    }
                }
                if (toRemove != null)
                    list.Remove(toRemove);
            }

            public U Item()
            {
                return list.First.Value.Item;
            }

            public bool Contains(U item)
            {
                foreach (WrappedItem<U> u in list)
                {
                    if (ReferenceEquals(item, u.Item))
                    {
                        return true;
                    }
                }
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<U> GetEnumerator()
            {
                foreach (WrappedItem<U> u in list)
                {
                    yield return u.Item;
                }
            }
        }

        public class WrappedItem<V>
        {
            public V Item {get;}
            public WrappedItem(V item)
            {
                this.Item = item;
            }
        }

    }

}


