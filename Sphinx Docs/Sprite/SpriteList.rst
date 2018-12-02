==========
SpriteList
==========

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A basic manager for the sprite class that can be used to add in various sprites in the game world. Automatically created by the :doc:`Game <../Game>` class, should not be manually created by the user.

Methods
^^^^^^^^

Any methods that edit a :doc:`Sprite <Sprite>` will simply call the relevant method within the :doc:`Sprite <Sprite>`.

.. _SpriteListAdd:

:csharp:`public void Add(string key, Sprite sprite)`

Adds a new :doc:`Sprite <Sprite>` to the SpriteList, using a string to reference it for later use from anywhere.

.. code-block:: csharp

	Sprites.Add("sprite1", new Rect(10, 10, 20, 20, Color.Blue));

----

.. _SpriteListDisplay:

:csharp:`public void Display(string key, bool disp)`

Choose whether or not to display the indicated :doc:`Sprite <Sprite>`. Even if it is not displayed, it will still update.

.. code-block:: csharp

	//Display a sprite:
	Sprites.Display("sprite1", true);

	//Don't display a sprite:
	Sprites.Display("sprite1", false);

----

.. _SpriteListMove:

:csharp:`public void Move(string key, double x, double y)`

Move the indicated :doc:`Sprite <Sprite>` by the indicated amount. A positive value will move it down or to the right, depending on x or y, while a negative value will move it up or to the left.

.. code-block:: csharp

	//Move a sprite up 30 units and right 50 units
	Sprites.Move("sprite1", 50, -30);

----

.. _SpriteListMoveTo:

:csharp:`public voide MoveTo(string key, double x, double y)`

Move the indicated :doc:`Sprite <Sprite>` to the indicated position in the world.

.. code-block:: csharp

	//Move a sprite to the position 70, 800
	Sprites.MoveTo("sprite1", 70, 800);

----

.. _SpriteListMoveX:

:csharp:`public void MoveX(string key, double x)`

Moves the indicated :doc:`Sprite <Sprite>` horizontally by the indicated amount. A positive value will move right while a negative value will move left.

.. code-block:: csharp

	//Move a sprite right 80 units.
	Sprites.MoveX("sprite1", 80);

----

.. _SpriteListMoveY:

:csharp:`public void MoveY(string key, double y)`

Moves the indicated :doc:`Sprite <Sprite>` vertically by the indicated amount. A positive value will move down while a negative value will move up.

.. code-block:: csharp

	//Move a sprite down 80 units.
	Sprites.MoveY("sprite1", 80);

----

.. _SpriteListSetRotation:

:csharp:`public void SetRotation(string key, float r)`

Sets the rotation of the indicated :doc:`Sprite <Sprite>` to the indicated amount. Mainly used for drawing the image at a specific angle.

.. code-block:: csharp

	//Set an images rotation to 90 degrees.
	Sprites.SetRotation("sprite1", 90);

----

.. _SpriteListRotate:

:csharp:`public void Rotate(string key, float r)`

Rotates the indicated :doc:`Sprite <Sprite>` by the indicated amount. A positive number rotates counter-clockwise, and a negative number rotates clockwise.

.. code-block:: csharp

	//Rotate an image clockwise 80 degrees.
	Sprites.Rotate("sprite1", -80);

----

.. _SpriteListScaleX:

:csharp:`public void ScaleX(string key, double scale)`

Sets the horizontal scaling of the :doc:`Sprite <Sprite>`. A number greater than 1 increases the size, while a number between 0 and 1 will reduce the size.

.. code-block:: csharp

	//Double the horizontal size of the sprite
	Sprites.ScaleX("sprite1", 2);

	//Halve the horizontal size of the sprite.
	Sprites.ScaleX("sprite1", 0.5);

----

.. _SpriteListScaleY:

:csharp:`public void ScaleY(string key, double scale)`

Sets the vertical scaling of the :doc:`Sprite <Sprite>`. A number greater than 1 increases the size, while a number between 0 and 1 will reduce the size.

.. code-block:: csharp

	//Double the vertical size of the sprite
	Sprites.ScaleY("sprite1", 2);

	//Halve the vertical size of the sprite.
	Sprites.ScaleY("sprite1", 0.5);

----

.. _SpriteListScale:

:csharp:`public void Scale(string key, double scale)`

Sets the overall scaling of the :doc:`Sprite <Sprite>`. A number greater than 1 increases the size, while a number between 0 and 1 will reduce the size.

.. code-block:: csharp

	//Double the size of the sprite
	Sprites.Scale("sprite1", 2);

	//Halve the size of the sprite.
	Sprites.Scale("sprite1", 0.5);

----

.. _SpriteListGetSize:

:csharp:`public Vector2 GetSize(string key)`

Returns the overall width and height of the indicated :doc:`Sprite <Sprite>` in the form of a :doc:`Vector2 <../Vector2>` object.

.. code-block:: csharp

	Sprites.GetSize("sprite1");

----

.. _SpriteListSetAnchor:

:csharp:`public void SetAnchor(string key, double anchor)`

Sets the anchor of an :doc:`Sprite <Sprite>`. Primarily used for rotation of the object, to choose which point the object rotates round. The anchor value is a value from 0.0 to 1.0 in terms of percentage.

.. code-block:: csharp

	//Set the anchor to the middle of the sprite.
	Spriets.SetAnchor("sprite1", 0.5);

----

.. _SpriteListSetAnchorX:

:csharp:`public void SetAnchorX(string key, double anchor)`

Sets the x value of the anchor of a :doc:`Sprite <Sprite>`. Primarily used for rotation of the object, to choose which point the object rotates round. The anchor value is a value from 0.0 to 1.0 in terms of percentage.

.. code-block:: csharp

	//Set the anchor to the middle of the sprite, lengthwise
	Spriets.SetAnchorX("sprite1", 0.5);

----

.. _SpriteListSetAnchorY:

:csharp:`public void SetAnchorY(string key, double anchor)`

Sets the y value of the anchor of a :doc:`Sprite <Sprite>`. Primarily used for rotation of the object, to choose which point the object rotates round. The anchor value is a value from 0.0 to 1.0 in terms of percentage.

.. code-block:: csharp

	//Set the anchor to the middle of the sprite, heightwise
	Spriets.SetAnchorY("sprite1", 0.5);

----

.. _SpriteListKill:

:csharp:`public void Kill(string key)`

Kills a :doc:`Sprite <Sprite>`. The sprite will no longer draw or update in any way.

.. code-block:: csharp

	Sprites.Kill("sprite1");

----

.. _SpriteListIsAlive:

:csharp:`public bool IsAlive(string key)`

Returns a boolean value that indicates whether or not the indicated :doc:`Sprite <Sprite>` is alive or not.

.. code-block:: csharp

	if (Sprites.IsAlive("sprite1")) {
		//Do some action
	}

----

.. _SpriteListReincarnate:

:csharp:`public void Reincarnate(string key)`

The reverse of the Kill() function. A :doc:`Sprite <Sprite>` will now update, though Display() will still have to be called to allow the :doc:`Sprite <Sprite>` to be drawn.

.. code-block:: csharp

	//Allow a sprite to update and display again.
	Sprites.Reincarnate("sprite1");
	Sprites.Display("sprite1", true);

----

.. _SpriteListSetVelocityX:

:csharp:`public void SetVelocityX(string key, double x)`

Set the horizontal velocity of a :doc:`Sprite <Sprite>`. The :doc:`Sprite <Sprite>` will move at the indicated speed.

.. code-block:: csharp

	//Move a sprite to the right at a speed of 30.
	Sprites.SetVelocityX("sprite1", 30);

----

.. _SpriteListSetVelocityY:

:csharp:`public void SetVelocityY(string key, double y)`

Set the vertical velocity of a :doc:`Sprite <Sprite>`. The :doc:`Sprite <Sprite>` will move at the indicated speed.

.. code-block:: csharp

	//Move a sprite down at a speed of 30.
	Sprites.SetVelocityY("sprite1", 30);

----

.. _SpriteListSetGravityX:

:csharp:`public void SetGravityX(string key, double x)`

Set the horizontal gravity of a :doc:`Sprite <Sprite>`, to accelerate at the indicated speed.

.. code-block:: csharp

	//Accelerate a sprite to the right by 30/update
	Sprites.SetGravityX("sprite1", 30);

----

.. _SpriteListSetGravityY:

:csharp:`public void SetGravityY(string key, double x)`

Set the vertical gravity of a :doc:`Sprite <Sprite>`, to accelerate at the indicated speed.

.. code-block:: csharp

	//Accelerate a sprite down by 30/update
	Sprites.SetGravityY("sprite1", 30);

----

.. _SpriteListSetCollider:

:csharp:`public void SetCollider(string key, Physics.Collider collider)`

Sets the :doc:`Collider <../Physics/Collider>` of a :doc:`Sprite <Sprite>`.

.. code-block:: csharp

	Sprites.SetCollider("sprite1", new EllipseCollider(0, 0, 30, 50));

----

.. _SpriteListAddCollision:

:csharp:`public void AddCollision(string key, string other)`

The :doc:`Sprite <Sprite>` indicated by key will now collide with the :doc:`Sprite <Sprite>` indicated by other. This collision is one-way, the second sprite will not check for collision with the first.

.. code-block:: csharp

	Sprites.AddCollision("sprite1", "sprite2");

.. warning::
	Not fully functional. Will not work in all cases.

----

.. _SpriteListGetSprite:

:csharp:`public Sprite GetSprite(string key)`

Returns a copy of the :doc:`Sprite <Sprite>` to view values if necessary. In the future, may return a reference instead.