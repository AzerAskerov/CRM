using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Xml;



namespace CRM.Operation.Jobs
{
    public abstract class AbstractJob
    {
        /// <summary>
        /// Display name of the service. This is set by service manager before calling start.
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// Display description of the service. This is set by service manager before calling start.
        /// </summary>
        public string Description;

        /// <summary>
        /// Is called when service manager is started.
        /// </summary>
        public abstract void Start (IEnumerable<IConfigurationSection> config, DbContext dbContext);


        /// <summary>
        /// Is called when service should run.
        /// If false is returned or exception is thrown,
        /// manager will atempt to run service again if configured accordingly.
        /// </summary>
        /// <param name="isTestMode">Defines wether service is being run in test mode
        /// by default should be false, true if run from ServiceTest form</param>
        public abstract bool DoRun(bool isTestMode, IEnumerable<IConfigurationSection> config);

        /// <summary>
        /// Is called when service manager is stopped.
        /// </summary>
        public virtual void Stop()
        {
        }

        public virtual void Run(IEnumerable<IConfigurationSection> config)
        {
            DoRun(false, config);
        }

        protected T? TryGetConfigurationParam<T>(XmlNode config, String paramName) where T : struct
        {
            try
            {
                return GetConfigurationParam<T>(config, paramName);
            }
            catch (ArgumentException e)
            {
                if (!(e.InnerException is InvalidCastException))
                    return null;
                
                throw;
            }
        }

        protected T GetConfigurationParam<T>(XmlNode config, String paramName)
        {
            var node = config.SelectSingleNode(paramName);
            if (node == null)
            {
                throw new ArgumentException(String.Format("Configuration parameter {0} not found.", paramName));
            }
            try
            {
                if (typeof(T) == typeof(Guid))
                    return (T)(object)new Guid(node.InnerText);

                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), node.InnerText);

                return (T)Convert.ChangeType(node.InnerText, typeof(T));
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException(String.Format("Configuration parameter {0} is not convertable to {1}", paramName, typeof(T).Name), e);
            }
        }

       
    }
}