=======
Vector2
=======

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A simple 2D Vector struct with various helper math functions.

Constructors
^^^^^^^^^^^^

:csharp:`Vector2(float x, float y)`

Initializes a new instance of the Vector2 struct with the given x and y components.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _Vector2X:

:csharp:`float X { get; }`

Gets the x component of the Vector2.

----

.. _Vector2Y:

:csharp:`float Y { get; }`

Gets the y component of the Vector2.

----

.. _Vector2Length:

:csharp:`float Length { get; }`

Gets the length/magnitude of the Vector2.

----

.. _Vector2Zero:

:csharp:`static Vector2 Zero { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(0f, 0f);

----

.. _Vector2One:

:csharp:`static Vector2 One { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(1f, 1f);

----

.. _Vector2Half:

:csharp:`static Vector2 Half { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(0.5f, 0.5f);

----

.. _Vector2Left:

:csharp:`static Vector2 Left { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(-1f, 0f);

----

.. _Vector2Right:

:csharp:`static Vector2 Right { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(1f, 0f);

----

.. _Vector2Up:

:csharp:`static Vector2 Up { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(0f, -1f);

----

.. _Vector2Down:

:csharp:`static Vector2 Down { get; }`

Shorthand for:

.. code-block:: csharp
   
   new Vector2(0f, 1f);

Methods
^^^^^^^

.. _Vector2Rotate:

:csharp:`Vector2 Rotate(Vector2 origin, float r)`

Rotates the Vector2 around the point "origin" by "r" degrees counterclockwise
and returns a new Vector2 containing the result.

.. code-block:: csharp
   
   // Rotate Vector2.One by 90 degrees around the origin
   Vector2 rotated = Vector2.One.Rotate(Vector2.Zero, 90);
   
   // Print out this Vector2
   Console.WriteLine(rotated);
   
   // Program outputs:
   // (X: -1, Y: 1)

----

.. _Vector2Normalize:

:csharp:`Vector2 Normalize()`

Creates a new Vector2 with the same component ratio as the current one, but with
:ref:`Length <Vector2Length>` equal to 1.

.. code-block:: csharp
   
   // Create a non-normalized Vector2
   Vector2 testVec = new Vector2(5, 10);
   
   // Print out the non-normalized Vector2's length
   Console.WriteLine(testVec.Length);
   
   // Normalize this Vector2
   testVec = testVec.Normalize();
   
   // Print out the normalized Vector2 and its length
   Console.WriteLine(testVec);
   Console.WriteLine(testVec.Length);
   
   // Program outputs:
   // 11.18034
   // (X: 0.4472136, Y: 0.8944272
   // 1

----

.. _Vector2ToString:

:csharp:`string ToString()`

Returns a string representation of the Vector2.

.. code-block:: csharp
   
   // Print out Vector2.One
   Console.WriteLine(Vector2.One);
   
   // Program outputs:
   // (X: 1, Y: 1)

----

.. _Vector2CrossProduct:

:csharp:`static float CrossProduct(Vector2 v1, Vector2 v2)`

Computes the Z component of the cross product of the two given Vector2s.

----

:csharp:`static float CrossProduct(Vector2 sub, Vector2 v1, Vector2 v2)`

Equivalent to calling:

.. code-block:: csharp
   
   Vector2.CrossProduct(v1 - sub, v2 - sub);
