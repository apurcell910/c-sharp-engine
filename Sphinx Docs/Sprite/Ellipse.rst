=======
Ellipse
=======

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

An override of the :doc:`Sprite <Sprite>` class to display an ellipse on-screen.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _EllipseColor:

:csharp:`public Color color`

The color of the ellipse to draw.

----

.. _EllipseFill:

:csharp:`public bool fill`

Controls whether or not to draw a filled-in ellipse or an outline of an ellipse.


Constructor
^^^^^^^^^^^

.. _EllipseConstructor:

:csharp:`public Ellipse(Game game, double x, double y, double w, double h, Color color, bool fill = true)`

Creates a new ellipse with the specified location, size, color, and fill option.

.. code-block:: csharp

	//Add an ellipse to the SpriteList
	Sprites.Add("ellipse", new Rect(this, 0, 0, 40, 40, Color.Red, false));