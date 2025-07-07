
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


    window.openConfirmModal = function (actionUrl, message, mode = 'disclaimMode') {
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

    window.openConfirmImagesModal = function (actionUrl) {
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

    document.body.addEventListener('click', function (e) {
        const target = e.target.closest('.confirmable-link');
        if (target) {
            e.preventDefault();

            const href = target.getAttribute('data-confirm-href');
            const message = target.getAttribute('data-confirm-message') || "آیا مطمئن هستید؟";

            openSimpleConfirmModal(message, function () {
                window.location.href = href;
            });
        }
    });

    window.openSimpleConfirmModal = function (message, onConfirm, mode = 'disclaimMode') {
        document.getElementById('simpleConfirmMessage').innerText = message || "آیا مطمئن هستید؟";

        const confirmBtn = document.getElementById('simpleConfirmBtn');
        const cancelBtn = document.querySelector('#simpleConfirmModal .btn-secondary');

        confirmBtn.className = 'btn';
        cancelBtn.className = 'btn';

        if (mode === 'acceptMode') {
            confirmBtn.classList.add('btn-success');
            cancelBtn.classList.add('btn-danger');
        } else {
            confirmBtn.classList.add('btn-danger');
            cancelBtn.classList.add('btn-secondary');
        }

        const newConfirmBtn = confirmBtn.cloneNode(true);
        confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);

        newConfirmBtn.addEventListener('click', function () {
            const modal = bootstrap.Modal.getInstance(document.getElementById('simpleConfirmModal'));
            modal.hide();
            if (typeof onConfirm === 'function') {
                onConfirm();
            }
        });

        const modal = new bootstrap.Modal(document.getElementById('simpleConfirmModal'));
        modal.show();
    };

    // Options
    let optionIndex = 0;

    window.createOptionElement = function (text = "", isChecked = false) {
        const id = `option-${optionIndex}`;
        const container = document.createElement("div");
        container.className = "input-group mb-2";
        container.dataset.optionId = id;

        container.innerHTML = `
      <div class="input-group-text">
        <input type="radio" name="optionRadio" value="${text}" ${isChecked ? "checked" : ""
            } onchange="setCorrectAnswer(this)">
      </div>
      <input name='Options[${optionIndex}]' type="text" class="form-control option-text" value="${text}" placeholder="متن گزینه..." oninput="updateRadioValue(this)">
      <button class="btn btn-danger" type="button" onclick="removeOption('${id}')">حذف</button>
    `;

        optionIndex++;
        return container;
    }

    let addOptionButton = document
        .getElementById("addOptionBtn");

    if (addOptionButton) {
        addOptionButton.addEventListener("click", () => {
            const option = createOptionElement();
            document.getElementById("optionsList").appendChild(option);
        });
    }

    window.openAddQuestionConfirm = function () {

        const modal = new bootstrap.Modal(document.getElementById('questionModal'));
        modal.show();
    }

    // For Edit Option
    window.optionIndexForQuestionEdit = document.querySelectorAll('#optionList .option-item').length;

    // حذف گزینه
    $(document).on('click', '.remove-option', function () {
        $(this).closest('.option-item').remove();
        updateOptionNames();
        optionIndexForQuestionEdit = $('#optionList .option-item').length;
    });


    window.openAddOption = function () {

        const modal = new bootstrap.Modal(document.getElementById('optionModal'));
        modal.show();
    }


    window.openEditOption = function () {

        const modal = new bootstrap.Modal(document.getElementById('editOptionModal'));
        modal.show();
    }


})

function setCorrectAnswer(radio) {
    document.getElementById("correctAnswerInput").value = radio.value;
}

function reindexOptions() {
    const inputs = document.querySelectorAll("#optionsList .option-text");
    inputs.forEach((input, index) => {
        input.name = `Options[${index}]`;
    });
    optionIndex = inputs.length; // همگام‌سازی با شمارنده
}

function updateRadioValue(input) {
    const radio = input
        .closest(".input-group")
        .querySelector('input[type="radio"]');
    radio.value = input.value;
    if (radio.checked) setCorrectAnswer(radio);
}

function removeOption(id) {
    const el = document.querySelector(`[data-option-id="${id}"]`);
    if (el) el.remove();
    reindexOptions();
}

// For Edit Option

function updateOptionNames() {
    $('#optionList .option-item').each(function (index) {
        $(this).find('input').attr('name', `Options[${index}]`);
    });
}


function addOptionForEditQuestion(text = "") {
    const optionListContainer = document.getElementById("optionList");

    const div = document.createElement("div");
    div.className = "input-group mb-2 option-item";

    div.innerHTML = `
    <input type="text" class="form-control" name="Options[${optionIndexForQuestionEdit}]" value="${text}" placeholder="متن گزینه">
    <button type="button" class="btn btn-danger remove-option">حذف</button>
  `;

    optionListContainer.appendChild(div);
    optionIndexForQuestionEdit++;
}


function openEditModal(button, actionUrl) {
    const form = document.getElementById("optionFormEdit");
    if (!form) return;

    form.setAttribute("action", actionUrl);

    const optionItem = button.closest(".option-item");
    const textSpan = optionItem.querySelector(".option-text");

    if (textSpan) {
        const input = form.querySelector("input[name='Option']");
        if (input) {
            input.value = textSpan.innerText.trim();
        }
    }
}