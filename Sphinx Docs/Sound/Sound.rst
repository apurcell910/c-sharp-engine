=====
Sound
=====

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp

Provides an interface through which to play audio. This class cannot be instantiated
directly, for creating a Sound use
:ref:`ContentManager.AddSound <ContentManagerAddSound>` and
:ref:`ContentManager.GetSound <ContentManagerGetSound>`.

Events
^^^^^^

.. _SoundSoundEvent:

:csharp:`delegate void SoundEvent(Sound sound)`

The delegate type used for all Sound events. Contains the Sound calling the event as a
parameter.

----

.. _SoundPlaybackFinished:

:csharp:`event SoundEvent PlaybackFinished`

Called when the Sound completes playing.

Fields/Properties
^^^^^^^^^^^^^^^^^

.. _SoundLoop:

:csharp:`bool SoundLoop { get; set; }`

Gets or sets a bool indicating if the Sound should automatically restart on reaching
the end of playback. By default, this value is false.

.. note::
   This setting will be ignored if :ref:`DisposeOnFinished <SoundDisposeOnFinished>`
   is true.

----

.. _SoundDisposeOnFinished:

:csharp:`bool DisposeOnFinished { get; set; }`

Gets or sets a bool indicating if the sound should be automatically disposed after
playing.

----

.. _SoundVolume:

:csharp:`int Volume { get; set; }`

Gets or sets the volume of the Sound on a scale from 0-100. Values outside of this
range will be ignored.

----

.. _SoundMuted:

:csharp:`bool Muted { get; set; }`

Gets or sets a bool indicating if the Sound is muted.

----

.. _SoundPosition:

:csharp:`float Position { get; set; }`

Gets or sets the position in playback of the Sound on a scale from
0-:ref:`Duration <SoundDuration>`. Values outside of this range will be ignored.

----

.. _SoundSpeed:

:csharp:`float Speed { get; set; }`

Gets or sets the speed at which the Sound plays on a scale from 0-infinity. By default,
this value is 1.

----

.. _SoundDuration:

:csharp:`float Duration { get; }`

Gets the length of the Sound in seconds.

Methods
^^^^^^^

.. _SoundPause:

:csharp:`void Pause()`

Pauses playback of the Sound.

----

.. _SoundStop:

:csharp:`void Stop()`

Stops playback of the Sound.

----

.. _SoundPlay:

:csharp:`void Play()`

Resumes or restarts playback of the Sound, depending on if it is paused.

----

.. _SoundDispose:

:csharp:`void Dispose()`

Returns the Sound to the pool of cached Sounds so that it may be reused elsewhere.

.. warning::
   If sounds are not disposed properly,
   :ref:`ContentManager.GetSound <ContentManagerGetSound>` may become unable to return
   Sound instances.
