================
TriangleCollider
================

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A :doc:`Collider <Collider>` class that allows triangles.

Constructors
^^^^^^^^^^^^

:csharp:`TriangleCollider(Vector v1, Vector v2, Vector v3)`

Creates a new TriangleCollider with the specified edge vertices.

-----

:csharp:`TriangleCollider(PTriangle tri)`

Creates a new TriangleCollider with a PTriangle.

-----

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _PTriangleGet:

:csharp:`PTriangle Triangle {get; }`

Gets the PTriangle of the TriangleCollider.
