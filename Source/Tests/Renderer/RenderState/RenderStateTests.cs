﻿#region License
//
// (C) Copyright 2009 Patrick Cozzi and Deron Ohlarik
//
// Distributed under the Boost Software License, Version 1.0.
// See License.txt or http://www.boost.org/LICENSE_1_0.txt.
//
#endregion

using NUnit.Framework;
using System.Drawing;
using MiniGlobe.Core.Geometry;

namespace MiniGlobe.Renderer
{
    [TestFixture]
    public class RenderStateTests
    {
        [Test]
        public void PrimitiveRestart()
        {
            PrimitiveRestart primitiveRestart = new PrimitiveRestart();
            primitiveRestart.Enabled = true;
            primitiveRestart.Index = 11;

            Assert.IsTrue(primitiveRestart.Enabled);
            Assert.AreEqual(11, primitiveRestart.Index);
        }

        [Test]
        public void FacetCulling()
        {
            FacetCulling facetCulling = new FacetCulling();
            facetCulling.Enabled = true;
            facetCulling.Face = CullFace.FrontAndBack;
            facetCulling.FrontFaceWindingOrder = WindingOrder.Clockwise;

            Assert.IsTrue(facetCulling.Enabled);
            Assert.AreEqual(CullFace.FrontAndBack, facetCulling.Face);
            Assert.AreEqual(WindingOrder.Clockwise, facetCulling.FrontFaceWindingOrder);
        }

        [Test]
        public void RasterizationMode()
        {
            RenderState renderState = new RenderState();
            renderState.RasterizationMode = MiniGlobe.Renderer.RasterizationMode.Line;

            Assert.AreEqual(MiniGlobe.Renderer.RasterizationMode.Line, renderState.RasterizationMode);
        }

        [Test]
        public void ScissorTest()
        {
            ScissorTest scissorTest = new ScissorTest();
            scissorTest.Enabled = true;
            scissorTest.Rectangle = new Rectangle(0, 1, 2, 3);

            Assert.IsTrue(scissorTest.Enabled);
            Assert.AreEqual(new Rectangle(0, 1, 2, 3), scissorTest.Rectangle);
        }

        [Test]
        public void StencilTest()
        {
            StencilTest stencilTest = new StencilTest();
            StencilTestFace frontStencil = stencilTest.FrontFace;
            StencilTestFace backStencil = stencilTest.BackFace;

            stencilTest.Enabled = true;

            frontStencil.StencilFailOperation = StencilOperation.Keep;
            frontStencil.DepthPassStencilFailOperation = StencilOperation.Keep;
            frontStencil.DepthPassStencilPassOperation = StencilOperation.Increment;
            frontStencil.Function = StencilTestFunction.Always;
            frontStencil.ReferenceValue = 0;
            frontStencil.Mask = 0xFF;

            backStencil.StencilFailOperation = StencilOperation.Zero;
            backStencil.DepthPassStencilFailOperation = StencilOperation.Zero;
            backStencil.DepthPassStencilPassOperation = StencilOperation.Decrement;
            backStencil.Function = StencilTestFunction.Equal;
            backStencil.ReferenceValue = 1;
            backStencil.Mask = 0x03;

            Assert.IsTrue(stencilTest.Enabled);

            Assert.AreEqual(StencilOperation.Keep, frontStencil.StencilFailOperation);
            Assert.AreEqual(StencilOperation.Keep, frontStencil.DepthPassStencilFailOperation);
            Assert.AreEqual(StencilOperation.Increment, frontStencil.DepthPassStencilPassOperation);
            Assert.AreEqual(StencilTestFunction.Always, frontStencil.Function);
            Assert.AreEqual(0, frontStencil.ReferenceValue);
            Assert.AreEqual(0xFF, frontStencil.Mask);

            Assert.AreEqual(StencilOperation.Zero, backStencil.StencilFailOperation);
            Assert.AreEqual(StencilOperation.Zero, backStencil.DepthPassStencilFailOperation);
            Assert.AreEqual(StencilOperation.Decrement, backStencil.DepthPassStencilPassOperation);
            Assert.AreEqual(StencilTestFunction.Equal, backStencil.Function);
            Assert.AreEqual(1, backStencil.ReferenceValue);
            Assert.AreEqual(0x03, backStencil.Mask);
        }

        [Test]
        public void DepthTest()
        {
            DepthTest depthTest = new DepthTest();
            depthTest.Enabled = true;
            depthTest.Function = DepthTestFunction.LessThanOrEqual;

            Assert.IsTrue(depthTest.Enabled);
            Assert.AreEqual(DepthTestFunction.LessThanOrEqual, depthTest.Function);
        }

        [Test]
        public void Blending()
        {
            Blending blending = new Blending();
            blending.Enabled = true;
            blending.SourceRGBFactor = SourceBlendingFactor.SourceAlpha;
            blending.SourceAlphaFactor = SourceBlendingFactor.SourceAlpha;
            blending.DestinationRGBFactor = DestinationBlendingFactor.OneMinusSourceAlpha;
            blending.DestinationAlphaFactor = DestinationBlendingFactor.OneMinusSourceAlpha;
            blending.RGBEquation = BlendEquation.Add;
            blending.AlphaEquation = BlendEquation.Add;
            blending.Color = Color.White;

            Assert.IsTrue(blending.Enabled);
            Assert.AreEqual(SourceBlendingFactor.SourceAlpha, blending.SourceRGBFactor);
            Assert.AreEqual(SourceBlendingFactor.SourceAlpha, blending.SourceAlphaFactor);
            Assert.AreEqual(DestinationBlendingFactor.OneMinusSourceAlpha, blending.DestinationRGBFactor);
            Assert.AreEqual(DestinationBlendingFactor.OneMinusSourceAlpha, blending.DestinationAlphaFactor);
            Assert.AreEqual(BlendEquation.Add, blending.RGBEquation);
            Assert.AreEqual(BlendEquation.Add, blending.AlphaEquation);
            Assert.AreEqual(Color.White, blending.Color);
        }

        [Test]
        public void ApplyDefaultStateBlock()
        {
            MiniGlobeWindow window = Device.CreateWindow(1, 1);

            //
            // Entire state
            //
            window.Context.Bind(new RenderState());

            //
            // Individual states
            //
            window.Context.Bind(new PrimitiveRestart());
            window.Context.Bind(new FacetCulling());
            window.Context.Bind(MiniGlobe.Renderer.RasterizationMode.Line);
            window.Context.Bind(new ScissorTest());
            window.Context.Bind(new StencilTest());
            window.Context.Bind(new DepthTest());
            window.Context.Bind(new Blending());

            window.Dispose();
        }
    }
}