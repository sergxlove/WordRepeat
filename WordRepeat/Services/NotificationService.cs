using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WordRepeat.Abstractions;

namespace WordRepeat.Services
{
    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public class NotificationMessage
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public int Duration { get; set; } = 4000;
    }

    public class NotificationService : INotificationService
    {
        private Panel _notificationContainer;
        private ObservableCollection<NotificationMessage> _notifications = new();

        public ObservableCollection<NotificationMessage> Notifications
        {
            get
            {
                return _notifications;
            }
        }

        public NotificationService(Panel container)
        {
            _notificationContainer = container;
        }

        private void Show(string message, string title = "", NotificationType type = NotificationType.Info, int duration = 4000)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                NotificationMessage notification = new NotificationMessage
                {
                    Title = string.IsNullOrEmpty(title) ? GetDefaultTitle(type) : title,
                    Message = message,
                    Type = type,
                    Duration = duration
                };

                _notifications.Add(notification);
                CreateToastNotification(notification);
            });
        }

        public void ShowSuccess(string message, string title = "Успех", int duration = 4000)
        {
            Show(message, title, NotificationType.Success, duration);
        }

        public void ShowError(string message, string title = "Ошибка", int duration = 4000)
        {
            Show(message, title, NotificationType.Error, duration);
        }

        public void ShowWarning(string message, string title = "Предупреждение", int duration = 4000)
        {
            Show(message, title, NotificationType.Warning, duration);
        }

        public void ShowInfo(string message, string title = "Информация", int duration = 4000)
        {
            Show(message, title, NotificationType.Info, duration);
        }

        private string GetDefaultTitle(NotificationType type)
        {
            return type switch
            {
                NotificationType.Success => "Успех",
                NotificationType.Error => "Ошибка",
                NotificationType.Warning => "Предупреждение",
                NotificationType.Info => "Информация",
                _ => "Уведомление"
            };
        }

        private void CreateToastNotification(NotificationMessage notification)
        {
            if (_notificationContainer == null)
                return;

            // Создаем контрол уведомления
            Border border = new Border
            {
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(8),
                Background = GetBackgroundBrush(notification.Type),
                BorderBrush = GetBorderBrush(notification.Type),
                BorderThickness = new Thickness(1),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 2,
                    Opacity = 0.3
                },
                Opacity = 0,
                MaxWidth = 400
            };

            Grid mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3) }); // Полоска прогресса

            // Верхняя часть с контентом
            StackPanel contentPanel = new StackPanel
            {
                Margin = new Thickness(15)
            };

            // Заголовок
            if (!string.IsNullOrEmpty(notification.Title))
            {
                var titleText = new TextBlock
                {
                    Text = notification.Title,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                contentPanel.Children.Add(titleText);
            }

            // Сообщение
            var messageText = new TextBlock
            {
                Text = notification.Message,
                Foreground = Brushes.White,
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap
            };
            contentPanel.Children.Add(messageText);

            Grid.SetRow(contentPanel, 0);
            mainGrid.Children.Add(contentPanel);

            // Полоска обратного отсчета
            Border progressBorder = new Border
            {
                Height = 3,
                CornerRadius = new CornerRadius(0, 0, 8, 8),
                Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            Rectangle progressBar = new Rectangle
            {
                Height = 3,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = GetProgressBarColor(notification.Type),
                RadiusX = 0,
                RadiusY = 0
            };

            progressBorder.Child = progressBar;
            Grid.SetRow(progressBorder, 1);
            mainGrid.Children.Add(progressBorder);

            border.Child = mainGrid;

            // Кнопка закрытия
            var closeButton = new Button
            {
                Content = "×",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Width = 25,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, -10, -10, 0),
                Padding = new Thickness(0)
            };

            closeButton.Click += (s, e) =>
            {
                RemoveNotification(border, progressBar);
            };

            var outerGrid = new Grid();
            outerGrid.Children.Add(border);
            outerGrid.Children.Add(closeButton);

            // Добавляем в контейнер
            _notificationContainer.Children.Add(outerGrid);

            // Анимация появления
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            border.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            // Анимация полоски прогресса
            DoubleAnimation progressAnimation = new DoubleAnimation
            {
                From = border.ActualWidth > 0 ? border.ActualWidth : 400, // Если ширина еще не определена, используем примерную
                To = 0,
                Duration = TimeSpan.FromMilliseconds(notification.Duration),
                FillBehavior = FillBehavior.HoldEnd
            };

            // Подписываемся на событие загрузки, чтобы получить реальную ширину
            border.Loaded += (s, e) =>
            {
                double actualWidth = border.ActualWidth;
                if (actualWidth > 0)
                {
                    // Пересоздаем анимацию с правильной шириной
                    progressAnimation.From = actualWidth;
                    progressBar.Width = actualWidth;
                    progressBar.BeginAnimation(Rectangle.WidthProperty, progressAnimation);
                }
            };

            // Старт анимации
            progressBar.BeginAnimation(Rectangle.WidthProperty, progressAnimation);

            // Автоматическое закрытие
            Task.Delay(notification.Duration).ContinueWith(_ =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    RemoveNotification(border, progressBar);
                });
            });
        }

        private void RemoveNotification(Border border, Rectangle progressBar)
        {
            // Останавливаем анимацию прогресса
            if (progressBar != null)
            {
                progressBar.BeginAnimation(Rectangle.WidthProperty, null);
            }

            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            fadeOut.Completed += (s, e) =>
            {
                var parent = border.Parent as Grid;
                if (parent != null && _notificationContainer != null)
                {
                    _notificationContainer.Children.Remove(parent);
                }
            };

            border.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private Brush GetBackgroundBrush(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Success:
                    return new SolidColorBrush(Color.FromArgb(220, 76, 175, 80));
                case NotificationType.Error:
                    return new SolidColorBrush(Color.FromArgb(220, 211, 47, 47));
                case NotificationType.Warning:
                    return new SolidColorBrush(Color.FromArgb(220, 255, 152, 0));
                case NotificationType.Info:
                    return new SolidColorBrush(Color.FromArgb(220, 33, 150, 243));
                default:
                    return new SolidColorBrush(Color.FromArgb(220, 97, 97, 97));
            }
        }

        private Brush GetBorderBrush(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Success:
                    return new SolidColorBrush(Color.FromArgb(255, 56, 142, 60));
                case NotificationType.Error:
                    return new SolidColorBrush(Color.FromArgb(255, 183, 28, 28));
                case NotificationType.Warning:
                    return new SolidColorBrush(Color.FromArgb(255, 245, 124, 0));
                case NotificationType.Info:
                    return new SolidColorBrush(Color.FromArgb(255, 21, 101, 192));
                default:
                    return new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
            }
        }

        private Brush GetProgressBarColor(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Success:
                    return new SolidColorBrush(Color.FromArgb(255, 46, 125, 50));
                case NotificationType.Error:
                    return new SolidColorBrush(Color.FromArgb(255, 213, 0, 0));
                case NotificationType.Warning:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 111, 0));
                case NotificationType.Info:
                    return new SolidColorBrush(Color.FromArgb(255, 13, 71, 161));
                default:
                    return Brushes.White;
            }
        }
    }
}