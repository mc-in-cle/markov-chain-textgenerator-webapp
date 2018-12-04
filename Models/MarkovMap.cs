


using System;
using System.Collections.Generic;

namespace markov_chain_generator_webapp.Models
{
    //Represents one node in a Markov chain.
    //Used in conjunction with MarkovGenerator.
    ///M.C. 2018
    public class MarkovMap<T>
    {
        /**Number of documented occurrences of the node sequence*/
        int freq;
        /**The Markov state-change map itself, with frequencies in integers rather than probabilities*/
        Dictionary<T, int> occurrenceMap;
        /**A prepared O(1) map for fast Markov chain generation**/
        List<T> randMap;

        public bool ready { get; private set; }


        public MarkovMap()
        {
            freq = 0;
            ready = false;
            occurrenceMap = new Dictionary<T, int>();
            randMap = null;
        }

        ///<summary>Document an instance of a suffix following the node.
        ///If called after <code>getReady(), getReady()</code> must be called again before <code>random()</code>.
        ///</summary>
        ///<param name="suffix"> a suffix observed to follow the node sequence.</param>
        ///<returns> true if suffix was documented in this map for the first time.</returns>
        public bool Add(T suffix)
        {
            if (ready) ready = false;
            int prevCount = 0;
            if (occurrenceMap.ContainsKey(suffix))
            {
                prevCount = occurrenceMap[suffix];
            }

            occurrenceMap[suffix] = prevCount + 1;
            freq++;
            if (prevCount == 0) return true;
            return false;
        }


        ///<summary>Produces a random suffix state based on the model.</summary>
        public T Random()
        {
            if (!ready)
                throw new InvalidOperationException("This MarkovMap has not been readied for text generation yet.");
            int rand = new Random().Next(freq);
            return randMap[rand];
        }


        ///<summary>Prepares the MarkovMap for fast random chain generation. 
        /// Must be called before <code>random()</code> can be called.</summary>
        public void GetReady()
        {
            randMap = new List<T>(freq);

            foreach (KeyValuePair<T, int> entry in occurrenceMap)
            {
                int value = entry.Value;
                T key = entry.Key;
                for (int j = 0; j < value; j++)
                {
                    randMap.Add(key);
                }
            }
            ready = true;
        }
    }
}
