==========
IUpdatable
==========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A simple interface that allows any class to receive updates
from the main loop. Register objects using this interface to
receive updates via :ref:`Game.AddUpdatable <GameAddUpdatable>`,
and deregister them via :ref:`Game.RemoveUpdatable <GameRemoveUpdatable>`

Properties
^^^^^^^^^^

.. _IUpdatableAlive:

:csharp:`bool Alive { get; }`

Gets a value indicating whether the object should continue receiving
updates. If false, the parent :doc:`Game <Game>` will automatically
remove the IUpdatable from its update loop. Returning true again will
not add the IUpdatable back, :ref:`Game.AddUpdatable <GameAddUpdatable>`
must be used.

Methods
^^^^^^^

.. _IUpdatableUpdate:

:csharp:`void Update(GameTime gameTime)`

Called on every iteration of the main loop by the parent
:doc:`Game <Game>`. See :doc:`GameTime <GameTime>` for information
regarding the parameter "gameTime".