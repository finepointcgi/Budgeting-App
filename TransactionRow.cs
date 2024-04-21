using BudgetingApp;
using Godot;
using System;

public partial class TransactionRow : Panel
{

	public Transaction CurrentTransaction;
	public int currentIndex;
	public int previousIndex;

	[Signal]
	public delegate void EditTransactionEventHandler(int currentIndex, int previousIndex,
	string guid);
	public void _on_type_item_selected(int index){
		previousIndex = (int)CurrentTransaction.Type;
		CurrentTransaction.Type = (TransactionType)index;
		currentIndex = index;
		EmitSignal(SignalName.EditTransaction,currentIndex, previousIndex, CurrentTransaction.id.ToString());
	}	
}
