using StructureMap;

namespace RentalTracker {

    /// <summary>
    /// Class representing the StructureMap Inversion of Control container.
    /// </summary>
    public static class IoC {
        /// <summary>
        /// Initialises the IoC container by scanning the all assemblies and 
        /// looking for interface classes with corresponding implemtation classes.
        /// </summary>
        /// <returns></returns>
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.AssembliesFromApplicationBaseDirectory();
                                        scan.WithDefaultConventions();
                                    });
            //                x.For<IExample>().Use<Example>();
                        });
            return ObjectFactory.Container;
        }
    }
}