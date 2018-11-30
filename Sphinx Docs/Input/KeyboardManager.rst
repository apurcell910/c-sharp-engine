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
