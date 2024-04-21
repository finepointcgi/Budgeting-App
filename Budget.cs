using System.Collections.Generic;
using Microsoft.VisualBasic;

public class Budget{
    public string Name;
    public DateAndTime DateStart;
    public DateAndTime DateEnd;
    public List<Transaction> Income;
    public List<Transaction> Expenses;
}