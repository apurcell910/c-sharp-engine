============
SEvent
============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A class that is used to store actions that sprites will take. 

.. warning::
   Currently not functioning. Please manually add in actions for sprites to take.

Events
^^^^^^

.. _SEventSpriteAction:

:csharp:`delegate void SpriteAction()`

The delegate type used for storing an action.

----

.. _SEventChange:

:csharp:`event SpriteAction Change`

Called when :ref:`Call <SEventCall>` is called.

Methods
^^^^^^^^

.. _SEventCall:

:csharp:`void Call()`

Calls :ref:`Change <SEventChange>`.

----

.. _SEventEnable:

:csharp:`void Enable()`

Enables the SEvent so that it will be called.

----

.. _SEventDisable:

:csharp:`void Disable()`

Disables the SEvent so that it won't be called.

----

.. _SEventSwap:

:csharp:`void Swap()`

If the SEvent is enabled, disables it.
If it is disabled, enables it.