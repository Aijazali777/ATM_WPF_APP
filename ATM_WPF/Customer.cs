using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_WPF
{
    public class Customer
    {
        public string accountNumber;
        public string password;
        public string name;
        public double balance;

        public Customer()
        {

        }

        public Customer(string acctNum, string pass, string custName, double blnce)
        {
            this.accountNumber = acctNum;
            this.password = pass;
            this.name = custName;
            this.balance = blnce;
        }
    }
}
