//Bit.cs

using System;
using System.Linq;

namespace GoldenTicket.Apriori
{
    public class Bit
    {
        public static ItemsetCollection FindSubsets(Itemset itemset, int n)
        {
            var subsets = new ItemsetCollection();

            var subsetCount = (int)Math.Pow(2, itemset.Count);
            for (var i = 0; i < subsetCount; i++)
            {
                if (n == 0 || GetOnCount(i, itemset.Count) == n)
                {
                    var binary = DecimalToBinary(i, itemset.Count);

                    var subset = new Itemset();
                    for (var charIndex = 0; charIndex < binary.Length; charIndex++)
                    {
                        if (binary[charIndex] == '1')
                        {
                            subset.Add(itemset[charIndex]);
                        }
                    }
                    subsets.Add(subset);
                }
            }

            return (subsets);
        }

        public static int GetBit(int value, int position)
        {
            var bit = value & (int)Math.Pow(2, position);
            return (bit > 0 ? 1 : 0);
        }

        public static string DecimalToBinary(int value, int length)
        {
            var binary = string.Empty;
            for (var position = 0; position < length; position++)
            {
                binary = GetBit(value, position) + binary;
            }
            return (binary);
        }

        public static int GetOnCount(int value, int length)
        {
            var binary = DecimalToBinary(value, length);
            return (from char c in binary.ToCharArray()
                    where c == '1'
                    select c).Count();
        }
    }
}
