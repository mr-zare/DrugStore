using System;

namespace project
{
    class Program
    {
        static void UserActions(int flag){
            if (flag==1)
            {
                
            }
            else if(flag==2)
            {
                
            }
            else if(flag==3)
            {
                
            }
            else if(flag==4)
            {
                
            }
            else if(flag==5)
            {
                
            }
            else if(flag==6)
            {
                
            }
            else if(flag==7)
            {
                
            }
            else
            {
                throw new Exception("Invalid input!! your input is not in the correct range!!");
            }
        }
        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("         Welcome Back!");
            Console.ForegroundColor = ConsoleColor.White;
            int flag = 0;
            while (flag!=8)
            {
                Console.WriteLine();  
                Console.WriteLine("Enter your desired option:");
                Console.WriteLine("1.Reading data files");
                Console.WriteLine("2.Presence or absence of drug interactions in a prescription drug");
                Console.WriteLine("3.Evaluate the presence or absence of drug allergy in a prescription with referring disease");
                Console.WriteLine("4.Calculating the price of prescription drugs");
                Console.WriteLine("5.Rising prices of drugs");
                Console.WriteLine("6.Adding or removing from the data structures");
                Console.WriteLine("7.Search");
                Console.WriteLine("8.Exit");
                try
                {
                    
                    flag= int.Parse(Console.ReadLine());
                    Console.Clear();
                }
                catch (System.Exception)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    
                    Console.WriteLine("Invalid input!! your input is not a number!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                if(flag==8) break;
                try
                {
                     UserActions(flag);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
            


        }
    }
}


