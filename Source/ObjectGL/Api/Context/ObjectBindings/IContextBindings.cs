using System.Collections.Generic;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Samplers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.TransformFeedbacks;
using ObjectGL.Api.Objects.VertexArrays;

namespace ObjectGL.Api.Context.ObjectBindings
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