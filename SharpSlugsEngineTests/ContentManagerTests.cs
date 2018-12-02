using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpSlugsEngine;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Tests
{
    [TestClass()]
    public class ContentManagerTests
    {
        [TestMethod()]
        public void AddImageTest()
        {
            TestGame game = new TestGame();
            game.Content.AddImage("test.png", "image");
            if (!game.Content.InManager("image")) Assert.Fail("Image was not added");
        }

        [TestMethod()]
        public void AddFontTest()
        {
            TestGame game = new TestGame();

            Assert.Fail();
        }

        [TestMethod()]
        public void GetFontTest()
        {
            TestGame game = new TestGame();

            Assert.Fail();
        }

        [TestMethod()]
        public void AddSoundTest()
        {
            TestGame game = new TestGame();

            Assert.Fail();
        }

        [TestMethod()]
        public void GetSoundTest()
        {
            TestGame game = new TestGame();

            Assert.Fail();
        }

        [TestMethod()]
        public void ScaleImageTest()
        {
            TestGame game = new TestGame();
            Size size1;
            Size size2;
            game.Content.AddImage("test.png", "test");
            size1 = game.Content.GetImage("test").Size;

            Bitmap bmp = game.Content.ScaleImage(game.Content.GetImage("test"), 2);
            size2 = bmp.Size;
            if (size1 == size2) Assert.Fail("Size was not scaled");
        }

        [TestMethod()]
        public void GetImageTest()
        {
            TestGame game = new TestGame();
            game.Content.AddImage("test.png", "test");

            Bitmap bmp = game.Content.GetImage("test");
            if (bmp == null) Assert.Fail("Bitmap is not gotten");
        }

        [TestMethod()]
        public void InManagerTest()
        {
            TestGame game = new TestGame();
            game.Content.AddImage("test.png", "test");
            if (!game.Content.InManager("test")) Assert.Fail("Image was not found in manager");
        }

        [TestMethod()]
        public void PrintNamesTest()
        {
            TestGame game = new TestGame();
            game.Content.AddImage("test.png", "test");
        }

        [TestMethod()]
        public void SplitImageTest()
        {
            TestGame game = new TestGame();
            game.Content.SplitImage(new Bitmap("test.png"), 4, "file");

            if (!game.Content.InManager("file1")) Assert.Fail("Splits didn't occur");
        }

        private class TestGame : Game
        {
            protected override void Draw(GameTime gameTime)
            {
            }

            protected override void Update(GameTime gameTime)
            {
            }
        }
    }
}