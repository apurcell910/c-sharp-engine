========
PEllipse
========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Defines a ellipse for use in physics calculations.

Constructors
^^^^^^^^^^^^

:csharp:`PEllipse(Vector2 v1, float height, float width)`

Initializes a new instance of PEllipse struct with given
center v1, and (Width, Height) width, height.

.. note::
	Width and height are in terms of the x and y radius of the ellipse, not the diameter.

Field/Properties
^^^^^^^^^^^^^^^^

.. _PEllipseArea:

:csharp:`float Area { get; }`

Returns area of the PEllipse.

-----

.. _PEllipseCenter:

:csharp:`Vector2 Center { get; }`

Center of the PEllipse.

-----

.. _PEllipseTriangles:

:csharp:`List<PTriangle> Triangles { get; }`

List of all triangles that the PEllipse is 
made out of.


