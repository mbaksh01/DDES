namespace DDES.Common.Enums;

public enum MessageType
{
    Authenticate,
    Unknown,
    ClientConnected,
    GetThreads,
    SendThreadMessage,
    GetProducts,
    AddProduct,
    UpdateProduct,
    DeleteProduct,
    BroadcastMessage,
    UpdateSubscription
}