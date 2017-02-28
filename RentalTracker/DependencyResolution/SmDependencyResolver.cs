using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace RentalTracker
{
    /// <summary>
    /// Class representing the StructureMap dependency resolver.
    /// </summary>
    public class SmDependencyResolver : IDependencyResolver {

        private readonly IContainer _container;

        /// <summary>
        /// Constructor of the dependency resolver.
        /// </summary>
        /// <param name="container"></param>
        public SmDependencyResolver(IContainer container) {
            _container = container;
        }

        /// <summary>
        /// Gets a service from the IoC container
        /// </summary>
        /// <param name="serviceType">The type (interface) of the service.</param>
        /// <returns>The instance of the implementation of the service.</returns>
        public object GetService(Type serviceType) {
            if (serviceType == null) return null;
            try {
                  return serviceType.IsAbstract || serviceType.IsInterface
                           ? _container.TryGetInstance(serviceType)
                           : _container.GetInstance(serviceType);
            }
            catch {

                return null;
            }
        }

        /// <summary>
        /// Gets a collection of implementations of a service.
        /// </summary>
        /// <param name="serviceType">The type (interface) of the service.</param>
        /// <returns>A list of all implementors of this service.</returns>
        public IEnumerable<object> GetServices(Type serviceType) {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}