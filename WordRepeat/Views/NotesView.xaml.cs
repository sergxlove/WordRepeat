using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WordRepeat.Abstractions;
using WordRepeat.Application.Abstractions;
using WordRepeat.Core.Models;
using WordRepeat.Models;

namespace WordRepeat.Views
{
    public partial class NotesView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;
        private readonly INotesService _notesService;
        private readonly INotificationService _notificationService;
        private ObservableCollection<Notes> _allNotes = new ObservableCollection<Notes>();
        private ObservableCollection<Notes> _displayedNotes = new ObservableCollection<Notes>();
        private CancellationTokenSource _cancellationTokenSource;

        private enum ModalMode { Create, Edit, View }
        private ModalMode _currentMode;
        private Notes _currentNote;

        public NotesView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
            _cancellationTokenSource = new CancellationTokenSource();
            _notesService = _serviceProvider.GetRequiredService<INotesService>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();

            // Привязываем DataGrid к ObservableCollection
            NotesDataGrid.ItemsSource = _displayedNotes;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadNotesAsync();
        }

        private async Task LoadNotesAsync()
        {
            try
            {
                var notes = await _notesService.GetAllAsync(_cancellationTokenSource.Token);
                _allNotes.Clear();
                foreach (var note in notes.OrderByDescending(n => n.DateUpdate))
                {
                    _allNotes.Add(note);
                }

                ApplyFilterAndSearch();
            }
            catch (OperationCanceledException)
            {
                // Операция отменена
            }
            catch (Exception)
            {
                _notificationService.ShowError("Ошибка загрузки заметок");
            }
        }

        private void ApplyFilterAndSearch()
        {
            if (SearchTextBox == null) return;

            var query = _allNotes.AsEnumerable();

            string searchText = SearchTextBox.Text?.Trim().ToLower() ?? string.Empty;
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(n => (n.Title?.ToLower().Contains(searchText) ?? false) ||
                                        (n.Content?.ToLower().Contains(searchText) ?? false));
            }

            // Обновляем отображаемую коллекцию
            _displayedNotes.Clear();
            foreach (var note in query)
            {
                _displayedNotes.Add(note);
            }
        }

        // Двойной клик по заметке для просмотра
        private void NotesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedNote = NotesDataGrid.SelectedItem as Notes;
            if (selectedNote != null)
            {
                OpenViewModal(selectedNote);
            }
        }

        private void CreateNoteButton_Click(object sender, RoutedEventArgs e)
        {
            _currentMode = ModalMode.Create;
            _currentNote = null;

            ModalTitleIcon.Text = "➕";
            ModalTitle.Text = "Создание заметки";
            TitleTextBox.Text = string.Empty;
            ContentTextBox.Text = string.Empty;

            TitleTextBox.IsReadOnly = false;
            ContentTextBox.IsReadOnly = false;

            // Скрываем панель с датой
            DateUpdatePanel.Visibility = Visibility.Collapsed;

            SaveNoteButton.Visibility = Visibility.Visible;
            EditFromViewButton.Visibility = Visibility.Collapsed;
            DeleteFromViewButton.Visibility = Visibility.Collapsed;
            CancelModalButton.Visibility = Visibility.Visible;

            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void OpenViewModal(Notes note)
        {
            _currentMode = ModalMode.View;
            _currentNote = note;

            ModalTitleIcon.Text = "👁️";
            ModalTitle.Text = "Просмотр заметки";
            TitleTextBox.Text = note.Title;
            ContentTextBox.Text = note.Content;

            TitleTextBox.IsReadOnly = true;
            ContentTextBox.IsReadOnly = true;

            // Показываем панель с датой и устанавливаем значение
            DateUpdatePanel.Visibility = Visibility.Visible;
            DateUpdateText.Text = note.DateUpdate.ToString("dd.MM.yyyy HH:mm:ss");

            SaveNoteButton.Visibility = Visibility.Collapsed;
            EditFromViewButton.Visibility = Visibility.Visible;
            DeleteFromViewButton.Visibility = Visibility.Visible;
            CancelModalButton.Visibility = Visibility.Visible;

            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void EditFromViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentNote != null)
            {
                _currentMode = ModalMode.Edit;

                ModalTitleIcon.Text = "✏️";
                ModalTitle.Text = "Редактирование заметки";

                TitleTextBox.IsReadOnly = false;
                ContentTextBox.IsReadOnly = false;

                // Скрываем панель с датой при редактировании
                DateUpdatePanel.Visibility = Visibility.Collapsed;

                SaveNoteButton.Visibility = Visibility.Visible;
                EditFromViewButton.Visibility = Visibility.Collapsed;
                DeleteFromViewButton.Visibility = Visibility.Collapsed;
                CancelModalButton.Visibility = Visibility.Visible;
            }
        }

        private async void DeleteFromViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentNote != null)
            {
                try
                {
                    await _notesService.DeleteAsync(_currentNote.Id, _cancellationTokenSource.Token);

                    // Удаляем из коллекций
                    var noteToRemove = _allNotes.FirstOrDefault(n => n.Id == _currentNote.Id);
                    if (noteToRemove != null)
                    {
                        _allNotes.Remove(noteToRemove);
                    }

                    _notificationService.ShowSuccess("Заметка успешно удалена");
                    CloseModal();
                    ApplyFilterAndSearch(); // Обновляем отображение без перезагрузки
                }
                catch (Exception)
                {
                    _notificationService.ShowError("Ошибка при удалении заметки");
                }
            }
        }

        private async void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                _notificationService.ShowError("Необходимо ввести название заметки");
                return;
            }

            try
            {
                if (_currentMode == ModalMode.Edit && _currentNote != null)
                {
                    bool hasChanges = false;

                    // Обновляем заголовок
                    if (_currentNote.Title != TitleTextBox.Text.Trim())
                    {
                        await _notesService.UpdateTitleAsync(_currentNote.Id, TitleTextBox.Text.Trim(), _cancellationTokenSource.Token);
                        _currentNote.Title = TitleTextBox.Text.Trim();
                        hasChanges = true;
                    }

                    // Обновляем содержание
                    if (_currentNote.Content != ContentTextBox.Text.Trim())
                    {
                        await _notesService.UpdateContentAsync(_currentNote.Id, ContentTextBox.Text.Trim(), _cancellationTokenSource.Token);
                        _currentNote.Content = ContentTextBox.Text.Trim();
                        hasChanges = true;
                    }

                    if (hasChanges)
                    {
                        // Обновляем дату
                        _currentNote.DateUpdate = DateTime.UtcNow;

                        // Обновляем заметку в коллекции _allNotes
                        var existingNote = _allNotes.FirstOrDefault(n => n.Id == _currentNote.Id);
                        if (existingNote != null)
                        {
                            existingNote.Title = _currentNote.Title;
                            existingNote.Content = _currentNote.Content;
                            existingNote.DateUpdate = _currentNote.DateUpdate;
                        }

                        // Пересортировываем коллекцию
                        var sortedNotes = _allNotes.OrderByDescending(n => n.DateUpdate).ToList();
                        _allNotes.Clear();
                        foreach (var note in sortedNotes)
                        {
                            _allNotes.Add(note);
                        }

                        _notificationService.ShowSuccess("Заметка успешно обновлена");
                        CloseModal();
                        ApplyFilterAndSearch(); // Обновляем отображение
                    }
                    else
                    {
                        _notificationService.ShowInfo("Изменений не было");
                        CloseModal();
                    }
                }
                else if (_currentMode == ModalMode.Create)
                {
                    var createResult = Notes.Create(TitleTextBox.Text.Trim(), ContentTextBox.Text.Trim());

                    if (createResult.IsSuccess)
                    {
                        var newNote = createResult.Value;
                        await _notesService.AddAsync(newNote, _cancellationTokenSource.Token);

                        // Добавляем в коллекцию
                        _allNotes.Add(newNote);

                        // Пересортировываем
                        var sortedNotes = _allNotes.OrderByDescending(n => n.DateUpdate).ToList();
                        _allNotes.Clear();
                        foreach (var note in sortedNotes)
                        {
                            _allNotes.Add(note);
                        }

                        _notificationService.ShowSuccess("Заметка успешно создана");
                        CloseModal();
                        ApplyFilterAndSearch(); // Обновляем отображение
                    }
                    else
                    {
                        _notificationService.ShowError(createResult.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при сохранении: {ex.Message}");
                _notificationService.ShowError("Ошибка при сохранении заметки");
            }
        }

        private void CancelModalButton_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();
        }

        private void CloseModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            _currentNote = null;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            await LoadNotesAsync();
            _notificationService.ShowInfo("Список заметок обновлен");
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterAndSearch();
        }
    }
}