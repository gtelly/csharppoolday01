using System;

namespace d00_ex00
{
    class Program
    {
        static void Main(string[] args)
        {
            double sum = 1000000;
            double payment = 100000;
            double rate = 12.0;
            int selectedMonth = 5, term = 10;
            double ovrReducePayment, ovrReduceTerm;
            if (args.Length == 0)
                Console.WriteLine("If no arg(s), default(s) will be used!");
            if (args.Length > 0)
                double.TryParse(args[0], out sum);
            Console.WriteLine("'sum' is {0}", sum.ToString("N2"));
            if (args.Length > 1)
                double.TryParse(args[1], out rate);
            Console.WriteLine("'rate' is {0}", rate.ToString("N2"));
            if (args.Length > 2)
                int.TryParse(args[2], out term);
            Console.WriteLine("'term' is {0}", term);
            if (args.Length > 3)
                int.TryParse(args[3], out selectedMonth);
            Console.WriteLine("'selectedMonth' is {0}", selectedMonth);
            if (args.Length > 4)
                double.TryParse(args[4], out payment);
            Console.WriteLine("'payment' is {0:N2}\nCalculating loan overpayment..\n", payment);
            ovrReducePayment = loanCost(sum, rate, term, selectedMonth, payment, true);
            ovrReduceTerm = loanCost(sum, rate, term, selectedMonth, payment, false);
            Console.WriteLine("Overpayment in case of a decrease in payment: {0:N2} rub.", ovrReducePayment);
            Console.WriteLine("Overpayment in case of a decrease in the term: {0:N2} rub.", ovrReduceTerm);
            if (ovrReducePayment < ovrReduceTerm)
                Console.WriteLine("Decrease in payment is better, you profit is: {0:N2} rub.", (ovrReduceTerm - ovrReducePayment));
            else if (ovrReducePayment > ovrReduceTerm)
                Console.WriteLine("Decrease in the term is better, you profit is: {0:N2} rub.",  (ovrReducePayment - ovrReduceTerm));
            else
            {
                Console.WriteLine("Does not matter :)");
            }
        }

        public static double loanCost(double sum, double rate, int term, int selectedMonth, double payment, bool reducePayment)
        {
            double currentPrincipal, interestAmount, partOfPrincipal, paymentMonthly, totalInterestAmount = 0;
            DateTime startDate, endDate;
            double yearDays;
            double totalDays;

            yearDays = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
            
            startDate = DateTime.Now;
            paymentMonthly = Pmt(rate, term, sum);
            Console.WriteLine($"No    Payment   Part of Principal   Part of Interest   Principal Balance  Date of paymentMonthly");
            currentPrincipal = sum;
            for (int i = 1; i <= term; i++)
            {
                endDate = startDate.AddMonths(1);
                totalDays = (endDate - startDate).TotalDays;
                interestAmount = (currentPrincipal * rate * totalDays) / (100.00 * yearDays);
                totalInterestAmount += interestAmount;
                partOfPrincipal = paymentMonthly - interestAmount;
                currentPrincipal = currentPrincipal - partOfPrincipal;
                if (reducePayment || (!reducePayment && currentPrincipal > 0))
                    Console.WriteLine("{0,-3}{1,11:N2}{2,15:N2}{3,20:N2}{4,20:N2}{5,17:dd-MMM-yy}", i, paymentMonthly,
                        partOfPrincipal, interestAmount, currentPrincipal, endDate);
                else
                {
                    Console.WriteLine("{0,-3}{1,11:N2}{2,15:N2}{3,20:N2}{4,20:N2}{5,17:dd-MMM-yy}", i,
                        (paymentMonthly + currentPrincipal), (partOfPrincipal + currentPrincipal), interestAmount,
                        (currentPrincipal - currentPrincipal), endDate);
                    break;

                }
              
                if (i == selectedMonth)
                    if (reducePayment)
                    {
                        currentPrincipal -= payment;
                        paymentMonthly = Pmt(rate, term - selectedMonth, currentPrincipal);
                        Console.WriteLine("{0,-3}{1,11:N2}{2,15:N2}{3,20:N2}{4,20:N2}{5,17:dd-MMM-yy} <-- reducing monthly payment",
                            " ", payment, payment, 0.0, currentPrincipal, endDate);
                    }
                    else
                    {
                        currentPrincipal -= payment;
                        Console.WriteLine("{0,-3}{1,11:N2}{2,15:N2}{3,20:N2}{4,20:N2}{5,17:dd-MMM-yy} <-- reducing credit term",
                            " ", payment, payment, 0.0, currentPrincipal, endDate);
                    }
                startDate = endDate;
            }
            Console.WriteLine("Total Interest paid: {0, 28}\n", totalInterestAmount.ToString("N2"));
            return (totalInterestAmount);
        }
        
        public static double Pmt(double yearlyInterestRate, int totalNumberOfMonths, double principal)
        {
            double rate = yearlyInterestRate / 100 / 12;
            double denominator = Math.Pow((1 + rate), totalNumberOfMonths) - 1;
            return (rate + (rate/denominator)) * principal;
        }
    }
}