using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace GoldenTicket.DI
{
    /// <summary>
    /// Factory for dependency injection using
    /// </summary>
    public static class Factory
    {
        // Declare static container
        private static readonly WindsorContainer Container;
       
        /// <summary>
        /// C-tor
        /// Occurs in first using if the class
        /// </summary>
        static Factory()
        {
            // Initialize container
            Container = new WindsorContainer();

            // Load registrations
            var registrations = Registrations.LoadRepository();

            // Execute mapping
            RegisterDependencies(registrations);
        }

        /// <summary>
        /// Map all dependencies witch can find in executing directory
        /// </summary>
        private static void RegisterDependencies(IEnumerable<IRegistration> registrations)
        {
            // Iterate by registrations 
            foreach (var item in registrations)
            {
                // Register in castle container
                Container.Register(item);
            }
        }
        
        /// <summary>
        /// Method returns object registered in container
        /// </summary>
        public static TEntityType GetInstance<TEntityType>()
             where TEntityType : class
        {
            // Resolve container
            var result = Container.Resolve<TEntityType>();

            // Return instance
            return result;
        }
    }
}