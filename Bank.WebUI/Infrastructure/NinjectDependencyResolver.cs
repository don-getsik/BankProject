using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Bank.Domain.Abstract;
using Bank.Domain.Concrete;
using Ninject;

namespace Bank.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBlindigs();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBlindigs()
        {
            _kernel.Bind<ITransctionsRepository>().To<EFTransactionsRepositiory>();
        }
    }
}