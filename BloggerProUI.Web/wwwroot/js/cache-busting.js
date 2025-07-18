// Cache Busting Utility for insanlikHallerimiz
// Otomatik CSS ve JS cache temizleme sistemi

class CacheBustingManager {
    constructor() {
        this.storageKey = 'insanlikhallerimiz_asset_versions';
        this.apiEndpoint = '/api/AssetVersion/versions';
        this.init();
    }

    init() {
        // Sayfa yüklendiğinde version kontrolü yap
        this.checkVersions();
        
        // Development modunda dosya değişikliklerini takip et
        if (this.isDevelopment()) {
            this.startVersionPolling();
        }
    }

    isDevelopment() {
        return window.location.hostname === 'localhost' || 
               window.location.hostname === '127.0.0.1' ||
               window.location.hostname.includes('local');
    }

    async checkVersions() {
        try {
            const response = await fetch(this.apiEndpoint);
            const result = await response.json();
            
            if (result.success) {
                const currentVersions = result.data;
                const storedVersions = this.getStoredVersions();
                
                if (this.hasVersionChanged(currentVersions, storedVersions)) {
                    console.log('🔄 Asset versions changed, clearing cache...');
                    await this.clearCache();
                    this.storeVersions(currentVersions);
                    this.showCacheUpdateNotification();
                }
            }
        } catch (error) {
            console.error('Error checking asset versions:', error);
        }
    }

    getStoredVersions() {
        const stored = localStorage.getItem(this.storageKey);
        return stored ? JSON.parse(stored) : null;
    }

    storeVersions(versions) {
        localStorage.setItem(this.storageKey, JSON.stringify(versions));
    }

    hasVersionChanged(current, stored) {
        if (!stored) return false;
        
        return current.cssVersion !== stored.cssVersion || 
               current.jsVersion !== stored.jsVersion;
    }

    async clearCache() {
        // Browser cache temizleme
        if ('caches' in window) {
            const cacheNames = await caches.keys();
            await Promise.all(
                cacheNames.map(cacheName => caches.delete(cacheName))
            );
        }

        // CSS dosyalarını yeniden yükle
        this.reloadStylesheets();
        
        // LocalStorage'da cache'lenmiş verileri temizle
        this.clearLocalStorageCache();
    }

    reloadStylesheets() {
        const stylesheets = document.querySelectorAll('link[rel="stylesheet"]');
        stylesheets.forEach(link => {
            if (link.href.includes('/css/')) {
                const newHref = this.addTimestamp(link.href);
                const newLink = link.cloneNode();
                newLink.href = newHref;
                newLink.onload = () => {
                    link.remove();
                };
                link.parentNode.insertBefore(newLink, link.nextSibling);
            }
        });
    }

    addTimestamp(url) {
        const separator = url.includes('?') ? '&' : '?';
        return `${url}${separator}t=${Date.now()}`;
    }

    clearLocalStorageCache() {
        // Belirli pattern'lere sahip localStorage item'larını temizle
        const keysToRemove = [];
        for (let i = 0; i < localStorage.length; i++) {
            const key = localStorage.key(i);
            if (key && (key.includes('cache') || key.includes('asset'))) {
                keysToRemove.push(key);
            }
        }
        keysToRemove.forEach(key => localStorage.removeItem(key));
    }

    showCacheUpdateNotification() {
        // Kullanıcıya cache güncellendiğini bildiren notification
        const notification = document.createElement('div');
        notification.className = 'cache-update-notification';
        notification.innerHTML = `
            <div class="notification-content">
                <i class="fas fa-sync-alt"></i>
                <span>Sayfanın güncel sürümü yüklendi</span>
                <button class="close-btn">&times;</button>
            </div>
        `;
        
        // Notification stilini ekle
        if (!document.querySelector('#cache-notification-styles')) {
            const style = document.createElement('style');
            style.id = 'cache-notification-styles';
            style.textContent = `
                .cache-update-notification {
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    background: #2d3748;
                    color: white;
                    padding: 12px 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
                    z-index: 10000;
                    animation: slideIn 0.3s ease-out;
                    font-size: 14px;
                    max-width: 300px;
                }
                
                .notification-content {
                    display: flex;
                    align-items: center;
                    gap: 10px;
                }
                
                .notification-content i {
                    color: #48bb78;
                    animation: spin 1s linear;
                }
                
                .close-btn {
                    background: none;
                    border: none;
                    color: white;
                    font-size: 18px;
                    cursor: pointer;
                    margin-left: auto;
                }
                
                @keyframes slideIn {
                    from { transform: translateX(100%); opacity: 0; }
                    to { transform: translateX(0); opacity: 1; }
                }
                
                @keyframes spin {
                    from { transform: rotate(0deg); }
                    to { transform: rotate(360deg); }
                }
            `;
            document.head.appendChild(style);
        }
        
        document.body.appendChild(notification);
        
        // Close button functionality
        const closeBtn = notification.querySelector('.close-btn');
        closeBtn.addEventListener('click', () => {
            notification.remove();
        });
        
        // Auto remove after 5 seconds
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }

    startVersionPolling() {
        // Development modunda her 30 saniyede bir version kontrolü
        setInterval(() => {
            this.checkVersions();
        }, 30000);
    }

    // Manual cache clear function
    async forceClearCache() {
        console.log('🔄 Forcing cache clear...');
        await this.clearCache();
        localStorage.removeItem(this.storageKey);
        this.showCacheUpdateNotification();
        
        // Sayfayı yeniden yükle
        setTimeout(() => {
            window.location.reload();
        }, 1000);
    }
}

// Global instance oluştur
window.cacheBustingManager = new CacheBustingManager();

// Console'dan erişim için global fonksiyon
window.clearAssetCache = () => {
    window.cacheBustingManager.forceClearCache();
};

// Development modunda keyboard shortcut ekle (Ctrl+Shift+R)
document.addEventListener('keydown', (e) => {
    if (e.ctrlKey && e.shiftKey && e.key === 'R') {
        e.preventDefault();
        window.clearAssetCache();
    }
});

console.log('🚀 Cache Busting Manager initialized');
console.log('💡 Use Ctrl+Shift+R to force clear cache or call clearAssetCache() in console');