using System;
using System.Collections;
using System.Collections.Generic;

namespace markov_chain_generator_webapp.Models
{
    public class HashPileTest
    {
        HashPile<string> sut = new HashPile<string>();

        /* public static void Main(string[] args)
        {
            HashPileTest test = new HashPileTest();
            Dictionary<string, bool> testResults = new Dictionary<string, bool>();
            
            testResults["TestAdd1CheckSize"] =  test.TestAdd1CheckSize();
            testResults["TestAdd1removeExactlySameCheckSize"] =  test.TestAdd1removeExactlySameCheckSize();
            testResults["TestAdd1removeExactlyEqualCheckSize"] =  test.TestAdd1removeExactlyEqualCheckSize();
            testResults["TestremoveExactlyNonMemberElement"] =  test.TestremoveExactlyNonMemberElement();
            testResults["TestAddTwoCheckSize"] =  test.TestAddTwoCheckSize();
            testResults["TestAddTworemoveExactly1CheckSize"] =  test.TestAddTworemoveExactly1CheckSize();
            testResults["TestAddTwoEqualNotSame"] =  test.TestAddTwoEqualNotSame();

            bool pass = true;
            foreach ( KeyValuePair<string, bool> entry in testResults)
            {
                if (entry.Value == false)
                {
                    Console.WriteLine("Test failure: " + entry.Key);
                    pass = false;
                }
            }
            if (pass)
                Console.WriteLine("Passed all Tests.");
        }
        */
        
        public bool TestAdd1CheckSize()
        {
            sut.Clear();
            sut.Add("abcd");
            return sut.Count == 1;
        }
        
        public bool TestAdd1removeExactlySameCheckSize()
         {
            string s = "abcd";
            sut.Clear();
            sut.Add(s);
            sut.RemoveExactly(s);
            return sut.Count == 0;
        }
        
        public bool TestAdd1removeExactlyEqualCheckSize() 
        {
            string s = "abcd";
            string t = "ab";
            t = t + "cd";
            sut.Clear();
            sut.Add(s);
            sut.RemoveExactly(t);
            return sut.Count == 1;
        }
        
        public bool TestremoveExactlyNonMemberElement() 
        {
            sut.Clear();
            sut.Add("aa");
            sut.RemoveExactly("zz");
            return sut.Count == 1;
        }
        
        public bool TestAddTwoCheckSize() 
        {
            sut.Clear();
            sut.Add("aa");
            sut.Add("bb");
            return sut.Count == 2;
        }
        
        public bool TestAddTworemoveExactly1CheckSize() 
        {
            sut.Clear();
            sut.Add("aa");
            sut.Add("bb");
            sut.RemoveExactly("aa");
            return sut.Count == 1;
        }
        
        public bool TestAddTwoEqualNotSame() 
        {
            string s = "abcd";
            string t = "ab";
            t = t + "cd";
            Console.WriteLine(s == t);
            sut.Clear();
            sut.Add(s);
            sut.Add(t);
            return sut.Count == 2;
        }
    }
}
