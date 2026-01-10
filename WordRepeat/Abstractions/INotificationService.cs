using System.Collections.ObjectModel;
using WordRepeat.Services;

namespace WordRepeat.Abstractions
{
    public interface INotificationService
    {
        ObservableCollection<NotificationMessage> Notifications { get; }

        void ShowError(string message, string title = "Ошибка", int duration = 4000);
        void ShowInfo(string message, string title = "Информация", int duration = 4000);
        void ShowSuccess(string message, string title = "Успех", int duration = 4000);
        void ShowWarning(string message, string title = "Предупреждение", int duration = 4000);
    }
}