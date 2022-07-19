using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Library.Model;

namespace Library.App.Windows.Main;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public ObservableCollection<Book> Books { get; set; }

    private Book _book;

    public Book Book
    {
        get => _book;
        set
        {
            if (value is null)
            {
                return;
            }

            if (value == _book)
            {
                return;
            }

            _book = value;
            OnPropertyChanged(nameof(Book));
        }
    }

    public MainWindow()
    {
        _book = new Book();
        Books = new ObservableCollection<Book>(Db.GetBooks());

        InitializeComponent();
        
        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfBooks.ItemsSource);
        view.Filter = item =>
        {
            if(string.IsNullOrEmpty(Search.Text))
                return true;
            else
                return ((item as Book).Title.IndexOf(Search.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Clear()
    {
        ListOfBooks.UnselectAll();

        InputTitle.Clear();
        InputAuthor.Clear();
        InputGenre.Clear();
    }

    private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
    {
        Clear();
    }

    private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
    {
        Books.Remove(Book);
        Db.DeleteBook(Book.Id);
        
        Clear();
    }

    private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
    {
        var book = new Book
        {
            Title = InputTitle.Text,
            Author = InputAuthor.Text,
            Genre = InputGenre.Text
        };
        
        if (!Books.Contains(Book))
        {
            Books.Add(book);
            Db.AddBook(book);
        }
        else
        {
            Books[ListOfBooks.SelectedIndex] = book;
            Db.UpdateBook(book, Book.Id);
        }

        Clear();
    }

    private void Search_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        CollectionViewSource.GetDefaultView(ListOfBooks.ItemsSource).Refresh();
    }
}
