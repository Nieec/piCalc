using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public class piSpigotAlgorithm
{
    private static string piCheck = "";
    private static string calculatedPi = "";
    private static bool isDone;

    public static void Main()
    {
        Console.WriteLine("Wähle Anzahl Nachkommastellen:");
        int.TryParse(Console.ReadLine(), out int digits);

        Task.WaitAll(
            printTimeAsync(),
            loadChecker(digits),
            calculatePiToDigit(digits)
            );
        if (piCheck.Equals(calculatedPi))
        {
            Console.WriteLine("Check complete: Calculation is accurate");
        }
        writeToFile();
        
    }

    private static async Task calculatePiToDigit(int digits)
    {
        await Task.Yield();

        isDone = false;
        int[] quotient = fillQuotient(digits);
        int[] rest = new int[digits * 10 / 3];
        //int nines = 0;
        //int predigit = 0;

        int[] pie = new int[digits];

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
            pie[j] = quotient[0] / 10;
            for (int k = 0; k < quotient.Length; k++)
            {
                quotient[k] = rest[k] * 10;
            }
        }

        int carryfix = 0;
        for (int i = digits - 1; i >= 0; i--)
        {
            pie[i] += carryfix;
            carryfix = pie[i] / 10;
            calculatedPi = (pie[i] % 10).ToString() + calculatedPi;
        }
        calculatedPi = calculatedPi.Insert(1, ".");

        isDone = true;
        Console.WriteLine("Calculation complete.");
    }

    static int[] fillQuotient(int digits)
    {
        int[] q = new int[digits * 10 / 3];
        for (int i = 0; i < (digits * 10 / 3); i++)
        {
            q[i] = 20;
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

    static async Task loadChecker(int digits)
    {
        await Task.Yield();
        char[] text = File.ReadAllText("/users/nicsauer/pi1000000.txt").ToCharArray();
        for (int i = 0; i < (digits + 1); i++)
        {
            piCheck += text[i];
        }
    }

    static void writeToFile()
    {
        //File.WriteAllTextAsync("", calculatedPi);
    }
}