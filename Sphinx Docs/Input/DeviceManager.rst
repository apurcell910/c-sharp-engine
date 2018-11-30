=============
DeviceManager
=============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A simple class containing all of the :doc:`GameControllers <GameController>` used by
the Game. Cannot be instantiated directly, instead use
:ref:`Game.Controllers <GameControllers>`.

Events
^^^^^^

.. _DeviceManagerNewController:

:csharp:`delegate void NewController(GameController controller)`

The delegate type used by :ref:`ControllerAdded <DeviceManagerControllerAdded>`.
Contains the newly connected :doc:`GameController <GameController>` as a parameter.

----

.. _DeviceManagerControllerAdded:

:csharp:`event NewController ControllerAdded`

Called when a new :doc:`GameController <GameController>` is connected to the system.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _DeviceManagerControllers:

:csharp:`GameController[] Controllers { get; }`

Gets a :doc:`GameController <GameController>` array containing all of the controllers
managed by this DeviceManager. This array is a duplicate of the internal array, and is
thus safe to modify.

.. warning::
   A new array is created by every call to this property. Limit calls to
   this by caching the returned array to prevent unnecessary garbage
   creation.

----

.. _DeviceManagerXboxControllers:

:csharp:`XboxController[] XboxControllers { get; }`

Gets an :doc:`XboxController <XboxController>` array containing all of the xbox
controllers managed by this DeviceManager. This array is a duplicate of the internal 
array, and is thus safe to modify.

.. warning::
   A new array is created by every call to this property. Limit calls to
   this by caching the returned array to prevent unnecessary garbage
   creation.
