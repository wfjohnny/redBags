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
    /// IoCFactory  implementation 
    /// </summary>
    public sealed class IoCFactory
    {
        #region Singleton
        static IoCFactory instance;
        static object lockObj = new object();
        /// <summary>
        /// Get singleton instance of IoCFactory
        /// </summary>
        public static IoCFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new IoCFactory();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Members

        IContainer _CurrentContainer;

        /// <summary>
        /// Get current configured IContainer
        /// </summary>
        public IContainer CurrentContainer
        {
            get
            {
                return _CurrentContainer;
            }
        }

        #endregion

        #region Constructor

        private IoCFactory()
        {
            switch (ConfigManager.Config.IocContaion.IoCType)
            {
                case 0:
                    throw new ArgumentException("不支持此IoC类型");
                case 1:
                    _CurrentContainer = new AutofacAdapterContainer();
                    break;
                default:
                    throw new ArgumentException("不支持此IoC类型");
            }
        }
        #endregion
    }
}