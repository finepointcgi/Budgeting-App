using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GdUnit4;
public partial class AppManagertwo : Control
{
	person p = new person();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		person returnstring = printHelloWorld(delta);
		//GD.Print(returnstring.name);
	}


	private person printHelloWorld(double delta)
	{
		string s = "this is a word";
		int number = 50;
		float numberwithdecimal = 5.234f;
		p.name = s;
		//GD.Print("Hello World" + delta.ToString());
		return p;
	}



	private void _on_button_button_down(){
		LineEdit lineEdit = GetNode<LineEdit>("LineEdit");
		string choice = lineEdit.Text;
		if(choice == "1"){
			printToTerminal("choice was 1");
		}else if(choice == "2"){
			printToTerminal("choice was 2");
		}
		else{
			printToTerminal("choice was not 1 or 2");
		}
		//                          0 1  2 3  4
		int[] intArray = new int[5]{1,10,3,30,5};
		
		intArray[2] = 18;

		List<person> intList = new List<person>();

		string[] stringArray = new string[5]{"John", "Madison", "Mitch", "Victoria", "Alisha"};

		foreach (var name in stringArray)
		{
			person p = new person();
			p.name = name;

			intList.Add(p);
		}

		foreach(person per in intList){
			printToTerminal(per.name);
		}

	}

	private void printToTerminal(string message){
		GetNode<TextEdit>("TextEdit").Text += message + "\n";
		GD.Print(message);
	}
}

class person
{
	public string name = "";
}