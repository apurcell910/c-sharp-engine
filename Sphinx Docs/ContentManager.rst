===============
Content Manager
===============

.. toctree::
   :maxdepth: 4

.. role:: csharp(code)
   :language: csharp
   
A basic Content Manager that contains external assets that can 
be used in game. Assets that can be stored using the manager
include external images, sounds and fonts.

Methods
^^^^^^^^
.. _ContentManagerAddImage:

:csharp:`void AddImage(string filePath, string fileName, int scale)`

Stores an external image into a dictionary containing all stored 
images in the Content Manager. Image is grabbed by the given filePath 
and can be given an optional fileName and can be scaled with a given
scale.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Grab an image called "test.png" name it "test" and scale it by half
   content.AddImage("test.png", "test", 0.5)

----

.. _ContentManagerAddFont:

:csharp:`void AddFont(string filePath, string fileName)`

Stores an external font type into a font cache. Font is grabbed
from the string filePath and given a certain fileName that is to
be stored in the font cache.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Grab a certain font from a filePath and stored in the cache with the fileName
   content.AddFont("consolas.ttf", "new_font");

----

.. _ContentManagerGetFont:

:csharp:`Font GetFont(string name, int fontSize)`

Returns a font from the font cache using the font's name
that was set in :ref:`GetFont`. Font size can be set using
optional argument fontSize and defaulted at 32.

.. code-block:: csharp
   
	// Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   Font new_font = content.GetFont("new_font");

----

.. _ContentManagerAddSound:

:csharp:`void AddSound(string filePath, string fileName, int cacheSize)`

Adds a sound file into a sound cache which exists within the
Content Manager. File is found using the filePath and the name 
of the file is set with fileName. The optional argument cacheSize
is the amount of sounds to load defaulted at once.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Add a file "music.mp3" and name it "main_theme" 
   // and allow allow it to load once
   content.AddSound("music.mp3", "main_theme", 1);

----

.. _ContentManagerGetSound:

:csharp:`Sound.Sound GetSound(string name)`

Pull a cached sound from the Content Manager with the 
given name "name".

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Create a new sound and set it to a sound pulled from the
   // Content Manager
   Sound main_theme = Content.GetSound("main_theme");

----
   
.. _ContentManagerScaleImageBitmap:

:csharp:`Bitmap ScaleImage(Bitmap bmp, int scale)`

Scale a given Bitmap image with an integer scale. 
Returns the newly scaled Bitmap.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Scale an bitmap by half
   Bitmap bmp = new Bitmap("test.bmp");
   Bitmap scale_test = ScaleImage(bmp, 2);

----

.. _ContentManagerScaleImageBitmapArray:

:csharp:`Bitmap[] ScaleImage(Bitmap bmp[], int scale)`

Scales an array of Bitmap images all with the scaling factor.
Returns newly scaled Bitmap array.

.. code-block:: csharp
   
   //Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Scale an array of bitmaps
   int num_iamges = 5;
   Bitmap bmp[num_images];
   for(int i = 0; i < num_images; i++)
   {
		Bitmap bmp[i] = new Bitmap("test.png")
   }
   
   // Scale all images in bmp by half
   Bitmap test[num_images] = ScaleImage(bmp, 2);

----

.. _ContentManagerGetImage:

:csharp:`Bitmap GetImage(string name)`

Returns a Bitmap image from the Content Manager
using the given name that is used to store the image
in the Content Manager.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Add an image to the Content Manager and use the 
   // GetImage function to pull it from the Content Manager
   content.AddImage("test.png", "test");
   Bitmap bmp = content.GetImage("test");

----

.. _ContentManagerInManager:

:csharp:`bool InManager(string name)`

Checks to see if a given image is in the Content Manager.
Returns true if the image exists within the Content Manager
and false otherwise.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Add an image to the Content Manager and check if
   // it exists in there.
   content.AddImage("test.png", "test");
   if(content.InManager("test"))
   {
		Console.WriteLine("Image was found");
   }

----

.. _ContentManagerPrintNames:

:csharp:`void PrintNames()`

Prints the names of all images that are stored
within the Content Manager.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // Add images to the Content Manager and print their names
   content.AddImage("test1.png", "test1");
   content.AddImage("test2.png", "test2");
   content.AddImage("test3.png", "test3");
   
   content.PrintNames();
   
   // This will print the following
   // test1
   // test2
   // test3

----

.. _ContentManagerSplitImageString:

:csharp:`Bitmap[] SplitImage(string filePath, int numCuts, string fileNames)`

Tiles an external image that is pulled from filePath. The numCuts argument is
how many tiles are cut by cutting the image numCuts times both vertically and horizontally.
Returns a Bitmap array of the tiled images. Argument fileNames is an optional name for each 
new image in the return Bitmap array. The new array is also stored in the Content Manager.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   // This will create an array of 8 Bitmap named with each index named test1, test2 ... test8
   Bitmap test[] = content.SplitImage("test.png", 4, "test");

----

.. _ContentManagerSplitImageBitmap:

:csharp:`Bitmap[] SplitImage(Bitmap bmp, int numCuts, string fileNames)`

Tiles an internal image. The numCuts argument is how many tiles are cut by 
cutting the image numCuts times both vertically and horizontally. Returns a Bitmap
array of the tiled images. Argument fileNames is an optional name for each 
new image in the return Bitmap array. The new array is also stored in the Content Manager.

.. code-block:: csharp
   
   // Create a ContentManager by passing a game reference
   ContentManager content = new ContentManager(game);
   
   Bitmap bmp = new Bitmap("test.png");
   
   // This will create an array of 8 Bitmap named with each index named test1, test2 ... test8
   Bitmap test[] = content.SplitImage(bmp, 4, "test");
