using System.Collections.Generic;
using ObjectGL.Api.Objects;
using ObjectGL.Api.Objects.Resources;

namespace ObjectGL.Api.Context.Subsets
{
    public interface IContextBindings
    {
        IContextBufferBindings Buffers { get; }
        IContextTextureBindings Textures { get; }
        IContextFramebufferBindings Framebuffers { get; }
        
        IBinding<IShaderProgram> Program { get; }
        IBinding<IVertexArray> VertexArray { get; }
        IBinding<ITransformFeedback> TransformFeedback { get; }
        IBinding<IRenderbuffer> Renderbuffer { get; }
        IReadOnlyList<IBinding<ISampler>> Samplers { get; }
    }
}