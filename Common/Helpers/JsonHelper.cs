
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema.Generation;

namespace Common.Helpers;

public static class JsonHelper
{
    public static bool Validate<T>(string jsonString)
    {
        JSchemaGenerator generator = new JSchemaGenerator();
        JSchema schema = generator.Generate(typeof(T));

        JArray jarray = JArray.Parse(jsonString);

        return jarray.IsValid(schema);
    }
}
