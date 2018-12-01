==========
PRectangle
==========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A defines a rectangle for use in physics calculations.

Constructors
^^^^^^^^^^^^

:csharp:`PRectangle(Vector v1, Vector v2, Vector v3, Vector v4)`

Initializes a new instance of the PRectangle struct with the given
vertices. v1 is top left, v2 is top right, v3 is bottom left, and v4
is bottom right.

-----

:csharp:`PRectangle(Vector v1, Vector v2)`

Initializes a new instance of the PRectangle struct with the given
top left, v1, and bottom right, v2, vertices of the rectangle.

-----

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _PRectangleTriOne:

:csharp:`PTriangle TriOne {get; }`

A triangle that is used to create half of the PRectangle.

-----

.. _PRectangleTriTwo:

:csharp:`PTriangle TriOne {get; }`

A triangle that is used to create the other half of PRectangle.

-----

.. _PRectangleArea:

:csharp:`float Area {get; }`

Returns the area of the rectangle.

-----

Methods
^^^^^^^

.. _PTriangleContainsPoints:

:csharp:`bool ContainsPoint(Vector2 point)`

Checks to see if a Vector2 point exists within the PRectangle.

.. code-block:: csharp

   // Create PRectangle
   PTriangle test = new PTriangle(Vector2.Zero, Vector2.One, Vector2.Right, Vector2.Up);
   
   // Test multiple points
   Console.WriteLine(test.ContainsPoint(new Vector2(5, 5)));
   Console.WriteLine(test.ContainsPoint(new Vector2(0.5f, 0.25f)));
   
   // Program outputs:
   // false
   // true