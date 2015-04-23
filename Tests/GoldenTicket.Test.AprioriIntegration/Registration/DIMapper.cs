using System.Collections.Generic;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.DataProxy.Parse;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using GoldenTicket.RuleEngine.AprioriCommander;
using GoldenTicket.RuleEngine.Queue;
using Parse;

namespace GoldenTicket.Test.AprioriIntegration.Registration
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

                Component.For<IConcertDataProvider<ParseObject>>()
                         .ImplementedBy<ConcertDataProvider>(),

                Component.For<IArtistDataProvider<ParseObject>>()
                         .ImplementedBy<ArtistDataProvider>(),

                Component.For<ISuggestDataProvider<ParseObject>>()
                         .ImplementedBy<SuggestDataProvider>(),

                Component.For<IUserDataProvider<ParseObject>>()
                         .ImplementedBy<UserDataProvider>(),

                Component.For<IRequestDataProvider<ParseObject>>()
                         .ImplementedBy<RequestDataProvider>(),

                Component.For<ISettingsDataProvider<ParseObject>>()
                         .ImplementedBy<SettingsDataProvider>(),

                Component.For<ICommandFactory<UserRecientBlock>>()
                         .ImplementedBy<CommandFactory>(),

                Component.For<IQueueProvider<UserRecientBlock>>()
                         .ImplementedBy<QueueProvider>(),

                Component.For<IRecientDataProvider<ParseObject>>()
                         .ImplementedBy<RecientDataProvider>(),

                Component.For<IRuleDataProvider<ParseObject>>()
                         .ImplementedBy<RuleDataProvider>()
            };

            return registrations;
        }
    }
}
