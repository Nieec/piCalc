using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public class piSpigotAlgorithm
{
    private static bool isDone;

    public static void Main()
    {
        Console.WriteLine("Wähle Anzahl Nachkommastellen:");
        int.TryParse(Console.ReadLine(), out int digits);

        Task.WaitAll(
            printTimeAsync(),
            calculatePiToDigit(digits)
            );

        checkForCorrectResult(digits);
    }

    private static async Task calculatePiToDigit(int digits)
    {
        await Task.Yield();

        isDone = false;
        int[] quotient = fillQuotient(digits);
        int[] rest = new int[digits * 10 / 3];
        int[] pi = new int[digits];
        int carryFix = 0;
        //int nines = 0;
        //int predigit = 0;
        string calculatedPi = "";


        for (int j = 0; j < digits; j++)
        {
            for (int i = quotient.Length - 1; i >= 0; i--)
            {
                int carry = 0;
                int num = i;
                int enumerator;
                if (i > 0)
                {
                    enumerator = i * 2 + 1;
                }
                else
                {
                    enumerator = 10;
                }

                rest[i] = quotient[i] % enumerator;
                carry = quotient[i] / enumerator;

                if (i > 0)
                {
                    quotient[i - 1] += carry * num;
                }
            }
            pi[j] = quotient[0] / 10;
            for (int k = 0; k < quotient.Length; k++)
            {
                quotient[k] = rest[k] * 10;
            }
        }

        for (int i = digits - 1; i >= 0; i--)
        {
            pi[i] += carryFix;
            carryFix = pi[i] / 10;
            calculatedPi = (pi[i] % 10).ToString() + calculatedPi;
        }
        calculatedPi = calculatedPi.Insert(1, ".");

        isDone = true;
        Console.WriteLine("Calculation complete.");
        writeToFile(calculatedPi);
    }

    static int[] fillQuotient(int digits)
    {
        int[] q = new int[digits * 10 / 3];
        for (int i = 0; i < (digits * 10 / 3); i++)
        {
            q[i] = 2 * 10;
        }
        return q;
    }

    static async Task printTimeAsync()
    {
        while (!isDone)
        {
            Console.WriteLine(DateTime.Now);
            await Task.Delay(1000);
        }
    }

    static void checkForCorrectResult(int digits)
    {
        if(File.ReadAllText("/workspaces/piCalc/piTest/piTest/pi1000000.txt").Remove(digits+1)
        .Equals(File.ReadAllText("/workspaces/piCalc/piTest/piTest/piOutput.txt"))) {
            Console.WriteLine("Calculation correct.");
        } else {
            Console.WriteLine("Calculation incorrect");
        }
    }

    static void writeToFile(string calculatedPi)
    {
        File.WriteAllTextAsync("/workspaces/piCalc/piTest/piTest/piOutput.txt", calculatedPi);
    }
}