using System;
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

            var oldSuggests = GetOldSuggestedConcerts(item.Username);

            SaveUserSuggests(rules, item.Username);

            DeleteOldSuggests(oldSuggests);
        }

        private void DeleteOldSuggests(IEnumerable<Suggest> oldSuggests)
        {
            var dataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            dataProvider.DeleteMany(oldSuggests);
        }

        private void SaveUserSuggests(IEnumerable<Rule> rules, string username)
        {
            var dataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            var rulesTargetIdents = rules.Select(item => item.TargetConcertIds);
            var rulesTargetIdentsAsOne = string.Join(",", rulesTargetIdents);
            var concertIds = rulesTargetIdentsAsOne.Split(new[] { "," }, StringSplitOptions.None);

            var suggests = concertIds.Select(concertId =>
                new Suggest
                {
                    ConcertId = concertId, 
                    Username = username

                }).ToList();

            dataProvider.SaveMany(suggests);
        }

        private IEnumerable<Suggest> GetOldSuggestedConcerts(string username)
        {
            var dataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            var suggestConcerts = dataProvider.GetSuggestByUser(username);

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
