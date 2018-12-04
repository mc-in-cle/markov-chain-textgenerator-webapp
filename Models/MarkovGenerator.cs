
/* Establishes a Markov chain for sequenced objects, and uses this model to generate random output 
 * based on the input sequence.
 * 
 * M.C. 2018
 */

using System;
using System.Collections.Generic;
namespace markov_chain_generator_webapp.Models
{
    public class MarkovGenerator<T>
    {
        int order;
        Trie<T, MarkovMap<T>> subsequences;
        List<T> firstInputSubseq;
        LinkedList<T> outputSubseq;
        
        public bool Finalized { get; private set; }
        public int Count => subsequences.Count;

        ///<summary>Creates a new Markov chain from the data found by the given file reader.</summary>
        ///<param name="order"> the order of the Markov model; length of observed subsequences</param>
        public MarkovGenerator(int order)
        {
            Finalized = false;
            this.order = order;
            subsequences = new Trie<T, MarkovMap<T>>();
        }

        ///<summary>Reads all the input from another iterator and adds that information to the Markov chain,
        ///unless the generator has already been finalized.</summary>
        ///<param name="dataIterator">iterates through the next input sequence.</param>
        ///<returns>true if the information from the iterator was added to the model, or false if the model 
        ///was not changed as a result of the call.</returns>
        public bool AddEnumerator(IEnumerator<T> dataIterator)
        {
            if(Finalized)
                return false;
            GetFirst(dataIterator);
            BuildMarkovs(dataIterator);
            return true;
        }

        ///<summary>Obtains the first subsequence of length <code>order</code> from the iterator.</summary>
        void GetFirst(IEnumerator<T> dataReader)
        {
            firstInputSubseq = new List<T>();
            int i = 0;
            while (dataReader.MoveNext() && i < order)
            {
                firstInputSubseq.Add(dataReader.Current);
                i++;
            }
            if (firstInputSubseq.Count < order)
                throw new InvalidOperationException("Not enough data provided. Obtained only "
                                                    + firstInputSubseq.Count + "items.");
        }


        ///<summary>Reads through the entire file and builds Markov objects for each subsequence found in the file.
        ///Does not build a Markov object for the last subsequence found in the input because there is no 
        ///data to follow it.</summary>
        void BuildMarkovs(IEnumerator<T> dataReader)
        {

            subsequences[firstInputSubseq] = new MarkovMap<T>();
            LinkedList<T> subSeq = new LinkedList<T>();
            foreach (T t in firstInputSubseq)
                subSeq.AddLast(t);
            MarkovMap<T> mFirst = subsequences[subSeq];
            T first = dataReader.Current;
            mFirst.Add(first);
            subSeq.RemoveFirst();
            subSeq.AddLast(first);

            while (dataReader.MoveNext())
            {
                T nextItem = dataReader.Current;
                MarkovMap<T> m = subsequences[subSeq];
                if (m == null)
                {
                    m = new MarkovMap<T>();
                    subsequences[subSeq] = m;
                }
                m.Add(nextItem);
                //shift left
                subSeq.RemoveFirst();
                subSeq.AddLast(nextItem);
            }
        }


        ///<summary>Prepares for generation. Must be called before 
        /// <code>nextRandom()</code> can be used.</summary>
        public void FinalizeGenerator()
        {
            if (subsequences.Count == 0)
            {
                throw new InvalidOperationException("Not enough information provided through addIterator() to finalize MarkovGenerator.");
            }
            IEnumerator<MarkovMap<T>> maps = subsequences.GetValuesEnumerator();
            while (maps.MoveNext())
            {
                maps.Current.GetReady();
            }
            Finalized = true;
            outputSubseq = new LinkedList<T>();
        }

        ///<summary>Gets the next random item based on the Markov chain for given sequence.</summary>
        ///<param name="seq">the substring of length k which is referred to in the Markov chain.</param> 
        ///<return>a random item based on the Markov chain, or <code>default(T)</code> 
        ///if the last subsequence in the source is inputted, since it is excluded from the model.</return> 
        private T NextRandom(IEnumerable<T> seq)
        {
            if(!Finalized)
                throw new InvalidOperationException("MarkovGenerator has not been finalized.");
            MarkovMap<T> m;
            T t;
            try{
                m = subsequences[seq];
                t = m.Random();
            }catch(ArgumentNullException e){
                return default(T);
            }catch(NullReferenceException e){
                return default(T);
            }       
            return t;
        }

        ///<summary>Uses the complete Markov model to generate a random sequence of given length
        ///based on the model and the previous sequence of items generated.</summary>
        ///<param name="length">the length of output to generate</param>
        public List<T> GenerateList(int length)
        {
            List<T> fullSeq = new List<T>(length);
            fullSeq.AddRange(outputSubseq);
            while(fullSeq.Count < length){
                fullSeq.Add(Generate());
            }
            return fullSeq;
        }

        ///<summary>Uses the complete Markov model to generate a random item based on the model
        ///and the previous sequence of items generated.</summary>
        public T Generate()
        {
            if (!Finalized)
                    throw new InvalidOperationException("MarkovGenerator has not yet been finalized.");
            //This is how the program keeps track of having reset to the start of the sequence.

            if (outputSubseq.Count < order)
            {
                T next = firstInputSubseq[outputSubseq.Count];
                outputSubseq.AddLast(next);
                return next;
            }
            else
            {
                T nextRand = NextRandom(outputSubseq);
                //If nextRandom returns null, that means outputSubeq is the last subsequence in the source 
                //and has no Markov model associated with it.
                //In this case, reset to the first subsequence.
                if (nextRand.Equals(default(T)))
                {
                    outputSubseq.Clear();
                    T next = firstInputSubseq[0];
                    outputSubseq.AddLast(firstInputSubseq[0]);
                    return next;
                }
                else{
                    //shift left
                    outputSubseq.RemoveFirst();
                    outputSubseq.AddLast(nextRand);
                }
            return nextRand;
            }
        }
    }
}
