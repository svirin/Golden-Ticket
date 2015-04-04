using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;

namespace GoldenTicket.DI
{
    /// <summary>
    /// Class load registrations
    /// </summary>
    public static class Registrations
    {
        /// <summary>
        /// Load registrations from DIMapper class of current running process
        /// </summary>
        public static List<IRegistration> LoadRepository()
        {
            // Declare registrations collection
            var registrations = new List<IRegistration>();

            // Get current domain assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Iterate by each assembly
            foreach (var asm in assemblies)
            {
                // Get all assemblies wich cintains DIMapper class
                var containsMapperType = asm.GetTypes().Where(type => type.Name.EndsWith("DIMapper"));

                // Iterate by all founded types 
                foreach (var foundedType in containsMapperType)
                {
                    // Find register method 
                    var registerMethod = foundedType.GetMethod("Registrations", BindingFlags.Public | BindingFlags.Static);

                    // Check that method existed
                    if (registerMethod == null)

                        // Throw exception
                        throw new Exception("Static method Registrations not found in Mapper class.");

                    // Invoke method and collect all registrations
                    var foundedRegistrations = (IEnumerable<IRegistration>)registerMethod.Invoke(null, null);

                    // Add founded registrations to common
                    registrations.AddRange(foundedRegistrations);
                }
            }

            // If mapper file has no contains mappers
            if (!registrations.Any())

                // Throw critical exception
                throw new Exception("Mapper of DI not found. Please check that executive assembly contains Mapper class with mappers");

            // Return result
            return registrations;
        }

    }
}
