using System.Collections.Generic;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Suggestion.Queue;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.DataProxy.Parse;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using GoldenTicket.Suggestion.Test;
using Parse;

namespace GoldenTicket.Suggestion.Service.Registration
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

                Component.For<ICommandFactory<User>>()
                         .ImplementedBy<CommandFactory>(),

                Component.For<IQueueProvider<User>>()
                         .ImplementedBy<QueueProvider>(),

            };

            return registrations;
        }
    }
}
