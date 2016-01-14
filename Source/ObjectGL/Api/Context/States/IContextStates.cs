using ObjectGL.Api.Context.States.Blend;
using ObjectGL.Api.Context.States.DepthStencil;
using ObjectGL.Api.Context.States.Rasterizer;
using ObjectGL.Api.Context.States.ScreenClipping;

namespace ObjectGL.Api.Context.States
{
    public interface IContextStates
    {
        IContextScreenClippingBindings ScreenClipping { get; }
        IContextRasterizerBindings Rasterizer { get; }
        IContextDepthStencilBindings DepthStencil { get; }
        IContextBlendBindings Blend { get; }

        IBinding<int> PatchVertexCount { get; }
        IBinding<int> UnpackAlignment { get; } 
    }
}