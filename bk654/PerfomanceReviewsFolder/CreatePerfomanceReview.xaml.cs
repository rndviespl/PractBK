using bk654.Data;
using bk654.Models;
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
using System.Windows.Shapes;

namespace bk654.PerfomanceReviewsFolder
{
    /// <summary>
    /// Логика взаимодействия для CreatePerfomanceReview.xaml
    /// </summary>
    public partial class CreatePerfomanceReview : Window
    {
        private ApplicationContext dbContext;
        private List<PerformanceReview> perfomanceReviews;
        public CreatePerfomanceReview()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }
        private void ReviewerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ReviewerNameTextBox.Text.Length > 25)
            {
                ReviewerNameTextBox.Text = ReviewerNameTextBox.Text.Substring(0, 25);
                MessageBox.Show("Превышение длины! Имя обрезано до 25 символов.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void CommentsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ReviewerNameTextBox.Text.Length > 25)
            {
                ReviewerNameTextBox.Text = ReviewerNameTextBox.Text.Substring(0, 25);
                MessageBox.Show("Превышение длины! Имя обрезано до 25 символов.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void addReviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(PerformanceRatingTextBox.Text.Substring(0, 2), out int rating))
                {
                    // Create and initialize the performanceReview object
                    PerformanceReview performanceReview = new PerformanceReview
                    {
                        WorkerId = int.Parse(WorkerIdTextBox.Text),
                        ReviewerName = ReviewerNameTextBox.Text,
                        ReviewDate = ReviewDatePicker.SelectedDate ?? DateTime.Now,
                        PerformanceRating = rating, // Use the parsed rating
                        Comments = CommentsTextBox.Text
                    };
                    dbContext.PerformanceReviews.Add(performanceReview);
                    dbContext.SaveChanges();
                }

                MessageBox.Show("отзыв успешно добавлен", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
