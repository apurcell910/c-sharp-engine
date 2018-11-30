===============
GraphicsManager
===============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp
   
A basic Graphics Manager used to draw different
shapes as well as Bitmaps and Text. This class also
handles drawing to either World Space or Screen Space.

Properties
^^^^^^^^^^

.. _GraphicsManagerWorldScaleProperty:

:csharp:`Vector2 WorldScale`

Gets a default World Scale. World space is a user defined
scale that is allows the user to draw objects to a their own scale
rather than draw using pixel coordinates on the screen.

-----

.. _GraphicsManagerBackColor:

:csharp:`Color BackColor`

Gets the background color and sets the background color.
