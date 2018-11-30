==============
XboxController
==============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Provides functionality to check input for a connected Xbox controller. Works for all
types of Xbox controller that can be connected to a PC. This class cannot be instantiated
directly, obtain instances from a :doc:`DeviceManager <DeviceManager>`. Inherits from
:doc:`GameController <GameController>`.

Events
^^^^^^

.. _XboxControllerButtonPressed:

:csharp:`delegate void ButtonPressed()`

The delegate type used for all button press events.

----

.. _XboxControllerControllerConnection:

:csharp:`delegate void ControllerConnection()`

The delegate type used for all controller connect/disconnect events.

----

.. _XboxControllerAPressed:

:csharp:`event ButtonPressed APressed`

Called when the A button is pressed.

----

.. _XboxControllerBPressed:

:csharp:`event ButtonPressed BPressed`

Called when the B button is pressed.

----

.. _XboxControllerXPressed:

:csharp:`event ButtonPressed XPressed`

Called when the X button is pressed.

----

.. _XboxControllerYPressed:

:csharp:`event ButtonPressed YPressed`

Called when the Y button is pressed.

----

.. _XboxControllerLBPressed:

:csharp:`event ButtonPressed LBPressed`

Called when the left bumper is pressed.

----

.. _XboxControllerRBPressed:

:csharp:`event ButtonPressed RBPressed`

Called when the right bumper is pressed.

----

.. _XboxControllerBackPressed:

:csharp:`event ButtonPressed BackPressed`

Called when the Back button is pressed.

----

.. _XboxControllerStartPressed:

:csharp:`event ButtonPressed StartPressed`

Called when the Start button is pressed.

----

.. _XboxControllerDPadUpPressed:

:csharp:`event ButtonPressed DPadUpPressed`

Called when dpad up is pressed.

----

.. _XboxControllerDPadDownPressed:

:csharp:`event ButtonPressed DPadDownPressed`

Called when dpad down is pressed.

----

.. _XboxControllerDPadLeftPressed:

:csharp:`event ButtonPressed DPadLeftPressed`

Called when dpad left is pressed.

----

.. _XboxControllerDPadRightPressed:

:csharp:`event ButtonPressed DPadRightPressed`

Called when dpad right is pressed.

----

.. _XboxControllerLeftStickPressed:

:csharp:`event ButtonPressed LeftStickPressed`

Called when the left stick is pressed.

----

.. _XboxControllerRightStickPressed:

:csharp:`event ButtonPressed RightStickPressed`

Called when the right stick is pressed.

----

.. _XboxControllerDisconnected:

:csharp:`event ControllerConnection Disconnected`

Called when the XboxController disconnects from the system.

----

.. _ XboxControllerConnected:

:csharp:`event ControllerConnection Connected`

Called when the XboxController reconnects to the system.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _XboxControllerButtons:

:csharp:`XboxController.ButtonState Buttons { get; }`

Returns a :ref:`ButtonState <InputStructsXboxControllerButtonState>` containing
information on all of the XboxController's buttons.

----

.. _XboxControllerLeftStick:

:csharp:`ControlStick LeftStick { get; }`

Returns a :ref:`ControlStick <InputStructsControlStick>` containing the state of the
XboxController's left stick.

----

.. _XboxControllerRightStick:

:csharp:`ControlStick RightStick { get; }`

Returns a :ref:`ControlStick <InputStructsControlStick>` containing the state of the
XboxController's right stick.

Methods
^^^^^^^

.. _XboxControllerAnyIsPressed:

:csharp:`bool AnyIsPressed(ButtonType buttons)`

Returns a bool indicating if any of the :ref:`ButtonTypes <XboxControllerButtonType>`
in the parameter "buttons" are pressed.

----

.. _XboxControllerAnyWasPressed:

:csharp:`bool AnyWasPressed(ButtonType buttons)`

Returns a bool indicating if any of the :ref:`ButtonTypes <XboxControllerButtonType>`
in the parameter "buttons" were pressed on the last frame.

----

.. _XboxControllerGetButtonState:

:csharp:`ButtonState GetButtonState(ButtonType button)`

Returns the :ref:`ButtonState <InputStructsXboxControllerButtonState>`
of the :ref:`ButtonType <XboxControllerButtonType>` in the parameter "button".

.. note::
   If the parameter "button" contains a bitmask rather than a single value, the
   function will instead return the default state of
   :ref:`ButtonState <InputStructsXboxControllerButtonState>`.

Enums
^^^^^

.. _XboxControllerButtonType:

XboxController.ButtonType
-------------------------

A bitmask enum for all of the potential buttons on an Xbox controller.

.. code-block:: csharp
   
   enum XboxController.ButtonType
   {
      A = 1,
      B = 2,
      X = 4,
      Y = 8,
      LB = 16,
      RB = 32,
      Back = 64,
      Start = 128,
      DPadLeft = 256,
      DPadRight = 512,
      DPadUp = 1024,
      DPadDown = 2048,
      LStick = 4096,
      RStick = 8192
   }
