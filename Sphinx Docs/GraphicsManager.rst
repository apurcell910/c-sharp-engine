===============
GraphicsManager
===============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp
   
A basic Graphics Manager used to draw different
shapes as well as Bitmaps and Text. This class also
handles drawing to either World Space or Screen Space.

Properties
^^^^^^^^^^

.. _GraphicsManagerWorldScaleProperty:

:csharp:`Vector2 WorldScale{ get; private set; }`

Gets a default World Scale. World space is a user defined
scale that is allows the user to draw objects to a their own scale
rather than draw using pixel coordinates on the screen.

-----

.. _GraphicsManagerBackColor:

:csharp:`Color BackColor{ get; set; }`

Gets the background color and sets the background color.

-----

Method
^^^^^^

.. _GraphicsManagerToWorldScale:

:csharp:`Vector2 ToWorldScale(Vector2 unscaledVector)`

Converts a Vector2 from Screen coordinates to World coordinates.
Returns the newly scaled Vector2.

-----

.. _GraphicsManagerToResolutionScale:

:csharp:`Vector2 ToResolutionScale(Vector2 scaledVector)`

Converts a Vector2 from World Scaled coordinates to Resolution coordinates.
Returns the newly unscaled Vector2.

-----

.. _GraphicsManagerDrawRectangleDefault:

:csharp:`void DrawRectangle(Vector2 pos, Vector2 size, Color color, bool fill, float angle, double xAnchor, double yAnchor, DrawType type)`

Draws a rectangle on the screen with the given (X,Y) position given by Vector2 pos and size (Width, Height) given by Vector2 size. 
The color of the rectangle can be set with color and the rectangle can be filled by setting the argument bool fill to true. The 
rectangle can be rotated by setting float angle and the anchor of rotation is set to xAnchor and yAnchor. The rectangle can be 
drawn to either World Space or Screen Space by setting the argument DrawType type to World or Screen.

.. code-block:: csharp
   
   //Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this);
   
   //Creates a Red rectangle 
   Vector2 center = new Vector2(100,100);
   Vector2 size = new Vector2(200, 300);
   graphics.DrawRectangle(center, size, Color.Red, true, 0, 0, 0, DrawType.World);

----

.. _GraphicsManagerDrawRectangleFloats:

:csharp:`void DrawRectangle(float x, float y, float w, float h, Color color, bool fill, float angle, double xAnchor, double yAnchor, DrawType type)`

Same functionality as :ref:`Float Draw Rectangle<GraphicsManagerDrawRectangleDefault>` except floats are used 
to set (X,Y) coordinates and (Width, Height) parameters.

----

.. _GraphicsManagerDrawRectangleF:

:csharp:`void DrawRectangle(RectangleF rect, Color color, bool fill, float angle, double xAnchor, double yAnchor, DrawType type)`

Same functionality as :ref:`Float Draw Rectangle<GraphicsManagerDrawRectangleDefault>` except a RectangleF is 
used instead of (X,Y) coordinates and (Width, Height) parameters.

----

.. _GraphicsManagerDrawLineDefault:

:csharp:`void DrawLine(Vector2 v1, Vector2 v2, Color color, DrawType type)`

Draws a line by connecting two points, Vector2 v1 and Vector2 v2. Color of the line is set with Color color
and the line can be drawn onto either World space or Screen Space by setting DrawType type to either World
or Screen.

.. code-block:: csharp
   
   //Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this);
   
   //Create a line
   Vector2 p1 = new Vector2(0, 100);
   Vector2 p2 = new Vector2(100, 0);
   graphics.DrawLine(p1, p2, Color.MediumAquaMarine, DrawType.World);

----

.. _GraphicsManagerDrawLineFloat:

:csharp:`void DrawLine(float x1, float y1, float x2, float x2, Color color, DrawType type)`

Same functionality as :ref:`Float DrawLine<GraphicsManagerDrawLineDefault>` except floats are used to set the (X,Y) coordinates of both sets of points
rather than Vector2.

----

.. _GraphicsManagerDrawEllipseDefault:

:csharp:`void DrawEllipse(Vector2 pos, Vector2 size, Color color, bool fill, float r, double xAnchor, double yAnchor, DrawType type)`

Draws an ellipse with (Width, Height) given by Vector2 size and origin (X,Y) Vector2 pos. The ellipse can be 
filled with Color color by setting bool fill to true. The ellipse can be rotated by setting float r with 
the anchor of the rotation set with xAnchor and yAnchor. The ellipse can be drawn onto either World or 
Screen space by setting the argument DrawType type to either World or Screen.

.. code-block:: csharp
   
   //Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this);
   
   Vector2 pos = new Vector2(100,100);
   Vector2 size = new Vector2(100,150);
   
   graphics.DrawEllipse(pos, size, Color.Red, true, 0, 0, 0, DrawType.World);

----

.. _GraphicsManagerDrawEllipseFloat:

:csharp:`void DrawEllipse(float x, float y, float w, float h, Color color, bool fill, float r, double xAnchor, double yAnchor, DrawType type)`

Same functionality as :ref:`Float DrawEllipse<GraphicsManagerDrawEllipseDefault>` except floats are used to set the (X,Y) and (Width, Height);

-----

.. _GraphicsManagerDrawCircleDefault:

:csharp:`void DrawCircle(Vector2 pos, float r, Color color, bool fill, DrawType type)`

Draws a circle with origin (X,Y) of Vector2 pos with radius float r. The circle is drawn with Color color and can 
be filled with this color by setting bool fill to true. The circle can be drawn onto either World or Screen space
by setting the argument DrawType type to either World or Screen.

.. code-block:: csharp
   
   //Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this);
   
   Vector2 center = new Vector2(100,100);
   float radius = 50f;
   
   //Draws a red circle with origin (100,100) and radius 50 onto World Space
   graphics.DrawCircle(center, radius, Color.blanchedAlmond, DrawType.World);

-----

.. _GraphicsManagerDrawCircleFloats:

:csharp:`void DrawCircle(float x, float y, float r, Color color, bool fill, DrawType type)`

Same functionality as :ref:`Float DrawCircle<GraphicsManagerDrawCircleDefault>` except floats are used to set the (X,Y) origin.

-----

.. _GraphicsManagerDrawBMPFloatBMP:

:csharp:`void DrawBMP(Bitmap bmp, float x, float y, float w, float h, float r, DrawType type)`

Draws a Bitmap image onto a buffer that will be displayed onto the screen by passing in a existing bitmap.
Float x and Float y are used as the (X,Y) coordinates of the origin of the image. Float w and float h 
are used as the (Width, Height) of the image to be drawn. Float r is used to rotate the image when set. 
The Bitmap can be drawn onto either World or Screenspace by setting the argument DrawType type to 
either World or Screen.

.. code-block:: csharp

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this);
   
   
   Bitmap bmp = new Bitmap("example.png");
   float x = 50;
   float y = 50;
   float w = 50;
   float h = 50;
   
   
   // Draws a Bitmap with origin (x,y) and size of (w, h) onto the World Space
   graphics.DrawBMP(bmp, x, y, w, h, 0, DrawType.World);
   
-----

.. _GraphicsManagerDrawBMPVector2BMP:

:csharp:`void DrawBMP(Bitmap bmp, Vector2 position, Vector2 size, float r, DrawType type)`

Same functionality as :ref:`Vector2 DrawBMP<GraphicsManagerDrawBMPFloatBMP>` except uses Vector2 to set (x,y) origin and (width,height) parameters.

-----

.. _GraphicsManagerDrawBMPRectF:

Same functionality as :ref:`Vector2 DrawBMP<GraphicsManagerDrawBMPFloatBMP>` except uses Rectangle to set both (x,y) and (width, height) parameters 
by setting the origin and size of the Bitmap equal to the origin and size of the Rectangle that is passed in.

-----

.. _GraphicsManagerDrawBMPFloatString:

:csharp:`void DrawBMP(string image, float x, float y, float w, float h, float r, DrawType type)`

Draws a Bitmap image onto a buffer that will be displayed onto the screen by passing in a string name of 
a Bitmap that exists within the Content Manager. Float x and Float y are used as the (X,Y) coordinates of 
the origin of the image. Float w and float h are used as the (Width, Height) of the image to be drawn. 
Float r is used to rotate the image when set. The Bitmap can be drawn onto either World or Screen space 
by setting the argument DrawType type to either World or Screen.

.. code-block:: csharp
   
   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   ContentManager content = new ContentManager(this);
   
   // Add an external image into the Content Manager
   content.AddImage("test.png", "test");
   float x = 50;
   float y = 50;
   float w = 50;
   float h = 50;
   
   // Draws the "test.png" picture that is pulled from the Content Manager
   graphics.DrawBMP("test", x, y, w, h, 0, DrawType.World);
   
-----

.. _GraphicsManagerDrawBMPSubRegionBMP:

:csharp:`void DrawBMP(Bitmap bmp, float x, float y, float w, float h, int ix, int iy, int iw, int ih, float r, DrawType type)`

Draws a subregion of a Bitmap onto a buffer that will be drawn onto the screen. Bitmap bmp is the image to be drawn
float x and float y are the (x,y) coordinates of the origin of the image and float w and float h are the (width, height)
parameters of the Bitmap bmp. Int ix and int iy are used as the origin of the subregion to be drawn and int iw and ih are
the (width, height) of the subregion. Float r is used to set rotation speed. The subregion of the Bitmap can be drawn onto 
either World or Screen space by setting the argument DrawType type to either World or Screen.

.. code-block:: csharp

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   
   Bitmap bmp = new Bitmap("example.png");
   
   // Size of the Bitmap 
   float x = 100;
   float y = 100;
   float w = 100;
   float h = 100;
   
   // Size of subregion to draw
   int ix = 50;
   int iy = 50;
   int iw = 50;
   int ih = 50;
   
   // Draws the subregion of the Bitmap onto the screen.
   graphics.DrawBMP(bmp, x, y, w, h, ix, iy, iw, ih, 0, DrawType.World);
   
-----

.. _GraphicsManagerDrawBMPSubRegionString:

:csharp:`void DrawBMP(string file, float x, float y, float w, float h, int ix, int iy, int iw, int ih, float r, DrawType type)`

Draws a subregion of a Bitmap image onto a buffer that will be displayed onto the screen by passing in a 
string name of a Bitmap that exists within the Content Manager. Float x and Float y are used as the (X,Y)
coordinates of the origin of the image. Float w and float h are used as the (Width, Height) of the image
to be drawn. Int ix and int iy are the (x,y) origin of the subregion to draw and int iw and int ih are 
the (width, height) of the subregion. Float r is used to rotate the image when set. The Bitmap can be 
drawn onto either World or Screen space by setting the argument DrawType type to either World or Screen.

.. code-block:: csharp

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   ContentManager content = new ContentManager(this);
   
   content.AddImage("example.png", "example");
   
   // Size of the Bitmap 
   float x = 100;
   float y = 100;
   float w = 100;
   float h = 100;
   
   // Size of subregion to draw
   int ix = 50;
   int iy = 50;
   int iw = 50;
   int ih = 50;
   
   // Draws the subregion of the Bitmap onto the screen.
   graphics.DrawBMP(string, x, y, w, h, ix, iy, iw, ih, 0, DrawType.World);
   
-----

.. _GraphicsManagerDrawTriVector2:

:csharp:`void DrawTri(Vector2 v1, Vector2 v2, Vector2 v3, Color color, bool fill, float r, DrawType type)`

Draws a triangle by connecting three Vector2 points together and drawing lines between them. Color color is used 
to set the color of the triangle and bool float is used to either fill the triangle or not. Float r is used 
to set the rotation of the triangle. The triangle can be drawn onto either World or Screen space by setting
the argument DrawType type to either World or Screen.

.. code-block:: csharp

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   
   // Create the Vector2 points to be connected
   Vector2 p1 = new Vector2(100,100);
   Vector2 p2 = new Vector2(200,200);
   Vector2 p3 = new Vector2(300,300);
   
   graphics.DrawTri(p1, p2, p3, Color.Red, true, 0, DrawType.World);
   
-----

.. _GraphicsManagerDrawTriPTri:

:csharp:`void DrawTri(pTriangle tri, Color color, bool fill, float r, DrawType type)`

Same functionality as :ref:`Vector2 DrawTri<GraphicsManagerDrawTriVector2>` except uses a physics Triangle, pTriangle, instead of 3
Vector2 arguments to draw the triangle.

-----

.. _GraphicsManagerDrawText:

:csharp:`void DrawText(string text, Font font, Color color, Vector2 pos, Vector2 size, float r, TextAlign align, DrawType type)`

Draws text onto the screen. The text to be drawn is string text with Font font. The color 
is set with Color color. The origin of the text's alignment is Vector2 size. The size of 
the text is set with Vector2 size. TextAllign is used to set the text's alignment. The 
text can be drawn onto either World or Screen space by setting the argument DrawType 
type to either World or Screen.

.. code-block:: csharp

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   ContentManager content = new ContentManager(this);
   
   content.AddFont("example.ttf", "example");
   
   Vector2 pos = new Vector2(100,100);
   Vector2 size = new Vector2(100,100);
   
   
   graphics.DrawText("example", GetFont("example", 32), pos, size, 0, TextAlign.TopRight, DrawType.World);

-----

.. _GraphicsManagerDrawPolygon:

:csharp:`void DrawPolygon(Vector2[] vertices, Color color, bool fill, float r, DrawType type)`

Draws a polygon by connecting an array of Points, Vector2[] vertices, together. The color
of the polygon is set with Color color and can be filled by setting bool fill to true. Rotation
can be set by setting float r. The polygon can be drawn onto either World or Screen space 
by setting the argument DrawType type to either World or Screen.


.. code-block:: csharp

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   
   int num_verts = 5;
   Vector2 vertices[num_verts];
   
   for(int i = 0; i < num_verts; i++)
   {
		verticies[i] = new Vector2(100 * i, 50 * i);
   }
   
   graphics.DrawPolygon(vertices, Color.Red, false, 0, DrawType.World);
   
-----

.. _GraphicsManagerSetWorldScale:

:csharp:`void SetWorldScale(Vector2 scaleFactor)`

   // Create a GraphicsManager by giving it a reference to the game.
   GraphicsManager graphics = new GraphicsManager(this); 
   
   // Sets a new world scale so that the X axis is 50 units and Y axis is 100 units
   Vector2 newWorldScale = new Vector2(50, 100);
   graphics.SetWorldScale(newWorldScale);
   
----

Enums
^^^^^

.. _GraphicsManagerDrawType:

:csharp:`enum DrawType`

DrawType is an enum that tells each of the Draw functions whether or not 
to draw to World or Screen space. Contains Screen and World to draw to
World or Screen space.

-----

.. _GraphicsManagerTextAlign:

:csharp:`enum TextAlign`

TextAlign is used by the :ref:`DrawText<GraphicsManagerDrawText>`. This Enum tells
the function to use TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight,
BottomLeft, BottomCenter, BottomRight alignment. 

-----