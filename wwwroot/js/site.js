// General site JavaScript

// Initialize dental chart when appointment form loads
document.addEventListener('DOMContentLoaded', function() {
    // Dental chart initialization is handled in dental-chart.js
    
    // Form validation
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
});

// Auto-refresh notifications every minute
if (window.location.pathname.includes('/Notifications')) {
    setInterval(function() {
        window.location.reload();
    }, 60000);
}

