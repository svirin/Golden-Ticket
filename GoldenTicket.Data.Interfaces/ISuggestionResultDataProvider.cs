using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface ISuggestionResultDataProvider<TRawEntity> : IDataProvider<SuggestionResult, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<SuggestionResult> GetSuggestionResultByUser(User user);
        bool IsExisted(SuggestionResult item);
        void SaveMany(IEnumerable<SuggestionResult> items);
    }
}
