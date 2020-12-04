using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace Random_Restaurant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Restaurant> restaurant_list = new List<Restaurant>();
        public MainWindow()
        {
            InitializeComponent();
            DeSerialize();
        }

        private void Gen_Button_Click(object sender, RoutedEventArgs e)
        {
            string script = "Restaurant_Picker.py";
            run_cmd(script);
            DeSerialize();
        }

        private void Find_Restaurants_Button_Click(object sender, RoutedEventArgs e)
        {
            //Variables
            List<Restaurant> chosen_restaurants = new List<Restaurant>();
            List<string> chosen_cuisines = new List<string>();
            
            //Clear the error box
            Error_Label.Text = "";

            //Getting checkbox values and adding them to cuisines list
            if ((bool)American_CB.IsChecked)
                chosen_cuisines.Add("American");
            if ((bool)Chinese_CB.IsChecked)
                chosen_cuisines.Add("Chinese");
            if ((bool)Mexican_CB.IsChecked)
                chosen_cuisines.Add("Mexican");
            if ((bool)Irish_CB.IsChecked)
                chosen_cuisines.Add("Irish");
            if ((bool)Pizza_CB.IsChecked)
                chosen_cuisines.Add("Pizza");
            if ((bool)Fast_Food_CB.IsChecked)
                chosen_cuisines.Add("Fast Food");
            if ((bool)Bar_CB.IsChecked)
                chosen_cuisines.Add("Bar");
            if ((bool)Pub_CB.IsChecked)
                chosen_cuisines.Add("Pub");
            if ((bool)Vegetarian_Friendly_CB.IsChecked)
                chosen_cuisines.Add("Vegetarian Friendly");
            if ((bool)Seafood_CB.IsChecked)
                chosen_cuisines.Add("Seafood");
            if ((bool)Italian_CB.IsChecked)
                chosen_cuisines.Add("Italian");

            //Get Restaurants based off user's choice
            if (chosen_cuisines.Count == 0)
                chosen_restaurants.AddRange(restaurant_list);
            else
                chosen_restaurants = restaurant_list.Where(res => res.Cuisine.Any(y => chosen_cuisines.Contains(y))).ToList();

            //Generate random number
            RandomGenerator rand = new RandomGenerator();
            int randInt = rand.Next(0, chosen_restaurants.Count);

            //Select restaurant at random number and display data
            Title_Label.Content = "You should try...";
            Name_Label.Content = $"{chosen_restaurants[randInt].Name}";
            Cuisine_Label.Content = $"{string.Join(", ", chosen_restaurants[randInt].Cuisine)}";
            Address_Label.Content = $"{chosen_restaurants[randInt].Address}";
            Website_Label.Text = $"{chosen_restaurants[randInt].Website}";
            if(chosen_restaurants[randInt].Website != null)
                Website_Uri.NavigateUri = new Uri(chosen_restaurants[randInt].Website);
            else
                Website_Uri.NavigateUri = new Uri("https://www.google.com/search?q=" + chosen_restaurants[randInt].Name);
        }

        private void Surprise_Button_Click(object sender, RoutedEventArgs e)
        {
            //Clear the error box
            Error_Label.Text = "";

            //Generate random number
            RandomGenerator rand = new RandomGenerator();
            int randInt = rand.Next(0, 1001);

            //Select restaurant at random number and display data
            Title_Label.Content = "You should try...";
            Name_Label.Content = $"{restaurant_list[randInt].Name}";
            Cuisine_Label.Content = $"{string.Join(", ", restaurant_list[randInt].Cuisine)}";
            Address_Label.Content = $"{restaurant_list[randInt].Address}";
            Website_Label.Text = $"{restaurant_list[randInt].Website}";
            if(restaurant_list[randInt].Website != null)
                Website_Uri.NavigateUri = new Uri(restaurant_list[randInt].Website);
            else
                Website_Uri.NavigateUri = new Uri("https://google.com/" + restaurant_list[randInt].Name);
        }

        private void RequestToNavigate(object sender, RequestNavigateEventArgs e)

        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Read serialized data into list of objects
        /// </summary>
        private void DeSerialize()
        {
            try
            {
                if (File.Exists("restaurants.txt"))
                {
                    string JSONtxt = File.ReadAllText("restaurants.txt");
                    JSONtxt = JSONtxt.Substring(12, JSONtxt.Length - 13);
                    restaurant_list = JsonConvert.DeserializeObject<List<Restaurant>>(JSONtxt);
                }
                else
                {
                    Error_Label.Text = "Please click the 'Refresh Data' button to get restaurant list.";
                }
            }
            catch (Exception e)
            {
                Error_Label.Text = e.Message;
            }
        }

        /// <summary>
        /// Runs Python script that's passed in as argument
        /// </summary>
        /// <param name="args">Full file path to Python script</param>
        private void run_cmd(string args)
        {
            try
            {
                Error_Label.Text = "Starting Python script...\n";
                string PYTHON_PATH = "";
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Python\\PythonCore\\3.8-32\\InstallPath"))
                    {
                        if (key != null)
                        {
                            Object o = key.GetValue("ExecutablePath");
                            if (o != null)
                            {
                                PYTHON_PATH = o.ToString();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Error_Label.Text = e.Message;
                }

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(PYTHON_PATH, args)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                Error_Label.Text += output;
            }
            catch (Exception e)
            {
                Error_Label.Text = e.Message;
            }
        }
    }
}
