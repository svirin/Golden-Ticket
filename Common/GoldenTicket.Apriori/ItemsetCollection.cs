//ItemsetCollection.cs

using System.Collections.Generic;
using System.Linq;

namespace GoldenTicket.Apriori
{
    public class ItemsetCollection : List<Itemset>
    {
        #region Methods

        public Itemset GetUniqueItems()
        {
            var unique = new Itemset();

            foreach (Itemset itemset in this)
            {
                unique.AddRange(from item in itemset
                                where !unique.Contains(item)
                                select item);
            }

            return (unique);
        }

        public double FindSupport(string item)
        {
            int matchCount = (from itemset in this
                              where itemset.Contains(item)
                              select itemset).Count();

            double support = (matchCount / (double)Count) * 100.0;
            return (support);
        }

        public double FindSupport(Itemset itemset)
        {
            int matchCount = (from i in this
                              where i.Contains(itemset)
                              select i).Count();

            double support = (matchCount / (double)Count) * 100.0;
            return (support);
        }

        public override string ToString()
        {
            return (string.Join("\r\n", (from itemset in this select itemset.ToString()).ToArray()));
        }

        #endregion
    }
}
