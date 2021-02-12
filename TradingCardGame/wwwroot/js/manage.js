async function renderUsers() {
    const users = await getUsers();
    const dataContainer = document.querySelector('div.data-container');
    dataContainer.innerHTML = '';

    for (const user of users) {
        const userEmailElement = document.createElement('span');
        userEmailElement.classList.add('user-email');
        userEmailElement.textContent = user.email;

        const deleteUserElement = document.createElement('span');
        deleteUserElement.classList.add('delete-user');
        deleteUserElement.textContent = 'X';
        deleteUserElement.addEventListener('click', () => {
            fetch('/Manage/RemoveUser?userId=' + user.id);
            userCardElement.remove();
        });

        const moderatorOption = document.createElement('option');
        moderatorOption.value = 'Moderator';
        moderatorOption.text = 'Moderator';

        const userOption = document.createElement('option');
        userOption.value = 'User';
        userOption.text = 'User';

        if (user.role == 'Moderator') {
            moderatorOption.selected = true;
        } else if (user.role == 'User') {
            userOption.selected = true;
        }

        const roleSelect = document.createElement('select');
        roleSelect.appendChild(moderatorOption);
        roleSelect.appendChild(userOption);

        roleSelect.addEventListener('change', () => {
            const selected = roleSelect.value;
            if (selected === user.role) {
                return;
            }

            fetch(`/Manage/ChangeRole?userId=${user.id}&role=${selected}`);
        });

        const userRoleElement = document.createElement('span');
        userRoleElement.classList.add('user-role');
        userRoleElement.appendChild(roleSelect);

        const userCardElement = document.createElement('div');
        userCardElement.classList.add('user-card');

        userCardElement.appendChild(userEmailElement);
        userCardElement.appendChild(deleteUserElement);
        userCardElement.appendChild(userRoleElement);

        dataContainer.appendChild(userCardElement);
    }
}

function getUsers() {
    return fetch('/Manage/GetUsers')
        .then(response => response.json())
        .then(data => data);
}

window.onload = async function () {
    renderUsers();
}