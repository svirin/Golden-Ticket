using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface ISearchResultDataProvider<TRawEntity> : IDataProvider<SearchResult, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<SearchResult> SearchResultByUser(User user);
        void SaveMany(IEnumerable<SearchResult> items);
        void Save(SearchResult item);
        bool IsExisted(SearchResult item);
    }
}
