// Reading Time and Content Analytics
document.addEventListener('DOMContentLoaded', function() {
    calculateReadingTime();
    addReadingProgress();
    highlightCurrentSection();
});

// Calculate reading time and word count
function calculateReadingTime() {
    const articleContent = document.querySelector('.article-text, .article-content');
    const readingTimeDisplay = document.getElementById('reading-time-value');
    const wordCountDisplay = document.getElementById('word-count-value');
    
    if (!articleContent || !readingTimeDisplay || !wordCountDisplay) return;
    
    // Get all text content including modules
    let fullText = articleContent.textContent || articleContent.innerText || '';
    
    // Add content from post modules
    const modules = document.querySelectorAll('.content-with-image, .article-quote, .custom-module');
    modules.forEach(module => {
        const moduleText = module.textContent || module.innerText || '';
        fullText += ' ' + moduleText;
    });
    
    // Clean and count words
    const words = fullText.trim().split(/\s+/).filter(word => word.length > 0);
    const wordCount = words.length;
    
    // Calculate reading time (average 200 words per minute for Turkish)
    const readingTime = Math.ceil(wordCount / 200);
    
    // Update display
    readingTimeDisplay.textContent = readingTime;
    wordCountDisplay.textContent = wordCount;
    
    // Add to page metadata for SEO
    addReadingTimeMetadata(readingTime, wordCount);
}

// Add reading time metadata
function addReadingTimeMetadata(readingTime, wordCount) {
    // Add meta tags for reading time
    const readingTimeMeta = document.createElement('meta');
    readingTimeMeta.name = 'twitter:label1';
    readingTimeMeta.content = 'Okuma Süresi';
    document.head.appendChild(readingTimeMeta);
    
    const readingTimeValue = document.createElement('meta');
    readingTimeValue.name = 'twitter:data1';
    readingTimeValue.content = `${readingTime} dakika`;
    document.head.appendChild(readingTimeValue);
    
    // Add word count meta
    const wordCountMeta = document.createElement('meta');
    wordCountMeta.name = 'twitter:label2';
    wordCountMeta.content = 'Kelime Sayısı';
    document.head.appendChild(wordCountMeta);
    
    const wordCountValue = document.createElement('meta');
    wordCountValue.name = 'twitter:data2';
    wordCountValue.content = `${wordCount} kelime`;
    document.head.appendChild(wordCountValue);
}

// Add reading progress indicator
function addReadingProgress() {
    const progressBar = document.createElement('div');
    progressBar.id = 'reading-progress';
    progressBar.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 0%;
        height: 3px;
        background: linear-gradient(to right, #667eea 0%, #764ba2 100%);
        z-index: 1000;
        transition: width 0.3s ease;
    `;
    
    document.body.appendChild(progressBar);
    
    // Update progress on scroll
    window.addEventListener('scroll', function() {
        const article = document.querySelector('.article-content, .article-container');
        if (!article) return;
        
        const articleTop = article.offsetTop;
        const articleHeight = article.offsetHeight;
        const windowHeight = window.innerHeight;
        const scrollTop = window.pageYOffset;
        
        const articleBottom = articleTop + articleHeight;
        const windowBottom = scrollTop + windowHeight;
        
        if (scrollTop >= articleTop && scrollTop <= articleBottom) {
            const progress = ((scrollTop - articleTop) / (articleHeight - windowHeight)) * 100;
            progressBar.style.width = Math.min(100, Math.max(0, progress)) + '%';
        }
    });
}

// Highlight current section in navigation
function highlightCurrentSection() {
    const headings = document.querySelectorAll('h1, h2, h3, h4, h5, h6');
    if (headings.length === 0) return;
    
    // Table of contents disabled for better user experience
    // if (headings.length > 3) {
    //     createTableOfContents(headings);
    // }
    
    // Add scroll spy functionality
    window.addEventListener('scroll', function() {
        let current = '';
        
        headings.forEach(heading => {
            const rect = heading.getBoundingClientRect();
            if (rect.top <= 100) {
                current = heading.id || heading.textContent.toLowerCase().replace(/\s+/g, '-');
            }
        });
        
        // Update active section in TOC
        const tocLinks = document.querySelectorAll('.toc-link');
        tocLinks.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href') === '#' + current) {
                link.classList.add('active');
            }
        });
    });
}

// Create table of contents
function createTableOfContents(headings) {
    const toc = document.createElement('div');
    toc.className = 'table-of-contents';
    toc.innerHTML = '<h3>İçindekiler</h3>';
    
    const tocList = document.createElement('ul');
    tocList.className = 'toc-list';
    
    headings.forEach((heading, index) => {
        // Generate ID for heading if it doesn't have one
        if (!heading.id) {
            heading.id = 'heading-' + (index + 1);
        }
        
        const listItem = document.createElement('li');
        listItem.className = 'toc-item';
        
        const link = document.createElement('a');
        link.href = '#' + heading.id;
        link.className = 'toc-link';
        link.textContent = heading.textContent;
        link.addEventListener('click', function(e) {
            e.preventDefault();
            heading.scrollIntoView({ behavior: 'smooth' });
        });
        
        listItem.appendChild(link);
        tocList.appendChild(listItem);
    });
    
    toc.appendChild(tocList);
    
    // Add TOC styles
    const tocStyles = document.createElement('style');
    tocStyles.textContent = `
        .table-of-contents {
            background: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 8px;
            padding: 20px;
            margin: 30px 0;
            position: sticky;
            top: 20px;
        }
        
        .table-of-contents h3 {
            margin-top: 0;
            margin-bottom: 15px;
            font-size: 18px;
            color: #333;
        }
        
        .toc-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }
        
        .toc-item {
            margin-bottom: 8px;
        }
        
        .toc-link {
            color: #666;
            text-decoration: none;
            padding: 5px 10px;
            border-radius: 4px;
            display: block;
            transition: all 0.3s ease;
        }
        
        .toc-link:hover,
        .toc-link.active {
            background: #e9ecef;
            color: #333;
            text-decoration: none;
        }
        
        @media (max-width: 768px) {
            .table-of-contents {
                position: static;
                margin: 20px 0;
            }
        }
    `;
    document.head.appendChild(tocStyles);
    
    // Insert TOC after the first paragraph
    const firstParagraph = document.querySelector('.article-text p, .article-content p');
    if (firstParagraph) {
        firstParagraph.parentNode.insertBefore(toc, firstParagraph.nextSibling);
    }
}

// Add estimated reading time to social shares
function enhanceShareData() {
    const shareButtons = document.querySelectorAll('.share-btn');
    const readingTime = document.getElementById('reading-time-value')?.textContent || '0';
    
    shareButtons.forEach(button => {
        const platform = button.dataset.platform;
        if (platform === 'twitter') {
            const originalHref = button.href;
            const readingTimeText = `\n\n📖 ${readingTime} dakika okuma süresi`;
            button.href = originalHref + encodeURIComponent(readingTimeText);
        }
    });
}

// Initialize share data enhancement after reading time calculation
setTimeout(enhanceShareData, 1000);