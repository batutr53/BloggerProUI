// Notification management for real-time notifications
class NotificationManager {
    constructor() {
        this.connection = null;
        this.unreadCount = 0;
        this.init();
    }

    async init() {
        // Initialize SignalR connection
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7028/chathub")
            .build();

        // Listen for new notifications
        this.connection.on("ReceiveNotification", (notification) => {
            this.handleNewNotification(notification);
        });

        try {
            await this.connection.start();
            console.log("Notification SignalR connection started");
            
            // Get initial unread count
            await this.updateUnreadCount();
        } catch (err) {
            console.error("Error starting notification connection:", err);
        }
    }

    handleNewNotification(notification) {
        // Increment unread count
        this.unreadCount++;
        this.updateNotificationBadge();

        // Show toast notification
        this.showToastNotification(notification);

        // Play notification sound (optional)
        this.playNotificationSound();

        // Update notification dropdown if it's open
        this.updateNotificationDropdown(notification);
    }

    showToastNotification(notification) {
        // Create toast notification element
        const toast = document.createElement('div');
        toast.className = 'notification-toast';
        toast.innerHTML = `
            <div class="toast-content">
                <div class="toast-icon">
                    <i class="fas fa-bell"></i>
                </div>
                <div class="toast-message">
                    <strong>Yeni Bildirim</strong>
                    <p>${notification.message}</p>
                </div>
                <button class="toast-close" onclick="this.parentElement.parentElement.remove()">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;

        // Add to page
        document.body.appendChild(toast);

        // Auto remove after 5 seconds
        setTimeout(() => {
            if (toast.parentElement) {
                toast.remove();
            }
        }, 5000);

        // Add click handler to mark as read
        toast.addEventListener('click', () => {
            this.markNotificationAsRead(notification.id);
            toast.remove();
        });
    }

    async updateUnreadCount() {
        try {
            const response = await fetch('/Notification/GetUnreadCount');
            const result = await response.json();
            
            if (result.success) {
                this.unreadCount = result.count;
                this.updateNotificationBadge();
            }
        } catch (error) {
            console.error('Error fetching unread count:', error);
        }
    }

    updateNotificationBadge() {
        const badge = document.querySelector('.notification-badge');
        const bellIcon = document.querySelector('.notification-bell');
        
        if (badge) {
            if (this.unreadCount > 0) {
                badge.textContent = this.unreadCount > 99 ? '99+' : this.unreadCount;
                badge.style.display = 'inline-block';
                
                // Add animation to bell icon
                if (bellIcon) {
                    bellIcon.classList.add('shake');
                    setTimeout(() => bellIcon.classList.remove('shake'), 1000);
                }
            } else {
                badge.style.display = 'none';
            }
        }
    }

    updateNotificationDropdown(notification) {
        const dropdown = document.querySelector('.notification-dropdown');
        if (dropdown && dropdown.style.display !== 'none') {
            // Add new notification to the top of the list
            const notificationItem = this.createNotificationItem(notification);
            const firstItem = dropdown.querySelector('.notification-item');
            
            if (firstItem) {
                dropdown.insertBefore(notificationItem, firstItem);
            } else {
                dropdown.appendChild(notificationItem);
            }
        }
    }

    createNotificationItem(notification) {
        const item = document.createElement('div');
        item.className = 'notification-item unread';
        item.setAttribute('data-id', notification.id);
        
        item.innerHTML = `
            <div class="notification-content">
                <div class="notification-message">${notification.message}</div>
                <div class="notification-meta">
                    <small class="text-muted">Şimdi</small>
                    <button type="button" class="btn btn-sm btn-link p-0 ml-2" onclick="markAsRead('${notification.id}')">
                        <i class="fas fa-check text-success"></i>
                    </button>
                </div>
            </div>
            <div class="notification-indicator">
                <span class="badge badge-primary">Yeni</span>
            </div>
        `;
        
        return item;
    }

    async markNotificationAsRead(notificationId) {
        try {
            const response = await fetch('/Notification/MarkAsRead', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify(notificationId)
            });
            
            const result = await response.json();
            if (result.success) {
                this.unreadCount = Math.max(0, this.unreadCount - 1);
                this.updateNotificationBadge();
            }
        } catch (error) {
            console.error('Error marking notification as read:', error);
        }
    }

    playNotificationSound() {
        try {
            // Create a simple notification sound
            const audioContext = new (window.AudioContext || window.webkitAudioContext)();
            const oscillator = audioContext.createOscillator();
            const gainNode = audioContext.createGain();
            
            oscillator.connect(gainNode);
            gainNode.connect(audioContext.destination);
            
            oscillator.frequency.value = 800;
            oscillator.type = 'sine';
            
            gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
            gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.5);
            
            oscillator.start(audioContext.currentTime);
            oscillator.stop(audioContext.currentTime + 0.5);
        } catch (error) {
            console.error('Error playing notification sound:', error);
        }
    }
}

// CSS for toast notifications
const toastStyles = `
.notification-toast {
    position: fixed;
    top: 20px;
    right: 20px;
    background: white;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    z-index: 9999;
    max-width: 350px;
    animation: slideInRight 0.3s ease-out;
}

.toast-content {
    display: flex;
    align-items: center;
    padding: 15px;
}

.toast-icon {
    color: #4e73df;
    margin-right: 12px;
    font-size: 20px;
}

.toast-message {
    flex: 1;
}

.toast-message strong {
    display: block;
    margin-bottom: 4px;
    color: #2c3e50;
}

.toast-message p {
    margin: 0;
    color: #6c757d;
    font-size: 14px;
}

.toast-close {
    background: none;
    border: none;
    color: #6c757d;
    cursor: pointer;
    padding: 0;
    margin-left: 10px;
}

.toast-close:hover {
    color: #495057;
}

.notification-bell.shake {
    animation: shake 0.5s ease-in-out;
}

@keyframes slideInRight {
    from {
        transform: translateX(100%);
        opacity: 0;
    }
    to {
        transform: translateX(0);
        opacity: 1;
    }
}

@keyframes shake {
    0%, 100% { transform: rotate(0deg); }
    25% { transform: rotate(-10deg); }
    75% { transform: rotate(10deg); }
}

.notification-badge {
    position: absolute;
    top: -8px;
    right: -8px;
    background: #dc3545;
    color: white;
    border-radius: 50%;
    width: 20px;
    height: 20px;
    font-size: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
}
`;

// Add styles to head
const styleSheet = document.createElement('style');
styleSheet.textContent = toastStyles;
document.head.appendChild(styleSheet);

// Initialize notification manager when page loads
document.addEventListener('DOMContentLoaded', function() {
    if (typeof signalR !== 'undefined') {
        window.notificationManager = new NotificationManager();
    }
});

// Global function to mark notifications as read (for compatibility)
window.markAsRead = function(notificationId) {
    if (window.notificationManager) {
        window.notificationManager.markNotificationAsRead(notificationId);
    }
};