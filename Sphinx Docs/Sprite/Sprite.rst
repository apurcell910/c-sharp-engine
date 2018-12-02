======
Sprite
======

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A class that is used for physics and drawing. These are the objects that are drawn to the screen-space. This class is abstract and cannot be instantiated directly. For creating sprites, use
:doc:`Rect <Rect>`, :doc:`Line <Line>`, :doc:`Ellipse <Ellipse>`, :doc:`SImage <SImage>`, or create your own to use.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _SpritePos:

:csharp:`public double x`
:csharp:`public double y`
:csharp:`public double w`
:csharp:`public double h`

The position and size of the Sprite.

----

.. _SpriteVelocity:

:csharp:`public double velocityX`
:csharp:`public double velocityY`

The x and y velocity values for the Sprite. By default, this is set to zero.

----

.. _SpriteGravity:

:csharp:`public double gravityX`
:csharp:`public double gravityY`

The x and y gravity values for the Sprite. By default, these are set to zero.

----

.. _SpriteAngle:

:csharp:`public float angle`

The angle of the object, in degrees. By default, this is set to zero.

----

.. _SpriteAnchor:

:csharp:`public double xAnchor`
:csharp:`public double yAnchor`

The x and y values for the anchor of the sprite. These should be from 0.0 to 1.0. By default, these are set to zero.

----

.. _SpriteAlive:

:csharp:`public bool alive`

The value that stores whether or not the sprite can be drawn and updated each loop. By default, set to true.

----

.. _SpriteDisp:

:csharp:`public bool disp`

The value that stores whether or not to display the sprite. By default, set to false.

----

.. _SpritePhysicsCollider:

:csharp:`public Physics.Collider collider`

The :doc:`Collider <../Physics/Collider>` for the sprite. Should be automatically set depinding on what kind of sprite that it is.

This is moved and scaled with the sprite itself.

----

.. _SpritePhysicsCollisions:

:csharp:`public List<string> collisions`

A list of all of the different sprites that the sprite will check for collision with.

Methods
^^^^^^^

These methods should all be called by a sprite within it's own update loop, and cannot, with the current implementation, be called outside of it.

.. _SpriteDraw:

:csharp:`public virtual void Draw(GraphicsManager graphics)`

An overridable function that will detail how to draw the specific Sprite.

----

.. _SpriteUpdate:

:csharp:`public virtual void Update(GameTime gameTime)`

An overridable function that will detail how the sprite updates each update loop. Any and all actions should take into account the current gameTime, so as to avoid the actions of the sprite being locked to the framerate of the game itself.

----

.. _SpriteKill:

:csharp:`public void Kill()`

For any sprite, "kills" the sprite such that it will not update or display at all.

.. code-block:: csharp

	if (/*Some action*/) {
		this.Kill();
	}

----

.. _SpriteReincarnate:

:csharp:`public void Reincarnate()`

For any "dead" sprite, "reincarnates" it, allowing it to update and draw again. Must separately set the value of :ref:`disp <SpriteDisp>` to true.

.. code-block:: csharp

	if (/*Some action*/) {
		//Bring a sprite back to life and draw it.
		this.Reincarnate();
		this.Display(true);
	}

----

.. _SpriteDisplay:

:csharp:`public void Display(bool disp)`

Sets the value of :ref:`disp <SpriteDisp>`.

----

.. _SpriteMove:

:csharp:`public void Move(double x, double y)`

Moves the sprite by the indicated amount of units.

.. code-block:: csharp

	//Move this sprite down 50 and left 25.
	this.Move(-25, 50);

----

.. _SpriteMoveTo:

:csharp:`public void MoveTo(double x, double y)`

Moves the sprite to the indicated position.

.. code-block:: csharp

	this.MoveTo(30, 800);

----

.. _SpriteMoveY:

:csharp:`public void MoveY(double y)`

Moves the sprite vertically by the indicated amount.

.. code-block:: csharp

	//Move the sprite up 80 units.
	this.MoveY(-80);

----

.. _SpriteMoveX:

:csharp:`public void MoveX(double x)`

Moves the sprite horizontally by the indicated amount.

.. code-block:: csharp

	//Move the sprite to the right by 60 units.
	this.moveX(60);

----

.. _SpriteSetRotation:

:csharp:`public void SetRotation(float r)`

Sets the :ref:`angle <SpriteAngle>` of the sprite to the indicated amount.

----

.. _SpriteRotate:

:csharp:`public void Rotate(float r)`

Rotates the :ref:`angle <SpriteAngle>` of the sprite by the indicated amount. A positive value will rotate counter-clockwise, while a negative value will rotate clockwise.

----

.. _SpriteScaleX:

:csharp:`public void ScaleX(double scale)`

Scales the sprite horizontally by the indicated amount. A decimal will make the sprite smaller, while a number larger than 1.0 will make the sprite larger.

----

.. _SpriteScaleY:

:csharp:`public void ScaleY(double scale)`

Scales the sprite vertically by the indicated amount. A decimal will make the sprite smaller, while a number larger than 1.0 will make the sprite larger.

----

.. _SpriteScale:

:csharp:`public void Scale(double scale)`

Scales the sprite by the indicated amount. A decimal will make the sprite smaller, while a number larger than 1.0 will make the sprite larger.

----

.. _SpriteSetAnchor:

:csharp:`public void SetAnchor(double anchor)`

Sets the :ref:`anchor <SpriteAnchor>` of the sprite to the indicated value. 0.5 will be in the middle of the sprite.

----

.. _SpriteSetAnchorX:

:csharp:`public void SetAnchorX(double anchor)`

Sets the :ref:`horizontal anchor <SpriteAnchor>` of the sprite to the indicated value. 0.5 will be in the middle of the sprite.

----

.. _SpriteSetAnchorY:

:csharp:`public void SetAnchorY(double anchor)`

Sets the :ref:`vertical anchor <SpriteAnchor>` of the sprite to the indicated value. 0.5 will be in the middle of the sprite.

----

.. _SpriteSetVelocityX:

:csharp:`public void SetVelocityX(double x)`

Sets the :ref:`horizontal velocity <SpriteVelocity>` of the sprite to the indicated value.

----

.. _SpriteSetVelocityY:

:csharp:`public void SetVelocityY(double y)`

Sets the :ref:`vertical velocity <SpriteVelocity>` of the sprite to the indicated value.

----

.. _SpriteAddVelocityX:

:csharp:`public void AddVelocityX(double x)`

Adds to the :ref:`horizontal velocity <SpriteVelocity>` of the sprite by the indicated value. Mainly used for gravity purposes.

----

.. _SpriteAddVelocityY:

:csharp:`public void SetVelocityY(double Y)`

Adds to the :ref:`vertical velocity <SpriteVelocity>` of the sprite by the indicated value. Mainly used for gravity purposes.

----

.. _SpriteSetGravityX:

:csharp:`public void SetGravityX(double x)`

Sets the :ref:`horizontal gravity <SpriteGravity>` of the sprite to the indicated amount.

----

.. _SpriteSetGravityY:

:csharp:`public void SetGravityY(double y)`

Sets the :ref:`vertical gravity <SpriteGravity>` of the sprite to the indicated amount.

----

.. _SpriteSetCollider:

:csharp:`public void SetCollider(Physics.Collider collider)`

Sets the :doc:`Collider <../Physics/Collider>` of the Sprite to the indicated :doc:`Collider <../Physics/Collider>`.

----

.. _SpriteAddCollision:

:csharp:`public void AddCollision(string collision)`

Add to the :ref:`collisions <SpritePhysicsCollisions>` that the sprite checks against each update loop.

.. warning::
	Not fully functional. Will not work in all cases.