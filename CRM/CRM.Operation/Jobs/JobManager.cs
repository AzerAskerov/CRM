using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace CRM.Operation.Jobs
{
    public class JobManager
    {
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;
        public JobManager(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        [DisableConcurrentExecution(0)]
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public void Run(string configName)
        {
            var settings = _configuration.GetSection($"jobconfig:{configName}:service").GetChildren();

            string disabled = settings.FirstOrDefault(x => x.Key == "@disabled")?.Value;

            if (disabled == "no")
            {
                string assembly = settings.FirstOrDefault(x => x.Key == "@assembly").Value;
                string type = settings.FirstOrDefault(x => x.Key == "@type").Value;

                string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(String.Format("{0}\\{1}", AssemblyPath, assembly));
                var myType = myAssembly.GetType(type);
                var myInstance = (AbstractJob)Activator.CreateInstance(myType, _serviceProvider);

                myInstance.Run(settings);
            }
        }
    }
}
