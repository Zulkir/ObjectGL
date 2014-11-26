namespace ObjectGL.Api.Raw
{
    public interface IContextScreenClippingBindings
    {
        IContextScreenClippingUnitedBindings United { get; }
        IContextScreenClippingSeparateBindings Separate { get; }
        SeparationMode SeparationModeCache { get; set; }
    }
}