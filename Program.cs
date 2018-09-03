using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs
{

    internal class Program
    {


        public static void Main(string[] args)
        {
            try
            {

                Commands c = null;
                if(args.Length == 0){
                    c = new Commands();
                }
                else if(args.Length == 1){
                    c = new Commands(args[0]);
                }
                else{
                    c = new Commands(args[0], args[1]);
                }
                c.e();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error!\n\"{ex.FileName}\" was not found.");
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }




        }
    }
}
