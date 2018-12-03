using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpSlugsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Tests
{
    [TestClass()]
    public class GraphicsManagerTests
    {
        [TestMethod()]
        public void ToWorldScaleTest()
        {
            TestGame game = new TestGame();
            Vector2 worldScale = Vector2.Zero;

            worldScale = game.Graphics.ToWorldScale(worldScale);
            if(worldScale != Vector2.Zero) Assert.Fail();
        }

        [TestMethod()]
        public void ToResolutionScaleTest()
        {
            TestGame game = new TestGame();
            Vector2 worldScale = Vector2.Zero;

            worldScale = game.Graphics.ToResolutionScale(worldScale);
            if (worldScale != Vector2.Zero) Assert.Fail();
        }
    }

    class TestGame : Game
    {
        protected override void Draw(GameTime gameTime)
        {
        }

        protected override void Update(GameTime gameTime)
        {
        }
    }
}