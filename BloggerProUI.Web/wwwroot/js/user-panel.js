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

// Modern Logout Confirmation
function confirmLogout() {
    // Modern confirmation modal
    const modal = document.createElement('div');
    modal.className = 'logout-modal-overlay';
    modal.innerHTML = `
        <div class="logout-modal">
            <div class="logout-modal-header">
                <i class="fas fa-sign-out-alt"></i>
                <h3>Çıkış Yapmak İstediğinize Emin misiniz?</h3>
            </div>
            <div class="logout-modal-body">
                <p>Oturumunuz sonlandırılacak ve giriş sayfasına yönlendirileceksiniz.</p>
            </div>
            <div class="logout-modal-footer">
                <button class="cancel-btn" onclick="closeLogoutModal()">
                    <i class="fas fa-times"></i> İptal
                </button>
                <button class="confirm-btn" onclick="performLogout()">
                    <i class="fas fa-check"></i> Evet, Çıkış Yap
                </button>
            </div>
        </div>
    `;
    
    document.body.appendChild(modal);
    
    // Animation
    setTimeout(() => {
        modal.classList.add('show');
    }, 10);
}

function closeLogoutModal() {
    const modal = document.querySelector('.logout-modal-overlay');
    if (modal) {
        modal.classList.remove('show');
        setTimeout(() => {
            modal.remove();
        }, 300);
    }
}

function performLogout() {
    // Loading state
    const confirmBtn = document.querySelector('.confirm-btn');
    if (confirmBtn) {
        confirmBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Çıkış yapılıyor...';
        confirmBtn.disabled = true;
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