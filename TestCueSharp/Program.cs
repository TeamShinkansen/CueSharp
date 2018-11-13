using System;
using System.Text;
using CueSharp;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenExistingFile();

            Console.WriteLine("\n\n");

            NewCueSheet();

            //keep the console open 'till enter is pressed
            Console.Read();
        }

        /// <summary>
        /// Create a new cuesheet from scratch
        /// </summary>
        static void NewCueSheet()
        {
            CueSheet cue = new CueSheet();

            //Album performer
            cue.Performer = "Rotterdam Philharmonic Orchestra";
            //Album title
            cue.Title = "Edo de Waart / Rachmaninoff: The 3 Symphonies, The Rock - Disc 1";

            //Create 1st track, with a filename that future tracks will inherit
            cue.AddTrack("Symphony No. 2 in E minor, Op. 27: I. Largo - Allegro moderato", "", "CDImage.ape", FileType.WAVE);
            cue.AddIndex(0, 0, 0, 0, 0);
            cue.AddIndex(0, 1, 0, 0, 33);

            //Create 2nd track, with optional 'Performance' field used
            cue.AddTrack("II. Allegro molto", "Fake G. Uy: Timpani");
            cue.AddIndex(1, 0, 18, 39, 33);
            cue.AddIndex(1, 2, 22, 14, 10); //add another index we'll delete later
            cue.AddIndex(1, 1, 18, 44, 25);

            //Create 3rd track
            cue.AddTrack("III. Adagio", "");
            cue.AddIndex(2, 0, 27, 56, 33);
            cue.AddIndex(2, 1, 27, 59, 40);

            //Create 4th track using a method that gives us more control over the data
            Track tempTrack = new Track(4, DataType.AUDIO);
            tempTrack.Title = "IV. Allegro vivace";
            tempTrack.ISRC = "0000078652395";
            tempTrack.AddFlag(Flags.CH4);
            tempTrack.AddIndex(0, 41, 57, 33);
            tempTrack.AddIndex(1, 42, 00, 60);
            cue.AddTrack(tempTrack);

            //Create 5th track we'll delete later
            cue.AddTrack("Symphony No. Infinity", "Rachmaninoff's Dog");


            //Remove the bad index from the 2nd track
            cue.RemoveIndex(1, 1);//(trackIndex, indexIndex)
            //Notice the index (array-wise) of the Index (cuesheet min/sec/frame) is '1'
            //but the Index Number is 2. This is to show that index and the Index Number are
            //not the same thing and may or may not be equal.

            //Remove the 5th track
            cue.RemoveTrack(4);

            Console.WriteLine(cue.ToString());
            cue.SaveCue("newCDImage.cue");
        }

        /// <summary>
        /// Open an manipulate an existing cuesheet.
        /// </summary>
        static void OpenExistingFile()
        {
            //Incomplete FreeDB feature
            //Console.WriteLine(cue.CalculateCDDBdiscID());
            //Console.Read();

            //open a cuesheet from file with the default encoding.
            CueSheet cue = new CueSheet("CDImage.cue");

            //print out the title from the global section of the cue file
            Console.WriteLine(cue.Title);

            //print out the performer from the global section of the cue file
            Console.WriteLine(cue.Performer);

            //Show how many track there are
            Console.WriteLine("There are " + cue.Tracks.Length.ToString() + "tracks in this cue file.");

            //Write out the first track
            Console.WriteLine(cue[0].ToString());

            Console.WriteLine("------ Start CueSheet -------");
            Console.WriteLine(cue.ToString());
            Console.WriteLine("------  End CueSheet  -------");

            //print out the title of the second track
            Console.WriteLine(cue[1].Title);

            //print out the Minutes, seconds, and frames of the first index of the second track.
            Console.WriteLine(cue[1][0].Minutes);
            Console.WriteLine(cue[1][0].Seconds);
            Console.WriteLine(cue[1][0].Frames);

            //Print out the image filename
            Console.WriteLine("Data file location: " + cue[0].DataFile.Filename);

            //change the title of the cuesheet
            cue.Title = "Great Music";

            //Manipulate the first track
            Track tempTrack = cue[0];//create a tempTrack to change info
            tempTrack.AddFlag(Flags.CH4);//add a new flag
            tempTrack.Title = "Wonderful track #1";//change the title
            cue[0] = tempTrack;//Set the 1st track with the newly manipulated track.

            cue.SaveCue("newcue.cue");

        }
    }
}
