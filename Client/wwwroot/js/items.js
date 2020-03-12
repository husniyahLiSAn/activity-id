var table;
$(document).ready(function () {
    debugger;
    table = $("#tblItems").DataTable({
        serverSide: true,
        //"responsive": false,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        //"searching": false,
        "columnDefs": [{
            "targets": [1, 2, 3],
            "orderable": false
            //"searchable": false
        }],
        ajax: "/Items/PageData/",
        "columns": [
            { "data": "name" },
            { "data": "stock" },
            { "data": "price" },
            {
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetbyID(' + row.id + ')" ><i class="material-icons" title="Edit Item Name">&#xE254;</i></button>  <button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" title="Delete">&#xE872;</i></button>';
                }
            }
        ]
    });
});

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var addItem = new Object();
    addItem.Id = $('#Id').val();
    addItem.Name = $('#Name').val();
    addItem.Stock = $('#Stock').val();
    addItem.Price = $('#Price').val();

    $.ajax({
        url: "/Items/Insert",
        data: addItem,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            clearTextBox();
            table.ajax.reload();
            $('#myModal').modal('hide');
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Insert Successfully',
                showConfirmButton: false,
                timer: 1000
            });
        }
        else {
            Swal.fire('Error', 'Insert Failed', 'error');
            table.ajax.reload();
        }
    });
}

//Function for getting the Data Based upon Item ID
function GetbyID(ID) {
    $('#Name').css('border-color', 'lightgrey');
    $('#Stock').css('border-color', 'lightgrey');
    $('#Price').css('border-color', 'lightgrey');
    $('#Supplier').css('border-color', 'lightgrey');

    $.ajax({
        url: "/Items/GetbyID/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        //async: false,
        success: function (result) {
            $('#Id').val(result[0]['id']);
            $('#Name').val(result[0]['name']);
            $('#Stock').val(result[0]['stock']);
            $('#Price').val(result[0]['price']);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var upItem = new Object();
    upItem.Id = $('#Id').val();
    upItem.Name = $('#Name').val();
    upItem.Stock = $('#Stock').val();
    upItem.Price = $('#Price').val();

    $.ajax({
        url: "/Items/Update/",
        data: upItem,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            $('#myModal').modal('hide');
            clearTextBox();
            table.ajax.reload();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 1000
            });
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
            table.ajax.reload();
        }
    })
}

//function for deleting employee's record
function Delete(ID) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Items/Delete/",
                data: { id: ID },
                type: "POST"
            }).then((result) => {
                if (result.statusCode == 200) {
                    table.ajax.reload();
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully',
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
                else {
                    Swal.fire(
                        'Error!',
                        'Delete Failed.',
                        'error'
                    );
                    table.ajax.reload();
                }
            })
        }
    })
}

//Function for clearing the textboxes
function clearTextBox() {
    $('#Id').val("");
    $('#Name').val("");
    $('#Stock').val("");
    $('#Price').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#Stock').css('border-color', 'lightgrey');
    $('#Price').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        $('#Name').focus();
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }
    if ($('#Stock').val().trim() == "") {
        $('#Stock').css('border-color', 'Red');
        $('#Stock').focus();
        isValid = false;
    }
    else {
        $('#Stock').css('border-color', 'lightgrey');
    }
    if ($('#Price').val().trim() == "") {
        $('#Price').css('border-color', 'Red');
        $('#Price').focus();
        isValid = false;
    }
    else {
        $('#Price').css('border-color', 'lightgrey');
    }
    return isValid;
}