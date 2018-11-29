=========
PTriangle
=========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Defines a triangle for use in physics calculations.

Constructors
^^^^^^^^^^^^

:csharp:`PTriangle(Vector2 vert1, Vector2 vert2, Vector2 vert3)`

Initializes a new instance of the PTriangle struct with the given vertices.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _PTriangleVertexOne:

:csharp:`Vector2 VertexOne { get; }`

Equal to the :doc:`Vector2 <../Vector2>` "vert1" passed to the constructor.

----

.. _PTriangleVertexTwo:

:csharp:`Vector2 VertexTwo { get; }`

Equal to the :doc:`Vector2 <../Vector2>` "vert2" passed to the constructor.

----

.. _PTriangleVertexThree:

:csharp:`Vector2 VertexThree { get; }`

Equal to the :doc:`Vector2 <../Vector2>` "vert3" passed to the constructor.

----

.. _PTriangleArea:

:csharp:`float Area { get; }`

Returns the area of the PTriangle.

Methods
^^^^^^^

.. _PTriangleContainsPoint:

:csharp:`bool ContainsPoint(Vector2 point)`

Checks whether or not the :doc:`Vector2 <../Vector2>` "point" is located within the
PTriangle.

.. code-block:: csharp
   
   // Create PTriangle
   PTriangle test = new PTriangle(Vector2.Zero, Vector2.One, Vector2.Right);
   
   // Test multiple points
   Console.WriteLine(test.ContainsPoint(new Vector2(5, 5)));
   Console.WriteLine(test.ContainsPoint(new Vector2(0.5f, 0.25f)));
   
   // Program outputs:
   // false
   // true

----

.. _PTriangleIsTouching:

:csharp:`bool IsTouching(PTriangle other)`

Checks if two PTriangles are touching. Equivalent to checking if either PTriangle
contains any of the vertices of the other.
