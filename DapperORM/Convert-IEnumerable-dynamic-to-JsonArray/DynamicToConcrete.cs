public static object ToConcrete<T>(System.Dynamic.ExpandoObject dynObject)
    {
        object instance = Activator.CreateInstance<T>();
        var dict = dynObject as IDictionary<string, object>;
        PropertyInfo[] targetProperties = instance.GetType().GetProperties();

        foreach (PropertyInfo property in targetProperties)
        {
            object propVal;
            if (dict.TryGetValue(property.Name, out propVal))
            {
                property.SetValue(instance, propVal, null);
            }
        }

        return instance;
    }

    public static System.Dynamic.ExpandoObject ToExpando(object staticObject)
    {
        System.Dynamic.ExpandoObject expando = new ExpandoObject();
        var dict = expando as IDictionary<string, object>;
        PropertyInfo[] properties = staticObject.GetType().GetProperties();

        foreach (PropertyInfo property in properties)
        {
            dict[property.Name] = property.GetValue(staticObject, null);
        }

        return expando;
    }
