using System;

namespace Zenject
{
    public class Binder
    {
        readonly protected Type _contractType;
        readonly protected DiContainer _container;
        readonly protected string _bindIdentifier;

        public Binder(
            DiContainer container,
            Type contractType,
            string bindIdentifier)
        {
            _container = container;
            _contractType = contractType;
            _bindIdentifier = bindIdentifier;
        }

        public virtual BindingConditionSetter ToProvider(ProviderBase provider)
        {
            _container.RegisterProvider(
                provider, new BindingId(_contractType, _bindIdentifier));

            if (_contractType.IsValueType)
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(_contractType);

                // Also bind to nullable primitives
                // this is useful so that we can have optional primitive dependencies
                _container.RegisterProvider(
                    provider, new BindingId(nullableType, _bindIdentifier));
            }

            return new BindingConditionSetter(provider);
        }
    }
}
