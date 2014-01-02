[assembly: WebActivator.PreApplicationStartMethod(typeof(Logistics.Portal.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Logistics.Portal.App_Start.NinjectWebCommon), "Stop")]
namespace Logistics.Portal.App_Start {
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Logistics.EFRepository.Impl;
    using Ninject.Syntax;
    using Logistics.Domain.Repository;

    public static class NinjectWebCommon {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop() {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel() {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel) {
            //<ISystemRep>.SystemService
            kernel.Bind<ISystemRep>().To<SystemRep>();
            kernel.Bind<IButtonRep>().To<ButtonRep>();
            kernel.Bind<IMenuRep>().To<MenuRep>();
            kernel.Bind<ICustomerRep>().To<CustomerRep>();
            kernel.Bind<IGroupRep>().To<GroupRep>();
            kernel.Bind<IStockRep>().To<StockRep>();
            kernel.Bind<IStockDetailRep>().To<StockDetailRep>();
            kernel.Bind<IMemoryCardRep>().To<MemorycardRep>();
            kernel.Bind<IStoreRep>().To<StoreRep>();
            kernel.Bind<IJobRep>().To<JobRep>();
            kernel.Bind<IBillRep>().To<BillRep>();
            kernel.Bind<IVenueStaffRep>().To<VenueStaffRep>();
            kernel.Bind<IRoleRep>().To<RoleRep>();
            kernel.Bind<ILoginUserRep>().To<LoginUserRep>();
        }
    }
}