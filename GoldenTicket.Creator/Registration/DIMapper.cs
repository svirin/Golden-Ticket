using System.Collections.Generic;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Crawler.Queue;
using GoldenTicket.Crawler.Test;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.DataProxy.Parse;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;

namespace GoldenTicket.Creator.Registration
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

                Component.For<IRequestDataProvider<ParseObject>>()
                         .ImplementedBy<RequestDataProvider>(),

                Component.For<ICommandFactory<Artist>>()
                         .ImplementedBy<CommandFactory>(),

                Component.For<IQueueProvider<Artist>>()
                         .ImplementedBy<QueueProvider>(),

                Component.For<ISettingsDataProvider<ParseObject>>()
                         .ImplementedBy<SettingsDataProvider>(),

            };

            return registrations;
        }
    }
}
