==============
GameController
==============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

An abstract class containing information about a specific controller connected to the
system. Contains limited functionality, check the inherited type of a GameController
for more controller specific use.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _GameControllerType:

:csharp:`ControllerType Type { get; }`

Gets the :ref:`ControllerType <GameControllerControllerType>` of the GameController.

----

.. _GameControllerIsConnected:

:csharp:`bool IsConnected { get; }`

Get a bool indicating whether the GameController is currently connected to the system.

Enums
^^^^^

.. _GameControllerControllerType:

.. code-block:: csharp
   
   enum ControllerType
   {
      Unknown,
      Xbox,
      Xbox360,
      XboxOneS,
      Playstation3,
      Playstation4
   }
