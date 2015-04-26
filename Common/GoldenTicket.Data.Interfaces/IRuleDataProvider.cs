using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface IRuleDataProvider<TRawEntity> : IDataProvider<Rule, TRawEntity>
         where TRawEntity : class
    {
        Rule GetRuleBySource(string concertId);
        IEnumerable<Rule> GetRulesBySources(IEnumerable<string> sourcesIds);
        bool IsRuleExisted(string sourceConcert);
        void Save(Rule item);
        void UpdateRates(Rule item);
        void Delete(Rule item);
        void DeleteMany(IEnumerable<Rule> items);
    }
}
