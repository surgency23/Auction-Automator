public class Listing
{
    public Listing() { }

    public Listing(string OriginatingAuction, string Title, string Link, string Description)
    {
        originatingAuction = OriginatingAuction
        title = Title;
        link = Link;
        descrption = Description
    }
    public string originatingAuction{
        get;
        set;
    }
    public string title
    {
        get;
        set;
    }
    public string link
    {
        get;
        set;
    }
    public string descrption
    {
        get;
        set;
    }
}