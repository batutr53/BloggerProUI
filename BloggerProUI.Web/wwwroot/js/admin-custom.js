// BloggerPro Admin Custom JavaScript

$(document).ready(function() {
    // Initialize DataTables for all admin tables
    if ($.fn.DataTable) {
        $('.admin-datatable').DataTable({
            responsive: true,
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json'
            },
            dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
                 "<'row'<'col-sm-12'tr>>" +
                 "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            pageLength: 25,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tümü"]],
            columnDefs: [
                { 
                    targets: [-1], 
                    orderable: false,
                    className: 'text-center'
                }
            ],
            order: [[0, 'desc']],
            drawCallback: function() {
                // Reinitialize tooltips after table redraw
                $('[data-toggle="tooltip"]').tooltip();
            }
        });
    }

    // Initialize tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Initialize popovers
    $('[data-toggle="popover"]').popover();

    // Sidebar toggle functionality
    $('#sidebarToggle').on('click', function() {
        $('body').toggleClass('sidebar-toggled');
        $('.sidebar').toggleClass('toggled');
    });

    // Auto-hide alerts after 5 seconds
    $('.alert').each(function() {
        const alert = $(this);
        if (!alert.hasClass('alert-permanent')) {
            setTimeout(function() {
                alert.fadeOut('slow');
            }, 5000);
        }
    });

    // Loading button states
    $('[data-loading-text]').on('click', function() {
        const btn = $(this);
        const originalText = btn.text();
        const loadingText = btn.data('loading-text') || 'Yükleniyor...';
        
        btn.prop('disabled', true)
           .html('<span class="loading-spinner"></span> ' + loadingText);
        
        // Re-enable after form submission or 30 seconds
        setTimeout(function() {
            btn.prop('disabled', false).text(originalText);
        }, 30000);
    });

    // Form validation enhancement
    $('form').on('submit', function() {
        const form = $(this);
        const submitBtn = form.find('button[type="submit"]');
        
        if (submitBtn.length) {
            submitBtn.prop('disabled', true)
                    .html('<span class="loading-spinner"></span> İşleniyor...');
        }
    });

    // Image preview functionality
    $('input[type="file"][accept*="image"]').on('change', function() {
        const input = this;
        const preview = $(input).siblings('.image-preview');
        
        if (input.files && input.files[0]) {
            const reader = new FileReader();
            
            reader.onload = function(e) {
                preview.html('<img src="' + e.target.result + '" class="img-fluid rounded">');
            };
            
            reader.readAsDataURL(input.files[0]);
        }
    });

    // Confirm delete functionality
    $('.btn-delete').on('click', function(e) {
        e.preventDefault();
        
        const url = $(this).data('url') || $(this).attr('href');
        const title = $(this).data('title') || 'Bu öğeyi silmek istediğinizden emin misiniz?';
        const text = $(this).data('text') || 'Bu işlem geri alınamaz!';
        
        Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#e74a3b',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Evet, Sil',
            cancelButtonText: 'İptal',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                if ($(this).is('form')) {
                    $(this).submit();
                } else {
                    window.location.href = url;
                }
            }
        });
    });

    // Success/Error message handling
    window.showMessage = function(type, title, message) {
        const iconMap = {
            'success': 'success',
            'error': 'error',
            'warning': 'warning',
            'info': 'info'
        };
        
        Swal.fire({
            icon: iconMap[type] || 'info',
            title: title,
            text: message,
            timer: 3000,
            timerProgressBar: true,
            showConfirmButton: false
        });
    };

    // AJAX form handling
    $('.ajax-form').on('submit', function(e) {
        e.preventDefault();
        
        const form = $(this);
        const url = form.attr('action');
        const method = form.attr('method') || 'POST';
        const data = new FormData(this);
        
        $.ajax({
            url: url,
            type: method,
            data: data,
            processData: false,
            contentType: false,
            beforeSend: function() {
                form.find('button[type="submit"]')
                    .prop('disabled', true)
                    .html('<span class="loading-spinner"></span> İşleniyor...');
            },
            success: function(response) {
                if (response.success) {
                    showMessage('success', 'Başarılı', response.message);
                    if (response.redirectUrl) {
                        setTimeout(() => window.location.href = response.redirectUrl, 1500);
                    }
                } else {
                    showMessage('error', 'Hata', response.message || 'İşlem başarısız');
                }
            },
            error: function(xhr) {
                let message = 'Sunucu hatası oluştu';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    message = xhr.responseJSON.message;
                }
                showMessage('error', 'Hata', message);
            },
            complete: function() {
                form.find('button[type="submit"]')
                    .prop('disabled', false)
                    .html(form.data('original-submit-text') || 'Kaydet');
            }
        });
    });

    // Chart initialization (if Chart.js is loaded)
    if (typeof Chart !== 'undefined') {
        Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
        Chart.defaults.global.defaultFontColor = '#858796';
    }

    // Real-time clock in topbar
    function updateClock() {
        const now = new Date();
        const timeString = now.toLocaleTimeString('tr-TR');
        const dateString = now.toLocaleDateString('tr-TR', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
        
        $('#current-time').text(timeString);
        $('#current-date').text(dateString);
    }
    
    if ($('#current-time').length) {
        updateClock();
        setInterval(updateClock, 1000);
    }

    // Search functionality enhancement
    $('#global-search').on('input', function() {
        const searchTerm = $(this).val().toLowerCase();
        const searchResults = $('#search-results');
        
        if (searchTerm.length >= 2) {
            // Implement global search functionality
            searchResults.show();
            // Add your search implementation here
        } else {
            searchResults.hide();
        }
    });

    // Notification handling
    function checkNotifications() {
        $.get('/Admin/Notifications/GetUnread', function(data) {
            if (data && data.length > 0) {
                $('#notification-count').text(data.length).show();
                
                // Update notification dropdown
                const dropdown = $('#notification-dropdown');
                dropdown.empty();
                
                data.forEach(function(notification) {
                    dropdown.append(`
                        <div class="dropdown-item notification-item" data-id="${notification.id}">
                            <div class="font-weight-bold">${notification.title}</div>
                            <div class="small text-gray-500">${notification.message}</div>
                            <div class="small text-gray-500">${new Date(notification.createdAt).toLocaleString('tr-TR')}</div>
                        </div>
                    `);
                });
            } else {
                $('#notification-count').hide();
            }
        });
    }

    // Check notifications every 30 seconds
    if ($('#notification-count').length) {
        checkNotifications();
        setInterval(checkNotifications, 30000);
    }

    // Keyboard shortcuts
    $(document).on('keydown', function(e) {
        // Ctrl + S to save (prevent default save)
        if (e.ctrlKey && e.which === 83) {
            e.preventDefault();
            $('form:visible button[type="submit"]').first().click();
        }
        
        // Escape to close modals
        if (e.which === 27) {
            $('.modal').modal('hide');
            Swal.close();
        }
    });

    // Smooth scrolling for anchor links
    $('a[href^="#"]').on('click', function(e) {
        e.preventDefault();
        
        const target = $($(this).attr('href'));
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top - 80
            }, 500);
        }
    });

    // Auto-resize textareas
    $('textarea').each(function() {
        this.style.height = 'auto';
        this.style.height = this.scrollHeight + 'px';
    }).on('input', function() {
        this.style.height = 'auto';
        this.style.height = this.scrollHeight + 'px';
    });

    // Initialize Select2 for enhanced dropdowns (if loaded)
    if ($.fn.select2) {
        $('.select2').select2({
            theme: 'bootstrap4',
            width: '100%',
            placeholder: 'Seçiniz...',
            allowClear: true
        });
    }

    // Initialize TinyMCE for rich text editing (if loaded)
    if (typeof tinymce !== 'undefined') {
        tinymce.init({
            selector: '.rich-editor',
            height: 300,
            plugins: 'advlist autolink lists link image charmap print preview hr anchor pagebreak',
            toolbar_mode: 'floating',
            toolbar: 'undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
        });
    }
});

// Global utility functions
window.AdminUtils = {
    formatDate: function(date) {
        return new Date(date).toLocaleDateString('tr-TR');
    },
    
    formatDateTime: function(date) {
        return new Date(date).toLocaleString('tr-TR');
    },
    
    formatNumber: function(num) {
        return new Intl.NumberFormat('tr-TR').format(num);
    },
    
    copyToClipboard: function(text) {
        navigator.clipboard.writeText(text).then(function() {
            showMessage('success', 'Kopyalandı', 'Metin panoya kopyalandı');
        });
    },
    
    downloadFile: function(url, filename) {
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
};