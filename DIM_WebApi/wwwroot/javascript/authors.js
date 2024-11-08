document.addEventListener('DOMContentLoaded', () => {
    loadAuthors();

    document.getElementById('addAuthorBtn').addEventListener('click', () => {
        document.getElementById('addAuthorFormContainer').style.display = 'block';
    });

    document.getElementById('addAuthorForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const name = document.getElementById('name').value;
        const surname = document.getElementById('surname').value;

        await fetch('/api/Author', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, surname })
        });
        clearAddAuthorForm();
        loadAuthors();
    });

    document.getElementById('updateAuthorForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const id = document.getElementById('editAuthorId').value;
        const name = document.getElementById('editName').value;
        const surname = document.getElementById('editSurname').value;

        await fetch(`/api/Author/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, surname })
        });
        clearUpdateAuthorForm();
        loadAuthors();
    });
});

async function loadAuthors() {
    const response = await fetch('/api/Author');
    const authors = await response.json();
    const authorsList = document.getElementById('authorsList');
    authorsList.innerHTML = '';

    authors.forEach(author => {
        const card = document.createElement('div');
        card.className = 'card';

        const cardBody = document.createElement('div');
        cardBody.className = 'card-body';
        cardBody.innerHTML = `<h5 class="card-title">${author.name} ${author.surname}</h5>`;

        const buttonGroup = document.createElement('div');
        buttonGroup.className = 'button-group';

        const editButton = document.createElement('button');
        editButton.className = 'btn btn-warning btn-sm';
        editButton.textContent = 'Edit';
        editButton.onclick = () => editAuthor(author.id, author.name, author.surname);

        const deleteButton = document.createElement('button');
        deleteButton.className = 'btn btn-danger btn-sm';
        deleteButton.textContent = 'Delete';
        deleteButton.onclick = () => deleteAuthor(author.id);

        buttonGroup.appendChild(editButton);
        buttonGroup.appendChild(deleteButton);
        cardBody.appendChild(buttonGroup);
        card.appendChild(cardBody);
        authorsList.appendChild(card);
    });
}

function editAuthor(id, name, surname) {
    document.getElementById('editAuthorId').value = id;
    document.getElementById('editName').value = name;
    document.getElementById('editSurname').value = surname;
    document.getElementById('updateFormContainer').style.display = 'block';
}

async function deleteAuthor(id) {
    await fetch(`/api/Author/${id}`, { method: 'DELETE' });
    loadAuthors();
}

function clearAddAuthorForm() {
    document.getElementById('addAuthorForm').reset();
    document.getElementById('addAuthorFormContainer').style.display = 'none';
}

function clearUpdateAuthorForm() {
    document.getElementById('updateAuthorForm').reset();
    document.getElementById('updateFormContainer').style.display = 'none';
}
