using BudgetingApp;
using Godot;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
public partial class AppManager : Control
{
	public static Budget currentBudget;

	[Export]
	public PackedScene TransactionList;
	[Export]
	public PackedScene AddTransactionMenu;
	[Export]
	public PackedScene TransactionCatagoryLineItem;

	float totalIncome = 0;
	float totalExpense = 0;
	VBoxContainer container;
	bool lastTransactionStyleBoxLight;
	Dictionary<TransactionType, float> catagoryExpenses = new Dictionary<TransactionType, float>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		catagoryExpenses = Utilities.LoadCatagoryPlanned("plannedCatagory.json");
		setupCatagories();
		currentBudget = new Budget(){
			Name = "current Budget",
			Expenses = new List<Transaction>(),
			Income = new List<Transaction>(),
		};
		container = GetNode<VBoxContainer>("TransactionList/ScrollContainer/TransactionList");
		currentBudget = Utilities.LoadBudgetFromJson("save.json");
		//loadCSV("C:/Users/FinePointCGI/Downloads/AccountHistory.csv");
		Utilities.SaveBudgetToJson(currentBudget);
		AddTransactionsToTable();
		
	}

	private void AddTransactionsToTable(){
		foreach (var item in currentBudget.Expenses)
		{
			AddTransactionToTransactions(item, false);
		}

		foreach (var item in currentBudget.Income)
		{
			AddTransactionToTransactions(item, true);
		}		
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_add_transaction_button_down(){
		AddTransactionWindow transactionMenu = AddTransactionMenu.Instantiate<AddTransactionWindow>();
		AddChild(transactionMenu);
		transactionMenu.AddTransaction += AddTransactionToTransactions;
	}

	private Transaction AddTransactionToTransactions(string name, DateTime date, float amount, TransactionType type, bool income){
		name = name.Replace("POS Withdrawal - ", "");
		Transaction transaction = new Transaction(){
				Name = name,
				Date = date.Date,
				Amount = amount,
				Type = type 
			};

		return AddTransactionToTransactions(transaction, income);

	}
    private void AddTransactionToTransactions(string name, string date, float amount, int type, bool income)
    {
		Transaction t = AddTransactionToTransactions(name, DateTime.Parse(date), amount, (TransactionType)type, income);
		
		if(income){
			currentBudget.Income.Add(t);
		}else{
 			currentBudget.Expenses.Add(t);
		}
    }
	
	private Transaction AddTransactionToTransactions(Transaction transaction, bool income){
		
		if(income){
			totalIncome += transaction.Amount;
			GetNode<RichTextLabel>("TotalInE/Income/IncomeAmount").Text = "[center][b]" + totalIncome.ToString();
		}else{
			updateCatagoriesValues(transaction);
			totalExpense += transaction.Amount;
			GetNode<RichTextLabel>("TotalInE/Expense/ExpenseAmount").Text = "[center][b]" + totalExpense.ToString();
		}

		StyleBox box;

		if(lastTransactionStyleBoxLight){
			box = ResourceLoader.Load<StyleBox>("res://transaction_row_dark.tres");
			
		}else{
			box = ResourceLoader.Load<StyleBox>("res://transaction_row_light.tres");
		}
		lastTransactionStyleBoxLight = !lastTransactionStyleBoxLight;
       	AddTransactionToList(transaction,box);

		GetNode<TextureProgressBar>("TotalInE/TextureProgressBar").MaxValue = totalIncome;
		GetNode<TextureProgressBar>("TotalInE/TextureProgressBar").Value = totalExpense;

		return transaction;
	}

	private void AddTransactionToList(Transaction transaction, StyleBox box){
		TransactionRow tableRow = TransactionList.Instantiate<TransactionRow>();


		tableRow.AddThemeStyleboxOverride("panel", box);
		tableRow.GetNode<RichTextLabel>("TransactionRow/Date").Text = transaction.Date.ToString("d");
		tableRow.GetNode<RichTextLabel>("TransactionRow/Name").Text = transaction.Name;
		tableRow.GetNode<RichTextLabel>("TransactionRow/Amount").Text = transaction.Amount.ToString();
		tableRow.GetNode<OptionButton>("TransactionRow/Type").Selected = (int)transaction.Type;
		tableRow.CurrentTransaction = transaction;
		container.AddChild(tableRow);
		tableRow.EditTransaction += onEditTransaction;
	}

    private void onEditTransaction(int currentIndex, int previousIndex, string guid)
    {
        foreach (var item in currentBudget.Expenses)
		{
			if(item.id.ToString() == guid){
				item.Type = (TransactionType)currentIndex;
				updateCatagoriesValues(item, previousIndex);
				Utilities.SaveBudgetToJson(currentBudget);
			}
		}
    }

    private void loadCSV(string path){
		List<string[]> parsedData = new List<string[]>();

		using(StreamReader reader = new StreamReader(path)){
			while(!reader.EndOfStream){ // < while reader is still reading the file do V
				string line = reader.ReadLine(); // reading the files line 
				string[] fields = line.Split(','); // spliting the data by ,
				parsedData.Add(fields); // adding data to parsed data
			}
		}

		foreach (var tableRow in parsedData)
		{
			if(tableRow[0] == "Account Number"){
				continue;
			}

			AddTransactionToTransactions(tableRow[3], tableRow[1], 
					tableRow[4] != "" ? float.Parse(tableRow[4]) : float.Parse(tableRow[5]), 
					5, tableRow[4] == "");
		}
	}

	private void setupCatagories(){
		foreach (var item in Enum.GetValues(typeof(TransactionType)))
		{
			CatagoryListItem transactionLineItem = TransactionCatagoryLineItem.Instantiate<CatagoryListItem>();
			
			GetNode("TransactionCatagoryItems/Body").AddChild(transactionLineItem);
			
			if(!catagoryExpenses.ContainsKey((TransactionType)item))
				catagoryExpenses.Add((TransactionType)item, 0);

			transactionLineItem.SetupCatagory((TransactionType)item, 0, catagoryExpenses[(TransactionType)item]);
			transactionLineItem.PlannedTextChanged += onPlanTextChanged;
		}
	}

    private void onPlanTextChanged(int type, float amount)
    {
        catagoryExpenses[(TransactionType)type] = amount;
		Utilities.SaveCatagoryPlanned(catagoryExpenses);
    }

    private void updateCatagoriesValues(Transaction transaction, int previousIndex = -1){
		foreach(var item in GetNode("TransactionCatagoryItems/Body").GetChildren()){
			if(item is CatagoryListItem catagoryListItem){
				if(catagoryListItem.Type == transaction.Type){
					catagoryListItem.UpdateActual(transaction.Amount); // 100
				}
				if(((int)catagoryListItem.Type) == previousIndex){
					catagoryListItem.UpdateActual(-transaction.Amount); // -100
				}
				
			}
		}
	}

	private void _on_file_dialog_file_selected(string path){
		loadCSV(path);
		GetNode<FileDialog>("FileDialog").Visible = false;
	}

	private void _on_button_button_down(){
		GetNode<FileDialog>("FileDialog").Visible = true;
	}
}