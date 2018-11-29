======
Camera
======

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A basic class used by the :doc:`GraphicsManager <GraphicsManager>` to display
world objects. Cannot be instantiated directly, instead use
:ref:`CameraManager.Create <CameraManagerCreate>`.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _CameraPosition:

:csharp:`Vector2 Position { get; set; }`

Gets or sets a :doc:`Vector2 <../Vector2>` containing the coordinates of the Camera in
world space.

----

.. _CameraSize:

:csharp:`Vector2 Size { get; set; }`

Gets or sets a :doc:`Vector2 <../Vector2>` containing the width and height of the
Camera in world space.

----

.. _CameraX:

:csharp:`float X { get; set; }`

Gets or sets the x coordinate of the Camera in world space. This is simply a shortcut
to :ref:`Position <CameraPosition>`.X.

----

.. _CameraY:

:csharp:`float Y { get; set; }`

Gets or sets the y coordinate of the Camera in world space. This is simply a shortcut
to :ref:`Position <CameraPosition>`.Y.

----

.. _CameraWidth:

:csharp:`float Width { get; set; }`

Gets or sets the width of the Camera in world space. This is simply a shortcut to
:ref:`Size <CameraSize>`.X.

----

.. _CameraHeight:

:csharp:`float Height { get; set; }`

Gets or sets the height of the Camera in world space. This is simply a shortcut to
:ref:`Size <CameraSize>`.Y.

----

.. _CameraDrawPosition:

:csharp:`Vector2 DrawPosition { get; set; }`

Gets or sets the position the Camera is drawn at in screen space.

----

.. _CameraDrawSize:

:csharp:`Vector2 DrawSize { get; set; }`

Gets or sets the draw size of the Camera in pixels.

----

.. _CameraDrawX:

:csharp:`int DrawX { get; set; }`

Gets or sets the x coordinate the Camera is drawn at in pixels. This is simply a
shortcut to :ref:`DrawPosition <CameraDrawPosition>`.X.

----

.. _CameraDrawY:

:csharp:`int DrawY { get; set; }`

Gets or sets the y coordinate the Camera is drawn at in pixels. This is simply a
shortcut to :ref:`DrawPosition <CameraDrawPosition>`.Y.

----

.. _CameraDrawWidth:

:csharp:`int DrawWidth { get; set; }`

Gets or sets the width of the camera draw area in pixels. This is simply a
shortcut to :ref:`DrawPosition <CameraDrawSize>`.X.

----

.. _CameraDrawHeight:

:csharp:`int DrawHeight { get; set; }`

Gets or sets the height of the camera draw area in pixels. This is simply a
shortcut to :ref:`DrawPosition <CameraDrawHeight>`.Y.

Methods
^^^^^^^

.. _CameraCameraToWorld:

:csharp:`Vector2 CameraToWorld(Vector2 cameraCoord, bool ignorePos = false)`

Converts a pixel position on the Camera into world coordinates. If ignorePos is true,
the Camera's position will not be used in the scaling of the given
:ref:`Vector2 <../Vector2>`. This is useful for scaling sizes rather than positions.

.. code-block:: csharp
   
   // Setup test
   Resolution = new Vector2(1600, 900);
   Graphics.SetWorldScale(new Vector2(128, 72));
   Camera cam = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
   
   // Convert coordinate 750, 750 to world space
   Console.WriteLine(cam.CameraToWorld(new Vector2(750, 750)));
   
   // Program outputs:
   // (X: 60, Y: 60)

----

.. _CameraWorldToCamera:

:csharp:`Vector2 WorldToCamera(Vector2 worldCoord, bool ignorePos = false)`

Converts a world coordinate position into a pixel position on the Camera. If ignorePos
is true, the Camera's position will not be used in the scaling of the given
:ref:`Vector2 <../Vector2>`. This is useful for scaling sizes rather than positions.

.. code-block:: csharp
   
   // Setup test
   Resolution = new Vector2(1600, 900);
   Graphics.SetWorldScale(new Vector2(128, 72));
   Camera cam = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
   
   // Convert coordinate 60, 60 to Camera pixels
   Console.WriteLine(cam.WorldToCamera(new Vector2(60, 60)));
   
   // Program outputs:
   // (X: 750, Y: 750)

----

.. _CameraToString:

:csharp:`string ToString()`

Returns a string containing information about the Camera's world and screen positions.

.. code-block:: csharp
   
   // Setup test
   Resolution = new Vector2(1600, 900);
   Graphics.SetWorldScale(new Vector2(128, 72));
   Camera cam = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
   
   // Display Camera information
   Console.WriteLine(cam);
   
   // Program outputs:
   // {Position: (X: 0, Y: 0), Size: (X: 128, Y: 72, DrawPosition: (X: 0, Y: 0), DrawSize: (X: 1600, Y: 900)}

----

.. _CameraDispose:

:csharp:`void Dispose()`

Disposes the Camera's internal buffers and removes it from the parent
:doc:`CameraManager <CameraManager>`. Equivalent to calling
:ref:`CameraManager.RemoveCamera <CameraManagerRemoveCamera>`

.. code-block:: csharp
   
   // Setup test
   Resolution = new Vector2(1600, 900);
   Graphics.SetWorldScale(new Vector2(128, 72));
   Camera cam = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
   
   // Dispose Camera, set to null
   cam.Dispose();
   cam = null;
   
   // Result: Camera is not used for drawing, despite being created
   // It is also free to be garbage collected since nothing holds a reference to it
