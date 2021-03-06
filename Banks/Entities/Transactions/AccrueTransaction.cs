using System;
using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
    public class AccrueTransaction : AbstractTransaction
    {
        private string _errorMessage;

        public AccrueTransaction(decimal amount, AbstractAccount to, DateTime time)
            : base(amount, time, null, to)
        {
            _errorMessage = string.Empty;
        }

        internal AccrueTransaction()
        {
        }

        public override string ErrorMessage { get => _errorMessage; internal init => _errorMessage = value; }

        public override void Execute()
        {
            if (Status is not TransactionStatus.Ready)
                throw new InvalidOperationException("Transaction is already done");
            To.IncreaseBalance(Amount);
            Status = TransactionStatus.Successful;
        }

        internal override void Cancel()
        {
            if (Status is not TransactionStatus.Successful)
                throw new InvalidOperationException("Transaction can not be canceled");
            To.DecreaseBalanceWithoutLimit(Amount);
            Status = TransactionStatus.Canceled;
        }
    }
}