using ObjectGL.Api.Context;
using ObjectGL.Api.Context.States;
using ObjectGL.Api.Context.States.ScreenClipping;

namespace ObjectGL.CachingImpl.Context.States
{
    public class ContextScreenClippingUnitedBindings : IContextScreenClippingUnitedBindings
    {
        public IBinding<ViewportInt> Viewport { get; private set; }
        public IBinding<DepthRangeFloat> DepthRange { get; private set; }
        public IBinding<ScissorBox> ScissorBox { get; private set; }

        public ContextScreenClippingUnitedBindings(IContext context)
        {
            Viewport = new Binding<ViewportInt>(context, (c, x) =>
            {
                c.States.ScreenClipping.SeparationModeCache = SeparationMode.United;
                c.GL.Viewport(x.X, x.Y, x.Width, x.Height);
            });
            DepthRange = new Binding<DepthRangeFloat>(context, (c, x) =>
            {
                c.States.ScreenClipping.SeparationModeCache = SeparationMode.United;
                c.GL.DepthRange(x.Near, x.Far);
            });
            ScissorBox = new Binding<ScissorBox>(context, (c, x) =>
            {
                c.States.ScreenClipping.SeparationModeCache = SeparationMode.United;
                c.GL.Scissor(x.X, x.Y, x.Width, x.Height);
            });
        }
        
        public void OnExternalChange()
        {
            Viewport.OnExternalChange();
            DepthRange.OnExternalChange();
            ScissorBox.OnExternalChange();
        }
    }
}