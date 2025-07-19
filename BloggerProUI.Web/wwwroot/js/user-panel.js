// Modern User Dropdown Functions
function toggleUserDropdown() {
    const trigger = document.querySelector('.user-profile-trigger');
    const menu = document.getElementById('userDropdownMenu');
    
    if (menu && trigger) {
        const isOpen = menu.classList.contains('show');
        
        if (isOpen) {
            menu.classList.remove('show');
            trigger.classList.remove('active');
        } else {
            menu.classList.add('show');
            trigger.classList.add('active');
        }
    }
}

// Blog Theme Logout Confirmation
function confirmLogout() {
    // Blog-themed confirmation modal
    const modal = document.createElement('div');
    modal.className = 'blog-modal-overlay';
    modal.innerHTML = `
        <div class="blog-modal">
            <div class="blog-modal-header">
                <h3>Güvenli Çıkış</h3>
                <button class="blog-modal-close" onclick="closeLogoutModal()">
                    <i class="fas fa-times"></i>
                </button>
            </div>
            <div class="blog-modal-body">
                <div class="logout-icon">
                    <i class="fas fa-sign-out-alt"></i>
                </div>
                <p>Oturumunuzu sonlandırmak istediğinizden emin misiniz?</p>
                <p class="text-muted">Bu işlem sizi giriş sayfasına yönlendirecektir.</p>
            </div>
            <div class="blog-modal-footer">
                <button class="blog-btn blog-btn-secondary" onclick="closeLogoutModal()">
                    İptal
                </button>
                <button class="blog-btn blog-btn-primary" onclick="performLogout()">
                    Güvenli Çıkış
                </button>
            </div>
        </div>
    `;
    
    document.body.appendChild(modal);
    
    // Prevent body scroll
    document.body.style.overflow = 'hidden';
    
    // Animation
    setTimeout(() => {
        modal.classList.add('show');
    }, 10);
}

function closeLogoutModal() {
    const modal = document.querySelector('.blog-modal-overlay');
    if (modal) {
        modal.classList.remove('show');
        // Restore body scroll
        document.body.style.overflow = '';
        setTimeout(() => {
            modal.remove();
        }, 300);
    }
}

function performLogout() {
    // Loading state
    const confirmBtn = document.querySelector('.blog-btn-primary');
    if (confirmBtn) {
        confirmBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Çıkış yapılıyor...';
        confirmBtn.disabled = true;
        confirmBtn.classList.add('loading');
    }
    
    // Redirect to logout
    setTimeout(() => {
        window.location.href = '/Auth/Logout';
    }, 1000);
}

// Close dropdown when clicking outside
document.addEventListener('click', function(event) {
    const dropdown = document.querySelector('.user-profile-dropdown');
    const menu = document.getElementById('userDropdownMenu');
    const trigger = document.querySelector('.user-profile-trigger');
    
    if (dropdown && menu && trigger && !dropdown.contains(event.target)) {
        menu.classList.remove('show');
        trigger.classList.remove('active');
    }
});

// Close dropdown on escape key
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        const menu = document.getElementById('userDropdownMenu');
        const trigger = document.querySelector('.user-profile-trigger');
        
        if (menu && trigger) {
            menu.classList.remove('show');
            trigger.classList.remove('active');
        }
        
        // Also close logout modal
        closeLogoutModal();
    }
});

// Profile Management Functionality
document.addEventListener('DOMContentLoaded', function() {
    // Character counter for bio
    const bioTextarea = document.getElementById('bio');
    const bioCharCount = document.getElementById('bioCharCount');
    
    if (bioTextarea && bioCharCount) {
        bioTextarea.addEventListener('input', function() {
            const currentLength = this.value.length;
            bioCharCount.textContent = currentLength;
            
            const counter = document.querySelector('.char-counter');
            counter.classList.remove('warning', 'error');
            
            if (currentLength > 450) {
                counter.classList.add('warning');
            }
            if (currentLength > 500) {
                counter.classList.add('error');
                this.value = this.value.substring(0, 500);
                bioCharCount.textContent = 500;
            }
        });
    }
    
    // Profile image preview
    const profileImageInput = document.getElementById('profileImageInput');
    const profilePreview = document.getElementById('profilePreview');
    
    if (profileImageInput && profilePreview) {
        profileImageInput.addEventListener('change', function(e) {
            const file = e.target.files[0];
            if (file) {
                // Validate file size (5MB max)
                if (file.size > 5 * 1024 * 1024) {
                    showNotification('Dosya boyutu 5MB\'dan küçük olmalıdır.', 'error');
                    this.value = '';
                    return;
                }
                
                // Validate file type
                if (!file.type.match(/^image\/(jpeg|jpg|png|webp)$/)) {
                    showNotification('Sadece JPG, PNG ve WebP formatları desteklenir.', 'error');
                    this.value = '';
                    return;
                }
                
                // Preview image
                const reader = new FileReader();
                reader.onload = function(e) {
                    profilePreview.src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }
    
    // Load current user profile data
    loadCurrentProfile();
    
    // Profile form submission
    const profileForm = document.getElementById('profileUpdateForm');
    if (profileForm) {
        profileForm.addEventListener('submit', handleProfileSubmit);
    }
    
    // Cancel button
    const cancelBtn = document.getElementById('cancelBtn');
    if (cancelBtn) {
        cancelBtn.addEventListener('click', function() {
            if (confirm('Değişiklikleriniz kaydedilmeyecek. Devam etmek istediğinizden emin misiniz?')) {
                loadCurrentProfile();
            }
        });
    }
});

// Load current user profile
async function loadCurrentProfile() {
    try {
        showProfileLoading(true);
        
        const result = await window.profileApiService.getCurrentProfile();
        
        if (result.success && result.data) {
            populateProfileForm(result.data);
        } else {
            showNotification(result.message || 'Profil bilgileri yüklenemedi.', 'error');
        }
        
    } catch (error) {
        console.error('Profile load error:', error);
        showNotification('Profil bilgileri yüklenirken hata oluştu.', 'error');
    } finally {
        showProfileLoading(false);
    }
}

// Populate form with profile data
function populateProfileForm(data) {
    const fields = [
        'firstName', 'lastName', 'bio', 'location', 
        'website', 'birthDate', 'facebookUrl', 'twitterUrl', 
        'instagramUrl', 'linkedInUrl', 'tikTokUrl', 'youTubeUrl'
    ];
    
    fields.forEach(field => {
        const element = document.getElementById(field);
        if (element) {
            if (field === 'birthDate' && data[field]) {
                // Handle date formatting
                const date = new Date(data[field]);
                if (!isNaN(date.getTime())) {
                    element.value = date.toISOString().split('T')[0];
                }
            } else if (data[field]) {
                element.value = data[field];
            } else {
                element.value = ''; // Clear field if no data
            }
        }
    });
    
    // Update profile image
    const profilePreview = document.getElementById('profilePreview');
    if (profilePreview) {
        if (data.profileImageUrl) {
            profilePreview.src = data.profileImageUrl;
        } else {
            profilePreview.src = '/images/placeholder.jpg'; // Default image
        }
    }
    
    // Set email separately (from User identity)
    const emailElement = document.getElementById('email');
    if (emailElement && data.username) {
        emailElement.value = data.username; // Assuming username is email in Identity
    }
    
    // Update bio character count
    const bioTextarea = document.getElementById('bio');
    const bioCharCount = document.getElementById('bioCharCount');
    if (bioTextarea && bioCharCount) {
        bioCharCount.textContent = bioTextarea.value.length;
    }
}

// Handle profile form submission
async function handleProfileSubmit(e) {
    e.preventDefault();
    
    const saveBtn = document.getElementById('saveBtn');
    const originalText = saveBtn.innerHTML;
    
    try {
        // Show loading state
        saveBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Kaydediliyor...';
        saveBtn.disabled = true;
        
        // Collect form data
        const formData = collectProfileFormData();
        
        // Validate form data
        if (!validateProfileForm(formData)) {
            return;
        }
        
        // Check if password fields are filled for password change
        const hasPasswordData = formData.currentPassword || formData.newPassword || formData.confirmPassword;
        
        if (hasPasswordData) {
            if (!validatePasswordChange(formData)) {
                return;
            }
            
            // Handle password change separately
            await handlePasswordChange({
                currentPassword: formData.currentPassword,
                newPassword: formData.newPassword,
                confirmPassword: formData.confirmPassword
            });
        }
        
        // Handle profile image upload if changed
        const profileImageInput = document.getElementById('profileImageInput');
        if (profileImageInput.files[0]) {
            const uploadResult = await handleProfileImageUpload(profileImageInput.files[0]);
            if (uploadResult.success && uploadResult.data) {
                // Update profile preview with new image URL
                const profilePreview = document.getElementById('profilePreview');
                if (profilePreview) {
                    profilePreview.src = uploadResult.data;
                }
                // Add the new profile image URL to form data
                formData.profileImageUrl = uploadResult.data;
            }
        }
        
        // Update profile data
        await updateProfile(formData);
        
        showNotification('Profil başarıyla güncellendi!', 'success');
        
        // Clear password fields
        document.getElementById('currentPassword').value = '';
        document.getElementById('newPassword').value = '';
        document.getElementById('confirmPassword').value = '';
        
    } catch (error) {
        console.error('Profile update error:', error);
        showNotification('Profil güncellenirken hata oluştu.', 'error');
    } finally {
        // Restore button state
        saveBtn.innerHTML = originalText;
        saveBtn.disabled = false;
    }
}

// Collect form data
function collectProfileFormData() {
    const getValue = (id) => {
        const element = document.getElementById(id);
        if (!element) return null;
        const value = element.value.trim();
        return value === '' ? null : value;
    };

    return {
        firstName: getValue('firstName'),
        lastName: getValue('lastName'),
        bio: getValue('bio'),
        location: getValue('location'),
        website: getValue('website'),
        birthDate: getValue('birthDate'),
        facebookUrl: getValue('facebookUrl'),
        twitterUrl: getValue('twitterUrl'),
        instagramUrl: getValue('instagramUrl'),
        linkedInUrl: getValue('linkedInUrl'),
        tikTokUrl: getValue('tikTokUrl'),
        youTubeUrl: getValue('youTubeUrl'),
        currentPassword: document.getElementById('currentPassword')?.value || '',
        newPassword: document.getElementById('newPassword')?.value || '',
        confirmPassword: document.getElementById('confirmPassword')?.value || ''
    };
}

// Validate profile form
function validateProfileForm(data) {
    clearValidationErrors();
    let isValid = true;
    
    // Validate URLs (only if they are not null/empty)
    const urlFields = ['website', 'facebookUrl', 'twitterUrl', 'instagramUrl', 'linkedInUrl', 'tikTokUrl', 'youTubeUrl'];
    urlFields.forEach(field => {
        if (data[field] && data[field].trim() !== '' && !isValidUrl(data[field])) {
            showFieldError(field, 'Geçerli bir URL girin');
            isValid = false;
        }
    });
    
    // Validate bio length
    if (data.bio && data.bio.length > 500) {
        showFieldError('bio', 'Biyografi 500 karakterden uzun olamaz');
        isValid = false;
    }
    
    return isValid;
}

// Validate password change
function validatePasswordChange(data) {
    clearPasswordValidationErrors();
    let isValid = true;
    
    if (!data.currentPassword) {
        showFieldError('currentPassword', 'Mevcut şifre gerekli');
        isValid = false;
    }
    
    if (!data.newPassword) {
        showFieldError('newPassword', 'Yeni şifre gerekli');
        isValid = false;
    } else if (data.newPassword.length < 6) {
        showFieldError('newPassword', 'Şifre en az 6 karakter uzunluğunda olmalıdır');
        isValid = false;
    }
    
    if (data.newPassword !== data.confirmPassword) {
        showFieldError('confirmPassword', 'Şifreler eşleşmiyor');
        isValid = false;
    }
    
    return isValid;
}

// Utility functions
function isValidUrl(string) {
    try {
        new URL(string);
        return true;
    } catch (_) {
        return false;
    }
}

function showFieldError(fieldId, message) {
    const field = document.getElementById(fieldId);
    if (field) {
        field.classList.add('error');
        
        // Remove existing error message
        const existingError = field.parentNode.querySelector('.error-message');
        if (existingError) {
            existingError.remove();
        }
        
        // Add new error message
        const errorElement = document.createElement('span');
        errorElement.className = 'error-message';
        errorElement.textContent = message;
        field.parentNode.appendChild(errorElement);
    }
}

function clearValidationErrors() {
    document.querySelectorAll('.form-input, .form-textarea').forEach(field => {
        field.classList.remove('error', 'success');
    });
    document.querySelectorAll('.error-message').forEach(error => {
        error.remove();
    });
}

function clearPasswordValidationErrors() {
    const passwordFields = ['currentPassword', 'newPassword', 'confirmPassword'];
    passwordFields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (field) {
            field.classList.remove('error');
            const errorElement = field.parentNode.querySelector('.error-message');
            if (errorElement) {
                errorElement.remove();
            }
        }
    });
}

function showProfileLoading(isLoading) {
    const form = document.getElementById('profileUpdateForm');
    if (form) {
        if (isLoading) {
            form.style.opacity = '0.6';
            form.style.pointerEvents = 'none';
        } else {
            form.style.opacity = '1';
            form.style.pointerEvents = 'auto';
        }
    }
}

function showNotification(message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.innerHTML = `
        <div class="notification-content">
            <span class="notification-message">${message}</span>
            <button class="notification-close" onclick="this.parentElement.parentElement.remove()">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;
    
    // Add styles if not already added
    if (!document.getElementById('notification-styles')) {
        const styles = document.createElement('style');
        styles.id = 'notification-styles';
        styles.textContent = `
            .notification {
                position: fixed;
                top: 20px;
                right: 20px;
                min-width: 300px;
                max-width: 500px;
                padding: 16px;
                border-radius: 8px;
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
                z-index: 10000;
                animation: slideInRight 0.3s ease-out;
                font-family: inherit;
            }
            
            .notification-success {
                background: #d4edda;
                border: 1px solid #c3e6cb;
                color: #155724;
            }
            
            .notification-error {
                background: #f8d7da;
                border: 1px solid #f5c6cb;
                color: #721c24;
            }
            
            .notification-info {
                background: #d1ecf1;
                border: 1px solid #bee5eb;
                color: #0c5460;
            }
            
            .notification-content {
                display: flex;
                justify-content: space-between;
                align-items: center;
                gap: 10px;
            }
            
            .notification-message {
                flex: 1;
                font-size: 14px;
                line-height: 1.4;
            }
            
            .notification-close {
                background: none;
                border: none;
                font-size: 14px;
                cursor: pointer;
                padding: 4px;
                border-radius: 4px;
                color: inherit;
                opacity: 0.7;
                transition: opacity 0.2s;
            }
            
            .notification-close:hover {
                opacity: 1;
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
        `;
        document.head.appendChild(styles);
    }
    
    // Add to body
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentElement) {
            notification.style.animation = 'slideInRight 0.3s ease-out reverse';
            setTimeout(() => notification.remove(), 300);
        }
    }, 5000);
}

// API call functions
async function updateProfile(data) {
    try {
        const updateDto = {
            firstName: data.firstName,
            lastName: data.lastName,
            bio: data.bio,
            location: data.location,
            website: data.website,
            birthDate: data.birthDate,
            facebookUrl: data.facebookUrl,
            twitterUrl: data.twitterUrl,
            instagramUrl: data.instagramUrl,
            linkedInUrl: data.linkedInUrl,
            tikTokUrl: data.tikTokUrl,
            youTubeUrl: data.youTubeUrl
        };

        const result = await window.profileApiService.updateProfile(updateDto);
        
        if (!result.success) {
            throw new Error(result.message || 'Profil güncellenemedi');
        }
        
        return result;
    } catch (error) {
        console.error('Update profile error:', error);
        throw error;
    }
}

async function handlePasswordChange(data) {
    try {
        const passwordDto = {
            currentPassword: data.currentPassword,
            newPassword: data.newPassword,
            confirmPassword: data.confirmPassword
        };

        const result = await window.profileApiService.changePassword(passwordDto);
        
        if (!result.success) {
            throw new Error(result.message || 'Şifre değiştirilemedi');
        }
        
        return result;
    } catch (error) {
        console.error('Password change error:', error);
        throw error;
    }
}

async function handleProfileImageUpload(file) {
    try {
        const formData = new FormData();
        formData.append('file', file);

        const result = await window.profileApiService.uploadProfileImage(formData);
        
        if (!result.success) {
            throw new Error(result.message || 'Profil resmi yüklenemedi');
        }
        
        return result;
    } catch (error) {
        console.error('Profile image upload error:', error);
        throw error;
    }
}