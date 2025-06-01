
document.addEventListener("DOMContentLoaded", () => {
    // Toggle mobile menu
    let menu = document.querySelector('.menu-toggle');
    if (menu != null) {
        menu.addEventListener('click', function () {
            document.querySelector('.sidebar').classList.toggle('active');
        });

        // Close mobile menu when clicking outside
        document.addEventListener('click', function (event) {
            const sidebar = document.querySelector('.sidebar');
            const menuToggle = document.querySelector('.menu-toggle');

            if (!sidebar.contains(event.target) && !menuToggle.contains(event.target)) {
                sidebar.classList.remove('active');
            }
        });
    }


    // Profile picture preview
    let userPic = document.getElementById('profilePicture');
    if (userPic != null) {
        userPic.addEventListener('change', function (e) {
            if (e.target.files && e.target.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    document.querySelector('.profile-picture').src = e.target.result;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        });
    }


    window.openConfirmModal = function(actionUrl, message, mode = 'disclaimMode') {
        const form = document.getElementById('confirmActionForm');
        form.action = actionUrl;

        if (!form) {
            console.error('confirmActionForm not found');
            return;
        }

        document.getElementById('confirmActionMessage').innerText = message || "آیا مطمئن هستید؟";

        const confirmBtn = form.querySelector('button[type="submit"]');
        const cancelBtn = form.querySelector('button[data-close="true"]');

        confirmBtn.className = "btn";
        cancelBtn.className = "btn";

        if (mode === 'acceptMode') {
            confirmBtn.classList.add('btn-success');
            cancelBtn.classList.add('btn-danger');
        } else {
            confirmBtn.classList.add('btn-danger');
            cancelBtn.classList.add('btn-secondary');
        }

        const modal = new bootstrap.Modal(document.getElementById('actionConfirmModal'));
        modal.show();
    }

    window.openConfirmImagesModal = function(actionUrl) {
        const form = document.getElementById('confirmImagesActionForm');

        if (!form) {
            console.error('confirmImagesActionForm not found');
            return;
        }
        form.action = actionUrl;


        const modal = new bootstrap.Modal(document.getElementById('actionConfirmImagesModal'));
        modal.show();
    }

    let topButton = document.getElementById('goTopBtn');

    if (topButton) {
        topButton.addEventListener('click', () => {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }
    


})