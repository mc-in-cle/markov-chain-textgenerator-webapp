/*
 * Wraps a StreamReader for a file in an IEnumerable.
 * Suppresses IOExceptions by stopping iteration.
 */

using System.IO;
using System.Collections.Generic;
using System.Collections;
namespace markov_chain_generator_webapp.Models
{
    public class EnumerableTextFile : IEnumerable<char>
    {
        string fileName;
        StreamReader reader;
        int buff;

        public EnumerableTextFile(string fileName)
        {
            //try
            //{
                reader = new StreamReader(fileName);
                buff = reader.Read();
            //} catch (IOException e)
            //{
            //    buff = -1;
            //}
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public IEnumerator<char> GetEnumerator()
        {
            while (buff != -1)
            {
                char next = (char)buff;
                try
                {
                    buff = reader.Read();
                }catch (IOException e)
                {
                    buff = -1;
                }
                yield return next;
            }
        }
    }
}
