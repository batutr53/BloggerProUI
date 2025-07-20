// Q. Studio JavaScript
document.addEventListener('DOMContentLoaded', function() {
    
    // Tab System
    const tabButtons = document.querySelectorAll('.sidebar-menu a');
    const tabContents = document.querySelectorAll('.tab-content');

    tabButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Remove active class from all tabs
            tabButtons.forEach(btn => btn.classList.remove('active'));
            tabContents.forEach(content => content.classList.remove('active'));
            
            // Add active class to clicked tab
            this.classList.add('active');
            
            // Show corresponding content
            const targetTab = this.getAttribute('data-tab');
            if (targetTab) {
                const targetContent = document.getElementById(targetTab);
                if (targetContent) {
                    targetContent.classList.add('active');
                }
            }
        });
    });

    // Profile Form Submission
    const profileForm = document.querySelector('.profile-form');
    if (profileForm) {
        profileForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Show success message
            const saveBtn = this.querySelector('.save-btn');
            const originalText = saveBtn.textContent;
            saveBtn.textContent = 'Kaydedildi!';
            saveBtn.style.background = '#28a745';
            
            setTimeout(() => {
                saveBtn.textContent = originalText;
                saveBtn.style.background = '#333';
            }, 1500);
        });
    }

    // Reading progress animation
    function animateProgressBars() {
        const progressBars = document.querySelectorAll('.progress-bar');
        progressBars.forEach(bar => {
            const width = bar.getAttribute('data-width') || '0';
            bar.style.width = width + '%';
        });
    }

    // Trigger progress animation when reading tab is opened
    const readingTab = document.querySelector('[data-tab="reading"]');
    if (readingTab) {
        readingTab.addEventListener('click', function() {
            setTimeout(animateProgressBars, 300);
        });
    }

    // Stats counter animation
    function animateCounters() {
        const counters = document.querySelectorAll('.stat-number');
        counters.forEach(counter => {
            const target = parseInt(counter.getAttribute('data-target')) || 0;
            const duration = 2000;
            const step = target / (duration / 16);
            let current = 0;
            
            const timer = setInterval(() => {
                current += step;
                if (current >= target) {
                    counter.textContent = target;
                    clearInterval(timer);
                } else {
                    counter.textContent = Math.floor(current);
                }
            }, 16);
        });
    }

    // Trigger counter animation on page load
    setTimeout(animateCounters, 500);

    // Activity items hover effect
    document.querySelectorAll('.activity-item').forEach(item => {
        item.addEventListener('mouseenter', function() {
            this.style.background = '#f0f0f0';
        });
        
        item.addEventListener('mouseleave', function() {
            this.style.background = 'transparent';
        });
    });

    // Book items interaction
    document.querySelectorAll('.book-item-small').forEach(book => {
        book.addEventListener('click', function() {
            const title = this.querySelector('.book-title-small').textContent;
            console.log('Kitap seçildi:', title);
        });
    });

    // Bookmark items interaction
    document.querySelectorAll('.bookmark-item').forEach(bookmark => {
        bookmark.addEventListener('click', function(e) {
            e.preventDefault();
            const url = this.getAttribute('href');
            if (url) {
                window.open(url, '_blank');
            }
        });
    });

    // Real-time notifications (simulated)
    function addNotification() {
        const notifications = [
            'Yeni bir yorum aldınız',
            'Profiliniz güncellendi',
            'Yeni bir takipçiniz var',
            'Haftalık rapor hazır'
        ];
        
        const randomNotification = notifications[Math.floor(Math.random() * notifications.length)];
        console.log('Bildirim:', randomNotification);
    }

    // Add random notification every 30 seconds (demo)
    setInterval(addNotification, 30000);

    // Keyboard shortcuts
    document.addEventListener('keydown', function(e) {
        if (e.altKey) {
            switch(e.key) {
                case '1':
                    e.preventDefault();
                    const dashboardTab = document.querySelector('[data-tab="dashboard"]');
                    if (dashboardTab) dashboardTab.click();
                    break;
                case '2':
                    e.preventDefault();
                    const readingTabShortcut = document.querySelector('[data-tab="reading"]');
                    if (readingTabShortcut) readingTabShortcut.click();
                    break;
                case '3':
                    e.preventDefault();
                    const bookmarksTab = document.querySelector('[data-tab="bookmarks"]');
                    if (bookmarksTab) bookmarksTab.click();
                    break;
            }
        }
    });

    // Theme toggle (future feature)
    function toggleTheme() {
        const body = document.body;
        const isDark = body.classList.contains('dark-theme');
        
        if (isDark) {
            body.classList.remove('dark-theme');
            localStorage.setItem('theme', 'light');
        } else {
            body.classList.add('dark-theme');
            localStorage.setItem('theme', 'dark');
        }
    }

    // Load saved theme
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
        document.body.classList.add('dark-theme');
    }

    // Auto-save form data
    const formInputs = document.querySelectorAll('.form-input');
    formInputs.forEach(input => {
        input.addEventListener('input', function() {
            localStorage.setItem(this.name, this.value);
        });
    });

    // Load saved form data
    formInputs.forEach(input => {
        const savedValue = localStorage.getItem(input.name);
        if (savedValue) {
            input.value = savedValue;
        }
    });

    // Responsive sidebar toggle
    function createMobileToggle() {
        if (window.innerWidth <= 768) {
            let mobileToggle = document.querySelector('.mobile-toggle');
            if (!mobileToggle) {
                mobileToggle = document.createElement('button');
                mobileToggle.className = 'mobile-toggle';
                mobileToggle.innerHTML = '<i class="fas fa-bars"></i>';
                mobileToggle.style.cssText = `
                    display: block;
                    position: fixed;
                    top: 20px;
                    left: 20px;
                    z-index: 1001;
                    background: #333;
                    color: #fff;
                    border: none;
                    padding: 10px;
                    border-radius: 4px;
                    cursor: pointer;
                `;
                
                document.body.appendChild(mobileToggle);
                
                mobileToggle.addEventListener('click', function() {
                    const sidebar = document.querySelector('.sidebar');
                    if (sidebar) {
                        const isVisible = sidebar.style.display === 'block';
                        sidebar.style.display = isVisible ? 'none' : 'block';
                        sidebar.style.position = 'fixed';
                        sidebar.style.top = '60px';
                        sidebar.style.left = '20px';
                        sidebar.style.zIndex = '1000';
                        sidebar.style.width = '250px';
                    }
                });
            }
        } else {
            const mobileToggle = document.querySelector('.mobile-toggle');
            if (mobileToggle) {
                mobileToggle.remove();
            }
            const sidebar = document.querySelector('.sidebar');
            if (sidebar) {
                sidebar.style.display = '';
                sidebar.style.position = '';
                sidebar.style.top = '';
                sidebar.style.left = '';
                sidebar.style.zIndex = '';
                sidebar.style.width = '';
            }
        }
    }

    // Call mobile toggle on resize
    window.addEventListener('resize', createMobileToggle);
    createMobileToggle();

    // Home page interactions
    document.querySelectorAll('.trending-item, .article-item, .podcast-item').forEach(item => {
        item.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.transition = 'transform 0.3s ease';
        });
        
        item.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });

    // Newsletter subscription
    const newsletterBtn = document.querySelector('.newsletter-input button');
    if (newsletterBtn) {
        newsletterBtn.addEventListener('click', function() {
            const email = this.previousElementSibling.value;
            if (email) {
                alert('E-bülten aboneliğiniz başarıyla tamamlandı!');
                this.previousElementSibling.value = '';
            }
        });
    }

    // Book purchase buttons
    document.querySelectorAll('.book-price').forEach(button => {
        button.addEventListener('click', function() {
            const bookTitle = this.closest('.book-item').querySelector('.book-title').textContent;
            alert(`"${bookTitle}" kitabı sepete eklendi!`);
        });
    });

    // Play button for podcasts
    document.querySelectorAll('.play-button').forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            const podcastTitle = this.closest('.podcast-item').querySelector('.podcast-title').textContent;
            alert(`"${podcastTitle}" podcast'i oynatılıyor...`);
        });
    });

    // Blog detail page interactions
    // Share buttons functionality
    document.querySelectorAll('.share-btn').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            const platform = this.getAttribute('data-platform');
            const url = window.location.href;
            const title = document.querySelector('.article-title')?.textContent || '';
            
            let shareUrl = '';
            switch(platform) {
                case 'twitter':
                    shareUrl = `https://twitter.com/intent/tweet?url=${encodeURIComponent(url)}&text=${encodeURIComponent(title)}`;
                    break;
                case 'facebook':
                    shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}`;
                    break;
                case 'linkedin':
                    shareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(url)}`;
                    break;
                default:
                    shareUrl = url;
            }
            
            if (shareUrl) {
                window.open(shareUrl, '_blank', 'width=600,height=400');
            }
        });
    });

    // Newsletter subscription for sidebar
    const newsletterSidebarBtn = document.querySelector('.newsletter-sidebar-btn');
    if (newsletterSidebarBtn) {
        newsletterSidebarBtn.addEventListener('click', function() {
            const email = this.previousElementSibling.querySelector('input').value;
            if (email) {
                alert('E-bülten aboneliğiniz başarıyla tamamlandı!');
                this.previousElementSibling.querySelector('input').value = '';
            }
        });
    }

    // Tag clicks
    document.querySelectorAll('.tag').forEach(tag => {
        tag.addEventListener('click', function(e) {
            e.preventDefault();
            const tagName = this.textContent;
            console.log('Tag tıklandı:', tagName);
        });
    });

    // Related article hover effects
    document.querySelectorAll('.related-article').forEach(article => {
        article.addEventListener('mouseenter', function() {
            this.style.backgroundColor = '#f8f8f8';
            this.style.transition = 'background-color 0.3s ease';
        });
        
        article.addEventListener('mouseleave', function() {
            this.style.backgroundColor = 'transparent';
        });
    });

    // Reading progress indicator
    const progressBar = document.createElement('div');
    progressBar.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 0%;
        height: 3px;
        background: #333;
        z-index: 9999;
        transition: width 0.3s ease;
    `;
    document.body.appendChild(progressBar);

    window.addEventListener('scroll', function() {
        const winScroll = document.body.scrollTop || document.documentElement.scrollTop;
        const height = document.documentElement.scrollHeight - document.documentElement.clientHeight;
        const scrolled = (winScroll / height) * 100;
        progressBar.style.width = Math.min(scrolled, 100) + '%';
    });

    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const href = this.getAttribute('href');
            if (href && href.length > 1) {
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });

    // Image lazy loading
    const lazyLoadImages = document.querySelectorAll('img[data-src]');
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });

    lazyLoadImages.forEach(img => imageObserver.observe(img));

    // Initialize animations
    const animatedElements = document.querySelectorAll('.fade-in');
    const animationObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.animationPlayState = 'running';
            }
        });
    });

    animatedElements.forEach(el => {
        el.style.animationPlayState = 'paused';
        animationObserver.observe(el);
    });

    // Enhanced Navigation and Smooth Scrolling - handled by earlier anchor link code

    // Newsletter subscription with validation
    const newsletterForm = document.querySelector('.newsletter-input');
    if (newsletterForm) {
        const emailInput = newsletterForm.querySelector('input[type="email"]');
        const submitBtn = newsletterForm.querySelector('button');
        
        if (submitBtn) {
            submitBtn.addEventListener('click', function(e) {
                e.preventDefault();
                
                const email = emailInput.value.trim();
                if (!email) {
                    showNotification('Lütfen e-posta adresinizi girin.', 'error');
                    return;
                }
                
                if (!isValidEmail(email)) {
                    showNotification('Lütfen geçerli bir e-posta adresi girin.', 'error');
                    return;
                }
                
                // Simulate subscription
                submitBtn.textContent = 'Kaydediliyor...';
                submitBtn.disabled = true;
                
                setTimeout(() => {
                    showNotification('Başarıyla abone oldunuz!', 'success');
                    emailInput.value = '';
                    submitBtn.textContent = 'Abone Ol';
                    submitBtn.disabled = false;
                }, 1500);
            });
        }
    }

    // Contact form handling
    const contactForm = document.querySelector('.profile-form');
    if (contactForm) {
        contactForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const name = this.querySelector('input[type="text"]').value.trim();
            const email = this.querySelector('input[type="email"]').value.trim();
            const subject = this.querySelectorAll('input[type="text"]')[1].value.trim();
            const message = this.querySelector('textarea').value.trim();
            
            // Validation
            if (!name || !email || !subject || !message) {
                showNotification('Lütfen tüm alanları doldurun.', 'error');
                return;
            }
            
            if (!isValidEmail(email)) {
                showNotification('Lütfen geçerli bir e-posta adresi girin.', 'error');
                return;
            }
            
            // Simulate form submission
            const submitBtn = this.querySelector('.save-btn');
            const originalText = submitBtn.textContent;
            submitBtn.textContent = 'Gönderiliyor...';
            submitBtn.disabled = true;
            
            setTimeout(() => {
                showNotification('Mesajınız başarıyla gönderildi! En kısa sürede size dönüş yapacağız.', 'success');
                this.reset();
                submitBtn.textContent = originalText;
                submitBtn.disabled = false;
            }, 2000);
        });
    }

    // Social media hover effects
    document.querySelectorAll('a[href="#"]').forEach(link => {
        if (link.innerHTML.includes('fab')) {
            link.addEventListener('mouseenter', function() {
                this.style.color = '#333';
            });
            
            link.addEventListener('mouseleave', function() {
                this.style.color = '#666';
            });
        }
    });

    // Search input event listener
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', performSearch);
        searchInput.addEventListener('keydown', function(event) {
            if (event.key === 'Escape') {
                hideSearchBox();
            }
        });
    }

    // Handle image loading errors
    const images = document.querySelectorAll('img');
    
    images.forEach(img => {
        // Set loading placeholder
        img.addEventListener('loadstart', function() {
            this.style.opacity = '0.5';
        });
        
        // Handle successful load
        img.addEventListener('load', function() {
            this.style.opacity = '1';
            this.classList.add('loaded');
        });
        
        // Handle loading errors
        img.addEventListener('error', function() {
            console.log('Image failed to load:', this.src);
            
            // Set fallback image based on context
            if (this.closest('.podcast-image')) {
                this.src = 'https://images.unsplash.com/photo-1513475382585-d06e58bcb0e0?ixlib=rb-4.0.3&auto=format&fit=crop&w=100&q=80';
            } else if (this.closest('.article-item')) {
                this.src = 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=387&q=80';
            } else if (this.closest('.trending-item')) {
                this.src = 'https://images.unsplash.com/photo-1502920917128-1aa500764cbd?ixlib=rb-4.0.3&auto=format&fit=crop&w=435&q=80';
            } else {
                this.src = 'https://images.unsplash.com/photo-1513475382585-d06e58bcb0e0?ixlib=rb-4.0.3&auto=format&fit=crop&w=435&q=80';
            }
            
            this.style.opacity = '1';
            this.classList.add('error-fallback');
        });
        
        // Check if image is already loaded (cached)
        if (img.complete && img.naturalHeight !== 0) {
            img.style.opacity = '1';
            img.classList.add('loaded');
        }
    });
    
    // Lazy loading for images
    const lazyImages = document.querySelectorAll('img[loading="lazy"]');
    
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.style.opacity = '0.5';
                    
                    // Preload image
                    const tempImg = new Image();
                    tempImg.onload = function() {
                        img.style.opacity = '1';
                        img.classList.add('loaded');
                    };
                    tempImg.onerror = function() {
                        img.style.opacity = '1';
                        img.classList.add('error-fallback');
                    };
                    tempImg.src = img.src;
                    
                    observer.unobserve(img);
                }
            });
        });
        
        lazyImages.forEach(img => imageObserver.observe(img));
    }

    // Fix for disappearing images in recent comments
    function fixRecentCommentsImages() {
        const commentImages = document.querySelectorAll('.podcast-image img');
        
        commentImages.forEach(img => {
            // Force reload if image is not visible
            if (img.offsetWidth === 0 || img.offsetHeight === 0) {
                const originalSrc = img.src;
                img.src = '';
                setTimeout(() => {
                    img.src = originalSrc;
                }, 100);
            }
            
            // Add error handling
            img.onerror = function() {
                this.src = 'https://images.unsplash.com/photo-1513475382585-d06e58bcb0e0?ixlib=rb-4.0.3&auto=format&fit=crop&w=100&q=80';
            };
        });
    }

    // Run fix on page load and after dynamic content updates
    fixRecentCommentsImages();
    setTimeout(fixRecentCommentsImages, 1000); // Run again after 1 second

});

// Utility functions
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.textContent = message;
    
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 15px 20px;
        background: ${type === 'success' ? '#28a745' : type === 'error' ? '#dc3545' : '#007bff'};
        color: white;
        border-radius: 5px;
        z-index: 10000;
        animation: slideInRight 0.3s ease;
    `;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.3s ease';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

function formatDate(date) {
    return new Intl.DateTimeFormat('tr-TR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    }).format(date);
}

function truncateText(text, maxLength) {
    if (text.length <= maxLength) return text;
    return text.substring(0, maxLength) + '...';
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Mobile Menu Toggle Function
function toggleMobileMenu() {
    const mainNav = document.getElementById('mainNav');
    if (mainNav) {
        mainNav.classList.toggle('show');
    }
}

// Search functionality
(function() {
    let searchTimeout;

    window.showSearchBox = function() {
    const dropdown = document.getElementById('searchDropdown');
    const input = document.getElementById('searchInput');
    
    if (dropdown && input) {
        dropdown.classList.add('show');
        input.focus();
        input.value = '';
        const results = document.getElementById('searchResults');
        if (results) {
            results.innerHTML = '';
        }
    }
}

    window.hideSearchBox = function() {
        const dropdown = document.getElementById('searchDropdown');
        const input = document.getElementById('searchInput');
        
        if (dropdown && input) {
            dropdown.classList.remove('show');
            input.value = '';
        }
    };

    window.performSearch = function() {
    const input = document.getElementById('searchInput');
    if (!input) return;
    
    const keyword = input.value.trim();
    
    if (!keyword) {
        const results = document.getElementById('searchResults');
        if (results) {
            results.innerHTML = '';
        }
        return;
    }
    
    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(() => {
        searchPosts(keyword);
    }, 300);
    };

    async function searchPosts(keyword) {
    const results = document.getElementById('searchResults');
    
    if (!results) return;
    
    results.innerHTML = '<div class="search-loading"><i class="fas fa-spinner"></i> Aranıyor...</div>';
    
    try {
        // UI Controller'ındaki Search action'ına istek at
        const response = await fetch(`/Home/Search?keyword=${encodeURIComponent(keyword)}&pageSize=8`);
        
        if (!response.ok) {
            throw new Error('Arama başarısız');
        }
        
        const result = await response.json();
        
        if (result.success && result.data && result.data.length > 0) {
            displaySearchResults(result.data);
        } else {
            results.innerHTML = '<div class="search-no-results">Sonuç bulunamadı</div>';
        }
    } catch (error) {
        console.error('Search error:', error);
        results.innerHTML = '<div class="search-no-results">Arama hatası oluştu</div>';
    }
    }

    function displaySearchResults(posts) {
    const results = document.getElementById('searchResults');
    if (!results) return;
    
    let html = '';
    
    posts.forEach(post => {
        const excerpt = post.excerpt || '';
        const createdAt = new Date(post.createdAt).toLocaleDateString('tr-TR');
        
        html += `
            <a href="/Blog/Detail/${post.id}" class="search-result-item" onclick="hideSearchBox()">
                <div class="search-result-title">${escapeHtml(post.title)}</div>
                <div class="search-result-excerpt">${escapeHtml(excerpt)}</div>
                <div class="search-result-meta">
                    ${escapeHtml(post.author?.userName || 'Yazar')} • ${createdAt}
                </div>
            </a>
        `;
    });
    
    results.innerHTML = html;
    }
})();

function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Search Event Listeners - moved to main DOMContentLoaded event

// Close search when clicking outside
document.addEventListener('click', function(event) {
    const searchToggle = document.querySelector('.search-toggle');
    if (searchToggle && !searchToggle.contains(event.target)) {
        hideSearchBox();
    }
});

// Dynamic header update removed - cache sorunu çözüldü

// Image Error Handling - code moved to main DOMContentLoaded event