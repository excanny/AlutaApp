using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
public class NotificationHub: Hub{
    private IConnectionManager _connection;

    public NotificationHub(IConnectionManager connection){
        _connection = connection;
    }

    public override Task OnConnectedAsync(){
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception ex){
        return base.OnDisconnectedAsync(ex);
    }


}

public interface IConnectionManager{
    void AddConnection(string username,string connectionId);
    void RemoveConnection(string connectionId);
    HashSet<string> GetConnections(string username);
    IEnumerable<string> OnlineUsers {get;}
}