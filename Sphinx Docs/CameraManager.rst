=============
CameraManager
=============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Manages references to all currently active :doc:`Cameras <Camera>`. Cannot be
instantiated directly, instead use :ref:`Game.Cameras <GameCameras>`.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _CameraManagerMain:

:csharp:`Camera Main { get; set; }`

Gets or sets the main :doc:`Camera <Camera>` used by the :doc:`Game <Game>`. If
:ref:`DisplayAll <CameraManagerDisplayAll>` is false, this will be the only
:doc:`Camera <Camera>` displayed.

.. note::
   This property cannot be null unless there are no :doc:`Cameras <Camera>` in
   the CameraManager. Attempting to manually set it to null will have no effect.

----

.. _CameraManagerAll:

:csharp:`Camera[] All { get; }`

Gets an array containing all :doc:`Cameras <Camera>` in the CameraManager. This is
a duplicate of the internal array and is thus safe to modify.

.. warning::
   A new array is created by every call to this property. Limit calls to this by
   caching the returned array to prevent unnecessary garbage creation.

----

.. _CameraManagerDisplayAll:

:csharp:`bool DisplayAll { get; set; }`

Gets or sets a bool indicating whether or not all :doc:`Cameras <Camera>` should be
displayed. If false, only :ref:`Main <CameraManagerMain>` will be displayed. By
default, this is true.

Methods
^^^^^^^

.. _CameraManagerCreate:

.. _CameraManagerCreateExplicitSize:

:csharp:`Camera Create(float x, float y, float w, float h, float drawX, float drawY, float drawW, float drawH)`

Creates a new :doc:`Camera <Camera>` with the given coordinates for position, size,
draw position, and draw size. If there were no :doc:`Cameras <Camera>` prior to this,
the new :doc:`Camera <Camera>` will be registered as :ref:`Main <CameraManagerMain>`.

.. code-block:: csharp
   
   // Create new Camera
   Camera cam = Cameras.Create(0, 100, 128, 72, 0, 0, 1280, 720);
   
   // Print out Camera
   Console.WriteLine(cam);
   
   // Program outputs:
   // {Position: (X: 0, Y: 100), Size: (X: 128, Y: 72, DrawPosition: (X: 0, Y: 0), DrawSize: (X: 1280, Y: 720)}

----

.. _CameraManagerCreateScreenSize:

:csharp:`Camera Create(float x, float y, float w, float h)`

Creates a new :doc:`Camera <Camera>` with the given coordinates for world position
and size. Screen position and size are set such that the :doc:`Camera <Camera>` covers
the full screen. If there were no :doc:`Cameras <Camera>` prior to this, the new
:doc:`Camera <Camera>` will be registered as :ref:`Main <CameraManagerMain>`.

.. code-block:: csharp
   
   // Set resolution and create Camera
   Resolution = new Vector2(1280, 720);
   Camera cam = Cameras.Create(0, 100, 128, 72);
   
   // Print out Camera
   Console.WriteLine(cam);
   
   // Program outputs:
   // {Position: (X: 0, Y: 100), Size: (X: 128, Y: 72, DrawPosition: (X: 0, Y: 0), DrawSize: (X: 1280, Y: 720)}

----

.. _CameraManagerRemoveCamera:

:csharp:`void RemoveCamera(Camera cam)`

Disposes the given :doc:`Camera <Camera>` and removes it from the CameraManager.
Equivalent to calling :ref:`Camera.Dispose <CameraDispose>`.

.. code-block:: csharp
   
   // Setup test
   Resolution = new Vector2(1600, 900);
   Graphics.SetWorldScale(new Vector2(128, 72));
   Camera cam = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
   
   // Dispose Camera, set to null
   Cameras.RemoveCamera(cam);
   cam = null;
   
   // Result: Camera is not used for drawing, despite being created
   // It is also free to be garbage collected since nothing holds a reference to it
