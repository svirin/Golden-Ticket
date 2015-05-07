using System.Collections.Generic;
using System.Linq;
using GoldenTicket.Apriori;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.RuleEngine.AprioriRuler
{
    public class Command : ICommand<UserRecientBlock>
    {
        public void ExecuteCommand(UserRecientBlock item)
        {
            var trainingDB = PerformCollection(item);

            var aprioriResult = AprioriMining.DoApriori(trainingDB, 40.0);

            var rules = AprioriMining.Mine(trainingDB, aprioriResult, 70.0);

            UpdateConcertSuggestions(rules);
        }

        private void UpdateConcertSuggestions(IEnumerable<AssociationRule> rules)
        {
            foreach (var rule in rules)
            {
                if (rule.X.Count == 1)
                {
                    var dataProvider = DI.Factory.GetInstance<IRuleDataProvider<ParseObject>>();

                    var ruleToSave = new Rule
                    {
                        SourceConcertId = string.Join(",", rule.X),
                        Support = rule.Support,
                        Confidence = rule.Confidence,
                        TargetConcertIds = string.Join(",", rule.Y)
                    };

                    var identity = dataProvider.IsRuleExisted(ruleToSave.SourceConcertId);
                    if (!string.IsNullOrEmpty(identity))
                    {
                        ruleToSave.UniqueID = identity;
                        dataProvider.UpdateRates(ruleToSave);
                    }
                    else
                    {
                        dataProvider.Save(ruleToSave);
                    }
                }
            }
        }

        private ItemsetCollection PerformCollection(UserRecientBlock item)
        {
            var trainingDB = new ItemsetCollection();

            foreach (var visitedUser in item.VisitedUsers)
            {
                var itemSet = new Itemset();
                itemSet.AddRange(visitedUser.VisitedConcertsIds);
                trainingDB.Add(itemSet);
            }

            return trainingDB;
        }
    }
}
