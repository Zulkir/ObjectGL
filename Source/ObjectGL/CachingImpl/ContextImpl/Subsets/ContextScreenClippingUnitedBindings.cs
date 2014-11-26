using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
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
                c.ScreenClipping.SeparationModeCache = SeparationMode.United;
                c.GL.Viewport(x.X, x.Y, x.Width, x.Height);
            });
            DepthRange = new Binding<DepthRangeFloat>(context, (c, x) =>
            {
                c.ScreenClipping.SeparationModeCache = SeparationMode.United;
                c.GL.DepthRange(x.Near, x.Far);
            });
            ScissorBox = new Binding<ScissorBox>(context, (c, x) =>
            {
                c.ScreenClipping.SeparationModeCache = SeparationMode.United;
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