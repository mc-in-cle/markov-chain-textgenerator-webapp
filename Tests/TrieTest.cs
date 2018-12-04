using System;
using System.Collections;
using System.Collections.Generic;

namespace markov_chain_generator_webapp.Models
{
    public class TrieTest
    {
        //Uncomment the lists and comment out the arrays to test with different data type.
        //Both work.
        char[] listAB = { 'a', 'b' };
        char[] listABCD = { 'a', 'b', 'c', 'd' };
        char[] listABC = { 'a', 'b', 'c' };
        char[] listABCE = { 'a', 'b', 'c', 'e' };
        char[] listZZ = { 'z', 'z' };
        
        /* 
        List<char> listAB = new List<char>{ 'a', 'b' };
        List<char> listABCD = new List<char>{ 'a', 'b', 'c', 'd' };
        List<char> listABC = new List<char>{ 'a', 'b', 'c' };
        List<char> listABCE = new List<char>{ 'a', 'b', 'c', 'e' };
        List<char> listZZ = new List<char>{ 'z', 'z' };
        */

        Trie<char, string> sut = new Trie<char, string>();
        /* 
        public static void Main(string[] args)
        {
            TrieTest test = new TrieTest();
            Dictionary<string, bool> testResults = new Dictionary<string, bool>();
        
            testResults["TestPut1"] = test.TestPut1();
            testResults["TestPut2"] = test.TestPut2();
            testResults["TestGet1"] = test.TestGet1();
            testResults["TestContains1"] = test.TestContains1();
            testResults["TestDoesNotContain1"] = test.TestDoesNotContain1();
            
            testResults["TestRemoveContains"] = test.TestRemoveContains();
            
            testResults["TestRemoveDoesNotRemoveParent"] = test.TestRemoveDoesNotRemoveParent();
            testResults["TestRemoveDoesNotRemoveSibling"] = test.TestRemoveDoesNotRemoveSibling();
            testResults["TestSizeEmpty"] = test.TestSizeEmpty();
            testResults["TestSizeAddOne"] = test.TestSizeAddOne();
            testResults["TestSizeAddSeveral"] = test.TestSizeAddSeveral();
            testResults["TestSizeRemoveOne"] = test.TestSizeRemoveOne();
        
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

        public bool TestPut1()
        {
            sut.Clear();
            sut[listABCD] = "TestABCD";
            string result = sut[listABCD];
            return "TestABCD".Equals(result);
        }
        
        public bool TestPut2()
        {
            sut.Clear();
            sut[listABCD] = "TestABCD";
            sut[listABC] = "TestABC";
            string result = sut[listABC];
            return "TestABC".Equals(result);
        }

        public bool TestGet1()
        {
            sut.Clear();
            sut[listABCD] = "TestABCD";
            string result = sut[listABCD];
            return "TestABCD".Equals(result);
        }

        public bool TestContains1()
        {
            sut.Clear();
            sut[listABCD] = "GO-ABCD!";
            bool result = sut.Contains(listABCD);
            return result == true;
        }

        public bool TestDoesNotContain1()
        {
            sut.Clear();
            sut[listABC] = "GO-ABC!";
            bool result = sut.Contains(listAB);
            return result == false;
        }

        public bool TestDoesNotContain2()
        {
            sut.Clear();
            sut[listABC] = "GO-ABC!";
            bool result = sut.Contains(listZZ);
            return result == false;
        }

        public bool TestRemoveContains()
        {
            sut.Clear();
            sut[listAB] = "YE";
            sut[listABCD] = "GO-ABCD!";
            sut.Remove(listABCD);
            bool result = sut.Contains(listABCD);
            return result == false;
        }

        public bool TestRemoveDoesNotRemoveParent()
        {
            sut.Clear();
            sut[listABCD] = "GO-ABCD!";
            sut[listABC] = "YESABC";
            sut.Remove(listABCD);
            bool result = sut.Contains(listABC);
            return result == true;
        }

        public bool TestRemoveDoesNotRemoveSibling()
        {
            sut.Clear();
            sut[listABCD] = "GO-ABCD!";
            sut[listABCE] = "YESABCE";
            sut.Remove(listABCD);
            bool result = sut.Contains(listABCE);
            return result == true;
        }

        public bool TestSizeEmpty()
        {
            sut.Clear();
            return sut.Count == 0;
        }

        public bool TestSizeAddOne()
        {
            sut.Clear();
            sut[listZZ] = "000";
            return sut.Count == 1;
        }

        public bool TestSizeAddSeveral()
        {
            sut.Clear();
            sut[listABC] = "ABC!";
            sut[listABCD] = "ABCD!";
            sut[listAB] = "AB!";
            return sut.Count == 3;
        }

        public bool TestSizeRemoveOne()
        {
            sut.Clear();
            sut[listABC] = "ABC!";
            sut[listABCD] = "ABCD!";
            sut.Remove(listABC);
            return sut.Count == 1;
        }
    }
}

