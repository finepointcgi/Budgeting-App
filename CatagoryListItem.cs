using BudgetingApp;
using Godot;
using System;
using System.Reflection;

public partial class CatagoryListItem : HBoxContainer
{
	public TransactionType Type;
	public float Actual;
	public float Planned;
	[Signal]
	public delegate void PlannedTextChangedEventHandler(int type, float amount);

	public void SetupCatagory(TransactionType type, float actual, float planned){
		Type = type;
		Actual = actual;
		Planned = planned;
		GetNode<Label>("Type").Text = type.ToString();
		GetNode<Label>("Actual").Text = actual.ToString();
		GetNode<LineEdit>("Planned").Text = planned.ToString();
		GetNode<Label>("Difference").Text = (planned - actual).ToString();
	}

	public void _on_planned_text_changed(string text){
		float oldPlanned = Planned;
		
		try{
			Planned = float.Parse(text);
		}
		catch{
			Planned = oldPlanned;
		}

		GetNode<Label>("Difference").Text = (Planned - Actual).ToString();
		float result;
		if(float.TryParse(text, out result))
			EmitSignal(SignalName.PlannedTextChanged, (int)Type, result);
		
		
	}

	public void UpdateActual(float actual){
		Actual += actual;
		GetNode<Label>("Actual").Text = (Actual).ToString();
		GetNode<Label>("Difference").Text = (Planned - Actual).ToString();
	}
}
