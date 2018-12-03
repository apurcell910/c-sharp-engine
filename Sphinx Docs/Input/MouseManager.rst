============
MouseManager
============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Offers functionality for checking the state of the locations and buttons on the mouse. This 
class cannot be instantiated directly, instead use :ref:`Game.Mouse <GameMouse>`.

Events
^^^^^^

.. _MouseManagerBroadcastLocation:

:csharp:`delegate void BroadcastLocation(Vector2 Location)`

The delegate type used for broadcasting a location.

----

.. _MouseManagerMouseclick:

:csharp:`delegate void MouseClick()`

The delegate type used for all click events.

----

.. _MouseManagerLeftClick:

:csharp:`event MouseClickLeftClick`

Called when left mouse button is clicked.

----

.. _MouseManagerRightClick:

:csharp:`event MouseClick RightClick`

Called when right mouse button is clicked.

----

.. _MouseManagerMiddleClick:

:csharp:`event MouseClick MiddleClick`

Called when middle mouse button is clicked.

----

.. _MouseManagerBroadcast:

:csharp:`event BroadcastLocation Broadcast`

Called to broadcast the current mouse location.

Methods
^^^^^^^

.. _MouseManagerAddLeftClick:

:csharp:`void AddLeftClick(Event e)`

Adds an event to trigger on left click.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerAddRightClick:

:csharp:`void AddRightClick(Event e)`

Adds an event to trigger on right click.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerAddMiddleClick:

:csharp:`void AddMiddleClick(Event e)`

Adds an event to trigger on middle click.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerAddLocationBind:

:csharp:`void AddLocationBind(Event e)`

Adds an event to get sent current mouse location.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerRemoveLeftClick:

:csharp:`void RemoveLeftClick(Event e)`

Removes an event to trigger on left click.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerRemoveRightClick:

:csharp:`void RemoveRightClick(Event e)`

Removes an event to trigger on right click.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerRemoveMiddleClick:

:csharp:`void RemoveMiddleClick(Event e)`

Removes an event to trigger on middle click.

.. warning::
   Event class currently not functioning.

----

.. _MouseManagerRemoveLocationBind:

:csharp:`void RemoveLocationBind(Event e)`

Removes an event to get sent current mouse location.

.. warning::
   Event class currently not functioning.