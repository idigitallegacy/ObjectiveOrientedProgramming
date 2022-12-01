using Banks.Models.TimeFlowConcept;

namespace Banks.Console;

public class TimeFlowInterface
{
    private TimeFlowInterface() { }

    public static void Execute()
    {
        bool loopFlag = true;
        while (loopFlag)
        {
            System.Console.WriteLine("Choose action:");
            System.Console.WriteLine("\t 1) Scroll time");
            System.Console.WriteLine("\t 2) Get time");
            System.Console.WriteLine("\t 0) << Back");

            switch (Convert.ToInt32(System.Console.ReadLine()))
            {
                case 1:
                {
                    DateTime newDate = RegisterInterface.SetupDate("\t");
                    try
                    {
                        TimeFlow.Instance.SetTime(newDate);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("\tUnable to set new time:");
                        System.Console.WriteLine(e);
                    }

                    break;
                }

                case 2:
                {
                    System.Console.WriteLine(TimeFlow.Instance.Now);
                    break;
                }

                case 0:
                {
                    loopFlag = false;
                    break;
                }
            }
        }
    }
}