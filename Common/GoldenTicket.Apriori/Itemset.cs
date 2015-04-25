//Itemset.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenTicket.Apriori
{
    public class Itemset : List<string>
    {
        #region Properties

        public double Support { get; set; }

        #endregion

        #region Methods

        public bool Contains(Itemset itemset)
        {
            return (this.Intersect(itemset).Count() == itemset.Count);
        }

        public Itemset Remove(Itemset itemset)
        {
            var removed = new Itemset();
            removed.AddRange(from item in this
                             where !itemset.Contains(item)
                             select item);
            return (removed);
        }

        public override string ToString()
        {
            return ("{" + string.Join(", ", ToArray()) + "}" + (Support > 0 ? " (support: " + Math.Round(Support, 2) + "%)" : string.Empty));
        }

        #endregion
    }
}
