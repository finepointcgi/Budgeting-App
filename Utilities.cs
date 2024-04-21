using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;

namespace BudgetingApp;

public static class Utilities
{
    public static void SaveBudgetToJson(Budget budget, string name = "save"){
        string json = JsonConvert.SerializeObject(budget);

        File.WriteAllText(ProjectSettings.GlobalizePath("user://" + name + ".json"), json);
    }


    public static Budget LoadBudgetFromJson(string budgetName){
        string json = File.ReadAllText(ProjectSettings.GlobalizePath("user://" + budgetName));
        Budget budget = JsonConvert.DeserializeObject<Budget>(json);

        return budget;
    }

    // home, 100,
    // utils, 500
    public static void SaveCatagoryPlanned(Dictionary<TransactionType, float> data){
        string json = JsonConvert.SerializeObject(data);

        File.WriteAllText(ProjectSettings.GlobalizePath("user://plannedCatagory.json"), json);
    }

    public static Dictionary<TransactionType, float> LoadCatagoryPlanned(string catagoryName){
        string json = File.ReadAllText(ProjectSettings.GlobalizePath("user://" + catagoryName));
        Dictionary<TransactionType, float> budget = JsonConvert.DeserializeObject<Dictionary<TransactionType, float>>(json);

        return budget;
    }
}
