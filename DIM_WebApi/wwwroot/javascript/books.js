let books = [];
let authors = [];
let editingBookId = null;

document.addEventListener('DOMContentLoaded', async () => {
    await fetchBooks();
    await fetchAuthors();
});

async function fetchBooks() {
    const response = await fetch('/api/Book');
    books = await response.json();
    displayBooks();
}

async function fetchAuthors() {
    const response = await fetch('/api/Author');
    authors = await response.json();
    populateAuthorDropdown();
}

function populateAuthorDropdown() {
    const authorSelect = document.getElementById('authorSelect');
    const editAuthorSelect = document.getElementById('editAuthorSelect');
    authorSelect.innerHTML = '';
    editAuthorSelect.innerHTML = '';

    authors.forEach(author => {
        const option = document.createElement('option');
        option.value = author.id;
        option.textContent = `${author.name} ${author.surname}`;
        authorSelect.appendChild(option);

        const editOption = document.createElement('option');
        editOption.value = author.id;
        editOption.textContent = `${author.name} ${author.surname}`;
        editAuthorSelect.appendChild(editOption);
    });
}

function displayBooks() {
    const bookList = document.getElementById('bookList');
    bookList.innerHTML = '';

    books.forEach(book => {
        const card = document.createElement('div');
        card.className = 'card';

        const cardBody = document.createElement('div');
        cardBody.className = 'card-body d-flex justify-content-between align-items-center';
        cardBody.innerHTML = `
            <div>
                <h5 class="card-title">${book.name}</h5>
                <p class="card-text">Year: ${book.year} - Price: $${book.price}</p>
            </div>
        `;

        const buttonGroup = document.createElement('div');
        buttonGroup.className = 'button-group';

        const editButton = document.createElement('button');
        editButton.textContent = 'Edit';
        editButton.className = 'btn btn-warning btn-sm';
        editButton.onclick = () => showEditBookForm(book);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.className = 'btn btn-danger btn-sm';
        deleteButton.onclick = () => deleteBook(book.id);

        buttonGroup.appendChild(editButton);
        buttonGroup.appendChild(deleteButton);

        cardBody.appendChild(buttonGroup);
        card.appendChild(cardBody);
        bookList.appendChild(card);
    });
}

async function addBook() {
    const bookName = document.getElementById('bookName').value;
    const bookYear = document.getElementById('bookYear').value;
    const bookPrice = document.getElementById('bookPrice').value;
    const authorId = document.getElementById('authorSelect').value;

    const response = await fetch('/api/Book', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: bookName, year: parseInt(bookYear), price: parseFloat(bookPrice), authorId: parseInt(authorId) }),
    });

    if (response.ok) {
        await fetchBooks();
        clearAddBookForm();
    } else {
        alert('Error adding book.');
    }
}

async function deleteBook(id) {
    const response = await fetch(`/api/Book/${id}`, { method: 'DELETE' });
    if (response.ok) await fetchBooks();
    else alert('Error deleting book.');
}

function showAddBookForm() {
    document.getElementById('addBookForm').style.display = 'block';
}

function clearAddBookForm() {
    document.getElementById('addBookForm').style.display = 'none';
    document.getElementById('bookName').value = '';
    document.getElementById('bookYear').value = '';
    document.getElementById('bookPrice').value = '';
    document.getElementById('authorSelect').value = '';
}

function showEditBookForm(book) {
    editingBookId = book.id;
    document.getElementById('editBookName').value = book.name;
    document.getElementById('editBookYear').value = book.year;
    document.getElementById('editBookPrice').value = book.price;
    document.getElementById('editAuthorSelect').value = book.authorId;
    document.getElementById('editBookForm').style.display = 'block';
}

function clearEditBookForm() {
    document.getElementById('editBookForm').style.display = 'none';
    editingBookId = null;
}

async function updateBook() {
    const bookName = document.getElementById('editBookName').value;
    const bookYear = document.getElementById('editBookYear').value;
    const bookPrice = document.getElementById('editBookPrice').value;
    const authorId = document.getElementById('editAuthorSelect').value;

    const response = await fetch(`/api/Book/${editingBookId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: bookName, year: parseInt(bookYear), price: parseFloat(bookPrice), authorId: parseInt(authorId) }),
    });

    if (response.ok) {
        await fetchBooks();
        clearEditBookForm();
    } else {
        alert('Error updating book.');
    }
}
