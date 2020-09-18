﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Prise.AssemblyScanning;
using Prise.Infrastructure;
using Prise.Providers;
using Prise.Proxy;

namespace Prise
{
    public class PluginLoadOptionsBuilder<T> : IDisposable
    {
        protected bool disposed = false;

        internal IPluginLogger<T> logger;
        internal Type loggerType;
        internal ServiceLifetime priseServiceLifetime;
        internal CacheOptions<IPluginCache<T>> cacheOptions;
        internal IAssemblyScanner<T> assemblyScanner;
        internal Type assemblyScannerType;
        internal IAssemblyScannerOptions<T> assemblyScannerOptions;
        internal Type assemblyScannerOptionsType;
        internal IPluginPathProvider<T> pluginPathProvider;
        internal Type pluginPathProviderType;
        internal IAssemblyLoadStrategyProvider assemblyLoadStrategyProvider;
        internal Type assemblyLoadStrategyProviderType = typeof(DefaultAssemblyLoadStrategyProvider);
        internal ISharedServicesProvider<T> sharedServicesProvider;
        internal Type sharedServicesProviderType;
        internal IRemotePluginActivator<T> activator;
        internal IPluginActivationContextProvider<T> pluginActivationContextProvider;
        internal Type pluginActivationContextProviderType;
        internal IPluginTypesProvider<T> pluginTypesProvider;
        internal Type pluginTypesProviderType;
        internal Type activatorType;
        internal IPluginProxyCreator<T> proxyCreator;
        internal Type proxyCreatorType;
        internal IResultConverter resultConverter;
        internal Type resultConverterType;
        internal IParameterConverter parameterConverter;
        internal Type parameterConverterType;
        internal IPluginAssemblyLoader<T> assemblyLoader;
        internal Type assemblyLoaderType;
        internal IPluginAssemblyNameProvider<T> pluginAssemblyNameProvider;
        internal Type pluginAssemblyNameProviderType;
        internal IAssemblyLoadOptions<T> assemblyLoadOptions;
        internal Type assemblyLoadOptionsType;
        internal INetworkAssemblyLoaderOptions<T> networkAssemblyLoaderOptions;
        internal Type networkAssemblyLoaderOptionsType;
        internal Action<IServiceCollection> configureServices;
        internal IHostTypesProvider<T> hostTypesProvider;
        internal Type hostTypesProviderType;
        internal IDowngradableDependenciesProvider<T> downgradableDependenciesProvider;
        internal Type downgradableDependenciesProviderType;
        internal IRemoteTypesProvider<T> remoteTypesProvider;
        internal Type remoteTypesProviderType;
        internal IDependencyPathProvider<T> dependencyPathProvider;
        internal Type dependencyPathProviderType;
        internal IProbingPathsProvider<T> probingPathsProvider;
        internal Type probingPathsProviderType;
        internal IRuntimePlatformContext runtimePlatformContext;
        internal Type runtimePlatformContextType;
        internal IAssemblySelector<T> assemblySelector;
        internal Type assemblySelectorType;
        internal IPluginSelector<T> pluginSelector;
        internal Type pluginSelectorType;
        internal IDepsFileProvider<T> depsFileProvider;
        internal Type depsFileProviderType;
        internal IPluginDependencyResolver<T> pluginDependencyResolver;
        internal Type pluginDependencyResolverType;
        internal ITempPathProvider<T> tempPathProvider;
        internal Type tempPathProviderType;
        internal INativeAssemblyUnloader nativeAssemblyUnloader;
        internal Type nativeAssemblyUnloaderType;
        internal IHostFrameworkProvider hostFrameworkProvider;
        internal Type hostFrameworkProviderType;

        internal PluginLoadOptionsBuilder()
        {
        }

        public PluginLoadOptionsBuilder<T> WithLogger(IPluginLogger<T> logger)
        {
            this.logger = logger;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithLoggerType<TType>()
            where TType : IPluginLogger<T>
        {
            this.loggerType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginPath(string path)
        {
            this.pluginPathProvider = new DefaultPluginPathProvider<T>(path);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPriseServiceLifetime(ServiceLifetime serviceLifetime)
        {
            this.priseServiceLifetime = serviceLifetime;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginPathProvider<TType>()
            where TType : IPluginPathProvider<T>
        {
            this.pluginPathProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSingletonCache()
        {
            this.cacheOptions = CacheOptions<IPluginCache<T>>.SingletonPluginCache();
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithCachingOptions(ServiceLifetime serviceLifetime)
        {
            this.cacheOptions = new CacheOptions<IPluginCache<T>>(serviceLifetime);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoadStrategyProvider(IAssemblyLoadStrategyProvider provider)
        {
            this.assemblyLoadStrategyProvider = provider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoadStrategyProvider<TType>()
            where TType : IAssemblyLoadStrategyProvider
        {
            this.assemblyLoadStrategyProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProxyCreator(IPluginProxyCreator<T> proxyCreator)
        {
            this.proxyCreator = proxyCreator;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProxyCreator<TType>()
            where TType : IPluginProxyCreator<T>
        {
            this.proxyCreatorType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginAssemblyName(string pluginAssemblyName)
        {
            this.pluginAssemblyNameProvider = new PluginAssemblyNameProvider<T>(pluginAssemblyName);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithPluginAssemblyNameProvider<TType>()
                   where TType : IPluginAssemblyNameProvider<T>
        {
            this.pluginAssemblyNameProviderType = typeof(TType);
            return this;
        }

        private bool useCollectibleAssemblies = true;
        // TODO PORT OVER
        public PluginLoadOptionsBuilder<T> UseCollectibleAssemblies(bool useCollectibleAssemblies)
        {
            this.useCollectibleAssemblies = useCollectibleAssemblies;
            this.assemblyLoadOptions = new DefaultAssemblyLoadOptions<T>(
                PluginPlatformVersion.Empty(),
                this.ignorePlatformInconsistencies,
                this.useCollectibleAssemblies,
                NativeDependencyLoadPreference.PreferInstalledRuntime);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithActivator(IRemotePluginActivator<T> activator)
        {
            this.activator = activator;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithActivator<TType>()
            where TType : IRemotePluginActivator<T>
        {
            this.activatorType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithParameterConverter(IParameterConverter parameterConverter)
        {
            this.parameterConverter = parameterConverter;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithParameterConverter<TType>()
            where TType : IParameterConverter
        {
            this.parameterConverterType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithResultConverter(IResultConverter resultConverter)
        {
            this.resultConverter = resultConverter;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithResultConverter<TType>()
            where TType : IResultConverter
        {
            this.resultConverterType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoader(IPluginAssemblyLoader<T> assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblyLoader<TType>()
           where TType : IPluginAssemblyLoader<T>
        {
            this.assemblyLoaderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithLocalDiskAssemblyLoader(
            PluginPlatformVersion pluginPlatformVersion = null,
            bool useCollectibleAssemblies = true,
            NativeDependencyLoadPreference nativeDependencyLoadPreference = NativeDependencyLoadPreference.PreferInstalledRuntime
            )
        {
            if (pluginPlatformVersion == null)
                pluginPlatformVersion = PluginPlatformVersion.Empty();

            this.assemblyLoadOptions = new DefaultAssemblyLoadOptions<T>(
                pluginPlatformVersion,
                this.ignorePlatformInconsistencies,
                useCollectibleAssemblies,
                nativeDependencyLoadPreference
            );

#if NETCORE3_0 || NETCORE3_1
            return this.WithAssemblyLoader<DefaultAssemblyLoaderWithNativeResolver<T>>();
#endif
#if NETCORE2_1
            return this.WithAssemblyLoader<DefaultAssemblyLoader<T>>();
#endif
        }

        public PluginLoadOptionsBuilder<T> WithLocalDiskAssemblyLoader<TType>()
            where TType : IAssemblyLoadOptions<T>
        {
            this.assemblyLoadOptionsType = typeof(TType);

#if NETCORE3_0 || NETCORE3_1
            return this.WithAssemblyLoader<DefaultAssemblyLoaderWithNativeResolver<T>>();
#endif
#if NETCORE2_1
            return this.WithAssemblyLoader<DefaultAssemblyLoader<T>>();
#endif
        }

        public PluginLoadOptionsBuilder<T> WithNetworkAssemblyLoader(
            string baseUrl,
            PluginPlatformVersion pluginPlatformVersion = null,
            NativeDependencyLoadPreference nativeDependencyLoadPreference = NativeDependencyLoadPreference.PreferInstalledRuntime
        )
        {
            if (pluginPlatformVersion == null)
                pluginPlatformVersion = PluginPlatformVersion.Empty();

            this.networkAssemblyLoaderOptions = new DefaultNetworkAssemblyLoaderOptions<T>(
                baseUrl,
                pluginPlatformVersion,
                false,
                nativeDependencyLoadPreference
            );

            this.depsFileProviderType = typeof(NetworkDepsFileProvider<T>);
            this.pluginDependencyResolverType = typeof(NetworkPluginDependencyResolver<T>);
            this.assemblyLoaderType = typeof(NetworkAssemblyLoader<T>);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithNetworkAssemblyLoader<TType>()
            where TType : INetworkAssemblyLoaderOptions<T>
        {
            this.networkAssemblyLoaderOptionsType = typeof(TType);
            this.depsFileProviderType = typeof(NetworkDepsFileProvider<T>);
            this.pluginDependencyResolverType = typeof(NetworkPluginDependencyResolver<T>);
            this.assemblyLoaderType = typeof(NetworkAssemblyLoader<T>);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithNetworkTempPathProvider<TType>()
           where TType : ITempPathProvider<T>
        {
            this.tempPathProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithNetworkTempPathProvider(ITempPathProvider<T> tempPathProvider)
        {
            this.tempPathProvider = tempPathProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSharedServicesProvider<TType>()
           where TType : ISharedServicesProvider<T>
        {
            this.sharedServicesProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> ConfigureServices(Action<IServiceCollection> configureServices)
        {
            this.configureServices = configureServices;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostTypeProvider<TType>()
            where TType : IHostTypesProvider<T>
        {
            this.hostTypesProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostTypeProvider(IHostTypesProvider<T> hostTypesProvider)
        {
            this.hostTypesProvider = hostTypesProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithRemoteTypesProvider<TType>()
            where TType : IRemoteTypesProvider<T>
        {
            this.remoteTypesProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithRemoteTypesProvider(IRemoteTypesProvider<T> remoteTypesProvider)
        {
            this.remoteTypesProvider = remoteTypesProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblySelector(Func<IEnumerable<AssemblyScanResult<T>>, IEnumerable<AssemblyScanResult<T>>> assemblySelector)
        {
            this.assemblySelector = new DefaultAssemblySelector<T>(assemblySelector);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAssemblySelector<TType>()
            where TType : IAssemblySelector<T>
        {
            this.assemblySelectorType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSelector(Func<IEnumerable<Type>, IEnumerable<Type>> pluginSelector)
        {
            this.pluginSelector = new DefaultPluginSelector<T>(pluginSelector);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithSelector<TType>()
            where TType : IPluginSelector<T>
        {
            this.pluginSelectorType = typeof(TType);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> AllowDowngradeForType<TType>()
        {
            var downgradableDependenciesProvider = this.downgradableDependenciesProvider as DowngradableDependenciesProvider<T>;
            if (downgradableDependenciesProvider == null)
                throw new PrisePluginException($"You're not using the default IDowngradableDependenciesProvider {nameof(DowngradableDependenciesProvider<T>)}. Please add downgradable types using your own provider.");
            downgradableDependenciesProvider.AddDowngradableType<TType>();
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> AllowDowngradeForType(Type type)
        {
            var downgradableDependenciesProvider = this.downgradableDependenciesProvider as DowngradableDependenciesProvider<T>;
            if (downgradableDependenciesProvider == null)
                throw new PrisePluginException($"You're not using the default IDowngradableDependenciesProvider {nameof(DowngradableDependenciesProvider<T>)}. Please add downgradable types using your own provider.");
            downgradableDependenciesProvider.AddDowngradableType(type);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> AllowDowngradeForAssembly(string assemblyFileName)
        {
            var downgradableDependenciesProvider = this.downgradableDependenciesProvider as DowngradableDependenciesProvider<T>;
            if (downgradableDependenciesProvider == null)
                throw new PrisePluginException($"You're not using the default IDowngradableDependenciesProvider {nameof(DowngradableDependenciesProvider<T>)}. Please add downgradable types using your own provider.");
            downgradableDependenciesProvider.AddDowngradableAssembly(assemblyFileName);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> WithHostType(Type type)
        {
            var hostTypesProvider = this.hostTypesProvider as HostTypesProvider<T>;
            if (hostTypesProvider == null)
                throw new PrisePluginException($"You're not using the default IHostTypesProvider {nameof(HostTypesProvider<T>)}. Please add host types using your own provider.");
            hostTypesProvider.AddHostType(type);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> WithHostAssembly(string assemblyFileName)
        {
            var hostTypesProvider = this.hostTypesProvider as HostTypesProvider<T>;
            if (hostTypesProvider == null)
                throw new PrisePluginException($"You're not using the default IHostTypesProvider {nameof(HostTypesProvider<T>)}. Please add host types using your own provider.");
            hostTypesProvider.AddHostAssembly(assemblyFileName);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> WithRemoteType(Type type)
        {
            var remoteTypesProvider = this.remoteTypesProvider as RemoteTypesProvider<T>;
            if (remoteTypesProvider == null)
                throw new PrisePluginException($"You're not using the default IRemoteTypesProvider {nameof(RemoteTypesProvider<T>)}. Please add remote types using your own provider.");
            remoteTypesProvider.AddRemoteType(type);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithDependencyPathProvider<TType>()
            where TType : IDependencyPathProvider<T>
        {
            this.dependencyPathProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> UsePluginContextAsDependencyPath()
        {
            this.dependencyPathProviderType = typeof(PluginContextAsDependencyPathProvider<T>);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> WithProbingPath(string path)
        {
            var probingPathsProvider = this.probingPathsProvider as ProbingPathsProvider<T>;
            if (probingPathsProvider == null)
                throw new PrisePluginException($"You're not using the default IProbingPathsProvider {nameof(ProbingPathsProvider<T>)}. Please add probing paths using your own provider.");
            probingPathsProvider.AddProbingPath(path);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProbingPathsProvider<TType>()
           where TType : IProbingPathsProvider<T>
        {
            this.probingPathsProviderType = typeof(TType);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithProbingPathsProvider(IProbingPathsProvider<T> probingPathsProvider)
        {
            this.probingPathsProvider = probingPathsProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithDependencyPathProvider(IDependencyPathProvider<T> dependencyPathProvider)
        {
            this.dependencyPathProvider = dependencyPathProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostFrameworkProvider(IHostFrameworkProvider hostFrameworkProvider)
        {
            this.hostFrameworkProviderType = null;
            this.hostFrameworkProvider = hostFrameworkProvider;
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostFrameworkProvider<TType>()
           where TType : IHostFrameworkProvider
        {
            this.hostFrameworkProviderType = typeof(TType);
            return this;
        }

        private bool ignorePlatformInconsistencies = false;
        public PluginLoadOptionsBuilder<T> IgnorePlatformInconsistencies(bool ignore = true)
        {
            this.ignorePlatformInconsistencies = ignore;

            if (this.assemblyLoadOptionsType != null || this.networkAssemblyLoaderOptionsType != null || this.assemblyLoader != null)
                throw new PrisePluginException("Custom loaders and custom load options are not supported with IgnorePlatformInconsistencies(), please provide your own value for IgnorePlatformInconsistencies.");

            if (this.assemblyLoadOptions != null)
                this.assemblyLoadOptions = new DefaultAssemblyLoadOptions<T>(
                    this.assemblyLoadOptions.PluginPlatformVersion,
                    this.ignorePlatformInconsistencies,
                    this.useCollectibleAssemblies,
                    this.assemblyLoadOptions.NativeDependencyLoadPreference
                );

            if (this.networkAssemblyLoaderOptions != null)
                this.networkAssemblyLoaderOptions = new DefaultNetworkAssemblyLoaderOptions<T>(
                   this.networkAssemblyLoaderOptions.BaseUrl,
                   this.networkAssemblyLoaderOptions.PluginPlatformVersion,
                   this.ignorePlatformInconsistencies,
                   this.networkAssemblyLoaderOptions.NativeDependencyLoadPreference
                );

            return this;
        }

        public PluginLoadOptionsBuilder<T> WithAgressiveUnloading()
        {
            if (this.assemblyLoadOptionsType != null || this.networkAssemblyLoaderOptionsType != null || this.assemblyLoader != null)
                throw new PrisePluginException("Custom loaders and custom load options are not supported with IgnorePlatformInconsistencies(), please provide your own value for IgnorePlatformInconsistencies.");

            if (this.assemblyLoadOptions != null)
                this.assemblyLoadOptions = new DefaultAssemblyLoadOptions<T>(
                    this.assemblyLoadOptions.PluginPlatformVersion,
                    this.assemblyLoadOptions.IgnorePlatformInconsistencies,
                    this.useCollectibleAssemblies,
                    this.assemblyLoadOptions.NativeDependencyLoadPreference
                );

            if (this.networkAssemblyLoaderOptions != null)
                this.networkAssemblyLoaderOptions = new DefaultNetworkAssemblyLoaderOptions<T>(
                   this.networkAssemblyLoaderOptions.BaseUrl,
                   this.networkAssemblyLoaderOptions.PluginPlatformVersion,
                   this.networkAssemblyLoaderOptions.IgnorePlatformInconsistencies,
                   this.networkAssemblyLoaderOptions.NativeDependencyLoadPreference
                );

            return this;
        }

        public PluginLoadOptionsBuilder<T> WithHostType<THostType>() => WithHostType(typeof(THostType));

        public PluginLoadOptionsBuilder<T> WithRemoteType<TRemoteType>() => WithRemoteType(typeof(TRemoteType));

        private IServiceCollection hostServices = new ServiceCollection();
        private IServiceCollection sharedServices = new ServiceCollection();

        private bool IsPriseService(Type type) => type.Namespace.StartsWith("Prise.");
        private bool Includes(Type type, IEnumerable<Type> includeTypes)
        {
            if (includeTypes == null)
                return true;
            return includeTypes.Contains(type);
        }

        private bool Excludes(Type type, IEnumerable<Type> excludeTypes)
        {
            if (excludeTypes == null)
                return false;
            return excludeTypes.Contains(type);
        }

        public PluginLoadOptionsBuilder<T> UseHostServices(
            IServiceCollection hostServices,
            IEnumerable<Type> includeTypes = null,
            IEnumerable<Type> excludeTypes = null)
        {
            if (this.sharedServicesProviderType != null)
                throw new PrisePluginException($"A custom {typeof(ISharedServicesProvider<T>).Name} type cannot be used in combination with {nameof(ConfigureSharedServices)}service");

            this.hostServices = new ServiceCollection();

            var priseServices = hostServices.Where(s => IsPriseService(s.ServiceType));
            var includeServices = hostServices.Where(s => Includes(s.ServiceType, includeTypes));
            var excludeServices = hostServices.Where(s => Excludes(s.ServiceType, excludeTypes));

            foreach (var service in hostServices
                .Except(priseServices)
                .Union(includeServices)
                .Except(excludeServices))
                this.hostServices.Add(service);

            foreach (var hostService in this.hostServices)
                this
                    // A host type will always live inside the host
                    .WithHostType(hostService.ServiceType)
                    // The implementation type will always exist on the Host, since it will be created here
                    .WithHostType(hostService.ImplementationType ?? hostService.ImplementationInstance?.GetType() ?? hostService.ImplementationFactory?.Method.ReturnType)
                ;

            this.sharedServicesProvider = new DefaultSharedServicesProvider<T>(this.hostServices, this.sharedServices);
            this.activator = new DefaultRemotePluginActivator<T>(this.sharedServicesProvider);
            return this;
        }

        //TODO PORT ??
        public PluginLoadOptionsBuilder<T> ConfigureHostServices(Action<IServiceCollection> hostServicesConfig)
        {
            if (this.sharedServicesProviderType != null)
                throw new PrisePluginException($"A custom {typeof(ISharedServicesProvider<T>).Name} type cannot be used in combination with {nameof(ConfigureSharedServices)}service");

            this.hostServices = new ServiceCollection();
            hostServicesConfig.Invoke(this.hostServices);

            foreach (var hostService in this.hostServices)
                this
                    // A host type will always live inside the host
                    .WithHostType(hostService.ServiceType)
                    // The implementation type will always exist on the Host, since it will be created here
                    .WithHostType(hostService.ImplementationType ?? hostService.ImplementationInstance?.GetType() ?? hostService.ImplementationFactory?.Method.ReturnType)
                ;

            this.sharedServicesProvider = new DefaultSharedServicesProvider<T>(this.hostServices, this.sharedServices);
            this.activator = new DefaultRemotePluginActivator<T>(this.sharedServicesProvider);
            return this;
        }

        //TODO PORT
        public PluginLoadOptionsBuilder<T> ConfigureSharedServices(Action<IServiceCollection> sharedServicesConfig)
        {
            if (this.sharedServicesProviderType != null)
                throw new PrisePluginException($"A custom {typeof(ISharedServicesProvider<T>).Name} type cannot be used in combination with {nameof(ConfigureSharedServices)}service");

            this.sharedServices = new ServiceCollection();
            sharedServicesConfig.Invoke(this.sharedServices);

            foreach (var sharedService in this.sharedServices)
                this
                    // The service type must exist on the remote to support backwards compatability
                    .WithRemoteType(sharedService.ServiceType)
                    // The implementation type will always exist on the Host, since it will be created here
                    .WithHostType(sharedService.ImplementationType ?? sharedService.ImplementationInstance?.GetType() ?? sharedService.ImplementationFactory?.Method.ReturnType)
                ; // If a shared service is added, it must be a added as a host type

            this.sharedServicesProvider = new DefaultSharedServicesProvider<T>(this.hostServices, this.sharedServices);
            this.activator = new DefaultRemotePluginActivator<T>(this.sharedServicesProvider);
            return this;
        }

        public PluginLoadOptionsBuilder<T> ScanForAssemblies(Action<AssemblyScanningComposer<T>> composerOptions)
        {
            var composer = new AssemblyScanningComposer<T>();
            composerOptions(composer.WithDefaultOptions<DefaultAssemblyScanner<T>, DefaultAssemblyScannerOptions<T>>());
            var composition = composer.Compose();

            this.assemblyScanner = composition.Scanner;
            this.assemblyScannerType = composition.ScannerType;
            this.assemblyScannerOptions = composition.ScannerOptions;
            this.assemblyScannerOptionsType = composition.ScannerOptionsType;

            return this;
        }

        public PluginLoadOptionsBuilder<T> LogToConsole()
        {
            this.loggerType = typeof(ConsolePluginLogger<T>);
            return this;
        }

        public PluginLoadOptionsBuilder<T> WithDefaultOptions(string pluginPath = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (String.IsNullOrEmpty(pluginPath))
                pluginPath = Path.Join(GetLocalExecutionPath(), "Plugins");

            this.priseServiceLifetime = serviceLifetime;
            this.loggerType = typeof(NullPluginLogger<T>);
            this.pluginPathProvider = new DefaultPluginPathProvider<T>(pluginPath);
            this.dependencyPathProvider = new DependencyPathProvider<T>(pluginPath);
            this.cacheOptions = CacheOptions<IPluginCache<T>>.ScopedPluginCache();

            this.runtimePlatformContext = new RuntimePlatformContext();
            this.ScanForAssemblies(composer =>
                composer.WithDefaultOptions<DefaultAssemblyScanner<T>, DefaultAssemblyScannerOptions<T>>());

            this.pluginAssemblyNameProvider = new PluginAssemblyNameProvider<T>($"{typeof(T).Name}.dll");
            this.sharedServicesProvider = new DefaultSharedServicesProvider<T>(this.hostServices, this.sharedServices);
            this.pluginActivationContextProviderType = typeof(DefaultPluginActivationContextProvider<T>);
            this.pluginTypesProviderType = typeof(DefaultPluginTypesProvider<T>);
            this.activatorType = typeof(DefaultRemotePluginActivator<T>);
            this.proxyCreatorType = typeof(PluginProxyCreator<T>);

            this.parameterConverterType = typeof(JsonSerializerParameterConverter);
            this.resultConverterType = typeof(JsonSerializerResultConverter);

            this.assemblyLoaderType = typeof(DefaultAssemblyLoader<T>);
#if NETCORE3_0 || NETCORE3_1 // Replace with 3.x loader
            this.assemblyLoaderType = typeof(DefaultAssemblyLoaderWithNativeResolver<T>);
#endif
            this.assemblySelectorType = typeof(DefaultAssemblySelector<T>);
            this.assemblyLoadOptions = new DefaultAssemblyLoadOptions<T>(
                PluginPlatformVersion.Empty(),
                this.ignorePlatformInconsistencies,
                this.useCollectibleAssemblies,
                NativeDependencyLoadPreference.PreferInstalledRuntime
            );

            this.probingPathsProviderType = typeof(ProbingPathsProvider<T>);

            var hostTypesProvider = new HostTypesProvider<T>();
            hostTypesProvider.AddHostType(typeof(Prise.Plugin.PluginAttribute)); // Add the Prise.Infrastructure assembly to the host types
            hostTypesProvider.AddHostType(typeof(ServiceCollection));  // Adds the BuildServiceProvider assembly to the host types
            this.hostTypesProvider = hostTypesProvider;

            var downgradableDependenciesProvider = new DowngradableDependenciesProvider<T>();
            downgradableDependenciesProvider.AddDowngradableType(typeof(Prise.Plugin.PluginAttribute)); // Add the Prise.Infrastructure assembly to the host types
            this.downgradableDependenciesProvider = downgradableDependenciesProvider;

            var remoteTypesProvider = new RemoteTypesProvider<T>();
            remoteTypesProvider.AddRemoteType(typeof(T)); // Add the contract to the remote types, so that we can have backwards compatibility
            this.remoteTypesProvider = remoteTypesProvider;

            this.pluginSelectorType = typeof(DefaultPluginSelector<T>);
            this.depsFileProviderType = typeof(DefaultDepsFileProvider<T>);
            this.pluginDependencyResolverType = typeof(DefaultPluginDependencyResolver<T>);
            // Typically used for downloading and storing plugins from the network, but it could be useful for caching local plugins as well
            this.tempPathProviderType = typeof(UserProfileTempPathProvider<T>);

            this.nativeAssemblyUnloaderType = typeof(DefaultNativeAssemblyUnloader);
            this.hostFrameworkProviderType = typeof(HostFrameworkProvider);

            return this;
        }

        internal IServiceCollection RegisterOptions(IServiceCollection services)
        {
            // Caching
            services.Add(new ServiceDescriptor(typeof(IPluginCache<T>), typeof(DefaultScopedPluginCache<T>), this.cacheOptions.Lifetime));

            services
                // Plugin-specific services
                .RegisterTypeOrInstance<IPluginLogger<T>>(loggerType, this.logger, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginPathProvider<T>>(pluginPathProviderType, this.pluginPathProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IAssemblyScanner<T>>(assemblyScannerType, this.assemblyScanner, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IAssemblyScannerOptions<T>>(assemblyScannerOptionsType, this.assemblyScannerOptions, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginTypesProvider<T>>(pluginTypesProviderType, this.pluginTypesProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginActivationContextProvider<T>>(pluginActivationContextProviderType, this.pluginActivationContextProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginProxyCreator<T>>(proxyCreatorType, this.proxyCreator, this.priseServiceLifetime)
                .RegisterTypeOrInstance<ISharedServicesProvider<T>>(sharedServicesProviderType, this.sharedServicesProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginAssemblyNameProvider<T>>(pluginAssemblyNameProviderType, this.pluginAssemblyNameProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginAssemblyLoader<T>>(assemblyLoaderType, this.assemblyLoader, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IRemoteTypesProvider<T>>(remoteTypesProviderType, this.remoteTypesProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IDependencyPathProvider<T>>(dependencyPathProviderType, this.dependencyPathProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IProbingPathsProvider<T>>(probingPathsProviderType, this.probingPathsProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IAssemblySelector<T>>(assemblySelectorType, this.assemblySelector, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginSelector<T>>(pluginSelectorType, this.pluginSelector, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IDepsFileProvider<T>>(depsFileProviderType, this.depsFileProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IPluginDependencyResolver<T>>(pluginDependencyResolverType, this.pluginDependencyResolver, this.priseServiceLifetime)
                .RegisterTypeOrInstance<ITempPathProvider<T>>(tempPathProviderType, this.tempPathProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IHostTypesProvider<T>>(hostTypesProviderType, this.hostTypesProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IDowngradableDependenciesProvider<T>>(downgradableDependenciesProviderType, this.downgradableDependenciesProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IRemotePluginActivator<T>>(activatorType, this.activator, this.priseServiceLifetime)

                // Global services
                .RegisterTypeOrInstance<IAssemblyLoadStrategyProvider>(assemblyLoadStrategyProviderType, this.assemblyLoadStrategyProvider, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IResultConverter>(resultConverterType, this.resultConverter, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IParameterConverter>(parameterConverterType, this.parameterConverter, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IRuntimePlatformContext>(runtimePlatformContextType, this.runtimePlatformContext, this.priseServiceLifetime)
                .RegisterTypeOrInstance<INativeAssemblyUnloader>(nativeAssemblyUnloaderType, this.nativeAssemblyUnloader, this.priseServiceLifetime)
                .RegisterTypeOrInstance<IHostFrameworkProvider>(hostFrameworkProviderType, this.hostFrameworkProvider, this.priseServiceLifetime)
                ;

            if (assemblyLoadOptions != null)
                services
                    .Add(new ServiceDescriptor(typeof(IAssemblyLoadOptions<T>), s => assemblyLoadOptions, this.priseServiceLifetime));
            if (assemblyLoadOptionsType != null)
                services
                    .Add(new ServiceDescriptor(typeof(IAssemblyLoadOptions<T>), assemblyLoadOptionsType, this.priseServiceLifetime));

            if (networkAssemblyLoaderOptions != null)
            {
                services.Add(new ServiceDescriptor(typeof(INetworkAssemblyLoaderOptions<T>), s => networkAssemblyLoaderOptions, this.priseServiceLifetime));
                services.Add(new ServiceDescriptor(typeof(IAssemblyLoadOptions<T>), s => networkAssemblyLoaderOptions, this.priseServiceLifetime));
            }
            if (networkAssemblyLoaderOptionsType != null)
            {
                services.Add(new ServiceDescriptor(typeof(INetworkAssemblyLoaderOptions<T>), networkAssemblyLoaderOptionsType, this.priseServiceLifetime));
                services.Add(new ServiceDescriptor(typeof(IAssemblyLoadOptions<T>), networkAssemblyLoaderOptionsType, this.priseServiceLifetime));
            }

            configureServices?.Invoke(services);

            // Make use of DI by providing an injected instance of the registered services above
            services.Add(new ServiceDescriptor(typeof(IPluginLoadOptions<T>), typeof(PluginLoadOptions<T>), this.priseServiceLifetime));

            return services;
        }

        private string GetLocalExecutionPath() => AppDomain.CurrentDomain.BaseDirectory;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                // What to do here ?
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal static class PluginLoadOptionsBuilderExtensions
    {
        internal static IServiceCollection RegisterTypeOrInstance<TType>(this IServiceCollection services, Type type, TType instance, ServiceLifetime serviceLifetime)
            where TType : class
        {
            if (type != null)
                services.Add(new ServiceDescriptor(typeof(TType), type, serviceLifetime));
            else if (instance != null)
                services.Add(new ServiceDescriptor(typeof(TType), s => instance, serviceLifetime));
            else
                throw new PrisePluginException($"Could not find type {type?.Name} or instance {typeof(TType).Name} to register");

            return services;
        }
    }
}