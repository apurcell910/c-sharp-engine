﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpSlugsEngine.Physics;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Enum to choose whether to draw in World Space or Screen Space
    /// </summary>
    public enum DrawType
    {
        Screen,
        World
    }

    /// <summary>
    /// Enum to tell the system where to Align text
    /// </summary>
    public enum TextAlign
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    /// <summary>
    /// A manager to handle graphics
    /// </summary>
    public class GraphicsManager
    {
        internal static readonly Vector2 DefaultScale = new Vector2(100, 56.25f);

        private readonly Game game;
        private readonly Platform platform;

        private SolidBrush brush;
        private Pen pen;
        private Graphics formGraphics;

        private Bitmap buffer;
        private Graphics bitmapGraphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsManager"/> class with given Game and Platform
        /// </summary>
        /// <param name="game">Game for which the Graphics Manger is created for</param>
        /// <param name="platform">Platform with which Graphics will be drawn upon</param>
        internal GraphicsManager(Game game, Platform platform)
        {
            this.game = game;
            this.platform = platform;

            brush = new SolidBrush(Color.Red);
            pen = new Pen(brush);

            formGraphics = platform.Form.CreateGraphics();
        }

        /// <summary>
        /// Gets a default scale
        /// </summary>
        public Vector2 WorldScale { get; private set; } = DefaultScale;

        /// <summary>
        /// Gets or sets a Background Color
        /// </summary>
        public Color BackColor
        {
            get => platform.Form.BackColor;
            set => platform.Form.BackColor = value;
        }

        /// <summary>
        /// Scales an unscaled <see cref="Vector2"/> to World Scale
        /// </summary>
        /// <param name="unscaledVector">Unscaled <see cref="Vector2"/> that will be scaled to World Scale</param>
        /// <returns>New <see cref="Vector2"/> that is now scaled to World Scale</returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "False positive, unscaled is a word.")]
        public Vector2 ToWorldScale(Vector2 unscaledVector)
        {
            return (unscaledVector * WorldScale) / game.Resolution;
        }

        /// <summary>
        /// Scales a World Scaled <see cref="Vector2"/> to Resolution Scale
        /// </summary>
        /// <param name="scaledVector">A world scaled <see cref="Vector2"/></param>
        /// <returns>New <see cref="Vector2"/> that is now scaled to Resolution Scale</returns>
        public Vector2 ToResolutionScale(Vector2 scaledVector)
        {
            return (scaledVector / WorldScale) * game.Resolution;
        }

        /// <summary>
        /// Overload to draw a rectangle using Vector2s instead of resolution coordinates
        /// </summary>
        /// <param name="pos">Center of the rectangle</param>
        /// <param name="size">Size(width,height) of the rectangle</param>
        /// <param name="color">Color to fill the rectangle</param>
        /// <param name="fill">Whether or not to fill the rectangle</param>
        /// <param name="angle">Angle of which to rotate the rectangle</param>
        /// <param name="xAnchor">An anchor point for the X of the rectangle</param>
        /// <param name="yAnchor">An anchor point for the Y of the rectangle</param>
        /// <param name="type">Whether or not to draw on Camera or World</param>
        public void DrawRectangle(Vector2 pos, Vector2 size, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCamera(pos);
                    Vector2 camSize = cam.WorldToCamera(size, true);
                    DrawRectangle(cam.BitmapGraphics, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, color, fill, angle, xAnchor, yAnchor);
                }
            }
            else
            {
                DrawRectangle(bitmapGraphics, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, color, fill, angle, xAnchor, yAnchor);
            }
        }

        /// <summary>
        /// Draw the rectangle using floats instead of integers
        /// </summary>
        /// <param name="x">X component of the center of the rectangle</param>
        /// <param name="y">Y component of the center of the rectangle</param>
        /// <param name="w">Width of the rectangle</param>
        /// <param name="h">Height of the rectangle</param>
        /// <param name="color">Color to fill the rectangle</param>
        /// <param name="fill">Whether or not to fill the rectangle</param>
        /// <param name="angle">Angle of which to rotate the rectangle</param>
        /// <param name="xAnchor">An anchor point for the X of the rectangle</param>
        /// <param name="yAnchor">An anchor point for the Y of the rectangle</param>
        /// <param name="type">Whether or not to draw on Camera or World</param>
        public void DrawRectangle(float x, float y, float w, float h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
            => DrawRectangle(new Vector2(x, y), new Vector2(w, h), color, fill, angle, xAnchor, yAnchor, type);

        /// <summary>
        /// Draw a rectangle by passing a rectangle
        /// </summary>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color for the rectangle</param>
        /// <param name="fill">Whether or not to fill the rectangle</param>
        /// <param name="angle">Angle of which to rotate the rectangle</param>
        /// <param name="xAnchor">An anchor point for the X of the rectangle</param>
        /// <param name="yAnchor">An anchor point for the Y of the rectangle</param>
        /// <param name="type">Whether or not to draw on Camera or World</param>
        public void DrawRectangle(RectangleF rect, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
            => DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, fill, angle, xAnchor, yAnchor, type);

        /// <summary>
        /// Use Vector2 to draw line
        /// </summary>
        /// <param name="v1">Coordinate for one point of the line</param>
        /// <param name="v2">Coordinate for the second point of the line</param>
        /// <param name="color">Color of the line</param>
        /// <param name="type">Whether to draw on World Space or Screen Space</param>
        public void DrawLine(Vector2 v1, Vector2 v2, Color color, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camV1 = cam.WorldToCamera(v1);
                    Vector2 camV2 = cam.WorldToCamera(v2);
                    DrawLine(cam.BitmapGraphics, (int)camV1.X, (int)camV1.Y, (int)camV2.X, (int)camV2.Y, color);
                }
            }
            else
            {
                DrawLine(bitmapGraphics, (int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y, color);
            }
        }

        /// <summary>
        /// Draw a line with floats instead of Vector2
        /// </summary>
        /// <param name="x1">First X</param>
        /// <param name="y1">First Y</param>
        /// <param name="x2">Second X</param>
        /// <param name="y2">Second Y</param>
        /// <param name="color">Color of the line</param>
        /// <param name="type">Whether to draw to Screen Space or Resolution Space</param>
        public void DrawLine(float x1, float y1, float x2, float y2, Color color, DrawType type = DrawType.World)
            => DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, type);

        /// <summary>
        /// Draw an Ellipse using Vector2
        /// </summary>
        /// <param name="pos">(X,Y) coordinate for the Center of Ellipse</param>
        /// <param name="size">Width and Height of the Ellipse</param>
        /// <param name="color">Color of the Ellipse</param>
        /// <param name="fill">Whether or not to fill the Ellipse</param>
        /// <param name="r">Angle to rotate the Ellipse around</param>
        /// <param name="xAnchor">Anchor for X of the Ellipse</param>
        /// <param name="yAnchor">Anchor for Y of the Ellipse</param>
        /// <param name="type">Draw on World or Resolution space</param>
        public void DrawEllipse(Vector2 pos, Vector2 size, Color color, bool fill = true, float r = 0, double xAnchor = 0f, double yAnchor = 0f, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCamera(pos);
                    Vector2 camSize = cam.WorldToCamera(size, true);

                    DrawEllipse(cam.BitmapGraphics, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, color, fill, r, xAnchor, yAnchor);
                }
            }
            else
            {
                DrawEllipse(bitmapGraphics, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, color, fill, r, xAnchor, yAnchor);
            }
        }

        /// <summary>
        /// Draw Ellipse using floats instead of Vector2
        /// </summary>
        /// <param name="x">X component of the Center</param>
        /// <param name="y">Y component of the Center</param>
        /// <param name="w">Width of the Ellipse</param>
        /// <param name="h">Height of the Ellipse</param>
        /// <param name="color">Color of the Ellipse</param>
        /// <param name="fill">Whether or not to fill the Ellipse</param>
        /// <param name="r">Angle to rotate the Ellipse around</param>
        /// <param name="xAnchor">Anchor for X of the Ellipse</param>
        /// <param name="yAnchor">Anchor for Y of the Ellipse</param>
        /// <param name="type">Whether to draw to World or Resolution space</param>
        public void DrawEllipse(float x, float y, float w, float h, Color color, bool fill = true, float r = 0, double xAnchor = 0f, double yAnchor = 0f, DrawType type = DrawType.World)
            => DrawEllipse(new Vector2(x, y), new Vector2(w, h), color, fill, r, xAnchor, yAnchor, type);

        /// <summary>
        /// Draw a circle using a Vector2
        /// </summary>
        /// <param name="pos">Center of the Circle</param>
        /// <param name="r">Radius of the Circle</param>
        /// <param name="color">Color of the Circle</param>
        /// <param name="fill">Whether to fill the Circle or not</param>
        /// <param name="type">Whether to draw to World or Resolution space</param>
        public void DrawCircle(Vector2 pos, float r, Color color, bool fill = true, DrawType type = DrawType.World)
        {
            Vector2 size = new Vector2(r * 2, r * 2);
            pos = new Vector2(pos.X - r, pos.Y - r);

            DrawEllipse(pos, size, color, fill, r, 0.5f, 0.5f, type);
        }

        /// <summary>
        /// Draws a circle using the Ellipse function
        /// </summary>
        /// <param name="x">X component of center of Circle</param>
        /// <param name="y">Y component of center of Circle</param>
        /// <param name="r">Radius of the Circle</param>
        /// <param name="color">Color of the Circle</param>
        /// <param name="fill">Whether to fill the Circle or not</param>
        /// <param name="type">Whether to draw to World or Resolution space</param>
        public void DrawCircle(float x, float y, float r, Color color, bool fill = true, DrawType type = DrawType.World)
            => DrawCircle(new Vector2(x, y), r, color, fill, type);

        /// <summary>
        /// Draws a Bitmap with given Bitmap bmp
        /// </summary>
        /// <param name="bmp">The Bitmap to draw on the screen</param>
        /// <param name="x">X component of origin of Bitmap</param>
        /// <param name="y">Y component of origin of Bitmap</param>
        /// <param name="w">Width of the Bitmap</param>
        /// <param name="h">Height of the Bitmap</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="type">Whether to draw Bitmap onto Camera or Screen space</param>
        public void DrawBMP(Bitmap bmp, float x, float y, float w, float h, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                Vector2 pos = new Vector2(x, y);
                Vector2 size = new Vector2(w, h);
                
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCamera(pos);
                    Vector2 camSize = cam.WorldToCamera(size, true);
                    DrawBMP(cam.BitmapGraphics, bmp, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, r, 0, 0, bmp.Width, bmp.Height);
                }
            }
            else
            {
                DrawBMP(bitmapGraphics, bmp, (int)x, (int)y, (int)w, (int)h, r, 0, 0, bmp.Width, bmp.Height);
            }
        }

        /// <summary>
        /// Draw a Bitmap using Vector2 origin and Vector2 width/height
        /// </summary>
        /// <param name="bmp">Bitmap to draw</param>
        /// <param name="position">Vector2 containing origin (x,y) of the Bitmap</param>
        /// <param name="size">Vector2 containing size (w,h) of the Bitmap</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="type">Whether to draw Bitmap onto Camera or Screen space</param>
        public void DrawBMP(Bitmap bmp, Vector2 position, Vector2 size, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(bmp, position.X, position.Y, size.X, size.Y, r, type);

        /// <summary>
        /// Draw Bitmap using a RectangleF
        /// </summary>
        /// <param name="bmp">Bitmap to draw</param>
        /// <param name="drawRect">Rectangle that should contain the Bitmap</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="type">Whether to draw Bitmap onto Camera or Screen space</param>
        public void DrawBMP(Bitmap bmp, RectangleF drawRect, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(bmp, drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height, r, type);

        /// <summary>
        /// Pulls an Bitmap from the Content Manager to draw
        /// </summary>
        /// <param name="image">Name of image to be pulled from Content Manager</param>
        /// <param name="x">X component of origin of Bitmap</param>
        /// <param name="y">Y component of origin of Bitmap</param>
        /// <param name="w">Width of the Bitmap</param>
        /// <param name="h">Height of the Bitmap</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="type">Whether to draw Bitmap onto Camera or Screen space</param>
        public void DrawBMP(string image, float x, float y, float w, float h, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(game.Content.GetImage(image), x, y, w, h, r, type);

        /// <summary>
        /// Draws a Bitmap using user input coordinates
        /// </summary>
        /// <param name="bmp">Bitmap to draw</param>
        /// <param name="x">X component of center of Bitmap</param>
        /// <param name="y">Y component of center of Bitmap</param>
        /// <param name="w">Width of the Bitmap</param>
        /// <param name="h">Height of the Bitmap</param>
        /// <param name="ix">X of Origin of the Rectangle that the Bitmap needs</param>
        /// <param name="iy">Y of Origin of the Rectangle that the Bitmap needs</param>
        /// <param name="iw">Width of the Rectangle that the Bitmap needs</param>
        /// <param name="ih">Height of the Rectangle that the Bitmap needs</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="type">Whether to draw Bitmap onto Camera or Screen space</param>
        public void DrawBMP(Bitmap bmp, float x, float y, float w, float h, int ix, int iy, int iw, int ih, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                Vector2 pos = new Vector2(x, y);
                Vector2 size = new Vector2(w, h);

                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCamera(pos);
                    Vector2 camSize = cam.WorldToCamera(size, true);
                    DrawBMP(cam.BitmapGraphics, bmp, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, r, ix, iy, iw, ih);
                }
            }
            else
            {
                DrawBMP(bitmapGraphics, bmp, (int)x, (int)y, (int)w, (int)h, r, ix, iy, iw, ih);
            }
        }

        /// <summary>
        /// Draws an Bitmap pulled from the Content Manager
        /// </summary>
        /// <param name="image">Image to be pulled from Content Manager</param>
        /// <param name="x">X component of center of Bitmap</param>
        /// <param name="y">Y component of center of Bitmap</param>
        /// <param name="w">Width of the Bitmap</param>
        /// <param name="h">Height of the Bitmap</param>
        /// <param name="ix">X of Origin of the Rectangle that the Bitmap needs</param>
        /// <param name="iy">Y of Origin of the Rectangle that the Bitmap needs</param>
        /// <param name="iw">Width of the Rectangle that the Bitmap needs</param>
        /// <param name="ih">Height of the Rectangle that the Bitmap needs</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="type">Whether to draw Bitmap onto Camera or Screen space</param>
        public void DrawBMP(string image, float x, float y, float w, float h, int ix, int iy, int iw, int ih, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(game.Content.GetImage(image), x, y, w, h, ix, iy, iw, ih, r, type);

        /// <summary>
        /// Draws a triangle by connecting 3 points together
        /// </summary>
        /// <param name="v1">Vector containing (x,y) components of point 1</param>
        /// <param name="v2">Vector containing (x,y) components of point 2</param>
        /// <param name="v3">Vector containing (x,y) components of point 3</param>
        /// <param name="color">Color to draw the triangle</param>
        /// <param name="fill">Whether to fill the triangle or not</param>
        /// <param name="r">Angle to draw the triangle</param>
        /// <param name="type">Whether to draw onto World or Screen space</param>
        public void DrawTri(Vector2 v1, Vector2 v2, Vector2 v3, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camV1 = cam.WorldToCamera(v1);
                    Vector2 camV2 = cam.WorldToCamera(v2);
                    Vector2 camV3 = cam.WorldToCamera(v3);
                    DrawTri(cam.BitmapGraphics, (int)camV1.X, (int)camV1.Y, (int)camV2.X, (int)camV2.Y, (int)camV3.X, (int)camV3.Y, color, fill, r);
                }
            }
            else
            {
                DrawTri(bitmapGraphics, (int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y, (int)v3.X, (int)v3.Y, color, fill, r);
            }
        }

        /// <summary>
        /// Draws a triangle by passing a pTriangle
        /// </summary>
        /// <param name="tri">Physics triangle to draw</param>
        /// <param name="color">Color to draw the triangle</param>
        /// <param name="fill">Whether or not to fill the triangle</param>
        /// <param name="r">Angle to rotate the triangle</param>
        /// <param name="type">Whether to draw on World or Screen space</param>
        public void DrawTri(PTriangle tri, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
            => DrawTri(tri.VertexOne, tri.VertexTwo, tri.VertexThree, color, fill, r, type);

        /// <summary>
        /// Draws Text
        /// </summary>
        /// <param name="text">String to be drawn</param>
        /// <param name="font">Font to draw the text with</param>
        /// <param name="color">Color to draw the text</param>
        /// <param name="pos">Position to draw the text</param>
        /// <param name="size">Size of text to be drawn</param>
        /// <param name="r">Angle to rotate text</param>
        /// <param name="align">Type of Text Alignment</param>
        /// <param name="type">Whether to draw onto World or Screen space</param>
        public void DrawText(string text, Font font, Color color, Vector2 pos, Vector2 size, float r = 0, TextAlign align = TextAlign.TopLeft, DrawType type = DrawType.World)
        {
            SetColor(color);
            
            Vector2 bmpSize = bitmapGraphics.MeasureString(text, font);
            using (Bitmap textBmp = new Bitmap((int)bmpSize.X, (int)bmpSize.Y))
            {
                using (Graphics textBmpGraphics = Graphics.FromImage(textBmp))
                {
                    textBmpGraphics.DrawString(text, font, brush, 0, 0);
                    DrawBMP(textBmp, GetTextCoordinates(pos, size, align), size, r, type);
                }
            }
        }

        /// <summary>
        /// Modify the World Scale
        /// </summary>
        /// <param name="scaleFactor">Vector2 to be used to reset world scale</param>
        public void SetWorldScale(Vector2 scaleFactor)
        {
            WorldScale = scaleFactor;
        }

        /// <summary>
        /// Draws a polygon using an array of Vector2
        /// </summary>
        /// <param name="vertices">Array of Vector2 containing (x,y) coordinates to be connected to draw Polygon</param>
        /// <param name="color">Color to draw the Polygon</param>
        /// <param name="fill">Whether to fill the Polygon or not</param>
        /// <param name="r">Angle to rotate the Polygon</param>
        /// <param name="type">Whether to draw onto World or Screen space</param>
        public void DrawPolygon(Vector2[] vertices, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
        {
            Point[] points = new Point[vertices.Length];
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = (Point)cam.WorldToCamera(vertices[i]);
                    }

                    DrawPolygon(cam.BitmapGraphics, points, color, fill, r);
                }
            }
            else
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = (Point)vertices[i];
                }

                DrawPolygon(bitmapGraphics, points, color, fill, r);
            }
        }

        /// <summary>
        /// Disposes of Graphics and Create new Graphics
        /// </summary>
        internal void RecreateGraphics()
        {
            formGraphics.Dispose();
            formGraphics = platform.Form.CreateGraphics();
        }

        /// <summary>
        /// Draw onto the buffer from the Camera or just straight onto the buffer
        /// </summary>
        internal void End()
        {
            foreach (Camera cam in GetCameras())
            {
                DrawBMP(cam.Buffer, cam.DrawPosition.X, cam.DrawPosition.Y, cam.DrawSize.X, cam.DrawSize.Y, 0, DrawType.Screen);
                cam.End();
            }

            formGraphics.DrawImage(buffer, 0, 0);
            buffer.Dispose();
            bitmapGraphics.Dispose();
        }

        /// <summary>
        /// Sets the color of the brush or pen
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to set the pen/brush to</param>
        internal void SetColor(Color color)
        {
            brush.Color = color;
            pen?.Dispose();
            pen = new Pen(brush);
        }

        /// <summary>
        /// Rotates a matrix about a center
        /// </summary>
        /// <param name="center"><see cref="PointF"/> that will be considered the center of the matrix</param>
        /// <param name="angle">The angle to rotate about</param>
        internal void Rotate(PointF center, float angle)
        {
            Matrix m = new Matrix();
            m.RotateAt(angle, center);
            bitmapGraphics.Transform = m;
            m.Dispose();
        }

        /// <summary>
        /// Reset the transform of a <see cref="Bitmap"/>
        /// </summary>
        internal void ResetRotation()
        {
            bitmapGraphics.ResetTransform();
        }

        /// <summary>
        /// Create Cameras and create the buffer to draw upon
        /// </summary>
        internal void Begin()
        {
            foreach (Camera cam in GetCameras())
            {
                cam.Begin();
            }

            buffer = new Bitmap(platform.Form.ClientSize.Width, platform.Form.ClientSize.Height, formGraphics);
            bitmapGraphics = Graphics.FromImage(buffer);

            SetColor(BackColor);
            bitmapGraphics.FillRectangle(brush, new Rectangle(0, 0, buffer.Width, buffer.Height));
        }

        /// <summary>
        /// Get the coordinates to draw Text
        /// </summary>
        /// <param name="pos">(x,y) coordinates to draw the Text</param>
        /// <param name="size">Size to draw Text</param>
        /// <param name="align">What type of Text Alignment to use</param>
        /// <returns>Position to draw text</returns>
        private Vector2 GetTextCoordinates(Vector2 pos, Vector2 size, TextAlign align)
        {
            float x = pos.X;
            float y = pos.Y;

            if (align == TextAlign.TopCenter || align == TextAlign.MiddleCenter || align == TextAlign.BottomCenter)
            {
                x = (x - size.X) / 2;
            }
            else if (align == TextAlign.TopRight || align == TextAlign.MiddleRight || align == TextAlign.BottomRight)
            {
                x = x - size.X;
            }

            if (align == TextAlign.MiddleLeft || align == TextAlign.MiddleCenter || align == TextAlign.MiddleRight)
            {
                y = (y - size.Y) / 2;
            }
            else if (align == TextAlign.BottomLeft || align == TextAlign.BottomCenter || align == TextAlign.BottomRight)
            {
                y = y - size.Y;
            }

            return new Vector2(x, y);
        }

        /// <summary>
        /// Get all <see cref="Camera"/> from the Game
        /// </summary>
        /// <returns>Array of <see cref="Camera"/></returns>
        private Camera[] GetCameras()
        {
            if (!game.Cameras.DisplayAll)
            {
                return new Camera[]
                {
                    game.Cameras.Main
                };
            }

            return game.Cameras.All;
        }

        // To draw rectangle at angle:
        // https://stackoverflow.com/questions/10210134/using-a-matrix-to-rotate-rectangles-individually

        /// <summary>
        /// Rectangle draw function to be used for all other overloads
        /// </summary>
        /// <param name="g">The graphics to draw the rectangle on</param>
        /// <param name="x">X component of the center of the rectangle</param>
        /// <param name="y">Y component of the center of the rectangle</param>
        /// <param name="w">Width of the rectangle</param>
        /// <param name="h">Height of the rectangle</param>
        /// <param name="color">Color to fill the rectangle</param>
        /// <param name="fill">Whether or not to fill the rectangle</param>
        /// <param name="angle">Angle of which to rotate the rectangle</param>
        /// <param name="xAnchor">An anchor point for the X of the rectangle</param>
        /// <param name="yAnchor">An anchor point for the Y of the rectangle</param>
        private void DrawRectangle(Graphics g, int x, int y, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            SetColor(color);
            Rotate(new PointF((float)(x + (w * xAnchor)), (float)(y + (h * yAnchor))), angle);

            if (fill)
            {
                g.FillRectangle(brush, new Rectangle(x, y, w, h));
            }
            else
            {
                g.DrawRectangle(pen, new Rectangle(x, y, w, h));
            }

            ResetRotation();
        }

        /// <summary>
        /// Private DrawTriangle function that all other DrawTri functions will use
        /// </summary>
        /// <param name="g">Graphics to draw onto</param>
        /// <param name="x1">X component of point 1</param>
        /// <param name="y1">Y component of point 1</param>
        /// <param name="x2">X component of point 2</param>
        /// <param name="y2">Y component of point 2</param>
        /// <param name="x3">X component of point 3</param>
        /// <param name="y3">Y component of point 3</param>
        /// <param name="color">Color to draw the Triangle</param>
        /// <param name="fill">Whether or not to fill the Triangle</param>
        /// <param name="r">Angle to rotate the Triangle</param>
        private void DrawTri(Graphics g, int x1, int y1, int x2, int y2, int x3, int y3, Color color, bool fill = true, float r = 0)
        {
            SetColor(color);

            if (!fill)
            {
                g.DrawLine(pen, x1, y1, x2, y2);
                g.DrawLine(pen, x2, y2, x3, y3);
                g.DrawLine(pen, x3, y3, x1, y1);
            }
            else
            {
                g.FillPolygon(brush, new Point[] { new Point(x1, y1), new Point(x2, y2), new Point(x3, y3) });
            }
        }

        /// <summary>
        /// Private function to draw Polygons that all other DrawPoly functions will use
        /// </summary>
        /// <param name="g">Graphics to draw Polygon onto</param>
        /// <param name="points">Array of points that need to be connected to draw the Polygon</param>
        /// <param name="color">Color to draw the Polygon</param>
        /// <param name="fill">Whether or not to fill the Polygon</param>
        /// <param name="r">Angle to rotate the Polygon</param>
        private void DrawPolygon(Graphics g, Point[] points, Color color, bool fill = true, float r = 0)
        {
            SetColor(color);

            if (fill)
            {
                g.FillPolygon(brush, points);
            }
            else
            {
                g.DrawPolygon(pen, points);
            }
        }

        /// <summary>
        /// Private function of DrawBMP that is used by all other DrawBMP functions.
        /// </summary>
        /// <param name="g">The graphics to draw to</param>
        /// <param name="bmp">Bitmap to draw</param>
        /// <param name="x">X component of center of Bitmap</param>
        /// <param name="y">Y component of center of Bitmap</param>
        /// <param name="w">Width of the Bitmap</param>
        /// <param name="h">Height of the Bitmap</param>
        /// <param name="r">Angle to rotate the Bitmap</param>
        /// <param name="ix">X of Origin of the Rectangle that the Bitmap needs</param>
        /// <param name="iy">Y of Origin of the Rectangle that the Bitmap needs</param>
        /// <param name="iw">Width of the Rectangle that the Bitmap needs</param>
        /// <param name="ih">Height of the Rectangle that the Bitmap needs</param>
        private void DrawBMP(Graphics g, Bitmap bmp, int x, int y, int w, int h, float r, int ix, int iy, int iw, int ih)
        {
            Rotate(new PointF((x + w) / 2f, (y + h) / 2f), r);

            g.DrawImage(bmp, new Rectangle(x, y, w, h), new Rectangle(ix, iy, iw, ih), GraphicsUnit.Pixel);
            ResetRotation();
        }

        /// <summary>
        /// Ellipse draw function to be used for all other overloads
        /// </summary>
        /// <param name="g">Graphics to draw Ellipse onto</param>
        /// <param name="x">X component of the Center</param>
        /// <param name="y">Y component of the Center</param>
        /// <param name="w">Width of the Ellipse</param>
        /// <param name="h">Height of the Ellipse</param>
        /// <param name="color">Color of the Ellipse</param>
        /// <param name="fill">Whether or not to fill the Ellipse</param>
        /// <param name="r">Angle to rotate the Ellipse around</param>
        /// <param name="xAnchor">Anchor for X of the Ellipse</param>
        /// <param name="yAnchor">Anchor for Y of the Ellipse</param>
        private void DrawEllipse(Graphics g, int x, int y, int w, int h, Color color, bool fill = true, float r = 0, double xAnchor = 0.5f, double yAnchor = 0.5f)
        {
            SetColor(color);
            Rotate(new PointF((float)(x + (w * xAnchor)), (float)(y + (h * yAnchor))), r);

            if (fill)
            {
                g.FillEllipse(brush, x, y, w, h);
            }
            else
            {
                g.DrawEllipse(pen, x, y, w, h);
            }

            ResetRotation();
        }

        /// <summary>
        /// Line draw function to be used for all other overloads
        /// </summary>
        /// <param name="g">Graphics to draw line to</param>
        /// <param name="x1">First X</param>
        /// <param name="y1">First Y</param>
        /// <param name="x2">Second X</param>
        /// <param name="y2">Second Y</param>
        /// <param name="color">Color of the line</param>
        private void DrawLine(Graphics g, int x1, int y1, int x2, int y2, Color color)
        {
            SetColor(color);
            g.DrawLine(pen, x1, y1, x2, y2);
        }
    }
}
