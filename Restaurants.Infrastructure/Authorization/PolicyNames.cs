namespace Restaurants.Infrastructure.Authorization;

public static class PolicyNames
{
    public const string HasNationality = "HasNationality";
    public const string AtLeast20 =  "AtLeast20";
    public const string Has2Restaurants = "Has2Restaurants";
}

public static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string BirthDate = "BirthDate";
}