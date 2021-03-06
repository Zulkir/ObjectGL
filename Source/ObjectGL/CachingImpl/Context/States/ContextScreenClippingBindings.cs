﻿using ObjectGL.Api.Context;
using ObjectGL.Api.Context.States;
using ObjectGL.Api.Context.States.ScreenClipping;

namespace ObjectGL.CachingImpl.Context.States
{
    public class ContextScreenClippingBindings : IContextScreenClippingBindings
    {
        public IContextScreenClippingUnitedBindings United { get; private set; }
        public IContextScreenClippingSeparateBindings Separate { get; private set; }
        private SeparationMode separationModeCache;

        public ContextScreenClippingBindings(IContext context, IContextCaps caps)
        {
            United = new ContextScreenClippingUnitedBindings(context);
            Separate = new ContextScreenClippingSeparateBindings(context, caps);
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