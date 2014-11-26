namespace ObjectGL.Api.Context.Subsets
{
    public interface IContextScreenClippingBindings
    {
        IContextScreenClippingUnitedBindings United { get; }
        IContextScreenClippingSeparateBindings Separate { get; }
        SeparationMode SeparationModeCache { get; set; }
    }
}