using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModestTree;
using ModestTree.Util;
using ModestTree.Util.Debugging;

#if !ZEN_NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    // Responsibilities:
    // - Expose methods to configure object graph via Bind() methods
    // - Build object graphs via Resolve() method
    public class DiContainer : IInstantiator, IResolver, IBinder
    {
        readonly Dictionary<BindingId, List<ProviderBase>> _providers = new Dictionary<BindingId, List<ProviderBase>>();
        readonly SingletonProviderMap _singletonMap;
        readonly List<IInstaller> _installedInstallers = new List<IInstaller>();
        readonly Stack<Type> _instantiatesInProgress = new Stack<Type>();

#if !ZEN_NOT_UNITY3D
        readonly Transform _rootTransform;
#endif

        bool _allowNullBindings;
        bool _checkForCircularDependencies;
        ProviderBase _fallbackProvider;

        // NOTE! For multi-threaded environments outside unity, you need to pass in checkForCircularDependencies as false
        public DiContainer(
#if !ZEN_NOT_UNITY3D
            Transform rootTransform,
#endif
            bool checkForCircularDependencies)
        {
#if !ZEN_NOT_UNITY3D
            _rootTransform = rootTransform;
#endif

            _checkForCircularDependencies = checkForCircularDependencies;

            _singletonMap = new SingletonProviderMap(this);

            this.Bind<DiContainer>().ToInstance(this);
            this.Bind<IInstantiator>().ToInstance(this);
            this.Bind<SingletonProviderMap>().ToInstance(_singletonMap);

#if !ZEN_NOT_UNITY3D
            this.Bind<PrefabSingletonProviderMap>().ToSingle<PrefabSingletonProviderMap>();
#endif
            this.Bind<SingletonInstanceHelper>().ToSingle<SingletonInstanceHelper>();
        }

#if !ZEN_NOT_UNITY3D
        public DiContainer(Transform rootTransform)
            : this(rootTransform, true)
        {
        }
#endif

        public DiContainer()
#if ZEN_NOT_UNITY3D
            : this(true)
#else
            : this(null)
#endif
        {
        }

        public bool CheckForCircularDependencies
        {
            get
            {
                return _checkForCircularDependencies;
            }
            set
            {
                _checkForCircularDependencies = value;
            }
        }

        public IEnumerable<IInstaller> InstalledInstallers
        {
            get
            {
                return _installedInstallers;
            }
        }

#if !ZEN_NOT_UNITY3D
        public Transform RootTransform
        {
            get
            {
                return _rootTransform;
            }
        }
#endif

        // This can be used to handle the case where the given contract is not
        // found in any other providers, and the contract is not optional
        // For example, to automatically mock-out missing dependencies, you can
        // do this:
        // _container.FallbackProvider = new TransientMockProvider(_container);
        // It can also be used to create nested containers:
        // var nestedContainer = new DiContainer();
        // nestedContainer.FallbackProvider = new DiContainerProvider(mainContainer);
        public ProviderBase FallbackProvider
        {
            get
            {
                return _fallbackProvider;
            }
            set
            {
                _fallbackProvider = value;
            }
        }

        // This flag is used during validation
        // in which case we use nulls to indicate whether we have an instance or not
        // Should be set to false otherwise
        public bool AllowNullBindings
        {
            get
            {
                return _allowNullBindings;
            }
            set
            {
                _allowNullBindings = value;
            }
        }

        public IEnumerable<BindingId> AllContracts
        {
            get
            {
                return _providers.Keys;
            }
        }

        // Note that this list is not exhaustive or even accurate so use with caution
        public IEnumerable<Type> AllConcreteTypes
        {
            get
            {
                return (from x in _providers from p in x.Value select p.GetInstanceType()).Where(x => x != null && !x.IsInterface && !x.IsAbstract).Distinct();
            }
        }

        public IFactoryUntypedBinder<TContract> BindIFactoryUntyped<TContract>(string identifier)
        {
            return new IFactoryUntypedBinder<TContract>(this, identifier);
        }

        public IFactoryUntypedBinder<TContract> BindIFactoryUntyped<TContract>()
        {
            return BindIFactoryUntyped<TContract>(null);
        }

        public IFactoryBinder<TContract> BindIFactory<TContract>(string identifier)
        {
            return new IFactoryBinder<TContract>(this, identifier);
        }

        public IFactoryBinder<TContract> BindIFactory<TContract>()
        {
            return BindIFactory<TContract>(null);
        }

        public IFactoryBinder<TParam1, TContract> BindIFactory<TParam1, TContract>(string identifier)
        {
            return new IFactoryBinder<TParam1, TContract>(this, identifier);
        }

        public IFactoryBinder<TParam1, TContract> BindIFactory<TParam1, TContract>()
        {
            return BindIFactory<TParam1, TContract>(null);
        }

        public IFactoryBinder<TParam1, TParam2, TContract> BindIFactory<TParam1, TParam2, TContract>(string identifier)
        {
            return new IFactoryBinder<TParam1, TParam2, TContract>(this, identifier);
        }

        public IFactoryBinder<TParam1, TParam2, TContract> BindIFactory<TParam1, TParam2, TContract>()
        {
            return BindIFactory<TParam1, TParam2, TContract>(null);
        }

        public IFactoryBinder<TParam1, TParam2, TParam3, TContract> BindIFactory<TParam1, TParam2, TParam3, TContract>(string identifier)
        {
            return new IFactoryBinder<TParam1, TParam2, TParam3, TContract>(this, identifier);
        }

        public IFactoryBinder<TParam1, TParam2, TParam3, TContract> BindIFactory<TParam1, TParam2, TParam3, TContract>()
        {
            return BindIFactory<TParam1, TParam2, TParam3, TContract>(null);
        }

        public IFactoryBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TContract>(string identifier)
        {
            return new IFactoryBinder<TParam1, TParam2, TParam3, TParam4, TContract>(this, identifier);
        }

        public IFactoryBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TContract>()
        {
            return BindIFactory<TParam1, TParam2, TParam3, TParam4, TContract>(null);
        }

        public BinderGeneric<TContract> Rebind<TContract>()
        {
            this.Unbind<TContract>();
            return this.Bind<TContract>();
        }

        public BinderGeneric<TContract> Bind<TContract>(string identifier)
        {
            Assert.That(!typeof(TContract).DerivesFromOrEqual<IInstaller>(),
                "Deprecated usage of Bind<IInstaller>, use Install<IInstaller> instead");
            return new BinderGeneric<TContract>(this, identifier, _singletonMap);
        }

        // Note that this can include open generic types as well such as List<>
        public BinderUntyped Bind(Type contractType, string identifier)
        {
            Assert.That(!contractType.DerivesFromOrEqual<IInstaller>(),
                "Deprecated usage of Bind<IInstaller>, use Install<IInstaller> instead");
            return new BinderUntyped(this, contractType, identifier, _singletonMap);
        }

        public BindScope CreateScope()
        {
            return new BindScope(this, _singletonMap);
        }

        public void RegisterProvider(
            ProviderBase provider, BindingId bindingId)
        {
            if (_providers.ContainsKey(bindingId))
            {
                // Prevent duplicate singleton bindings:
                if (_providers[bindingId].Find(item => ReferenceEquals(item, provider)) != null)
                {
                    throw new ZenjectBindException(
                        "Found duplicate singleton binding for contract '{0}' and id '{1}'".Fmt(bindingId.Type, bindingId.Identifier));
                }

                _providers[bindingId].Add(provider);
            }
            else
            {
                _providers.Add(bindingId, new List<ProviderBase> {provider});
            }
        }

        public int UnregisterProvider(ProviderBase provider)
        {
            int numRemoved = 0;

            foreach (var keyValue in _providers)
            {
                numRemoved += keyValue.Value.RemoveAll(x => x == provider);
            }

            Assert.That(numRemoved > 0, "Tried to unregister provider that was not registered");

            // Remove any empty contracts
            foreach (var bindingId in _providers.Where(x => x.Value.IsEmpty()).Select(x => x.Key).ToList())
            {
                _providers.Remove(bindingId);
            }

            provider.Dispose();

            return numRemoved;
        }

        public IEnumerable<Type> GetDependencyContracts<TContract>()
        {
            return GetDependencyContracts(typeof(TContract));
        }

        public IEnumerable<ZenjectResolveException> ValidateResolve<TContract>()
        {
            return ValidateResolve<TContract>((string)null);
        }

        public IEnumerable<ZenjectResolveException> ValidateObjectGraph<TConcrete>(params Type[] extras)
        {
            return ValidateObjectGraph(typeof(TConcrete), extras);
        }

        public IEnumerable<ZenjectResolveException> ValidateResolve<TContract>(string identifier)
        {
            return ValidateResolve(new InjectContext(this, typeof(TContract), identifier));
        }

        public IEnumerable<ZenjectResolveException> ValidateValidatables(params Type[] ignoreTypes)
        {
            foreach (var pair in _providers)
            {
                var bindingId = pair.Key;

                if (ignoreTypes.Where(i => bindingId.Type.DerivesFromOrEqual(i)).Any())
                {
                    continue;
                }

                // Validate all IValidatableFactory's
                List<ProviderBase> validatableFactoryProviders;

                var providers = pair.Value;

                if (bindingId.Type.DerivesFrom<IValidatableFactory>())
                {
                    validatableFactoryProviders = providers;
                }
                else
                {
                    validatableFactoryProviders = providers.Where(x => x.GetInstanceType().DerivesFrom<IValidatableFactory>()).ToList();
                }

                var injectCtx = new InjectContext(this, bindingId.Type, bindingId.Identifier);

                foreach (var provider in validatableFactoryProviders)
                {
                    var factory = (IValidatableFactory)provider.GetInstance(injectCtx);

                    var type = factory.ConstructedType;
                    var providedArgs = factory.ProvidedTypes;

                    foreach (var error in ValidateObjectGraph(type, injectCtx, providedArgs))
                    {
                        yield return error;
                    }
                }

                // Validate all IValidatable's
                List<ProviderBase> validatableProviders;

                if (bindingId.Type.DerivesFrom<IValidatable>())
                {
                    validatableProviders = providers;
                }
                else
                {
                    validatableProviders = providers.Where(x => x.GetInstanceType().DerivesFrom<IValidatable>()).ToList();
                }

                Assert.That(validatableFactoryProviders.Intersect(validatableProviders).IsEmpty(),
                    "Found provider implementing both IValidatable and IValidatableFactory.  This is not allowed.");

                foreach (var provider in validatableProviders)
                {
                    var factory = (IValidatable)provider.GetInstance(injectCtx);

                    foreach (var error in factory.Validate())
                    {
                        yield return error;
                    }
                }
            }
        }

        // Walk the object graph for the given type
        // Throws ZenjectResolveException if there is a problem
        // Note: If you just want to know whether a binding exists for the given TContract,
        // use HasBinding instead
        // Returns all ZenjectResolveExceptions found
        public IEnumerable<ZenjectResolveException> ValidateResolve(InjectContext context)
        {
            return BindingValidator.ValidateContract(this, context);
        }

        public IEnumerable<ZenjectResolveException> ValidateObjectGraph(
            Type contractType, params Type[] extras)
        {
            return ValidateObjectGraph(
                contractType, new InjectContext(this, contractType), extras);
        }

        public IEnumerable<ZenjectResolveException> ValidateObjectGraph(
            Type contractType, InjectContext context, params Type[] extras)
        {
            if (contractType.IsAbstract)
            {
                throw new ZenjectResolveException(
                    "Expected contract type '{0}' to be non-abstract".Fmt(contractType.Name()));
            }

            return BindingValidator.ValidateObjectGraph(this, contractType, context, null, extras);
        }

        // Wrap IEnumerable<> to avoid LINQ mistakes
        internal List<ProviderBase> GetProviderMatches(InjectContext context)
        {
            return GetProviderMatchesInternal(context).ToList();
        }

        // Be careful with this method since it is a coroutine
        IEnumerable<ProviderBase> GetProviderMatchesInternal(InjectContext context)
        {
            return GetProvidersForContract(context.BindingId).Where(x => x.Matches(context));
        }

        internal IEnumerable<ProviderBase> GetProvidersForContract(BindingId bindingId)
        {
            List<ProviderBase> providers;

            if (_providers.TryGetValue(bindingId, out providers))
            {
                return providers;
            }

            // If we are asking for a List<int>, we should also match for any providers that are bound to the open generic type List<>
            // Currently it only matches one and not the other - not totally sure if this is better than returning both
            if (bindingId.Type.IsGenericType && _providers.TryGetValue(new BindingId(bindingId.Type.GetGenericTypeDefinition(), bindingId.Identifier), out providers))
            {
                return providers;
            }

            return Enumerable.Empty<ProviderBase>();
        }

        public bool HasBinding(InjectContext context)
        {
            List<ProviderBase> providers;

            if (!_providers.TryGetValue(context.BindingId, out providers))
            {
                return false;
            }

            return providers.Where(x => x.Matches(context)).HasAtLeast(1);
        }

        public IList ResolveAll(InjectContext context)
        {
            // Note that different types can map to the same provider (eg. a base type to a concrete class and a concrete class to itself)

            var matches = GetProviderMatchesInternal(context).ToList();

            if (matches.Any())
            {
                return ReflectionUtil.CreateGenericList(
                    context.MemberType, matches.Select(x => x.GetInstance(context)).ToArray());
            }

            if (!context.Optional)
            {
                if (_fallbackProvider != null)
                {
                    var listType = typeof(List<>).MakeGenericType(context.MemberType);
                    var subContext = context.ChangeMemberType(listType);

                    return (IList)_fallbackProvider.GetInstance(subContext);
                }

                throw new ZenjectResolveException(
                    "Could not find required dependency with type '" + context.MemberType.Name() + "' \nObject graph:\n" + context.GetObjectGraphString());
            }

            return ReflectionUtil.CreateGenericList(context.MemberType, new object[] {});
        }

        public List<Type> ResolveTypeAll(InjectContext context)
        {
            if (_providers.ContainsKey(context.BindingId))
            {
                return _providers[context.BindingId].Select(x => x.GetInstanceType()).Where(x => x != null).ToList();
            }

            return new List<Type> {};
        }

        public void Install(IInstaller installer)
        {
            if (_installedInstallers.Where(x => x.GetType() == installer.GetType()).IsEmpty())
            // Do not install the same installer twice
            {
                _installedInstallers.Add(installer);
                this.Inject(installer);
                installer.InstallBindings();
            }
        }

        public void Install<T>()
            where T : IInstaller
        {
            Install(typeof(T));
        }

        public void Install(Type installerType)
        {
            Assert.That(installerType.DerivesFrom<IInstaller>());

            if (_installedInstallers.Where(x => x.GetType() == installerType).IsEmpty())
            // Do not install the same installer twice
            {
                var installer = (IInstaller)this.Instantiate(installerType);
                installer.InstallBindings();
                _installedInstallers.Add(installer);
            }
        }

        // Return single instance of requested type or assert
        public object Resolve(InjectContext context)
        {
            // Note that different types can map to the same provider (eg. a base type to a concrete class and a concrete class to itself)

            var providers = GetProviderMatchesInternal(context).ToList();

            if (providers.IsEmpty())
            {
                // If it's a generic list then try matching multiple instances to its generic type
                if (ReflectionUtil.IsGenericList(context.MemberType))
                {
                    var subType = context.MemberType.GetGenericArguments().Single();
                    var subContext = context.ChangeMemberType(subType);

                    return ResolveAll(subContext);
                }

                if (!context.Optional)
                {
                    if (_fallbackProvider != null)
                    {
                        return _fallbackProvider.GetInstance(context);
                    }

                    throw new ZenjectResolveException(
                        "Unable to resolve type '{0}'{1}. \nObject graph:\n{2}"
                        .Fmt(
                            context.MemberType.Name() + (context.Identifier == null ? "" : " with ID '" + context.Identifier.ToString() + "'"),
                            (context.ObjectType == null ? "" : " while building object with type '{0}'".Fmt(context.ObjectType.Name())),
                            context.GetObjectGraphString()));
                }

                return null;
            }

            ProviderBase provider;

            if (providers.Count > 1)
            {
                // If we find multiple providers and we are looking for just one, then
                // choose the one with a condition before giving up and throwing an exception
                // This is nice because it allows us to bind a default and then override with conditions
                provider = providers.Where(x => x.Condition != null).OnlyOrDefault();

                if (provider == null)
                {
                    throw new ZenjectResolveException(
                        "Found multiple matches when only one was expected for type '{0}'{1}. \nObject graph:\n {2}"
                        .Fmt(
                            context.MemberType.Name(),
                            (context.ObjectType == null ? "" : " while building object with type '{0}'".Fmt(context.ObjectType.Name())),
                            context.GetObjectGraphString()));
                }
            }
            else
            {
                provider = providers.Single();
            }

            return provider.GetInstance(context);
        }

        public bool Unbind<TContract>(string identifier)
        {
            List<ProviderBase> providersToRemove;
            var bindingId = new BindingId(typeof(TContract), identifier);

            if (_providers.TryGetValue(bindingId, out providersToRemove))
            {
                _providers.Remove(bindingId);

                // Only dispose if the provider is not bound to another type
                foreach (var provider in providersToRemove)
                {
                    if (_providers.Where(x => x.Value.Contains(provider)).IsEmpty())
                    {
                        provider.Dispose();
                    }
                }

                return true;
            }

            return false;
        }

        public IEnumerable<Type> GetDependencyContracts(Type contract)
        {
            foreach (var injectMember in TypeAnalyzer.GetInfo(contract).AllInjectables)
            {
                yield return injectMember.MemberType;
            }
        }

        // Same as Instantiate except you can pass in null value
        // however the type for each parameter needs to be explicitly provided in this case
        public object InstantiateExplicit(
            Type concreteType, List<TypeValuePair> extraArgMap, InjectContext currentContext, string concreteIdentifier, bool autoInject)
        {
#if PROFILING_ENABLED
            using (ProfileBlock.Start("Zenject.Instantiate({0})", concreteType))
#endif
            {
                if (_checkForCircularDependencies)
                {
                    if (_instantiatesInProgress.Contains(concreteType))
                    {
                        throw new ZenjectResolveException(
                            "Circular dependency detected! \nObject graph:\n" + concreteType.Name() + "\n" + currentContext.GetObjectGraphString());
                    }

                    _instantiatesInProgress.Push(concreteType);
                    try
                    {
                        return InstantiateInternal(concreteType, extraArgMap, currentContext, concreteIdentifier, autoInject);
                    }
                    finally
                    {
                        Assert.That(_instantiatesInProgress.Peek() == concreteType);
                        _instantiatesInProgress.Pop();
                    }
                }
                else
                {
                    return InstantiateInternal(concreteType, extraArgMap, currentContext, concreteIdentifier, autoInject);
                }
            }
        }

        object InstantiateInternal(
            Type concreteType, IEnumerable<TypeValuePair> extraArgs, InjectContext currentContext, string concreteIdentifier, bool autoInject)
        {
#if !ZEN_NOT_UNITY3D
            Assert.That(!concreteType.DerivesFrom<UnityEngine.Component>(),
                "Error occurred while instantiating object of type '{0}'. Instantiator should not be used to create new mono behaviours.  Must use InstantiatePrefabForComponent, InstantiatePrefab, InstantiateComponentOnNewGameObject, InstantiateGameObject, or InstantiateComponent.  You may also want to use GameObjectFactory class or plain old GameObject.Instantiate.", concreteType.Name());
#endif

            var typeInfo = TypeAnalyzer.GetInfo(concreteType);

            if (typeInfo.InjectConstructor == null)
            {
                throw new ZenjectResolveException(
                    "More than one (or zero) constructors found for type '{0}' when creating dependencies.  Use one [Inject] attribute to specify which to use.".Fmt(concreteType));
            }

            // Make a copy since we remove from it below
            var extraArgList = extraArgs.ToList();
            var paramValues = new List<object>();

            foreach (var injectInfo in typeInfo.ConstructorInjectables)
            {
                object value;

                if (!InstantiateUtil.PopValueWithType(extraArgList, injectInfo.MemberType, out value))
                {
                    value = Resolve(injectInfo.CreateInjectContext(this, currentContext, null, concreteIdentifier));
                }

                paramValues.Add(value);
            }

            object newObj;

            try
            {
#if PROFILING_ENABLED
                using (ProfileBlock.Start("{0}.{0}()", concreteType))
#endif
                {
                    newObj = typeInfo.InjectConstructor.Invoke(paramValues.ToArray());
                }
            }
            catch (Exception e)
            {
                throw new ZenjectResolveException(
                    "Error occurred while instantiating object with type '{0}'".Fmt(concreteType.Name()), e);
            }

            if (autoInject)
            {
                InjectExplicit(newObj, extraArgList, true, typeInfo, currentContext, concreteIdentifier);
            }
            else
            {
                if (!extraArgList.IsEmpty())
                {
                    throw new ZenjectResolveException(
                        "Passed unnecessary parameters when injecting into type '{0}'. \nExtra Parameters: {1}\nObject graph:\n{2}"
                        .Fmt(newObj.GetType().Name(), String.Join(",", extraArgList.Select(x => x.Type.Name()).ToArray()), currentContext.GetObjectGraphString()));
                }
            }

            return newObj;
        }

        // Iterate over fields/properties on the given object and inject any with the [Inject] attribute
        public void InjectExplicit(
            object injectable, IEnumerable<TypeValuePair> extraArgs,
            bool shouldUseAll, ZenjectTypeInfo typeInfo, InjectContext context, string concreteIdentifier)
        {
            Assert.IsEqual(typeInfo.TypeAnalyzed, injectable.GetType());
            Assert.That(injectable != null);

#if !ZEN_NOT_UNITY3D
            Assert.That(injectable.GetType() != typeof(GameObject),
                "Use InjectGameObject to Inject game objects instead of Inject method");
#endif

            // Make a copy since we remove from it below
            var extraArgsList = extraArgs.ToList();

            foreach (var injectInfo in typeInfo.FieldInjectables.Concat(typeInfo.PropertyInjectables))
            {
                object value;

                if (InstantiateUtil.PopValueWithType(extraArgsList, injectInfo.MemberType, out value))
                {
                    injectInfo.Setter(injectable, value);
                }
                else
                {
                    value = Resolve(
                        injectInfo.CreateInjectContext(this, context, injectable, concreteIdentifier));

                    if (injectInfo.Optional && value == null)
                    {
                        // Do not override in this case so it retains the hard-coded value
                    }
                    else
                    {
                        injectInfo.Setter(injectable, value);
                    }
                }
            }

            foreach (var method in typeInfo.PostInjectMethods)
            {
#if PROFILING_ENABLED
                using (ProfileBlock.Start("{0}.{1}()", injectable.GetType(), method.MethodInfo.Name))
#endif
                {
                    var paramValues = new List<object>();

                    foreach (var injectInfo in method.InjectableInfo)
                    {
                        object value;

                        if (!InstantiateUtil.PopValueWithType(extraArgsList, injectInfo.MemberType, out value))
                        {
                            value = Resolve(
                                injectInfo.CreateInjectContext(this, context, injectable, concreteIdentifier));
                        }

                        paramValues.Add(value);
                    }

                    method.MethodInfo.Invoke(injectable, paramValues.ToArray());
                }
            }

            if (shouldUseAll && !extraArgsList.IsEmpty())
            {
                throw new ZenjectResolveException(
                    "Passed unnecessary parameters when injecting into type '{0}'. \nExtra Parameters: {1}\nObject graph:\n{2}"
                    .Fmt(injectable.GetType().Name(), String.Join(",", extraArgsList.Select(x => x.Type.Name()).ToArray()), context.GetObjectGraphString()));
            }
        }

#if !ZEN_NOT_UNITY3D

        // NOTE: gameobject here is not a prefab prototype, it is an instance
        public Component InstantiateComponent(
            Type componentType, GameObject gameObject, params object[] extraArgMap)
        {
            Assert.That(componentType.DerivesFrom<Component>());

            var monoBehaviour = (Component)gameObject.AddComponent(componentType);
            this.Inject(monoBehaviour, extraArgMap);
            return monoBehaviour;
        }

        public GameObject InstantiatePrefabExplicit(
            GameObject prefab, IEnumerable<object> extraArgMap, InjectContext context)
        {
            Assert.IsNotNull(_rootTransform);

            var gameObj = (GameObject)GameObject.Instantiate(prefab);

            // By default parent to comp root
            // This is good so that the entire object graph is
            // contained underneath it, which is useful for cases
            // where you need to delete the entire object graph
            gameObj.transform.SetParent(_rootTransform, false);

            gameObj.SetActive(true);

            this.InjectGameObject(gameObj, true, false, extraArgMap, context);

            return gameObj;
        }

        // Create a new empty game object under the composition root
        public GameObject InstantiateGameObject(string name)
        {
            Assert.IsNotNull(_rootTransform);
            var gameObj = new GameObject(name);
            gameObj.transform.SetParent(_rootTransform, false);
            return gameObj;
        }

        public object InstantiateComponentOnNewGameObjectExplicit(
            Type componentType, string name, List<TypeValuePair> extraArgMap, InjectContext currentContext)
        {
            Assert.IsNotNull(_rootTransform);

            Assert.That(componentType.DerivesFrom<Component>(), "Expected type '{0}' to derive from UnityEngine.Component", componentType.Name());

            var gameObj = new GameObject(name);

            gameObj.transform.SetParent(_rootTransform, false);

            if (componentType == typeof(Transform))
            {
                Assert.That(extraArgMap.IsEmpty());
                return gameObj.transform;
            }

            var component = (Component)gameObj.AddComponent(componentType);

            this.InjectExplicit(component, extraArgMap, currentContext);

            return component;
        }

        public object InstantiatePrefabForComponentExplicit(
            Type componentType, GameObject prefab, List<TypeValuePair> extraArgs, InjectContext currentContext)
        {
            Assert.IsNotNull(_rootTransform);
            Assert.That(prefab != null, "Null prefab found when instantiating game object");

            // It could be an interface so this may fail in valid cases so you may want to comment out
            // Leaving it in for now to catch the more likely scenario of it being a mistake
            Assert.That(componentType.DerivesFrom<Component>(), "Expected type '{0}' to derive from UnityEngine.Component", componentType.Name());

            var gameObj = (GameObject)GameObject.Instantiate(prefab);

            // By default parent to comp root
            // This is good so that the entire object graph is
            // contained underneath it, which is useful for cases
            // where you need to delete the entire object graph
            gameObj.transform.SetParent(_rootTransform, false);

            gameObj.SetActive(true);

            Component requestedScript = null;

            // Inject on the children first since the parent objects are more likely to use them in their post inject methods
            foreach (var component in UnityUtil.GetComponentsInChildrenBottomUp<Component>(gameObj, false))
            {
                if (component != null)
                {
                    if (component.GetType().DerivesFromOrEqual(componentType))
                    {
                        Assert.IsNull(requestedScript,
                            "Found multiple matches with type '{0}' when instantiating new game object from prefab '{1}'", componentType, prefab.name);
                        requestedScript = component;

                        this.InjectExplicit(component, extraArgs);
                    }
                    else
                    {
                        this.Inject(component);
                    }
                }
                else
                {
                    Log.Warn("Found null component while instantiating prefab '{0}'.  Possible missing script.", prefab.name);
                }
            }

            if (requestedScript == null)
            {
                throw new ZenjectResolveException(
                    "Could not find component with type '{0}' when instantiating new game object".Fmt(componentType));
            }

            return requestedScript;
        }
#endif
    }
}
