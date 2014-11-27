namespace ObjectGL.Api.Context.Subsets
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