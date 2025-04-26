document.addEventListener("DOMContentLoaded", () => {
    // Toggle mobile menu
    document.querySelector('.menu-toggle').addEventListener('click', function () {
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





})