//Welcome use PrintDialog, here is an example below
//PrintDialog have some other great helpers
//Like PaperHelper, PrinterHelper, SettingsHepler
//You can find them in namespace which named by thier name
//They can help you to make and print some good documents
//
//Copyright © Jet Wang 2020

using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;

namespace PrintDialogExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestButtonClick(object sender, RoutedEventArgs e)
        {
            OpenPrintDialog();
        }

        private void OpenPrintDialog()
        {
            //Create a new document ( A document contains many pages )
            //PrintDialog can only print and preview a FixedDocument
            //Here are some codes to make a document, if you already know how to do it, you can skip it or put your document instead
            FixedDocument fixedDocument = new FixedDocument();
            fixedDocument.DocumentPaginator.PageSize = PaperHelper.PaperHelper.GetPaperSize(System.Printing.PageMediaSizeName.ISOA4, true); //Use PaperHelper class to get A4 page size

            //Define document inner margin;
            int margin = 60;

            //Loop 5 times to add 5 pages.
            for (int i = 0; i < 5; i++)
            {
                //Create a new page and set its size
                //Page's size is equals document's size
                FixedPage fixedPage = new FixedPage()
                {
                    Width = fixedDocument.DocumentPaginator.PageSize.Width,
                    Height = fixedDocument.DocumentPaginator.PageSize.Height
                };

                //Create a StackPanel and make it cover entire page
                //FixedPage can contains any UIElement. But VerticalAlignment="Stretch" or HorizontalAlignment="Stretch" doesn't work, so you need calculate its size to make it cover page
                StackPanel stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Background = Brushes.LightYellow,
                    Width = fixedDocument.DocumentPaginator.PageSize.Width - margin * 2, //Width = Page width - (Left margin + Right margin)
                    Height = fixedDocument.DocumentPaginator.PageSize.Height - margin * 2 //Height = Page height - (Top margin + Bottom margin)
                };

                //Put some elements into StackPanel ( As same way as normal and it may have styles, but triggers and animations don't work )
                stackPanel.Children.Add(new TextBlock() { Text = "This is " + (i + 1).ToString() + " Page Title", FontWeight = FontWeights.Bold, FontSize = 28, TextAlignment = TextAlignment.Center, Margin = new Thickness(0, 5, 0, 35) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some regular text.", Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some bold text.", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some italic text.", FontStyle = FontStyles.Italic, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some different color text.", Foreground = Brushes.Red, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock()
                {
                    Text = "This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph.This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph.",
                    MaxWidth = stackPanel.Width,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 5)
                }); //You need to set MaxWidth and TextWrapping properties to make a multi-line paragraph.
                stackPanel.Children.Add(new Button() { Content = "This is a button.", Margin = new Thickness(0, 5, 0, 5), Width = 250, Height = 30, VerticalContentAlignment = VerticalAlignment.Center });
                stackPanel.Children.Add(new Button() { Content = "This is a button with different color.", BorderBrush = Brushes.Black, Background = Brushes.DarkGray, Foreground = Brushes.White, Width = 250, Height = 30, VerticalContentAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new Button() { Content = "This is a button with different color.", BorderBrush = Brushes.Orange, Background = Brushes.Yellow, Foreground = Brushes.OrangeRed, Width = 250, Height = 30, VerticalContentAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBox() { Text = "This is a textbox, but you can't type text in FixedDocument.", Margin = new Thickness(0, 5, 0, 5), Width = 550, Height = 30, VerticalContentAlignment = VerticalAlignment.Center });

                //Set element's margin ( You can set top, bottom, left and right. But usually, we only set top and left )
                //FixedPage doesn't have Margin or Padding property, so if you want a inner margin, you can use a container control ( Like Grid, StackPanel, WrapPanel, etc ) to contains any element in the page and set its margin.
                //You can use Margin property to make same thing, but the best way is use FixedPage.SetTop, FixedPage.SetLeft, FixedPage.SetBottom and FixedPage.SetRight methods
                FixedPage.SetTop(stackPanel, margin); //Top margin
                FixedPage.SetLeft(stackPanel, margin); //Left margin

                //Add element into page
                //You can add many elements into page, but at here we only add one
                fixedPage.Children.Add(stackPanel);

                //Add page into document
                //You can't just add FixedPage into FixedDocument, you need use PageContent to contains FixedPage
                fixedDocument.Pages.Add(new PageContent() { Child = fixedPage });
            }

            //Initialize PrintDialog and set its properties
            PrintDialog.PrintDialog printDialog = new PrintDialog.PrintDialog()
            {
                Owner = this, //Set PrintDialog's owner
                Title = "Test Print", //Set PrintDialog's title
                Icon = null, //Set PrintDialog's icon ( Null means use default icon )
                Topmost = true, //Allow PrintDialog at top most
                ShowInTaskbar = false, //Don't allow PrintDialog show in taskbar
                ResizeMode = ResizeMode.NoResize, //Don't allow PrintDialog resize
                WindowStartupLocation = WindowStartupLocation.CenterScreen, //PrintDialog's startup location is center of the screen

                PrintDocument = fixedDocument, //Set document that need to print
                DocumentName = "Test Document", //Set document name that will display in print list.
                DocumentMargin = margin, //Set document margin info.
                DefaultSettings = new PrintDialog.PrintDialogSettings() //Set default settings. Or you can just use PrintDialog.PrintDialogSettings.PrinterDefaultSettings() to get a PrintDialogSettings that use printer default settings
                {
                    Layout = PrintSettings.PageOrientation.Portrait,
                    Color = PrintSettings.PageColor.Color,
                    Quality = PrintSettings.PageQuality.Normal,
                    PageSize = PrintSettings.PageSize.ISOA4,
                    PageType = PrintSettings.PageType.Plain,
                    TwoSided = PrintSettings.TwoSided.TwoSidedLongEdge,
                    PagesPerSheet = 1,
                    PageOrder = PrintSettings.PageOrder.Horizontal
                },
                //Or DefaultSettings = PrintDialog.PrintDialogSettings.PrinterDefaultSettings().ChangePropertyValue("TwoSided", PrintSettings.TwoSided.TwoSidedLongEdge),

                AllowScaleOption = true, //Allow scale option
                AllowPagesOption = true, //Allow pages option (contains "All Pages", "Current Page", and "Custom Pages")
                AllowTwoSidedOption = true, //Allow two-sided option
                AllowPagesPerSheetOption = true, //Allow pages per sheet option
                AllowPageOrderOption = true, //Allow page order option
                AllowMoreSettingsExpander = true, //Allow more settings expander

                CustomReloadDocumentMethod = ReloadDocumentMethod //Set the method that will use to get document when reload document. You can only change the content in the pages.
            };

            //Show PrintDialog
            //It may need a longer time to connect printers
            //But after first time, it will faster
            if (printDialog.ShowDialog() == true)
            {
                //When Print button clicked, document printed and window closed
                //You can put your own codes at here instead
                MessageBox.Show("Document printed.\nIt need " + printDialog.TotalSheets + " sheet(s) of paper.", "PrintDialog", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                //When Cancel button clicked and window closed
                //You can put your own codes at here instead
                MessageBox.Show("Print job canceled.", "PrintDialog", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private List<PageContent> ReloadDocumentMethod(PrintDialog.DocumentInfo documentInfo)
        {
            //The PrintDialog.CustomReloadDocumentMethod property's value set as this method
            //This method can make the StackPanel cover the page with different margin
            //You must set a parameter and its type is PrintDialog.DocumentInfo
            //You can use this parameter to get the current document settings
            //And this method must return a list of PageContent that include each page content in order
            List<PageContent> pages = new List<PageContent>();

            for (int i = 0; i < 5; i++)
            {
                Size pageSize = PaperHelper.PaperHelper.GetPaperSize(System.Printing.PageMediaSizeName.ISOA4, true);

                FixedPage fixedPage = new FixedPage()
                {
                    Width = pageSize.Width,
                    Height = pageSize.Height
                };

                StackPanel stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Background = Brushes.LightYellow,
                    Width = pageSize.Width - documentInfo.Margin.Value * 2,
                    Height = pageSize.Height - documentInfo.Margin.Value * 2
                };

                stackPanel.Children.Add(new TextBlock() { Text = "This is " + (i + 1).ToString() + " Page Title", FontWeight = FontWeights.Bold, FontSize = 28, TextAlignment = TextAlignment.Center, Margin = new Thickness(0, 5, 0, 35) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some regular text.", Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some bold text.", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some italic text.", FontStyle = FontStyles.Italic, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock() { Text = "These are some different color text.", Foreground = Brushes.Red, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBlock()
                {
                    Text = "This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph.This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph. This is a very long paragraph.",
                    MaxWidth = stackPanel.Width,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 5)
                });
                stackPanel.Children.Add(new Button() { Content = "This is a button.", Margin = new Thickness(0, 5, 0, 5), Width = 250, Height = 30, VerticalContentAlignment = VerticalAlignment.Center });
                stackPanel.Children.Add(new Button() { Content = "This is a button with different color.", BorderBrush = Brushes.Black, Background = Brushes.DarkGray, Foreground = Brushes.White, Width = 250, Height = 30, VerticalContentAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new Button() { Content = "This is a button with different color.", BorderBrush = Brushes.Orange, Background = Brushes.Yellow, Foreground = Brushes.OrangeRed, Width = 250, Height = 30, VerticalContentAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 0, 5) });
                stackPanel.Children.Add(new TextBox() { Text = "This is a textbox, but you can't type text in FixedDocument.", Margin = new Thickness(0, 5, 0, 5), Width = 550, Height = 30, VerticalContentAlignment = VerticalAlignment.Center });

                FixedPage.SetTop(stackPanel, 60);
                FixedPage.SetLeft(stackPanel, 60);

                fixedPage.Children.Add(stackPanel);
                pages.Add(new PageContent() { Child = fixedPage });
            }

            return pages;
        }
    }
}
