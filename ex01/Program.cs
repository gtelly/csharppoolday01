using System;
using System.IO;
using System.Text.RegularExpressions;

namespace d00_ex01
{
    class Program
    {
        static int Main()
        {
            string[] knownLogins;
            string login;
            string answer;
            int numOfLogins;
            int sd;

            try
            {
                knownLogins = File.ReadAllLines(@"./us.txt");
                numOfLogins = knownLogins.Length;
                Console.WriteLine(">Enter name:");
                login = GetValidName();
                for (int z = 0; z < 3; z++)
                {
                    for (int i = 0; i < numOfLogins; i++)
                    {
                        sd = StrDist(login, knownLogins[i]);
                        if (sd == z)
                        {
                            answer = "Y";
                            while (answer != "N")
                            {
                                Console.WriteLine(">Did you mean “{0}”? Y/N", knownLogins[i]);
                                answer = Console.ReadLine();
                                if (answer == "Y")
                                {
                                    Console.WriteLine(">Hello, {0}!", knownLogins[i]);
                                    return (1);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Your name was not found.");
                return (0);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("us.txt not found");
                return (-1);
            }
        }
        public static string GetValidName()
        {
            bool validName;
            string inpName;

            validName = false;
            inpName = "";
            while (!validName)
            {
                inpName = "";
                inpName = Console.ReadLine();
                validName = Regex.IsMatch(inpName.Trim(), "^([A-Za-z])+$");
                if (!validName)
                    Console.WriteLine("Invalid name, please repeat input.");
            }

            return (inpName);
        }
        public static int StrDist(string value1, string value2)
        {
            int res;
            if (value2.Length == 0)
                return value1.Length;
            int[] costs = new int[value2.Length];
            for (int i = 0; i < costs.Length;)
                costs[i] = ++i;
            for (int i = 0; i < value1.Length; i++)
            {
                int cost = i;
                int previousCost = i;
                char value1Char = value1[i];
                for (int j = 0; j < value2.Length; j++)
                {
                    int currentCost = cost;
                    cost = costs[j];
                    if (value1Char != value2[j])
                    {
                        if (previousCost < currentCost)
                            currentCost = previousCost;
                        if (cost < currentCost)
                            currentCost = cost;
                        ++currentCost;
                    }
                    costs[j] = currentCost;
                    previousCost = currentCost;
                }
            }
            res = costs[costs.Length - 1];
            return (res);
        }
    }
}