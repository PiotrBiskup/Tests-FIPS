using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Testy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String dlugieSerieOutput;

        public MainWindow()
        {
            InitializeComponent();

            RunButton.IsEnabled = false;
            SaveToFileButton.IsEnabled = false;
        }

        private void keyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-1]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            test1TextBlock.Text = testPojedynczychBitow();
            test2TextBlock.Text = TestSerii();
            test3TextBlock.Text = dlugieSerieOutput;
            test4TextBlock.Text = testPokerowy();
            dlugieSerieOutput = "";
            SaveToFileButton.IsEnabled = true;
        }

        private void LoadFromAFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Text documents (.txt)|*.txt";

            if (ofd.ShowDialog() == true)
            {
                keyTextBox.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void SaveToFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".txt";
            sfd.Filter = "Text documents (.txt)|*.txt";

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, "1. Test pojedynczych bitów:\n" + test1TextBlock.Text + "\n\n2. Test serii:\n" + test2TextBlock.Text +
                                                 "\n\n3. Test długiej serii:\n" + test3TextBlock.Text + "\n\n4. Test pokerowy:\n" + test4TextBlock.Text +
                                                 "\n\nKey:\n" + keyTextBox.Text);
            }
        }

        private void keyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            shouldRunButtonBeActive();
        }

        private void shouldRunButtonBeActive()
        {
            if(keyTextBox.Text.Length > 0)
            {
                RunButton.IsEnabled = true;
            } else
            {
                RunButton.IsEnabled = false;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            keyTextBox.Clear();
            test1TextBlock.Text = "";
            test2TextBlock.Text = "";
            test3TextBlock.Text = "";
            test4TextBlock.Text = "";
            SaveToFileButton.IsEnabled = false;
        }

        private String testPojedynczychBitow()
        {
            String output = "";
            int counter = 0;

            for(int i = 0; i < keyTextBox.Text.Length; i++)
            {
                if (keyTextBox.Text[i] == '1') counter++;
            }

            if(counter > 9725 && counter < 10275)
            {
                output = "Test passed!\n\nAmount of '1' is " + counter + " and it remains between (9725, 10275)";
            }
            else
            {
                output = "Test failed!\n\nAmount of '1' is " + counter + " and it does not remain between (9725, 10275)";
            }

            return output;
        }

        private String TestSerii()
        {
            String output = "";

            int[] serieZer = { 0, 0, 0, 0, 0, 0 };
            int[] serieJedynek = { 0, 0, 0, 0, 0, 0 };
            int[] dlugieSerie = {0, 0 };
            int counter = 0;

            char previous = keyTextBox.Text[0];

            for (int i = 1; i < keyTextBox.Text.Length; i++)
            {
                if(previous == keyTextBox.Text[i])
                {
                    counter++;

                    if(i == keyTextBox.Text.Length - 1)
                    {
                        if(keyTextBox.Text[i] == '0')
                        {
                            if(counter < 5)
                            {
                                serieZer[counter]++;
                            } else
                            {
                                serieZer[5]++;
                                if (counter >= 26) dlugieSerie[0]++;
                            }
                        } else
                        {
                            if (counter < 5)
                            {
                                serieJedynek[counter]++;

                            }
                            else
                            {
                                serieJedynek[5]++;
                                if (counter >= 26) dlugieSerie[1]++;
                            }
                        }
                    }

                } else 
                {
                    if(previous == '0')
                    {
                        if (counter < 5)
                        {
                            serieZer[counter]++;
                            
                        } else
                        {
                            serieZer[5]++;
                            if (counter >= 26) dlugieSerie[0]++;
                        }
                    } else
                    {
                        if (counter < 5)
                        {
                            serieJedynek[counter]++;

                        }
                        else
                        {
                            serieJedynek[5]++;
                            if (counter >= 26) dlugieSerie[1]++;
                        }
                    }

                    counter = 0;
                }

                previous = keyTextBox.Text[i];
            }

            if(serieJedynek[0] >= 2315 && serieJedynek[0] <= 2685 &&
               serieJedynek[1] >= 1114 && serieJedynek[1] <= 1386 &&
               serieJedynek[2] >= 527 && serieJedynek[2] <= 723 &&
               serieJedynek[3] >= 240 && serieJedynek[3] <= 384 &&
               serieJedynek[4] >= 103 && serieJedynek[4] <= 209 &&
               serieJedynek[5] >= 103 && serieJedynek[5] <= 209 &&
               serieZer[0] >= 2315 && serieZer[0] <= 2685 &&
               serieZer[1] >= 1114 && serieZer[1] <= 1386 &&
               serieZer[2] >= 527 && serieZer[2] <= 723 &&
               serieZer[3] >= 240 && serieZer[3] <= 384 &&
               serieZer[4] >= 103 && serieZer[4] <= 209 &&
               serieZer[5] >= 103 && serieZer[5] <= 209)
            {
                output = "Test passed!\n";
            }
            else
            {
                output = "Test failed!\n";
            }

            output += "\nFor \"1\":\n1 in a row: " + serieJedynek[0] +
                "\n2 in a row: " + serieJedynek[1] +
                "\n3 in a row: " + serieJedynek[2] +
                "\n4 in a row: " + serieJedynek[3] +
                "\n5 in a row: " + serieJedynek[4] +
                "\n6 or more in a row: " + serieJedynek[5] +
                "\n\nFor \"0\":\n1 in a row: " + serieZer[0] +
                "\n2 in a row: " + serieZer[1] +
                "\n3 in a row: " + serieZer[2] +
                "\n4 in a row: " + serieZer[3] +
                "\n5 in a row: " + serieZer[4] +
                "\n6 or more in a row: " + serieZer[5];

            if (dlugieSerie[0] > 0 || dlugieSerie[1] > 0)
            {
                dlugieSerieOutput = "Test failed!\n\nNumber of series with 26 or more\nFor \"0\": " + dlugieSerie[0] +
                    "\nFor \"1\": " + dlugieSerie[1];
            }
            else dlugieSerieOutput = "Test passed!\n\nThere is no series with 26 or more \"0\" or \"1\"";

            return output;
        }

        private String testPokerowy()
        {
            String output = "";
            double[] tabWartosci = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            int counter = 0;
            String temp = "";
            int result;
            double X;

            for(int i = 0; i < keyTextBox.Text.Length; i++)
            {
                temp += keyTextBox.Text[i];
                counter++;
                if(counter == 4)
                {
                    counter = 0;
                    result = Convert.ToInt32(temp, 2);

                    if (result == 0) tabWartosci[0]++;
                    else if (result == 1) tabWartosci[1]++;
                    else if (result == 2) tabWartosci[2]++;
                    else if (result == 3) tabWartosci[3]++;
                    else if (result == 4) tabWartosci[4]++;
                    else if (result == 5) tabWartosci[5]++;
                    else if (result == 6) tabWartosci[6]++;
                    else if (result == 7) tabWartosci[7]++;
                    else if (result == 8) tabWartosci[8]++;
                    else if (result == 9) tabWartosci[9]++;
                    else if (result == 10) tabWartosci[10]++;
                    else if (result == 11) tabWartosci[11]++;
                    else if (result == 12) tabWartosci[12]++;
                    else if (result == 13) tabWartosci[13]++;
                    else if (result == 14) tabWartosci[14]++;
                    else if (result == 15) tabWartosci[15]++;

                    temp = "";
                }
            }

            double suma = 0.0;
            for(int j = 0; j < tabWartosci.Length; j++)
            {
                suma += tabWartosci[j] * tabWartosci[j];
            }

            Console.WriteLine("dadasd: " + suma);

            X = ( (16.0 / 5000.0 ) * suma) - 5000.0;
            Console.WriteLine("dadasd: " + X);

            if(X > 2.16 && X < 46.17)
            {
                output = "Test passed!\n\nX is in (2.16, 46.17)\nX = " + X;
            } else output = "Test failed!\n\nX is not in (2.16, 46.17)\nX = " + X;


            return output;
        }
    }
}
