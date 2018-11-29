====
Game
====

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

The main class through which all functionality of the framework is accessed. 
Additionally provides Update/Draw loop through which to create a game. This class is
abstract and must be overridden in order to be instantiated.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _GameControllers:

:csharp:`DeviceManager Controllers { get; }`

Gets the :doc:`DeviceManager <Input/DeviceManager>` containing the Game's
:doc:`GameControllers <Input/GameController>`.

----

.. _GameKeyboard:

:csharp:`KeyboardManager Keyboard { get; }`

Gets the :doc:`KeyboardManager <Input/KeyboardManager>` that tracks keyboard input for
the Game.

----

.. _GameMouse:

:csharp:`MouseManager Mouse { get; }`

Gets the :doc:`MouseManager <Input/MouseManager>` that tracks mouse input for the Game.

----

.. _GameCameras:

:csharp:`CameraManager Cameras { get; }`

Gets the :doc:`CameraManager <CameraManager>` that contains all of the Game's
:doc:`Cameras <Camera>`.

----

.. _GameSprites:

:csharp:`SpriteList Sprites { get; }`

Gets the :doc:`SpriteList <Sprite/SpriteList>` that manages all of the Game's
:doc:`Sprites <Sprite/Sprite>`.

----

.. _GameActions:

:csharp:`SpriteEvents Actions { get; }`

Gets the :doc:`SpriteEvents <Sprite/SpriteEvents>` that manages
:doc:`Sprite <Sprite/Sprite>` events for the Game.

----

.. _GameGraphics:

:csharp:`GraphicsManager Graphics { get; }`

Gets the :doc:`GraphicsManager <GraphicsManager>` that handles all drawing for the
Game.

----

.. _GameContent:

:csharp:`ContentManager Content { get; }`

Gets the :doc:`ContentManager <ContentManager>` that handles loading of images,
sounds, fonts, etc for the Game.

----

.. _GameVsync:

:csharp:`bool Vsync { get; protected set; }`

Gets or sets a bool indicating if the Game should lock frame rate to the refresh rate
of the current monitor. Overrides :ref:`TargetFramerate <GameTargetFramerate>` if 
applicable. By default, this value is false.

.. warning::
   Not yet implemented. The Game will always run at uncapped frame rate.

----

.. _GameTargetFramerate:

:csharp:`int TargetFramerate { get; protected set; }`

Gets or sets the frame rate the Game will attempt to reach (but not exceed). Set to -1
for no target. Overridden by :ref:`Vsync <GameVsync>` if applicable. By default, there
is no target framerate.

.. warning::
   Not yet implemented. The Game will always run at uncapped frame rate.

----

.. _GameFixedTimestep:

:csharp:`bool FixedTimestep { get; protected set; }`

Gets or sets a bool indicating if the Game will always assume the
:ref:`TargetFramerate <GameTargetFramerate>` is being met. By default, this value is
false.

.. warning::
   Not yet implemented. The Game will always calculate the actual time step.

----

.. _GameResolution:

:csharp:`Vector2 Resolution { get; set; }`

Gets or sets the resolution of the Game window. By default, this value is 1280x720.

.. note::
   All of the Game's :doc:`Cameras <Camera>` will be automatically resized
   proportional to the new resolution when this is set.

----

.. _GameShowCursor:

:csharp:`bool ShowCursor { get; set; }`

Gets or sets a bool indicating whether or not to show the cursor when hovering
over the Game window. By default, this value is true.

----

.. _GameLockCursor:

:csharp:`bool LockCursor { get; set; }`

Gets or sets a bool indicating whether or not to lock the mouse cursor inside of the
Game window. By default, this value is false.

Methods
^^^^^^^

.. _GameRun:

:csharp:`void Run()`

Sets up the Game class and begins the main loop of :ref:`Update <GameUpdate>` and
:ref:`Draw <GameDraw>`.

.. code-block:: csharp
   
   // Create an instance of a Game
   Game game = new MyGame();
   
   // Begin running the Game
   game.Run();

.. warning::
   This call is blocking and will not return until the Game has been closed.

----

.. _GameInitialize:

:csharp:`virtual void Initialize()`

Optionally override in order to change settings or set up other Game variables. Called
directly before :ref:`LoadContent <GameLoadContent>`.

----

.. _GameLoadContent:

:csharp:`virtual void LoadContent()`

Optionally override in order to load from :ref:`Content <GameContent>`. Called after
:ref:`Initialize <GameInitialize>` and before beginning the main loop.

----

.. _GameUpdate:

:csharp:`abstract void Update(GameTime gameTime)`

The main update loop of the Game. Called before :ref:`Draw <GameDraw>` on every frame.
Parameter "gameTime" is a :doc:`GameTime <GameTime>` struct containing delta time
information for this frame.

----

.. _GameDraw:

:csharp:`abstract void Draw(GameTime gameTime)`

The main draw loop of the Game. Called before :ref:`Update <GameUpdate>` on every
frame. Parameter "gameTime" is a :doc:`GameTime <GameTime>` struct containing delta
time information for this frame.

----

.. _GameAddUpdatable:

:csharp:`void AddUpdatable(IUpdatable updatable)`

Registers an :doc:`IUpdatable <IUpdatable>` to receive update calls from the Game.
:ref:`updatable.Update <IUpdatableUpdate>` is called directly before the call to
the Game's :ref:`Update <GameUpdate>` on every frame. If
:ref:`updatable.Alive <IUpdatableAlive>` returns false, the add call will silently
fail.

----

.. _GameRemoveUpdatable:

:csharp:`void RemoveUpdatable(IUpdatable updatable)`

Deregisters an :doc:`IUpdatable <IUpdatable>` so that it will no longer receive update
calls from the Game.

.. code-block:: csharp
   
   // Create IUpdatable and register it with the Game
   IUpdatable updatable = new MyUpdatable();
   AddUpdatable(updatable);
   
   // The new instance of MyUpdatable will now receive updates from the Game indefinitely
   
   RemoveUpdatable(updatable);
   
   // The instance of MyUpdatable will no longer receive updates

----

.. _GameAddDrawable:

:csharp:`void AddDrawable(IDrawable drawable)`

Registers an :doc:`IDrawable <IDrawable>` to receive draw calls from the Game.
:ref:`drawable.Draw <IDrawableDraw>` is called directly before the call to the Game's
:ref:`Draw <GameDraw>` on every frame. If :ref:`drawable.Alive <IDrawableAlive>`
returns false, the add call will silently fail.

----

.. _GameRemoveDrawable:

:csharp:`void RemoveDrawable(IDrawable drawable)`

Deregisters an :doc:`IDrawable <IDrawable>` so that it will no longer receive draw
calls from the Game.

.. code-block:: csharp
   
   // Create IDrawable and register it with the Game
   IDrawable drawable = new MyDrawable();
   AddDrawable(drawable);
   
   // The new instance of MyDrawable will now receive draw calls from the Game indefinitely
   
   RemoveDrawable(drawable);
   
   // The instance of MyDrawable will no longer receive draw calls
