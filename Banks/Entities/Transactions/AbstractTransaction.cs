using System;
using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
#pragma warning disable SA1602
    public enum TransactionStatus
    {
        Ready,
        Canceled,
        Successful,
        Failed,
    }
#pragma warning restore SA1602

    public abstract class AbstractTransaction
    {
        public AbstractTransaction(decimal amount, DateTime time, AbstractAccount from, AbstractAccount to)
        {
            To = to;
            From = from;
            Amount = amount;
            Time = time;
            Id = Guid.NewGuid();
            Status = TransactionStatus.Ready;
        }

        internal AbstractTransaction()
        {
        }

        public AbstractAccount From { get; init; }
        public AbstractAccount To { get; init; }
        public Guid Id { get; internal init; }
        public abstract string ErrorMessage { get; internal init; }
        public decimal Amount { get; internal init; }
        public TransactionStatus Status { get; internal set; }
        public DateTime Time { get; internal init; }

        public abstract void Execute();
        internal abstract void Cancel();
    }
}