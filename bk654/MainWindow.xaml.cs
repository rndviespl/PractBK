using bk654.Data;
using bk654.Models;
using bk654.PerfomanceReviewsFolder;
using bk654.PositionAtWorkFolder;
using bk654.RestaurantFolder;
using bk654.WorkerFolder;
using bk654.WorkShiftFolder;
using System.Windows;

namespace bk654
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationContext dbContext;
        private List<WorkShift> _workShifts;
        public MainWindow()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();

        }
        private DataTablePosAtWork dataTablePosAtWork;
        private bool dataTablePosAtWorkOpen = false;
        private WorkerInformation viewWorker;
        private bool workerOpen = false;
        private ViewWorkerShift viewWorkShift;
        private bool viewWorkShiftOpen = false;
        private ViewRestaurant viewRestaurant;
        private bool viewRestaurantOpen = false;
        private DataGridPerfomanceReview performanceReview;
        private bool performanceReviewOpen = false;

        private void Table1_Click(object sender, RoutedEventArgs e)
        {

            if (!dataTablePosAtWorkOpen)
            {
                dataTablePosAtWork = new DataTablePosAtWork();
                dataTablePosAtWork.Closed += (s, args) => dataTablePosAtWorkOpen = false;
                dataTablePosAtWorkOpen = true;
                dataTablePosAtWork.Show();
            }
            else
            {
                // Можно показать сообщение о том, что окно уже открыто
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void Table2_Click(object sender, RoutedEventArgs e)
        {
            if (!workerOpen)
            {
                viewWorker = new WorkerInformation();
                viewWorker.Closed += (s, args) => workerOpen = false;
                workerOpen = true;
                viewWorker.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }
        //uu
        private void Table3_Click(object sender, RoutedEventArgs e)
        {
            if (!viewWorkShiftOpen)
            {
                viewWorkShift = new ViewWorkerShift();
                viewWorkShift.Closed += (s, args) => viewWorkShiftOpen = false;
                viewWorkShiftOpen = true;
                viewWorkShift.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        //public void Button_Click(object sender, RoutedEventArgs e)
        //{
        //   // GenerateWorkShiftForEmployeeAllYear();
        //}

        private void Restaurant_Click(object sender, RoutedEventArgs e)
        {
            if (!viewRestaurantOpen)
            {
                viewRestaurant = new ViewRestaurant();
                viewRestaurant.Closed += (s, args) => viewRestaurantOpen = false;
                viewWorkShiftOpen = true;
                viewRestaurant.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }
        private void PerfomanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!performanceReviewOpen)
            {
                performanceReview = new DataGridPerfomanceReview();
                performanceReview.Closed += (s, args) => performanceReviewOpen = false;
                performanceReviewOpen = true;
                performanceReview.Show();
            }
        }
       

        Random random = new Random();
        public void GenerateRandomWorkers(int numWorkers)
        {
            List<int> restaurantIds = Enumerable.Range(1, 50).ToList();
            Random random = new Random();


            for (int i = 1; i <= numWorkers; i++)
            {
                var male = random.Next(2) == 0;
                Worker worker = new Worker
                {
                    Surname = GetRandomSurname(male),
                    Name = GetRandomName(male),
                    Patronymic = GetRandomPatronymic(),
                    StartDate = DateTime.Now.AddDays(-random.Next(1, 365)),
                    EndDate = null,
                    DismissalReason = "",
                    PositionId = GetRandomPositionId(),
                    RestaurantId = GetRandomRestaurantId()
                };

                dbContext.Workers.Add(worker);
                dbContext.SaveChanges();

                //Получаем список всех работников
                List<Worker> workers = dbContext.Workers.ToList();

                //Проходим по списку и меняем фамилию у каждого работника
                foreach (var individualWorker in workers)
                {
                    individualWorker.Surname = GetRandomSurname(random.Next(2) == 0); // Изменяем фамилию

                    // Обновляем запись работника
                    dbContext.Workers.Update(individualWorker);
                }


                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();



            } 
        }
        private string GetRandomSurname(bool isMale)
            {
                List<string> names = isMale ? ["Краснов", "Зубов", "Панкратов", "Абрамов", "Лыков", "Михайлов", "Румянцев", "Игнатов", "Литвинов", "Васильев", "Кузьмин", "Владимиров", "Александров", "Латышев", "Борисов", "Терехов", "Петров",
                        "Гришин", "Филиппов", "Макеев", "Казаков", "Юдин", "Козлов", "Белов", "Горлов", "Воробьев", "Олейников", "Федотов", "Архипов", "Морозов", "Крылов", "Павлов", "Гусев", "Быков", "Куликов", "Родионов", "Захаров", "Баранов",
                        "Комаров", "Осипов", "Павлов", "Моисеев", "Никифоров", "Скворцов", "Павлов", "Никифоров", "Пономарев", "Михайлов", "Иванов", "Кузнецов", "Борисов", "Филатов", "Богданов", "Овсянников", "Сизов", "Капустин", "Котов", "Капустин"]


                 : ["Краснова", "Сахарова", "Абрамова", "Лыкова", "Михайлова", "Румянцева", "Игнатова", "Литвинова", "Васильева", "Кузьмина", "Владимирова", "Александрова", "Латышева", "Борисова", "Краснова", "Макеева", "Казакова", "Лебедева",
                         "Горелова", "Казакова", "Петрова", "Козлова", "Белова", "Горлова", "Сергеева", "Глушкова", "Овсянникова", "Соловьева", "Никифорова", "Тарасова", "Скворцова", "Павлова", "Максимова", "Гусева", "Быкова", "Куликова", "Козлова",
                         "Кулешова", "Панфилова", "Масленникова", "Овсянникова", "Соловьева", "Баранова", "Никифорова", "Тарасова", "Сазонова"];
                return names[new Random().Next(names.Count)];
            }

        private string GetRandomName(bool isMale)
        {
            List<string> names = isMale ? new List<string> { "Иван", "Максим", "Григорий", "Станислав", "Пётр", "Алексей", "Максим", "Тигран", "Даниил", "Демид", "Георгий", "Марьям", "Кирилл", "Денис", "Дмитрий", "Роман", "Алексей", "Илья", }
            : new List<string> { "Анна", "Мария", "Екатерина", "Фатима", "Дарья", "Ясмина", "Юлия", "Алиса", "Маргарита", "Вероника", "Лилия", "Майя", "Александра", "Елизавета", "Вероника", "Милана", "Ульяна", "Мария", "Камилла", "Арина", "Элина", "Есения", };
            return names[new Random().Next(names.Count)];
        }


        private string GetRandomPatronymic()
        {
            List<string> patronymics = [ "Данииловна", "Андреевна", "Никитична", "Михайловна", "Александровна", "Всеволодовна", "Эминовна", "Александровна", "Артёмовна", "Михайловна", "Демидовна", "Юрьевич", "Макаровна", "Матвеевна", "Матвеевна", "Фёдоровна", "Сергеевна", "Захарович", "Родионовна", "Александрович", "Ильич",
                        "Романович", "Макарович", "Микайловна", "Николаевна", "Олеговна", "Степанович", "Максимович", "Дмитриевна", "Евгеньевич", "Владиславовна", "Даниилович", "Иванович", "Алиевна", "Денисович", "Яновна", "Егорович", "Михайлович", "Фёдоровна", "Артёмовна", "Матвеевна", "Тимуровна", "Михаиловна", "Станиславовна", "Артёмович", "Серафимовна",
                        "Ярославович", "Викторовна", "Миронович", "Алиевна", "Юрьевна", "Георгиевич", "Иванович", "Андреевич", "Григорьевна", "Александровна", "Владиславовна", "Михайловна", "Робертович", "Дмитриевич", "Александро..." ];

            return patronymics[new Random().Next(patronymics.Count)];
        }

        private Worker GetRandomWorker()
        {
            Worker worker = new Worker
            {
                WorkerId = random.Next(1, 1000)
            };
            return worker;
        }

        private int GetRandomPositionId()
        {
            List<int> positionIds = new List<int> { 1, 2, 3, 3, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7, 7, 8, 8, 8 };
            return positionIds[new Random().Next(positionIds.Count)];
        }

        private int GetRandomRestaurantId()
        {
            // Assuming restaurant IDs range from 1 to 100
            List<int> idRest = new List<int>();
            idRest.Add(random.Next(1, 51));
            return idRest[new Random().Next(idRest.Count)];
        }
        public void GenerateWorkShiftForEmployeeAllYear()
        {
            Random rnd = new Random();

            var workerIds = dbContext.Workers.Select(w => w.WorkerId).ToList();

            foreach (int workerId in workerIds)
            {
                for (DateTime date = new DateTime(DateTime.Now.Year, 1, 1); date <= DateTime.Now; date = date.AddDays(1))
                {
                    // DayOfWeek randomRestDay = (DayOfWeek)rnd.Next(1, 6); // Выбор случайного дня недели как выходного

                    if (date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        int hourStart = rnd.Next(8, 17);
                        int minStart = rnd.Next(0, 60);
                        int shiftDuration = rnd.Next(4, 12);
                        int totalMinutes = hourStart * 60 + minStart + shiftDuration * 60;

                        if (totalMinutes > 1330)
                        {
                            shiftDuration = (1330 - (hourStart * 60 + minStart)) / 60;
                        }

                        int hourEnd = hourStart + shiftDuration;
                        int minEnd = rnd.Next(0, 60);

                        DateTime startShift = new DateTime(date.Year, date.Month, date.Day, hourStart, minStart, 0);
                        DateTime endShift = new DateTime(date.Year, date.Month, date.Day, hourEnd, minEnd, 0);

                        WorkShift newWorkShift = new WorkShift
                        {
                            WorkerId = workerId,
                            StartShift = startShift,
                            EndShift = endShift,
                            DescriptionManualEntry = "Generated Shift"
                        };

                        dbContext.WorkShifts.Add(newWorkShift);
                    }
                }
            }
            dbContext.SaveChanges();
        }
    }
}





