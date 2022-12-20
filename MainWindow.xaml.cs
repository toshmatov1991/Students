using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Students
{

   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ZaprosDlyaSpiska();
            GoToGroupBox();
            GoToFaculity();
        }

        int temp;

        private void GoToGroupBox()
        {
            //Заполним наши группБоксы
            using(StudContext db = new())
            {
                List<int> strings = new List<int>();
                var Grp = db.Gruppas.ToList();
                foreach (var item in Grp)
                {
                    strings.Add((int)item.NumberGroup);
                }
                Groups.ItemsSource = strings.ToList();
                Groups.SelectedIndex = 0;
            }
        }


        private void GoToFaculity()
        {
            //Заполним наши группБоксы
            using (StudContext db = new())
            {
                List<string> strings = new List<string>();
                var Grp = db.Fakulities.ToList();
                foreach (var item in Grp)
                {
                    strings.Add(item.NameFaculity);
                }
                Faculityes.ItemsSource = strings.ToList();
                Faculityes.SelectedIndex = 0;
            }
        }




        private void ZaprosDlyaSpiska()
        {
            using(StudContext db = new())
            {
                //Многотабличный запрос
                var query = from stud in db.Students
                            join grup in db.Gruppas on stud.FkIdGroup equals grup.Id
                            join fakul in db.Fakulities on stud.FkIdFakulity equals fakul.Id
                            select new
                            {
                                id = stud.Id,
                                firstn = stud.Firstname,
                                name = stud.Name,
                                lastn = stud.Lastname,
                                birth = stud.Birthday,
                                gr = grup.NumberGroup,
                                fac = fakul.NameFaculity
                            };
                //Присваиваем результат запроса в наш лист
                listviewCards.ItemsSource = query.ToList();
            }
            
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить студента
            //https://professorweb.ru/my/entity-framework/6/level3/3_5.php


            using (StudContext db = new())
            {
                int g = 0;
                int f = 0;
                var grup = db.Gruppas.Where(u => u.NumberGroup == Convert.ToInt64(Groups.Text)).ToList();
                var fac = db.Fakulities.Where(r => r.NameFaculity == Faculityes.Text).ToList();
                foreach (var item in grup)
                {
                    g = (int)item.Id;
                    break;
                }
                foreach (var item in fac)
                {
                    f = (int)item.Id;
                    break;
                }

                Student student = new();
                student.Firstname = Family.Text;
                student.Name = Name.Text;
                student.Lastname = Lastname.Text;
                student.Birthday = Birth.Text;
                student.FkIdGroup = g;
                student.FkIdFakulity = f;

                db.Students.Add(student);
                db.SaveChanges();

                ZaprosDlyaSpiska();

                MessageBox.Show("Студентик добавлен");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Обновить студента
            //https://professorweb.ru/my/entity-framework/6/level3/3_6.php


            using (StudContext db = new())
            {
                var getmyfriends = db.Students
                    .Where(u => u.Id == temp)
                    .FirstOrDefault();

                //Внести изменения
                getmyfriends.Firstname = Family.Text;
                getmyfriends.Name = Name.Text;
                getmyfriends.Lastname = Lastname.Text;
                getmyfriends.Birthday = Birth.Text;

                var getIdGroup = db.Gruppas.Where(u => u.NumberGroup == Convert.ToInt64(Groups.Text));
                foreach (var item in getIdGroup)
                {
                    getmyfriends.FkIdGroup = item.Id;
                }

                var getMyFaculity = db.Fakulities.Where(u => u.NameFaculity == Faculityes.Text);
                foreach (var item in getMyFaculity)
                {
                    getmyfriends.FkIdFakulity = item.Id;
                }

                db.SaveChanges();

            }

            ZaprosDlyaSpiska();

            MessageBox.Show("Запись обновлена");

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Удалить студента
            //https://professorweb.ru/my/entity-framework/6/level3/3_7.php


            using (StudContext db = new())
            {
                var deleteMyStud = db.Students
                    .Where(u => u.Id == temp)
                    .FirstOrDefault();

                db.Students.Remove(deleteMyStud);
                db.SaveChanges();

                ZaprosDlyaSpiska();
                MessageBox.Show("Студентик удален");


            };


        }

        private void GoToTextBoxes(object sender, MouseButtonEventArgs e)
        {
            //Получить студентика по двойному клику мыши
            string _id = "";
            var str = listviewCards.SelectedItem.ToString();
            //Получаем id пользователя
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    _id += str[i].ToString();
                }
                else if (str[i] == ',')
                {
                    break;
                }
            }

            using(StudContext db = new())
            {
                var query = from stud in db.Students
                            join grup in db.Gruppas on stud.FkIdGroup equals grup.Id
                            join fakul in db.Fakulities on stud.FkIdFakulity equals fakul.Id
                            where stud.Id == Convert.ToInt64(_id)
                            select new
                            {
                                stud.Id,
                                firstn = stud.Firstname,
                                name = stud.Name,
                                lastn = stud.Lastname,
                                birth = stud.Birthday,
                                gr = grup.NumberGroup,
                                fac = fakul.NameFaculity
                            };
                foreach (var item in query)
                {
                    temp = (int)item.Id;
                    Family.Text = item.firstn;
                    Name.Text = item.name;
                    Lastname.Text = item.lastn;
                    Birth.Text = item.birth;
                    Groups.Text = item.gr.ToString();
                    Faculityes.Text = item.fac;
                    break;
                }

            }

        }
    }
}
