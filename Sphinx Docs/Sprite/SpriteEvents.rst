============
SpriteEvents
============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A basic manager for the sprite class that can be used to add in actions to various sprites. Automatically created by the :doc:`Game <../Game>` class, should not be manually created by the user.

.. warning::
   Currently not functioning. Please manually add in actions for sprites to take.

Methods
^^^^^^^^

.. _SpriteEventsAdd:

:csharp:`void Add(string s, SEvent e)`

Adds a new :doc:`SEvent <SEvent>` to the Events List, using a string to reference it for later use from anywhere.

----

.. _SpriteEventsRemove:

:csharp:`void Delete(string s)`

Deletes the indicated :doc:`SEvent <SEvent>` from the events list.

----

.. _SpriteEventsEnable:

:csharp:`void Enable(string s)`

Enables the indicated :doc:`SEvent <SEvent>`.

----

.. _SpriteEventsDisable:

:csharp:`void Disable(string s)`

Disables the indicated :doc:`SEvent <SEvent>`.

----

.. _SpriteEventsSwap:

:csharp:`void Swap(string s)`

If the indicated :doc:`SEvent <SEvent>` is enabled, disable it.
If it is disabled, enable it.
