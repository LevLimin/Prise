using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Prise.AssemblyLoading
{
    public class DefaultAssemblyLoadStrategy : IAssemblyLoadStrategy
    {
        public virtual AssemblyFromStrategy LoadAssembly(
            string initialPluginLoadDirectory,
            AssemblyName assemblyName,
            IPluginDependencyContext pluginDependencyContext,
            Func<string, AssemblyName, ValueOrProceed<AssemblyFromStrategy>> loadFromDependencyContext,
            Func<string, AssemblyName, ValueOrProceed<AssemblyFromStrategy>> loadFromRemote,
            Func<string, AssemblyName, ValueOrProceed<RuntimeAssemblyShim>> loadFromAppDomain)
        {
            if (assemblyName.Name == null) return null;
            Debug.WriteLine($"{initialPluginLoadDirectory} Loading {assemblyName.Name} {assemblyName.Version}");
            ValueOrProceed<AssemblyFromStrategy> valueOrProceed = ValueOrProceed<AssemblyFromStrategy>.FromValue(null, true);

            var isHostAssembly = IsHostAssembly(assemblyName, pluginDependencyContext);
            var isRemoteAssembly = IsRemoteAssembly(assemblyName, pluginDependencyContext);

            if (isHostAssembly && !isRemoteAssembly) // Load from Default App Domain (host)
            {
                Debug.WriteLine($"{initialPluginLoadDirectory} Loading {assemblyName.Name} {assemblyName.Version} from appDomain");
                var assemblyShim = loadFromAppDomain(initialPluginLoadDirectory, assemblyName);
                if (assemblyShim.Value != null)
                    switch (assemblyShim.Value.RuntimeLoadFlag)
                    {
                        case RuntimeLoadFlag.FromRequestedVersion:
                            return null; // fallback to default loading mechanism
                        case RuntimeLoadFlag.FromRuntimeVersion:
                            return AssemblyFromStrategy.NotReleasable(assemblyShim.Value.Assembly);
                    }
            }

            if (valueOrProceed.CanProceed)
                valueOrProceed = loadFromDependencyContext(initialPluginLoadDirectory, assemblyName);
            Debug.WriteLineIf(!valueOrProceed.CanProceed, $"{initialPluginLoadDirectory} Loaded {assemblyName.Name} {assemblyName.Version} from dependency context");

            if (valueOrProceed.CanProceed)
            {
                valueOrProceed = loadFromRemote(initialPluginLoadDirectory, assemblyName);
                Debug.WriteLineIf(!valueOrProceed.CanProceed, $"{initialPluginLoadDirectory} Loaded {assemblyName.Name} {assemblyName.Version} from remote");
            }

            return valueOrProceed.Value;
        }

        public virtual NativeAssembly LoadUnmanagedDll(
            string initialPluginLoadDirectory,
            string unmanagedDllName,
            IPluginDependencyContext pluginDependencyContext,
            Func<string, string, ValueOrProceed<string>> loadFromDependencyContext,
            Func<string, string, ValueOrProceed<string>> loadFromRemote,
            Func<string, string, ValueOrProceed<IntPtr>> loadFromAppDomain)
        {
            ValueOrProceed<string> valueOrProceed = ValueOrProceed<string>.FromValue(String.Empty, true);
            ValueOrProceed<IntPtr> ptrValueOrProceed = ValueOrProceed<IntPtr>.FromValue(IntPtr.Zero, true);

            valueOrProceed = loadFromDependencyContext(initialPluginLoadDirectory, unmanagedDllName);

            if (valueOrProceed.CanProceed)
                ptrValueOrProceed = loadFromAppDomain(initialPluginLoadDirectory, unmanagedDllName);

            if (valueOrProceed.CanProceed && ptrValueOrProceed.CanProceed)
                valueOrProceed = loadFromRemote(initialPluginLoadDirectory, unmanagedDllName);

            return NativeAssembly.Create(valueOrProceed.Value, ptrValueOrProceed.Value);
        }

        protected virtual bool IsHostAssembly(AssemblyName assemblyName, IPluginDependencyContext pluginDependencyContext) =>
            pluginDependencyContext.HostDependencies.Any(h =>
                h.DependencyName.Name == assemblyName.Name
            );

        protected virtual bool IsRemoteAssembly(AssemblyName assemblyName, IPluginDependencyContext pluginDependencyContext) =>
            pluginDependencyContext.RemoteDependencies.Any(r =>
                r.DependencyName.Name == assemblyName.Name
            );
    }
}