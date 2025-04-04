﻿$(document).ready(function () {

    connection.start().then(function () {

        connection.on("LockReceived", function (EditContactID) {
            var button = $("#" + EditContactID);
            if (button.length) {
                button.prop("disabled", true); 
            } else {
                console.warn("Button with ID " + EditContactID + " not found.");
            }
        });

        connection.on("UnlockReceived", function (Contact) {
            var button = $("#" + Contact);
            if (button.length) {
                button.prop("disabled", false);  // Enables the Edit button
            } else {
                console.warn("Button with ID " + Contact + " not found.");
            }
        });

    }).catch(function (err) {
        console.error("SignalR connection error: " + err.toString());
    });

    $("#contactForm").submit(function (event) {
        event.preventDefault();

        var contactId = $("#contactId").val();

        connection.invoke("UnlockRecord", contactId, userId)
            .catch(function (err) {
                console.error("SignalR UnlockRecord error: " + err.toString());
            });

        $.ajax({
            url: $("#contactForm").attr("action"),
            type: $("#contactForm").attr("method"),
            data: $(this).serialize(),
            success: function (response) {
                updateContactTable(response)

                $("#contactModal").modal("hide");

                $("#" + contactId).prop("disabled", false);
            },
            error: function () {

            }
        });
    });

    $("#cancelEditBtn").click(function () {
        var contactId = $("#contactId").val();

        connection.invoke("UnlockRecord", contactId, "@UserId")
            .catch(function (err) {
                console.error("err : " + err.toString());
            });

        $("#" + contactId).prop("disabled", false);

        $("#contactModal").modal("hide");
    });
});

function updateContactTable(contacts) {
    const tableBody = $('#contactTableBody');
    let html = '';
    console.log(contacts)
    contacts.forEach(contact => {
        html += `
                <tr data-id="${contact.id}">
                    <td>${escapeHtml(contact.name)}</td>
                    <td>${escapeHtml(contact.email)}</td>
                    <td>${escapeHtml(contact.countryCode)} ${escapeHtml(contact.phone)}</td>
                    <td>${escapeHtml(contact.notes)}</td>
                    <td>${escapeHtml(contact.address)}</td>
                    <td>${escapeHtml(contact.user.username)}</td>
                    <td>
                        <button id="${contact.id}" onclick="OpenEditContact(${contact.id})" class="btn btn-warning btn-sm edit-btn" data-id="${contact.id}">Edit</button>
                        <button data-bs-toggle="modal" data-bs-target="#deleteModal" onclick="SetContactID(${contact.id})" class="btn btn-danger delete-btn" data-id="${contact.id}">Delete</button>
                    </td>
                </tr>
            `;
    });

    tableBody.html(html);
}

function escapeHtml(str) {
    return str?.toString()
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;') || '';
}



var ContactID = "";
function SetContactID(id, userId) {
    ContactID = id;
}

// Delete Contact using AJAX
function DeleteContact() {
    $.ajax({
        url: `/Contact/DeleteContact?ContactID=${ContactID}`,
        type: 'DELETE',
        success: function (response) {
            console.log("DeleteContact")
            console.log(response)
            updateContactTable(response)
        },
        error: function () {
        }
    });
}

function OpenEditContact(id) {
    EditContactID = id;

    $.ajax({
        url: `/Contact/GetContact/${id}`,
        type: 'GET',
        success: function (contact) {
            $("#contactId").val(contact.id);
            $("#contactName").val(contact.name);
            $("#contactEmail").val(contact.email);
            $("#contactCountryCode").val(contact.countryCode);
            $("#contactPhone").val(contact.phone);
            $("#contactAddress").val(contact.address);
            $("#contactNotes").val(contact.notes);

            connection.invoke("LockRecord", id, contact.insertedUser).catch(function (err) {
                console.error("SignalR LockRecord error: " + err.toString());
            });

            $("#modalTitle").text("Edit Contact");
            $("#contactForm").attr("action", "/Contact/EditContact");
            $("#contactModal").modal("show");
        },
        error: function () {
        }
    });
}

function OpenAddContact() {
    $("#contactForm")[0].reset();
    $("#contactId").val("");
    $("#modalTitle").text("Add Contact");
    $("#contactForm").attr("action", "/Contact/AddContact");
    $("#contactModal").modal("show");
}


        