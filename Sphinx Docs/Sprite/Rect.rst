====
Rect
====

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

An override of the :doc:`Sprite <Sprite>` class to display a rectangle on-screen.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _RectColor:

:csharp:`public Color color`

The color of the rectangle to draw.

----

.. _RectFill:

:csharp:`public bool fill`

Controls whether or not to draw a filled-in rectangle or an outline of a rectangle.


Constructor
^^^^^^^^^^^

.. _RectConstructor:

:csharp:`public Rect(Game game, double x, double y, double w, double h, Color color, bool fill = true)`

Creates a new rectangle with the specified location, size, color, and fill option.

.. code-block:: csharp

	//Add a Rect to the SpriteList
	Sprites.Add("rectangle", new Rect(this, 0, 0, 40, 40, Color.Red, false));