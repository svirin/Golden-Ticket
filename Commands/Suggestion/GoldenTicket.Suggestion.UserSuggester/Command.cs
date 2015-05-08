using System;
using GoldenTicket.Utilities;
using System.Collections.Generic;
using System.Linq;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.Suggestion.UserSuggester
{
    public class Command : ICommand<User>
    {
        public void ExecuteCommand(User item)
        {
            var recients = GetRecientsOfUser(item.Username);

            var rules = GetRulesByRecients(recients);

            if (rules.Count() > 0)
            {
                var oldSuggests = GetOldSuggestedConcerts(item.Username).ToList();

                var newSuggests = ConvertRulesToSuggests(rules, item.Username).ToList();

                RemoveDuplicated(oldSuggests, newSuggests);

                SaveUserSuggests(newSuggests);

                //DeleteOldSuggests(oldSuggests);
            }
        }

        private void RemoveDuplicated(List<Suggest> oldSuggests, List<Suggest> newSuggests)
        {
            var newOldSuggests = new List<Suggest>();

            foreach (var oldSuggest in oldSuggests)
            {
                if (newSuggests.Any(suggest => suggest.ConcertId.Equals(oldSuggest.ConcertId)))
                {
                    newSuggests.Remove(newSuggests.First(suggest => suggest.ConcertId.Equals(oldSuggest.ConcertId)));
                }
                else
                {
                    newOldSuggests.Add(oldSuggest);
                }
            }

            oldSuggests = newOldSuggests;
        }

        private void DeleteOldSuggests(IEnumerable<Suggest> oldSuggests)
        {
            var dataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            dataProvider.DeleteMany(oldSuggests);
        }

        private void SaveUserSuggests(IEnumerable<Suggest> newSuggests)
        {
            var dataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            dataProvider.SaveMany(newSuggests);
        }

        private IEnumerable<Suggest> ConvertRulesToSuggests(IEnumerable<Rule> rules, string username) 
        {
            var rulesTargetIdents = rules.Select(item => item.TargetConcertIds);
            var rulesTargetIdentsAsOne = string.Join(",", rulesTargetIdents);
            var concertIds = rulesTargetIdentsAsOne.Split(new[] { "," }, StringSplitOptions.None);

            var suggests = concertIds.Select(concertId =>
                new Suggest
                {
                    ConcertId = concertId,
                    Username = username

                }).ToList();

            return suggests.DistinctBy(suggest => suggest.ConcertId);
        }

        private IEnumerable<Suggest> GetOldSuggestedConcerts(string username)
        {
            var dataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            var suggestConcerts = dataProvider.GetSuggestByUser(username).ToList();

            return suggestConcerts;
        }

        private IEnumerable<Recient> GetRecientsOfUser(string username)
        {
            var recientProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();

            var recients = recientProvider.GetRecientItems(username).ToList();

            return recients;
        }

        private IEnumerable<Rule> GetRulesByRecients(IEnumerable<Recient> recients)
        {
            var recientIdents = recients.Select(item => item.ConcertId);

            var rulesProvider = DI.Factory.GetInstance<IRuleDataProvider<ParseObject>>();

            var rules = rulesProvider.GetRulesBySources(recientIdents).ToList();

            return rules;
        }
    }
}
