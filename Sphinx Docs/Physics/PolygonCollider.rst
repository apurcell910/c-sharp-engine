===============
PolygonCollider
===============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A :doc:`Collider <Collider>` class that allows arbitrary simple polygons.

Constructors
^^^^^^^^^^^^

:csharp:`PolygonCollider(params Vector2[] vertices)`

Creates a new PolygonCollider with the specified edge vertices.

.. warning::
   May throw ArgumentException if vertices array does not form a simple polygon.

Methods
^^^^^^^

.. _PolygonColliderGenerateRandom:

:csharp:`static PolygonCollider GenerateRandom(float radius)`

Creates a circle with the specified radius and does a simple randomization of the points
to create a rough PolygonCollider.
