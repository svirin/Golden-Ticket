//Apriori.cs

using System.Collections.Generic;
using System.Linq;

namespace GoldenTicket.Apriori
{
    public class AprioriMining
    {
        public static ItemsetCollection DoApriori(ItemsetCollection db, double supportThreshold)
        {
            var I = db.GetUniqueItems();
            var l = new ItemsetCollection(); //resultant large itemsets
            var li = new ItemsetCollection(); //large itemset in each iteration
            var ci = new ItemsetCollection(); //pruned itemset in each iteration
            ci.AddRange(I.Select(item => new Itemset {item}));

            //first iteration (1-item itemsets)
            //next iterations
            var k = 2;
            while (ci.Count != 0)
            {
                //set Li from Ci (pruning)
                li.Clear();
                foreach (var itemset in ci)
                {
                    itemset.Support = db.FindSupport(itemset);
                    if (itemset.Support >= supportThreshold)
                    {
                        li.Add(itemset);
                        l.Add(itemset);
                    }
                }

                //set Ci for next iteration (find supersets of Li)
                ci.Clear();
                ci.AddRange(Bit.FindSubsets(li.GetUniqueItems(), k)); //get k-item subsets
                k += 1;
            }

            return (l);
        }

        public static List<AssociationRule> Mine(ItemsetCollection db, ItemsetCollection l, double confidenceThreshold)
        {
            var allRules = new List<AssociationRule>();

            foreach (Itemset itemset in l)
            {
                var subsets = Bit.FindSubsets(itemset, 0); //get all subsets
                foreach (var subset in subsets)
                {
                    var confidence = (db.FindSupport(itemset) / db.FindSupport(subset)) * 100.0;
                    if (confidence >= confidenceThreshold)
                    {
                        var rule = new AssociationRule();
                        rule.X.AddRange(subset);
                        rule.Y.AddRange(itemset.Remove(subset));
                        rule.Support = db.FindSupport(itemset);
                        rule.Confidence = confidence;
                        if (rule.X.Count > 0 && rule.Y.Count > 0)
                        {
                            allRules.Add(rule);
                        }
                    }
                }
            }

            return (allRules);
        }
    }
}
