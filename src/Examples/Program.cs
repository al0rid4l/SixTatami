using Examples;

Console.WriteLine("Book0");
Console.WriteLine(Book0.CSAPP.Data());
Console.WriteLine(Book0.APUE.Data());
Console.WriteLine(Book0.SICP.Data());
Console.WriteLine(Book0.CSAPP.ToStringFast());
Console.WriteLine(Book0.APUE.ToStringFast());
Console.WriteLine(Book0.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book1");
Console.WriteLine(Book1.CSAPP.Data());
Console.WriteLine(Book1.APUE.Data());
Console.WriteLine(Book1.SICP.Data());
Console.WriteLine(Book1.CSAPP.ToStringFast());
Console.WriteLine(Book1.APUE.ToStringFast());
Console.WriteLine(Book1.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book2");
Console.WriteLine(Book2.CSAPP.Data());
Console.WriteLine(Book2.APUE.Data());
Console.WriteLine(Book2.SICP.Data());
Console.WriteLine(Book2.CSAPP.ToStringFast());
Console.WriteLine(Book2.APUE.ToStringFast());
Console.WriteLine(Book2.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book3");
Console.WriteLine(Book3.CSAPP.Data());
Console.WriteLine(Book3.APUE.Data());
Console.WriteLine(Book3.SICP.Data());
Console.WriteLine(Book3.CSAPP.ToStringFast());
Console.WriteLine(Book3.APUE.ToStringFast());
Console.WriteLine(Book3.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book0Extension");
Console.WriteLine(ExamplesBook0Extension.GetDataByName("CSAPP"));
Console.WriteLine(ExamplesBook0Extension.GetValueByName("CSAPP"));