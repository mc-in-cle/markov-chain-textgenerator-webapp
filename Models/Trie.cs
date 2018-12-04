/*
 * A dictionary that stores data in a tree structure where the nodes
 * are bits of the keys instead of the actual keys.
 * For example, a Trie whose values are strings will
 * have character as its key bit type.
 * It is not immediately obvious that in this implementation,
 * generic type K is the bit, and the actual keys are
 * iterable sequences or arrays of type K.
 * 
 * This structure offers O(key.length) access, which is an improvement
 * over typical tree maps that offer O(log n) access.
 * Additionally, the storage required is substantially less.
 * 
 * M.C. 2018
 */

using System.Collections.Generic;
using System.Linq;

namespace markov_chain_generator_webapp.Models
{
    public class Trie<TKey, TValue>
    {
        Node<TKey, TValue> head = new Node<TKey, TValue>();
        HashPile<TValue> values = new HashPile<TValue>();

        public int Count { get; private set; }
        public TValue this[IEnumerable<TKey> key]
        {
            get {
                return Get(key.GetEnumerator());
            }

            set {
                Add(key.GetEnumerator(), value);
            }
        }

        public TValue this[TKey[] key]
        {
            get
            {
                return Get(key.Cast<TKey>().GetEnumerator());
            }

            set
            {
                Add(key.Cast<TKey>().GetEnumerator(), value);
            }
        }

        public void Clear()
        {
            head = new Node<TKey, TValue>();
            this.Count = 0;
        }

        private void Add(IEnumerator<TKey> sequence, TValue value)
        {
            if (value == null)
                return;

            Node<TKey, TValue> node = head;
            while (sequence.MoveNext())
            {
                TKey k = sequence.Current;
                Node<TKey, TValue> child = null;
                if (!node.Children.ContainsKey(k))
                {
                    child = new Node<TKey, TValue>();
                    node.Children.Add(k, child);
                }
                else {
                    child = node.Children[k];
                }
                node = child;
            }
            if (node.Value == null)
                Count++;

            //Overwrite previous value
            if (!ReferenceEquals(node.Value,value))
            {
                values.Add(value);
                values.RemoveExactly(node.Value);
                node.Value = value;
            }
        }

        public bool Contains(TKey[] sequence)
        {
            return Contains(sequence.Cast<TKey>().GetEnumerator());
        }

        public bool Contains(IEnumerable<TKey> sequence)
        {
            return Contains(sequence.GetEnumerator());
        }

        public bool Contains(IEnumerator<TKey> sequence)
        {
            Node<TKey, TValue> child = head;
            Node<TKey, TValue> node = head;
            while (sequence.MoveNext())
            {
                TKey k = sequence.Current;
                if (!node.Children.ContainsKey(k))
                {
                    return false;
                }
                child = node.Children[k];
                node = child;
            }
            return child.Value != null;
        }

        public void Remove(TKey[] sequence)
        {
            Remove(sequence.Cast<TKey>().GetEnumerator());
        }

        public void Remove(IEnumerable<TKey> sequence)
        {
            Remove(sequence.GetEnumerator());
        }

        void Remove(IEnumerator<TKey> sequence)
        {
            Node<TKey, TValue> child = head;
            Node<TKey, TValue> node = head;

            TKey emptyBranchParent = default(TKey);
            Node<TKey, TValue> lastNonEmpty = head;

            TKey current, next;
            if (!sequence.MoveNext())
                return;
            current = sequence.Current;
            bool hasNext = true;
            
            while (hasNext){
                hasNext = sequence.MoveNext();
                next = sequence.Current;

                if (!node.Children.ContainsKey(current)){
                    return;
                }
                child = node.Children[current];

                if (node.Value != null && hasNext){
                    lastNonEmpty = node;
                    emptyBranchParent = current;
                }
                if (hasNext){
                    node = child;
                }
                current = next;
            }

            Count--;
            values.RemoveExactly(child.Value);
            lastNonEmpty.Children.Remove(emptyBranchParent);
        }

        private TValue Get(IEnumerator<TKey> sequence)
        {
            Node<TKey, TValue> node = head;
            while (sequence.MoveNext())
            {
                TKey k = sequence.Current;
                if (!node.Children.ContainsKey(k))
                {
                    return default(TValue);
                }
                Node<TKey, TValue> child = node.Children[k];
                node = child;
            }
            return node.Value;
        }

        public IEnumerator<TValue> GetValuesEnumerator()
        {
            return values.GetEnumerator();

        }
    }

    public class Node<TKey_1, TValue_1>
    {
        public Dictionary<TKey_1, Node<TKey_1, TValue_1>> Children { get; set; }
        public TValue_1 Value { get; set; }

        public Node()
        {
            Children = new Dictionary<TKey_1, Node<TKey_1, TValue_1>>();
        }
    }

}
