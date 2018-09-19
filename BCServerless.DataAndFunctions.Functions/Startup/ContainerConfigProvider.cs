using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host.Config;
using SimpleInjector;
using System;

namespace BCServerlessDemo.DataAndFunctions.Functions.Startup
{
    public class ContainerConfigProvider: IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var container = ContainerRegistrar.GetContainer();
            
            context.AddBindingRule<InjectAttribute>().BindToInput<dynamic>(i => container.GetInstance(i.Type));        
        }      
    }
}
