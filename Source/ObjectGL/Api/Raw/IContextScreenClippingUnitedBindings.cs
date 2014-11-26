namespace ObjectGL.Api.Raw
{
    public interface IContextScreenClippingUnitedBindings
    {
        IBinding<ViewportInt> Viewport { get; }
        IBinding<DepthRangeFloat> DepthRange { get; }
        IBinding<ScissorBox> ScissorBox { get; }
        void OnExternalChange();
    }
}