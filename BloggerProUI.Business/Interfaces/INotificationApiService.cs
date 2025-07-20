using BloggerProUI.Models.Notification;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface INotificationApiService
    {
        Task<IDataResult<List<NotificationDto>>> GetUserNotificationsAsync(bool onlyUnread = false, int page = 1, int pageSize = 20);
        Task<IDataResult<NotificationDto>> GetNotificationByIdAsync(Guid id);
        Task<IDataResult<int>> GetUnreadCountAsync();
        Task<IResult> MarkAsReadAsync(Guid id);
        Task<IResult> MarkAllAsReadAsync();
        Task<IDataResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto notificationDto);
    }
}