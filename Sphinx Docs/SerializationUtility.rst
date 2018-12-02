====================
SerializationUtility
====================

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

A static class offering serialization functionality.

.. note::
   This class does not currently work with generic types.

Methods
^^^^^^^

.. _SerializationUtilitySerialize:

:csharp:`static byte[] Serialize(object obj, bool cacheFields = true)`

Serializes a given object into a byte array and returns it. Parameter "cacheFields"
controls whether or not to cache information about the object type to speed up
future calls to this method or :ref:`Deserialize <SerializationUtilityDeserialize>`

.. warning::
   If parameter "obj" is not serializable, an error will be thrown.

----

.. _SerializationUtilityDeserialize:

:csharp:`static T Deserialize<T>(byte[] bytes, bool cacheFields = true`

Deserializes a given byte array into a new object of type T and returns this new
object. Parameter "cacheFields" controls whether or not to cache information about the object type to speed up future calls to this method or
:ref:`Serialize <SerializationUtilitySerialize>`

.. warning::
   Can potentially throw ArgumentException or ArgumentNullException with invalid
   inputs.

----

.. _SerializationUtilityIsSerializable:

:csharp:`static bool IsSerializable(object obj)`

:csharp:`static bool IsSerializable(Type t)`

Returns a bool indicating if the object can be serialized.

.. warning::
   Potentially throws NullReferenceException.

Sub-Classes
^^^^^^^^^^^

.. _SerializationUtilityNonSerializable:

NonSerializable
---------------

An attribute that can be applied to a field or class to prevent it from being
serialized.
