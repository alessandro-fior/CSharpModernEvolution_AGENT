using CSharpModernEvolution.Models;

namespace CSharpModernEvolution.Tests;

/// <summary>Fabbrica di oggetti di test: rende leggibili le asserzioni.</summary>
internal static class TestData
{
    public static Employee Employee(
        int id = 1,
        string firstName = "Marco",
        string lastName = "Rossi",
        string department = "IT",
        int age = 34,
        decimal salary = 40_000m,
        int yearsOfExperience = 5,
        bool isActive = true) =>
        new(id, firstName, lastName, department, age, salary, yearsOfExperience, isActive);
}
