**here.All() return ienumerable<dynamic>

List<MyClass> vats = here.All()
                         .Select(item => new MyClass(item.Name, item.Value))
                         .ToList();
                         

List<MyClass> vats = here.All()
                         .Select(item => new MyClass {
                                     Name = item.Name,
                                     Value = item.Value,
                                 })
                         .ToList();                         
