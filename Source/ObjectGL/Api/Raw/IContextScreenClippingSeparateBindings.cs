using System.Collections.Generic;

namespace ObjectGL.Api.Raw
{
    public interface IContextScreenClippingSeparateBindings
    {
        IReadOnlyList<IBinding<ViewportFloat>> Viewports { get; }
        IReadOnlyList<IBinding<DepthRangeDouble>> DepthRanges { get; }
        IReadOnlyList<IBinding<ScissorBox>> ScissorBoxes { get; }
        void OnExternalChange();
    }
}