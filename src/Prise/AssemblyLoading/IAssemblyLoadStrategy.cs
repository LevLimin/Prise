using System;
using System.Reflection;

namespace Prise.AssemblyLoading
{
    public interface IAssemblyLoadStrategy
    {
        /// <summary>
        /// Loads a dependency assembly for the current plugin
        /// </summary>
        /// <param name="initialPluginLoadDirectory">Directory where the plugin was initially loaded from</param>
        /// <param name="assemblyName"></param>
        /// <param name="pluginDependencyContext"></param>
        /// <param name="loadFromDependencyContext"></param>
        /// <param name="loadFromRemote"></param>
        /// <param name="loadFromAppDomain"></param>
        /// <returns>A loaded assembly</returns>
        AssemblyFromStrategy LoadAssembly(
            string initialPluginLoadDirectory, 
            AssemblyName assemblyName,
            IPluginDependencyContext pluginDependencyContext,
            Func<string, AssemblyName, ValueOrProceed<AssemblyFromStrategy>> loadFromDependencyContext,
            Func<string, AssemblyName, ValueOrProceed<AssemblyFromStrategy>> loadFromRemote,
            Func<string, AssemblyName, ValueOrProceed<RuntimeAssemblyShim>> loadFromAppDomain);

        /// <summary>
        /// Loads a native assembly
        /// </summary>
        /// <param name="initialPluginLoadDirectory">Directory where the plugin was initially loaded from</param>
        /// <param name="unmanagedDllName"></param>
        /// <param name="pluginDependencyContext"></param>
        /// <param name="loadFromDependencyContext"></param>
        /// <param name="loadFromRemote"></param>
        /// <param name="loadFromAppDomain"></param>
        /// <returns>The path to a native assembly</returns>
        NativeAssembly LoadUnmanagedDll(
            string initialPluginLoadDirectory, 
            string unmanagedDllName,
            IPluginDependencyContext pluginDependencyContext,
            Func<string, string, ValueOrProceed<string>> loadFromDependencyContext,
            Func<string, string, ValueOrProceed<string>> loadFromRemote,
            Func<string, string, ValueOrProceed<IntPtr>> loadFromAppDomain);
    }
}