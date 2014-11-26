using ObjectGL.Api;
using ObjectGL.Api.Raw;
using IContext = ObjectGL.Api.Raw.IContext;

namespace ObjectGL.CachingImpl.Raw
{
    public class RawContextScreenClippingBindings : IContextScreenClippingBindings
    {
        public IContextScreenClippingUnitedBindings United { get; private set; }
        public IContextScreenClippingSeparateBindings Separate { get; private set; }
        private SeparationMode separationModeCache;

        public RawContextScreenClippingBindings(IContext context, IImplementation implementation)
        {
            United = new RawContextScreenClippingUnitedBindings(context);
            Separate = new RawContextScreenClippingSeparateBindings(context, implementation);
        }

        public SeparationMode SeparationModeCache
        {
            get { return separationModeCache; }
            set
            {
                if (separationModeCache == value)
                    return;
                separationModeCache = value;
                if (separationModeCache == SeparationMode.United)
                    Separate.OnExternalChange();
                else
                    United.OnExternalChange();
            }
        }
    }
}