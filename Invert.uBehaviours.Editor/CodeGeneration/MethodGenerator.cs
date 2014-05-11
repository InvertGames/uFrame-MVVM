using System;
using System.Text;

public class MethodGenerator
{
    public string Name { get; set; }

    public string Decleration { get; set; }

    public StringBuilder Body
    {
        get;
        set;
    }

    public int TabCount { get; set; }

    public string Tabs
    {
        get
        {
            var str = "";
            for (var i = 0; i < TabCount; i++)
            {
                str += "\t";
            }
            return str;
        }
    }

    public void Enclose(string header, Action<MethodGenerator> body)
    {
        Body.Append(Tabs + header).Append("{").AppendLine();
        TabCount++;
        body(this);
        TabCount--;
        Body.AppendLine();
        Body.Append(Tabs + "}").AppendLine();
    }

    public MethodGenerator Tab()
    {
        Body.Append(Tabs);
        return this;
    }

    public MethodGenerator A(string val)
    {
        Body.Append(val);
        return this;
    }

    public MethodGenerator AF(string format, params object[] args)
    {
        Body.AppendFormat(Tabs + format, args);
        return this;
    }

    public MethodGenerator AL(string value = null)
    {
        Body.AppendLine(Tabs + value);
        return this;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(Tabs).AppendFormat(Decleration, Name).Append("{").AppendLine();

        sb.Append(Body.ToString()).AppendLine();

        sb.Append(Tabs).Append("}").AppendLine();
        return sb.ToString();
    }
}