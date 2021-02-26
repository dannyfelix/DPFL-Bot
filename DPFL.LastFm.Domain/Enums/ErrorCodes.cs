namespace DPFL.LastFm.Domain.Enums
{
    public enum ErrorCodes
    {
        BadService = 2,
        
        BadMethod = 3,
        
        AuthFailed = 4,
        
        BadFormat = 5,
        
        BadParams = 6,
        
        BadResource = 7,
        
        OperationFailed = 8,
        
        BadSessionKey = 9,
        
        BadApiKey = 10,
        
        ServiceOffline = 11,
        
        SubOnly = 12,
        
        BadMethodSignature = 13,
        
        UnauthorizedToken = 14,
        
        ItemUnavailable = 15,
        
        TempUnavailable = 16,
        
        LoginRequired = 17,
        
        TrialExpired = 18,
        
        NotEnoughContent = 20,
        
        NotEnoughMembers = 21,
        
        NotEnoughFans = 22,
        
        NotEnoughNeighbours = 23,
        
        NoPeakRadio = 24,
        
        RadioNotFound = 25,
        
        ApiKeySuspended = 26,
        
        Deprecated = 27,
        
        RateLimited = 29
    }
}