var table;
$(document).ready(function () {
    var Status = $('#selectActivityStatus').val();
    table = $("#tblLists").DataTable({
        serverSide: true,
        //"responsive": false,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        //"searching": false,
        "columnDefs": [{
            "targets": [0, 2, 3],
            "orderable": false
            //"searchable": false
        }],
        ajax: 
            //url: "/Home/ListAll/" + Status,
            "/Home/PageData/" + Status
            //type: "GET"
        ,
        "columns": [
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        return '<button type="button" class="btn btn-default" id="Update" onclick="return UpdateStatus(' + row.id + ')"><i class="fa fa-square-o" title="Checklist"></i></button>';
                    } else {
                        return '<button type="button" class="btn btn-default disabled" id="Uncheck"><i class="fa fa-check-square-o" title="Unchecklist"></i></button>';
                    }
                }
            },
            { "data": "name" },
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        return "Active";
                    } else {
                        return "Completed";
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        //return '<a href="" id="Edit" class="edit" onclick="return GetbyID(' + row.id + ')" ><i class="material-icons" title="Edit Activity Name">&#xE254;</i></a>  <a href="" id="Delete" class="delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" title="Delete">&#xE872;</i></a>';
                        return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetbyID(' + row.id + ')" ><i class="material-icons" title="Edit Activity Name">&#xE254;</i></button>  <button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" title="Delete">&#xE872;</i></button>';
                    } else {
                        return '<button type="button" class="btn btn-warning disabled" id="Edit" ><i class="material-icons" title="Edit Activity Name">&#xE254;</i></button>  <button type="button" class="btn btn-danger disabled" id="Delete"><i class="material-icons" title="Delete">&#xE872;</i></button>';
                    }
                }
            }
        ]
    });
});
$('#selectActivityStatus').change(function () {
    debugger;
    var sel = $('#selectActivityStatus').val();
    table.ajax.url('/Home/PageData/' + $('#selectActivityStatus').val()).load();
});

function GetbyID(id) {
    $('#Activity').css('border-color', 'lightgrey');
    //$('#Status').css('border-color', 'lightgrey');

    $.ajax({
        url: "/Home/GetbyID",
        data: { Id: id },
        type: "GET",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        success: function (result) {
            debugger;
            $('#Id').val(result[0]['id']);
            $('#Activity').val(result[0]['name']);
            //Status = result[0]['status'];
            //if (result[0]['status'] == false) {
            //    $("#Status option:contains(false)").attr('selected', 'selected');
            //}
            //else {
            //    $("#Status option:contains(true)").attr('selected', 'selected');
            //}

            $('#myModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Add() {
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        $('#Name').focus();
        return false;
    }
    var todolist = new Object();
    todolist.Name = $('#Name').val();

    $.ajax({
        url: "/Home/Insert/",
        data: todolist,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            $('#Name').val("");
            $('#Name').css('border-color', 'lightgrey');
            table.ajax.reload();
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Insert Successfully',
                showConfirmButton: false,
                timer: 1000
            });
            //window.location.href = "/Home/Index";
        }
        else {
            table.ajax.reload();
            Swal.fire('Error', 'Insert Failed', 'error');
            //window.location.href = "/Home/Index";
        }
    });
}

//Function for getting the Data Based upon Employee ID
function UpdateStatus(Id) {
    //var todolist = new Object();
    //todolist.Id = Id;
    $.ajax({
        url: "/Home/UpdateStatus/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            //loadDataAll();
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 1000
            });
            table.ajax.reload();
            //window.location.href = "/Home/Index";
        }
        else {
            table.ajax.reload();
            Swal.fire('Error!', 'Update Failed.', 'error');
            //window.location.href = "/Home/Index";
        }
    });
}

function UncheckStatus(Id) {
    //var todolist = new Object();
    //todolist.Id = Id;
    $.ajax({
        url: "/Home/UncheckStatus/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            ////loadDataAll();
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 1000
            });
            table.ajax.reload();
            //window.location.href = "/Home/Index";
        }
        else {
            table.ajax.reload();
            Swal.fire('Error!', 'Update Failed.', 'error');
            //window.location.href = "/Home/Index";
        }
    });
}

//function for updating todolist's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var todolist = new Object();
    todolist.Id = $('#Id').val();
    todolist.Name = $('#Activity').val();
    //todolist.Status = $('#Status').val();

    $.ajax({
        url: "/Home/Update/",
        data: todolist,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            //loadDataAll();
            $('#myModal').modal('hide');
            table.ajax.reload();
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 1000
            });
            //window.location.href = "/Home/Index";
        }
        else {
            table.ajax.reload();
            Swal.fire('Error!', 'Update Failed.', 'error');
            //window.location.href = "/Home/Index";
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
                url: "/Home/Delete/",
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
                    //window.location.href = "/Home/Index";
                }
                else {
                    table.ajax.reload();
                    Swal.fire(
                        'Error!',
                        'Delete Failed.',
                        'error'
                    );
                    //window.location.href = "/Home/Index";
                }
            })
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#Activity').val().trim() == "") {
        $('#Activity').css('border-color', 'Red');
        $('#Activity').focus();
        isValid = false;
    }
    else {
        $('#Activity').css('border-color', 'lightgrey');
    }
    //if ($('#Status').val().trim() == "") {
    //    $('#Status').css('border-color', 'Red');
    //    $('#Status').focus();
    //    isValid = false;
    //}
    //else {
    //    $('#Status').css('border-color', 'lightgrey');
    //}
    return isValid;
}