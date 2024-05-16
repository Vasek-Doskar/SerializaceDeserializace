using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Windows.Controls;
namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Pozadi.ItemsSource = new List<string>() { "Modrá", "Zelená", "Bílá" };
            this.Closing += (s, e) => Serializuj();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   // dynamické zjištění cesty k souboru
            string cesta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FILE.txt");
            if (!File.Exists(cesta))
            {
                MessageBox.Show("Soubor neexistuje");
                return;
            }

            //Každá práce se souborem MUSÍ probíhat v bloku using nebo try-catch-finally
            using (StreamWriter sw = new StreamWriter(cesta, true))
            {
                string obsah = Obsah.Text;
                sw.WriteLine(obsah);
                sw.Flush();
                sw.Close();
            }
            Obsah.Text = string.Empty;
            Kontrolka.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void Obsah_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Kontrolka.Foreground = new SolidColorBrush(Colors.Red);
        }


        private void Serializuj()
        {
            Settings set = new();
            set.State1 = (bool)Check1.IsChecked;
            set.State2 = (bool)Check2.IsChecked;
            set.Index = Pozadi.SelectedIndex;

            XmlSerializer writer = new(typeof(Settings));
            string CestaNastaveni = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wpfinit.txt");
            using (FileStream file = File.Create(CestaNastaveni))
            {
                writer.Serialize(file, set);
                file.Close();
            }
        }

        private void Deserializuj()
        {
            XmlSerializer reader = new(typeof(Settings));
        }

        private void Pozadi_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.Background = ((ComboBox)sender).SelectedItem switch
            {
                "Modrá" => new SolidColorBrush(Colors.LightSkyBlue),
                "Zelená" => new SolidColorBrush(Colors.LightGreen),
                "Bílá" => new SolidColorBrush(Colors.White),
                _ => new SolidColorBrush(Colors.White)
            };
        }
    }
}