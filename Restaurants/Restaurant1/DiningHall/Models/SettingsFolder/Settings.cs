﻿namespace DiningHall.Models.SettingsFolder;

public static class Settings
{
    public const int NrOfTables = 10;
    public const int NrOfWaiters = 4;

    public const int FoodListSize = 10;

    // public const string KitchenUrl = "http://host.docker.internal:7284/kitchen";
    public const string KitchenUrl = "https://localhost:7284/kitchen";
    
    public const string FoodOrderingServiceRegisterUrl = "https://localhost:7192/register";
    
    
    public const string Menu = "U:\\ThirdYear\\PR\\DiningHall\\JSON\\Menu.json";
    public const string RestaurantData = "U:\\ThirdYear\\PR\\DiningHall\\JSON\\Restaurant.json";
}