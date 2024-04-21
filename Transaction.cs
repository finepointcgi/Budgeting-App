using System;
using System.Reflection.Metadata.Ecma335;
using BudgetingApp;

public class Transaction{
    public Guid id = Guid.NewGuid();
    public string Name;
    public DateTime Date;
    public float Amount;
    public TransactionType Type;
}