namespace AppMGL.DAL.UDT
{
    public enum EnumResult
    {
        Success = 1001,

        Failed = 2001,
        AnotherUserModifiedRecord = 2002,
        ValidationFailed = 2003,
        InvalidGrant = 2004,
        UnauthorizedAccess = 2005,
        InvalidAction = 2006,
        UserDeactivated = 2007
    };
}
