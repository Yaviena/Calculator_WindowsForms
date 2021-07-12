using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.WindowsFormsApp
{
    public enum Operation
    {
        None,
        Addition,
        Substraction,
        Multiplication,
        Dividion
    }
    public partial class Form1 : Form
    {
        private string _firstValue;
        private string _secondValue;
        private Operation _currentOperation = Operation.None;   // na wstępie nie jest wybrana żadna operacja
        private bool _isTheResultOnTheScreen;                    // czy jest obecnie wyświetlany wynik
        public Form1()
        {
            InitializeComponent();

            tbScreen.Text = "0";                            // zero defaultowo kiedy zaczynamy
        }

        private void OnBtnNumberClick(object sender, EventArgs e)
        {
            var clickedValue = (sender as Button).Text;     // pobieram tekst z klikniętego przycisku

            if (tbScreen.Text == "0" && clickedValue != ",")
                tbScreen.Text = string.Empty;

            if (_isTheResultOnTheScreen)
            {
                _isTheResultOnTheScreen = false;
                tbScreen.Text = string.Empty;

                if (clickedValue == ",")
                {
                    clickedValue = "0,";
                }
            }

            tbScreen.Text += clickedValue;                  // wypisujemy/ dopisujemy wciśnięty btn na "wyświetlaczu"
            SetResultBtnState(true);                        // if klikniemy jakąś cyfrę to btn wyniku może być już dostępny

            if (_currentOperation != Operation.None)
            {
                _secondValue += clickedValue;
            }
            else
            {
                SetOperationBtnState(true);                 // odblokowujemy przyciski z operacjami ale tylko wtedy gdy żadna operacja nie została jeszcze wybrana
            }
        }

        private void OnBtnOperationClick(object sender, EventArgs e)
        {
            _firstValue = tbScreen.Text;                    // wszystkie te cyfry, już które wpisaliśmy

            var operation = (sender as Button).Text;        // sprawdzam który btn został kliknięty

            _currentOperation = operation switch
            {
                "+" => Operation.Addition,
                "-" => Operation.Substraction,
                "*" => Operation.Multiplication,
                "/" => Operation.Dividion,
                _ => Operation.None,
            };

            tbScreen.Text += $" {operation} ";              // doklejamy do liczby kliknięte działanie

            if (_isTheResultOnTheScreen)                    // if user wyświetlił wynik i kliknął jakąś operację, to tez trzeba to ustawić na false
                _isTheResultOnTheScreen = false;

            SetOperationBtnState(false);                    // działanie nie zostało skończone więc nie można kliknąć ani operacji
            SetResultBtnState(false);                       // ani wyniku
        }

        private void OnBtnResultClick(object sender, EventArgs e)
        {
            if (_currentOperation == Operation.None)
            {
                return;
            }

            var firstNumber = double.Parse(_firstValue);
            var secondNumber = double.Parse(_secondValue);
            var result = Calculate(firstNumber, secondNumber);
            tbScreen.Text = result.ToString();
            _secondValue = string.Empty;                        // pierwszej liczby nie zeruję, bo user może chcieć np. ciągle dodawać do wyniku kolejną liczbę 
            _currentOperation = Operation.None;
            _isTheResultOnTheScreen = true;
            SetOperationBtnState(true);
            SetResultBtnState(true);
        }

        private void OnBtnClearClick(object sender, EventArgs e)
        {
            tbScreen.Text = "0";
            _firstValue = string.Empty;
            _secondValue = string.Empty;
            _isTheResultOnTheScreen = false;                // wydaje się nie być konieczne bo i tak ustawia się na false
            _currentOperation = Operation.None;
        }
        private double Calculate (double firstNumber, double secondNumber)
        {
            switch (_currentOperation)
            {
                case Operation.None:
                    return firstNumber;
                case Operation.Addition:
                    return firstNumber + secondNumber;
                case Operation.Substraction:
                    return firstNumber - secondNumber;
                case Operation.Multiplication:
                    return firstNumber * secondNumber;
                case Operation.Dividion:
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("Nie można dzielić przez zero!");
                        return 0;
                    }
                    return firstNumber / secondNumber;
                default:
                    break;
            }
            return 0;
        }
        private void SetOperationBtnState(bool value)
        {
            btnAdd.Enabled = value;
            btnSubstraction.Enabled = value;
            btnMultiplication.Enabled = value;
            btnDivision.Enabled = value;
        }
        private void SetResultBtnState(bool value)
        {
            btnResult.Enabled = value;
        }
    }
}
