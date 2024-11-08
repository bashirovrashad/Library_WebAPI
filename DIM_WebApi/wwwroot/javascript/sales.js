let sales = [];
let books = [];
let readers = [];

document.addEventListener('DOMContentLoaded', async () => {
    await loadInitialData();
});

async function loadInitialData() {
    try {
        await Promise.all([fetchBooks(), fetchReaders()]);
        await fetchSales();
    } catch (error) {
        console.error('Error loading initial data:', error);
    }
}

async function fetchSales() {
    try {
        const response = await fetch('/api/Sale');
        if (!response.ok) throw new Error('Network response was not ok');
        sales = await response.json();
        displaySales();
    } catch (error) {
        console.error('Error fetching sales data:', error);
        alert('Error fetching sales data.');
    }
}

async function fetchBooks() {
    try {
        const response = await fetch('/api/Book');
        if (!response.ok) throw new Error('Network response was not ok');
        books = await response.json();
    } catch (error) {
        console.error('Error fetching books data:', error);
        alert('Error fetching books data.');
    }
}

async function fetchReaders() {
    try {
        const response = await fetch('/api/Reader');
        if (!response.ok) throw new Error('Network response was not ok');
        readers = await response.json();
    } catch (error) {
        console.error('Error fetching readers data:', error);
        alert('Error fetching readers data.');
    }
}

function displaySales() {
    const salesList = document.getElementById('salesList');
    salesList.innerHTML = '';

    if (sales.length === 0) {
        salesList.innerHTML = '<p>No sales found.</p>';
        return;
    }

    sales.forEach(sale => {
        const book = books.find(b => b.id === sale.bookId);
        const reader = readers.find(r => r.id === sale.readerId);
        const bookName = book ? book.name : 'Unknown';
        const readerName = reader ? `${reader.name} ${reader.surname}` : 'Unknown';

        const card = document.createElement('div');
        card.className = 'card';

        const cardBody = document.createElement('div');
        cardBody.className = 'card-body';

        cardBody.innerHTML = `          
            <p class="card-text">Book: ${bookName} | Reader: ${readerName} | Date: ${new Date(sale.dateTime).toLocaleString()}</p>
        `;

        const buttonGroup = document.createElement('div');
        buttonGroup.className = 'button-group';

        const editButton = document.createElement('button');
        editButton.className = 'btn btn-warning btn-sm';
        editButton.textContent = 'Edit';
        editButton.onclick = () => openEditSaleModal(sale.id);

        const deleteButton = document.createElement('button');
        deleteButton.className = 'btn btn-danger btn-sm';
        deleteButton.textContent = 'Delete';
        deleteButton.onclick = () => deleteSale(sale.id);

        buttonGroup.appendChild(editButton);
        buttonGroup.appendChild(deleteButton);

        cardBody.appendChild(buttonGroup);
        card.appendChild(cardBody);
        salesList.appendChild(card);
    });
}

function fillBookAndReaderOptions() {
    const bookSelect = document.getElementById('bookSelect');
    const readerSelect = document.getElementById('readerSelect');

    bookSelect.innerHTML = '<option value="">Select a book</option>';
    readerSelect.innerHTML = '<option value="">Select a reader</option>';

    books.forEach(book => {
        const option = document.createElement('option');
        option.value = book.id;
        option.textContent = book.name;
        bookSelect.appendChild(option);
    });

    readers.forEach(reader => {
        const option = document.createElement('option');
        option.value = reader.id;
        option.textContent = `${reader.name} ${reader.surname}`;
        readerSelect.appendChild(option);
    });
}

async function openAddSaleModal() {
    document.getElementById('saleModalTitle').textContent = "Add Sale";
    document.getElementById('saleId').value = '';
    document.getElementById('bookSelect').value = '';
    document.getElementById('readerSelect').value = '';
    fillBookAndReaderOptions();
    $('#saleModal').modal('show');
}

async function openEditSaleModal(saleId) {
    const sale = sales.find(s => s.id === saleId);
    if (!sale) return;

    document.getElementById('saleModalTitle').textContent = "Edit Sale";
    document.getElementById('saleId').value = saleId;
    document.getElementById('bookSelect').value = sale.bookId;
    document.getElementById('readerSelect').value = sale.readerId;
    fillBookAndReaderOptions();
    $('#saleModal').modal('show');
}

async function saveSale() {
    const saleId = document.getElementById('saleId').value;
    const bookId = document.getElementById('bookSelect').value;
    const readerId = document.getElementById('readerSelect').value;
    const dateTime = new Date().toISOString();

    const method = saleId ? 'PUT' : 'POST';
    const url = saleId ? `/api/Sale/${saleId}` : '/api/Sale';

    try {
        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id: saleId ? parseInt(saleId) : undefined, bookId: parseInt(bookId), readerId: parseInt(readerId), dateTime })
        });

        if (response.ok) {
            alert(saleId ? "Sale updated successfully." : "Sale added successfully.");
            await fetchSales();
            $('#saleModal').modal('hide');
        } else {
            alert("Error saving sale.");
        }
    } catch (error) {
        console.error("Error saving sale:", error);
    }
}

async function deleteSale(saleId) {
    if (confirm("Are you sure you want to delete this sale?")) {
        try {
            const response = await fetch(`/api/Sale/${saleId}`, { method: 'DELETE' });
            if (response.ok) {
                alert("Sale deleted successfully.");
                await fetchSales();
            } else {
                alert("Error deleting sale.");
            }
        } catch (error) {
            console.error("Error deleting sale:", error);
        }
    }
}
