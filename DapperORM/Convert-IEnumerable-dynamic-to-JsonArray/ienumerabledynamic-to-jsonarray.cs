http://stackoverflow.com/questions/10046496/convert-ienumerabledynamic-to-jsonarray

[WebGet(UriTemplate = "/tools/data/get?tool={tool}&filters={filters}")]
    public JsonArray GetData(string tool, string[,] filters)
    {
        IEnumerable<dynamic> list = _repository.All("", "", 0).ToList();


        IEnumerable<JsonObject> jsonList = from item in list
                                select new JsonObject()
                                           {
                                               new KeyValuePair<string, JsonValue>("Id", item.Id),
                                               new KeyValuePair<string, JsonValue>("Name", item.Title),
                                               new KeyValuePair<string, JsonValue>("Data", new JsonObject()
                                                                                               {
                                                                                                    new KeyValuePair<string, JsonValue>("Product", item.Product),
                                                                                                    new KeyValuePair<string, JsonValue>("Suite", item.Suite),
                                                                                                    new KeyValuePair<string, JsonValue>("Package", item.Package),
                                                                                                    new KeyValuePair<string, JsonValue>("Description", item.Description)
                                                                                               })

                                           };

        JsonArray returnValue = new JsonArray(jsonList);

        return returnValue;
    }
    
    
    
*** must implement - Getting dynamic ExpandoObject to serialize to JSON as expected
http://www.patridgedev.com/2011/08/24/getting-dynamic-expandoobject-to-serialize-to-json-as-expected/


***Using C# 4.0 and dynamic to parse JSON
http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2010/08/22/using-c-4.0-and-dynamic-to-parse-json.aspx

