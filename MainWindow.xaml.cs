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
            UpdateList();
            GoToGroupBox();
            GoToFaculity();
        }

        //Глобальная переменная для хранения id выбранного студента
        int temp;




        //Метод для заполнения ComboBox Группы
        //https://metanit.com/sharp/wpf/5.8.php
        private void GoToGroupBox()
        {
            //Используем эту конструкцию для подключения к нашей БД и чтобы освободить ресурсы памяти, при окончании ее работы
            using (StudContext db = new())
            {
                //Создаю лист для добавления в него списка групп
                List<int> groups = new List<int>();

                //Делаю запрос к таблице "Группы" и и получу все данные из этой таблицы
                var Grp = db.Gruppas.ToList();

                //С помощью цикла переберу результат запроса, и присвою в свой лист только номера групп
                foreach (var item in Grp)
                {
                    groups.Add((int)item.NumberGroup);
                }

                //Присвою в Groups свой список
                Groups.ItemsSource = groups.ToList();

                //Выберу первый элемент списка для отображения по умолчанию при старте программы
                Groups.SelectedIndex = 0;
            }
        }


        //Метод для заполнения ComboBox Факультеты
        //https://metanit.com/sharp/wpf/5.8.php
        private void GoToFaculity()
        {
            //Используем эту конструкцию для подключения к нашей БД и чтобы освободить ресурсы памяти, при окончании ее работы
            using (StudContext db = new())
            {
                //Создаю лист для добавления в него списка Факультетов
                List<string> facultety = new List<string>();

                //Делаю запрос к таблице "Факультеты" и получу все данные из этой таблицы
                var Grp = db.Fakulities.ToList();

                //С помощью цикла переберу результат запроса, и присвою в свой лист только наименования факультетов
                foreach (var item in Grp)
                {
                    facultety.Add(item.NameFaculity);
                }

                //Присвою в Faculityes свой список
                Faculityes.ItemsSource = facultety.ToList();

                //Выберу первый элемент списка для отображения по умолчанию при старте программы
                Faculityes.SelectedIndex = 0;
            }
        }



        //Метод для обновления нашего списка
        //По сути просто запрос к трем таблицам используя соединение таблиц и выборки всех данных
        private void UpdateList()
        {
            using(StudContext db = new())
            {
                //Многотабличный запрос
                //Объединим три таблицы в один набор:
                //https://metanit.com/sharp/efcore/5.3.php

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

                //Присваиваем результат запроса в нашу табличку
                listviewCards.ItemsSource = query.ToList();
            }
            
        }




        //При нажатии на кнопку Добавить
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить студента
            //https://professorweb.ru/my/entity-framework/6/level3/3_5.php

            using (StudContext db = new())
            {
                // Таблица Студенты содержит два внешних ключа, один ссылается на Группу, второй на Факультет
                // Для того чтобы добавить в таблицу Студенты помимо основных данных, еще и внешние ключи,
                // нам нужно получить Id той группы и факультета, которые в данный момент выбрано в нашем ComboBoxe
                //Нам понадобятся две переменные соотвественно для сохрание Id
                int g = 0;
                int f = 0;

                //Запрос к таблице Группы--> Группы.Где(НомерГруппы равен Номер группы из Combobox)
                //В данном запросе мы получили один объект, номер группы которой соответствует выбранной в данный момент группе из ComboBox
                var grup = db.Gruppas.Where(u => u.NumberGroup == Convert.ToInt64(Groups.Text)).ToList();

                //Перебираем список и присвоим id в нашу переменную
                foreach (var item in grup)
                {
                    g = (int)item.Id;
                    break;
                }

                //Аналогично действия проделаем для получания id факультета, название которой соответствует выбранной в данный момент названию Факультета из ComboBox
                var fac = db.Fakulities.Where(r => r.NameFaculity == Faculityes.Text).ToList();

                //Перебираем список и присвоим id в нашу переменную
                foreach (var item in fac)
                {
                    f = (int)item.Id;
                    break;
                }

                //Для добавления данных в таблицу Студенты, мы создадит объект Студент на основе уже существующего класса Студент и присвоим его полям соответствующие значения

                //Создание объекта
                Student student = new();

                //Присвоим значения
                student.Firstname = Family.Text; //Фамилия
                student.Name = Name.Text; //Имя
                student.Lastname = Lastname.Text; //Отчество
                student.Birthday = Birth.Text; // Дата рождения
                student.FkIdGroup = g; // id группы
                student.FkIdFakulity = f; // id факультета

                //Добавим в нашу таблицу новый объект
                db.Students.Add(student);

                //Сохраним в нашу базу данных новую запись
                db.SaveChanges();

                //Обновим нашу таблицу List
                UpdateList();

                //Выведем сообщение
                MessageBox.Show("Студентик добавлен");
            }
        }


        //При нажатии на кнопку Обновить
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Обновить студента
            //https://professorweb.ru/my/entity-framework/6/level3/3_6.php


            using (StudContext db = new())
            {
                //Пишем запрос
                //Получить список студентов
                var getmyfriends = db.Students
                    //где id равен выбранному в данный момент студенту; При двойном клике по любому студенту, его id сразу присваивается глобальной переменной temp
                    .Where(u => u.Id == temp)
                    //Первая попавшаяся запись
                    .FirstOrDefault();

                //Внести изменения
                getmyfriends.Firstname = Family.Text;
                getmyfriends.Name = Name.Text;
                getmyfriends.Lastname = Lastname.Text;
                getmyfriends.Birthday = Birth.Text;

                //Получим id группы, которая в данный момент выбрана в Combobox
                var getIdGroup = db.Gruppas.Where(u => u.NumberGroup == Convert.ToInt64(Groups.Text));

                foreach (var item in getIdGroup)
                {
                    //Присвоили id
                    getmyfriends.FkIdGroup = item.Id;
                }

                //Получим id факультета, которая в данный момент выбрана в Combobox
                var getMyFaculity = db.Fakulities.Where(u => u.NameFaculity == Faculityes.Text);

                foreach (var item in getMyFaculity)
                {
                    //Присвоили id
                    getmyfriends.FkIdFakulity = item.Id;
                }

                //Сохраним изменения
                db.SaveChanges();

            }

            //Обновим список
            UpdateList();

            //Выведем сообщение
            MessageBox.Show("Запись обновлена");

        }


        //При нажатии на кнопку Удалить
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Удалить студента
            //https://professorweb.ru/my/entity-framework/6/level3/3_7.php


            using (StudContext db = new())
            {
                //Аналогичный запрос
                //Получить студента
                var deleteMyStud = db.Students
                    //Где id равен выбранному стденту в данный момент
                    .Where(u => u.Id == temp)
                    //Первая попавшаяся запись
                    .FirstOrDefault();

                //Удалим студента
                db.Students.Remove(deleteMyStud);

                //Сохраним изменения
                db.SaveChanges();

                //Обновим наш список
                UpdateList();

                //Выведем сообщение
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
