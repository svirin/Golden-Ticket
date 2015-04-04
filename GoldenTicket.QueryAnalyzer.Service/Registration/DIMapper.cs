using System.Collections.Generic;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.DataProxy.Parse;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;
using GoldenTicket.QueryAnalyzer.ArtistCopier;
using GoldenTicket.QueryAnalyzer.Queue;

namespace GoldenTicket.QueryAnalyzer.Service
{
    public static class DIMapper
    {
        public static IEnumerable<IRegistration> Registrations()
        {
            var registrations = new List<IRegistration>
            {
                Component.For<IInterceptor>()
                         .ImplementedBy<Interceptor.Interceptor>()
                         .Named("GoldenTicketLogger"),

                Component.For<ISearchResultDataProvider<ParseObject>>()
                         .ImplementedBy<SearchResultDataProvider>(),

                Component.For<IArtistDataProvider<ParseObject>>()
                         .ImplementedBy<ArtistDataProvider>(),

                Component.For<ISuggestionResultDataProvider<ParseObject>>()
                         .ImplementedBy<SuggestionResultDataProvider>(),

                Component.For<IUserDataProvider<ParseObject>>()
                         .ImplementedBy<UserDataProvider>(),

                Component.For<IUserRequestDataProvider<ParseObject>>()
                         .ImplementedBy<UserRequestDataProvider>(),

                Component.For<ICommandFactory<UserRequest>>()
                         .ImplementedBy<CommandFactory>(),

                Component.For<IQueueProvider<UserRequest>>()
                         .ImplementedBy<QueueProvider>(),

            };

            return registrations;
        }
    }
}
