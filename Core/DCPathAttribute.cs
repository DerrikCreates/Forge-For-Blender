namespace ForgeTools.Core;

public class DCPathAttribute : DCBaseAttribute
{
    public string XPath { get; }

    public DCPathAttribute(string xPathString)
    {
        XPath = xPathString;
    }
}

public class DCParseAttribute : DCBaseAttribute
{
    public DCParseAttribute()
    {
    }
}