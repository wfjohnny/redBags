using Autofac;
using ISoftSmart.Core.GlobalConfig;
using ISoftSmart.Core.IoC.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.IoC
{
    /// <summary>
    /// 服务定位器
    /// 作者：Johnny
    /// 集成：unity & autofac，统一的容器接口IContainer
    /// </summary>
    public sealed class ServiceLocator : IServiceProvider
    {
        #region Private Fields
        private readonly IContainer _container;
        #endregion

        #region Private Static Fields
        private static readonly ServiceLocator instance = new ServiceLocator();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of ServiceLocator class.
        /// </summary>
        private ServiceLocator()
        {

            #region Autofac注册
            Action<AutofacAdapterContainer, ContainerBuilder> autofacAction = (_container, builder) =>
            {
                // builder.RegisterModule(new Autofac.Configuration.ConfigurationSettingsReader("autofac"));
                //_container.container = builder.Build();
            };
            #endregion

            switch (ConfigManager.Config.IocContaion.IoCType)
            {
                case 0:
                    /*
                     * unity实现比较容易，直接将config的对象装载到容器即可
                     */
                    throw new ArgumentException("不支持此IoC类型");
                case 1:
                    /*
                     * autofac的实现，将先建立ContainerBuilder,然后注册config的对象，最后为它的AutofacAdapterContainer的contaion属性赋值
                     */
                    var builder = new ContainerBuilder();
                    _container = new AutofacAdapterContainer(builder);
                    autofacAction((AutofacAdapterContainer)_container, builder);
                    break;
                default:
                    throw new ArgumentException("不支持此IoC类型");
            }

        }


        #endregion

        #region Public Static Properties
        /// <summary>
        /// Gets the singleton instance of the ServiceLocator class.
        /// </summary>
        public static ServiceLocator Instance
        {
            get { return instance; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            return _container.Resolve<T>();
        }
        /// <summary>
        /// Gets the service instance with the given type by using the overrided arguments.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="overridedArguments">The overrided arguments.</param>
        /// <returns>The service instance.</returns>
        public T GetService<T>(object overridedArguments)
        {
            var overrides = Utils.GetParameter(overridedArguments);
            return _container.Resolve<T>(overrides.ToArray());
        }
        /// <summary>
        /// Gets the service instance with the given type by using the overrided arguments.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="overridedArguments">The overrided arguments.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType, object overridedArguments)
        {
            var overrides = Utils.GetParameter(overridedArguments);
            return _container.Resolve(serviceType, overrides.ToArray());
        }
        #endregion

        #region IServiceProvider Members
        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        #endregion
    }
}