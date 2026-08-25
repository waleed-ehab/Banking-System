namespace BankingSystem.Domain.DataModels
{
    public class AccountDataModel
    {
        public string AccountId { get; set; } = string.Empty;
        public string PinHash { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; 
        public decimal Balance { get; set; } 
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime? DeletedAt { get; set; }
    }
}