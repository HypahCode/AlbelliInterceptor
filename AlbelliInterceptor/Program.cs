
using AlbelliInterceptor;

Console.WriteLine("Albelli Photobook interceptor.");
// https://www.albelli.nl/
// (Not verified, but it might also work with Photobox, PosterXXL, fotoknudsen,
// hofmann and onskefoto software as they all belong to Storio Group and use the
// same software -> https://www.storiogroup.com/our-brands/)
Console.WriteLine("Captures the photobook as PDF while ordering and copies it to desktop as Photobook.pdf");
Console.WriteLine("");
Console.WriteLine("Do you want to kill the Albelli software before copying the PDF?");
Console.WriteLine("      Killing prevents sending the data to albelli");
Console.WriteLine("Kill software before copying? Y/N? : ");

var key = Console.ReadKey();
bool killSoftware = false;
if (key.KeyChar == 'Y' || key.KeyChar == 'y')
{
    Console.WriteLine("Killing software before order process -> YES");
    killSoftware = true;
}

var monitor = new AlbelliFileMonitor(killSoftware);

Console.WriteLine("Running, press key to stop...");
Console.ReadKey();

