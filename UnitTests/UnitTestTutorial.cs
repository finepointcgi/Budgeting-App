using Godot;
using System;
using GdUnit4;
using System.Collections.Generic;
using BudgetingApp;
using System.Threading.Tasks;

[TestSuite]
public class UnitTestTutorial
{
    [TestCase]
    public void TestSaveBudgetToJson(){
        ///set up 
        Budget budget = new Budget();
        budget.Name = "test";
        budget.Expenses = new List<Transaction>();
        Transaction transaction = new Transaction(){
				Name = "test",
				Date = DateTime.Now,
				Amount = 100,
				Type = BudgetingApp.TransactionType.Home 
			};
            budget.Expenses.Add(transaction);
        // test
        Utilities.SaveBudgetToJson(budget, "test");
        
        Budget loadedBudget = new Budget();
        loadedBudget = Utilities.LoadBudgetFromJson("test.json");
        // verification
        Assertions.AssertString(loadedBudget.Name).Equals(budget.Name);
        Assertions.AssertString(loadedBudget.Expenses[0].id.ToString()).Equals(budget.Expenses[0].id.ToString());
        Assertions.AssertString(loadedBudget.Expenses[0].Name).Equals(budget.Expenses[0].Name);
        Assertions.AssertFloat(loadedBudget.Expenses[0].Amount).Equals(budget.Expenses[0].Amount);
        // clean up

    }

    [TestCase]
    public void TestNegSaveBudgetToJson(){
        Budget budget = null;

        Utilities.SaveBudgetToJson(budget, "neg");
        
        Budget loadedBudget = new Budget();
        loadedBudget = Utilities.LoadBudgetFromJson("neg.json");
        // verification
        Assertions.AssertObject(loadedBudget).IsNull();
       
    }

    [TestCase]
    public async Task TestMouseButtonClick(){
    var runner = ISceneRunner.Load("res://test scene.tscn");

    runner.SetMousePos(new Vector2(316, 52));

    await runner.AwaitIdleFrame();

    runner.SimulateMouseButtonPress(MouseButton.Left);

    await runner.AwaitIdleFrame();

    var output = runner.FindChild("TextEdit", true) as TextEdit;

    Assertions.AssertString(output.Text).IsEqual("choice was not 1 or 2\nJohn\nMadison\nMitch\nVictoria\nAlisha\n");

    output.Text = "";

    }

    [TestCase]
    public async Task TestMouseButtonClick2(){
    var runner = ISceneRunner.Load("res://test scene.tscn");
    
    var lineEdit = runner.FindChild("LineEdit", true) as LineEdit;

    lineEdit.Text = "1";

    runner.SetMousePos(new Vector2(316, 52));

    await runner.AwaitIdleFrame();

    runner.SimulateMouseButtonPress(MouseButton.Left);

    await runner.AwaitIdleFrame();

    var output = runner.FindChild("TextEdit", true) as TextEdit;

    Assertions.AssertString(output.Text).IsEqual("choice was 1\nJohn\nMadison\nMitch\nVictoria\nAlisha\n");

    output.Text = "";

    }

    [TestCase]
    public async Task TestMouseButtonClick3(){
    var runner = ISceneRunner.Load("res://test scene.tscn");

    runner.SetMousePos(new Vector2(316, 52));
    var lineEdit = runner.FindChild("LineEdit", true) as LineEdit;

    lineEdit.Text = "2";

    await runner.AwaitIdleFrame();

    runner.SimulateMouseButtonPress(MouseButton.Left);

    await runner.AwaitIdleFrame();

    var output = runner.FindChild("TextEdit", true) as TextEdit;

    Assertions.AssertString(output.Text).IsEqual("choice was 2\nJohn\nMadison\nMitch\nVictoria\nAlisha\n");

    output.Text = "";

    }

    [TestCase]
    public async Task TestCharacterRightMovement(){
        var runner = ISceneRunner.Load("res://test scene.tscn");
        var character = runner.FindChild("CharacterBody2D", true) as CharacterBody2D;

        runner.SimulateKeyPress(Key.Right);

        await runner.AwaitMillis(1000);

        Assertions.AssertFloat(character.Position.X).IsGreater(0);
    }

    [TestCase]
    public async Task TestCharacterLeftMovement(){
        var runner = ISceneRunner.Load("res://test scene.tscn");
        var character = runner.FindChild("CharacterBody2D", true) as CharacterBody2D;

        runner.SimulateKeyPress(Key.Left);

        await runner.AwaitMillis(1000);

        Assertions.AssertFloat(character.Position.X).IsLess(0);
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(20)]
    [TestCase(12)]
    [TestCase(15)]
    [TestCase(18)]
    public async Task TestUpdateActual(int value){
        var runner = ISceneRunner.Load("res://catagory_list_item.tscn");

        runner.Invoke("UpdateActual", value);

        var actual = runner.FindChild("Actual", true) as Label;
        var difference = runner.FindChild("Difference", true) as Label;

        Assertions.AssertString(actual.Text).IsEqual(value.ToString());
        Assertions.AssertString(difference.Text).IsEqual((-value).ToString());

    }


}
