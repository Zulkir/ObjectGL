using System.Collections.Generic;
using System.Linq;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.States;
using ObjectGL.Api.Context.States.ScreenClipping;

namespace ObjectGL.CachingImpl.Context.States
{
    public class ContextScreenClippingSeparateBindings : IContextScreenClippingSeparateBindings
    {
        public IReadOnlyList<IBinding<ViewportFloat>> Viewports { get; private set; }
        public IReadOnlyList<IBinding<DepthRangeDouble>> DepthRanges { get; private set; }
        public IReadOnlyList<IBinding<ScissorBox>> ScissorBoxes { get; private set; }

        public ContextScreenClippingSeparateBindings(IContext context, IContextCaps caps)
        {
            Viewports = Enumerable.Range(0, caps.MaxViewports)
                .Select(i => new Binding<ViewportFloat>(context, (c, x) =>
                {
                    c.States.ScreenClipping.SeparationModeCache = SeparationMode.Separate;
                    c.GL.ViewportIndexed((uint)i, x.X, x.Y, x.Width, x.Height);
                })).ToArray();
            DepthRanges = Enumerable.Range(0, caps.MaxViewports)
                .Select(i => new Binding<DepthRangeDouble>(context, (c, x) =>
                {
                    c.States.ScreenClipping.SeparationModeCache = SeparationMode.Separate;
                    c.GL.DepthRangeIndexed((uint)i, x.Near, x.Far);
                })).ToArray();
            ScissorBoxes = Enumerable.Range(0, caps.MaxViewports)
                .Select(i => new Binding<ScissorBox>(context, (c, x) =>
                {
                    c.States.ScreenClipping.SeparationModeCache = SeparationMode.Separate;
                    c.GL.ScissorIndexed((uint)i, x.X, x.Y, x.Width, x.Height);
                })).ToArray();
        }

        public void OnExternalChange()
        {
            foreach (var binding in Viewports)
                binding.OnExternalChange();
            foreach (var binding in DepthRanges)
                binding.OnExternalChange();
            foreach (var binding in ScissorBoxes)
                binding.OnExternalChange();
        }
    }
}