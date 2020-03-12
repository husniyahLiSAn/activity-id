var table;
$(document).ready(function () {
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
        ajax: "/Transactions/PageData/",
        "columns": [
            { "data": "itemName" },
            { "data": "quantity" },
            { "data": "total" },
            {
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetbyID(' + row.id + ')" ><i class="material-icons" title="Edit Item Name">&#xE254;</i></button>  <button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" title="Delete">&#xE872;</i></button>';
                }
            }
        ]
    });
});

function Pay() {
    var res = validatePayment();
    if (res == false) {
        return false;
    }
    var payItem = new Object();
    payItem.Id = $('#TransId').val();
    payItem.Total = $('#Total').val();
    payItem.Pay = $('#Pay').val();

    $.ajax({
        url: "/Transactions/Pay/" + payItem.Id,
        //data: {id: payment.Id},
        type: "POST"
    }).then((result) => {
        if (payItem.Pay > payItem.Total) {
            if (result.statusCode == 200) {
                clearTextBox();
                $('#payment').modal('hide');
                table.ajax.reload();
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Your Change: Rp. ' + (payItem.Pay - payItem.Total),
                    showConfirmButton: true,
                    timer: 1500
                });
            }
        }
        else {
            Swal.fire('Error', 'Payment Failed', 'error');
            table.ajax.reload();
        }
    });
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var addItem = new Object();
    addItem.Id = $('#Id').val();
    addItem.TransactionId = $('#IdTrans').val();
    addItem.Quantity = $('#Quantity').val();
    addItem.ItemId = $('#Item').val();

    $.ajax({
        url: "/Transactions/Insert",
        data: addItem,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            clearTextBox();
            $('#myModal').modal('hide');
            table.ajax.reload();
            Swal.fire({
                position: 'center',
                icon: 'success',
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

function GetPayment() {
    $('#Pay').css('border-color', 'lightgrey');
    $('#Total').css('border-color', 'lightgrey');

    $.ajax({
        url: "/Transactions/GetPay/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            clearTextBox();
            $('#TransId').val(result[0]['transactionId']);
            $('#Total').val(result[0]['total']);

            $('#payment').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//Function for getting the Data Based upon Item ID
function GetbyID(ID) {
    $('#Item').css('border-color', 'lightgrey');
    $('#Quantity').css('border-color', 'lightgrey');
    $('#IdTrans').css('border-color', 'lightgrey');

    $.ajax({
        url: "/Transactions/GetbyID/" + ID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        //async: false,
        success: function (result) {
            $('#Id').val(result[0]['id']);
            $('#IdTrans').val(result[0]['transactionId']);
            $('#Quantity').val(result[0]['quantity']);
            ItemId = result[0]['itemId'];
            $("#Item option:contains(" + result[0]['itemName'] + ")").attr('selected', 'selected');

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

var items = []
function LoadItem(element) {
    if (items.length == 0) {
        $.ajax({
            type: "GET",
            url: "/Items/List",
            success: function (data) {
                items = data;
                renderItem(element);
            }
        })
    }
    else {
        renderItem(element);
    }
}
function renderItem(element) {
    var $sup = $(element);
    $sup.empty();
    $sup.append($('<option/>').val('0').text('Select Item'));
    $.each(items, function (i, val) {
        $sup.append($('<option/>').val(val.id).text(val.name));
    })
}
LoadItem($('#Item'));

//function for updating employee's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var upItem = new Object();
    upItem.Id = $('#Id').val();
    upItem.TransactionId = $('#IdTrans').val();
    upItem.Quantity = $('#Quantity').val();
    upItem.ItemId = $('#Item').val();

    $.ajax({
        url: "/Transactions/Update/",
        data: upItem,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            $('#myModal').modal('hide');
            clearTextBox();
            table.ajax.reload();
            Swal.fire({
                position: 'center',
                icon: 'success',
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
                url: "/Transactions/Delete/",
                data: { id: ID },
                type: "POST"
            }).then((result) => {
                if (result.statusCode == 200) {
                    table.ajax.reload();
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
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
    $('#IdTrans').val("");
    $('#TransId').val("");
    $('#Quantity').val("");
    $('#Pay').val("");
    $('#Total').val("");
    $('#Item').val("Select Item");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Quantity').css('border-color', 'lightgrey');
    $('#Item').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#Quantity').val().trim() == "") {
        $('#Quantity').css('border-color', 'Red');
        $('#Quantity').focus();
        isValid = false;
    }
    else {
        $('#Quantity').css('border-color', 'lightgrey');
    }
    if ($('#Item').val().trim() == "") {
        $('#Item').css('border-color', 'Red');
        $('#Item').focus();
        isValid = false;
    }
    else {
        $('#Item').css('border-color', 'lightgrey');
    }
    return isValid;
}
function validatePayment() {
    var isValid = true;
    if ($('#Pay').val().trim() == "") {
        $('#Pay').css('border-color', 'Red');
        $('#Pay').focus();
        isValid = false;
    }
    else {
        $('#Pay').css('border-color', 'lightgrey');
    }
    return isValid;
}