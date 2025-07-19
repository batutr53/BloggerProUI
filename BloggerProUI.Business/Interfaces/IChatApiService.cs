using BloggerProUI.Models.Chat;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface IChatApiService
    {
        Task<DataResult<bool>> CanUsersStartConversationAsync(Guid userId1, Guid userId2);
        Task<DataResult<MessageDto>> SendMessageAsync(CreateMessageDto dto);
        Task<DataResult<List<MessageDto>>> GetMessagesAsync(Guid otherUserId, int page = 1, int pageSize = 50);
        Task<DataResult<List<ConversationDto>>> GetConversationsAsync(int page = 1, int pageSize = 20);
        Task<Result> MarkMessagesAsReadAsync(MarkMessagesAsReadDto dto);
        Task<Result> MarkMessagesAsDeliveredAsync(MarkMessagesAsDeliveredDto dto);
        Task<DataResult<int>> GetUnreadMessageCountAsync(Guid senderId);
        Task<DataResult<int>> GetTotalUnreadCountAsync();
    }
}