using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace ATM_WPF
{
    public partial class MainWindow : Window
    {
        string acctNum = "12345";
        string pass = "";
        double amount = 0;
        string name;
        string currentUserName = "";

        bool isBtnDepositClicked = false;
        bool isBtnWithDrawClicked = false;
        bool isBtnShowBalanceClicked = false;
        bool isBtnExitClicked = false;

        public MainWindow()
        {
            InitializeComponent();
            List<Customer> customer = deserilization();

            Welcome();
        }

        private void Welcome()
        {
            TxtWelcome.Text = "Welcome to Alfa ATM";
            TxtMainOption.Text = "Please Select any option below to continue";
            BtnExistingUser.Content = "Existing User";
            BtnNewUser.Content = "New User";

            TxtWelcome.Visibility = Visibility.Visible;
            TxtMainOption.Visibility = Visibility.Visible;
            SPMainChoice.Visibility = Visibility.Visible;
            SPLogin.Visibility = Visibility.Collapsed;
            SPSignUp.Visibility = Visibility.Collapsed;
            SPMenu.Visibility = Visibility.Collapsed;
            SPDepositAndWithdraw.Visibility = Visibility.Collapsed;
            TxtBalance.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Collapsed;
            TxtSignupResponse.Visibility = Visibility.Collapsed;
        }

        private void ExistingUser(object sender, RoutedEventArgs e)
        {
            SPMainChoice.Visibility = Visibility.Collapsed;
            SPSignUp.Visibility = Visibility.Collapsed;
            SPLogin.Visibility = Visibility.Visible;

            TxtMainOption.Text = "Login";
            TxtAccountNum.Text = "Enter account number";
            TxtPassword.Text = "Enter password";
            BtnLogin.Content = "Login";
            TxtResponse.Text = "";


            Binding b1 = new Binding("Text");
            b1.Source = FldAccountNum;
            FldAccountNum.SetBinding(TextBox.TextProperty, b1);


            Binding b2 = new Binding("Text");
            b2.Source = FldPassword;
            FldPassword.SetBinding(TextBox.TextProperty, b2);

        }

        private void NewUser(object sender, RoutedEventArgs e)
        {
            SPMainChoice.Visibility = Visibility.Collapsed;
            SPSignUp.Visibility = Visibility.Visible;

            TxtMainOption.Text = "SignUp";
            TxtNewAccountNum.Text = "Enter account number";
            TxtNewPassword.Text = "Enter password";
            TxtNewName.Text = "Enter name";
            TxtNewBalance.Text = "Enter an amount";
            BtnSignup.Content = "SignUp";
            TxtSignupResponse.Text = "";

            Binding b1 = new Binding("Text");
            b1.Source = FldNewAccountNum;
            FldNewAccountNum.SetBinding(TextBox.TextProperty, b1);


            Binding b2 = new Binding("Text");
            b2.Source = FldNewPassword;
            FldNewPassword.SetBinding(TextBox.TextProperty, b2);

            Binding b3 = new Binding("Text");
            b3.Source = FldNewName;
            FldNewName.SetBinding(TextBox.TextProperty, b3);


            Binding b4 = new Binding("Text");
            b4.Source = FldNewBalance;
            FldNewBalance.SetBinding(TextBox.TextProperty, b4);

        }

        //XML File Path
        string path = @"C:\Users\aaijaz\source\repos\ATM_WPF\account_deatils.xml";

        // SERILIZATION
        void serilization(List<Customer> customers)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, customers);

            }
        }

        // DESERILIZATION
        List<Customer> deserilization()
        {
            List<Customer> desCust = new List<Customer>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));
            using (StreamReader reader = new StreamReader(path))
            {
                desCust = (List<Customer>)serializer.Deserialize(reader);
            }
            return desCust;
        }

        private void signup(object sender, RoutedEventArgs e)
        {
            TxtSignupResponse.Visibility = Visibility.Visible;
            TxtMainOption.Visibility = Visibility.Visible;
            TxtMainOption.Text = "SignUp";
            acctNum = FldNewAccountNum.Text;
            pass = FldNewPassword.Text;
            name = FldNewName.Text;
            if (double.TryParse(FldNewBalance.Text, out double amount_d))
                amount = amount_d;
            bool isInvalid = false;

            if(acctNum == "" || acctNum == null)
            {
                TxtSignupResponse.Text = "Please enter an account number to register.";
                isInvalid = true;
            }
            else if (pass == "" || pass == null)
            {
                TxtSignupResponse.Text = "Please enter a password to register.";
                isInvalid = true;
            }
            else if (name == "" || name == null)
            {
                TxtSignupResponse.Text = "Please enter your name to register.";
                isInvalid = true;
            }
            else if (amount <= 500)
            {
                TxtSignupResponse.Text = "Please enter an amount of 500 or above to register.";
                isInvalid = true;
            }

            List<Customer> customer = deserilization();
            foreach (Customer cust in customer)
            {
                if (acctNum == cust.accountNumber)
                {
                    TxtSignupResponse.Text = "This account number already exists. Try another one.";
                    isInvalid = true;
                }
            }

            if(!isInvalid)
            {
                Customer newCustomer = new Customer();
                newCustomer.accountNumber = acctNum;
                newCustomer.password = pass;
                newCustomer.name = name;
                newCustomer.balance = amount;

                customer.Add(newCustomer);
                serilization(customer);

                TxtSignupResponse.Text = "Account created successfully!";
                ExistingUser(sender, e);
            }
            
        }

        private void login(object sender, RoutedEventArgs e)
        {
            TxtSignupResponse.Visibility = Visibility.Collapsed;
            acctNum = FldAccountNum.Text;
            pass = FldPassword.Text;

            bool isAccount = false;
            bool isPassword = false;
            List<Customer> desCustomer = deserilization();

            foreach (Customer arg in desCustomer)
            {
                if (acctNum == arg.accountNumber)
                {
                    isAccount = true;
                    if (pass == arg.password)
                    {
                        currentUserName = arg.name;
                        menu();
                    }
                }
            }

            if (!isAccount)
            {
                TxtResponse.Text = "Account Not Found";
            }
            else if(!isPassword)
            {
                TxtResponse.Text = "Incorrect Password";
            }
        }

        private void deposit(object sender, RoutedEventArgs e)
        {
            SPMenu.Visibility = Visibility.Collapsed;
            SPDepositAndWithdraw.Visibility = Visibility.Visible;
            TxtBalance.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Visible;
            TxtBalance.Text = "";
            BtnDone.Content = "Deposit";
            TxtMainOption.Text = "Please enter an amount to deposit!";
            Binding b1 = new Binding("Text");
            b1.Source = FldAmount;
            FldAmount.SetBinding(TextBox.TextProperty, b1);
            isBtnDepositClicked = true;
        }

        private void withdraw(object sender, RoutedEventArgs e)
        {
            SPMenu.Visibility = Visibility.Collapsed;
            SPDepositAndWithdraw.Visibility = Visibility.Visible;
            TxtBalance.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Visible;
            TxtBalance.Text = "";
            BtnDone.Content = "Withdraw";
            TxtMainOption.Text = "Please enter an amount to Withdraw!";
            Binding b1 = new Binding("Text");
            b1.Source = FldAmount;
            FldAmount.SetBinding(TextBox.TextProperty, b1);
            isBtnWithDrawClicked = true;
        }

        private void showBalance(object sender, RoutedEventArgs e)
        {
            SPMenu.Visibility = Visibility.Collapsed;
            SPDepositAndWithdraw.Visibility = Visibility.Collapsed;
            TxtBalance.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
            isBtnShowBalanceClicked = true;
            isBtnDepositClicked = false;
            isBtnWithDrawClicked = false;
            done(sender,e);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            SPMenu.Visibility = Visibility.Collapsed;
            isBtnExitClicked = true;
            done(sender, e);
        }

        private void done(object sender, RoutedEventArgs e)
        {
            SPDepositAndWithdraw.Visibility = Visibility.Collapsed;
            TxtBalance.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;

            if (double.TryParse(FldAmount.Text, out double amount_d))
                amount = amount_d;

            List<Customer> desCustomer = deserilization();

            foreach (Customer arg in desCustomer)
            {
                if (acctNum == arg.accountNumber)
                {
                    if (pass == arg.password)
                    {
                        if(isBtnDepositClicked)
                        {
                            if(amount >= 500)
                            {
                                arg.balance = arg.balance + amount;
                                TxtMainOption.Text = "Amount Added Successfully!";
                                TxtBalance.Text = "Your new Balance is " + arg.balance;
                                isBtnDepositClicked = false;
                            }
                            else
                            {
                                TxtMainOption.Text = "Please enter an amount of 500 or above to deposit";
                                TxtBalance.Visibility = Visibility.Collapsed;
                                SPDepositAndWithdraw.Visibility = Visibility.Visible;
                            }
                           
                            
                        }
                        else if(isBtnWithDrawClicked)
                        {
                            if(amount < arg.balance)
                            {
                                if (amount >= 500)
                                {
                                    arg.balance = arg.balance - amount;
                                    TxtMainOption.Text = "Amount withdrawn successfully!";
                                    TxtBalance.Text = "Your new balance is " + arg.balance;
                                    isBtnWithDrawClicked = false;
                                }
                                else
                                {
                                    TxtMainOption.Text = "Please enter an amount of 500 or above to Withdraw";
                                    TxtBalance.Visibility = Visibility.Collapsed;
                                    SPDepositAndWithdraw.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                TxtMainOption.Text = "You have insufficient balance. Please try another amount";
                                TxtBalance.Visibility = Visibility.Collapsed;
                                SPDepositAndWithdraw.Visibility = Visibility.Visible;
                            }
                            
                        }
                        else if (isBtnShowBalanceClicked)
                        {
                            TxtMainOption.Text = "";
                            TxtBalance.Text = "Your available Balance is " + arg.balance;
                            isBtnShowBalanceClicked = false;
                        }
                        else if (isBtnExitClicked)
                        {
                            SPDepositAndWithdraw.Visibility = Visibility.Collapsed;
                            TxtBalance.Visibility = Visibility.Collapsed;
                            BtnBack.Visibility = Visibility.Collapsed;
                            
                            isBtnExitClicked = false;
                            Welcome();
                        }
                        serilization(desCustomer);
                    }
                }
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            SPSignUp.Visibility = Visibility.Collapsed;
            TxtMainOption.Text = "";
            SPDepositAndWithdraw.Visibility = Visibility.Collapsed;
            TxtBalance.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Collapsed;
            menu();
        }

        void menu()
        {
            SPLogin.Visibility = Visibility.Collapsed;
            SPMenu.Visibility = Visibility.Visible;
            TxtMainOption.Text = "Welcome "+currentUserName+" :) ";
            TxtOption.Text = "Please select any option to process...";
        }
    }
}
