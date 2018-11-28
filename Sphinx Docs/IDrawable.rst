==========
IDrawable
==========

.. role:: csharp(code)
   :language: csharp

A simple interface that allows any class to receive draw calls
from the main loop. Register objects using this interface to
receive draw calls via :ref:`Game.AddDrawable <GameAddDrawable>`,
and deregister them via :ref:`Game.RemoveDrawable <GameRemoveDrawable>`

Properties
^^^^^^^^^^

.. _IDrawableAlive:

:csharp:`bool Alive { get; }`

Gets a value indicating whether the object should continue receiving
draw calls. If false, the parent :doc:`Game <Game>` will automatically
remove the IDrawable from its draw loop. Returning true again will
not add the IDrawable back, :ref:`Game.AddDrawable <GameAddDrawable>`
must be used.

Methods
^^^^^^^

.. _IDrawableDraw:

:csharp:`void Draw(GameTime gameTime)`

Called on every iteration of the main loop by the parent
:doc:`Game <Game>`. See :doc:`GameTime <GameTime>` for information
regarding the parameter "gameTime".