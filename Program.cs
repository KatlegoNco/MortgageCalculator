using System;
using System.Collections.Generic;

public class MortgageCalculator
{
    // Method to calculate monthly repayment amount
    public double CalculateMonthlyRepayment(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        double monthlyInterestRate = annualInterestRate / 12 / 100;
        int numberOfPayments = loanTermYears * 12;
        double monthlyRepayment = (loanAmount * monthlyInterestRate) / (1 - Math.Pow(1 + monthlyInterestRate, -numberOfPayments));
        return monthlyRepayment;
    }

    // Method to calculate total amount of interest paid over the life of the loan
    public double CalculateTotalInterestPaid(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        double totalRepayment = monthlyRepayment * loanTermYears * 12;
        double totalInterestPaid = totalRepayment - loanAmount;
        return totalInterestPaid;
    }

    // Method to calculate total amount paid over the life of the loan
    public double CalculateTotalAmountPaid(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        return loanAmount + CalculateTotalInterestPaid(loanAmount, annualInterestRate, loanTermYears);
    }

    // Method to generate amortization schedule
    public List<AmortizationEntry> GenerateAmortizationSchedule(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        List<AmortizationEntry> schedule = new List<AmortizationEntry>();

        double monthlyInterestRate = annualInterestRate / 12 / 100;
        int numberOfPayments = loanTermYears * 12;
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);

        double balance = loanAmount;
        for (int month = 1; month <= numberOfPayments; month++)
        {
            double interestPayment = balance * monthlyInterestRate;
            double principalPayment = monthlyRepayment - interestPayment;
            balance -= principalPayment;

            schedule.Add(new AmortizationEntry(month, monthlyRepayment, interestPayment, principalPayment, balance));
        }

        return schedule;
    }
}

// Data structure for amortization schedule entry
public class AmortizationEntry
{
    public double PaymentNumber { get; set; }
    public double PaymentAmount { get; set; }
    public double InterestPaid { get; set; }
    public double PrincipalPaid { get; set; }
    public double RemainingBalance { get; set; }

    public AmortizationEntry(double paymentNumber, double paymentAmount, double interestPaid, double principalPaid, double remainingBalance)
    {
        PaymentNumber = paymentNumber;
        PaymentAmount = paymentAmount;
        InterestPaid = interestPaid;
        PrincipalPaid = principalPaid;
        RemainingBalance = remainingBalance;
    }
}


class Program
{
    static void Main(string[] args)
    {
        MortgageCalculator calculator = new MortgageCalculator();

        // Prompt user for input
        Console.Write("Enter loan amount: ");
        double loanAmount = double.Parse(Console.ReadLine());

        Console.Write("Enter annual interest rate (%): ");
        double annualInterestRate = double.Parse(Console.ReadLine());

        Console.Write("Enter loan term in years: ");
        int loanTermYears = int.Parse(Console.ReadLine());
        
        // Calculate and print outputs
        double monthlyRepayment = calculator.CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        double totalInterestPaid = calculator.CalculateTotalInterestPaid(loanAmount, annualInterestRate, loanTermYears);
        double totalAmountPaid = calculator.CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
        List<AmortizationEntry> amortizationSchedule = calculator.GenerateAmortizationSchedule(loanAmount, annualInterestRate, loanTermYears);

        Console.WriteLine($"Monthly Repayment: {monthlyRepayment:C2}");
        Console.WriteLine($"Total Interest Paid: {totalInterestPaid:C2}");
        Console.WriteLine($"Total Amount Paid: {totalAmountPaid:C2}");
        Console.WriteLine("Amortization Schedule:");
        foreach (var entry in amortizationSchedule)
        {
            Console.WriteLine($"Payment #{entry.PaymentNumber}:" +
                $"Payment Amount: {entry.PaymentAmount:C2}, " +
                $"Interest Paid: {entry.InterestPaid:C2}, " +
                $"Principal Paid: {entry.PrincipalPaid:C2}, " +
                $"Remaining Balance: {entry.RemainingBalance:C2}");
        }
    }
}
