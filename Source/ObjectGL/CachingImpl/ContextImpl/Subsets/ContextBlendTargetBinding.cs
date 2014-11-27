#region License
/*
Copyright (c) 2012-2014 ObjectGL Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Subsets;

namespace ObjectGL.CachingImpl.ContextImpl.Subsets
{
    public class ContextBlendTargetBinding : IContextBlendTargetBindings
    {
        public IBinding<BlendEquation> Equation { get; private set; }
        public IBinding<BlendFunction> Function { get; private set; }

        public ContextBlendTargetBinding(IContext context, int? index)
        {
            if (!index.HasValue)
            {
                Equation = new Binding<BlendEquation>(context, (c, x) =>
                {
                    c.States.Blend.SeparationModeCache = SeparationMode.United;
                    if (x.Rgb == x.Alpha)
                        c.GL.BlendEquation((int)x.Rgb);
                    else
                        c.GL.BlendEquationSeparate((int)x.Rgb, (int)x.Alpha);
                });
                Function = new Binding<BlendFunction>(context, (c, x) =>
                {
                    c.States.Blend.SeparationModeCache = SeparationMode.United;
                    if (x.SourceRgb == x.SourceAlpha && x.DestinationRgb == x.DestinationAlpha)
                        c.GL.BlendFunc((int)x.SourceRgb, (int)x.DestinationRgb);
                    else
                        c.GL.BlendFuncSeparate((int)x.SourceRgb, (int)x.DestinationRgb,
                            (int)x.SourceAlpha, (int)x.DestinationAlpha);
                });
            }
            else
            {
                var indexLoc = (uint)index.Value;
                Equation = new Binding<BlendEquation>(context, (c, x) =>
                {
                    c.States.Blend.SeparationModeCache = SeparationMode.Separate;
                    if (x.Rgb == x.Alpha)
                        c.GL.BlendEquation(indexLoc, (int)x.Rgb);
                    else
                        c.GL.BlendEquationSeparate(indexLoc, (int)x.Rgb, (int)x.Alpha);
                });
                Function = new Binding<BlendFunction>(context, (c, x) =>
                {
                    c.States.Blend.SeparationModeCache = SeparationMode.Separate;
                    if (x.SourceRgb == x.SourceAlpha && x.DestinationRgb == x.DestinationAlpha)
                        c.GL.BlendFunc(indexLoc, (int)x.SourceRgb, (int)x.DestinationRgb);
                    else
                        c.GL.BlendFuncSeparate(indexLoc, (int)x.SourceRgb, (int)x.DestinationRgb,
                            (int)x.SourceAlpha, (int)x.DestinationAlpha);
                });
            }
        }

        public void OnExternalChange()
        {
            Equation.OnExternalChange();
            Function.OnExternalChange();
        }
    }
}