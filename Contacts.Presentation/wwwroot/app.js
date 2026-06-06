'use strict';

const API = '/api/contacts';
const PAGE_SIZE = 10;
const PHONE_REGEX = /^\+?[\d\s\-()]{7,20}$/;
const MIN_BIRTH_DATE = new Date('1900-01-02');

let currentPage = 1;
let totalCount = 0;
let pendingDeleteId = null;

const contactModal = new bootstrap.Modal(document.getElementById('contactModal'));
const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));

// Load & Render 

async function loadContacts() {
    const tbody = document.getElementById('contactsBody');
    tbody.innerHTML = '<tr><td colspan="5" class="text-center text-muted py-4">Loading...</td></tr>';

    try {
        const res = await fetch(`${API}/getAllContacts?PageNumber=${currentPage}&PageSize=${PAGE_SIZE}`);
        if (!res.ok) throw new Error('Failed to load contacts.');

        const data = await res.json();
        totalCount = data.totalCount;
        renderTable(data.items);
        renderPagination();
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="5" class="text-center text-danger py-4">${err.message}</td></tr>`;
    }
}

function renderTable(contacts) {
    const tbody = document.getElementById('contactsBody');

    if (!contacts || contacts.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" class="text-center text-muted py-4">No contacts found.</td></tr>';
        return;
    }

    tbody.innerHTML = contacts.map(c => `
        <tr>
            <td>${escapeHtml(c.name)}</td>
            <td>${escapeHtml(c.mobilePhone)}</td>
            <td>${escapeHtml(c.jobTitle || '—')}</td>
            <td>${c.birthDate}</td>
            <td class="table-actions">
                <button class="btn btn-sm btn-outline-primary me-1" onclick="openEditModal(${c.id})">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" onclick="openDeleteModal(${c.id}, '${escapeHtml(c.name)}')">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function renderPagination() {
    const totalPages = Math.ceil(totalCount / PAGE_SIZE);
    document.getElementById('pageInfo').textContent =
        `Page ${currentPage} of ${totalPages} (${totalCount} total)`;
    document.getElementById('prevBtn').disabled = currentPage <= 1;
    document.getElementById('nextBtn').disabled = currentPage >= totalPages;
}

function changePage(delta) {
    currentPage += delta;
    loadContacts();
}

// Add / Edit Modal 

function openAddModal() {
    document.getElementById('modalTitle').textContent = 'Add Contact';
    document.getElementById('contactId').value = '';
    document.getElementById('contactForm').reset();
    clearValidation();
    contactModal.show();
}

async function openEditModal(id) {
    try {
        const res = await fetch(`${API}/getContactById/${id}`);
        if (!res.ok) throw new Error('Contact not found.');

        const c = await res.json();
        document.getElementById('modalTitle').textContent = 'Edit Contact';
        document.getElementById('contactId').value = c.id;
        document.getElementById('name').value = c.name;
        document.getElementById('mobilePhone').value = c.mobilePhone;
        document.getElementById('jobTitle').value = c.jobTitle || '';
        document.getElementById('birthDate').value = c.birthDate;
        clearValidation();
        contactModal.show();
    } catch (err) {
        alert(err.message);
    }
}

async function saveContact() {
    const id = document.getElementById('contactId').value;
    const payload = {
        id: id ? parseInt(id) : 0,
        name: document.getElementById('name').value.trim(),
        mobilePhone: document.getElementById('mobilePhone').value.trim(),
        jobTitle: document.getElementById('jobTitle').value.trim(),
        birthDate: document.getElementById('birthDate').value,
    };

    const errors = validateContact(payload);
    if (errors.length > 0) {
        showValidation(errors);
        return;
    }

    try {
        const isEdit = !!id;
        const url = isEdit ? `${API}/updateContact/${id}` : `${API}/addContact`;
        const method = isEdit ? 'PUT' : 'POST';

        const res = await fetch(url, {
            method,
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(payload),
        });

        if (!res.ok) {
            const err = await res.json();
            showValidation([err.detail || 'An error occurred.']);
            return;
        }

        contactModal.hide();
        currentPage = isEdit ? currentPage : 1;
        loadContacts();
    } catch {
        showValidation(['Network error. Please try again.']);
    }
}

// Delete Modal

function openDeleteModal(id, name) {
    pendingDeleteId = id;
    document.getElementById('deleteContactName').textContent = name;
    deleteModal.show();
}

async function confirmDelete() {
    if (!pendingDeleteId) return;

    try {
        const res = await fetch(`${API}/deleteContact/${pendingDeleteId}`, {method: 'DELETE'});
        if (!res.ok) throw new Error('Failed to delete contact.');

        deleteModal.hide();
        const totalPages = Math.ceil((totalCount - 1) / PAGE_SIZE);
        if (currentPage > totalPages && currentPage > 1) currentPage--;
        loadContacts();
    } catch (err) {
        alert(err.message);
    } finally {
        pendingDeleteId = null;
    }
}

// Validation

function validateContact(c) {
    const errors = [];

    if (!c.name) {
        errors.push('Name is required.');
    } else if (c.name.length > 200) {
        errors.push('Name cannot exceed 200 characters.');
    }

    if (!c.mobilePhone) {
        errors.push('Mobile phone is required.');
    } else if (c.mobilePhone.length > 20) {
        errors.push('Mobile phone cannot exceed 20 characters.');
    } else if (!PHONE_REGEX.test(c.mobilePhone)) {
        errors.push('Mobile phone must be a valid phone number.');
    }

    if (c.jobTitle && c.jobTitle.length > 100) {
        errors.push('Job title cannot exceed 100 characters.');
    }

    if (!c.birthDate) {
        errors.push('Birth date is required.');
    } else {
        const date = new Date(c.birthDate);
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        if (date >= today) errors.push('Birth date must be in the past.');
        if (date <= MIN_BIRTH_DATE) errors.push('Birth date is not realistic.');
    }

    return errors;
}

function showValidation(errors) {
    const el = document.getElementById('validationSummary');
    el.innerHTML = errors.map(e => `<div>${escapeHtml(e)}</div>`).join('');
    el.classList.add('visible');
}

function clearValidation() {
    const el = document.getElementById('validationSummary');
    el.innerHTML = '';
    el.classList.remove('visible');
}

// Helpers 

function escapeHtml(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

// Init

loadContacts();
