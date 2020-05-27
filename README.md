# RestaurantPicker
C# WPF Application that will pick a random restaurant for you

## Dependencies
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [Python](https://www.python.org/downloads/)

### Application Changes
Within the MainWindow.xaml.cs file you will need to change two methods:
1. **Gen_Button_Click**
```C# 
line 26: string script = @"Y:\Documents\Projects\Python\RestaurantPicker\Restaurant_Picker.py";
```
to the path of the python program within this project.

2. **run_cmd**
```C#
line 142: p.StartInfo = new ProcessStartInfo(@"Y:\Python\python.exe", args)
```
to the path of the python executable on your machine.

If you live outside of Louisville, KY, do not worry! 
Go into the Restaurant_Picker.py file and change lines 8 and 9 to whatever city / state you need to download the restaurant data.
```Python
line 8: "city": "Louisville",
line 9: "state": "Kentucky"
```
