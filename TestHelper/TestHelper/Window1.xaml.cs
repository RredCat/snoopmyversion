using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TestHelper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="Window1"/> class.
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            //xInputBox.Text =
            //   @"/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAAaABUDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD3WsDw/wCKoNfvLy3jt2h8n54mZgfPj3Mu8egytHjLUpNO8Nzi3Vnu7ki2t0QEszvxxjvjJ/CuVm1KPR7zQbuDSNUs7eyUWdzLc22xGibABJB6hufqa56lXlkl0W/9fievgsAq9CUmveldR17K+3W+kUek0UUV0HkGde6PDf6pYX08kh+xMzxxcbCxGNx4zkdqm1PToNW0y5sLkEwzoUbHUehHuDz+FW6KnlWvma+2qJxafw7eWt/zILK2+x2UFt5ry+VGE8yTG5sDGTjvRU9FUtDOTcm2z//Z";
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Called when [convert click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnConvertClick(object sender, RoutedEventArgs e)
        {
            var bitmap = GenerateImageBitmap( xInputBox.Text );

            if ( null != bitmap )
            {
                xImageCanvas.Source = bitmap;
                xImageCanvas.Height = bitmap.Height;
                xImageCanvas.Width = bitmap.Width;
            }
            else
            {
                xImageCanvas.Source = null;
                xImageCanvas.Height = 0;
                xImageCanvas.Width = 0;
            }
        }
        /// <summary>
        /// Called when [text input key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTextInputKeyDown(object sender, KeyEventArgs e)
        {
            if ( Key.Enter == e.Key )
            {
                OnConvertClick( null, null );
            }
        }

        /// <summary>
        /// Generates the image.
        /// </summary>
        /// <param name="strSource">The string source.</param>
        /// <returns></returns>
        private static BitmapImage GenerateImageBitmap(string strSource)
        {
            if ( string.IsNullOrEmpty( strSource ) )
                return null;

            var bitmap = new BitmapImage();

            try
            {
                byte[] arrByte = Convert.FromBase64String( strSource );

                using ( var stream = new MemoryStream( arrByte ) )
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }
            }
            catch ( FormatException ex )
            {
                MessageBox.Show( ex.Message, "Can't convert data string!"
                    , MessageBoxButton.OK, MessageBoxImage.Error );
                bitmap = null;
            }
            catch ( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message, "Can't fill image current data!"
                   , MessageBoxButton.OK, MessageBoxImage.Error );
                bitmap = null;
            }

            return bitmap;
        }
        #endregion
    }
}