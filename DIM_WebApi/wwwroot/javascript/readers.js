let readers = [];

document.addEventListener('DOMContentLoaded', async () => {
    await fetchReaders();
});

async function fetchReaders() {
    const response = await fetch('/api/Reader');
    readers = await response.json();
    displayReaders();
}

function displayReaders() {
    const readerList = document.getElementById('readerList');
    readerList.innerHTML = '';

    readers.forEach(reader => {
        const listItem = document.createElement('li');
        listItem.className = 'list-group-item';

        listItem.textContent = `${reader.name} ${reader.surname}`;

        const buttonGroup = document.createElement('div');
        buttonGroup.className = 'button-group';

        const editButton = document.createElement('button');
        editButton.textContent = 'Edit';
        editButton.className = 'btn btn-warning btn-sm';
        editButton.onclick = () => editReader(reader.id);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.className = 'btn btn-danger btn-sm';
        deleteButton.onclick = () => deleteReader(reader.id);

        buttonGroup.appendChild(editButton);
        buttonGroup.appendChild(deleteButton);

        listItem.appendChild(buttonGroup);
        readerList.appendChild(listItem);
    });
}

async function addReader() {
    const readerName = document.getElementById('readerName').value;
    const readerSurname = document.getElementById('readerSurname').value;

    const response = await fetch('/api/Reader', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            name: readerName,
            surname: readerSurname
        }),
    });

    if (response.ok) {
        await fetchReaders();
        clearAddReaderForm();
    } else {
        alert('Error adding reader.');
    }
}

function editReader(readerId) {
    const reader = readers.find(r => r.id === readerId);
    if (reader) {
        document.getElementById('editReaderName').value = reader.name;
        document.getElementById('editReaderSurname').value = reader.surname;
        document.getElementById('editReaderId').value = readerId;
        document.getElementById('editReaderForm').style.display = 'block';
    }
}

async function saveEditedReader() {
    const readerId = document.getElementById('editReaderId').value;
    const newName = document.getElementById('editReaderName').value;
    const newSurname = document.getElementById('editReaderSurname').value;

    const response = await fetch(`/api/Reader/${readerId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            id: readerId,
            name: newName,
            surname: newSurname
        }),
    });

    if (response.ok) {
        await fetchReaders();
        clearEditReaderForm();
    } else {
        alert('Error updating reader.');
    }
}

async function deleteReader(readerId) {
    const confirmDelete = confirm("Are you sure you want to delete this reader?");
    if (confirmDelete) {
        const response = await fetch(`/api/Reader/${readerId}`, {
            method: 'DELETE',
        });

        if (response.ok) {
            await fetchReaders();
        } else {
            alert('Error deleting reader.');
        }
    }
}

function clearAddReaderForm() {
    document.getElementById('readerName').value = '';
    document.getElementById('readerSurname').value = '';
    document.getElementById('addReaderForm').style.display = 'none';
}

function clearEditReaderForm() {
    document.getElementById('editReaderName').value = '';
    document.getElementById('editReaderSurname').value = '';
    document.getElementById('editReaderId').value = '';
    document.getElementById('editReaderForm').style.display = 'none';
}
