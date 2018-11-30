=======
Structs
=======

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

.. _InputStructsButton:

Button
^^^^^^

A simple struct containing the state of a controller button.

Fields/Properties
-----------------

.. _InputStructsButtonIsPressed:

:csharp:`bool IsPressed { get; }`

Gets a bool indicating whether the Button is currently pressed.

----

.. _InputStructsButtonWasPressed:

:csharp:`bool WasPressed { get; }`

Gets a bool indicating whether the Button was pressed on the last frame.

.. _InputStructsControlStick:

ControlStick
^^^^^^^^^^^^

A simple struct containing the state of a controller stick.

Fields/Properties
-----------------

.. _InputStructsControlStickState:

:csharp:`Vector2 State { get; }`

Gets a :doc:`Vector2 <../Vector2>` containing the position of the ControlStick. Values
are on a scale from -1 to 1.

----

.. _InputStructsControlStickButton:

:csharp:`Button Button { get; }`

Gets a :ref:`Button <InputStructsButton>` struct containing the state of the
ControlStick button.
