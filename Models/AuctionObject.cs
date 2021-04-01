public class AuctionObject
{
    public AuctionObject() { }

    public AuctionObject(string title, string link, string address, string auctionNumber)
    {
        Title = title;
        Link = link;
        Address = address;
        AuctionNumber = auctionNumber;
    }
    public string Title
    {
        get;
        set;
    }
    public string Link
    {
        get;
        set;
    }
    public string Address
    {
        get;
        set;
    }
    public string AuctionNumber
    {
        get;
        set;
    }
}