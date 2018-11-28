========
Collider
========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A basic collision class composed of some amount of triangles. This
class is abstract and cannot be instantiated directly. For creating
colliders, use
:doc:`TriangleCollider <TriangleCollider>`,
:doc:`RectangleCollider <RectangleCollider>`,
:doc:`EllipseCollider <EllipseCollider>`, or
:doc:`PolygonCollider <PolygonCollider>`,

Events
^^^^^^

.. _ColliderColliderEvent:

:csharp:`delegate void ColliderEvent(Collider self, Collider other)`

The delegate type used for all collision events. Contains both
colliders involved in the collision event as parameters.

----

.. _ColliderColliderEnter:

:csharp:`event ColliderEvent ColliderEnter`

Event invoked whenever one Collider enters another.

.. warning::
   Not yet implemented.

----

.. _ColliderColliderLeave:

:csharp:`event ColliderEvent ColliderLeave`

Event invoked whenever one Collider leaves another.

.. warning::
   Not yet implemented.

----

.. _ColliderColliderStay:

:csharp:`event ColliderEvent ColliderStay`

Event invoked once every frame while two Colliders remain touching
after the initial frame of contact.

.. warning::
   Not yet implemented.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _ColliderTriangles:

:csharp:`PTriangle[] Triangles { get; }`

Gets a :doc:`PTriangle <PTriangle>` array containing all of the triangles
used by the Collider. This array is a duplicate of the internal array,
and is thus safe to modify.

.. warning::
   A new array is created by every call to this property. Limit calls to
   this by caching the returned array to prevent unnecessary garbage
   creation.

----

.. _ColliderVertices:

:csharp:`Vector2[] Vertices { get; }`

Gets a :doc:`Vector2 <../Vector2>` array containing all of the points on
the edges of the Collider. This array is a duplicate of the internal array,
and is thus safe to modify.

.. warning::
   A new array is created by every call to this property. Limit calls to
   this by caching the returned array to prevent unnecessary garbage
   creation.

----

.. _ColliderIsTrigger:

:csharp:`bool IsTrigger { get; set; }`

If true, the Collider will be nonsolid and allow other Colliders to pass
through it. By default, a Collider does not act as a trigger and will have
solid collision.

.. warning::
   Not yet implemented. All colliders will act as triggers.

----

.. _ColliderIsPhysicsObject:

:csharp:`bool IsPhysicsObject { get; set; }`

If true, the Collider will receive updates to its position based on its
:ref:`Velocity <ColliderVelocity>`. By default, a Collider acts as a
static object and does not receive position updates.

.. warning::
   Not yet implemented. All colliders will act as static objects.

----

.. _ColliderPosition:

:csharp:`Vector2 Position`

Gets or sets a :doc:`Vector2 <../Vector2>` indicating the position of the
Collider. :ref:`Triangles <ColliderTriangles>` and :ref:`Vertices <ColliderVertices>`
arrays are updated to reflect the new position when this value is set.

----

.. _ColliderVelocity:

:csharp:`Vector2 Velocity { get; set; }`

Gets or sets the current velocity of this Collider. If
:ref:`IsPhysicsObject <ColliderIsPhysicsObject>` is set false, this value will
always equal :ref:`Vector2.Zero <Vector2Zero>`.

.. warning::
   Not yet implemented. All colliders will act as static objects.

----

.. _ColliderRotation:

:csharp:`float Rotation { get; set; }`

Gets or sets the rotation of the Collider. :ref:`Triangles <ColliderTriangles>`
and :ref:`Vertices <ColliderVertices>` arrays are updated to reflect the new
rotation when this value is set.

----

.. _ColliderScale:

:csharp:`Vector2 Scale { get; set; }`

Gets or sets the scale of the Collider. Value must be positive.
:ref:`Triangles <ColliderTriangles>` and :ref:`Vertices <ColliderVertices>`
arrays are updated to reflect the new scale when this value is set.

.. warning::
   Not yet implemented. All colliders will act as if they have scale equal
   to :ref:`Vector2.One <Vector2One>`.

Methods
^^^^^^^

.. _ColliderIsTouchingCollider:

:csharp:`bool IsTouching(Collider other)`

Returns true if any of the Collider's :ref:`Triangles <ColliderTriangles>`
are touching any of the :ref:`Triangles <ColliderTriangles>` in the specified
Collider "other". Returns false if "other" is null.

.. code-block:: csharp
   
   // Create two Colliders
   Collider testOne = new TriangleCollider(Vector2.Zero, Vector2.Up, Vector2.One);
   Collider testTwo = new EllipseCollider(0, 0, 5, 5);

   // Check collision
   Console.WriteLine(testOne.IsTouching(testTwo));
   
   // Move testTwo and check again
   testTwo.Position = new Vector2(300, 300);
   Console.WriteLine(testOne.IsTouching(testTwo));
   
   // Program outputs:
   // true
   // false

----

.. _ColliderIsTouchingVector2:

:csharp:`bool IsTouching(Vector2 point)`

Returns true if any of the Collider's :ref:`Triangles <ColliderTriangles>`
contain the given :doc:`Vector2 <../Vector2>` "point".

.. code-block:: csharp
   
   // Create test Collider
   Collider testCollider = new EllipseCollider(200, 200, 50, 50);
   
   // Check if Collider contains several points
   Console.WriteLine(testCollider.IsTouching(new Vector2(210, 190));
   Console.WriteLine(testCollider.IsTouching(new Vector2(163, 200));
   Console.WriteLine(testCollider.IsTouching(new Vector2(100, 100));
   
   // Program outputs:
   // true
   // true
   // false

----

.. _ColliderGetBoundingBox:

:csharp:`RectangleF GetBoundingBox()`

Returns the smallest possible rectangle that completely contains the
Collider, specified in world coordinates.

.. code-block:: csharp
   
   // Create a test collider and get the bounding box
   Collider testCollider = new EllipseCollider(350, 300, 100, 200);
   RectangleF box = testCollider.GetBoundingBox();
   
   // box.X = (350 - 100 / 2) = 300
   // box.Y = (300 - 200 / 2) = 200
   // box.Width = 100
   // box.Height = 200

----

.. _ColliderGetCenter:

:csharp:`Vector2 GetCenter()`

Returns the center point of the Collider, calculated as the average
of all points in :ref:`Vertices <ColliderVertices>`

.. code-block:: csharp
   
   // Create a test collider
   Collider testCollider = new RectangleCollider(0, 0, 100, 100);
   
   // Print out the center point
   Console.WriteLine(testCollider.GetCenter());
   
   // Program outputs:
   // (X: 50, Y: 50)
