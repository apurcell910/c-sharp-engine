===============
KeyboardManager
===============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Offers functionality for checking the state of any key on the keyboard. This class cannot
be instantiated directly, instead use :ref:`Game.Keyboard <GameKeyboard>`.

Indexer
^^^^^^^

Indexing the KeyboardManager class returns a :ref:`KeyState <InputStructsKeyState>`
for the given key.

.. code-block:: csharp
   
   KeyState left = Keyboard[Keys.Left];
   
   if (left.IsPressed)
   {
      Console.WriteLine("Left is pressed");
   }
   
   if (left.WasPressed)
   {
      Console.WriteLine("Left was pressed");
   }

Events
^^^^^^

.. _KeyboardManagerKeyPress:

:csharp:`delegate void KeyPress()`

The delegate type used for all key press events.

----

.. _KeyboardManagerSingleKey:

:csharp:`event KeyPress SingleKey`

Called when a key is pressed.

Methods
^^^^^^^

.. _KeyboardManagerIsPressed:

:csharp:`bool IsPressed(Keys key)`

Returns a bool indicating if the given key is pressed.

.. code-block:: csharp
   
   if (Keyboard.IsPressed(Keys.A))
   {
      Console.WriteLine("A is pressed");
   }

----

.. _KeyboardManagerWasPressed:

:csharp:`bool WasPressed(Keys key)`

Returns a bool indicating if the given key was pressed on the last frame.

.. code-block:: csharp
   
   if (Keyboard.WasPressed(Keys.A))
   {
      Console.WriteLine("A was pressed");
   }

----

.. _KeyboardManagerAlphaIsPressed:

:csharp:`bool AlphaIsPressed()`

Returns a bool indicating if any alphabetical characters were pressed.

.. code-block:: csharp

   if (Keyboard.AlphaIsPressed())
   {
      Console.WriteLine("Alphabetical character is pressed")
   }

----

.. _KeyboardManagerListAlphaPressed:

:csharp:`List<Keys> ListAlphaPressed()`

Returns a List of currently pressed alphabetical keys.

----

.. _KeyboardManagerNumIsPresssed:

:csharp:`bool NumIsPressed()`

Returns a bool indicating if any numberical characters were pressed.

----

.. _KeyboardManagerListNumPressed:

:csharp:`List<Keys> ListNumPressed()`

Returns a List of currently pressed numerical keys.

----

.. _KeyboardManagerArrowIsPressed:

:csharp:`bool ArrowIsPressed()`

Returns a bool indicating if any alphabetical characters were pressed.

.. warning::
   This function currently is not implemented.

----

.. _KeyboardManagerAddKeyBind:

:csharp:`void AddKeybind(Keys key, Event e)`

Adds an event to be called on a specific key press.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerRemoveKeyBind:

:csharp:`void RemoveKeybind(Keys key, Event e)`

Removes an event from being called on a specific key press.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerAddMultiBind:

:csharp:`void AddMultiBind(List<Keys> l, Event e)`

Adds an event to be called when multiple specific keys are pressed.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerAddAlphaBind:

:csharp:`void AddAlphaBind(Event e)`

Adds an event to be called when any alphabetical key is pressed.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerRemoveAlphaBind:

:csharp:`void RemoveAlphaBind(Event e)`

Removes an event to be called when any alphabetical key is pressed.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerAddMassAlphaBind:

:csharp:`void AddMassAlphaBind(Event e)`

Adds an event to be called on each individual alphabetical key press.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerRemoveMassAlphaBind:

:csharp:`void RemoveMassAlphaBind(Event e)`

Removes an event to be called on each individual alphabetical key press.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerAddNumBind:

:csharp:`void AddNumBind(Event e)`

Adds an event to be called on any numeric key is pressed.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerRemoveNumBind:

:csharp:`void RemoveNumBind(Event e)`

Removes an event to be called on any numeric key is pressed.

.. warning::
   Event class currently not functioning.

.. _KeyboardManagerAddAlphaNumBind:

:csharp:`void AddAlphaNumBind(Event e)`

Adds an event to be called on any alphabetical or numeric key is pressed.

.. warning::
   Event class currently not functioning.

----

.. _KeyboardManagerRemoveAlphaNumBind:

:csharp:`void RemoveAlphaNumBind(Event e)`

Removes an event to be called on any alphabetical or numeric key is pressed.

.. warning::
   Event class currently not functioning.