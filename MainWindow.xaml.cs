using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
using System.Reflection.Emit;

namespace DLLGeneratorWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte minCountFields = 1;
        byte maxCountFields = 5;
        List<TextBox> txtBoxes = new List<TextBox>();
        List<ComboBox> comboBoxes = new List<ComboBox>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            for (byte i = minCountFields; i <= maxCountFields; i++)
            {
                cmbBoxNumbFields.Items.Add(i);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AssemblyName an = new AssemblyName(txtBoxClassName.Text.ToString());
            an.Version = new Version("1.0.0.0");

            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder mb = ab.DefineDynamicModule(an.Name, an.Name + ".dll");

            TypeBuilder tb = mb.DefineType(txtBoxClassName.Text.ToString(), TypeAttributes.Public);

            Type type = null;
            FieldBuilder fb = null;

            for (int i = 0; i < txtBoxes.Count; i++)
            {
                if (comboBoxes[i].SelectedItem.ToString() == "integer")
                    type = typeof(int);
                else if (comboBoxes[i].SelectedItem.ToString() == "string")
                    type = typeof(string);
                else if (comboBoxes[i].SelectedItem.ToString() == "float")
                    type = typeof(float);
                else if (comboBoxes[i].SelectedItem.ToString() == "double")
                    type = typeof(double);


                fb = tb.DefineField(txtBoxes[i].Text.ToString(), type, FieldAttributes.Private);
            }


            Type[] parameters = { typeof(int), typeof(int) };

            MethodBuilder mb1 = tb.DefineMethod("Multiple",MethodAttributes.Public,typeof(double),parameters);
            ILGenerator ilMb1=mb1.GetILGenerator();
            ilMb1.Emit(OpCodes.Ldarg_0);// Загружаю первый переданный агрумент в стек.
            ilMb1.Emit(OpCodes.Ldarg_1);// Загружаю второй переданный агрумент в стек.
            ilMb1.Emit(OpCodes.Mul);// Умножаю раннее загруженные два значения.
            ilMb1.Emit(OpCodes.Ret);// Возвращаю полученный результат из метода.

            tb.CreateType();

            ab.Save(an.Name + ".dll");

            MessageBox.Show("Successfully generated \"" + an.Name + ".dll\"");
        }

        void ClearFields()
        {
            for (int i = 0; i < txtBoxes.Count; i++)
            {
                mainWindow.mainGrid.Children.Remove(txtBoxes[i]);
                mainWindow.mainGrid.Children.Remove(comboBoxes[i]);
            }

            txtBoxes.Clear();
            comboBoxes.Clear();
        }


        private void cmbBoxNumbFields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearFields();
            comboBoxes.Clear();
            double marginX = 23;
            double marginY = 111;
            TextBox txtBox;
            ComboBox cmbBox;

            for (byte i = 1; i <= (byte)cmbBoxNumbFields.SelectedItem; i++)
            {

                txtBox = new TextBox();
                txtBox.Width = 170;
                txtBox.Height = 23;

                txtBox.Margin = new Thickness(marginX, marginY, 0, 0);
                txtBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                txtBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;


                cmbBox = new ComboBox();
                cmbBox.Items.Add("integer");
                cmbBox.Items.Add("string");
                cmbBox.Items.Add("float");
                cmbBox.Items.Add("double");

                cmbBox.Width = 170;
                cmbBox.Height = 23;

                cmbBox.Margin = new Thickness(marginX + txtBox.Width, marginY, 0, 0);
                cmbBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                cmbBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                marginY += 28;


                txtBoxes.Add(txtBox);
                comboBoxes.Add(cmbBox);
                mainWindow.mainGrid.Children.Add(txtBox);
                mainWindow.mainGrid.Children.Add(cmbBox);
            }

        }


    }
}
