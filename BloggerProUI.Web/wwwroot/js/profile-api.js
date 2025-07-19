// Profile API Service - Client-side API communication
class ProfileApiService {
    constructor() {
        this.baseUrl = '/UserPanel/api/profile';
    }

    // Get current user profile
    async getCurrentProfile() {
        try {
            const response = await fetch(`${this.baseUrl}/current`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error fetching profile:', error);
            throw error;
        }
    }

    // Update user profile
    async updateProfile(profileData) {
        try {
            const response = await fetch(this.baseUrl, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(profileData)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error updating profile:', error);
            throw error;
        }
    }

    // Change password
    async changePassword(passwordData) {
        try {
            const response = await fetch(`${this.baseUrl}/change-password`, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(passwordData)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error changing password:', error);
            throw error;
        }
    }

    // Upload profile image
    async uploadProfileImage(formData) {
        try {
            const response = await fetch(`${this.baseUrl}/upload-avatar`, {
                method: 'POST',
                body: formData // FormData is sent without Content-Type header
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error uploading profile image:', error);
            throw error;
        }
    }
}

// Global instance
window.profileApiService = new ProfileApiService();