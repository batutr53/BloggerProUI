// SEO and Performance Enhancements for insanlikHallerimiz

document.addEventListener('DOMContentLoaded', function() {
    // Lazy loading for images
    initLazyLoading();
    
    // Add structured data for breadcrumbs
    addBreadcrumbStructuredData();
    
    // Add article structured data if on blog detail page
    addArticleStructuredData();
    
    // Optimize external links
    optimizeExternalLinks();
    
    // Add print styles dynamically
    addPrintStyles();
    
    // Initialize image loading optimization
    optimizeImageLoading();
    
    // Add social media meta tags dynamically if needed
    enhanceSocialSharing();
});

// Lazy loading implementation
function initLazyLoading() {
    const lazyImages = document.querySelectorAll('img[loading="lazy"]');
    
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src || img.src;
                    img.classList.add('loaded');
                    observer.unobserve(img);
                }
            });
        });
        
        lazyImages.forEach(img => {
            imageObserver.observe(img);
        });
    } else {
        // Fallback for older browsers
        lazyImages.forEach(img => {
            img.src = img.dataset.src || img.src;
            img.classList.add('loaded');
        });
    }
}

// Add breadcrumb structured data
function addBreadcrumbStructuredData() {
    const breadcrumb = document.querySelector('.breadcrumb');
    if (!breadcrumb) return;
    
    const breadcrumbItems = breadcrumb.querySelectorAll('.breadcrumb-item');
    if (breadcrumbItems.length === 0) return;
    
    const breadcrumbList = {
        "@context": "https://schema.org",
        "@type": "BreadcrumbList",
        "itemListElement": []
    };
    
    breadcrumbItems.forEach((item, index) => {
        const link = item.querySelector('a');
        const text = link ? link.textContent.trim() : item.textContent.trim();
        const url = link ? link.href : window.location.href;
        
        breadcrumbList.itemListElement.push({
            "@type": "ListItem",
            "position": index + 1,
            "name": text,
            "item": url
        });
    });
    
    // Add structured data to head
    const script = document.createElement('script');
    script.type = 'application/ld+json';
    script.textContent = JSON.stringify(breadcrumbList);
    document.head.appendChild(script);
}

// Optimize external links
function optimizeExternalLinks() {
    const externalLinks = document.querySelectorAll('a[href^="http"]:not([href*="' + window.location.host + '"])');
    
    externalLinks.forEach(link => {
        // Add rel attributes for SEO
        link.rel = 'noopener noreferrer';
        
        // Add target blank for external links
        if (!link.target) {
            link.target = '_blank';
        }
        
        // Add title attribute if missing
        if (!link.title && link.textContent.trim()) {
            link.title = `${link.textContent.trim()} - Yeni sekmede açılır`;
        }
    });
}

// Add print styles
function addPrintStyles() {
    const printCSS = `
        @media print {
            .header-nav, .footer, .sidebar, .breadcrumb-nav {
                display: none !important;
            }
            
            .article-container {
                margin: 0 !important;
                padding: 0 !important;
            }
            
            .article-content {
                width: 100% !important;
                margin: 0 !important;
            }
            
            .article-title {
                page-break-after: avoid;
            }
            
            .article-image {
                max-width: 100% !important;
                height: auto !important;
                page-break-inside: avoid;
            }
            
            .article-text {
                font-size: 12pt !important;
                line-height: 1.4 !important;
                color: #000 !important;
            }
            
            .share-section, .post-interaction, .comments-section {
                display: none !important;
            }
            
            .author-box {
                page-break-inside: avoid;
                border: 1px solid #000;
                padding: 10pt;
                margin-top: 20pt;
            }
            
            a[href]:after {
                content: " (" attr(href) ")";
                font-size: 10pt;
                color: #666;
            }
        }
    `;
    
    const style = document.createElement('style');
    style.textContent = printCSS;
    document.head.appendChild(style);
}

// Optimize image loading
function optimizeImageLoading() {
    const images = document.querySelectorAll('img');
    
    images.forEach(img => {
        // Add loading="lazy" if not present
        if (!img.hasAttribute('loading')) {
            img.setAttribute('loading', 'lazy');
        }
        
        // Add decoding="async" for better performance
        if (!img.hasAttribute('decoding')) {
            img.setAttribute('decoding', 'async');
        }
        
        // Add proper alt text if missing
        if (!img.alt && img.title) {
            img.alt = img.title;
        } else if (!img.alt) {
            img.alt = 'insanlikHallerimiz blog görseli';
        }
        
        // Add error handling
        img.onerror = function() {
            this.src = '/images/placeholder.jpg';
            this.alt = 'Görsel yüklenemedi';
        };
    });
}

// Enhance social sharing
function enhanceSocialSharing() {
    const shareButtons = document.querySelectorAll('.share-btn');
    
    shareButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            const platform = this.dataset.platform;
            
            if (platform && platform !== 'copy') {
                // Add click tracking for analytics
                if (typeof gtag !== 'undefined') {
                    gtag('event', 'share', {
                        'method': platform,
                        'content_type': 'article',
                        'item_id': window.location.pathname
                    });
                }
                
                // Add Facebook Pixel tracking if available
                if (typeof fbq !== 'undefined') {
                    fbq('track', 'Share', {
                        'content_type': 'article',
                        'content_id': window.location.pathname
                    });
                }
            }
        });
    });
}

// Add viewport meta tag optimization
function optimizeViewport() {
    const viewport = document.querySelector('meta[name="viewport"]');
    if (viewport) {
        viewport.content = 'width=device-width, initial-scale=1.0, shrink-to-fit=no';
    }
}

// Add performance monitoring
function addPerformanceMonitoring() {
    if ('PerformanceObserver' in window) {
        // Monitor Largest Contentful Paint
        const lcpObserver = new PerformanceObserver((list) => {
            const entries = list.getEntries();
            const lastEntry = entries[entries.length - 1];
            
            if (typeof gtag !== 'undefined') {
                gtag('event', 'timing_complete', {
                    'name': 'lcp',
                    'value': Math.round(lastEntry.startTime)
                });
            }
        });
        
        try {
            lcpObserver.observe({ entryTypes: ['largest-contentful-paint'] });
        } catch (e) {
            console.log('LCP observer not supported');
        }
        
        // Monitor First Input Delay
        const fidObserver = new PerformanceObserver((list) => {
            const entries = list.getEntries();
            entries.forEach(entry => {
                if (typeof gtag !== 'undefined') {
                    gtag('event', 'timing_complete', {
                        'name': 'fid',
                        'value': Math.round(entry.processingStart - entry.startTime)
                    });
                }
            });
        });
        
        try {
            fidObserver.observe({ entryTypes: ['first-input'] });
        } catch (e) {
            console.log('FID observer not supported');
        }
    }
}

// Initialize performance monitoring
addPerformanceMonitoring();

// Add article structured data for blog detail pages
function addArticleStructuredData() {
    // Check if we're on a blog detail page
    const articleTitle = document.querySelector('.article-title');
    const articleMeta = document.querySelector('.article-meta');
    const articleImage = document.querySelector('.article-image');
    
    if (!articleTitle) return;
    
    const title = articleTitle.textContent.trim();
    const imageUrl = articleImage ? articleImage.src : (window.location.origin + '/images/og-default.jpg');
    const currentUrl = window.location.href;
    
    // Extract publish date from article meta
    const publishDateElement = document.querySelector('.article-meta');
    const publishDate = extractDateFromMeta(publishDateElement) || new Date().toISOString();
    
    // Extract author from article-author element
    const authorElement = document.querySelector('.article-author');
    const author = authorElement ? authorElement.textContent.replace('Yazan: ', '').trim() : 'insanlikHallerimiz';
    
    // Extract content for word count and reading time
    const articleContent = document.querySelector('.article-text, .article-content');
    const wordCount = articleContent ? countWords(articleContent.textContent) : 0;
    const readingTime = Math.ceil(wordCount / 200); // 200 words per minute
    
    // Extract stats
    const viewCountElement = document.querySelector('.article-stats span[title*="Görüntülenme"]');
    const likeCountElement = document.querySelector('.article-stats span[title*="Beğeni"]');
    const commentCountElement = document.querySelector('.article-stats span[title*="Yorum"]');
    
    const viewCount = viewCountElement ? parseInt(viewCountElement.textContent.match(/\d+/)?.[0] || '0') : 0;
    const likeCount = likeCountElement ? parseInt(likeCountElement.textContent.match(/\d+/)?.[0] || '0') : 0;
    const commentCount = commentCountElement ? parseInt(commentCountElement.textContent.match(/\d+/)?.[0] || '0') : 0;
    
    // Extract rating if available
    const ratingElement = document.querySelector('.article-stats span[title*="Ortalama"]');
    const rating = ratingElement ? parseFloat(ratingElement.textContent.match(/[\d.]+/)?.[0] || '0') : null;
    
    // Enhanced BlogPosting schema
    const structuredData = {
        "@context": "https://schema.org",
        "@type": "BlogPosting",
        "headline": title,
        "image": [imageUrl],
        "author": {
            "@type": "Person",
            "name": author,
            "url": window.location.origin + "/yazar/" + author.toLowerCase().replace(/\s+/g, '-')
        },
        "publisher": {
            "@type": "Organization",
            "name": "insanlikHallerimiz",
            "logo": {
                "@type": "ImageObject",
                "url": window.location.origin + "/images/logo.png"
            }
        },
        "datePublished": publishDate,
        "dateModified": publishDate,
        "mainEntityOfPage": {
            "@type": "WebPage",
            "@id": currentUrl
        },
        "url": currentUrl,
        "wordCount": wordCount,
        "timeRequired": `PT${readingTime}M`,
        "interactionStatistic": [
            {
                "@type": "InteractionCounter",
                "interactionType": "https://schema.org/ReadAction",
                "userInteractionCount": viewCount
            },
            {
                "@type": "InteractionCounter", 
                "interactionType": "https://schema.org/LikeAction",
                "userInteractionCount": likeCount
            },
            {
                "@type": "InteractionCounter",
                "interactionType": "https://schema.org/CommentAction", 
                "userInteractionCount": commentCount
            }
        ]
    };
    
    // Add rating if available
    if (rating && rating > 0) {
        structuredData.aggregateRating = {
            "@type": "AggregateRating",
            "ratingValue": rating,
            "ratingCount": 1,
            "bestRating": 5,
            "worstRating": 1
        };
    }
    
    // Add FAQ schema if there are comments
    if (commentCount > 0) {
        addFAQSchema();
    }
    
    // Add structured data to head
    const script = document.createElement('script');
    script.type = 'application/ld+json';
    script.textContent = JSON.stringify(structuredData);
    document.head.appendChild(script);
}

// Helper function to extract date from meta element
function extractDateFromMeta(metaElement) {
    if (!metaElement) return null;
    
    const dateText = metaElement.textContent;
    const dateMatch = dateText.match(/(\d{1,2})\s+(\w+)\s+(\d{4})/);
    
    if (dateMatch) {
        const [, day, month, year] = dateMatch;
        const monthNames = {
            'Ocak': '01', 'Şubat': '02', 'Mart': '03', 'Nisan': '04',
            'Mayıs': '05', 'Haziran': '06', 'Temmuz': '07', 'Ağustos': '08',
            'Eylül': '09', 'Ekim': '10', 'Kasım': '11', 'Aralık': '12'
        };
        
        const monthNum = monthNames[month] || '01';
        return `${year}-${monthNum}-${day.padStart(2, '0')}T00:00:00Z`;
    }
    
    return null;
}

// Helper function to count words
function countWords(text) {
    if (!text) return 0;
    return text.trim().split(/\s+/).filter(word => word.length > 0).length;
}

// Add FAQ schema based on comments
function addFAQSchema() {
    const comments = document.querySelectorAll('.comment');
    if (comments.length === 0) return;
    
    const faqItems = [];
    
    comments.forEach((comment, index) => {
        const questionElement = comment.querySelector('.comment-content');
        const answerElement = comment.querySelector('.reply .reply-content');
        
        if (questionElement && answerElement) {
            faqItems.push({
                "@type": "Question",
                "name": questionElement.textContent.trim().substring(0, 100) + "...",
                "acceptedAnswer": {
                    "@type": "Answer",
                    "text": answerElement.textContent.trim()
                }
            });
        }
    });
    
    if (faqItems.length > 0) {
        const faqSchema = {
            "@context": "https://schema.org",
            "@type": "FAQPage",
            "mainEntity": faqItems
        };
        
        const script = document.createElement('script');
        script.type = 'application/ld+json';
        script.textContent = JSON.stringify(faqSchema);
        document.head.appendChild(script);
    }
}

// Add service worker for better caching (optional)
if ('serviceWorker' in navigator) {
    window.addEventListener('load', function() {
        navigator.serviceWorker.register('/sw.js')
            .then(function(registration) {
                console.log('ServiceWorker registration successful');
            })
            .catch(function(error) {
                console.log('ServiceWorker registration failed');
            });
    });
}