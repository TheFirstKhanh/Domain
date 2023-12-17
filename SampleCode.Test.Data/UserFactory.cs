using Domain;

namespace SampleCode.Test.Data;
public static class UserFactory
{
    public static IEnumerable<User> CreateListUsers =>
    
         new List<User>()
        {
            new User() { Id = 53, First = "Bill", Last = "Bryson", Age = 23, Gender = "M" },
            new User() { Id = 62, First = "John", Last = "Travolta", Age = 54, Gender = "M" },
            new User() { Id = 41, First = "Frank", Last = "Zappa", Age = 23, Gender = "T" },
            new User() { Id = 31, First = "Jill", Last = "Scott", Age = 66, Gender = "Y" },
            new User() { Id = 31, First = "Anna", Last = "Meredith", Age = 66, Gender = "Y" },
            new User() { Id = 31, First = "Janet", Last = "Jackson", Age = 66, Gender = "F" }
        };
    

    public static string ListUserJsonData => @"[{ ""id"": 53, ""first"": ""Bill"", ""last"": ""Bryson"", ""age"":23, ""gender"":""M"" },
{ ""id"": 62, ""first"": ""John"", ""last"": ""Travolta"", ""age"":54, ""gender"":""M"" },
{ ""id"": 41, ""first"": ""Frank"", ""last"": ""Zappa"", ""age"":23, gender:""T"" },
{ ""id"": 31, ""first"": ""Jill"", ""last"": ""Scott"", ""age"":66, gender:""Y"" },
{ ""id"": 31, ""first"": ""Anna"", ""last"": ""Meredith"", ""age"":66, ""gender"":""Y"" },
{ ""id"": 31, ""first"": ""Janet"", ""last"": ""Jackson"", ""age"":66, ""gender"":""F"" }]";

    public static string ListUserBrokenJsonData => @"[ ""id"": 53, ""first"": ""Bill"", ""last"": ""Bryson"", ""age"":23, ""gender"":""M"" },
{ ""id"": 62, ""first"": ""John"", ""last"": ""Travolta"", ""age"":54, ""gender"":""M"" },
{ ""id"": 41, ""first"": ""Frank"", ""last"": ""Zappa"", ""age"":23, gender:""T"" },
{ ""id"": 31, ""first"": ""Jill"", ""last"": ""Scott"", ""age"":66, gender:""Y"" },
{ ""id"": 31, ""first"": ""Anna"", ""last"": ""Meredith"", ""age"":66, ""gender"":""Y"" },
{ ""id"": 31, ""first"": ""Janet"", ""last"": ""Jackson"", ""age"":66, ""gender"":""F"" }]";

    public static string ListUserInvalidSchemaJsonData => @"[{ ""test"": ""53"", ""first"": ""Bill"", ""last"": ""Bryson"", ""age"":23, ""gender"":""M"" },
{ ""id"": 62, ""first"": ""John"", ""last"": ""Travolta"", ""age"":54, ""gender"":""M"" },
{ ""id"": 41, ""first"": ""Frank"", ""last"": ""Zappa"", ""age"":23, gender:""T"" },
{ ""id"": 31, ""first"": ""Jill"", ""last"": ""Scott"", ""age"":66, gender:""Y"" },
{ ""id"": 31, ""first"": ""Anna"", ""last"": ""Meredith"", ""age"":66, ""gender"":""Y"" },
{ ""id"": 31, ""first"": ""Janet"", ""last"": ""Jackson"", ""age"":66, ""gender"":""F"" }]";
}
