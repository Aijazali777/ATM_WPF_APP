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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string acctNum = "12345";
        string pass = "";
        double amount = 0;

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
            TxtAccountNum.Visibility = Visibility.Collapsed;
            FldAccountNum.Visibility = Visibility.Collapsed;
            TxtPassword.Visibility = Visibility.Collapsed;
            FldPassword.Visibility = Visibility.Collapsed;
            BtnLogin.Visibility = Visibility.Collapsed;
            TxtResponse.Visibility = Visibility.Collapsed;
            SPOptionsAfterLogin.Visibility = Visibility.Collapsed;
            FldAmount.Visibility = Visibility.Collapsed;
            BtnDone.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Collapsed;
            TxtWelcome.Visibility = Visibility.Visible;
            BtnExistingUser.Visibility = Visibility.Visible;
            BtnNewUser.Visibility = Visibility.Visible;
        }

        private void ExistingUser(object sender, RoutedEventArgs e)
        {
            BtnExistingUser.Visibility = Visibility.Collapsed;
            BtnNewUser.Visibility = Visibility.Collapsed;
            TxtMainOption.Text = "Login";
            TxtAccountNum.Text = "Enter account number";
            TxtPassword.Text = "Enter password";
            BtnLogin.Content = "Login";
            TxtAccountNum.Visibility = Visibility.Visible;
            FldAccountNum.Visibility = Visibility.Visible;
            TxtPassword.Visibility = Visibility.Visible;
            FldPassword.Visibility = Visibility.Visible;
            BtnLogin.Visibility = Visibility.Visible;
            TxtResponse.Visibility = Visibility.Visible;

            Binding b1 = new Binding("Text");
            b1.Source = FldAccountNum;
            FldAccountNum.SetBinding(TextBox.TextProperty, b1);


            Binding b2 = new Binding("Text");
            b2.Source = FldPassword;
            FldPassword.SetBinding(TextBox.TextProperty, b2);

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

        private void login(object sender, RoutedEventArgs e)
        {
            acctNum = FldAccountNum.Text;
            pass = FldPassword.Text;

            bool isAccount = false;
            List<Customer> desCustomer = deserilization();

            foreach (Customer arg in desCustomer)
            {
                if (acctNum == arg.accountNumber)
                {
                    isAccount = true;
                    if (pass == arg.password)
                    {
                        TxtMainOption.Text = "Welcome " + arg.name;
                        menu();
                    }
                }
            }

            if (!isAccount)
            {
                TxtResponse.Text = "Incorrect Password. Please Try Again.";
            }
            else
            {
                TxtResponse.Text = "Logged In Succesfully!";
            }
        }

        private void deposit(object sender, RoutedEventArgs e)
        {
            SPOptionsAfterLogin.Visibility = Visibility.Collapsed;
            FldAmount.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
            BtnDone.Visibility = Visibility.Visible;
            BtnDone.Content = "Deposit";
            TxtMainOption.Text = "Please enter an amount to deposit!";
            Binding b1 = new Binding("Text");
            b1.Source = FldAmount;
            FldAmount.SetBinding(TextBox.TextProperty, b1);
            isBtnDepositClicked = true;
        }

        private void withdraw(object sender, RoutedEventArgs e)
        {
            SPOptionsAfterLogin.Visibility = Visibility.Collapsed;
            FldAmount.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
            BtnDone.Visibility = Visibility.Visible;
            BtnDone.Content = "Withdraw";
            TxtMainOption.Text = "Please enter an amount to Withdraw!";
            Binding b1 = new Binding("Text");
            b1.Source = FldAmount;
            FldAmount.SetBinding(TextBox.TextProperty, b1);
            isBtnWithDrawClicked = true;
        }

        private void showBalance(object sender, RoutedEventArgs e)
        {
            SPOptionsAfterLogin.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Visible;
            isBtnShowBalanceClicked = true;
            done(sender,e);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            SPOptionsAfterLogin.Visibility = Visibility.Collapsed;
            isBtnExitClicked = true;
            done(sender, e);
        }

        private void done(object sender, RoutedEventArgs e)
        {
            TxtResponse.Visibility = Visibility.Visible;
            FldAmount.Visibility = Visibility.Collapsed;
            BtnDone.Visibility = Visibility.Collapsed;
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
                            arg.balance = arg.balance + amount;
                            TxtMainOption.Text = "Amount Added Successfully!";
                            TxtResponse.Text = "Your new Balance is " + arg.balance;
                            isBtnDepositClicked = false;
                        }
                        else if(isBtnWithDrawClicked)
                        {
                            arg.balance = arg.balance - amount;
                            TxtMainOption.Text = "Amount Withdrawn Successfully!";
                            TxtResponse.Text = "Your new Balance is " + arg.balance;
                            isBtnWithDrawClicked = false;
                        }
                        else if (isBtnShowBalanceClicked)
                        {
                            arg.balance = arg.balance - amount;
                            TxtMainOption.Text = "";
                            TxtResponse.Text = "Your available Balance is " + arg.balance;
                            isBtnShowBalanceClicked = false;
                        }
                        else if (isBtnExitClicked)
                        {
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
            TxtMainOption.Visibility = Visibility.Collapsed;
            BtnBack.Visibility = Visibility.Collapsed;
            menu();
        }

        void menu()
        {
            TxtAccountNum.Visibility = Visibility.Collapsed;
            FldAccountNum.Visibility = Visibility.Collapsed;
            TxtPassword.Visibility = Visibility.Collapsed;
            FldPassword.Visibility = Visibility.Collapsed;
            BtnLogin.Visibility = Visibility.Collapsed;
            TxtResponse.Visibility = Visibility.Collapsed;
            SPOptionsAfterLogin.Visibility = Visibility.Visible;
            TxtOption.Text = "Please select any option to process...";
        }
    }
}
