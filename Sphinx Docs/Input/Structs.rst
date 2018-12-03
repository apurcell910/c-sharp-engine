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

.. _InputStructsClick:

Click
^^^^^

A simple struct containing the state of a mouse button

Fields/Properties
-----------------

.. _InputSturctsClickIsClicked:

:csharp:`bool IsClicked { get; }`

Gets a bool indicating whether the button is currently clicked.

----

.. _InputSturctsClickWasClicked:

:csharp:`bool WasClicked { get; }`

Gets a bool indicating whether the button was clicked on the last frame.

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

.. _InputStructsKeyState:

KeyState
^^^^^^^^

A simple struct containing the state of a keyboard key.

Fields/Properties
-----------------

.. _InputStructsKeyStateIsPressed:

:csharp:`bool IsPressed { get; }`

Gets a bool indicating whether the key is currently pressed.

----

.. _InputStructsKeyStateWasPressed:

:csharp:`bool WasPressed { get; }`

Gets a bool indicating whether the key was pressed on the last frame.

----

.. _InputStructsKeyStateKey:

:csharp:`Keys Key { get; }`

Gets the key that this KeyState represents.

.. _InputStructsMouseManagerMouseState:

MouseManager.MouseState
^^^^^^^^^^^^^^^^^^^^^^^

A simple struct containing information on the location and buttons on a mouse.

Fields/Properties
-----------------

.. _InputStructsMouseManagerMouseStateCenter:

:csharp:`Click Center { get; }`

Gets a :ref:`Click <InputStructsClick>` containing information on the Center button on
the mouse.

----

.. _InputStructsMouseManagerMouseStateLeft:

:csharp:`Click Left { get; }`

Gets a :ref:`Click <InputStructsClick>` containing information on the Left button on
the mouse.

----

.. _InputStructsMouseManagerMouseStateRight:

:csharp:`Click Right { get; }`

Gets a :ref:`Click <InputStructsClick>` containing information on the Right button on
the mouse.

----

.. _InputStructsMouseManagerMouseStateX:

:csharp:`int X { get; }`

Gets an int indicating the X coordinate of the mouse.

----

.. _InputStructsMouseManagerMouseStateY:

:csharp:`int Y { get; }`

Gets an int indicating the Y coordinate of the mouse.

----

.. _InputStructsMouseManagerMouseStateLocation:

:csharp:`Vector2 Location { get; }`

Gets a :doc:`Vector2 <../Vector2>` indicating the location of the mouse.

.. _InputStructsXboxControllerButtonState:

XboxController.ButtonState
^^^^^^^^^^^^^^^^^^^^^^^^^^

A simple struct containing information on all of the buttons of an Xbox controller.

Fields/Properties
-----------------

.. _InputStructsXboxControllerButtonStateA:

:csharp:`Button A { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the A button on
the controller.

----

.. _InputStructsXboxControllerButtonStateB:

:csharp:`Button B { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the B button on
the controller.

----

.. _InputStructsXboxControllerButtonStateX:

:csharp:`Button X { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the X button on
the controller.

----

.. _InputStructsXboxControllerButtonStateY:

:csharp:`Button Y { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the Y button on
the controller.

----

.. _InputStructsXboxControllerButtonStateLB:

:csharp:`Button LB { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the LB button on
the controller.

----

.. _InputStructsXboxControllerButtonStateRB:

:csharp:`Button RB { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the RB button on
the controller.

----

.. _InputStructsXboxControllerButtonStateBack:

:csharp:`Button Back { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the Back
button on the controller.

----

.. _InputStructsXboxControllerButtonStateStart:

:csharp:`Button Start { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the Start
button on the controller.

----

.. _InputStructsXboxControllerButtonStateDPadLeft:

:csharp:`Button DPadLeft { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the dpad left
button on the controller.

----

.. _InputStructsXboxControllerButtonStateDPadRight:

:csharp:`Button DPadRight { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the dpad right
button on the controller.

----

.. _InputStructsXboxControllerButtonStateDPadUp:

:csharp:`Button DPadUp { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the dpad up
button on the controller.

----

.. _InputStructsXboxControllerButtonStateDPadDown:

:csharp:`Button DPadDown { get; }`

Gets a :ref:`Button <InputStructsButton>` containing information about the dpad down
button on the controller.
