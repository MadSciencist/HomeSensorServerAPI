namespace NSModelInterfaces
{
    public interface INode
    {
        string Description { get; set; }
        string GatewayAddress { get; set; }
        string Identifier { get; set; }
        string IpAddress { get; set; }
        bool IsOn { get; set; }
        string LoginName { get; set; }
        string LoginPassword { get; set; }
        string Name { get; set; }
        //ENodeType? NodeType { get; set; }
        IUser Owner { get; set; }
        string RegistredProperties { get; set; }
    }
}