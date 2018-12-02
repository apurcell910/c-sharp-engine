======
SImage
======

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

An override of the :doc:`Sprite <Sprite>` class to display images on-screen.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _SImageImage:

:csharp:`public string image`

The key of the image, as stored in the :doc:`Content Manager <../ContentManager>`.

----

.. _SImagePortion:

:csharp:`public int ix`
:csharp:`public int iy`
:csharp:`public int iw`
:csharp:`public int ih`

The portions of the source image to draw, by pixel coordinate.


Constructor
^^^^^^^^^^^

.. _SImageConstructor:

:csharp:`public SImage(Game game, double x, double y, double w, double h, string image, int ix = 0, int iy = 0, int iw = 0, int ih = 0)`

Creates a new SImage with the specified location, size, and image.

.. code-block:: csharp

	//Add an SImage to the SpriteList
	Sprites.Add("image", new SImage(this, 0, 0, 40, 40, "imageName"));