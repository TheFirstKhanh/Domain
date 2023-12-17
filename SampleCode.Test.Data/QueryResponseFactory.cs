using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Users.Queries;

namespace SampleCode.Test.Data;

public static class QueryResponseFactory
{
    public static GetUserSummaryQueryResponse CreateGetUserSummaryQueryResponse()
    {
        return new GetUserSummaryQueryResponse { 
            FirstName = "Test FirstName",
            UserFullName = CreateListUserFullName,
            GenderPerAges = CreateListGenderPerAge
        };
    }

    public static List<string> CreateListUserFullName => new List<string>() { "Bill", "Bryson", "John", "Frank", "Frank", "Anna" };


    public static List<GenderPerAge> CreateListGenderPerAge => new List<GenderPerAge>() {

           new GenderPerAge()
                {
                     Age = 23, Female = 0, Male = 1,

                } ,
        new GenderPerAge()
                {
                     Age = 54, Female = 0, Male = 1,
                      
                } ,
        new GenderPerAge {
         Age = 66, Female = 2, Male = 0,
        }
    };

    public static string UserJsonData => @"{ ""id"": 53, ""first"": ""Bill"", ""last"": ""Bryson"", ""age"":23, ""gender"":""M"" }";

 
}
