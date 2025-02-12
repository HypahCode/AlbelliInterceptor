Albelli Photobook interceptor
=============================

Creates a PDF from from your photobook while ordering.
Optionally it kills the software before copying to prevent it from sending your book to Albelli.
Photobook software: [https://www.albelli.nl/](https://www.albelli.nl/fotoboek-maken/beginnen/kies-programma)

Not verified, but it might also work with Photobox, PosterXXL, fotoknudsen, hofmann and onskefoto software as they all belong to Storio Group and use the same software -> https://www.storiogroup.com/our-brands/

How it works:
-------------

While ordering a photobook, it will create a PDF called ORDER.APP in your local application data folder (C:\Users\USERNAME\AppData\Local\Temp). 
This file is put in a zip file, and then deleted from disk.
This tool registers the creation of ORDER.APP, and when it sees that the zip file is created, it knows that ORDER.APP is written.
It will copy the ORDER.APP file to the desktop as Photobook.pdf, and optionally kill the software to prevent it from creating an order online.

