// SignalR Chat Hub Connection
class ChatHubManager {
    constructor() {
        this.connection = null;
        this.isConnected = false;
        this.currentUserId = null;
        this.messageCallbacks = [];
        this.presenceCallbacks = [];
    }

    async connect(accessToken) {
        try {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/chathub", {
                    accessTokenFactory: () => accessToken
                })
                .withAutomaticReconnect()
                .build();

            // Set up event handlers
            this.setupEventHandlers();

            await this.connection.start();
            this.isConnected = true;
            console.log("SignalR Connected.");
            
            return true;
        } catch (err) {
            console.error("SignalR Connection Error: ", err);
            return false;
        }
    }

    setupEventHandlers() {
        // Receive new message
        this.connection.on("ReceiveMessage", (message) => {
            this.messageCallbacks.forEach(callback => callback(message));
        });

        // User connected
        this.connection.on("UserConnected", (userId) => {
            this.presenceCallbacks.forEach(callback => callback(userId, true));
        });

        // User disconnected
        this.connection.on("UserDisconnected", (userId) => {
            this.presenceCallbacks.forEach(callback => callback(userId, false));
        });

        // Messages marked as read
        this.connection.on("MessagesMarkedAsRead", (data) => {
            console.log("Messages marked as read:", data);
            // Update UI to show read status
            this.updateMessageReadStatus(data.MessageIds, data.ReadAt);
        });

        // Messages delivered
        this.connection.on("MessagesMarkedAsDelivered", (data) => {
            console.log("Messages delivered:", data);
            // Update UI to show delivered status
            this.updateMessageDeliveredStatus(data.MessageIds, data.DeliveredAt);
        });

        // User typing
        this.connection.on("UserTyping", (data) => {
            console.log("User typing:", data);
            // Update UI to show typing indicator
            this.showTypingIndicator(data.UserId, data.IsTyping);
        });
    }

    // Subscribe to message events
    onMessage(callback) {
        this.messageCallbacks.push(callback);
    }

    // Subscribe to presence events
    onPresence(callback) {
        this.presenceCallbacks.push(callback);
    }

    // Send message through SignalR
    async sendMessage(receiverId, message) {
        if (this.isConnected) {
            try {
                await this.connection.invoke("SendMessage", receiverId, message);
                return true;
            } catch (err) {
                console.error("Error sending message:", err);
                return false;
            }
        }
        return false;
    }

    // Mark messages as read
    async markMessagesAsRead(senderId, messageIds) {
        if (this.isConnected) {
            try {
                await this.connection.invoke("MarkMessagesAsRead", senderId, messageIds);
                return true;
            } catch (err) {
                console.error("Error marking messages as read:", err);
                return false;
            }
        }
        return false;
    }

    // Send typing indicator
    async sendTypingIndicator(receiverId, isTyping) {
        if (this.isConnected) {
            try {
                await this.connection.invoke("UserTyping", receiverId, isTyping);
                return true;
            } catch (err) {
                console.error("Error sending typing indicator:", err);
                return false;
            }
        }
        return false;
    }

    // Update message read status in UI
    updateMessageReadStatus(messageIds, readAt) {
        messageIds.forEach(messageId => {
            const messageElement = document.querySelector(`[data-message-id="${messageId}"]`);
            if (messageElement) {
                const statusElement = messageElement.querySelector('.message-status');
                if (statusElement) {
                    statusElement.innerHTML = '<i class="fas fa-check-double text-primary"></i>';
                    statusElement.title = `Okundu: ${new Date(readAt).toLocaleString()}`;
                }
            }
        });
    }

    // Update message delivered status in UI
    updateMessageDeliveredStatus(messageIds, deliveredAt) {
        messageIds.forEach(messageId => {
            const messageElement = document.querySelector(`[data-message-id="${messageId}"]`);
            if (messageElement) {
                const statusElement = messageElement.querySelector('.message-status');
                if (statusElement && !statusElement.innerHTML.includes('fa-check-double')) {
                    statusElement.innerHTML = '<i class="fas fa-check text-secondary"></i>';
                    statusElement.title = `Teslim edildi: ${new Date(deliveredAt).toLocaleString()}`;
                }
            }
        });
    }

    // Show typing indicator
    showTypingIndicator(userId, isTyping) {
        const typingIndicator = document.querySelector(`#typing-indicator-${userId}`);
        if (typingIndicator) {
            typingIndicator.style.display = isTyping ? 'block' : 'none';
        }
    }

    // Disconnect
    async disconnect() {
        if (this.connection) {
            await this.connection.stop();
            this.isConnected = false;
            console.log("SignalR Disconnected.");
        }
    }
}

// Global chat hub manager instance
window.chatHub = new ChatHubManager();