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
        //    //GenerateRandomPosAtWork();
        //    //GenerateRandomWorkers(500);
        //    //UpdateWorkersNames(4502);
        //    // GenerateWorkShiftForEmployeeAllYear();
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
    }
}




