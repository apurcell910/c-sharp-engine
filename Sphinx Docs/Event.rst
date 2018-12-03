=====
Event
=====

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A class that is used to store callable events.

.. warning::
   Currently not functioning. Please manually add in any needed actions.

Events
^^^^^^

.. _EventActions:

:csharp:`delegate void Actions(Keys key, Vector2 Location)`

The delegate type used for storing an action.

----

.. _EventSnippets:

:csharp:`event Actions Snippets`

Called when :ref:`CallEvent <EventCallEvent>` is called.

Methods
^^^^^^^^

.. _EventCallEvent:

:csharp:`virtual void CallEvent()`

Calls the code stored in this class.
