namespace ObjectGL.Api.Context.Subsets
{
    public interface IContextScreenClippingUnitedBindings
    {
        IBinding<ViewportInt> Viewport { get; }
        IBinding<DepthRangeFloat> DepthRange { get; }
        IBinding<ScissorBox> ScissorBox { get; }
        void OnExternalChange();
    }
}