using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace MODFReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("Data");         
            Console.WriteLine("Welcome to Dovah's MODF Reader.");
            Console.WriteLine("Please enter the path to your map folder.");      
            string path = Console.ReadLine();
            {

                string[] obj1files = Directory.GetFiles(path, "*_obj0*", System.IO.SearchOption.AllDirectories);

             //   string[] adtFiles = Directory.GetFiles(path, "*.adt*", System.IO.SearchOption.AllDirectories);
                int wmocount = 0;
                foreach (var obj1file in obj1files)
                {
                  
                    string[] listfile = File.ReadAllLines(@"listfile.txt");
                    using (Stream stream = File.Open(obj1file, FileMode.Open))
                    using (BinaryReader reader = new BinaryReader(stream))

                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)

                        {
                            bool modf_seen = false;
                            var magic = reader.ReadUInt32();
                            var size = reader.ReadUInt32();
                            var pos = reader.BaseStream.Position;

       
                          
                            if (magic == 1297040454)
                            {
                                while (reader.BaseStream.Position < pos + size)
                                {
                                    modf_seen = true;
                                    //  Console.WriteLine(magic);
                                    Console.WriteLine("Processing: " + obj1file + "-" + wmocount);
                                    var nameid = reader.ReadUInt32();
                                    var uniqueID = reader.ReadUInt32();
                                    float positionx = reader.ReadSingle();
                                    float positiony = reader.ReadSingle();
                                    float positionz = reader.ReadSingle();
                                    float rotationx = reader.ReadSingle();
                                    float rotationy = reader.ReadSingle();
                                    float rotationz = reader.ReadSingle();
                                    float extentsminx = reader.ReadSingle();
                                    float extentsminy = reader.ReadSingle();
                                    float ententsminz = reader.ReadSingle();
                                    float extentsmaxx = reader.ReadSingle();
                                    float extentsmaxy = reader.ReadSingle();
                                    float extentsmaxz = reader.ReadSingle();
                                    var flags = reader.ReadUInt16();

                                    var doodadset = reader.ReadUInt16();
                                    var nameset = reader.ReadUInt16();
                                    var scale = reader.ReadUInt16();
                                    string adtname = Path.GetFileNameWithoutExtension(obj1file);
                                    var adtname2 = adtname.Split('_');
                                    foreach (string line in listfile)
                                    {

                                        var filenameid = line.Split(';');
                                        if (filenameid[0] == nameid.ToString())
                                        {
                                            File.AppendAllText(@"Data\WMOData.csv", adtname2[0] + "_" + adtname2[1] + "_" + adtname2[2] + "," + nameid.ToString() + "," + filenameid[1] + "," + nameset.ToString() + Environment.NewLine);
                                            File.AppendAllText(@"Data\" + adtname2[0] + ".csv", adtname2[0] + "_" + adtname2[1] + "_" + adtname2[2] + "," + nameid.ToString() + "," + filenameid[1] + "," + nameset.ToString() + Environment.NewLine);
                                        }
                                    }
                                    //   File.AppendAllText(@"Data\test.csv", "nameid " + nameid.ToString() + "nameset: " + nameset.ToString() +  Environment.NewLine);

                                }
                            }
                            if (modf_seen == false)
                            {
                                Console.WriteLine(obj1file + ": No WMOs present.");
                            }
                            wmocount++;
                            reader.BaseStream.Position = pos + size;
                        }
                        wmocount = 0;
                    }
                    
                }
               
                }
            Console.WriteLine("Done Processing, Please enter the path to your csv files so I can add headers.");
            string csvfiles = Console.ReadLine();
            string[] csvfiles1 = Directory.GetFiles(csvfiles, "*.csv*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var csvfiles2 in csvfiles1)
            {
                StreamReader header = new StreamReader(csvfiles2);
                string initialtext = header.ReadToEnd();
                header.Close();
                string endtext = "ADT,FileDataID,FilePath,NamesetEntry" + Environment.NewLine + initialtext;
                StreamWriter headerwriter = new StreamWriter(csvfiles2, false);
                headerwriter.WriteLine(endtext);
                headerwriter.Close();
            }

            Console.WriteLine("Done! press any key to exit");
            Console.ReadKey();
        }

    }
}

 

