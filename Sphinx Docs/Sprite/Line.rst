====
Line
====

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

An override of the :doc:`Sprite <Sprite>` class to display a line on-screen.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _LineColor:

:csharp:`public Color color`

The color of the line to draw.

Constructors
^^^^^^^^^^^^

.. _LineConstructor:

:csharp:`public Line(Game game, Point p1, Point p2, Color color)`

Creates a new Line with the given points and color.

.. code-block:: csharp

	//Add a Line to the SpriteList
	Sprites.Add("line", new Line(this, new Point(0, 0), new Point(40, 40), Color.Red));

----

.. _LineConstructor2:

:csharp:`public Line(Game game, int x1, int y1, int x2, int y2, Color color)`

Creates a new Line with the given points and color.

.. code-block:: csharp

	//Add a Line to the SpriteList
	Sprites.Add("line", new Line(this, 0, 0, 40, 40, Color.Red));