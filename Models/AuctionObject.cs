public class AuctionObject {
    public AuctionObject() {}

    public AuctionObject(string title, string link, string address) {
        Title = title;
        Link = link;
        Address = address;
    }
    public string Title {
        get;
        set;
    }
    public string Link {
        get;
        set;
    }
    public string Address {
        get;
        set;
    }
}