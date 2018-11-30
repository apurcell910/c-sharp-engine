===========
InputAction
===========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Provides an interface for binding multiple buttons to a single action.

Constructors
^^^^^^^^^^^^

:csharp:`InputAction(Game game)`

Initializes a new instance fo the InputAction class with the given parent
:doc:`Game <../Game>`.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _InputActionIsPressed:

:csharp:`bool IsPressed { get; }`

Gets a bool indicating if any of the buttons registered to the InputAction are pressed.

----

.. _InputActionWasPressed:

:csharp:`bool WasPressed { get; }`

Gets a bool indicating if any of the buttons registered to the InputAction were pressed
on the last frame.

Methods
^^^^^^^

.. _InputActionAddDevice:

:csharp:`void AddDevice(GameController controller)`

Registers a :doc:`GameController <GameController>` with the InputAction so that
it will be checked when calling :ref:`IsPressed <InputActionIsPressed>` or
:ref:`WasPressed <InputActionWasPressed>`.

----

.. _InputActionRemoveDevice:

:csharp:`void RemoveDevice(GameController controller)`

Deregisters a :doc:`GameController <GameController>` from the InputAction so that it
will no longer be checked.

----

.. _InputActionAddXboxButtons:

:csharp:`void AddXboxButtons(XboxController.ButtonType buttons)`

Adds all of the values in the bitmasked :ref:`ButtonType <XboxControllerButtonType>`
parameter "buttons" to the list of buttons this InputAction checks against.

----

.. _InputActionRemoveXboxButtons:

:csharp:`void RemoveXboxButtons(XboxController.ButtonType buttons)`

Removes all of the values in the bitmasked :ref:`ButtonType <XboxControllerButtonType>`
parameter "buttons" from the list of buttons this InputAction checks against.

----

.. _InputActionAddKey:

:csharp:`void AddKey(Keys key)`

Adds the given parameter "key" to the list of keyboard buttons the InputAction checks
against.

----

.. _InputActionRemoveKey:

:csharp:`void RemoveKey(Keys key)`

Removes the given parameter "key" from the list of keyboard buttons the InputAction
checks against.
