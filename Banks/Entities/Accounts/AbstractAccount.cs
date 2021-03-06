using System;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Database;
using Banks.Entities.Transactions;
using Banks.Providers;

namespace Banks.Entities.Accounts
{
    public abstract class AbstractAccount
    {
        private decimal _balance;

        protected AbstractAccount(BanksContext context)
        {
            DateTimeProvider = context.DateTimeProvider;
        }

        protected AbstractAccount(Client client, UnverifiedLimitProvider unverifiedLimit, IDateTimeProvider dateTimeProvider)
        {
            Client = client;
            _balance = 0;
            UnverifiedLimit = unverifiedLimit;
            DateTimeProvider = dateTimeProvider;
        }

        public Guid Id { get; internal init; }
        public UnverifiedLimitProvider UnverifiedLimit { get; internal set; }
        [NotMapped]
        public IDateTimeProvider DateTimeProvider { get; internal set; }
        public decimal Balance { get => _balance; internal init => _balance = value; }
        public Client Client { get; internal init; }

        public virtual AbstractTransaction CreateAccrueTransaction(decimal amount)
        {
            var transaction = new AccrueTransaction(amount, this, DateTimeProvider.Now);
            return transaction;
        }

        public virtual AbstractTransaction CreateWithdrawTransaction(decimal amount)
        {
            var transaction = new WithdrawTransaction(amount, this, DateTimeProvider.Now);
            return transaction;
        }

        public virtual AbstractTransaction CreateTransferTransaction(decimal amount, AbstractAccount account)
        {
            var transaction = new TransferTransaction(amount, this, account, DateTimeProvider.Now);
            return transaction;
        }

        internal void DecreaseBalanceWithoutLimit(decimal amount)
            => _balance -= amount;
        internal void IncreaseBalance(decimal amount)
            => _balance += amount;

        internal abstract AbstractTransaction CreateServiceTransaction();

        internal virtual void DecreaseBalance(decimal amount)
        {
            if (!Client.Verified && amount > UnverifiedLimit.UnverifiedLimit)
                throw new InvalidOperationException("UnverifiedClient can not withdraw of transfer more than a limit");
            DecreaseBalanceWithoutLimit(amount);
        }
    }
}