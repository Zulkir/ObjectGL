namespace ObjectGL.Api.Context.States.ScreenClipping
{
    public interface IContextScreenClippingBindings
    {
        IContextScreenClippingUnitedBindings United { get; }
        IContextScreenClippingSeparateBindings Separate { get; }
        SeparationMode SeparationModeCache { get; set; }
    }
}